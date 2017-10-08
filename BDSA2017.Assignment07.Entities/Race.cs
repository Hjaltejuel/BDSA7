using System;
using System.Collections.Generic;

namespace BDSA2017.Assignment07.Entities
{
    public class Race
    {
        public int Id { get; set; }
        public int TrackId { get; set; }
        public Track Track { get; set; }
        public int NumberOfLaps { get; set; }
        public DateTime? PlannedStart { get; set; }
        public DateTime? ActualStart { get; set; }
        public DateTime? PlannedEnd { get; set; }
        public DateTime? ActualEnd { get; set; }
        public ICollection<CarInRace> CarsInRace { get; set; }
    }
}