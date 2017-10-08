using System;
using System.Linq;
using System.Threading.Tasks;

namespace BDSA2017.Assignment07.Models
{
    public interface ITrackRepository : IDisposable
    {
        Task<int> Create(TrackCreateDTO track);

        Task<TrackDTO> Find(int trackId);

        IQueryable<TrackDTO> Read();

        Task<bool> Update(TrackUpdateDTO track);

        Task<bool> Delete(int trackId);
    }
}
