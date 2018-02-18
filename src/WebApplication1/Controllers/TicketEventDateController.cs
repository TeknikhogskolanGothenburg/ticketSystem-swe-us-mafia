using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketSystem.DatabaseRepository;
using TicketSystemEngine;

namespace RESTapi.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    public class TicketEventDateController : Controller
    {
        TicketDatabase ticketDb = new TicketDatabase();

        /// <summary>
        /// Method that gets all TicketEventDates from database table TicketEventDates
        /// as well as number of seats for each TicketEventDate from SeatsAtEventDate 
        /// table.
        /// </summary>
        /// <returns>A List of TicketEventDate objects.</returns>
        public List<TicketEventDate> GetAllTicketEventDates()
        {
            return ticketDb.GetAllTicketEventDates();
        }

        /// <summary>
        /// Method that gets all TicketEventDates based on the query sent in
        /// when calling the method.
        /// </summary>
        /// <param name="query"></param>
        /// <returns>A list (IEnumberable) of TicketEventDate objects.</returns>
        // GET: /ticketeventdate
        [HttpGet("search/{query}")]
        public IEnumerable<TicketEventDate> FindTicketEventDates(string query)
        {
            return ticketDb.FindTicketEventDates(query);
        }

        /// <summary>
        /// This method returns the number of available seats at at specific TicketEventDate,
        /// based on the TicketEventDateID sent in to method. To be used for showing customers
        /// how many available seats there are at the moment (after tickets have been sold
        /// already).
        /// </summary>
        /// <param name="id">TicketEventDateID of the TicketEventDate to show available seats for.</param>
        /// <returns>A number representing the number of available seats at a specific TicketEventDate.</returns>
        [HttpGet("available/{id}")]
        public int GetNrOfAvailableSeatsAtTicketEventDate(int id)
        {
            if (ticketDb.FindTicketEventDateByID(id) == null)
            {
                Response.StatusCode = 404;
            }
            return ticketDb.GetNROfAvailableSeatsAtTicketEventDate(id);
        }

        /// <summary>
        /// Method that gets a specific ticketeventdate based on the
        /// ticketeventdateid sent in to the method.
        /// </summary>
        /// <param name="id">TicketEventDateID of the TicketEventDate to get data on.</param>
        /// <returns>A TicketEventDate object.</returns>
        // GET: api/TicketEventDate/5
        [HttpGet("{id}")]
        public TicketEventDate GetSpecificTicketEventDate(int id)
        {
            if (ticketDb.FindTicketEventDateByID(id) == null)
            {
                Response.StatusCode = 404;
            }
            return ticketDb.FindTicketEventDateByID(id);
        }

        /// <summary>
        /// Method that adds a new TicketEventDate to the database.
        /// </summary>
        /// <param name="ticketEventDate">Defining that it is a TicketEventDate object's values we are posting.</param>
        // POST: /ticketEventDates
        [HttpPost]
        public void AddNewTicketEventDate([FromBody]TicketEventDate ticketEventDate)
        {
            if (!ModelState.IsValid || ticketEventDate.VenueId <= 0)
            {
                Response.StatusCode = 400;
            }
            else
            {
                try
                {
                    ticketDb.AddTicketEventDate(ticketEventDate.TicketEventID, ticketEventDate.VenueId, ticketEventDate.EventStartDateTime, ticketEventDate.NumberOfSeats);
                }
                catch (ArgumentException)
                {
                    Response.StatusCode = 400;
                }
                catch(SqlException)
                {
                    Response.StatusCode = 400;
                }
            }
        }

        /// <summary>
        /// Method that updates a TicketEventDate in the database table TicketEventDates
        /// based on the TicketEventDateID sent in to the method.
        /// </summary>
        /// <param name="id">ID of the TicketEventDate we want to update.</param>
        /// <param name="ticketEventDate">Defining that it is a TicketEventDate object's values we want to update.</param>
        // PUT: /ticketeventdate/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]TicketEventDate ticketEventDate)
        {
            if (ticketDb.FindTicketEventDateByID(id) == null)
            {
                Response.StatusCode = 404;
                return;
            }
            else
            {
                ticketDb.UpdateTicketEventDate(id, ticketEventDate.TicketEventID, ticketEventDate.VenueId, ticketEventDate.EventStartDateTime);
            }
        }

        /// <summary>
        /// Method that Deletes a TicketEventDate from the database based on TicketEventDateID sent in to method.
        /// TicketEventDate can't be deleted if it has seats connected to it, then you first need to delete 
        /// the Tickets connected to the TicketEventDate.
        /// </summary>
        /// <param name="id">ID of the TicketEventDate we want to delete.</param>
        // DELETE: /ticketeventdates/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            if (ticketDb.FindTicketEventDateByID(id) == null)
            {
                Response.StatusCode = 404;
            }
            else
            {
                ticketDb.DeleteTicketEventDate(id);
            }
        }
    }
}
