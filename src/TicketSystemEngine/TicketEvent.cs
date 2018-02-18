using System.ComponentModel.DataAnnotations;

namespace TicketSystemEngine
{
    public class TicketEvent
    {
        public int TicketEventId { get; set; }
        [Required]
        public string EventName { get; set; }
        public string EventHtmlDescription { get; set; }
        [Range (150,1000)]
        public int TicketEventPrice { get; set; }
    }
}
