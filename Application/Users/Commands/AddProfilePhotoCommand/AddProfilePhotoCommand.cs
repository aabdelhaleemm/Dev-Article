using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Commands.AddProfilePhotoCommand
{
    public class AddProfilePhotoCommand : IRequest<bool>
    {
        public AddProfilePhotoCommand(IFormFile file, int userId)
        {
            File = file;
            UserId = userId;
        }
        public IFormFile File { get; set; }
        public int UserId { get; set; }
        
    }
    
    public class AddProfilePhotoCommandHandler : IRequestHandler<AddProfilePhotoCommand,bool>
    {
        private readonly IMediator _mediator;
        private readonly ICacheService _cacheService;
        private readonly IPhotoService _photoService;
        private readonly UserManager<Domain.Entities.Users> _userManager;

        public AddProfilePhotoCommandHandler(IMediator mediator ,ICacheService cacheService,IPhotoService photoService , UserManager<Domain.Entities.Users> userManager)
        {
            _mediator = mediator;
            _cacheService = cacheService;
            _photoService = photoService;
            _userManager = userManager;
        }
        public async Task<bool> Handle(AddProfilePhotoCommand request  ,CancellationToken cancellationToken)
        {
            var user =await _userManager.Users.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);
            if (user == null) return false;
            if (user.PhotoPublicId != null)
            {
                var deleteResult =await _mediator.Send(new DeleteProfilePhotoCommand.DeleteProfilePhotoCommand(request.UserId), cancellationToken);
                if (!deleteResult)
                {
                    return false;
                }
            }
            var result =await _photoService.AddPhotoAsync(request.File);
            if (result.Error != null) return false;
            user.PhotoURl = result.SecureUrl.AbsoluteUri;
            user.PhotoPublicId = result.PublicId;
            var updatedUser =await _userManager.UpdateAsync(user);
            await _cacheService.DeleteKeyAsync($"user{request.UserId}");
            return updatedUser.Succeeded;
        }
    }
}