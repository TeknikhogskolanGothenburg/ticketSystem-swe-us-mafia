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

        // GET events/5
        [HttpGet("{id}")]
        public TicketEvent GetSpecificEvent(int id)
        {
            return ticketDb.FindEventByID(id);
        }

        // POST /events
        [HttpPost]
        public TicketEvent CreateTicketEvent([FromBody]TicketEvent ticketEvent)
        {      
            return ticketDb.EventAdd(ticketEvent.EventName, ticketEvent.EventHtmlDescription, ticketEvent.TicketEventPrice);
        }

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
