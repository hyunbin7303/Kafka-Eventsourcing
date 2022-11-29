using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Api.Commands;
using Post.Common.DTOs;

namespace Post.Cmd.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class NewPostController : ControllerBase
    {
        private readonly ILogger<NewPostController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;
        public NewPostController(ILogger<NewPostController> logger, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }
        [HttpPost]
        public async Task<ActionResult> NewPostAsync(NewPostCommand cmd)
        {
            var id = Guid.NewGuid(); 
            try
            {
                cmd.Id = id;
                await _commandDispatcher.SendAsync(cmd);
                return StatusCode(StatusCodes.Status201Created, new NewPostResponse
                {
                    Message = "New post creation request completed."
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.Log(LogLevel.Warning, ex, "Client made a bad request");
                return BadRequest(new BaseResponse
                {
                    Message = ex.Message
                });
            }
            catch(Exception ex)
            {
                const string SAFE_ERROR_MSG = "Error while processing request to create a new Post!";
                _logger.Log(LogLevel.Error, ex, SAFE_ERROR_MSG);
                return StatusCode(StatusCodes.Status500InternalServerError, new NewPostResponse
                {
                    Id = id,
                    Message = SAFE_ERROR_MSG
                });
            }
        }
    }
}
