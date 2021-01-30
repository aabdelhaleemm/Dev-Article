using System;
using System.Threading;
using System.Threading.Tasks;
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

        [HttpPost("")]
        public async Task<IActionResult> AddLike([FromBody]int postId,CancellationToken cancellationToken)
        {
            var userId = Convert.ToInt32(HttpContext.User.FindFirst("Id")?.Value) ;
            var like =await _mediator.Send(new AddLikesCommand(postId, userId), cancellationToken);
            if (!like)
            {
                return BadRequest();
            }

            return Ok();
 
        }

        [HttpDelete("")]
        public async Task<IActionResult> DeleteLike([FromBody] int postId,CancellationToken cancellationToken)
        {
            var userId = Convert.ToInt32(HttpContext.User.FindFirst("Id")?.Value) ;
            var like =await _mediator.Send(new DeleteLikesCommand(postId, userId), cancellationToken);
            if (!like)
            {
                return BadRequest();
            }

            return Ok();

        }
    }
}