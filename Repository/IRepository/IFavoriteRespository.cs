using System;
using System.Collections.Generic;
using MovieApp.API.Models;

public interface IFavoriteRepository
{
    bool AddToFavorites(int userId, Guid movieId);
    IEnumerable<MovieModel> GetUserFavorites(int userId);
    bool UpdateFavorite(int userId, Guid movieId, string note);
    bool RemoveFromFavorites(int userId, Guid movieId);
    bool IsMovieFavoritedByUser(int userId, Guid movieId);
    bool UserExists(int userId); // New method
}

