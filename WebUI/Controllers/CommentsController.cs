using System.Threading;
using System.Threading.Tasks;
using Application.Comments.Commands.AddCommentCommand;
using Application.Comments.Commands.DeleteCommentCommand;
using Application.Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace WebUI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CommentsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost("")]
        public async Task<IActionResult> AddComment(AddCommentsCommand command,CancellationToken cancellationToken)
        {
            command.UserId = User.GetUserId(); 
            var comment = await _mediator.Send(command, cancellationToken);
            if (!comment)
            {
                return BadRequest("Cannot add comment try again");
            }

            return Created(string.Empty, string.Empty);
        }

        [HttpDelete("")]
        public async Task<IActionResult> DeleteComment(DeleteCommentCommand command,CancellationToken cancellationToken)
        {
            command.UserId = User.GetUserId();
            var comment =await _mediator.Send(command, cancellationToken);
            if (!comment)
            {
                return BadRequest();
            }
            return Ok();
        }
        
    }
}