using System.ComponentModel.DataAnnotations;

namespace TicketSystemEngine
{
    public class Venue
    {
        public int VenueId { get; set; }
        [Required]
        public string VenueName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
    }
}
