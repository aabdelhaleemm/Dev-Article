using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Users.Commands.UpdateUserCommand
{
    public class UpdateUserCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public DateTime JoinedAt { get; set; }
        public string Password { get; set; }
        public string Skills { get; set; }
        public string UserName { get; set; }
    }
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand , bool>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _applicationDbContext;

        public UpdateUserCommandHandler(IMapper mapper , IApplicationDbContext applicationDbContext)
        {
            _mapper = mapper;
            _applicationDbContext = applicationDbContext;
        }
        public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = _mapper.Map<Domain.Entities.Users>(request);
                var user = _applicationDbContext.Users.Update(entity);
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