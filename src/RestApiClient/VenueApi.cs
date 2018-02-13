using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;
using TicketSystem.RestApiClient.Model;

namespace TicketSystem.RestApiClient
{
    class VenueApi
    {
        // Venue is supposed to refer to a Model in a class library
        public List<Venue> VenueGet()
        {
            var client = new RestClient("http://localhost:18001/");
            var request = new RestRequest("Venue", Method.GET);
            var response = client.Execute<List<Venue>>(request);
            return response.Data;
        }

        public Venue VenueGetByID(int VenueId)
        {
            var client = new RestClient("http://localhost:18001/");
            var request = new RestRequest("Venue/{id}", Method.GET);
            request.AddUrlSegment("id", VenueId);
            var response = client.Execute<Venue>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException(string.Format("Venue with ID: {0} is not found", VenueId));
            }

            return response.Data;
        }

        public void AddNewVenue()
        {
            var client = new RestClient("http://localhost:18001/");
            var request = new RestRequest("Venue", Method.POST);
            var response = client.Execute(request);
            // how to get parameters in?
           // return response.Data;
        }

    }
}
