using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Posts
    {
        
        [Key]
        public int Id { get; set; }
        public int UserId { get;  set; }
        public string Title { get; set; }
        public string Topics { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Users User { get;  set; }
        public ICollection<Likes> Likes { get;  set; }
        public ICollection<Comments> Comments { get;  set; }
        
    }
}