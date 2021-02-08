using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Users.Commands.SignUpCommand
{
    public class SignUpCommand : IRequest<string>
    {
        public string Name { get; set; }
        public string Bio { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string Password { get; set; }
        public string Skills { get; set; }
    }

    public class AddUserCommandHandler : IRequestHandler<SignUpCommand, string>
    {
        private readonly UserManager<Domain.Entities.Users> _userManager;
        private readonly IMapper _mapper;
        private readonly IJwtManager _jwtManager;


        public AddUserCommandHandler(UserManager<Domain.Entities.Users> userManager, IMapper mapper,
            IJwtManager jwtManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _jwtManager = jwtManager;
        }

        public async Task<string> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.Users>(request);
            entity.JoinedAt = DateTime.Now;

            var user = await _userManager.CreateAsync(entity, request.Password);
            if (!user.Succeeded) return null;
            var token = _jwtManager.GenerateToken(entity.Id, entity.Name, "user");
            return token;
        }
    }
}