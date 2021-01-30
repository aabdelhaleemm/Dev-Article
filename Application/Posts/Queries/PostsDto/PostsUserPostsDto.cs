using System;

namespace Application.Posts.Queries.Dto
{
    public class PostsUserPostsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Topics { get; set; }
        public DateTime CreatedAt { get; set; } 
    }
}