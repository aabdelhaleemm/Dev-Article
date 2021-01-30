using System;

namespace Domain.Entities
{
    public class Comments
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; }
        
        public Posts Post { get; set; }
        public Users User { get; set; }
    }
}