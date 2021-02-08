
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Extensions;
using Application.Posts.Commands.AddPostsCommand;
using Application.Posts.Commands.DeletePostsCommand;
using Application.Posts.Commands.UpdatePostsCommand;
using Application.Posts.Queries.GetPostsByIdQuery;
using Application.Posts.Queries.GetPostsWithPagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using StackExchange.Redis;

namespace WebUI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IMediator _mediator;

        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public PostsController(IMediator mediator,  IConnectionMultiplexer connectionMultiplexer)
        {
            _mediator = mediator;

            _connectionMultiplexer = connectionMultiplexer;
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(int id,CancellationToken cancellationToken)
        {
            var posts= await _mediator.Send(new GetPostsByIdQuery(id,User.GetUserId()), cancellationToken);
            if (posts == null)
            {
                return NotFound();
            }
            return Ok(posts);
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetPosts([FromQuery] GetPostsWithPaginationQuery query,CancellationToken cancellationToken)
        {
            query.UserId = User.GetUserId();
            var post =await _mediator.Send(query, cancellationToken);
            
            return Ok(post);
        }
        [HttpPost]
        public async Task<IActionResult> AddPosts(AddPostsCommand command,CancellationToken cancellationToken)
        {
            command.UserId = User.GetUserId();
            var post =await _mediator.Send(command, cancellationToken);
            if (!post)
            {
                return BadRequest( "Couldn't add the post");
            }
            return Created(string.Empty, string.Empty);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePost(UpdatePostsCommand command,CancellationToken cancellationToken)
        {
            var post = await _mediator.Send(command, cancellationToken);
            if (!post)
            {
                return BadRequest("The operation couldn't be completed");
            }
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePost(DeletePostsCommand command,CancellationToken cancellationToken)
        {
            command.UserId = User.GetUserId();
            var post = await _mediator.Send(command, cancellationToken);
            if (!post)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}