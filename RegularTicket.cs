using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketOffice
{
    internal class RegularTicket : Ticket
    {
        public RegularTicket(Trip Trip, uint SeatNumber, string OwnerName) 
                                : base(Trip, SeatNumber, OwnerName){}

        public override double GetCost()
        {
            return base.GetCost();
        }

        public override string ToString()
        {
            return "Звичайний;"+ base.ToString();
        }
        public override string GetStringForConsole()
        {
            return base.GetStringForConsole() + string.Format(" {0:F2}$",GetCost());
        }
    }
}
