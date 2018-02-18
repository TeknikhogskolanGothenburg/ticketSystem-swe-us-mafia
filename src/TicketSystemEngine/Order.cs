using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TicketSystemEngine
{
    public class Order
    {
        public int TransactionID { get; set; }
        [Required]
        public string BuyerLastName { get; set; }
        [Required]
        public string BuyerFirstName { get; set; }
        public string BuyerAddress { get; set; }
        public string BuyerCity { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentReferenceID { get; set; }
        // this should really be int[], but RestClient gets confused when
        // confronted with JSON objects with array fields
        [Required]
        public string TicketIDs { get; set; }
        [Required]
        public string BuyerEmailAddress { get; set; }
    }
}
