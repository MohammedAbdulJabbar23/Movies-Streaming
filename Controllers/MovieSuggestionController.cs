// using System;
// using System.Net.Http;
// using System.Text;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Options;
// using Newtonsoft.Json;

// [ApiController]
// [Route("api/[controller]")]
// public class MovieSuggestionController : ControllerBase
// {
//     private readonly IHttpClientFactory _httpClientFactory;
//     private readonly GeminiSettings _geminiSettings;

//     public MovieSuggestionController(IHttpClientFactory httpClientFactory, IOptions<GeminiSettings> geminiSettings)
//     {
//         _httpClientFactory = httpClientFactory;
//         _geminiSettings = geminiSettings.Value;
//     }

//     [HttpGet("suggestions")]
//     public async Task<IActionResult> GetMovieSuggestions([FromQuery] string question)
//     {
//         if (string.IsNullOrEmpty(question))
//         {
//             return BadRequest("Please provide a question to suggest movies.");
//         }

//         var responseText = await GetMovieSuggestionsFromGemini(question);

//         if (string.IsNullOrEmpty(responseText))
//         {
//             return StatusCode(500, "Failed to fetch movie suggestions");
//         }

//         return Ok(new { suggestions = responseText });
//     }

//     private async Task<string> GetMovieSuggestionsFromGemini(string question)
//     {
//         var Gemini_API_KEY = _geminiSettings.ApiKey;
//         var client = _httpClientFactory.CreateClient();
//         var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={Gemini_API_KEY}";

//         // Structure the request body to include the user question for movie suggestions
//         var requestBody = new
//         {
//             contents = new[]
//             {
//             new
//             {
//                 parts = new[]
//                 {
//                     new { text = $"Suggest popular movies based on the following question: {question}" }
//                 }
//             }
//         }
//         };

//         // Serialize the request body to JSON
//         var jsonRequestBody = JsonConvert.SerializeObject(requestBody);
//         Console.WriteLine(jsonRequestBody);
//         var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

//         // Send the HTTP POST request
//         var response = await client.PostAsync(url, content);

//         // Handle error if the response is not successful
//         if (!response.IsSuccessStatusCode)
//         {
//             var errorResponse = await response.Content.ReadAsStringAsync();
//             Console.WriteLine($"Error: {response.StatusCode}, {errorResponse}");
//             return null;
//         }

//         // Parse the response
//         var jsonResponse = await response.Content.ReadAsStringAsync();
//         Console.WriteLine(jsonResponse);
//         try
//         {
//             var responseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

//             // Extract the movie suggestions from the response (assuming 'choices' and 'text' are correct fields)
//             return responseObject?.choices?[0]?.text?.ToString();  // Adjust if necessary based on the response
//         }
//         catch (JsonException ex)
//         {
//             Console.WriteLine($"Error parsing response: {ex.Message}");
//             return null;
//         }
//     }

// }
// using System;
// using System.Collections.Generic;
// using System.Net.Http;
// using System.Text;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Options;
// using Newtonsoft.Json;

// [ApiController]
// [Route("api/[controller]")]
// public class MovieSuggestionsController : ControllerBase
// {
//     private readonly IHttpClientFactory _httpClientFactory;
//     private readonly GeminiSettings _geminiSettings;

//     public MovieSuggestionsController(IHttpClientFactory httpClientFactory, IOptions<GeminiSettings> geminiSettings)
//     {
//         _geminiSettings = geminiSettings.Value;
//         _httpClientFactory = httpClientFactory;
//     }

//     [HttpPost("suggest-movies")]
//     public async Task<IActionResult> GetMovieSuggestions([FromBody] UserQuestionModel userQuestion)
//     {
//         var client = _httpClientFactory.CreateClient();
//         var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={_geminiSettings.ApiKey}";

