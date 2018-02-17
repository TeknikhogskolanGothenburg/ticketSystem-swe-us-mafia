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
        
        /// <summary>
        /// Method that get All TicketEvents as a list (IEnumrable) from the 
        /// database table TicketEvents.
        /// </summary>
        /// <returns>A list (IEnumerable) of TicketEvent objects.</returns>
        // GET /event
        [HttpGet]
        public IEnumerable<TicketEvent> GetAllEvents()
        {
            return ticketDb.FindAllEvents();
        }

        /// <summary>
        /// Method that searches for TicketEvents based on
        /// either TicketEventID or EventName (also part of 
        /// EventName works).
        /// </summary>
        /// <param name="query">The search criteria sent in to method.</param>
        /// <returns>A list (IEnumerable) of TicketEvents.</returns>
        // GET event/search/ark
        [HttpGet("search/{query}")]
        public IEnumerable<TicketEvent> FindEvents (string query)
        {
            return ticketDb.FindEvents(query);
        }

        /// <summary>
        /// Method that gets a specific TicketEvent from database table
        /// TicketEvents based on ticketeventid sent in to method.
        /// </summary>
        /// <param name="id">The TicketEventID of the TicketEvent we want to get data on.</param>
        /// <returns>A TicketEvent object.</returns>
        // GET event/5
        [HttpGet("{id}")]
        public TicketEvent GetSpecificEvent(int id)
        {
            return ticketDb.FindEventByID(id);
        }

        /// <summary>
        /// Method that creates a TicketEvent in TicketEvents database table.
        /// </summary>
        /// <param name="ticketEvent">The properties sent in to method are properties of a TicketEvent object.</param>
        /// <returns>A TicketEvent object.</returns>
        // POST /event
        [HttpPost]
        public TicketEvent CreateTicketEvent([FromBody]TicketEvent ticketEvent)
        {      
            return ticketDb.EventAdd(ticketEvent.EventName, ticketEvent.EventHtmlDescription, ticketEvent.TicketEventPrice);
        }

        /// <summary>
        /// Method that changes attributes of the TicketEvent found
        /// through parameter id (ticketeventID).
        /// </summary>
        /// <param name="id">The id of the TicketEvent to be updated.</param>
        /// <param name="ticketEvent">TicketEvent object that we are updating values for.</param>
        // PUT /event/5
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
        // DELETE /event/5
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
