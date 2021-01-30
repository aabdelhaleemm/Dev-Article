using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Likes.Commands.DeleteLikesCommand
{
    public class DeleteLikesCommand : IRequest<bool>
    {
        public int PostId { get; set; }
        public int UserId { get; set; }

        public DeleteLikesCommand(int postId , int userId)
        {
            PostId = postId;
            UserId = userId;
        }
        
    }
    
    public class DeleteLikesCommandHandler: IRequestHandler<DeleteLikesCommand,bool>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public DeleteLikesCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<bool> Handle(DeleteLikesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var like =await _applicationDbContext.Likes.SingleOrDefaultAsync(x =>
                    x.PostId == request.UserId && x.PostId == request.PostId, cancellationToken: cancellationToken);
                if (like == null)
                {
                    return false;
                }

                _applicationDbContext.Likes.Remove(like);
                await _applicationDbContext.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (Exception )
            {
                return false;
            }
        }
    }
}