using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieApp.API.Models;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        // Primary Key
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Text)
               .IsRequired()
               .HasMaxLength(1000); // Optional: Set max length for Text if needed.

        // Foreign Key Relationship
        builder.HasOne(c => c.Movie)
               .WithMany(m => m.Comments)  // MovieModel has many comments
               .HasForeignKey(c => c.MovieId)
               .OnDelete(DeleteBehavior.Cascade);  // Delete all comments when movie is deleted
        // Foreign Key Relationship with User
        builder.HasOne(c => c.User)  // A Comment belongs to one User
               .WithMany()  // Assuming User has many Comments (or can be omitted if not necessary)
               .HasForeignKey(c => c.UserId)  // UserId is the foreign key
               .OnDelete(DeleteBehavior.Cascade);  // Restrict deletion of User if they have comments (optional)

        // Enum or DateTime Configurations (if needed)
        // For example, setting a default value for CreatedAt if necessary
        builder.Property(c => c.CreatedAt)
               .HasDefaultValueSql("GETDATE()"); // Use CURRENT_TIMESTAMP for SQLite or other DBs
    }
}
