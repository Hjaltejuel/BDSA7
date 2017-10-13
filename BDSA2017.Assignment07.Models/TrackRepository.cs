using BDSA2017.Assignment07.Entities;
using System.Threading.Tasks;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;

namespace BDSA2017.Assignment07.Models
{
    public class TrackRepository : ITrackRepository
    {
        private readonly ISlotCarContext _context;

        public TrackRepository(ISlotCarContext context)
        {
            _context = context;
        }

        public async Task<int> Create(TrackCreateDTO track)
        {
            var entity = new Track
            {
                Name = track.Name,
                LengthInMeters = track.LengthInMeters,
                MaxCars = track.MaxCars
            };

            _context.Tracks.Add(entity);

            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<TrackDTO> Find(int trackId)
        {
            var tracks = from t in _context.Tracks
                         where t.Id == trackId
                         select new TrackDTO
                         {
                             Id = t.Id,
                             Name = t.Name,
                             LengthInMeters = t.LengthInMeters,
                             BestLap = t.BestLapInTicks.HasValue
                                ? TimeSpan.FromTicks(t.BestLapInTicks.Value)
                                : default(TimeSpan?),
                             MaxCars = t.MaxCars,
                             NumberOfRaces = t.Races.Count()
                         };

            return await tracks.FirstOrDefaultAsync();
        }

        public IQueryable<TrackDTO> Read()
        {
            
           return from t in _context.Tracks
                  select new TrackDTO
                  {
                      Id = t.Id,
                      Name = t.Name,
                      LengthInMeters = t.LengthInMeters,
                      BestLap = t.BestLapInTicks.HasValue
                         ? TimeSpan.FromTicks(t.BestLapInTicks.Value)
                         : default(TimeSpan?),
                      MaxCars = t.MaxCars,
                      NumberOfRaces = t.Races.Count()
                  };


      
        }

        public async Task<bool> Update(TrackUpdateDTO track)
        {
            var entity = await _context.Tracks.FindAsync(track.Id);

            if (entity == null)
            {
                return false;
            }

            entity.Name = track.Name;
            entity.LengthInMeters = track.LengthInMeters;
            entity.BestLapInTicks = track.BestLap?.Ticks;
            entity.MaxCars = track.MaxCars;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(int trackId)
        {
            var track = await _context.Tracks.FindAsync(trackId);

            if (track == null)
            {
                return false;
            }

            _context.Tracks.Remove(track);

            await _context.SaveChangesAsync();

            return true;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _context.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~TrackRepository() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
