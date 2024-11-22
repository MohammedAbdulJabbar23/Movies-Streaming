// CommentRepository.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieApp.API.Data;
using MovieApp.API.Models;
using MovieApp.API.Repositories;

namespace MovieApp.API.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all comments
        public async Task<IEnumerable<Comment>> GetAllCommentsAsync()
        {
            return await _context.Comments.ToListAsync();
        }

        // Get comment by ID
        public async Task<Comment> GetCommentByIdAsync(int id)
        {
            return await _context.Comments.FindAsync(id);
        }

        // Get comments by Movie ID
        public async Task<IEnumerable<Comment>> GetCommentsByMovieIdAsync(Guid movieId)
        {
            return await _context.Comments.Where(c => c.MovieId == movieId).ToListAsync();
        }

        // Add a new comment
        public async Task AddCommentAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        // Update an existing comment
        public async Task UpdateCommentAsync(Comment comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
        }

        // Delete a comment
        public async Task DeleteCommentAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }
    }
}