//         // Structure the request body to include the user question and ask for structured JSON response
//         var requestBody = new
//         {
//             contents = new[]
//             {
//                 new
//                 {
//                     parts = new[]
//                     {
//                         new
//                         {
//                             text = $@"
//                                 Suggest popular movies based on the following question: {userQuestion.Question}. 
//                                 Return the results in a structured JSON format with the keys: 'movieName' and 'releaseYear' for each movie.
//                                 Example format:
//                                 {{
//                                     ""popularMovies"": [
//                                         {{
//                                             ""movieName"": ""Movie Title"",
//                                             ""releaseYear"": 2023
//                                         }},
//                                         {{
//                                             ""movieName"": ""Another Movie"",
//                                             ""releaseYear"": 2022
//                                         }}
//                                     ]
//                                 }}
//                             "
//                         }
//                     }
//                 }
//             }
//         };

//         // Serialize the request body to JSON
//         var jsonRequestBody = JsonConvert.SerializeObject(requestBody);
//         var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

//         // Send the HTTP POST request to Gemini API
//         var response = await client.PostAsync(url, content);

//         // Handle error if the response is not successful
//         if (!response.IsSuccessStatusCode)
//         {
//             var errorResponse = await response.Content.ReadAsStringAsync();
//             Console.WriteLine($"Error: {response.StatusCode}, {errorResponse}");
//             return StatusCode(500, "Error fetching movie suggestions.");
//         }

//         // Parse the response from Gemini API
//         var jsonResponse = await response.Content.ReadAsStringAsync();
//         Console.WriteLine(jsonResponse);
//         try
//         {
//             var responseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

//             // Assuming the response contains a 'candidates' field and 'content' with the movie suggestions
//             var movieSuggestionsJson = responseObject?.candidates?[0]?.content?.parts?[0]?.text?.ToString();

//             if (string.IsNullOrEmpty(movieSuggestionsJson))
//             {
//                 return StatusCode(500, "No movie suggestions returned.");
//             }

//             // Remove the ```json and ``` from the response text to extract the actual JSON
//             movieSuggestionsJson = movieSuggestionsJson?.Replace("```json\n", "").Replace("\n```", "");

//             // Deserialize the movie suggestions into a structured format
//             var movieSuggestions = JsonConvert.DeserializeObject<MovieSuggestionsResponse>(movieSuggestionsJson);

//             if (movieSuggestions == null || movieSuggestions.PopularMovies == null)
//             {
//                 return StatusCode(500, "No movie suggestions returned.");
//             }

//             // Return the structured movie suggestions
//             return Ok(new { suggestions = movieSuggestions.PopularMovies });
//         }
//         catch (JsonException ex)
//         {
//             Console.WriteLine($"Error parsing response: {ex.Message}");
//             return StatusCode(500, "Error parsing movie suggestions.");
//         }
//     }

//     // Model to capture the user's question
//     public class UserQuestionModel
//     {
//         public string Question { get; set; }
//     }

//     // Model to represent the movie suggestion structure
//     public class MovieSuggestion
//     {
//         public string MovieName { get; set; }
//         public int ReleaseYear { get; set; }
//     }

//     // Model for the response containing movie suggestions
//     public class MovieSuggestionsResponse
//     {
//         [JsonProperty("popularMovies")]
//         public List<MovieSuggestion> PopularMovies { get; set; }
//     }
// }

// using System;
// using System.Collections.Generic;
// using System.Net.Http;
// using System.Text;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Options;
// using Newtonsoft.Json;

// [ApiController]
// [Route("api/[controller]")]
// public class MovieSuggestionsController : ControllerBase
// {
//     private readonly IHttpClientFactory _httpClientFactory;
//     private readonly GeminiSettings _geminiSettings;

//     public MovieSuggestionsController(IHttpClientFactory httpClientFactory, IOptions<GeminiSettings> geminiSettings)
//     {
//         _geminiSettings = geminiSettings.Value;
//         _httpClientFactory = httpClientFactory;
//     }

//     // Endpoint for general movie suggestions
//     [HttpPost("suggest-movies")]
//     public async Task<IActionResult> GetMovieSuggestions([FromBody] UserQuestionModel userQuestion)
//     {
//         var client = _httpClientFactory.CreateClient();
//         var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={_geminiSettings.ApiKey}";

