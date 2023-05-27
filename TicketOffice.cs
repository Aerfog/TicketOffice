using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketOffice
{
    internal class TicketOffice
    {
        public Dictionary<uint, Trip> Trips { get; }

        public TicketOffice()
        {
            Trips = new Dictionary<uint, Trip>();
        }

        public void FillDictionary()
        {
            string[] ticketBase = File.ReadAllLines(@"Config\TicketBase.txt", Encoding.UTF8);
            string[] tripBase = File.ReadAllLines(@"Config\TripBase.txt", Encoding.UTF8);
            foreach (var item in tripBase)
            {
                try
                {
                    var trip = CreateTrip(item);
                    Trips.Add(trip.RaceCode, trip);
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); continue; }
            }
            foreach (var item in ticketBase)
            {
                try
                {
                    var ticket = CreateTicket(item);
                    Trips[ticket.Trip.RaceCode].OccupiedPlaces.Add(ticket.SeatNumber, ticket);
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); continue; }
            }
        }

        private Ticket CreateTicket(string item)
        {
            var values = item.Split(';');
            var ticketValues = GetTicketValues(values);
            if (values[0].Equals("Звичайний"))
            {
                return new RegularTicket(ticketValues.Trip, ticketValues.SeatNumber,
                                         ticketValues.OwnerName);
            }
            else if (values[0].Equals("Просубсидований"))
            {

                return new SubsidizedTicket(ticketValues.Trip, ticketValues.SeatNumber,
                                            ticketValues.OwnerName);
            }
            throw new Exception("Такого типу квитка не існує!");
        }

        private (Trip Trip, uint SeatNumber, string OwnerName)
                 GetTicketValues(string[] values)
        {
            try
            {
                uint tripCode = Convert.ToUInt32(values[1]);
                uint SeatNumber = Convert.ToUInt32(values[2]);
                string OwnerName = values[3];
                if (Trips.ContainsKey(tripCode))
                {
                    return (Trips[tripCode], SeatNumber, OwnerName);
                }
                throw new Exception("Рейс з таким кодом не знайдений!");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private Trip CreateTrip(string data)
        {
            try
            {
                var parsedValues = ParseTripValues(data);
                return new Trip(parsedValues.RaceCode, parsedValues.DistanceInKilometers, parsedValues.NumberOfSeats,
                                parsedValues.DateOfDeparture, parsedValues.CityOfDeparture, parsedValues.CityOfArrival, parsedValues.StationNumber);
            }
            catch (Exception e) { throw new Exception(e.Message); }
        }

        private (uint RaceCode, DateTime DateOfDeparture, string CityOfDeparture,
                 string CityOfArrival, uint DistanceInKilometers, uint NumberOfSeats, uint StationNumber)
            ParseTripValues(string data)
        {
            try
            {
                string[] values = data.Split(';');
                uint RaceCode = Convert.ToUInt32(values[0]);
                uint DistanceInKilometers = Convert.ToUInt32(values[1]);
                uint NumberOfSeats = Convert.ToUInt32(values[2]);
                DateTime dateOfDeparture = Convert.ToDateTime(values[3]);
                string CityOfDeparture = values[4];
                string CityOfArrival = values[5];
                uint StationNumber = Convert.ToUInt32(values[6]);
                return (RaceCode, dateOfDeparture, CityOfDeparture, CityOfArrival, DistanceInKilometers, NumberOfSeats, StationNumber);
            }
            catch (Exception e) { throw new Exception(e.Message); }
        }

        public void UpdatePurchasedTicketFile()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in Trips.Values)
                foreach (var ticket in item.OccupiedPlaces.Values)
                    stringBuilder.Append(ticket.ToString());
            File.WriteAllText(@"Config\TicketBase.txt", stringBuilder.ToString());
        }

        public (Trip trip, uint SeatsNumber) Choose()
        {
            Console.WriteLine("Введіть номер рейсу та номер місця");
            try
            {
                var data = Console.ReadLine().Split(' ');
                Trip trip = Trips[Convert.ToUInt32(data[0])];
                uint SeatsNumber = Convert.ToUInt32(data[1]);
                return (trip, SeatsNumber);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public Ticket SellTicket(User user)
        {
            var ticketData = Choose();
            if (!ticketData.trip.OccupiedPlaces.ContainsKey(ticketData.SeatsNumber) &&
                 ticketData.SeatsNumber <= ticketData.trip.NumberOfSeats &&
                 ticketData.SeatsNumber > 0)
            {
                if (user.IsRecipientsOfSubsidies)
                {
                    var sub = new SubsidizedTicket(ticketData.trip, ticketData.SeatsNumber, user.Name);
                    Console.WriteLine("Внесіть готівку: " + sub.GetCost());
                    ticketData.trip.OccupiedPlaces.Add(ticketData.SeatsNumber, sub);
                    return sub;
                }
                else
                {
                    var reg = new RegularTicket(ticketData.trip, ticketData.SeatsNumber, user.Name);
                    Console.WriteLine("Внесіть готівку: " + reg.GetCost());
                    ticketData.trip.OccupiedPlaces.Add(ticketData.SeatsNumber, reg);
                    return reg;
                }
            }
            throw new Exception("Некоректний номер місця, або воно вже зайняте");
        }

        public bool TakeTicketBack(User user)
        {
            var ticketData = Choose();
            if (ticketData.trip.OccupiedPlaces.ContainsKey(ticketData.SeatsNumber) &&
                ticketData.SeatsNumber <= ticketData.trip.NumberOfSeats &&
                ticketData.SeatsNumber > 0)
            {
                if (user.Tickets.Contains(ticketData.trip.OccupiedPlaces[ticketData.SeatsNumber]))
                {
                    if ((ticketData.trip.DateOfDeparture - DateTime.Now).Days >= 1)
                    {
                        if (ticketData.trip.OccupiedPlaces[ticketData.SeatsNumber] is RegularTicket regular)
                        {
                            Console.WriteLine("Заберіть готівку: " + regular.GetCost());
                            user.Tickets.Remove(regular);
                        }
                        else if (ticketData.trip.OccupiedPlaces[ticketData.SeatsNumber] is SubsidizedTicket subsidized)
                        {
                            Console.WriteLine("Заберіть готівку: " + subsidized.GetCost());
                            user.Tickets.Remove(subsidized);
                        }
                        ticketData.trip.OccupiedPlaces.Remove(ticketData.SeatsNumber);
                        return true;
                    }
                    throw new Exception("Повернути квиток вже неможливо!");
                }
                throw new Exception("Такого купленого квитка не знайдено");
            }
            throw new Exception("Некоректний номер місця, або воно не зайняте");
        }

        public void GetTripInformation()
        {
            Console.WriteLine("|Номер рейсу|Відстань|Кількість місць|Дата та час відправлення|  Звідки  |   Куди   |Номер станції|");
            foreach (var item in Trips.Values)
                Console.WriteLine(item.GetStringForConsole());
        }

        internal void GetEmptySeatsInformation()
        {
            try
            {
                Console.WriteLine("Введіть номер рейсу");
                var item = Convert.ToUInt32(Console.ReadLine());
                if (!Trips.ContainsKey(item)) throw new Exception("Рейсу з таким номером не існує!");
                Console.WriteLine("|Номер рейсу|Відстань|Кількість місць|Дата та час відправлення|  Звідки  |   Куди   |Номер станції|");
                Console.WriteLine(Trips[item].GetStringForConsole());
                Trips[item].ShowFreeSeats();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
        internal void GetUserTicketsInformation(User user)
        {
            if (user.Tickets.Count == 0) { Console.WriteLine("Куплених квитків немає!"); return; }
            Console.WriteLine("|Номер рейсу| Дата та час відправлення |  Звідки  |   Куди   | Номер місця |              ПІБ Клієнта            | Вартість |");
            foreach (var item in user.Tickets)
                Console.WriteLine(item.GetStringForConsole());
        }
    }
}
