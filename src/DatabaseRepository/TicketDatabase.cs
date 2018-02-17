using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using TicketSystemEngine;
using TicketSystem.DatabaseRepository;
using TicketSystem.PaymentProvider;

namespace TicketSystem.DatabaseRepository
{
    public class TicketDatabase : ITicketDatabase
    {
        private readonly DateTime SQL_MIN_DATETIME = DateTime.Parse("1753-01-01 12:00:00");
        private readonly DateTime SQL_MAX_DATETIME = DateTime.Parse("9999-12-31 11:59:59");
        private const string CONNECTION_STRING = "Server=(local)\\SqlExpress; Database=TicketSystem; Trusted_connection=true";

        /// <summary>
        /// Method used to add a new venue to the database table Venues.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="address"></param>
        /// <param name="city"></param>
        /// <param name="country"></param>
        /// <returns>A Venue object.</returns>
        public Venue VenueAdd(string name, string address, string city, string country)
        {
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                var result = connection.ExecuteScalar<int>("insert into Venues([VenueName],[Address],[City],[Country]) values(@Name,@Address, @City, @Country); SELECT SCOPE_IDENTITY();", new { Name = name, Address = address, City = city, Country = country });
                return connection.Query<Venue>("SELECT * FROM Venues WHERE VenueID=@Id", new { Id = result }).First();
            }
        }