//         // Structure the request body for general movie suggestions
//         var requestBody = new
//         {
//             contents = new[]
//             {
//                 new
//                 {
//                     parts = new[]
//                     {
//                         new
//                         {
//                             text = $@"
//                                 Suggest popular movies based on the following question: {userQuestion.Question}. 
//                                 Return the results in a structured JSON format with the keys: 'movieName' and 'releaseYear' for each movie.
//                                 Example format:
//                                 {{
//                                     ""popularMovies"": [
//                                         {{
//                                             ""movieName"": ""Movie Title"",
//                                             ""releaseYear"": 2023
//                                         }},
//                                         {{
//                                             ""movieName"": ""Another Movie"",
//                                             ""releaseYear"": 2022
//                                         }}
//                                     ]
//                                 }}
//                             "
//                         }
//                     }
//                 }
//             }
//         };

//         // Serialize the request body to JSON
//         var jsonRequestBody = JsonConvert.SerializeObject(requestBody);
//         var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

//         // Send the HTTP POST request to Gemini API
//         var response = await client.PostAsync(url, content);

//         // Handle error if the response is not successful
//         if (!response.IsSuccessStatusCode)
//         {
//             var errorResponse = await response.Content.ReadAsStringAsync();
//             Console.WriteLine($"Error: {response.StatusCode}, {errorResponse}");
//             return StatusCode(500, "Error fetching movie suggestions.");
//         }

//         // Parse the response from Gemini API
//         var jsonResponse = await response.Content.ReadAsStringAsync();
//         Console.WriteLine(jsonResponse);
//         try
//         {
//             var responseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

//             // Assuming the response contains a 'candidates' field and 'content' with the movie suggestions
//             var movieSuggestionsJson = responseObject?.candidates?[0]?.content?.parts?[0]?.text?.ToString();

//             if (string.IsNullOrEmpty(movieSuggestionsJson))
//             {
//                 return StatusCode(500, "No movie suggestions returned.");
//             }

//             // Remove the ```json and ``` from the response text to extract the actual JSON
//             movieSuggestionsJson = movieSuggestionsJson?.Replace("```json\n", "").Replace("\n```", "");

//             // Deserialize the movie suggestions into a structured format
//             var movieSuggestions = JsonConvert.DeserializeObject<MovieSuggestionsResponse>(movieSuggestionsJson);

//             if (movieSuggestions == null || movieSuggestions.PopularMovies == null)
//             {
//                 return StatusCode(500, "No movie suggestions returned.");
//             }

//             // Return the structured movie suggestions
//             return Ok(new { suggestions = movieSuggestions.PopularMovies });
//         }
//         catch (JsonException ex)
//         {
//             Console.WriteLine($"Error parsing response: {ex.Message}");
//             return StatusCode(500, "Error parsing movie suggestions.");
//         }
//     }

//     [HttpPost("suggest-similar-movies")]
//     public async Task<IActionResult> GetSimilarMovies([FromBody] MovieNameModel movieNameModel)
//     {
//         var client = _httpClientFactory.CreateClient();
//         var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={_geminiSettings.ApiKey}";

//         // Structure the request body for suggesting similar movies based on the provided movie name
//         var requestBody = new
//         {
//             contents = new[]
//             {
//                 new
//                 {
//                     parts = new[]
//                     {
//                         new
//                         {
//                             text = $@"
//                                 Suggest similar movies to the following movie: {movieNameModel.MovieName}. 
//                                 Return the results in a structured JSON format with the keys: 'movieName' and 'releaseYear' for each similar movie.
//                                 Example format:
//                                 {{
//                                     ""similarMovies"": [
//                                         {{
//                                             ""movieName"": ""Movie Title"",
//                                             ""releaseYear"": 2023
//                                         }},
//                                         {{
//                                             ""movieName"": ""Another Movie"",
//                                             ""releaseYear"": 2022
//                                         }}
//                                     ]
//                                 }}
//                             "
//                         }
//                     }
//                 }
//             }
//         };

//         // Serialize the request body to JSON
//         var jsonRequestBody = JsonConvert.SerializeObject(requestBody);
//         var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

//         // Send the HTTP POST request to Gemini API
//         var response = await client.PostAsync(url, content);

