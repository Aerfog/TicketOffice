namespace TicketOffice
{
    internal class User
    {
        public bool IsRecipientsOfSubsidies { get; }
        public string Name { get; }
        public List<Ticket> Tickets { get; }

        public User(bool IsRecipientsOfSubsidies, string Name)
        {
            this.IsRecipientsOfSubsidies = IsRecipientsOfSubsidies;
            this.Name = Name;
            Tickets = new List<Ticket>();
        }

        public bool BuyTicket(TicketOffice ticketOffice)
        {
            try
            {
                Tickets.Add(ticketOffice.SellTicket(this));
                return true;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); return false; }
        }

        public bool ReturnTicket(TicketOffice ticketOffice)
        {
            try
            {
                if (ticketOffice.TakeTicketBack(this)) return true;
                return false;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); return false; }
        }

        public void TakeRequest(uint commandCode, TicketOffice ticketOffice)
        {
            switch (commandCode)
            {
                case 1:
                    ticketOffice.GetTripInformation();
                    break;
                case 2:
                    ticketOffice.GetEmptySeatsInformation();
                    break;
                case 3:
                    ticketOffice.GetUserTicketsInformation(this);
                    break;
                default: 
                    Console.WriteLine( "Такої команди не існує!");
                    return;
            }
        }
    }
}
