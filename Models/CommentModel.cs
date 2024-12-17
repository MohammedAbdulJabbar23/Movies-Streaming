using System;
using Microsoft.EntityFrameworkCore.Storage;
using MovieApp.API.Models;

public sealed class Comment 
{
    public int Id { get; set; }
    public Guid MovieId { get; set; }
    public MovieModel Movie { get; set; }
    public string UserName { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