//         // Handle error if the response is not successful
//         if (!response.IsSuccessStatusCode)
//         {
//             var errorResponse = await response.Content.ReadAsStringAsync();
//             Console.WriteLine($"Error: {response.StatusCode}, {errorResponse}");
//             return StatusCode(500, "Error fetching similar movie suggestions.");
//         }

//         // Parse the response from Gemini API
//         var jsonResponse = await response.Content.ReadAsStringAsync();
//         Console.WriteLine(jsonResponse);
//         try
//         {
//             var responseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

//             // Extract the raw movie suggestion text (which includes code block)
//             var similarMoviesText = responseObject?.candidates?[0]?.content?.parts?[0]?.text?.ToString();

//             if (string.IsNullOrEmpty(similarMoviesText))
//             {
//                 return StatusCode(500, "No similar movies returned.");
//             }

//             // Remove the ```json and ``` from the response text
//             similarMoviesText = similarMoviesText.Replace("```json\n", "").Replace("\n```", "");

//             // Deserialize the cleaned JSON text into a structured format
//             var similarMovies = JsonConvert.DeserializeObject<MovieSimilarSuggestionsResponse>(similarMoviesText);

//             if (similarMovies == null || similarMovies.SimilarMovies == null)
//             {
//                 return StatusCode(500, "No similar movies returned.");
//             }

//             // Return the structured similar movie suggestions
//             return Ok(new { suggestions = similarMovies.SimilarMovies });
//         }
//         catch (JsonException ex)
//         {
//             Console.WriteLine($"Error parsing response: {ex.Message}");
//             return StatusCode(500, "Error parsing similar movie suggestions.");
//         }
//     }

//     // Model to capture the movie name for similarity suggestion
//     public class MovieNameModel
//     {
//         public string MovieName { get; set; }
//     }

//     // Model to capture the user's question for general movie suggestions
//     public class UserQuestionModel
//     {
//         public string Question { get; set; }
//     }

//     // Model for the response containing movie suggestions
//     public class MovieSuggestion
//     {
//         public string MovieName { get; set; }
//         public int ReleaseYear { get; set; }
//     }

//     // Model for the response containing movie suggestions
//     public class MovieSuggestionsResponse
//     {
//         [JsonProperty("popularMovies")]
//         public List<MovieSuggestion> PopularMovies { get; set; }
//     }
//     public class MovieSimilarSuggestionsResponse
//     {
//         [JsonProperty("similarMovies")]
//         public List<MovieSuggestion> SimilarMovies { get; set; }
//     }
// }

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

