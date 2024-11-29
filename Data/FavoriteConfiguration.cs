using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieApp.API.Models;

public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
{
    public void Configure(EntityTypeBuilder<Favorite> builder)
    {
        // Primary Key
        builder.HasKey(c => c.Id);
        builder
            .HasOne(fav => fav.Movie)
            .WithMany(movie => movie.Favorites)
            .HasForeignKey(fav => fav.MovieId);

        // Enum or DateTime Configurations (if needed)
        // For example, setting a default value for CreatedAt if necessary
        builder.Property(c => c.CreatedAt)
               .HasDefaultValueSql("GETDATE()"); // Use CURRENT_TIMESTAMP for SQLite or other DBs
    }
}

