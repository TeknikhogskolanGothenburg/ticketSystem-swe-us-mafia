using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using TicketSystem.DatabaseRepository.Model;

namespace TicketSystem.DatabaseRepository
{
    public class TicketDatabase : ITicketDatabase
    {
        //test comment
        public TicketEvent EventAdd(string name, string description)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.Query("insert into TicketEvents(EventName, EventHtmlDescription) values(@Name, @Description)", new { Name = name, Description = description });
                var addedEventQuery = connection.Query<int>("SELECT IDENT_CURRENT ('TicketEvents') AS Current_Identity").First();
                return connection.Query<TicketEvent>("SELECT * FROM TicketEvents WHERE TicketEventID=@Id", new { Id = addedEventQuery }).First();
            }
        }

        public Venue VenueAdd(string name, string address, string city, string country)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.Query("insert into Venues([VenueName],[Address],[City],[Country]) values(@Name,@Address, @City, @Country)", new { Name = name, Address= address, City = city, Country = country });
                var addedVenueQuery = connection.Query<int>("SELECT IDENT_CURRENT ('Venues') AS Current_Identity").First();
                return connection.Query<Venue>("SELECT * FROM Venues WHERE VenueID=@Id", new { Id = addedVenueQuery }).First();
            }
        }

        public List<Venue> VenuesFind(string query)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.Query<Venue>("SELECT * FROM Venues WHERE VenueName like '%"+query+ "%' OR Address like '%" + query + "%' OR City like '%" + query + "%' OR Country like '%" + query + "%'").ToList();
            }
        }

        /// <summary>
        /// Method that is used to get all existing events from the database as a list,
        /// represented as a list of TicketEvent objects.
        /// </summary>
        /// <param name="query"></param>
        /// <returns>A list consisting of Ticket Event objects.</returns>
        public List<TicketEvent> EventFind()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.Query<TicketEvent>("SELECT * FROM TicketEvents").ToList();
            }
        }

        /// <summary>
        /// Method that fetches a specific event from the database table TicketEvent,
        /// based on either EventID or EventName.
        /// </summary>
        /// <param name="query"></param>
        /// <returns>A string with the database value that is cast as
        /// a TicketEvent </returns>
        public TicketEvent SpecificEventFind(string query)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return (TicketEvent)connection.Query<TicketEvent>("SELECT * FROM TicketEvent WHERE EventID like '%" + query + "%' OR EventName like '%" + query);
            }

        }
    }
}
