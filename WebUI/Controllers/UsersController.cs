using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3.Util;
using Application.Users.Commands.DeleteUserCommand;
using Application.Users.Commands.SignInCommand;
using Application.Users.Commands.SignUpCommand;
using Application.Users.Commands.UpdateUserCommand;
using Application.Users.Queries.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
            using var getUserByIdQuery = new GetUsersByIdQuery(id);
            var user =await  _mediator.Send(getUserByIdQuery, cancellationToken);
            
            
                
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
                return BadRequest(new {error= "invalid email or password"});
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
                return BadRequest(new {error = "Email or username already in use"});
            }
            return Ok(token);
        }
        
        [HttpPut]
        public async Task<IActionResult> EditUser(UpdateUserCommand command,CancellationToken cancellationToken)
        {
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
            if (HttpContext == null)
            {
                return BadRequest(new {error = "Can't specify user id"});
            }
            var id = Convert.ToInt32(HttpContext.User.FindFirst("Id")?.Value) ;
            var user =await _mediator.Send(new DeleteUserCommand(id), cancellationToken);
            if (!user)
            {
                return BadRequest(new {error = "The operation couldn't be completed"});
            }
            return Ok();
        }
    }
}