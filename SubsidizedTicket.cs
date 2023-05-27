using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketOffice
{
    internal class SubsidizedTicket : Ticket
    {
        private const double _discont = 0.25;
        public SubsidizedTicket(Trip Trip, uint SeatNumber, string OwnerName)
                                             : base(Trip, SeatNumber, OwnerName) { }

        public override double GetCost()
        {
            return base.GetCost() - (base.GetCost() * _discont);
        }

        public override string ToString()
        {
            return "Просубсидований;" + base.ToString();
        }

        public override string GetStringForConsole()
        {
            return base.GetStringForConsole() + string.Format(" {0:F2}$", GetCost());
        }

    }

}
