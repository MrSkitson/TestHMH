
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;

namespace TestHMH
{

    public class Tests
    {

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

           // AuthenticateAndSetToken().GetAwaiter().GetResult();
        }
        private async Task AuthenticateAndSetToken()
        {
            var loginUrl = "https://favqs.com/api/session";
            // get data for configuration
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            IConfiguration config = builder.Build();

            var login = config["UserLogin"];
            var password = config["UserPassword"];

            // request
            var requestContent = new StringContent(JsonConvert.SerializeObject(new
            {
                user = new { login, password }
            }), Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                // add headres to API
                client.DefaultRequestHeaders.Add("Authorization", $"Token token=\"{config["FavQsApiKey"]}\"");

                // request to craete new session
                var response = await client.PostAsync(loginUrl, requestContent);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JObject.Parse(responseContent);

                    // user-token from response
                    _userToken = jsonResponse["User-Token"].ToString();

                    // Save token
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", $"token=\"{_userToken}\"");
                }
                else
                {
                    throw new InvalidOperationException("Autification was failed in API FavQs");
                }
            }
        }
        private HttpClient CreateHttpClient(string apiKey)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new System.Uri("https://favqs.com/api")
            };
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Token token=\"{apiKey}\"");
           
            return httpClient;
        }
       
        //Test PUT method for Fav Qoute
        [Test]
        [TestCase("4")]
        public async Task PutQuotesFAV(string quoteId)
        {
            var requestUri = $"/api/quotes/{quoteId}/fav";
            var request = new HttpRequestMessage(HttpMethod.Put, requestUri);
            if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", $"token=\"{_userToken}\"");
            }
            var response = await _httpClient.SendAsync(request);
            Console.WriteLine($"Status Code: {response.StatusCode}");
            Console.WriteLine($"Response Content: {await response.Content.ReadAsStringAsync()}");
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseContent);

            // Parse the response JSON
            var jsonResponse = JObject.Parse(responseContent);
        }

        //Validation
        [Test]        
        public async Task ValidateFavoriteStatusChange()
        {
            // Sending a request to the API
            var response = await _httpClient.GetAsync("/quotes");
            var content = await response.Content.ReadAsStringAsync();
            // Outputting the response content to the console for diagnostics
            Console.WriteLine(content);
            // Attempting deserialization (skip this step if the response is not in JSON format) check if bool was updated
            try
            {
                var quotesResponse = JsonConvert.DeserializeObject<QuotesResponse>(content);
                var quote = quotesResponse.Quotes.FirstOrDefault(q => q.Id == 4);
                Assert.IsNotNull(quote);
                Assert.IsTrue(quote.UserDetails.Favorite);
            }
            catch (JsonReaderException ex)
            {
                Console.WriteLine("Error during deserialization: " + ex.Message);
                // You can add additional error handling here

            }
          
          
        }


        [Test]
        [TestCase("4")]
        public async Task PutQuotesUnFVav(string quoteId)
        {
            //string quoteId = "4";
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
            var requestUri = "https://favqs.com/api/quotes";
            var response = await _httpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            // Check if code is 200
            Assert.IsTrue(response.IsSuccessStatusCode);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }

    }
  

}