using System;
using MovieApp.API.Models;

public class Favorite
{
    public Guid Id { get; set; }
    public int UserId { get; set; }
    public Guid MovieId { get; set; }
    public string Note { get; set; }
    public DateTime CreatedAt { get; set; }

    public MovieModel Movie { get; set; } // Navigation property
}

