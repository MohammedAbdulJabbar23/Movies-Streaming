using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MovieApp.API.Models;

namespace MovieApp.API.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider, ApplicationDbContext context)
        {
            context.Database.Migrate();

            // Seed genres
                        var genres = new List<GenreModel>
            {
                new GenreModel { Name = "Action" },
                new GenreModel { Name = "Drama" },
                new GenreModel { Name = "Comedy" },
                new GenreModel { Name = "Horror" },
                new GenreModel { Name = "Sci-Fi" },
                new GenreModel { Name = "Fantasy" },
                new GenreModel { Name = "Romance" },
                new GenreModel { Name = "Thriller" },
                new GenreModel { Name = "Adventure" },
                new GenreModel { Name = "Animation" }
            };

            context.Genres.AddRange(genres);
            context.SaveChanges();

            // Seed subgenres
            var subGenres = new List<SubGenreModel>
            {
                new SubGenreModel { Name = "Superhero", GenreId = genres.First(g => g.Name == "Action").Id },
                new SubGenreModel { Name = "Spy", GenreId = genres.First(g => g.Name == "Action").Id },
                new SubGenreModel { Name = "Romantic", GenreId = genres.First(g => g.Name == "Drama").Id },
                new SubGenreModel { Name = "Historical", GenreId = genres.First(g => g.Name == "Drama").Id },
                new SubGenreModel { Name = "Slapstick", GenreId = genres.First(g => g.Name == "Comedy").Id },
                new SubGenreModel { Name = "Satire", GenreId = genres.First(g => g.Name == "Comedy").Id },
                new SubGenreModel { Name = "Psychological", GenreId = genres.First(g => g.Name == "Horror").Id },
                new SubGenreModel { Name = "Zombie", GenreId = genres.First(g => g.Name == "Horror").Id },
                new SubGenreModel { Name = "Space Opera", GenreId = genres.First(g => g.Name == "Sci-Fi").Id },
                new SubGenreModel { Name = "Dystopian", GenreId = genres.First(g => g.Name == "Sci-Fi").Id },
                new SubGenreModel { Name = "Sword and Sorcery", GenreId = genres.First(g => g.Name == "Fantasy").Id },
                new SubGenreModel { Name = "High Fantasy", GenreId = genres.First(g => g.Name == "Fantasy").Id },
                new SubGenreModel { Name = "Romantic Comedy", GenreId = genres.First(g => g.Name == "Romance").Id },
                new SubGenreModel { Name = "Teen Romance", GenreId = genres.First(g => g.Name == "Romance").Id },
                new SubGenreModel { Name = "Psychological Thriller", GenreId = genres.First(g => g.Name == "Thriller").Id },
                new SubGenreModel { Name = "Crime Thriller", GenreId = genres.First(g => g.Name == "Thriller").Id },
                new SubGenreModel { Name = "Action Adventure", GenreId = genres.First(g => g.Name == "Adventure").Id },
                new SubGenreModel { Name = "Survival", GenreId = genres.First(g => g.Name == "Adventure").Id },
                new SubGenreModel { Name = "Animated", GenreId = genres.First(g => g.Name == "Animation").Id }
            };

            context.SubGenres.AddRange(subGenres);
            context.SaveChanges();

            // Seed movies

            var movies = new List<MovieModel>
            {
                new MovieModel 
                { 
                    Name = "The Dark Knight", 
                    DateCreated = DateTime.UtcNow, 
                    Picture = "https://cdn-images.dzcdn.net/images/cover/db24b10af9499e1e5098c2a2328f8ef2/0x1900-000000-80-0-0.jpg",  // You can add an actual image byte array here 
                    Rating = MovieModel.RatingType.BlockBuster, 
                    GenreId = genres.First(g => g.Name == "Action").Id,
                    SubGenreId = subGenres.First(s => s.Name == "Superhero").Id,
                    Audience = MovieModel.AudienceType.PG
                },
                new MovieModel 
                { 
                    Name = "The Godfather", 
                    DateCreated = DateTime.UtcNow, 
                    Picture = "https://m.media-amazon.com/images/M/MV5BYTJkNGQyZDgtZDQ0NC00MDM0LWEzZWQtYzUzZDEwMDljZWNjXkEyXkFqcGc@._V1_.jpg",
                    Rating = MovieModel.RatingType.BlockBuster, 
                    GenreId = genres.First(g => g.Name == "Drama").Id,
                    SubGenreId = subGenres.First(s => s.Name == "Romantic").Id,
                    Audience = MovieModel.AudienceType.EIGHTEEN
                },
                new MovieModel 
                { 
                    Name = "The Hangover", 
                    DateCreated = DateTime.UtcNow, 
                    Picture = "https://images.fineartamerica.com/images/artworkimages/medium/3/the-hangover-movie-poster-essencejac-kowski.jpg",
                    Rating = MovieModel.RatingType.Average, 
                    GenreId = genres.First(g => g.Name == "Comedy").Id,
                    SubGenreId = subGenres.First(s => s.Name == "Slapstick").Id,
                    Audience = MovieModel.AudienceType.PG
                },
                new MovieModel 
                { 
                    Name = "The Conjuring", 
                    DateCreated = DateTime.UtcNow, 
                    Picture = "https://cdns-images.dzcdn.net/images/cover/75bca6e55ffd3271c3572c73a53d13ae/0x1900-000000-80-0-0.jpg",
                    Rating = MovieModel.RatingType.Poor, 
                    GenreId = genres.First(g => g.Name == "Horror").Id,
                    SubGenreId = subGenres.First(s => s.Name == "Psychological").Id,
                    Audience = MovieModel.AudienceType.FIFTEEN
                },
                                new MovieModel 
                { 
                    Name = "Mission Impossible: Fallout", 
                    DateCreated = DateTime.UtcNow, 
                    Picture = "https://upload.wikimedia.org/wikipedia/en/e/ed/Mission-_Impossible_%E2%80%93_Dead_Reckoning_Part_One_poster.jpg", 
                    Rating = MovieModel.RatingType.BlockBuster, 
                    GenreId = genres.First(g => g.Name == "Action").Id,
                    SubGenreId = subGenres.First(s => s.Name == "Spy").Id,
                    Audience = MovieModel.AudienceType.TWELVEA
                },
                                new MovieModel 
                { 
                    Name = "28 Days Later", 
                    DateCreated = DateTime.UtcNow, 
                    Picture = "https://m.media-amazon.com/images/M/MV5BM2I4NTI0ZGQtNGQ2ZC00ODIxLWI2N2QtMDBkNzI1NDhjYjE5XkEyXkFqcGc@._V1_.jpg",
                    Rating = MovieModel.RatingType.BlockBuster, 
                    GenreId = genres.First(g => g.Name == "Horror").Id,
                    SubGenreId = subGenres.First(s => s.Name == "Zombie").Id,
                    Audience = MovieModel.AudienceType.EIGHTEEN
                },

                // Sci-Fi
                new MovieModel 
                { 
                    Name = "Interstellar", 
                    DateCreated = DateTime.UtcNow, 
                    Picture = "https://i1.sndcdn.com/artworks-000097528991-8bytby-t500x500.jpg",
                    Rating = MovieModel.RatingType.BlockBuster, 
                    GenreId = genres.First(g => g.Name == "Sci-Fi").Id,
                    SubGenreId = subGenres.First(s => s.Name == "Space Opera").Id,
                    Audience = MovieModel.AudienceType.TWELVEA
                },

                // Fantasy
                new MovieModel 
                { 
                    Name = "The Lord of the Rings: The Fellowship of the Ring", 
                    DateCreated = DateTime.UtcNow, 
                    Picture = "https://resizing.flixster.com/-XZAfHZM39UwaGJIFWKAE8fS0ak=/v3/t/assets/p28828_p_v8_ao.jpg",
                    Rating = MovieModel.RatingType.BlockBuster, 
                    GenreId = genres.First(g => g.Name == "Fantasy").Id,
                    SubGenreId = subGenres.First(s => s.Name == "High Fantasy").Id,
                    Audience = MovieModel.AudienceType.TWELVEA
                },

                // Adventure
                new MovieModel 
                { 
                    Name = "Indiana Jones: Raiders of the Lost Ark", 
                    DateCreated = DateTime.UtcNow, 
                    Picture = "https://www.vintagemovieposters.co.uk/wp-content/uploads/2023/08/IMG_4796-scaled.jpeg",
                    Rating = MovieModel.RatingType.BlockBuster, 
                    GenreId = genres.First(g => g.Name == "Adventure").Id,
                    SubGenreId = subGenres.First(s => s.Name == "Action Adventure").Id,
                    Audience = MovieModel.AudienceType.TWELVEA
                },

            };

            context.Movies.AddRange(movies);
            context.SaveChanges();
        }
    }
}
