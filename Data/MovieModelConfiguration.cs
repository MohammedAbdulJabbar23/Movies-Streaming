using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieApp.API.Models;

public class MovieModelConfiguration : IEntityTypeConfiguration<MovieModel>
{
    public void Configure(EntityTypeBuilder<MovieModel> builder)
    {
        // Primary Key
        builder.HasKey(m => m.Id);

        // Required Fields
        builder.Property(m => m.Name).IsRequired();
        builder.Property(m => m.GenreId).IsRequired();
        builder.Property(m => m.SubGenreId).IsRequired();

        // Foreign Key Relationships
        builder.HasOne(m => m.Genres)
               .WithMany()
               .HasForeignKey(m => m.GenreId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.SubGenres)
               .WithMany()
               .HasForeignKey(m => m.SubGenreId)
               .OnDelete(DeleteBehavior.Restrict);

        // Enum Fields
        builder.Property(m => m.Rating).HasConversion<string>();
        builder.Property(m => m.Audience).HasConversion<string>();

        // Additional Configuration
        builder.Property(m => m.DateCreated)
               .HasDefaultValueSql("GETDATE()"); // Use CURRENT_TIMESTAMP for SQLite.
    }
}
