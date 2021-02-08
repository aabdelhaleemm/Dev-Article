using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class Users : IdentityUser<int>
    {
        public string PhotoURl { get; set; }
        public string PhotoPublicId { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
       
        public string Country { get; set; }
        public DateTime JoinedAt { get; set; }

        public string Skills { get; set; }
        

        public ICollection<Posts> Posts { get;  set; }
        public ICollection<Likes> Likes { get;  set; }
        public ICollection<Comments> Comments { get;  set; }
    }
}