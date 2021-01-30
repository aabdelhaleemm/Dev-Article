using System;
using System.Collections.Generic;

namespace Application.Posts.Queries.Dto
{
    public class PostsUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public string Country { get; set; }
        public DateTime JoinedAt { get; set; }
        public IEnumerable<PostsUserPostsDto> Posts { get;  set; }

    }
}