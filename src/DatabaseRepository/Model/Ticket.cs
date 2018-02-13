using System;
using System.Collections.Generic;
using System.Text;

namespace TicketSystem.DatabaseRepository.Model
{
    public class Ticket
    {
        public int TicketID { get; set; }
        public int SeatID { get; set; }
        public string VenueName { get; set; }
        public string EventName { get; set; }
        public int TicketEventPrice { get; set; }
        public DateTime EventStartDateTime { get; set; }
    }
}
