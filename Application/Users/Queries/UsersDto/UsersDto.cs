using System;
using System.Collections.Generic;
using Domain.Entities;

namespace Application.Users.Queries.UsersDto
{
    public class UsersDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public string Country { get; set; }
        public DateTime JoinedAt { get; set; }
        public string Skills { get; set; }
        public string UserName { get; set; }

        public IEnumerable<UsersPostsDto> Posts { get;  set; }
        public IEnumerable<UsersCommentsDto> Comments { get;  set; }
    }
}