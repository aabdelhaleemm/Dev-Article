using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Commands.SignInCommand
{
    public class SignInCommand : IRequest<string>
    {
        public string Password { get; set; }
        public string Email { get; set; }
    }
    public class SignInCommandHandler : IRequestHandler<SignInCommand , string>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IJwtManager _jwtManager;

        public SignInCommandHandler(IApplicationDbContext applicationDbContext , IJwtManager jwtManager)
        {
            _applicationDbContext = applicationDbContext;
            _jwtManager = jwtManager;
        }
        public async Task<string> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user =await _applicationDbContext.Users
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Email == request.Email && x.Password == request.Password,
                        cancellationToken: cancellationToken);
                if (user == null)
                {
                    return null;
                }
                var token = _jwtManager.GenerateToken(user.Id,user.Name,"user");
                return token;
            }
            catch (Exception )
            {
                return null;
            }
            

        }
    }
}