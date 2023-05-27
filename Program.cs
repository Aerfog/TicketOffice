using System.Text;

namespace TicketOffice
{
    internal class Program
    {
        const string commands = 
            @"Оберіть операцію:
1.Отримати інформацію
2.Придбати квиток
3.Повернути квиток
4.Вихід";
        const string requestCommands =
            @"Отримати інформацію про:
1.Наявні рейси
2.Вільні місця у обраному рейсі
3.Мої квитки ";

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;
            if (DateTime.Now.Hour >= 8 && DateTime.Now.Hour <= 20)
            {
                TicketOffice app = new TicketOffice();
            app.FillDictionary();
            var user = CreateUser();
            if (user != null)
            {
                FillUserTicketList(user, app);
                Work(app, user);
            }
            }
            else
            {
                Console.WriteLine("Каса зачинена! Графік роботи: 08:00 - 20:00");
            }
        }
        public static User CreateUser()
        {
            try
            {
                Console.WriteLine("Введіть дані:");
                Console.WriteLine("Введіть ПІБ:");
                var name = Console.ReadLine();
                Console.WriteLine("Чи маєте ви пільги? Так/Ні");
                switch (Console.ReadLine())
                {
                    case "Ні":
                        return new User(false, name);
                    case "Так":
                        return new User(true, name);
                    default:
                        throw new Exception("Такої відповіді не існує!");
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); return null; }
        }

        private static void FillUserTicketList(User user, TicketOffice ticketOffice)
        {
            foreach (var trip in ticketOffice.Trips.Values)
                foreach (var ticket in trip.OccupiedPlaces.Values)
                    if (ticket.OwnerName.Equals(user.Name))
                        user.Tickets.Add(ticket);
        }

        private static void Work(TicketOffice app, User user)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine(commands);
                    uint commandNumber;
                    Console.Write("Номер операції: ");
                    commandNumber = Convert.ToUInt32(Console.ReadLine());
                    if (!SelectOperation(commandNumber, app, user)) break;
                    Console.WriteLine("Натисніть будь-яку клавішу для продовження роботи");
                    Console.ReadKey();
                    Console.Clear();
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); continue; }
            }
        }

        private static bool SelectOperation(uint commandNumber, TicketOffice app, User user)
        {
            switch (commandNumber)
            {
                case 1:
                    try
                    {
                        Console.WriteLine("Яка інформація вам потрібна?");
                        Console.WriteLine(requestCommands);
                        user.TakeRequest(Convert.ToUInt32(Console.ReadLine()), app);
                    } catch (Exception ex) { Console.WriteLine(ex.Message); }
                    break;
                case 2:
                    if (user.BuyTicket(app)) app.UpdatePurchasedTicketFile();
                    break;
                case 3:
                    if (user.ReturnTicket(app)) app.UpdatePurchasedTicketFile();
                    break;
                case 4:
                    return false;
                default:
                    Console.WriteLine("Такої операції не існує!");
                    break;
            }
            return true;
        }

    }
}