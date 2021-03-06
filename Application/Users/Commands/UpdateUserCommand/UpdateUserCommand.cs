using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;

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

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
    {
        private readonly IMapper _mapper;
        private readonly UserManager<Domain.Entities.Users> _userManager;
        private readonly ICacheService _cacheService;

        public UpdateUserCommandHandler(IMapper mapper, UserManager<Domain.Entities.Users> userManager , ICacheService cacheService)
        {
            _mapper = mapper;
            _userManager = userManager;
            _cacheService = cacheService;
        }

        public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.Users>(request);
            var user = await _userManager.UpdateAsync(entity);
            await _cacheService.DeleteKeyAsync($"user{request.Id}");
            return user.Succeeded;
        }
    }
}