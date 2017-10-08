using System.ComponentModel.DataAnnotations;

namespace BDSA2017.Assignment07.Models
{
    public class TrackCreateDTO
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public double LengthInMeters { get; set; }

        public int MaxCars { get; set; }
    }
}
