using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicketSystem.DatabaseRepository;
using TicketSystem.DatabaseRepository.Model;
using Newtonsoft.Json;

namespace RestApplication.Controllers
{
    [Route("[controller]")]
    public class EventController : Controller
    {
        TicketDatabase ticketDb = new TicketDatabase();
        // GET api/values
        [HttpGet]
        public IEnumerable<TicketEvent> GetAllEvents()
        {
            return ticketDb.EventFind();
        }

        // GET api/values/5
        [HttpGet("{query}")]
        public string GetSpecificEvent(string query)
        {
            return JsonConvert.SerializeObject(ticketDb.SpecificEventFind(query));
        }
        // POST api/values
        [HttpPost]
        public void Post([FromBody]TicketEvent ticketEvent)
        {
            //TicketEvent ticketEvent = JsonConvert.DeserializeObject<TicketEvent>(body);
            
            ticketDb.CreateEvent(ticketEvent.EventName, ticketEvent.EventHtmlDescription);
        }

        // PUT api/values/5
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
                ticketDb.UpdateEvent(id, ticketEvent.EventName, ticketEvent.EventHtmlDescription);
            }
        }

        // DELETE api/values/5
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
