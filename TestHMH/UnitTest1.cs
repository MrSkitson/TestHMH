
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;

namespace TestHMH
{

    public class Tests
    {
        private HttpClient CreateHttpClient(string apiKey)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new System.Uri("https://favqs.com/api")
            };
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Token token=\"{apiKey}\"");

            return httpClient;
        }

        private HttpClient _httpClient;
        private string _userToken;
        // Logic create new httpClient and coming through authorization using token from json
        [SetUp]
        public void Setup()
        {
            //call to json file with the token data
            var builder = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            IConfiguration config = builder.Build();

            var apiKey = config["FavQsApiKey"];
            _httpClient = CreateHttpClient(apiKey);
        }
        //Test PUT method for Fav Qoute
        [Test]
        public async Task PutQuotesFAV()
        {
            string quoteId = "4";
            var requestUri = $"/api/quotes/{quoteId}/fav";

            // Create an empty request body (if the API does not require specific data)
            HttpContent content = new StringContent("", Encoding.UTF8, "application/json");

            // Add the user session token to the headers
            _httpClient.DefaultRequestHeaders.Add("User-Token", _userToken);

            var response = await _httpClient.PutAsync(requestUri, content);
            Assert.IsTrue(response.IsSuccessStatusCode);

            // Check the response status code
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseContent);

            // Parse the response JSON
            var jsonResponse = JObject.Parse(responseContent);
        }



        [Test]
        public async Task PutQuotesUnFVav()
        {
            string quoteId = "4";
            var requestUri = $"/api/quotes/{quoteId}/unfav";

            
            HttpContent content = new StringContent("", Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(requestUri, content);
            Assert.IsTrue(response.IsSuccessStatusCode);

           
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseContent);

        }
        //GET List Quotes
        [Test]
        public async Task GetsListQuotes()
        {
            var response = await _httpClient.GetAsync("/quotes");

            // Check if the response is successful (status code 200 OK)
            Assert.IsTrue(response.IsSuccessStatusCode);

            // Read the response body as a string
            var content = await response.Content.ReadAsStringAsync();

            // Check the response status code
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Print the response body to the console
            Console.WriteLine(content);

        }
    }

}