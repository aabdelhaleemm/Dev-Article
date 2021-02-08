using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Posts.Commands.DeletePostsCommand
{
    public class DeletePostsCommand : IRequest<bool>
    {
        public int PostId { get; set; }
        public int UserId { get; set; }

        public DeletePostsCommand(int postId, int userId)
        {
            PostId = postId;
            UserId = userId;
        }
    }

    public class DeletePostsCommandHandler : IRequestHandler<DeletePostsCommand, bool>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly ICacheService _cacheService;

        public DeletePostsCommandHandler(IApplicationDbContext applicationDbContext, ICacheService cacheService)
        {
            _applicationDbContext = applicationDbContext;
            _cacheService = cacheService;
        }

        public async Task<bool> Handle(DeletePostsCommand request, CancellationToken cancellationToken)
        {
            var post = await _applicationDbContext.Posts.SingleOrDefaultAsync(x => x.Id == request.PostId,
                cancellationToken);
            if (post.UserId != request.UserId)
            {
                return false;
            }
            
            _applicationDbContext.Posts.Remove(post);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            await _cacheService.DeleteKeyAsync($"post{request.PostId}");
            await _cacheService.DeleteKeyAsync($"user{request.UserId}");
            return true;
        }
    }
}