[ApiController]
[Route("api/[controller]")]
public class MovieSuggestionsController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly GeminiSettings _geminiSettings;

    public MovieSuggestionsController(IHttpClientFactory httpClientFactory, IOptions<GeminiSettings> geminiSettings)
    {
        _geminiSettings = geminiSettings.Value;
        _httpClientFactory = httpClientFactory;
    }

    // Endpoint for general movie suggestions
    [HttpPost("suggest-movies")]
    public async Task<IActionResult> GetMovieSuggestions([FromBody] UserQuestionModel userQuestion)
    {
        var client = _httpClientFactory.CreateClient();
        var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={_geminiSettings.ApiKey}";

        // Structure the request body for general movie suggestions
        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new
                        {
                            text = $@"
                                Suggest popular movies based on the following question: {userQuestion.Question}. 
                                Ensure that the movie suggestions and their names are returned in the same language as the movie name itself.
                                Return the results in a structured JSON format with the keys: 'movieName' and 'releaseYear' for each movie.
                                Example format:
                                {{
                                    ""popularMovies"": [
                                        {{
                                            ""movieName"": ""Movie Title"",
                                            ""releaseYear"": 2023
                                        }},
                                        {{
                                            ""movieName"": ""Another Movie"",
                                            ""releaseYear"": 2022
                                        }}
                                    ]
                                }}
                            "
                        }
                    }
                }
            }
        };

        var jsonRequestBody = JsonConvert.SerializeObject(requestBody);
        var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

        var response = await client.PostAsync(url, content);

        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error: {response.StatusCode}, {errorResponse}");
            return StatusCode(500, "Error fetching movie suggestions.");
        }

        // Parse the response from Gemini API
        var jsonResponse = await response.Content.ReadAsStringAsync();
        Console.WriteLine(jsonResponse);
        try
        {
            var responseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

            // Assuming the response contains a 'candidates' field and 'content' with the movie suggestions
            var movieSuggestionsJson = responseObject?.candidates?[0]?.content?.parts?[0]?.text?.ToString();

            if (string.IsNullOrEmpty(movieSuggestionsJson))
            {
                return StatusCode(500, "No movie suggestions returned.");
            }

            // Remove the ```json and ``` from the response text to extract the actual JSON
            movieSuggestionsJson = movieSuggestionsJson?.Replace("```json\n", "").Replace("\n```", "");

            // Deserialize the movie suggestions into a structured format
            var movieSuggestions = JsonConvert.DeserializeObject<MovieSuggestionsResponse>(movieSuggestionsJson);

            if (movieSuggestions == null || movieSuggestions.PopularMovies == null)
            {
                return StatusCode(500, "No movie suggestions returned.");
            }

            // Return the structured movie suggestions
            return Ok(new { suggestions = movieSuggestions.PopularMovies });
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Error parsing response: {ex.Message}");
            return StatusCode(500, "Error parsing movie suggestions.");
        }
    }

    [HttpPost("suggest-similar-movies")]
    public async Task<IActionResult> GetSimilarMovies([FromBody] MovieNameModel movieNameModel)
    {
        var client = _httpClientFactory.CreateClient();
        var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={_geminiSettings.ApiKey}";

        // Structure the request body for suggesting similar movies based on the provided movie name
        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new
                        {
                            text = $@"
                                Suggest similar movies to the following movie: {movieNameModel.MovieName}. 
                                Ensure that the movie suggestions and their names are returned in the same language as the movie name itself.
                                Return the results in a structured JSON format with the keys: 'movieName' and 'releaseYear' for each similar movie.
                                Example format:
                                {{
                                    ""similarMovies"": [
                                        {{
                                            ""movieName"": ""Movie Title"",
                                            ""releaseYear"": 2023
                                        }},
                                        {{
                                            ""movieName"": ""Another Movie"",
                                            ""releaseYear"": 2022
                                        }}
                                    ]
                                }}
                            "
                        }
                    }
                }
            }
        };

        var jsonRequestBody = JsonConvert.SerializeObject(requestBody);
        var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

        var response = await client.PostAsync(url, content);

        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error: {response.StatusCode}, {errorResponse}");
            return StatusCode(500, "Error fetching similar movie suggestions.");
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();
        Console.WriteLine(jsonResponse);
        try
        {
            var responseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

            var similarMoviesText = responseObject?.candidates?[0]?.content?.parts?[0]?.text?.ToString();

            if (string.IsNullOrEmpty(similarMoviesText))
            {
                return StatusCode(500, "No similar movies returned.");
            }

            similarMoviesText = similarMoviesText.Replace("```json\n", "").Replace("\n```", "");

            var similarMovies = JsonConvert.DeserializeObject<MovieSimilarSuggestionsResponse>(similarMoviesText);

            if (similarMovies == null || similarMovies.SimilarMovies == null)
            {
                return StatusCode(500, "No similar movies returned.");
            }

            return Ok(new { suggestions = similarMovies.SimilarMovies });
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Error parsing response: {ex.Message}");
            return StatusCode(500, "Error parsing similar movie suggestions.");
        }
    }

    public class MovieNameModel
    {
        public string MovieName { get; set; }
    }

    public class UserQuestionModel
    {
        public string Question { get; set; }
    }

    public class MovieSuggestion
    {
        public string MovieName { get; set; }
        public int ReleaseYear { get; set; }
    }

    public class MovieSuggestionsResponse
    {
        [JsonProperty("popularMovies")]
        public List<MovieSuggestion> PopularMovies { get; set; }
    }
    public class MovieSimilarSuggestionsResponse
    {
        [JsonProperty("similarMovies")]
        public List<MovieSuggestion> SimilarMovies { get; set; }
    }
}
