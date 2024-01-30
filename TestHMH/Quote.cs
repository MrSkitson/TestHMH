

using Newtonsoft.Json;

namespace TestHMH
{
    public class Quote
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Body { get; set; }
        //
        public UserDetails UserDetails { get; set; }
    }

    public class UserDetails
    {
        public bool Favorite { get; set; }
       //others details
    }
    public class QuotesResponse
    {
        public List<Quote> Quotes { get; set; }
    }

    public class ResponseConverter

    {
        public static void ResponseFromAPI(string[] args)
        {
            // JSON response from the API
            string jsonResponse = @"
            {
                ""id"": 4,
                ""author"": ""Albert Einstein"",
                ""body"": ""Make everything as simple as possible, but not simpler."",
                ""user_details"": {
                    ""favorite"": true
                }
            }";

            // Deserialize the JSON response into a Quote object
            Quote quote = JsonConvert.DeserializeObject<Quote>(jsonResponse);

            // Access the properties of the Quote object
            int quoteId = quote.Id;
            string author = quote.Author;
            string body = quote.Body;
            bool isFavorite = quote.UserDetails.Favorite;

            // Now you can work with the extracted data
            Console.WriteLine($"Quote ID: {quoteId}");
            Console.WriteLine($"Author: {author}");
            Console.WriteLine($"Body: {body}");
            Console.WriteLine($"Is Favorite: {isFavorite}");
        }
    }
}
