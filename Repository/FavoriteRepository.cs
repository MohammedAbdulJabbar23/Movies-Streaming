using System;
using System.Collections.Generic;
using System.Linq;
using MovieApp.API.Data;
using MovieApp.API.Models;

public class FavoriteRepository : IFavoriteRepository
{
    private readonly ApplicationDbContext _context;

    public FavoriteRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public bool AddToFavorites(int userId, Guid movieId)
    {
        try
        {
            var favorite = new Favorite
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                MovieId = movieId,
                Note = string.Empty,
                CreatedAt = DateTime.UtcNow
            };

            _context.Favorites.Add(favorite);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public IEnumerable<MovieModel> GetUserFavorites(int userId)
    {
        return _context.Favorites
            .Where(fav => fav.UserId == userId)
            .Select(fav => fav.Movie)
            .ToList();
    }

    public bool UpdateFavorite(int userId, Guid movieId, string note)
    {
        try
        {
            var favorite = _context.Favorites
                .FirstOrDefault(fav => fav.UserId == userId && fav.MovieId == movieId);

            if (favorite == null)
                return false;

            favorite.Note = note;
            _context.Favorites.Update(favorite);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool RemoveFromFavorites(int userId, Guid movieId)
    {
        try
        {
            var favorite = _context.Favorites
                .FirstOrDefault(fav => fav.UserId == userId && fav.MovieId == movieId);

            if (favorite == null)
                return false;

            _context.Favorites.Remove(favorite);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool IsMovieFavoritedByUser(int userId, Guid movieId)
    {
        return _context.Favorites.Any(fav => fav.UserId == userId && fav.MovieId == movieId);
    }

    public bool UserExists(int userId)
    {
        return _context.Users.Any(user => user.UserId == userId);
    }
}

