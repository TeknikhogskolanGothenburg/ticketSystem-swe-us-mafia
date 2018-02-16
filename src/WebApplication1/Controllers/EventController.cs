using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicketSystem.DatabaseRepository;
using TicketSystemEngine;
using Newtonsoft.Json;

namespace RestApplication.Controllers
{
    [Route("[controller]")]
    public class EventController : Controller
    {
        TicketDatabase ticketDb = new TicketDatabase();
        // GET /event
        [HttpGet]
        public IEnumerable<TicketEvent> GetAllEvents()
        {
            return ticketDb.FindAllEvents();
        }

        // GET event/search/ark
        [HttpGet("search/{query}")]
        public IEnumerable<TicketEvent> FindEvents (string query)
        {
            return ticketDb.FindEvents(query);
        }

        // GET event/search/ark
       /* [HttpGet("{query}")]
        public int GetTicketEventID(string query)
        {
            return ticketDb.GetTicketEventID(query);
        }*/

        // GET events/5
        [HttpGet("{id}")]
        public TicketEvent GetSpecificEvent(int id)
        {
            return ticketDb.FindEventByID(id);
        }

        /// <summary>
        /// Method that creates a TicketEvent in ticketevents database table.
        /// </summary>
        /// <param name="ticketEvent"></param>
        /// <returns></returns>
        // POST /events
        [HttpPost]
        public TicketEvent CreateTicketEvent([FromBody]TicketEvent ticketEvent)
        {      
            return ticketDb.EventAdd(ticketEvent.EventName, ticketEvent.EventHtmlDescription, ticketEvent.TicketEventPrice);
        }

        /// <summary>
        /// Changes attributes of the ticketevent found
        /// through parameter id (ticketeventID).
        /// </summary>
        /// <param name="id">The id of the ticketevent to be updated.</param>
        /// <param name="ticketEvent">TicketEvent object that we are updating values for.</param>
        // PUT /events/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]TicketEvent ticketEvent)
        {
            if(ticketDb.FindEventByID(id) == null)
            {
                Response.StatusCode = 404;
                return;
            }
            else
            {
                ticketDb.UpdateEvent(id, ticketEvent.EventName, ticketEvent.EventHtmlDescription, ticketEvent.TicketEventPrice);
            }
        }

        /// <summary>
        /// Method that deletes a ticketEvent from the database table ticketevents
        /// based on provided ticketeventdateid.
        /// Needs to be called before deleting a ticketEventDate because they are
        /// connected.
        /// </summary>
        /// <param name="id">The ticketeventId for the event to be deleted.</param>
        // DELETE /events/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            if (ticketDb.FindEventByID(id) == null)
            {
                Response.StatusCode = 404;
            }
            else
            {
                ticketDb.DeleteEvent(id);
            }
        }
    }
}
