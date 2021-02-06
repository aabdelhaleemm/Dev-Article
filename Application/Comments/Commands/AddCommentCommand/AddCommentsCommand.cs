using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
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
        private readonly IMapper _mapper;

        public AddCommentsCommandHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<bool> Handle(AddCommentsCommand request, CancellationToken cancellationToken)
        {
            var comment = _mapper.Map<Domain.Entities.Comments>(request);
            comment.CreatedAt=DateTime.Now;
            await _applicationDbContext.Comments.AddAsync(comment, cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}