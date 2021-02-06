using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
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

        public AddLikesCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<bool> Handle(AddLikesCommand request, CancellationToken cancellationToken)
        {
            var post = await _applicationDbContext.Posts
                .Include(x=>x.Likes)
                .FirstOrDefaultAsync(x => x.Id == request.PostId, cancellationToken: cancellationToken);
            var user =post.Likes.FirstOrDefault(x => x.UserId == request.UserId);
            if (user != null) return false;
            post.Likes.Add(new Domain.Entities.Likes()
            {
                UserId = request.UserId,
                PostId = request.PostId
            });
            return await _applicationDbContext.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}