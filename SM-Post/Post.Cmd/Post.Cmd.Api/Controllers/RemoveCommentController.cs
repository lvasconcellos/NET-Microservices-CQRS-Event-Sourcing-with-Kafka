using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Api.Commands;
using Post.Common.DTOs;

namespace Post.Cmd.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RemoveCommentController : ControllerBase
    {
        private readonly ILogger<RemoveCommentController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;

        public RemoveCommentController(ILogger<RemoveCommentController> logger, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveCommentAsync(Guid id, RemoveCommentCommand removeCommentCommand)
        {
            try
            {
                removeCommentCommand.Id = id;

                await _commandDispatcher.SendAsync(removeCommentCommand);

                return Ok(new BaseResponse
                {
                    Message = "Remove comment request completed successufully!"
                });
            }
            catch (InvalidOperationException exception)
            {
                _logger.Log(LogLevel.Warning, exception, "Client made a bad request");
                return BadRequest(new BaseResponse
                {
                    Message = exception.Message,
                });
            }
            catch (AggregateNotFoundException exception)
            {
                _logger.Log(LogLevel.Warning, exception, "Could not retrive aggregate, client passed an incorrect post ID targetting the aggregate!");
                return BadRequest(new BaseResponse
                {
                    Message = exception.Message,
                });
            }
            catch (Exception exception)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to remove comment!";
                _logger.Log(LogLevel.Error, exception, SAFE_ERROR_MESSAGE);

                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    Message = SAFE_ERROR_MESSAGE
                });
            }
        }
    }
}
