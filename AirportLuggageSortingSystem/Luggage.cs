using System;

namespace AirportLuggageSortingSystem
{
    /// <summary>
    /// class Luggage
    /// its purpose is to make an object of a Luggage
    /// </summary>
    public class Luggage
    {
        public string LuggageNumber { get; set; }
        public DateTime DateIn { get; set; }
        public DateTime ? DateOut { get; set; }

       
    }
}