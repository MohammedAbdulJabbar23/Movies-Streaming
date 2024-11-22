// CommentController.cs
using Microsoft.AspNetCore.Mvc;
using MovieApp.API.Models;
using MovieApp.API.Models.DTOs;
using MovieApp.API.Repositories;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        // POST: api/Comment
        [HttpPost]
        public async Task<ActionResult<CommentDTO>> CreateComment([FromBody] CommentCreateDTO commentCreateDTO)
        {
            if (commentCreateDTO == null)
            {
                return BadRequest();
            }

            var comment = _mapper.Map<Comment>(commentCreateDTO);
            await _commentRepository.AddCommentAsync(comment);
            
            var commentDTO = _mapper.Map<CommentDTO>(comment);
            return CreatedAtAction(nameof(GetCommentById), new { id = comment.Id }, commentDTO);
        }

        // PUT: api/Comment/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] CommentUpdateDTO commentUpdateDTO)
        {
            if (id != commentUpdateDTO.Id)
            {
                return BadRequest();
            }

            var existingComment = await _commentRepository.GetCommentByIdAsync(id);
            if (existingComment == null)
            {
                return NotFound();
            }

            var comment = _mapper.Map<Comment>(commentUpdateDTO);
            await _commentRepository.UpdateCommentAsync(comment);

            return NoContent();
        }

        // DELETE: api/Comment/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var existingComment = await _commentRepository.GetCommentByIdAsync(id);
            if (existingComment == null)
            {
                return NotFound();
            }

            await _commentRepository.DeleteCommentAsync(id);
            return NoContent();
        }
    }
}