        /// <summary>
        /// Method that fetches a list of venues from the database table Venues, matching a query
        /// based on either VenueID, VenueName, Address, City or Country.
        /// </summary>
        /// <param name="query"></param>
        /// <returns>A list of Venue objects.</returns>
        public IEnumerable<Venue> VenuesFind(string query)
        {
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                int id = -1;
                int.TryParse(query, out id);
                return connection.Query<Venue>("SELECT * FROM [Venues] WHERE VenueID LIKE @ID OR VenueName LIKE @Query OR Address LIKE @Query OR City LIKE @Query OR Country LIKE @Query", new { ID = id, Query = $"%{query}%" });
                //return connection.Query<Venue>("SELECT * FROM Venues WHERE VenueName like '%" + query + "%' OR Address like '%" + query + "%' OR City like '%" + query + "%' OR Country like '%" + query + "%'").ToList();
            }
        }

        /// <summary>
        /// Method that fetches all existing venues from the database table Venues.
        /// </summary>
        /// <returns>A list of Venues.</returns>
        public List<Venue> AllVenues()
        {
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                return connection.Query<Venue>("SELECT * FROM Venues").ToList();
            }
        }

        /// <summary>
        /// Method that updates a venue in the database table Venues, depending on
        /// which parameters are sent in when calling the method, all or some of
        /// the attributes for a venue will be changed. ID can't be changed since
        /// it is primary key.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="address"></param>
        /// <param name="city"></param>
        /// <param name="country"></param>
        public void UpdateVenue(int id, string name, string address, string city, string country)
        {
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                if (name != null)
                {
                    connection.Query("UPDATE Venues SET [VenueName] = @VenueName WHERE [VenueID] = @VenueID; ", new { VenueName = name, VenueID = id });
                }
                if (address != null)
                {
                    connection.Query("UPDATE Venues SET [Address] = @Address WHERE [VenueID] = @VenueID; ", new { Address = address, VenueID = id });
                }
                if (city != null)
                {
                    connection.Query("UPDATE Venues SET [City] = @City WHERE [VenueID] = @VenueID; ", new { City = city, VenueID = id });
                }
                if (country != null)
                {
                    connection.Query("UPDATE Venues SET [Country] = @Country WHERE [VenueID] = @VenueID; ", new { Country = country, VenueID = id });
                }
            }
        }

        /// <summary>
        /// Method that deletes a specific Venue from the Venues database table.
        /// </summary>
        /// <param name="id"></param>
        public void DeleteVenue(int id)
        {
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                connection.Query<Venue>("DELETE FROM [Venues] WHERE VenueID = @ID", new { ID = id }).FirstOrDefault();
            }
        }

        /// <summary>
        /// Method used to get a venue from the database based on a specific ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A specific venue.</returns>
        public Venue FindVenueByID(int id)
        {
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                return connection.Query<Venue>("SELECT * FROM [Venues] WHERE VenueID = @ID", new { ID = id }).FirstOrDefault();
            }
        }

        /// <summary>
        /// Method that creates a new TicketEvent in the database table TicketEvents.
        /// ID is automatically generated, thus not given as parameter in method.
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="htmlDescription"></param>
        /// <returns>A TicketEvent object.</returns>
        public TicketEvent EventAdd(string eventName, string htmlDescription, int ticketEventprice)
        {
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                var result = connection.Query<int>("insert into TicketEvents([EventName],[EventHtmlDescription],[TicketEventPrice]) values(@EventName, @EventHtmlDescription, @TicketEventPrice); SELECT SCOPE_IDENTITY();", new { EventName = eventName, EventHtmlDescription = htmlDescription, TicketEventPrice = ticketEventprice });
                var createdEventQuery = result.First();
                //var createdEventQuery = connection.Query<int>("SELECT IDENT_CURRENT ('TicketEvents') AS Current_Identity").First();
                return connection.Query<TicketEvent>("SELECT * FROM TicketEvents WHERE TicketEventID=@Id", new { Id = createdEventQuery }).First();
            }
        }

        /// <summary>
        /// Method that updates a TicketEvent in the TicketEvents database table, 
        /// the content of the update depends on which parameters that are given 
        /// new values when the method is called. ID can't be changed, just used
        /// to look up the event in the database that we want to update.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="eventName"></param>
        /// <param name="htmlDescription"></param>
        public void UpdateEvent(int id, string eventName, string htmlDescription, int ticketeventPrice)
        {
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                if (eventName != null)
                {
                    //connection.Query("insert into Venues([VenueName],[Address],[City],[Country]) values(@Name,@Address, @City, @Country)", new { Name = name, Address = address, City = city, Country = country })
                    connection.Query("UPDATE TicketEvents SET [EventName] = @EventName WHERE [TicketEventID] = @TicketEventID; ", new { EventName = eventName, TicketEventID = id });
                }
                if (htmlDescription != null)
                {
                    connection.Query("UPDATE TicketEvents SET [EventHtmlDescription] = @EventHtmlDescription WHERE [TicketEventID] = @TicketEventID; ", new { EventHtmlDescription = htmlDescription, TicketEventID = id });
                }
                if (ticketeventPrice != 0 && ticketeventPrice >= 150)
                {
                    connection.Query("UPDATE TicketEvents SET [TicketEventPrice] = @TicketEventPrice WHERE [TicketEventID] = @TicketEventID; ", new { TicketEventPrice = ticketeventPrice, TicketEventID = id });
                }
            }
        }

        /// <summary>
        /// Method that deletes a TicketEvent from the TicketEvents database table
        /// based on the id provided as parameter when calling method.
        /// </summary>
        /// <param name="id"></param>
        public void DeleteEvent(int id)
        {
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                connection.Query<TicketEvent>("DELETE FROM [TicketEvents] WHERE TicketEventID = @ID", new { ID = id }).FirstOrDefault();
            }
        }

        /// <summary>
        /// Method that is used to get all existing events from the database,
        /// represented as a list of TicketEvent objects.
        /// </summary>
        /// <param name="query"></param>
        /// <returns>A list consisting of TicketEvent objects.</returns>
        public List<TicketEvent> FindAllEvents()
        {
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                return connection.Query<TicketEvent>("SELECT * FROM TicketEvents").ToList();
            }
        }

        /// <summary>
        /// Method that fetches a list of events matching a query from the database table TicketEvent,
        /// based on either EventID or EventName.
        /// </summary>
        /// <param name="query"></param>
        /// <returns>A list of TicketEvent objects.</returns>
        public IEnumerable<TicketEvent> FindEvents(string query)
        {
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                int id = -1;
                int.TryParse(query, out id);
                return connection.Query<TicketEvent>("SELECT * FROM [TicketEvents] WHERE TicketEventID = @ID OR EventName like @Query", new { ID = id, Query = $"%{query}%" });
            }
        }

        public int GetTicketEventID(string query)
        {
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                return connection.Execute("SELECT TicketEvents.TicketEventID From TicketEvents WHERE EventName like @Query ", new { Query = $"%{query}" });
            }
        }

        /// <summary>
        /// Separate method used to get an event from the database based on a specific ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The event that has the specified ID</returns>
        public TicketEvent FindEventByID(int id)
        {
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                return connection.Query<TicketEvent>("SELECT * FROM [TicketEvents] WHERE TicketEventID = @ID", new { ID = id }).FirstOrDefault();
            }
        }

        /// <summary>
        /// Method that fetches a list of Orders belonging to a specific customer, matches the query against table
        /// TicketTransactions, orders can be found either through buyer name or buyer email address.
        /// </summary>
        /// <param name="query"></param>
        /// <returns>A list of Order objects.</returns>
        public IEnumerable<Order> FindCustomerOrders(string query)
        {
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                int id = -1;
                int.TryParse(query, out id);
                return FindOrdersSuchThat("TicketTransactions.BuyerFirstName LIKE @Query OR TicketTransactions.BuyerLastName Like @Query OR TicketTransactions.BuyerEmailAddress LIKE @Query", new { ID = id, Query = $"%{query}%" });
            }
        }

        /// <summary>
        /// Method that fetches all customer orders from the database table TicketTransactions.
        /// </summary>
        /// <returns>A list of all customer orders.</returns>     

        public List<Order> FindAllCustomerOrder()
        {            
            return FindOrdersSuchThat("1 = 1", new { }).ToList();          
        }


        /// <summary>
        /// Metod that is used to return Orders (tickettransactions) from the
        /// database based on the sent in parameters.
        /// </summary>
        /// <param name="wherePart"></param>
        /// <param name="parameters"></param>
        /// <returns>A list of Orders.</returns>
        private IEnumerable<Order> FindOrdersSuchThat(string wherePart, object parameters)
        {
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                var temp = connection.Query<Order>("SELECT *  FROM TicketTransactions WHERE " + wherePart, parameters);
                foreach (Order order in temp)
                {
                    bool first = true;
                    string ids = "";
                    foreach(int id in FindTicketsByTransactionID(order.TransactionID))
                    {
                        if (!first)
                        {
                            ids += ",";
                        }
                        ids += id.ToString();
                        first = false;
                    }
                    order.TicketIDs = ids;
                }
                return temp;
            }
        }

        /// <summary>
        /// Method used to find an order from the database based on transactionID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A specific customer order.</returns>
        public Order FindCustomerOrderByID(int id)
        {
            return FindOrdersSuchThat("TicketTransactions.TransactionID = @ID", new { ID = id }).FirstOrDefault();
        }

        /// <summary>
        /// Method that updates a customerorder depending on the parameters sent in when calling the method.
        /// Only fields allowed to edit are able to update through the method.
        /// </summary>
        /// <param name="transactionID"></param>
        /// <param name="paymentStatus"></param>
        /// <param name="buyerLastName"></param>
        /// <param name="buyerFirstName"></param>
        /// <param name="buyerAddress"></param>
        /// <param name="buyerCity"></param>
        public void UpdateCustomerOrder(int transactionID, string buyerLastName,
            string buyerFirstName, string buyerAddress, string buyerCity)
        {
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();

                if (buyerLastName != null)
                {
                    connection.Query("UPDATE TicketTransactions SET [BuyerLastName] = @BuyerLastName WHERE [TransactionID] = @TransactionID; ", new { BuyerLastName = buyerLastName, TransactionID = transactionID });
                }
                if (buyerAddress != null)
                {
                    connection.Query("UPDATE TicketTransactions SET[BuyerAddress] = @BuyerAddress WHERE[TransactionID] = @TransactionID; ", new { BuyerAddress = buyerAddress, TransactionID = transactionID });
                }
                if (buyerFirstName != null)
                {
                    connection.Query("UPDATE TicketTransactions SET [BuyerFirstName] = @BuyerFirstName WHERE [TransactionID] = @TransactionID; ", new { BuyerFirstName = buyerFirstName, TransactionID = transactionID });
                }
                if (buyerCity != null)
                {
                    connection.Query("UPDATE TicketTransactions SET [BuyerCity] = @BuyerCity WHERE [TransactionID] = @TransactionID; ", new { BuyerCity = buyerCity, TransactionID = transactionID });
                }
            }
        }

        /// <summary>
        /// Method that Deletes a customer Order 
        /// It deletes a row from table TicketsToTransactions
        /// and a row from table TicketTransactions.
        /// </summary>
        /// <param name="transactionID"></param>
        /// <param name="ticketID"></param>
        public void DeleteCustomerOrder(int transactionID)
        {
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                /*
                var seatIDsThatAreChangedToAvailable = connection.Query("SELECT Tickets.SeatID From Tickets " +
                    "INNER JOIN TicketsToTransactions ON TicketsToTransactions.TicketID = Tickets.TicketID " +
                    "INNER JOIN TicketTransactions ON TicketTransactions.TransactionID = TicketsToTransactions.TransactionID " +
                    "WHERE TicketTransactions.TransactionID =  @TransactionID", new { TransactionID = transactionID }).FirstOrDefault();

                var orderTicketID = connection.Query("SELECT Tickets.TicketID From Tickets " +
                    "INNER JOIN TicketsToTransactions ON TicketsToTransactions.TicketID = Tickets.TicketID " +
                    "INNER JOIN TicketTransactions ON TicketTransactions.TransactionID = TicketsToTransactions.TransactionID " +
                    "WHERE TicketTransactions.TransactionID = @TransactionID", new { TransactionID = transactionID }).FirstOrDefault();

                var orderTicketEventDateID = connection.ExecuteScalar<int>("Select TicketEventDates.TicketEventDateID From TicketEventDates " +
                    "INNER JOIN SeatsAtEventDate ON SeatsAtEventDate.TicketEventDateID = TicketEventDates.TicketEventDateID " +
                    "INNER JOIN Tickets ON Tickets.SeatID = SeatsAtEventDate.SeatID " +
                    "INNER JOIN TicketsToTransactions ON TicketsToTransactions.TicketID = Tickets.TicketID WHERE TicketID = @TicketID", new { TicketID = orderTicketID });
                */
                var tickets = connection.Query<int>("SELECT TicketsToTransactions.TicketID FROM TicketsToTransactions WHERE TicketsToTransactions.TransactionID = @ID", new { ID = transactionID });
                connection.Execute("DELETE FROM TicketsToTransactions WHERE TicketsToTransactions.TransactionID = @ID", new { ID = transactionID });
                connection.Execute("DELETE FROM TicketTransactions WHERE TransactionID = @ID ", new { ID = transactionID });
                foreach (var ticket in tickets)
                {
                    connection.Execute("DELETE FROM Tickets WHERE TicketID = @TicketID", new { TicketID = ticket });
                }
            }
        }


        // TODO: dokumentera att returvärdet är ett transaktions-ID!
        // TODO: flera tickets per order i mån av tid?
        /// <summary>
        /// Method that returns a transactionID that represents a customer order.
        /// </summary>
        /// <param name="buyerFirstName"></param>
        /// <param name="buyerLastName"></param>
        /// <param name="buyerAddress"></param>
        /// <param name="buyerCity"></param>
        /// <param name="paymentStatus"></param>
        /// <param name="paymentReferenceID"></param>
        /// <param name="ticketID"></param>
        /// <param name="buyerEmailAddress"></param>
        /// <returns>A transactionID that represents a customerOrder</returns>
        public int AddCustomerOrder(string buyerFirstName, string buyerLastName, string buyerAddress, string buyerCity, PaymentStatus paymentStatus, string paymentReferenceID,
                                      int[] ticketIDs, string buyerEmailAddress)
        {
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                string query = "INSERT INTO TicketTransactions " +
                    "(BuyerLastName, BuyerFirstName, BuyerAddress, BuyerCity, BuyerEmailAddress, PaymentStatus, PaymentReferenceId) " +
                    "VALUES (@ln, @fn, @addr, @city, @email, @status, @refid); SELECT SCOPE_IDENTITY();";
                var transactionParams = new
                {
                    fn = buyerFirstName,
                    ln = buyerLastName,
                    addr = buyerAddress,
                    city = buyerCity,
                    email = buyerEmailAddress,
                    status = paymentStatus,
                    refid = paymentReferenceID
                };
                int transactionId = connection.ExecuteScalar<int>(query, transactionParams);

                foreach (int tID in ticketIDs)
                {
                    connection.Execute($"INSERT INTO TicketsToTransactions (TicketID, TransactionID) VALUES ({tID}, {transactionId});");
                }
                return transactionId;
            }
        }


        /// <summary>
        /// Method used to add a new TicketEventDate and seats for that ticketeventdate.
        /// </summary>
        /// <param name="ticketEventID"></param>
        /// <param name="venueID"></param>
        /// <param name="eventDateTime"></param>
        /// <param name="numberOfSeats"></param>
        /// <returns>A new TicketEventDate object.</returns>
        public TicketEventDate AddTicketEventDate(int ticketEventID, int venueID, DateTime eventDateTime, int numberOfSeats)
        {

            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                if (!DateTimeIsValid(eventDateTime))
                {
                    throw new ArgumentException("Datetime out of range.");
                }
                var createdEventID = connection.ExecuteScalar<int>("INSERT INTO TicketEventDates([TicketEventID],[VenueID],[EventStartDateTime]) VALUES(@TicketEventID, @VenueID, @EventStartDateTime); SELECT SCOPE_IDENTITY();", new { TicketEventID = ticketEventID, VenueID = venueID, EventStartDateTime = eventDateTime });

                var seatQuery = "INSERT INTO SeatsAtEventDate([TicketEventDateID]) VALUES(@ID)";
                var parameters = new { ID = createdEventID };
                for (int i = 0; i < numberOfSeats; i++)
                {
                    connection.Query(seatQuery, parameters);
                }
                return connection.Query<TicketEventDate>("SELECT * FROM TicketEventDates WHERE TicketEventDateID=@Id", parameters).First();
            }
        }

        /// <summary>
        /// Method used for updating TicketEventDates, any of the attributes or all of the attributes,
        /// except primary key of the database table.
        /// </summary>
        /// <param name="ticketEventDateID"></param>
        /// <param name="ticketEventID"></param>
        /// <param name="venueID"></param>
        /// <param name="eventDateTime"></param>
        public void UpdateTicketEventDate(int ticketEventDateID, int ticketEventID, int venueID, DateTime eventDateTime)
        {
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                if (ticketEventID != 0)
                {
                    connection.Query("UPDATE TicketEventDates SET [TicketEventID] = @TicketEventID WHERE [TicketEventDateID] = @TicketEventDateID; ", new { TicketEventID = ticketEventID, TicketEventDateID = ticketEventDateID });
                }
                if (venueID != 0)
                {
                    connection.Query("UPDATE TicketEventDates SET [VenueID] = @VenueID WHERE [TicketEventDateID] = @TicketEventDateID; ", new { VenueID = venueID, TicketEventDateID = ticketEventDateID });
                }
                if (DateTimeIsValid(eventDateTime))
                {
                    connection.Query("UPDATE TicketEventDates SET [EventStartDateTime] = @EventStartDateTime WHERE [TicketEventDateID] = @TicketEventDateID; ", new { EventStartDateTime = eventDateTime, TicketEventDateID = ticketEventDateID });
                }
            }
        }

        /// <summary>
        /// Method that checks that defined datetimes is in the intervall of sql min datetime and sql max datetime.
        /// </summary>
        /// <param name="t"></param>
        /// <returns>true if the date is ok, otherwise false.</returns>
        private bool DateTimeIsValid(DateTime t)
        {
            return t != null && t >= SQL_MIN_DATETIME && t <= SQL_MAX_DATETIME;
        }

        /// <summary>
        /// Method that gets data on a specific Ticket: TicketID, SeatID from table
        /// Tickets. To get other relevant information we join with Venues, TicketEventDates, 
        /// TicketEvents tables. Represents a ticket that a customer should be able to view
        /// in her shopping basket when added.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A Ticket object.</returns>
        public Ticket FindTicketByTicketID(int id)
        {
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                return connection.Query<Ticket>("SELECT Tickets.*, Venues.VenueName AS VenueName, TicketEventDates.EventStartDateTime AS EventStartDateTime, " +
                    "TicketEvents.TicketEventPrice AS TicketEventPrice, TicketEvents.EventName AS EventName " +
                    "FROM Tickets " +
                    "INNER JOIN SeatsAtEventDate ON SeatsAtEventDate.SeatID = Tickets.SeatID " +
                    "INNER JOIN TicketEventDates ON TicketEventDates.TicketEventDateID = SeatsAtEventDate.TicketEventDateID " +
                    "INNER JOIN TicketEvents ON TicketEvents.TicketEventID = TicketEventDates.TicketEventID " +
                    "INNER JOIN Venues on Venues.VenueID = TicketEventDates.VenueID " +
                    "WHERE Tickets.TicketID = @ID", new { ID = id }).FirstOrDefault();
            }
        }

        /// <summary>
        /// Method that is just used to create a Ticket in the database table Tickets.
        /// </summary>
        /// <param name="ticketEventDateID"></param>
        /// <returns>A ticket from the database with a ticketid and a seatid.</returns>
        public Ticket CreateTicket(int ticketEventDateID)
        {

            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                var availableSeat = FindOneAvailableSeatAtTicketEventDate(ticketEventDateID);

                var ticketID = connection.ExecuteScalar<int>("INSERT INTO Tickets (Tickets.SeatID) VALUES (@AvSeat); SELECT SCOPE_IDENTITY() ", new { AvSeat = availableSeat });
                return FindTicketByTicketID(ticketID);
            }
        }

        private bool AreThereAnyFreeSeatsAtEventDate(int ticketEventDateID)
        {
            IEnumerable<int> seats = GetAvailableSeatsAtTicketEventDate(ticketEventDateID);
            if (seats.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public IEnumerable<Ticket> GetAllTickets()
        {
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                return connection.Query<Ticket>("SELECT Tickets.*, Venues.VenueName AS VenueName, TicketEventDates.EventStartDateTime AS EventStartDateTime, " +
                    "TicketEvents.TicketEventPrice AS TicketEventPrice, TicketEvents.EventName AS EventName " +
                    "FROM Tickets " +
                    "INNER JOIN SeatsAtEventDate ON SeatsAtEventDate.SeatID = Tickets.SeatID " +
                    "INNER JOIN TicketEventDates ON TicketEventDates.TicketEventDateID = SeatsAtEventDate.TicketEventDateID " +
                    "INNER JOIN TicketEvents ON TicketEvents.TicketEventID = TicketEventDates.TicketEventID " +
                    "INNER JOIN Venues on Venues.VenueID = TicketEventDates.VenueID ");
            }
        }
        /// <summary>
        /// Method that finds all ticketIDs that are connected to one specific order (tickettransaction).
        /// </summary>
        /// <param name="transactionID"></param>
        /// <returns>A list of TicketIDs.</returns>
        private IEnumerable<int> FindTicketsByTransactionID(int transactionID)
        {
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                return connection.Query<int>($"SELECT TicketsToTransactions.TicketID From TicketsToTransactions WHERE TicketsToTransactions.TransactionID = {transactionID}");
            }
        }

        /// <summary>
        /// Method that fetches a list of all seatids connected to a specific ticketeventdate in the ticketEventDate 
        /// database table.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>A list of seatIDs for the specified datetime</returns>
        public IEnumerable<int> GetSeatsAtTicketEventDate(int ticketEventDateID)
        {
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                return connection.Query<int>("SELECT SeatsAtEventDate.SeatID From SeatsAtEventDate INNER JOIN TicketEventDates ON TicketEventDates.TicketEventDateID = " +
                    "SeatsAtEventDate.TicketEventDateID " +
                    "WHERE(TicketEventDates.TicketEventDateID) = @TicketEventDID; ", new { TicketEventDID = ticketEventDateID });
            }
        }

        /// <summary>
        /// Gets the number of seats for a specific ticketeventdate, based on the ticketeventdateid sent in to the method.
        /// </summary>
        /// <param name="ticketEventDateID">The ticketeventdateid for which we want to return the number of seats for.</param>
        /// <returns>A list of all seats at a ticketeventdate.</returns>
        public int GetNumberOfSeatsAtTicketEventDate(int ticketEventDateID)
        {
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                return connection.Query<int>("SELECT COUNT(SeatsAtEventDate.SeatID) AS NumberOfSeats From SeatsAtEventDate " +
                    "INNER JOIN TicketEventDates ON TicketEventDates.TicketEventDateID = " +
                    "SeatsAtEventDate.TicketEventDateID " +
                    "WHERE(TicketEventDates.TicketEventDateID) = @TicketEventDID; ", new { TicketEventDID = ticketEventDateID }).FirstOrDefault();
            }
        }

        /// <summary>
        /// Method that returns a list of seatids of available seats at a specific eventdateTime.
        /// </summary>
        /// <param name="dateTime">The ticketeventid that we want to return the available seats for.</param>
        /// <returns>A list of seatids that are available for a specific ticketeventdate.</returns>
        public IEnumerable<int> GetAvailableSeatsAtTicketEventDate(int ticketEventDateID)
        {
            // TODO: skriv klart detta
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                return connection.Query<int>("SELECT SeatsatEventDate.SeatID  From SeatsAtEventDate " +
                    "INNER JOIN TicketEventDates ON TicketEventDates.TicketEventDateID = SeatsAtEventDate.TicketEventDateID " +
                    "WHERE(TicketEventDates.TicketEventDateID) = @TicketEventDID " +
                    "AND SeatsAtEventDate.SeatID NOT IN(SELECT Tickets.SeatID From Tickets); ", new { TicketEventDID = ticketEventDateID });
            }
        }

        /// <summary>
        /// Method that returns a seatid that are available on a specific ticketeventdate.
        /// </summary>
        /// <param name="ticketEventDateID">The id of the ticketeventdate that we want to get a seat for.</param>
        /// <returns>A free seatid for the specified ticketeventdate.</returns>
        private int FindOneAvailableSeatAtTicketEventDate(int ticketEventDateID)
        {
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                return connection.Query<int>("SELECT TOP 1 SeatsatEventDate.SeatID  From SeatsAtEventDate " +
                    "INNER JOIN TicketEventDates ON TicketEventDates.TicketEventDateID = SeatsAtEventDate.TicketEventDateID " +
                    "WHERE(TicketEventDates.TicketEventDateID) = @TicketEventDID " +
                    "AND SeatsAtEventDate.SeatID NOT IN(SELECT Tickets.SeatID From Tickets); ", new { TicketEventDID = ticketEventDateID }).FirstOrDefault();
            }
        }
        /// <summary>
        /// Method that returns the number of available seats at a specific ticketeventdates eventdatetime.
        /// </summary>
        /// <param name="dateTime">the ticketeventdateID that we want to check for how many seats that are available.</param>
        /// <returns>The number of available seats</returns>
        public int GetNROfAvailableSeatsAtTicketEventDate(int ticketEventDateID)
        {
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                return connection.Query<int>("SELECT COUNT(SeatsatEventDate.SeatID) AS NumberOfAvailableSeats " +
                    "INNER JOIN TicketEventDates ON TicketEventDates.TicketEventDateID = SeatsAtEventDate.TicketEventDateID WHERE(TicketEventDates.TicketEventDateID = @TicketEventDID " +
                    "AND SeatsAtEventDate.SeatID NOT IN(SELECT Tickets.SeatID From Tickets); ", new { TicketEventDID = ticketEventDateID }).FirstOrDefault();
            }
        }

        /// <summary>
        /// Methods that updates an existing Ticket in the Ticket database table.
        /// Only SeatID can be changed, method also checks that the seat number
        /// one tries to change into actually exists for the specific ticketeventdate.
        /// TO FIX: the above!
        /// </summary>
        /// <param name="ticketID"></param>
        /// <param name="seatID"></param>
        public void UpdateTicket(int ticketID, int seatID)
        {
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                var seatExists = connection.Query("SELECT SeatsAtEventDate.SeatID FROM SeatsAtEventDate WHERE SeatsAtEventDate.SeatID = @SeatID", new { SeatID = seatID });

                connection.Query("UPDATE TicketEvents SET [SeatID] = @SeatID WHERE [TicketID] = @TicketID; ", new { SeatID = seatID, TicketID = ticketID });
            }
        }

        /// <summary>
        /// Method used to delete a ticket from Ticket table in database.
        /// Ticket can only be deleted once the transaction connected to 
        /// the ticket has been deleted as well as the ticket to transaction
        /// row in tickestotransactions table.
        /// </summary>
        /// <param name="ticketID"></param>
        public void DeleteTicket(int ticketID)
        {
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                connection.Query("DELETE FROM [Tickets] WHERE TicketID = @ID", new { ID = ticketID });
            }
        }

        /// <summary>
        /// Method used to search for a specific TicketEventDate 
        /// by it's ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A TicketEventDate Object.</returns>
        public TicketEventDate FindTicketEventDateByID(int id)
        {
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                return connection.Query<TicketEventDate>("SELECT TicketEventDates.*, Venues.VenueName, TicketEvents.EventName, innerTable.seats AS NumberOfSeats " +
                    "FROM TicketEventDates " +
                    "INNER JOIN " +
                        "(SELECT SeatsAtEventDate.TicketEventDateID AS id, COUNT(*) AS seats FROM SeatsAtEventDate GROUP BY SeatsAtEventDate.TicketEventDateID) innerTable " +
                    "ON TicketEventDates.TicketEventDateID = innerTable.id " +
                    "INNER JOIN [Venues] ON TicketEventDates.VenueID = Venues.VenueID " +
                    "INNER JOIN [TicketEvents] ON TicketEventDates.TicketEventID = TicketEvents.TicketEventID " +
                    "WHERE TicketEventDates.TicketEventDateID = @ID", new { ID = id }).FirstOrDefault();
            }
        }

        /// <summary>
        /// Method that search in the database for TicketEventDates,
        /// one can search on eventname, eventdateid, eventid, venuename.
        /// Method joins TicketEventDates table with Venues, TicketEvents,
        /// and also has a subquery that makes it possible to show how many
        /// seats a specific eventdate have.
        /// </summary>
        /// <param name="query">The ticketeventdateid, ticketeventid, venueid, venuename or eventname 
        /// that one want to see ticketeventdates for.</param>
        /// <returns>A list of TicketEventDate objects.</returns>
        public IEnumerable<TicketEventDate> FindTicketEventDates(string query)
        {
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                int id = -1;
                int.TryParse(query, out id);
                return connection.Query<TicketEventDate>(
                    "SELECT TicketEventDates.*, Venues.VenueName, TicketEvents.EventName, innerTable.seats AS NumberOfSeats " +
                    "FROM TicketEventDates " +
                    "INNER JOIN " +
                        "(SELECT SeatsAtEventDate.TicketEventDateID AS id, COUNT(*) AS seats FROM SeatsAtEventDate GROUP BY SeatsAtEventDate.TicketEventDateID) innerTable " +
                    "ON TicketEventDates.TicketEventDateID = innerTable.id " +
                    "INNER JOIN [Venues] ON TicketEventDates.VenueID = Venues.VenueID " +
                    "INNER JOIN [TicketEvents] ON TicketEventDates.TicketEventID = TicketEvents.TicketEventID " +
                    "WHERE TicketEventDates.TicketEventDateID = @ID OR TicketEvents.TicketEventID = @ID OR Venues.VenueID = @ID OR Venues.VenueName LIKE @Query OR TicketEvents.EventName LIKE @Query;",
                    new { ID = id, Query = $"%{query}%" }
                );
            }
        }

        /// <summary>
        /// Method that gets all ticketeventdates from database table TicketEventDates
        /// represented as a list of TicketEventDate objects.
        /// </summary>
        /// <returns>A list of ticketEventDate objects.</returns>
        public List<TicketEventDate> GetAllTicketEventDates()
        {
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                return connection.Query<TicketEventDate>("SELECT TicketEventDates.*,innerTable.seats AS NumberOfSeats FROM TicketEventDates " +
                    "INNER JOIN (SELECT SeatsAtEventDate.TicketEventDateID AS id, COUNT(*) AS seats FROM SeatsAtEventDate GROUP BY " +
                    "SeatsAtEventDate.TicketEventDateID) innerTable ON TicketEventDates.TicketEventDateID = innerTable.id ").ToList();
            }
        }

        /// <summary>
        /// Method that deletes a specific TicketEventDate. First we need to delete the seats
        /// connected to the TicketEventDateID in table SeatsAtEventDate.
        /// </summary>
        /// <param name="id"></param>
        public void DeleteTicketEventDate(int id)
        {
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                connection.Query("DELETE FROM SeatsAtEventDate WHERE TicketEventDateID = @ID", new { ID = id }).FirstOrDefault();
                connection.Query<TicketEventDate>("DELETE FROM [TicketEventDates] WHERE TicketEventDateID = @ID", new { ID = id }).FirstOrDefault();
            }
        }
    }
}
