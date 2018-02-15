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

        public Order GetOrderByQuery(string query)
        {
            var client = new RestClient("http://localhost:51775/");
            var request = new RestRequest("Order/{query}", Method.GET);
            request.AddUrlSegment("query", query);
            var response = client.Execute<Order>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException(string.Format("Order with query {0} not found", query));
            }

            return response.Data;
        }

        public Order GetOrderById(int OrderId)
        {
            var client = new RestClient("http://localhost:51775/");
            var request = new RestRequest("Order/{id}", Method.GET);
            request.AddUrlSegment("id", OrderId);
            var response = client.Execute<Order>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException(string.Format("Order with ID: {0} not found", OrderId));
            }

            return response.Data;
        }

        public void AddNewOrder()
        {
            var client = new RestClient("http://localhost:51775/");
            var request = new RestRequest("Order", Method.POST);
            var response = client.Execute(request);
        }

        public void UpdateOrder()
        {
            var client = new RestClient("http://localhost:51775/");
            var request = new RestRequest("Order", Method.PUT);
            var response = client.Execute(request);
        }

        public void DeleteOrder()
        {
            var client = new RestClient("http://localhost:51775/");
            var request = new RestRequest("Order", Method.DELETE);
            var response = client.Execute(request);
        }
    }
}
