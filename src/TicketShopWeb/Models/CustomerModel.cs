using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketSystemEngine;

namespace TicketShopWeb.Models
{
    public class CustomerModel
    {
        public List<TicketEvent> tEvent = new List<TicketEvent>();
        public List<TicketEventDate> dates = new List<TicketEventDate>();
        public List<Venue> venues = new List<Venue>();
        public List<Ticket> tickets = new List<Ticket>();
    }

    
}
