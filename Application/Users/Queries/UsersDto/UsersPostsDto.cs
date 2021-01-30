using System;
using System.Collections.Generic;
using Domain.Entities;

namespace Application.Users.Queries.UsersDto
{
    public class UsersPostsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Topics { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
    }
}