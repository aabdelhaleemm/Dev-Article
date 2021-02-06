using System;
using System.Collections.Generic;
using Domain.Entities;

namespace Application.Posts.Queries.Dto
{
    public class PostsDto
    {
        public int Id { get; set; }
        public int UserId { get;  set; }
        public string Title { get; set; }
        public string Topics { get; set; }
        public string Content { get; set; }
        public int TotalLikes { get; set; }
        public bool CanLike { get; set; } = true;
        public DateTime CreatedAt { get; set; } 
        public PostsUserDto User { get;  set; }
        public IEnumerable<PostsLikesDto> Likes { get;  set; }
        public IEnumerable<PostsCommentsDto> Comments { get;  set; }
    }
}