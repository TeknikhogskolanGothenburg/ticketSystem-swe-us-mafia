using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;
using TicketSystemEngine;

namespace TicketSystem.RestApiClient
{
    public class TicketEventDateApi
    {
        public List<TicketEventDate> GetAllTicketEventDate()
        {
            var client = new RestClient("http://localhost:51775/");
            var request = new RestRequest("TicketEventDate", Method.GET);
            var response = client.Execute<List<TicketEventDate>>(request);
            return response.Data;
        }

        public TicketEventDate GetTicketEventDates(string query)
        {
            var client = new RestClient("http://localhost:51775/");
            var request = new RestRequest("TicketEventDate/{query}", Method.GET);
            request.AddUrlSegment("query", query);
            var response = client.Execute<TicketEventDate>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException(string.Format("Event with query {0} not found", query));
            }

            return response.Data;
        }

        public TicketEventDate GetTicketEventDateByID(int TicketEventDateId)
        {
            var client = new RestClient("http://localhost:51775/");
            var request = new RestRequest("TicketEventDate/{id}", Method.GET);
            request.AddUrlSegment("id", TicketEventDateId);
            var response = client.Execute<TicketEventDate>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException(string.Format("TicketEventDate with ID: {0} is not found", TicketEventDateId));
            }

            return response.Data;
        }

        public void AddNewTicketEventDate()
        {
            var client = new RestClient("http://localhost:51775/");
            var request = new RestRequest("TicketEventDate", Method.POST);
            var response = client.Execute(request);
        }

        public void UpdateTicketEventDate()
        {
            var client = new RestClient("http://localhost:51775/");
            var request = new RestRequest("TicketEventDate", Method.PUT);
            var response = client.Execute(request);
        }

        public void DeleteTicketEventDate()
        {
            var client = new RestClient("http://localhost:51775/");
            var request = new RestRequest("TicketEventDate", Method.DELETE);
            var response = client.Execute(request);
        }

    }
}
