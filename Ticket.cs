using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace TicketOffice
{
    internal abstract class Ticket
    {
        public Trip Trip { get; }
        public uint SeatNumber { get; }
        public string OwnerName { get; }

        public Ticket(Trip Trip, uint SeatNumber, string OwnerName)
        {
            this.Trip = Trip;
            this.SeatNumber = SeatNumber;
            this.OwnerName = OwnerName;
        }

        public virtual double GetCost()
        {
            return Trip.GetCost();
        }

        public override string ToString()
        {
            return string.Format("{0};{1};{2};\n", Trip.RaceCode, SeatNumber,OwnerName);
        }

        public virtual string GetStringForConsole()
        {
            return string.Format("{0,-13} {1,-25} {2,-10} {3,-10} {4,-14} {5,-37}", Trip.RaceCode, Trip.DateOfDeparture, Trip.CityOfDeparture, Trip.CityOfArrival,  SeatNumber, OwnerName);
        }
    }
}
