// CommentDTO.cs
using System;

namespace MovieApp.API.Models.DTOs
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid MovieId { get; set; }
        public UserDTO User { get; set; }
    }

}
