using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Comments.Commands.AddCommentCommand
{
    public class AddCommentsCommand : IRequest<bool>
    {
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; }
       
    }

    public class AddCommentsCommandHandler : IRequestHandler<AddCommentsCommand, bool>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;

        public AddCommentsCommandHandler(IApplicationDbContext applicationDbContext,ICacheService cacheService, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<bool> Handle(AddCommentsCommand request, CancellationToken cancellationToken)
        {
            var comment = _mapper.Map<Domain.Entities.Comments>(request);
            comment.CreatedAt=DateTime.Now;
            await _applicationDbContext.Comments.AddAsync(comment, cancellationToken);
            if (await _applicationDbContext.SaveChangesAsync(cancellationToken) <= 0) return false;
            await _cacheService.DeleteKeyAsync($"post{request.PostId}");
            return true;
        }
    }
}