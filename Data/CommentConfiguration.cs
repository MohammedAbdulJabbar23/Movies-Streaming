using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieApp.API.Models;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        // Primary Key
        builder.HasKey(c => c.Id);

        // Required Fields
        builder.Property(c => c.UserName)
               .IsRequired()
               .HasMaxLength(100);  // Optional: Set max length for UserName if needed.
        builder.Property(c => c.Text)
               .IsRequired()
               .HasMaxLength(1000); // Optional: Set max length for Text if needed.

        // Foreign Key Relationship
        builder.HasOne(c => c.Movie)
               .WithMany(m => m.Comments)  // MovieModel has many comments
               .HasForeignKey(c => c.MovieId)
               .OnDelete(DeleteBehavior.Cascade);  // Delete all comments when movie is deleted

        // Enum or DateTime Configurations (if needed)
        // For example, setting a default value for CreatedAt if necessary
        builder.Property(c => c.CreatedAt)
               .HasDefaultValueSql("GETDATE()"); // Use CURRENT_TIMESTAMP for SQLite or other DBs
    }
}
