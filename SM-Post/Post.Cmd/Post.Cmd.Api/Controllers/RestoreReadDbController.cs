using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Api.Commands;
using Post.Common.DTOs;

namespace Post.Cmd.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RestoreReadDbController : ControllerBase
    {
        private readonly ILogger<RestoreReadDbController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;

        public RestoreReadDbController(ILogger<RestoreReadDbController> logger, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        public async Task<ActionResult> RestorePostsAsync()
        {
            var id = Guid.NewGuid();

            try
            {
                await _commandDispatcher.SendAsync(new RestoreReadDbCommand());

                return StatusCode(StatusCodes.Status201Created, new BaseResponse
                {
                    Message = "Read database restore completed successufully!"
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
            catch (Exception exception)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to restore read database!";
                _logger.Log(LogLevel.Error, exception, SAFE_ERROR_MESSAGE);

                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    Message = SAFE_ERROR_MESSAGE
                });
            }
        }
    }
}
