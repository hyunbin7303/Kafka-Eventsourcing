using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Post.Common.DTOs;
using Post.Query.Api.DTOs;
using Post.Query.Api.Queries;
using Post.Query.Domain.Entities;

namespace Post.Query.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
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
                var posts = await _queryDispatcher.SendAsync(new FindAllPostsQuery());
                return NormalResponse(posts);
            }
            catch (Exception ex)
            {
                const string SAFE_ERR_MEG = "Error while processing request to retrieve all posts!";
                return ErrorResponse(ex, SAFE_ERR_MEG);
            }

        }
        [HttpGet("byId/{postId}")]
        public async Task<ActionResult> GetByPostIdAsync(Guid postId)
        {
            try
            {
                var posts = await _queryDispatcher.SendAsync(new FindPostByIdQuery { Id = postId });
                if (posts == null || !posts.Any())
                    return NoContent();

                return Ok(new PostLookupResponse { Posts = posts, Message = "Successfully returned post!" });
            }
            catch (Exception ex)
            {
                const string SAFE_ERR_MEG = "Error while processing request to Find by PostID!";
                return ErrorResponse(ex, SAFE_ERR_MEG);
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
            catch (Exception ex)
            {
                const string SAFE_ERR_MEG = "Error while processing request to find post by Author!";
                return ErrorResponse(ex, SAFE_ERR_MEG);
            }
        }

        [HttpGet("withComments")]
        public async Task<ActionResult> GetPostsWithCommentsAsync()
        {
            try
            {
                var posts = await _queryDispatcher.SendAsync(new FindPostsWithCommentsQuery());
                return NormalResponse(posts);
            }
            catch (Exception ex)
            {
                const string SAFE_ERR_MEG = "Error while processing request to find posts with Comments!";
                return ErrorResponse(ex, SAFE_ERR_MEG);
            }
        }
        [HttpGet("withLikes/{numberOfLikes}")]
        public async Task<ActionResult> GetPostsWithLikesAsync(int numberOfLikes)
        {
            try
            {
                var posts = await _queryDispatcher.SendAsync(new FindPostsWithLikesQuery { NumberOfLikes = numberOfLikes });
                return NormalResponse(posts);
            }
            catch (Exception ex)
            {
                const string SAFE_ERR_MEG = "Error while processing request to find posts with Likes!";
                return ErrorResponse(ex, SAFE_ERR_MEG);
            }
        }
        private ActionResult NormalResponse(List<PostEntity> posts)
        {
            if (posts == null || !posts.Any())
                return NoContent();
            var count = posts.Count;
            return Ok(new PostLookupResponse
            {
                Posts = posts,
                Message = $"Successfully resturned {count} post {(count > 1 ? "s" : string.Empty)}!"
            });
        }

        private ActionResult ErrorResponse(Exception ex, string safeErrorMsg)
        {
            _logger.LogError(ex, safeErrorMsg);
            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
            {
                Message = safeErrorMsg
            });
        }
    }
}
