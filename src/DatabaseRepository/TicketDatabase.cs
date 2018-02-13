using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using TicketSystemEngine;
using TicketSystem.DatabaseRepository;

namespace TicketSystem.DatabaseRepository
{
    public class TicketDatabase : ITicketDatabase
    {
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
                var result = connection.Query("insert into Venues([VenueName],[Address],[City],[Country]) values(@Name,@Address, @City, @Country); SELECT SCOPE_IDENTITY();", new { Name = name, Address = address, City = city, Country = country });
                var addedVenueQuery = result.First();
                return connection.Query<Venue>("SELECT * FROM Venues WHERE VenueID=@Id", new { Id = addedVenueQuery }).First();
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
        /// Method that creates a new TicketEvent in the database table TicketEvents.
        /// ID is automatically generated, thus not given as parameter in method.
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="htmlDescription"></param>
        /// <returns>A TicketEvent object.</returns>
        public TicketEvent EventAdd(string eventName, string htmlDescription)
        {
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                var result = connection.Query<int>("insert into TicketEvents([EventName],[EventHtmlDescription]) values(@EventName, @EventHtmlDescription); SELECT SCOPE_IDENTITY();", new { EventName = eventName, EventHtmlDescription = htmlDescription });
                var createdEventQuery = result.First();
                //var createdEventQuery = connection.Query<int>("SELECT IDENT_CURRENT ('TicketEvents') AS Current_Identity").First();
                return connection.Query<TicketEvent>("SELECT * FROM TicketEvents WHERE EventID=@Id", new { Id = createdEventQuery }).First();
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
        public void UpdateEvent(int id, string eventName, string htmlDescription)
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
                return connection.Query<Order>(TicketEventDateQuery("TicketTransactions.BuyerFirstName = @Query OR TicketTransactions.BuyerEmailAddress = @Query"), new { Query = query });
            }
        }

        /// <summary>
        /// Method that defines a query to be re-used in similar 
        /// individual methods that fetches data about TicketEventDates
        /// from the database.
        /// </summary>
        /// <param name="wherePart"></param>
        /// <returns>A specified query with varying where-statement
        /// depending on the result wanted.</returns>
        private string TicketEventDateQuery(string wherePart)
        {
            return "SELECT TicketsToTransactions.TransactionID AS TransactionID, TicketsToTransactions.TicketID AS TicketID, " +
                    "TicketTransactions.BuyerFirstName AS BuyerFirstName, TicketTransactions.BuyerLastName AS BuyerLastName, TicketTransactions.BuyerAddress " +
                    "AS BuyerAddress, TicketTransactions.BuyerCity AS BuyerCity, TicketTransactions.PaymentReferenceID AS PaymentReference, TicketTransactions.PaymentStatus " +
                    "AS PaymentStatus, TicketEventDates.EventStartDateTime AS EventStartDateTime, Tickets.SeatID AS SeatID, TicketEvents.EventName AS EventName, TicketTransactions.BuyerEmailAddress " +
                    "AS BuyerEmailAddress FROM [TicketsToTransactions] " +
                    "INNER JOIN TicketTransactions ON TicketsToTransactions.TransactionID = TicketTransactions.TransactionID " +
                    "INNER JOIN Tickets ON TicketsToTransactions.TicketID = Tickets.TicketID " +
                    "INNER JOIN SeatsAtEventDate ON Tickets.SeatID = SeatsAtEventDate.SeatID " +
                    "INNER JOIN TicketEventDates ON SeatsAtEventDate.TicketEventDateID = TicketEventDates.TicketEventDateID " +
                    "INNER JOIN TicketEvents ON TicketEventDates.TicketEventID = TicketEvents.TicketEventID " +
                    "INNER JOIN Venues ON TicketEventDates.VenueID = Venues.VenueID WHERE " + wherePart;
        }
        /// <summary>
        /// Method used to find an order from the database based on transactionID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A specific customer order.</returns>
        public Order FindCustomerOrderByID(int id)
        {
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                return connection.Query<Order>(TicketEventDateQuery("TicketTransactions.TransactionID = @ID"), new { ID = id }).FirstOrDefault();
            }
        }

        /// <summary>
        /// Method used to add a new TicketEventDate and numberOfSeats for the particular Event.
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
                if (eventDateTime != DateTime.MinValue)
                {
                    connection.Query("UPDATE TicketEventDates SET [EventStartDateTime] = @EventStartDateTime WHERE [TicketEventDateID] = @TicketEventDateID; ", new { EventStartDateTime = eventDateTime, TicketEventDateID = ticketEventDateID });
                }
            }
        }

        /// <summary>
        /// Method that gets data on a specific Ticket.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A Ticket object</returns>
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

        public IEnumerable<Ticket> FindTickets(string query)
        {
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                connection.Open();
                int id = -1;
                int.TryParse(query, out id);
                return connection.Query<Ticket>(("TicketTransac = @Query OR TicketTransactions.BuyerEmailAddress = @Query"), new { Query = query });
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
        /// <param name="query"></param>
        /// <returns>A list of TicketEventDates, TicketEventName, VenueName and Number of seats.</returns>
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
