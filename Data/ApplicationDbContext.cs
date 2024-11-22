using Microsoft.EntityFrameworkCore;
using MovieApp.API.Models;
using System;
using System.Linq;

namespace MovieApp.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<GenreModel> Genres { get; set; }
        public DbSet<SubGenreModel> SubGenres { get; set; }
        public DbSet<MovieModel> Movies { get; set; }
        public DbSet<UserModel> Users { get; set; }

        public DbSet<Comment> Comments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply snake_case to all table and column names
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // Convert table name to snake_case
                entity.SetTableName(ToSnakeCase(entity.GetTableName()));

                // Convert column names to snake_case
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(ToSnakeCase(property.GetColumnName()));
                }
            }
        }

        // Helper method to convert strings to snake_case
        private string ToSnakeCase(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // Add an underscore before each uppercase letter and convert to lowercase
            var result = System.Text.RegularExpressions.Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
            return result;
        }
    }
}
