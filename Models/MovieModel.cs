using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.API.Models
{
    public class MovieModel
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string DateCreated { get; set; }
        public string Picture { get; set; }
        public decimal Rating { get; set; } 
        public string VideoFilePath { get; set; }
        [Required]
        public Guid GenreId { get; set; }
        [ForeignKey("GenreId")]
        public virtual GenreModel Genres { get; set; }
        [Required]
        public Guid SubGenreId { get; set; }
        [ForeignKey("SubGenreId")]
        public virtual SubGenreModel SubGenres { get; set; }
        public enum AudienceType { U, PG, TWELVEA, FIFTEEN, EIGHTEEN }
        public AudienceType Audience { get; set; }

    }
}
