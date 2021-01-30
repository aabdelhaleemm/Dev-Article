using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Users.Commands.SignUpCommand
{
    public class SignUpCommand : IRequest<string>
    {
        public string Name { get; set; }
        public string Bio { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string Password { get; set; }
        public string Skills { get; set; }
    }
    
    public class AddUserCommandHandler : IRequestHandler<SignUpCommand , string>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IJwtManager _jwtManager;


        public AddUserCommandHandler(IMapper mapper , IApplicationDbContext applicationDbContext , IJwtManager jwtManager)
        {
            _mapper = mapper;
            _applicationDbContext = applicationDbContext;
            _jwtManager = jwtManager;
        }
        public async Task<string> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = _mapper.Map<Domain.Entities.Users>(request);
                    
                entity.JoinedAt= DateTime.Now;
                await _applicationDbContext.Users.AddAsync(entity, cancellationToken);
                await _applicationDbContext.SaveChangesAsync(cancellationToken);
                var token = _jwtManager.GenerateToken(entity.Id, entity.Name ,"user");
                Console.WriteLine(entity.JoinedAt);
                return token;
            }
            catch (Exception )
            {
                return null;
            }
            
            
        }
    }
}