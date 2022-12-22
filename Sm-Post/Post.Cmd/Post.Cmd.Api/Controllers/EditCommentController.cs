using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Api.Commands;
using Post.Common.DTOs;

namespace Post.Cmd.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EditCommentController : ControllerBase
    {
        private readonly ILogger<EditCommentController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;
        public EditCommentController(ILogger<EditCommentController> logger, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> EditCommentAsync(Guid id, EditCommentCommand cmd)
        {
            try
            {
                cmd.Id = id;
                await _commandDispatcher.SendAsync(cmd);

                return Ok(new BaseResponse
                {
                    Message = "Edit comment msg request completed successfully."
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.Log(LogLevel.Warning, ex, "Client made a bad request!");
                return BadRequest(new BaseResponse
                {
                    Message = ex.Message
                });
            }

            catch (AggregateNotFoundException ex)
            {
                _logger.Log(LogLevel.Warning, ex, "Could not retrieve aggregate, client passed a incorrect post ID targetting the aggregate.");
                return BadRequest(new BaseResponse
                {
                    Message = ex.Message
                });
            }
            catch (Exception e)
            {
                const string SAFE_ERROR_MSG = "Error while processing request to edit a comment on a post!";
                _logger.Log(LogLevel.Error, e, SAFE_ERROR_MSG);
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    Message = SAFE_ERROR_MSG
                });
            }

        }
    }
}
