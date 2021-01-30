using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using MediatR;

namespace Application.Likes.Commands.AddLikesCommand
{
    public class AddLikesCommand : IRequest<bool>
    {
        public int PostId { get; set; }
        public int UserId { get; set; }

        public AddLikesCommand(int postId , int userId)
        {
            PostId = postId;
            UserId = userId;
        }
        
    }
    public class AddLikesCommandHandler : IRequestHandler<AddLikesCommand,bool>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public AddLikesCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<bool> Handle(AddLikesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _applicationDbContext.Likes.AddAsync(new Domain.Entities.Likes()
                {
                    UserId = request.UserId,
                    PostId = request.PostId
                }, cancellationToken);
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