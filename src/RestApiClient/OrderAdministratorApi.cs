using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;
using TicketSystemEngine;
namespace TicketSystem.RestApiClient
{
    public class OrderAdministratorApi
    {
        public List<Order> GetAllOrders()
        {
            var client = new RestClient("http://localhost:51775/");
            var request = new RestRequest("Order", Method.GET);
            var response = client.Execute<List<Order>>(request);
            return response.Data;
        }
    }
}
