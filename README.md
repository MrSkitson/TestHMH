his is a Tech Test project.
The task is to code a suite of tests that validates the main functionality of the FAV QUOTE and LIST QUOTES endpoints. 
I used the C# language and NUnit test project to check functionality.
I've created a JSON file, appsettings.json, to save my data (Token, login, password).

I added a Setup method to prepare before testing: a JSON file with the token data & create an HttpClient with this token through the private HttpClient CreateHttpClient(string apiKey) method.
I added 4 Tasks for each Test:
PutQuotesFAV & PutQuotesUnFVav - for the FAV QUOTE endpoint.
GetsListQuotes - for the LIST QUOTES endpoint.
ValidateFavoriteStatusChange.

I used Assert.IsTrue(response.IsSuccessStatusCode) to check if the response was successful.

as result - I got list of Quotes trough GetsListQuotes method.
 But couln't get acces to FAV QUOTE endpoint, I see the error message from console. 
 Standard Output: 
Status Code: Unauthorized
Response Content: HTTP Token: Access denied.


