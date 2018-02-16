using System;

namespace TicketSystemEngine
{
    public class TicketEventDate
    {
        public int TicketEventDateID { get; set; }
        public int TicketEventID { get; set; }
        public int VenueId{get;set; }
        public DateTime EventStartDateTime { get; set; }
        //public string VenueName { get; set; }
        public int NumberOfSeats { get; set; }
        //public string EventName { get; set; }
    }
}
