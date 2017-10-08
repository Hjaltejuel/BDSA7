using BDSA2017.Assignment07.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BDSA2017.Assignment07.Models.Tests
{
    public class TrackRepositoryTests
    {
        [Fact]
        public async Task Create_given_track_adds_it()
        {
            var entity = default(Track);
            var context = new Mock<ISlotCarContext>();
            context.Setup(c => c.Tracks.Add(It.IsAny<Track>())).Callback<Track>(t => entity = t);

            using (var repository = new TrackRepository(context.Object))
            {
                var track = new TrackCreateDTO
                {
                    Name = "name",
                    LengthInMeters = 12.5,
                    MaxCars = 4
                };
                await repository.Create(track);
            }

            Assert.Equal("name", entity.Name);
            Assert.Equal(12.5, entity.LengthInMeters);
            Assert.Equal(4, entity.MaxCars);
        }

        [Fact]
        public async Task Create_given_track_calls_SaveChangesAsync()
        {
            var context = new Mock<ISlotCarContext>();
            context.Setup(c => c.Tracks.Add(It.IsAny<Track>()));

            using (var repository = new TrackRepository(context.Object))
            {
                var track = new TrackCreateDTO { Name = "name" };

                await repository.Create(track);
            }

            context.Verify(c => c.SaveChangesAsync(default(CancellationToken)));
        }

        [Fact]
        public async Task Create_given_track_returns_new_Id()
        {
            var entity = default(Track);

            var context = new Mock<ISlotCarContext>();
            context.Setup(c => c.Tracks.Add(It.IsAny<Track>()))
                .Callback<Track>(t => entity = t);
            context.Setup(c => c.SaveChangesAsync(default(CancellationToken)))
                .Returns(Task.FromResult(0))
                .Callback(() => entity.Id = 42);

            using (var repository = new TrackRepository(context.Object))
            {
                var track = new TrackCreateDTO { Name = "name" };

                var id = await repository.Create(track);

                Assert.Equal(42, id);
            }
        }

        [Fact]
        public async Task Find_given_non_existing_id_returns_null()
        {
            using (var connection = new SqliteConnection("DataSource=:memory:"))
            {
                connection.Open();

                var builder = new DbContextOptionsBuilder<SlotCarContext>()
                                  .UseSqlite(connection);

                var context = new SlotCarContext(builder.Options);
                await context.Database.EnsureCreatedAsync();

                using (var repository = new TrackRepository(context))
                {
                    var track = await repository.Find(42);

                    Assert.Null(track);
                }
            }
        }

        [Fact]
        public async Task Find_given_existing_id_returns_mapped_TrackDTO()
        {
            using (var connection = new SqliteConnection("DataSource=:memory:"))
            {
                connection.Open();

                var builder = new DbContextOptionsBuilder<SlotCarContext>()
                                  .UseSqlite(connection);

                var context = new SlotCarContext(builder.Options);
                await context.Database.EnsureCreatedAsync();

                var entity = new Track
                {
                    Name = "name",
                    LengthInMeters = 12.5,
                    BestLapInTicks = TimeSpan.FromSeconds(6.3).Ticks,
                    MaxCars = 4,
                    Races = new[] { new Race { NumberOfLaps = 12 }, new Race { NumberOfLaps = 24 } }
                };

                context.Tracks.Add(entity);
                await context.SaveChangesAsync();
                var id = entity.Id;

                using (var repository = new TrackRepository(context))
                {
                    var track = await repository.Find(id);

                    Assert.Equal("name", track.Name);
                    Assert.Equal(12.5, track.LengthInMeters);
                    Assert.Equal(TimeSpan.FromSeconds(6.3), track.BestLap);
                    Assert.Equal(4, track.MaxCars);
                    Assert.Equal(2, track.NumberOfRaces);
                }
            }
        }

        [Fact]
        public async Task Read_returns_mapped_TrackDTO()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var builder = new DbContextOptionsBuilder<SlotCarContext>()
                              .UseSqlite(connection);

            var context = new SlotCarContext(builder.Options);
            context.Database.EnsureCreated();

            var entity = new Track
            {
                Name = "name",
                LengthInMeters = 12.5,
                BestLapInTicks = TimeSpan.FromSeconds(6.3).Ticks,
                MaxCars = 4,
                Races = new[] { new Race { NumberOfLaps = 12 }, new Race { NumberOfLaps = 24 } }
            };

            context.Tracks.Add(entity);
            await context.SaveChangesAsync();
            var id = entity.Id;

            using (var repository = new TrackRepository(context))
            {
                var tracks = await repository.Read().ToListAsync();
                var track = tracks.First();

                Assert.Equal(id, track.Id);
                Assert.Equal("name", track.Name);
                Assert.Equal(12.5, track.LengthInMeters);
                Assert.Equal(TimeSpan.FromSeconds(6.3), track.BestLap);
                Assert.Equal(4, track.MaxCars);
                Assert.Equal(2, track.NumberOfRaces);
            }
        }

        [Fact]
        public async Task Update_given_existing_track_returns_true()
        {
            var context = new Mock<ISlotCarContext>();
            var entity = new Track { Id = 42 };
            context.Setup(c => c.Tracks.FindAsync(42)).ReturnsAsync(entity);

            using (var repository = new TrackRepository(context.Object))
            {
                var track = new TrackUpdateDTO { Id = 42 };

                var success = await repository.Update(track);

                Assert.True(success);
            }
        }

        [Fact]
        public async Task Update_given_non_existing_track_returns_false()
        {
            var context = new Mock<ISlotCarContext>();
            context.Setup(c => c.Tracks.FindAsync(42)).ReturnsAsync(default(Track));

            using (var repository = new TrackRepository(context.Object))
            {
                var track = new TrackUpdateDTO { Id = 42 };

                var success = await repository.Update(track);

                Assert.False(success);
            }
        }

        [Fact]
        public async Task Update_given_existing_track_Updates_properties()
        {
            var context = new Mock<ISlotCarContext>();
            var entity = new Track { Id = 42 };
            context.Setup(c => c.Tracks.FindAsync(42)).ReturnsAsync(entity);

            using (var repository = new TrackRepository(context.Object))
            {
                var track = new TrackUpdateDTO
                {
                    Id = 42,
                    Name = "name",
                    LengthInMeters = 12.5,
                    BestLap = TimeSpan.FromSeconds(6.3),
                    MaxCars = 4
                };

                await repository.Update(track);
            }

            Assert.Equal("name", entity.Name);
            Assert.Equal(12.5, entity.LengthInMeters);
            Assert.Equal(TimeSpan.FromSeconds(6.3).Ticks, entity.BestLapInTicks);
            Assert.Equal(4, entity.MaxCars);
        }

        [Fact]
        public async Task Update_given_existing_track_calls_SaveChangesAsync()
        {
            var context = new Mock<ISlotCarContext>();
            var entity = new Track { Id = 42 };
            context.Setup(c => c.Tracks.FindAsync(42)).ReturnsAsync(entity);

            using (var repository = new TrackRepository(context.Object))
            {
                var track = new TrackUpdateDTO { Id = 42 };

                await repository.Update(track);
            }

            context.Verify(c => c.SaveChangesAsync(default(CancellationToken)));
        }

        [Fact]
        public async Task Update_given_non_existing_track_does_not_call_SaveChangesAsync()
        {
            var context = new Mock<ISlotCarContext>();
            context.Setup(c => c.Tracks.FindAsync(42)).ReturnsAsync(default(Track));

            using (var repository = new TrackRepository(context.Object))
            {
                var track = new TrackUpdateDTO { Id = 42 };

                await repository.Update(track);
            }

            context.Verify(c => c.SaveChangesAsync(default(CancellationToken)), Times.Never);
        }

        [Fact]
        public async Task Delete_given_existing_trackId_removes_it()
        {
            var context = new Mock<ISlotCarContext>();
            var track = new Track { Id = 42 };
            context.Setup(c => c.Tracks.FindAsync(42)).ReturnsAsync(track);

            using (var repository = new TrackRepository(context.Object))
            {
                await repository.Delete(42);
            }

            context.Verify(c => c.Tracks.Remove(track));
        }

        [Fact]
        public async Task Delete_given_existing_trackId_calls_SaveChangesAsync()
        {
            var context = new Mock<ISlotCarContext>();
            var track = new Track { Id = 42 };
            context.Setup(c => c.Tracks.FindAsync(42)).ReturnsAsync(track);

            using (var repository = new TrackRepository(context.Object))
            {
                await repository.Delete(42);
            }

            context.Verify(c => c.SaveChangesAsync(default(CancellationToken)));
        }

        [Fact]
        public async Task Delete_given_existing_trackId_returns_true()
        {
            var context = new Mock<ISlotCarContext>();
            var track = new Track { Id = 42 };
            context.Setup(c => c.Tracks.FindAsync(42)).ReturnsAsync(track);

            using (var repository = new TrackRepository(context.Object))
            {
                var success = await repository.Delete(42);

                Assert.True(success);
            }
        }

        [Fact]
        public async Task Delete_given_non_existing_trackId_does_not_call_SaveChangesAsync()
        {
            var context = new Mock<ISlotCarContext>();
            context.Setup(c => c.Tracks.FindAsync(42)).ReturnsAsync(default(Track));

            using (var repository = new TrackRepository(context.Object))
            {
                await repository.Delete(42);
            }

            context.Verify(c => c.SaveChangesAsync(default(CancellationToken)), Times.Never);
        }

        [Fact]
        public async Task Delete_given_non_existing_trackId_does_not_remove_it()
        {
            var context = new Mock<ISlotCarContext>();
            context.Setup(c => c.Tracks.FindAsync(42)).ReturnsAsync(default(Track));

            using (var repository = new TrackRepository(context.Object))
            {
                await repository.Delete(42);
            }

            context.Verify(c => c.Tracks.Remove(It.IsAny<Track>()), Times.Never);
        }

        [Fact]
        public async Task Delete_given_non_existing_trackId_returns_false()
        {
            var context = new Mock<ISlotCarContext>();
            context.Setup(c => c.Tracks.FindAsync(42)).ReturnsAsync(default(Track));

            using (var repository = new TrackRepository(context.Object))
            {
                var success = await repository.Delete(42);

                Assert.False(success);
            }
        }

        [Fact]
        public void Dispose_disposes_context()
        {
            var context = new Mock<ISlotCarContext>();

            using (var repository = new TrackRepository(context.Object))
            {
            }

            context.Verify(c => c.Dispose());
        }
    }
}
