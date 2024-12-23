using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MovieApp.API.Models;
using MovieApp.API.Models.DTOs;
using MovieApp.API.Repository.IRepository;

namespace MovieApp.API.Controllers
{

    [Route("api/v{version:apiVersion}/Movies")]
    //[Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class MoviesController : ControllerBase
    {
        private readonly string _movieStoragePath;
        private readonly IMovieRepository _movieRepo;
        private readonly IMapper _mapper;
        public MoviesController(IMovieRepository movieRepo, IMapper mapper)
        {
            _movieRepo = movieRepo;
            _mapper = mapper;
            _movieStoragePath = Directory.GetCurrentDirectory() + "/videos";
        }

        /// <summary>
        /// Gets a list of all the Movies in the database
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<MoviesDTO>))]
        public IActionResult GetMovies()
        {
            var objList = _movieRepo.GetMovie();

            var objDto = new List<MoviesDTO>();

            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<MoviesDTO>(obj));
            }

            return Ok(objDto);
        }
        /// <summary>
        /// Search movies by keyword
        /// </summary>
        /// <param name="keyword">The search keyword</param>
        /// <returns></returns>
        [HttpGet("search")]
        [ProducesResponseType(200, Type = typeof(List<MoviesDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult SearchMovies([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return BadRequest("Keyword cannot be empty.");
            }

            var filteredMovies = _movieRepo.SearchMovies(keyword);
            if (!filteredMovies.Any())
            {
                return NotFound("No movies match the search criteria.");
            }

            var movieDtos = filteredMovies.Select(movie => _mapper.Map<MoviesDTO>(movie)).ToList();

            return Ok(movieDtos);
        }


        /// <summary>
        /// Gets individual Movie from database
        /// </summary>
        /// <param name="movieId">The id of the Movie</param>
        /// <returns></returns>
        [HttpGet("{movieId:Guid}", Name = "GetMovieById")]
        [ProducesResponseType(200, Type = typeof(List<MoviesDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        // [Authorize(Roles = "Admin")]
        public IActionResult GetMovieById(Guid movieId)
        {
            var obj = _movieRepo.GetMovie(movieId);

            if (obj is null)
            {
                return NotFound();
            }

            var objDto = _mapper.Map<MoviesDTO>(obj);

            return Ok(objDto);
        }

        /// <summary>
        /// Get list of all genre in the movie
        /// </summary>
        /// <param name="genreId">Id of genre</param>
        /// <returns></returns>
        [HttpGet("[action]/{genreId:Guid}")]
        [ProducesResponseType(200, Type = typeof(List<MoviesDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult GetGenreInMovie(Guid genreId)
        {
            var objList = _movieRepo.GetGenreInMovie(genreId);

            if (objList == null)
            {
                return NotFound();
            }
            var objDto = new List<MoviesDTO>();
            foreach (var obj in objList)
                objDto.Add(_mapper.Map<MoviesDTO>(obj));

            return Ok(objDto);
        }
        /// <summary>
        /// Get list of all subgenre in movie
        /// </summary>
        /// <param name="subGenreId">Id of genre</param>
        /// <returns></returns>
        [HttpGet("[action]/{subGenreId:Guid}")]
        [ProducesResponseType(200, Type = typeof(List<MoviesDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult GetSubGenreInMovie(Guid subGenreId)
        {
            var objList = _movieRepo.GetSubGenreInMovie(subGenreId);

            if (objList == null)
            {
                return NotFound();
            }
            var objDto = new List<MoviesDTO>();
            foreach (var obj in objList)
                objDto.Add(_mapper.Map<MoviesDTO>(obj));

            return Ok(objDto);
        }

        /// <summary>
        /// Create a Movie
        /// </summary>
        /// <param name="moviesDto">Movie Data transfer object</param>
        /// <returns></returns>

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(201, Type = typeof(List<MoviesDTO>))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequestSizeLimit(50073741824)] // 1 GB limit

        public IActionResult CreateMovie([FromForm] MoviesCreateDTO moviesDto)
        {
            if (moviesDto == null)
            {
                return BadRequest(ModelState);
            }

            if (moviesDto.VideoFile == null || moviesDto.VideoFile.Length == 0)
            {
                ModelState.AddModelError("", "No movie file uploaded.");
                return BadRequest(ModelState);
            }

            if (moviesDto.VideoFile.ContentType != "video/mp4")
            {
                ModelState.AddModelError("", "Invalid file type. Only MP4 video files are allowed.");
                return BadRequest(ModelState);
            }

            try
            {
                // Generate a unique file name using Guid
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(moviesDto.VideoFile.FileName);

                // Define the full file path
                var filePath = Path.Combine(_movieStoragePath, uniqueFileName);

                // Ensure the directory exists
                if (!Directory.Exists(_movieStoragePath))
                {
                    Directory.CreateDirectory(_movieStoragePath);
                }

                // Save the movie file to the specified path
                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    moviesDto.VideoFile.CopyTo(fileStream);
                }

                // Map the DTO to the model
                var movieObj = _mapper.Map<MovieModel>(moviesDto);
                movieObj.VideoFilePath = filePath; // Save the file path in the movie object

                // Create the movie in the database
                if (!_movieRepo.CreateMovie(movieObj))
                {
                    ModelState.AddModelError("", $"Something went wrong when trying to save the record {movieObj.Name}");
                    return StatusCode(500, ModelState);
                }

                return CreatedAtRoute("GetMovieById", new { movieId = movieObj.Id }, movieObj);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error uploading the movie: {ex.Message}");
                return StatusCode(500, ModelState);
            }
        }
        /// <summary>
        /// Updates existing movies in the database by passing movie Id
        /// </summary>
        /// <param name="moviesId">Movie id</param>
        /// <param name="moviesDto">Movie DTO</param>
        /// <returns></returns>
        [HttpPut("{moviesId:Guid}", Name = "UpdateMovie")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "AdminOnly")]
        public IActionResult UpdateMovie(Guid moviesId, [FromBody] MoviesUpdateDTO moviesDto)
        {
            if (moviesDto == null || moviesId != moviesDto.Id)
            {
                return BadRequest(ModelState);
            }

            var genreObj = _mapper.Map<MovieModel>(moviesDto);

            if (!_movieRepo.UpdateMovie(genreObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {genreObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Updates a part of the movie entity in the database
        /// </summary>
        /// <param name="movieId">Id parameter of the movie entity</param>
        /// <param name="patchDoc">Json Patch Document of the movie entity</param>
        /// <returns></returns>
        [HttpPatch("{movieId:Guid}", Name = "PartialUpdateMovie")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "AdminOnly")]

        public IActionResult PartialUpdateMovie(Guid movieId, JsonPatchDocument<MoviesUpdateDTO> patchDoc)
        {
            var movie = _movieRepo.GetMovie(movieId);
            if (movie == null)
            {
                return NotFound();
            }
            var moviePatchDto = _mapper.Map<MoviesUpdateDTO>(movie);

            patchDoc.ApplyTo(moviePatchDto, ModelState);

            _mapper.Map(moviePatchDto, movie);

            _movieRepo.UpdateMovie(movie);

            return NoContent();
        }

        /// <summary>
        /// Delete Movie from database by passing movie id
        /// </summary>
        /// <param name="moviesId">Movie id</param>
        /// <returns></returns>
        [HttpDelete("{moviesId:Guid}", Name = "DeleteMovie")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteMovie(Guid moviesId)
        {
            if (!_movieRepo.MovieExist(moviesId))
            {
                return NotFound();
            }

            var movieObj = _movieRepo.GetMovie(moviesId);

            if (!_movieRepo.DeleteMovie(movieObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {movieObj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        [HttpGet("stream/{movieId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status206PartialContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult StreamMovie(Guid movieId)
        {
            var movie = _movieRepo.GetMovie(movieId);
            if (movie == null)
            {
                return NotFound();
            }

            var filePath = movie.VideoFilePath;

            // Ensure the file exists before streaming
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var fileInfo = new FileInfo(filePath);
            var fileLength = fileInfo.Length;
            var fileExtension = Path.GetExtension(filePath).ToLower();

            string contentType = fileExtension switch
            {
                ".mp4" => "video/mp4",
                ".webm" => "video/webm",
                ".ogg" => "video/ogg",
                _ => "application/octet-stream"
            };

            // Check for Range header
            var rangeHeader = Request.Headers["Range"].ToString();

            if (string.IsNullOrEmpty(rangeHeader))
            {
                // No Range header, return the full file
                Response.Headers["Accept-Ranges"] = "bytes";
                Response.ContentLength = fileLength;
                return PhysicalFile(filePath, contentType);
            }

            // Parse Range header
            var range = rangeHeader.Replace("bytes=", "").Split('-');
            var start = long.Parse(range[0]);
            var end = range.Length > 1 && !string.IsNullOrEmpty(range[1])
                ? long.Parse(range[1])
                : fileLength - 1;

            // Validate range
            if (start < 0 || end >= fileLength || start > end)
            {
                return BadRequest("Invalid range");
            }

            var contentLength = end - start + 1;

            // Set response headers
            Response.StatusCode = StatusCodes.Status206PartialContent;
            Response.Headers["Accept-Ranges"] = "bytes";
            Response.Headers["Content-Range"] = $"bytes {start}-{end}/{fileLength}";
            Response.Headers["Content-Length"] = contentLength.ToString();
            Response.ContentType = contentType;

            // Stream the requested range
            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            stream.Seek(start, SeekOrigin.Begin);

            return File(stream, contentType, enableRangeProcessing: true);
        }


        [HttpGet("latest")]
        public IActionResult GetLatestMovies(int count = 5)
        {
            var latestMovies = _movieRepo.GetMovie()
                                          .OrderByDescending(m => m.Id)
                                          .Take(count)
                                          .ToList();

            if (latestMovies == null || !latestMovies.Any())
            {
                return NotFound("No movies found.");
            }

            var movieDTOs = _mapper.Map<List<MoviesDTO>>(latestMovies);
            return Ok(movieDTOs);
        }
        // Get 5 random movies
        [HttpGet("random5")]
        [ProducesResponseType(200, Type = typeof(List<MoviesDTO>))]
        public IActionResult GetRandomMovies()
        {
            var objList = _movieRepo.GetRandomMovies();
            if (!objList.Any())
            {
                return NotFound("No movies found.");
            }

            var objDto = objList.Select(obj => _mapper.Map<MoviesDTO>(obj)).ToList();
            return Ok(objDto);
        }

    }
}
