using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieApp.API.Repository.IRepository;

[ApiController]
[Route("api/[controller]")]
public class FavoritesController : ControllerBase
{
    private readonly IFavoriteRepository _favoriteRepository;

    public FavoritesController(IFavoriteRepository favoriteRepository)
    {
        _favoriteRepository = favoriteRepository;
    }
    [HttpGet("user/{userId}")]
    public IActionResult GetFavoritesByUserId(int userId)
    {
        // Check if the user exists
        if (!_favoriteRepository.UserExists(userId))
            return NotFound("User not found");

        // Retrieve the list of favorite movies for the given user ID
        var favorites = _favoriteRepository.GetUserFavorites(userId);

        return Ok(favorites);
    }
    // Add a movie to favorites
    [HttpPost("{movieId}")]
    [Authorize]
    public IActionResult AddToFavorites(Guid movieId)
    {
        var userIdClaim = User.FindFirst("userId");
        Console.WriteLine(userIdClaim.Value);
        if (userIdClaim == null)
            return Unauthorized("User ID not found in token");

        // Parse userId from token as int
        if (!int.TryParse(userIdClaim.Value, out int userId))
            return Unauthorized("Invalid user ID in token");

        // Check if user exists
        if (!_favoriteRepository.UserExists(userId))
            return NotFound("User not found");

        // Check if movie is already in favorites
        if (_favoriteRepository.IsMovieFavoritedByUser(userId, movieId))
            return BadRequest("Movie already in favorites");

        // Add movie to favorites
        if (_favoriteRepository.AddToFavorites(userId, movieId))
            return Ok("Movie added to favorites");

        return StatusCode(500, "Failed to add movie to favorites");
    }

    // Get all favorite movies for the user
    [HttpGet]
    public IActionResult GetFavorites()
    {
        var userIdClaim = User.FindFirst("userId");
        if (userIdClaim == null)
            return Unauthorized("User ID not found in token");

        // Parse userId from token as int
        if (!int.TryParse(userIdClaim.Value, out int userId))
            return Unauthorized("Invalid user ID in token");

        // Check if user exists
        if (!_favoriteRepository.UserExists(userId))
            return NotFound("User not found");

        // Get the list of favorite movies
        var favorites = _favoriteRepository.GetUserFavorites(userId);
        return Ok(favorites);
    }

    // Update a favorite movie (e.g., add a note)
    [HttpPut("{movieId}")]
    public IActionResult UpdateFavorite(Guid movieId, [FromBody] UpdateFavoriteRequest request)
    {
        var userIdClaim = User.FindFirst("userId");
        if (userIdClaim == null)
            return Unauthorized("User ID not found in token");

        // Parse userId from token as int
        if (!int.TryParse(userIdClaim.Value, out int userId))
            return Unauthorized("Invalid user ID in token");

        // Check if user exists
        if (!_favoriteRepository.UserExists(userId))
            return NotFound("User not found");

        // Check if movie is in user's favorites
        if (!_favoriteRepository.IsMovieFavoritedByUser(userId, movieId))
            return NotFound("Favorite not found");

        // Update favorite movie
        if (_favoriteRepository.UpdateFavorite(userId, movieId, request.Note))
            return Ok("Favorite updated");

        return StatusCode(500, "Failed to update favorite");
    }

    // Remove a movie from favorites
    [HttpDelete("{movieId}")]
    public IActionResult RemoveFromFavorites(Guid movieId)
    {
        var userIdClaim = User.FindFirst("userId");
        if (userIdClaim == null)
            return Unauthorized("User ID not found in token");

        // Parse userId from token as int
        if (!int.TryParse(userIdClaim.Value, out int userId))
            return Unauthorized("Invalid user ID in token");

        // Check if user exists
        if (!_favoriteRepository.UserExists(userId))
            return NotFound("User not found");

        // Check if movie is in user's favorites
        if (!_favoriteRepository.IsMovieFavoritedByUser(userId, movieId))
            return NotFound("Favorite not found");

        // Remove movie from favorites
        if (_favoriteRepository.RemoveFromFavorites(userId, movieId))
            return Ok("Favorite removed");

        return StatusCode(500, "Failed to remove favorite");
    }
}

public class UpdateFavoriteRequest
{
    public string Note { get; set; }
}