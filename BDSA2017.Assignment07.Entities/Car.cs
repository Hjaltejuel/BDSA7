using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BDSA2017.Assignment07.Entities
{
    public class Car
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Driver { get; set; }

        public ICollection<CarInRace> CarsInRace { get; set; }
    }
}