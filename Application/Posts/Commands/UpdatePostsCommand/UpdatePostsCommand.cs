using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Posts.Commands.UpdatePostsCommand
{
    public class UpdatePostsCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Topics { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class UpdatePostsCommandHandler : IRequestHandler<UpdatePostsCommand, bool>
    {
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private readonly IApplicationDbContext _applicationDbContext;

        public UpdatePostsCommandHandler(IMapper mapper, ICacheService cacheService,IApplicationDbContext applicationDbContext)
        {
            _mapper = mapper;
            _cacheService = cacheService;
            _applicationDbContext = applicationDbContext;
        }

        public async Task<bool> Handle(UpdatePostsCommand request, CancellationToken cancellationToken)
        {
            var post = _mapper.Map<Domain.Entities.Posts>(request);
             _applicationDbContext.Posts.Update(post);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            await _cacheService.DeleteKeyAsync($"post{request.Id}");
            await _cacheService.DeleteKeyAsync($"user{request.UserId}");
            return true;
        }
    }
}