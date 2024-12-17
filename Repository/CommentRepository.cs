using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieApp.API.Data;
using MovieApp.API.Models;

namespace MovieApp.API.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all comments with User data
        public async Task<IEnumerable<Comment>> GetAllCommentsAsync()
        {
            return await _context.Comments
                .Include(c => c.User) // Include User data
                .ToListAsync();
        }

        // Get comment by ID with User data
        public async Task<Comment> GetCommentByIdAsync(int id)
        {
            return await _context.Comments
                .Include(c => c.User) // Include User data
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Comment>> GetCommentsByMovieIdAsync(Guid movieId)
        {
            return await _context.Comments
                .Where(c => c.MovieId == movieId)
                .Include(c => c.User) // Include User data
                .ToListAsync();
        }

        // Add a new comment
        public async Task AddCommentAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        // Update an existing comment
        public async Task UpdateCommentAsync(int id, int userId, string newText)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                throw new Exception("Comment not found.");
            }

            if (comment.UserId != userId)  // Check if the current user is the owner
            {
                throw new UnauthorizedAccessException("You can only edit your own comments.");
            }

            comment.Text = newText;
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
        }

        // Delete a comment
        public async Task DeleteCommentAsync(int id, int userId)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                throw new Exception("Comment not found.");
            }

            if (comment.UserId != userId)  // Check if the current user is the owner
            {
                throw new UnauthorizedAccessException("You can only delete your own comments.");
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }
    }
}

