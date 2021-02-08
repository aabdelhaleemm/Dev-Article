using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Likes.Commands.DeleteLikesCommand
{
    public class DeleteLikesCommand : IRequest<bool>
    {
        public int PostId { get; set; }
        public int UserId { get; set; }

        public DeleteLikesCommand(int postId, int userId)
        {
            PostId = postId;
            UserId = userId;
        }
    }

    public class DeleteLikesCommandHandler : IRequestHandler<DeleteLikesCommand, bool>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly ICacheService _cacheService;

        public DeleteLikesCommandHandler(IApplicationDbContext applicationDbContext , ICacheService cacheService)
        {
            _applicationDbContext = applicationDbContext;
            _cacheService = cacheService;
        }

        public async Task<bool> Handle(DeleteLikesCommand request, CancellationToken cancellationToken)
        {
            var post =await _applicationDbContext.Posts
                .Include(x=>x.Likes)
                .FirstOrDefaultAsync(x => x.Id == request.PostId, cancellationToken: cancellationToken);
            post.Likes.Remove(new Domain.Entities.Likes()
            {
                UserId = request.UserId,
                PostId = request.PostId
            });
            post.TotalLikes -= 1;
            if (await _applicationDbContext.SaveChangesAsync(cancellationToken) <= 0) return false;
            await _cacheService.DeleteKeyAsync($"post{request.PostId}");
            return true;

        }
    }
}