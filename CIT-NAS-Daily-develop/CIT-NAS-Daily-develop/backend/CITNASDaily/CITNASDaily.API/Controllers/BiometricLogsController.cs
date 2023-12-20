using CITNASDaily.Entities.Dtos.BiometricLogDtos;
using CITNASDaily.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace CITNASDaily.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BiometricLogsController : ControllerBase
    {
        private readonly IBiometricLogService _logService;
        private readonly IAuthService _authService;
        private readonly INASService _nasService;
        private readonly ILogger<BiometricLogsController> _logger;

        public BiometricLogsController(
            IBiometricLogService logService, 
            IAuthService authService, 
            INASService nasService, 
            ILogger<BiometricLogsController> logger)
        {
            _nasService = nasService;
            _authService = authService;
            _logService = logService;
            _logger = logger;
        }

        /// <summary>
        /// Gets all logs for the nas
        /// </summary>
        /// <returns>A list of logs</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/logs
        /// 
        /// </remarks>
        /// <response code="200">Returns a list of logs</response>
        /// <response code="204">No logs found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [Authorize(Roles = "OAS")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<BiometricLogDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<BiometricLogDto>>> GetNASLogs(int nasId)
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null) return Forbid();

                var logs = await _logService.GetNASLogsAsync(nasId);
                if (logs.IsNullOrEmpty())
                {
                    return NotFound();
                }

                return Ok(logs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting logs.");

                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        /// <summary>
        /// Creates a new log
        /// </summary>
        /// <param name="logCreate"></param>
        /// <returns>The newly created log</returns>
        /// <response code="201">Returns the newly created log</response>
        /// <response code="400">Log details are invalid</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Authorize(Roles = "OAS")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(BiometricLogDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateLog([FromBody] BiometricLogCreateDto logCreate)
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null) return Forbid();

                var createdLog = await _logService.CreateLogAsync(logCreate.NASId, logCreate);

                return CreatedAtRoute("GetLog", new { enNo = createdLog.EnNo }, createdLog);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating log.");

                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }
    }
}
