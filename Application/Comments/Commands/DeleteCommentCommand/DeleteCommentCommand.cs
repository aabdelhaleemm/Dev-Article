using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Comments.Commands.DeleteCommentCommand
{
    public class DeleteCommentCommand : IRequest<bool>
    {
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }

        public DeleteCommentCommand(int commentId, int userId, int postId)
        {
            CommentId = commentId;
            UserId = userId;
            PostId = postId;
        }

        public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, bool>
        {
            private readonly IApplicationDbContext _applicationDbContext;
            private readonly ICacheService _cacheService;

            public DeleteCommentCommandHandler(IApplicationDbContext applicationDbContext , ICacheService cacheService)
            {
                _applicationDbContext = applicationDbContext;
                _cacheService = cacheService;
            }

            public async Task<bool> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
            {
                var comment = await _applicationDbContext.Comments
                    .SingleOrDefaultAsync(x => x.Id == request.CommentId && x.UserId == request.UserId, cancellationToken);
                if (comment == null || comment.UserId != request.UserId)
                {
                    return false;
                }
                _applicationDbContext.Comments.Remove(comment);
                if (await _applicationDbContext.SaveChangesAsync(cancellationToken) <= 0) return false;
                await _cacheService.DeleteKeyAsync($"post{request.PostId}");
                return true;
            }
        }
    }
}