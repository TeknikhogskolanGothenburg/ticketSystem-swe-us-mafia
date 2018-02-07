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
    [Route("api/[controller]")]
    public class ValuesController : Controller
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
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
