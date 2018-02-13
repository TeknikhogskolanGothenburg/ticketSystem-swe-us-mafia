using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketSystem.DatabaseRepository.Model;
using TicketSystem.DatabaseRepository;

namespace RESTapi.Controllers
{
    /// <summary>
    /// Här får vi hantera att lägga in ett event på ett specifikt datum och en specifik venue tror jag.
    /// </summary>
    [Produces("application/json")]
    //[Route("[controller]")]
    [Route("api/TicketEventDate")]
    public class TicketEventDateController : Controller
    {
        TicketDatabase ticketDb = new TicketDatabase();
        // GET: api/TicketEventDate
        [HttpGet("search/{query}")]
        public IEnumerable<TicketEventDate> FindTicketEventDates(string query)
        {
            return ticketDb.FindTicketEventDates(query);
        }

        // GET: api/TicketEventDate/5
        [HttpGet("{id}")]
        public TicketEventDate GetSpecificEventDate(int id)
        {
            return ticketDb.FindTicketEventDateByID(id);
        }

        // POST: /ticketEventDates
        [HttpPost]
        public void AddNewTicketEventDate([FromBody]TicketEventDate ticketEventDate)
        {
            ticketDb.AddTicketEventDate(ticketEventDate.TicketEventID, ticketEventDate.VenueId, ticketEventDate.EventStartDateTime, ticketEventDate.NumberOfSeats);
        }

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
