using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MovieApp.API.Data;
using Newtonsoft.Json;
[ApiController]
[Route("api/[controller]")]
public class MovieSuggestionsController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly GeminiSettings _geminiSettings;
    private readonly ApplicationDbContext _dbContext;

    public MovieSuggestionsController(
        IHttpClientFactory httpClientFactory,
        IOptions<GeminiSettings> geminiSettings,
        ApplicationDbContext dbContext)
    {
        _httpClientFactory = httpClientFactory;
        _geminiSettings = geminiSettings.Value;
        _dbContext = dbContext;
    }

    [HttpPost("suggest-movies")]
    public async Task<IActionResult> GetMovieSuggestions([FromBody] UserQuestionModel userQuestion)
    {
        // Retrieve movies from the database
        var movies = await _dbContext.Movies.ToListAsync();

        // Check if no movies are found
        if (movies == null || movies.Count == 0)
        {
            return NotFound("No movies found in the database.");
        }

        // Structure the input to send to Gemini API
        var movieData = movies.Select(m => new
        {
            MovieId = m.Id, // Guid ID
            MovieName = m.Name,
            ReleaseYear = m.DateCreated,
            Description = m.Description
        }).ToList();

        var client = _httpClientFactory.CreateClient();
        var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={_geminiSettings.ApiKey}";

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
                                Based on the following question: {userQuestion.Question}, 
                                suggest popular movies from this list of movies (including ID, name, and release year): 
                                {string.Join(", ", movieData.Select(m => $"{m.MovieName} ({m.ReleaseYear}) - ID: {m.MovieId}"))}. 
                                Ensure the suggestions are relevant to the question and returned in a structured JSON format.
                                Example format:
                                {{
                                    ""popularMovies"": [
                                        {{
                                            ""movieId"": ""ensure the Guid is taken from the sample data given"",  // Sample Guid ID
                                            ""movieName"": ""Movie Title"",
                                            ""releaseYear"": 2023,
                                            ""description"": ""Movie Description""
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

        var jsonResponse = await response.Content.ReadAsStringAsync();
        try
        {
            var responseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
            var movieSuggestionsJson = responseObject?.candidates?[0]?.content?.parts?[0]?.text?.ToString();

            if (string.IsNullOrEmpty(movieSuggestionsJson))
            {
                return StatusCode(500, "No movie suggestions returned.");
            }

            movieSuggestionsJson = movieSuggestionsJson.Replace("```json\n", "").Replace("\n```", "");
            var movieSuggestions = JsonConvert.DeserializeObject<MovieSuggestionsResponse>(movieSuggestionsJson);

            if (movieSuggestions?.PopularMovies == null)
            {
                return StatusCode(500, "No movie suggestions returned.");
            }

            // Return the movie suggestions with the IDs and other details
            return Ok(new { suggestions = movieSuggestions.PopularMovies });
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Error parsing response: {ex.Message}");
            return StatusCode(500, "Error parsing movie suggestions.");
        }
    }

    public class UserQuestionModel
    {
        public string Question { get; set; }
    }

    public class MovieSuggestion
    {
        public Guid MovieId { get; set; }  // Changed to Guid
        public string MovieName { get; set; }
        public int ReleaseYear { get; set; }
        public string Description { get; set; }
    }

    public class MovieSuggestionsResponse
    {
        [JsonProperty("popularMovies")]
        public List<MovieSuggestion> PopularMovies { get; set; }
    }

    public class Movie
    {
        public Guid Id { get; set; }  // Changed to Guid
        public string Name { get; set; }
        public int ReleaseYear { get; set; }
        public string Description { get; set; }
    }
}

