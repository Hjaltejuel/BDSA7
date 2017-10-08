using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BDSA2017.Assignment07.Entities
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<SlotCarContext>
    {
        public SlotCarContext CreateDbContext(string[] args)
        {
            var connectionString = @"Server=(localdb)\mssqllocaldb;Database=SlotCarTournament;Trusted_Connection=True;MultipleActiveResultSets=true";

            var builder = new DbContextOptionsBuilder<SlotCarContext>();
            builder.UseSqlServer(connectionString);

            return new SlotCarContext(builder.Options);
        }
    }
}
