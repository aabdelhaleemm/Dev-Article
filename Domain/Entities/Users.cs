using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Users
    {
        

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Bio { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public DateTime JoinedAt { get; set; }
        public string Password { get; set; }
        public string Skills { get; set; }
        public string UserName { get; set; }

        public IEnumerable<Posts> Posts { get;  set; }
        public IEnumerable<Likes> Likes { get;  set; }
        public IEnumerable<Comments> Comments { get;  set; }
    }
}