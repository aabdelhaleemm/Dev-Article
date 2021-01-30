using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Comments.Commands.AddCommentCommand;
using Application.Comments.Commands.DeleteCommentCommand;
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
            command.UserId = Convert.ToInt32(HttpContext.User.FindFirst("Id")?.Value); 
            var comment = await _mediator.Send(command, cancellationToken);
            if (!comment)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteComment([FromBody]int commentId,CancellationToken cancellationToken)
        {
            var userId= Convert.ToInt32(HttpContext.User.FindFirst("Id")?.Value);
            var comment =await _mediator.Send(new DeleteCommentCommand(commentId, userId), cancellationToken);
            if (!comment)
            {
                return BadRequest();
            }
            return Ok();
        }
        
    }
}