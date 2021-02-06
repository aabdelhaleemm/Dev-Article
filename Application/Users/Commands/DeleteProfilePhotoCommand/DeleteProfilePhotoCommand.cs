using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Commands.DeleteProfilePhotoCommand
{
    public class DeleteProfilePhotoCommand : IRequest<bool>
    {
        public DeleteProfilePhotoCommand(int userId)
        {
            UserId = userId;
        }

        public int UserId { get; set; }
        
    }
    
    public class DeleteProfilePhotoCommandHandler : IRequestHandler<DeleteProfilePhotoCommand , bool>
    {
        private readonly UserManager<Domain.Entities.Users> _userManager;
        private readonly IPhotoService _photoService;

        public DeleteProfilePhotoCommandHandler(UserManager<Domain.Entities.Users> userManager , IPhotoService photoService)
        {
            _userManager = userManager;
            _photoService = photoService;
        }
        public async Task<bool> Handle(DeleteProfilePhotoCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken: cancellationToken);
            if (user == null) return false;
            var result = await _photoService.DeletePhotoAsync(user.PhotoPublicId);
            if (result.Error != null) return false;
            user.PhotoURl = null;
            user.PhotoPublicId = null;
            var updatedUser = await _userManager.UpdateAsync(user);
            return updatedUser.Succeeded;
        }
    }
}