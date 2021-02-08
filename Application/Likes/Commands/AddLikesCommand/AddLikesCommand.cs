using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Likes.Commands.AddLikesCommand
{
    public class AddLikesCommand : IRequest<bool>
    {
        public int PostId { get; set; }
        public int UserId { get; set; }

        public AddLikesCommand(int postId, int userId)
        {
            PostId = postId;
            UserId = userId;
        }
    }

    public class AddLikesCommandHandler : IRequestHandler<AddLikesCommand, bool>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly ICacheService _cacheService;

        public AddLikesCommandHandler(IApplicationDbContext applicationDbContext , ICacheService cacheService)
        {
            _applicationDbContext = applicationDbContext;
            _cacheService = cacheService;
        }
        public async Task<bool> Handle(AddLikesCommand request, CancellationToken cancellationToken)
        {
            var post = await _applicationDbContext.Posts
                .Include(x=>x.Likes)
                .FirstOrDefaultAsync(x => x.Id == request.PostId,  cancellationToken);
            var user =post.Likes.FirstOrDefault(x => x.UserId == request.UserId);
            if (user != null) return false;
            post.Likes.Add(new Domain.Entities.Likes()
            {
                UserId = request.UserId,
                PostId = request.PostId
            });
            post.TotalLikes += 1;
            if (await _applicationDbContext.SaveChangesAsync(cancellationToken) <= 0) return false;
            await _cacheService.DeleteKeyAsync($"post{request.PostId}");
            return true;

        }
    }
}