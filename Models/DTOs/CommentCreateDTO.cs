// CommentCreateDTO.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace MovieApp.API.Models.DTOs
{
    public class CommentCreateDTO
    {

        [Required]
        public string Text { get; set; }

        [Required]
        public Guid MovieId { get; set; }
    }
}
