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
        private const string CONNECTION_STRING = "Server=(local)\\SqlExpress; Database=TicketSystem; Trusted_connection=true";
        //test comment
        public TicketEvent EventAdd(string name, string description)
        {
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                connection.Query("insert into TicketEvents(EventName, EventHtmlDescription) values(@Name, @Description)", new { Name = name, Description = description });
                var addedEventQuery = connection.Query<int>("SELECT IDENT_CURRENT ('TicketEvents') AS Current_Identity").First();
                return connection.Query<TicketEvent>("SELECT * FROM TicketEvents WHERE TicketEventID=@Id", new { Id = addedEventQuery }).First();
            }
        }

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

        public List<Venue> VenuesFind(string query)
        {
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                return connection.Query<Venue>("SELECT * FROM Venues WHERE VenueName like '%" + query + "%' OR Address like '%" + query + "%' OR City like '%" + query + "%' OR Country like '%" + query + "%'").ToList();
            }
        }

        public List<Venue> AllVenues()
        {
            // string connectionString = ConfigurationManager.ConnectionStrings["TicketSystem"].ConnectionString;
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                return connection.Query<Venue>("SELECT * FROM Venues").ToList();
            }
        }

        // update venue method here

        public void UpdateVenue(int id, string name, string address, string city, string country)
        {
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                if (name != null)
                {
                    connection.Query("UPDATE Venues SET VenueName = " + name + " WHERE VenueID =" + id);
                }
                if (address != null)
                {
                    connection.Query("UPDATE Venues SET VenueName = " + address + " WHERE VenueID =" + id);
                }
                if (city != null)
                {
                    connection.Query("UPDATE Venues SET VenueName = " + city + " WHERE VenueID =" + id);
                }
                if (country != null)
                {
                    connection.Query("UPDATE Venues SET VenueName = " + country + " WHERE VenueID =" + id);
                }

            }
        }

        public TicketEvent CreateEvent(string eventName, string htmlDescription)
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
        /// Method that is used to get all existing events from the database as a list,
        /// represented as a list of TicketEvent objects.
        /// </summary>
        /// <param name="query"></param>
        /// <returns>A list consisting of Ticket Event objects.</returns>
        public List<TicketEvent> EventFind()
        {
            using (var connection = new SqlConnection(CONNECTION_STRING))
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
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                /*SqlCommand cmd = new SqlCommand("SELECT * FROM TicketEvent WHERE TicketEventID = @EventID OR EventName LIKE @EventName", connection);

                SqlParameter idParam = new SqlParameter
                {
                    ParameterName = "@EventID",
                    Value = query
                };

                SqlParameter nameParam = new SqlParameter
                {
                    ParameterName = "@EventName",
                    Value = "%" + query + "%"
                };
                cmd.Parameters.Add(idParam);
                cmd.Parameters.Add(nameParam);
                var result = cmd.ExecuteReader();

                List<TicketEvent> events = new List<TicketEvent>();
                while (result.Read())
                {
                    var evt = new TicketEvent
                    {
                        TicketEventId = result["TicketEventID"],
                        EventName = result["EventName"],
                        EventHtmlDescription = result["EventHtmlDescription"]
                    };
                }*/

                // NOTE: super-vulnerable to SQL injection! How to do it with parameterized queries and still keep convenient return value?
                return connection.Query<TicketEvent>("SELECT * FROM [TicketEvents] WHERE TicketEventID = @ID OR EventName like @Query", new { ID = query, Query = $"%{query}%" }).FirstOrDefault();
            }
        }

        public TicketEvent FindEventByID(int id)
        {
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                return connection.Query<TicketEvent>("SELECT * FROM [TicketEvents] WHERE TicketEventID = @ID", new { ID = id}).FirstOrDefault();
            }
        }
    }
}
