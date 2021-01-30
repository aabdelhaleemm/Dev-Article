
using Application.Comments.Commands.AddCommentCommand;
using Application.Posts.Commands.AddPostsCommand;
using Application.Posts.Queries.Dto;
using Application.Users.Commands.SignUpCommand;
using Application.Users.Commands.UpdateUserCommand;
using Application.Users.Queries.UsersDto;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            //Source -> Target
            CreateMap<Domain.Entities.Posts, PostsDto>();
            CreateMap<Domain.Entities.Likes, PostsLikesDto>();
            CreateMap<Domain.Entities.Comments, PostsCommentsDto>();
            CreateMap<Domain.Entities.Users, PostsCommentsUsersDto>();
            CreateMap<Domain.Entities.Users, PostsUserDto>();
            CreateMap<Domain.Entities.Posts, PostsUserPostsDto>();
            CreateMap<Domain.Entities.Users, UsersDto>();
            CreateMap<Domain.Entities.Comments, UsersCommentsDto>();
            CreateMap<Domain.Entities.Posts, UsersPostsDto>();
            CreateMap<AddPostsCommand, Domain.Entities.Posts>();
            CreateMap<SignUpCommand, Domain.Entities.Users>();
            CreateMap<UpdateUserCommand, Domain.Entities.Users>();
            CreateMap<AddCommentsCommand, Domain.Entities.Comments>();
        }
    }
}