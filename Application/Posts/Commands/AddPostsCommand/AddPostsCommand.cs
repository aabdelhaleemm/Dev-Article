using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Posts.Commands.AddPostsCommand
{
    public class AddPostsCommand : IRequest<bool>
    {
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Topics { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    public class AddPostsCommandHandler : IRequestHandler<AddPostsCommand, bool>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _applicationDbContext;

        public AddPostsCommandHandler(IMapper mapper, IApplicationDbContext applicationDbContext)
        {
            _mapper = mapper;
            _applicationDbContext = applicationDbContext;
        }

        public async Task<bool> Handle(AddPostsCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.Posts>(request);
            await _applicationDbContext.Posts
                .AddAsync(entity, cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}