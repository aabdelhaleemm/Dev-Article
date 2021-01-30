using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Posts.Commands.AddPostsCommand;
using Application.Posts.Commands.DeletePostsCommand;
using Application.Posts.Commands.UpdatePostsCommand;
using Application.Posts.Queries.GetPostsByIdQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public PostsController(IMediator mediator)
        {
            _mediator = mediator;
           
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(int id,CancellationToken cancellationToken)
        {
            var posts= await _mediator.Send(new GetPostsByIdQuery(id), cancellationToken);
            if (posts == null)
            {
                return NotFound();
            }
            return Ok(posts);
        }

        [HttpPost]
        public async Task<IActionResult> AddPosts(AddPostsCommand command,CancellationToken cancellationToken)
        {
            command.UserId = Convert.ToInt32(User.FindFirst("Id")?.Value!); ;
            var post =await _mediator.Send(command, cancellationToken);
            if (!post)
            {
                return BadRequest(new {error = "Couldn't add the post"});
            }
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePost(UpdatePostsCommand command,CancellationToken cancellationToken)
        {
            var post = await _mediator.Send(command, cancellationToken);
            if (!post)
            {
                return BadRequest(new {error = "The operation couldn't be completed"});
            }
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePost([FromBody] int postId,CancellationToken cancellationToken)
        {
            var userId = Convert.ToInt32(HttpContext.User.FindFirst("Id")?.Value) ;
            var post = await _mediator.Send(new DeletePostsCommand(postId,userId), cancellationToken);
            if (!post)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}