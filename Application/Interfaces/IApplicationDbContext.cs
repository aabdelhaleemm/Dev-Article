using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<Domain.Entities.Comments> Comments { get; set; }
        public DbSet<Domain.Entities.Users> Users { get; set; }
        public DbSet<Domain.Entities.Posts> Posts { get; set; }
        public DbSet<Domain.Entities.Likes> Likes { get; set; }
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        
    }
}