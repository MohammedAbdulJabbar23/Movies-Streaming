// CommentUpdateDTO.cs
namespace MovieApp.API.Models.DTOs
{
    public class CommentUpdateDTO
    {
        public int Id { get; set; }  // The ID of the comment to update
        public string Text { get; set; }  // The updated text of the comment
        public string Username { get; set; }  // Optional: If you want to allow the username to be updated
    }
}
