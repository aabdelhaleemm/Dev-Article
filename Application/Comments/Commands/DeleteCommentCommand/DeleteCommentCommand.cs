using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Comments.Commands.DeleteCommentCommand
{
    public class DeleteCommentCommand : IRequest<bool>
    {
        public int CommentId { get; set; }
        public int UserId { get; set; }

        public DeleteCommentCommand(int commentId, int userId)
        {
            CommentId = commentId;
            UserId = userId;
        }

        public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, bool>
        {
            private readonly IApplicationDbContext _applicationDbContext;

            public DeleteCommentCommandHandler(IApplicationDbContext applicationDbContext)
            {
                _applicationDbContext = applicationDbContext;
            }

            public async Task<bool> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
            {
                var comment = await _applicationDbContext.Comments
                    .SingleOrDefaultAsync(x => x.Id == request.CommentId && x.UserId == request.UserId, cancellationToken: cancellationToken);
                if (comment == null || comment.UserId != request.UserId)
                {
                    return false;
                }
                _applicationDbContext.Comments.Remove(comment);
                await _applicationDbContext.SaveChangesAsync(cancellationToken);
                return true;
            }
        }
    }
}