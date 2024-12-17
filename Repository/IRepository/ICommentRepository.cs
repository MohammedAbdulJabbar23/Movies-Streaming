// ICommentRepository.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MovieApp.API.Models;

namespace MovieApp.API.Repositories
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetAllCommentsAsync();  // Get all comments
        Task<Comment> GetCommentByIdAsync(int id);  // Get comment by ID
        Task<IEnumerable<Comment>> GetCommentsByMovieIdAsync(Guid movieId);  // Get comments by Movie ID
        Task AddCommentAsync(Comment comment);  // Add a new comment
        Task UpdateCommentAsync(int id, int userId, string newText);  // Update an existing comment
        Task DeleteCommentAsync(int id, int userId);  // Delete a comment by ID
    }
}
