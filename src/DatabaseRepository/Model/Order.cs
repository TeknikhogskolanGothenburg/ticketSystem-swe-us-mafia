using System;
using System.Collections.Generic;
using System.Text;

namespace TicketSystem.DatabaseRepository.Model
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
        public int TicketID { get; set; }
        public int SeatID { get; set; }
        public string BuyerEmailAddress { get; set; }
        public DateTime EventDateTime { get; set; }
        public string EventName { get; set; }

    }
}
