using System;
using System.Collections.Generic;
using System.Text;

namespace TicketSystem.DatabaseRepository.Model
{
    public class TicketToTransaction
    {
        public int TicketID { get; set; }
        public int TransactionID { get; set; }
    }
}
