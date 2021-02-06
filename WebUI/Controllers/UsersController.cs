using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3.Util;
using Application.Users.Commands.AddProfilePhotoCommand;
using Application.Users.Commands.DeleteProfilePhotoCommand;
using Application.Users.Commands.DeleteUserCommand;
using Application.Users.Commands.SignInCommand;
using Application.Users.Commands.SignUpCommand;
using Application.Users.Commands.UpdateUserCommand;
using Application.Users.Queries.GetUserById;
using Infrastructure.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id , CancellationToken cancellationToken)
        {
            var user =await  _mediator.Send(new GetUsersByIdQuery(id), cancellationToken);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [AllowAnonymous]
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(SignInCommand command,CancellationToken cancellationToken)
        {
            
            var token = await _mediator.Send(command, cancellationToken);
            if (token == null)
            {
                return BadRequest( "invalid email or password");
            }
            return Ok(token);
        }
        [AllowAnonymous]
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpCommand command,CancellationToken cancellationToken)
        {
            var token =await _mediator.Send(command, cancellationToken);
            if (token == null)
            {
                return BadRequest("Email or username already in use");
            }
            return Ok(token);
        }
        
        [HttpPut]
        public async Task<IActionResult> EditUser(UpdateUserCommand command,CancellationToken cancellationToken)
        {
            command.Id = User.GetUserId();
            var user =await _mediator.Send(command, cancellationToken);
            if (!user)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(CancellationToken cancellationToken)
        {
            var user =await _mediator.Send(new DeleteUserCommand(User.GetUserId()), cancellationToken);
            if (!user)
            {
                return BadRequest( "The operation couldn't be completed");
            }
            return Ok();
        }
        [HttpPost("Photo")]
        public async Task<IActionResult> AddProfilePhoto(IFormFile file ,CancellationToken cancellationToken)
        {
            var user =await _mediator.Send(new AddProfilePhotoCommand(file, User.GetUserId()), cancellationToken);
            if (!user) return BadRequest("Cannot add the photo");
            return Created(string.Empty, string.Empty);
        }

        [HttpDelete("Photo")]
        public async Task<IActionResult> DeleteProfilePhoto()
        {
            var user =await _mediator.Send(new DeleteProfilePhotoCommand(User.GetUserId()));
            if (!user) return BadRequest("Cannot delete this photo");
            return Ok();
        }
        
    }
}