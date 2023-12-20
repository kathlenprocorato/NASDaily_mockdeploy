using CITNASDaily.Entities.Dtos.ActivitiesSummaryDtos;
using CITNASDaily.Entities.Models;
using CITNASDaily.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static CITNASDaily.Entities.Enums.Enums;

namespace CITNASDaily.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesSummaryController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IActivitiesSummaryService _activitiesSummaryService;
        private readonly ILogger<ActivitiesSummaryController> _logger;

        public ActivitiesSummaryController(IAuthService authService, IActivitiesSummaryService activitiesSummaryervice, ILogger<ActivitiesSummaryController> logger)
        {
            _authService = authService;
            _activitiesSummaryService = activitiesSummaryervice;
            _logger = logger;
        }

        #region CreateActivitiesSummary

        /// <summary>
        /// Creates Activities Summary
        /// </summary>
        /// <param name="activitiesSummaryCreate">Information of Activities Summary</param>
        /// <returns>Newly created activities summary</returns>
        /// <response code="201">Successfully created activities summary</response>
        /// <response code="400">Invalid Activities Summary</response>
        /// <response code="403">Forbidden error</response>
        /// <response code="422">Invalid semester input</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("{nasId}/{year}/{semester}")]
        [Authorize]
        [ProducesResponseType(typeof(ActivitiesSummary), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateActivitiesSummary([FromBody] ActivitiesSummaryCreateDto activitiesSummaryCreate, int nasId, int year, int semester)
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null)
                {
                    return Forbid();
                }

                if (!(Enum.IsDefined(typeof(Semester), semester)))
                {
                    return UnprocessableEntity("Invalid semester input.");
                }

                var createdActivitiesSummary = await _activitiesSummaryService.CreateActivitiesSummaryAsync(activitiesSummaryCreate, nasId, year, (Semester)semester);

                if (createdActivitiesSummary == null)
                {
                    return BadRequest("Activities Summary creation failed.");
                }

                return CreatedAtRoute("GetAllActivitiesSummaryByNASIdYearSemester", new { nasId = createdActivitiesSummary.NASId, year = createdActivitiesSummary.SchoolYear, semester = createdActivitiesSummary.Semester }, createdActivitiesSummary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Activities Summary.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        #endregion

        #region GetActivitiesSummary

        /// <summary>
        /// Retrieves all Activities Summary
        /// </summary>
        /// <returns>List of Activities Summary</returns>
        /// <response code="200">Successfully retrieved created Activities Summary</response>
        /// <response code="403">Forbidden error</response>
        /// <response code="404">No Activity Summaries found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<ActivitiesSummary>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllActivitiesSummary()
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null)
                {
                    return Forbid();
                }
                var activitiesSummaries = await _activitiesSummaryService.GetAllActivitiesSummaryAsync();
                if (activitiesSummaries == null)
                {
                    return NotFound("No Activities Summary found");
                }

                return Ok(activitiesSummaries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve Activities Summary");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        /// <summary>
        /// Retrieves all Activities Summary by NAS id 
        /// </summary>
        /// <param name="nasId">NAS unique identifier</param>
        /// <returns>List of Activities Summary under NAS id</returns>
        /// <response code="200">Successfully retrieved created Activities Summary</response>
        /// <response code="403">Forbidden error</response>
        /// <response code="404">No Activities Summary found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{nasId}", Name = "GetAllActivitiesSummary")]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<ActivitiesSummary>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllActivitiesSummary(int nasId)
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null)
                {
                    return Forbid();
                }
                var actSummaries = await _activitiesSummaryService.GetAllActivitiesSummaryByNASIdAsync(nasId);

                if (actSummaries == null)
                {
                    return NotFound($"No Activities Summary found for NAS ID #{nasId}");
                }

                return Ok(actSummaries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve Activities Summary.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        /// <summary>
        /// Retrieves a list of Activities Summary by nasId, month, and year
        /// </summary>
        /// <param name="nasId"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns>Requested activities summary</returns>
        /// <response code="200">Successfully retrieved created activities summary</response>
        /// <response code="404">act summary not found</response>
        /// <response code="500">Internal server error</response>
        /// <response code="403">Forbidden error</response>
        [HttpGet("GetByMonth/{nasId}/{year}/{month}", Name = "GetAllActivitiesSummaryByNASIdMonthYear")]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<ActivitiesSummary>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllActivitiesSummaryByNASIdMonthYear(int nasId, int month, int year)
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null)
                {
                    return Forbid();
                }
                var actSummaries = await _activitiesSummaryService.GetAllActivitiesSummaryByNASIdMonthYearAsync(nasId, month, year);

                if (actSummaries == null)
                {
                    return NotFound($"No Activities Summary found for NAS id #{nasId} with the specified month and year");
                }

                return Ok(actSummaries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve Activities Summary.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        /// <summary>
        /// Retrieves Activities Summary by NAS id, year, and semester
        /// </summary>
        /// <param name="nasId"></param>
        /// <param name="year"></param>
        /// <param name="semester"></param>
        /// <returns>Requested Activities Summary</returns>
        [HttpGet("{nasId}/{year}/{semester}", Name = "GetAllActivitiesSummaryByNASIdYearSemester")]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<ActivitiesSummary>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllActivitiesSummaryByNASIdYearSemester(int nasId, int year, int semester)
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null)
                {
                    return Forbid();
                }

                if (!(Enum.IsDefined(typeof(Semester), semester)))
                {
                    return UnprocessableEntity("Invalid semester input.");
                }

                var actSummaries = await _activitiesSummaryService.GetAllActivitiesSummaryByNASIdYearSemesterAsync(nasId, year, (Semester)semester);
                if (actSummaries == null)
                {
                    return NotFound($"No Activities Summary found for NAS id #{nasId} with the specified semester and year");
                }

                return Ok(actSummaries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve Activities Summary.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        #endregion
    }
}
