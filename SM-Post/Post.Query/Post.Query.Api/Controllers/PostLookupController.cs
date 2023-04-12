using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Common.DTOs;
using Post.Query.Api.DTOs;
using Post.Query.Api.Queries;
using Post.Query.Domain.Entities;

namespace Post.Query.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PostLookupController : ControllerBase
    {
        private readonly ILogger<PostLookupController> _logger;
        private readonly IQueryDispatcher<PostEntity> _queryDispatcher;

        public PostLookupController(ILogger<PostLookupController> logger, IQueryDispatcher<PostEntity> queryDispatcher)
        {
            _logger = logger;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllPostsAsync()
        {
            try
            {
                var posts = await _queryDispatcher.SendAsync(new FindAllPostQuery());
                return NormalResponse(posts);
            }
            catch (Exception exception)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to retrieve all posts!";
                return ErrorResponse(exception, SAFE_ERROR_MESSAGE);
            }     
           
        }

        [HttpGet("byId/{postId}")]
        public async Task<ActionResult> GetPostsByIdAsync(Guid postId)
        {
            try
            {
                var posts = await _queryDispatcher.SendAsync(new FindPostByIdQuery { Id = postId });
                if (posts == null || !posts.Any())
                    return NoContent();

                return Ok(new PostLookupResponse
                {
                    Posts = posts,
                    Message = $"Succefully returned post"
                });
            }
            catch (Exception exception)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to retrieve posts by Id!";
                return ErrorResponse(exception, SAFE_ERROR_MESSAGE);
            }

        }

        [HttpGet("byAuthor/{author}")]
        public async Task<ActionResult> GetPostsByAuthorAsync(string author)
        {
            try
            {
                var posts = await _queryDispatcher.SendAsync(new FindPostsByAuthorQuery { Author = author });
                return NormalResponse(posts);
            }
            catch (Exception exception)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to retrieve posts by author!";
                return ErrorResponse(exception, SAFE_ERROR_MESSAGE);
            }

        }

        [HttpGet("withComments")]
        public async Task<ActionResult> GetPostsWithCommentAsync()
        {
            try
            {
                var posts = await _queryDispatcher.SendAsync(new FindPostsWithCommentsQuery());
                return NormalResponse(posts);
            }
            catch (Exception exception)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to retrieve posts with comments!";
                return ErrorResponse(exception, SAFE_ERROR_MESSAGE);
            }

        }

        [HttpGet("withLikes/{numberoflikes}")]
        public async Task<ActionResult> GetPostsWithLikesAsync(int numberoflikes)
        {
            try
            {
                var posts = await _queryDispatcher.SendAsync(new FindPostsWithLikesQuery { NumberOfLikes = numberoflikes });
                return NormalResponse(posts);
            }
            catch (Exception exception)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to retrieve posts with likes!";
                return ErrorResponse(exception, SAFE_ERROR_MESSAGE);
            }

        }

        private ActionResult ErrorResponse(Exception exception, string safeErrorMessage)
        {
            _logger.Log(LogLevel.Error, exception, safeErrorMessage);

            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
            {
                Message = safeErrorMessage
            });
        }

        private ActionResult NormalResponse(List<PostEntity> posts)
        {
            if (posts == null || !posts.Any())
                return NoContent();

            var count = posts.Count();
            return Ok(new PostLookupResponse
            {
                Posts = posts,
                Message = $"Succefully returned {count} post{(count > 1 ? "s" : string.Empty)}"
            });
        }
    }
}
