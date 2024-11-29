using BCrypt.Net;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MovieApp.API.ApplicationOptions;
using MovieApp.API.Data;
using MovieApp.API.Models;
using MovieApp.API.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace MovieApp.API.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly AppSettings _appSettings;

        public UserRepository(ApplicationDbContext dbContext, IOptions<AppSettings> appSettings)
        {
            _dbContext = dbContext;
            _appSettings = appSettings.Value;
        }

        public UserModel Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = _dbContext.Users.SingleOrDefault(x => x.UserName == username);

            // Check if username exists
            if (user == null || string.IsNullOrEmpty(user.PasswordHash)) // Check if PasswordHash is null or empty
                return null;

            // Check if password is correct (using the hashed password)
            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)) // Changed from user.Password to user.PasswordHash
                return null;

            // Generate JWT Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("meowmeowwewewwdawaddasdsnandajsdjadwa");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
            new Claim(ClaimTypes.Name, user.UserId.ToString()),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("userId", user.UserId.ToString()), // Ensure the UserId is valid
        }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            return user;
        }


        public UserModel Create(UserModel user, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (_dbContext.Users.Any(x => x.UserName == user.UserName))
                throw new AppException($"Username \"{user.UserName}\" is already taken");

            // Hash the password using BCrypt
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            user.PasswordHash = passwordHash; // Changed from user.Password to user.PasswordHash
            user.Role = "Customer"; // Default role

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return user;
        }

        public void Delete(int id)
        {
            var user = _dbContext.Users.Find(id);
            if (user != null)
            {
                _dbContext.Users.Remove(user);
                _dbContext.SaveChanges();
            }
        }

        public IEnumerable<UserModel> GetAll()
        {
            return _dbContext.Users;
        }

        public UserModel GetUserById(int id)
        {
            return _dbContext.Users.Find(id);
        }

        public void Update(UserModel user, string password = null)
        {
            var userParam = _dbContext.Users.Find(user.UserId);

            if (userParam == null)
                throw new AppException("User not found");

            if (userParam.UserName != user.UserName)
            {
                // Check if the new username is already taken
                if (_dbContext.Users.Any(x => x.UserName == user.UserName))
                    throw new AppException($"Username \"{user.UserName}\" is already taken");
            }

            // Update user properties
            userParam.FirstName = user.FirstName;
            userParam.LastName = user.LastName;
            userParam.UserName = user.UserName;
            userParam.Email = user.Email;

            // Update password if provided
            if (!string.IsNullOrWhiteSpace(password))
            {
                userParam.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password); // Changed to PasswordHash
            }

            _dbContext.Users.Update(userParam);
            _dbContext.SaveChanges();
        }
    }
}
