using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using static MovieApp.API.Models.MovieModel;

namespace MovieApp.API.Models.DTOs
{
    public class MoviesCreateDTO
    {
        [Required]
        public string Name { get; set; }
        public string Picture { get; set; }
        public decimal Rating { get; set; }
        public DateTime DateCreated { get; set; }
        public IFormFile VideoFile { get; set; }
        public AudienceType Audience { get; set; }
        [Required]
        public Guid GenreId { get; set; }
        [Required]
        public Guid SubGenreId { get; set; }
    }
}
