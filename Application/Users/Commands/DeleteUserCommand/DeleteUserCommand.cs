using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Commands.DeleteUserCommand
{
    public class DeleteUserCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public DeleteUserCommand(int id)
        {
            Id = id;
        }
    }

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly UserManager<Domain.Entities.Users> _userManager;
        private readonly ICacheService _cacheService;

        public DeleteUserCommandHandler(UserManager<Domain.Entities.Users> userManager , ICacheService cacheService)
        {
            _userManager = userManager;
            _cacheService = cacheService;
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == request.Id,
                 cancellationToken);
            if (user == null) return false;
            var result = await _userManager.DeleteAsync(user);
            await _cacheService.DeleteKeyAsync($"user{request.Id}");
            return result.Succeeded;
        }
    }
}