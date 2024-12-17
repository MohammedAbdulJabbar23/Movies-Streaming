using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieApp.API.Models
{
    public class UserModel
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string UserName { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public string Role { get; set; }

        [NotMapped]
        public string Token { get; set; }

        public ICollection<Comment> Comments { get; set; }

        // Adding social media links
        public string FacebookLink { get; set; }
        public string TwitterLink { get; set; }
        public string LinkedInLink { get; set; }
        public string InstagramLink { get; set; }

        // Adding an image link for the user's profile picture
        public string ProfileImageUrl { get; set; }
    }
}
