using System;
using System.ComponentModel.DataAnnotations;

namespace BDSA2017.Assignment07.Models
{
    public class TrackUpdateDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public double LengthInMeters { get; set; }

        public TimeSpan? BestLap { get; set; }

        public int MaxCars { get; set; }
    }
}
