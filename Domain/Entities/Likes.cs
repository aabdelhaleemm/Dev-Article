using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Likes
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public int PostId { get; set; }
        public Posts Post { get; set; }
        public Users User { get; set; }
        
        
    }
}