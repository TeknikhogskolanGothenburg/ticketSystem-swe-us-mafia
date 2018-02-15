using System;
using System.Collections.Generic;
using System.Text;

namespace TicketSystemEngine
{
    public class Order
    {
        public int TransactionID { get; set; }
        public string BuyerLastName { get; set; }
        public string BuyerFirstName { get; set; }
        public string BuyerAddress { get; set; }
        public string BuyerCity { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentReferenceID { get; set; }
        public int[] TicketIDs { get; set; }
        public string BuyerEmailAddress { get; set; }
    }
}
