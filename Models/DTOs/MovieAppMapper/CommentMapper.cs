// CommentMapper.cs
using AutoMapper;
using MovieApp.API.Models;
using MovieApp.API.Models.DTOs;

namespace MovieApp.API.Models.DTOs.MovieAppMapper
{
    public class CommentMapper : Profile
    {
        public CommentMapper()
        {
            CreateMap<Comment, CommentDTO>().ReverseMap();
            CreateMap<CommentCreateDTO, Comment>().ReverseMap();
            CreateMap<CommentUpdateDTO, Comment>().ReverseMap();  // Added mapping for CommentUpdateDTO
        }
    }
}
