using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static MovieApp.API.Models.MovieModel;

namespace MovieApp.API.Models.DTOs
{
    public class MoviesDTO
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string DateCreated { get; set; }
        public string VideoFilePath { get; set; }
        public string Picture { get; set; }
        public decimal Rating { get; set; }
        public AudienceType Audience { get; set; }
        [Required]
        public Guid GenreId { get; set; }
        public virtual GenreDTO Genres { get; set; }
        [Required]
        public Guid SubGenreId { get; set; }
        public virtual SubGenreDTO SubGenres { get; set; }

    }
}
