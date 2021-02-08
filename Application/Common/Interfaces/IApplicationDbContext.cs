using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<Domain.Entities.Comments> Comments { get; set; }
        public DbSet<Domain.Entities.Users> Users { get; set; }
        public DbSet<Domain.Entities.Posts> Posts { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        
    }
}