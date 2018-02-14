using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;
using TicketSystem.RestApiClient.Model;
using TicketSystemEngine;

namespace TicketSystem.RestApiClient
{
    public class EventApi
    {
        // TicketEvent refers to a model in class library
        public List<TicketEvent> GetAllEvents()
        {
            var client = new RestClient("http://localhost:51775/");
            var request = new RestRequest("Event", Method.GET);
            var response = client.Execute<List<TicketEvent>>(request);
            return response.Data;
        }

        public TicketEvent GetEventByQuery(string query)
        {
            var client = new RestClient("http://localhost:51775/");
            var request = new RestRequest("Venue/{query}", Method.GET);
            request.AddUrlSegment("query", query);
            var response = client.Execute<TicketEvent>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException(string.Format("Event with query {0} not found", query));
            }

            return response.Data;
        }

        public TicketEvent GetEventById(int EventId)
        {
            var client = new RestClient("http://localhost:51775/");
            var request = new RestRequest("Event/{id}", Method.GET);
            request.AddUrlSegment("id", EventId);
            var response = client.Execute<TicketEvent>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException(string.Format("Event with ID: {0} not found", EventId));
            }

            return response.Data;
        }

        public void AddNewEvent()
        {
            var client = new RestClient("http://localhost:51775/");
            var request = new RestRequest("Event", Method.POST);
            var response = client.Execute(request);
        }

        public void UpdateEvent()
        {
            var client = new RestClient("http://localhost:51775/");
            var request = new RestRequest("Venue", Method.PUT);
            var response = client.Execute(request);
        }

        public void DeleteEvent()
        {
            var client = new RestClient("http://localhost:51775/");
            var request = new RestRequest("Event", Method.DELETE);
            var response = client.Execute(request);
        }
    }
}
