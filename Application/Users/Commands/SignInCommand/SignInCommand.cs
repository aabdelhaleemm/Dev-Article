using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Commands.SignInCommand
{
    public class SignInCommand : IRequest<string>
    {
        public string Password { get; set; }
        public string Email { get; set; }
    }

    public class SignInCommandHandler : IRequestHandler<SignInCommand, string>
    {
        private readonly IJwtManager _jwtManager;
        private readonly UserManager<Domain.Entities.Users> _userManager;

        public SignInCommandHandler(IJwtManager jwtManager, UserManager<Domain.Entities.Users> userManager)
        {
            _jwtManager = jwtManager;
            _userManager = userManager;
        }

        public async Task<string> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            
            var user = await _userManager.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken: cancellationToken);
            if (user == null)
            {
                return null;
            }

            var result = await _userManager.CheckPasswordAsync(user, request.Password);
            return !result ? null : _jwtManager.GenerateToken(user.Id, user.Name, "user");
        }
    }
}