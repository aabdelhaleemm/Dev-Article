
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Extensions;
using Application.Likes.Commands.AddLikesCommand;
using Application.Likes.Commands.DeleteLikesCommand;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LikesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LikesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{postId}")]
        public async Task<IActionResult> AddLike(int postId,CancellationToken cancellationToken)
        {
            var like =await _mediator.Send(new AddLikesCommand(postId, User.GetUserId()), cancellationToken);
            if (!like)
            {
                return BadRequest();
            }

            return Ok();
 
        }

        [HttpDelete("")]
        public async Task<IActionResult> DeleteLike([FromBody] int postId,CancellationToken cancellationToken)
        {
            var like =await _mediator.Send(new DeleteLikesCommand(postId, User.GetUserId()), cancellationToken);
            if (!like)
            {
                return BadRequest();
            }

            return Ok();

        }
    }
}