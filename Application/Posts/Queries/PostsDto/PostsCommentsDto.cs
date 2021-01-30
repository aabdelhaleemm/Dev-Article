using System;
using Domain.Entities;

namespace Application.Posts.Queries.Dto
{
    public class PostsCommentsDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        
        public PostsCommentsUsersDto User { get; set; }
        
    }
}