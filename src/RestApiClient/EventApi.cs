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

        /*OBS (added by Sofia): Example method on how to use methods in restapi
         * to create event with eventdates
         /*
        public ActionResult CreateAnEventWithMultipleDates(string eventName, string description, int eventPrice, int venueID)
        {
            // Get these dates from user or wherever; probably easier to add dates one at a time...
            TicketEventDate[] dummyDates = {
                new TicketEventDate
                {
                    VenueId = venueID,
                    EventStartDateTime = DateTime.Parse("2018-03-01 20:00"),
                    NumberOfSeats = 100
                },
                new TicketEventDate
                {
                    VenueId = venueID,ws
                    EventStartDateTime = DateTime.Parse("2018-04-01 20:00"),
                    NumberOfSeats = 100
                }
                // ...
            };

            var client = new RestClient("http://localhost:51775/");

            // Create the event
            var request = new RestRequest("event", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(new { EventName = eventName, EventHtmlDescription = description, TicketEventPrice = eventPrice });
            var theEvent = client.Execute<TicketEvent>(request);

            // Add some dates
            foreach (var date in dummyDates)
            {
                var req = new RestRequest("ticketeventdate", Method.POST);
                req.RequestFormat = DataFormat.Json;
                req.AddBody(date);
                client.Execute(request);
            }

            return null; // Return some appropriate view here
        }
        */

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
