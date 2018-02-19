using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace TicketShopWeb.Models
{
    public class CustomerSession
    {
        public CartModel Cart { get;}
        public List <CustomerModel> Customers { get; }
        public int CartID { get; }
        public CustomerSession(int cartID)
        { 
            Cart = new CartModel();
            CartID = cartID;
            Customers = new List<CustomerModel>();
        }
    }
}
