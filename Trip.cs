using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketOffice
{
    internal class Trip
    {
        private const double _pricePerKilometer = 0.45;
        public uint RaceCode { get; }
        public DateTime DateOfDeparture { get; }
        public string CityOfDeparture { get; }
        public string CityOfArrival { get; }
        private uint DistanceInKilometers { get; }
        public uint NumberOfSeats { get; }
        public Dictionary<uint, Ticket> OccupiedPlaces { get; }
        public uint StationNumber { get; }
        public Trip(uint RaceCode, uint DistanceInKilometers, uint NumberOfSeats, DateTime DateOfDeparture, string CityOfDeparture, string CityOfArrival, uint stationNumber)
        {
            this.RaceCode = RaceCode;
            this.DistanceInKilometers = DistanceInKilometers;
            this.NumberOfSeats = NumberOfSeats;
            this.DateOfDeparture = DateOfDeparture;
            this.CityOfDeparture = CityOfDeparture;
            this.CityOfArrival = CityOfArrival;
            OccupiedPlaces = new Dictionary<uint, Ticket>();
            StationNumber = stationNumber;
        }



        public double GetCost()
        {
            return _pricePerKilometer * DistanceInKilometers;
        }

        public override string ToString()
        {
            return string.Format("{0};{1};{2};{3};{4};{5};{6}\n", RaceCode, DistanceInKilometers, NumberOfSeats, DateOfDeparture, CityOfDeparture, CityOfArrival, StationNumber);
        }

        public string GetStringForConsole()
        {
            return string.Format("{0,-12} {1,-8} {2,-15} {3,-24:dd-MM-yy HH-mm} {4,-10} {5,-10} {6,-3}", RaceCode, DistanceInKilometers, NumberOfSeats, DateOfDeparture, CityOfDeparture, CityOfArrival, StationNumber);
        }

        internal void ShowFreeSeats()
        {
            int j = 0;
            for (uint i = 1; i <= NumberOfSeats; i++)
            {
                if (!OccupiedPlaces.ContainsKey(i)) 
                {
                    j++;
                    Console.Write("{0,-3}",i);
                    if (j % 5 == 0)
                    {
                        Console.WriteLine();
                    } 
                }
            }
            Console.WriteLine();
        }
    }
}
