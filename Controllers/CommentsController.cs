using Microsoft.AspNetCore.Mvc;
using MovieApp.API.Models;
using MovieApp.API.Models.DTOs;
using MovieApp.API.Repositories;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace MovieApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public CommentController(ICommentRepository commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        // GET: api/Comment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentDTO>>> GetAllComments()
        {
            var comments = await _commentRepository.GetAllCommentsAsync();
            return Ok(_mapper.Map<IEnumerable<CommentDTO>>(comments));
        }

        // GET: api/Comment/{movieId}
        [HttpGet("movie/{movieId}")]
        public async Task<ActionResult<IEnumerable<CommentDTO>>> GetCommentsByMovieId(Guid movieId)
        {
            var comments = await _commentRepository.GetCommentsByMovieIdAsync(movieId);
            return Ok(_mapper.Map<IEnumerable<CommentDTO>>(comments));
        }

        [HttpPost]
        [Authorize(Policy = "Auth")]
        public async Task<ActionResult<CommentDTO>> CreateComment([FromBody] CommentCreateDTO commentCreateDTO)
        {
            if (commentCreateDTO == null)
            {
                return BadRequest();
            }

            var userIdString = User.FindFirstValue("userId");  // Get the user ID from the JWT token
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                return Unauthorized();  // Return 401 if the user ID is invalid or not found
            }

            var comment = _mapper.Map<Comment>(commentCreateDTO);
            comment.UserId = userId;  // Set the UserId as an integer

            await _commentRepository.AddCommentAsync(comment);

            var commentDTO = _mapper.Map<CommentDTO>(comment);
            return CreatedAtAction(nameof(GetCommentById), new { id = comment.Id }, commentDTO);
        }

        // PUT: api/Comment/{id}
        [HttpPut("{id}")]
        [Authorize(Policy = "Auth")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] CommentUpdateDTO commentUpdateDTO)
        {
            if (id != commentUpdateDTO.Id)
            {
                return BadRequest();
            }

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);  // Get the UserId from the token
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                return Unauthorized();  // Return 401 if the user ID is invalid or not found
            }

            try
            {
                await _commentRepository.UpdateCommentAsync(id, userId, commentUpdateDTO.Text);
                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();  // Return 403 Forbidden if the user is not the owner
            }
            catch (Exception)
            {
                return NotFound();  // If the comment doesn't exist
            }
        }

        // DELETE: api/Comment/{id}
        [HttpDelete("{id}")]

        [Authorize(Policy = "Auth")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);  // Get the UserId from the token
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                return Unauthorized();  // Return 401 if the user ID is invalid or not found
            }

            try
            {
                await _commentRepository.DeleteCommentAsync(id, userId);
                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();  // Return 403 Forbidden if the user is not the owner
            }
            catch (Exception)
            {
                return NotFound();  // If the comment doesn't exist
            }
        }

        // GET: api/Comment/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CommentDTO>> GetCommentById(int id)
        {
            var comment = await _commentRepository.GetCommentByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<CommentDTO>(comment));
        }
    }
}

