using System;

namespace Application.Users.Queries.UsersDto
{
    public class UsersCommentsDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; }
        
    }
}