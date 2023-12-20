using CITNASDaily.Entities.Dtos.NASDtos;
using CITNASDaily.Entities.Dtos.TimekeepingSummaryDtos;
using CITNASDaily.Entities.Models;
using CITNASDaily.Services.Contracts;
using CITNASDaily.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static CITNASDaily.Entities.Enums.Enums;

namespace CITNASDaily.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimekeepingSummaryController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITimekeepingSummaryService _timekeepingSummaryService;
        private readonly ILogger<TimekeepingSummaryController> _logger;
        public TimekeepingSummaryController(IAuthService authService, ITimekeepingSummaryService timekeepingSummaryervice, ILogger<TimekeepingSummaryController> logger)
        {
            _authService = authService;
            _timekeepingSummaryService = timekeepingSummaryervice;
            _logger = logger;
        }

        #region CreateTKSummary

        /// <summary>
        /// Creates a new Timekeeping Summary entry.
        /// </summary>
        /// <param name="timekeepingSummaryCreate">The information to create a timekeeping summary entry.</param>
        /// <param name="nasId">NAS unique identifier</param>
        /// <param name="year">School year of NAS</param>
        /// <param name="semester">Semester of NAS</param>
        /// <returns>Newly created Timekeeping Summary.</returns>
        [HttpPost("{nasId}/{year}/{semester}")]
        [Authorize(Roles = "OAS")]
        [ProducesResponseType(typeof(TimekeepingSummary), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateTimekeepingSummary([FromBody] TimekeepingSummaryCreateDto timekeepingSummaryCreate, int nasId, int year, int semester)
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null)
                {
                    return Forbid();
                }

                if(!(Enum.IsDefined(typeof(Semester), semester)))
                {
                    return UnprocessableEntity("Invalid semester input.");
                }

                var createdTimekeepingSummary = await _timekeepingSummaryService.CreateTimekeepingSummaryAsync(timekeepingSummaryCreate, nasId, year, (Semester)semester);

                if (createdTimekeepingSummary == null)
                {
                    return BadRequest($"Timekeeping Summary creation of NAS id #{nasId} failed.");
                }

                return CreatedAtRoute("GetAllTimekeepingSummaryByNASId", new { nasId = createdTimekeepingSummary.NASId }, createdTimekeepingSummary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Timekeeping Summary.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        #endregion

        #region GetTKSummary

        /// <summary>
        /// Retrieves all Timekeeping Summary entries.
        /// </summary>
        /// <returns>List of all Timekeeping Summary entries</returns>
        [HttpGet(Name = "GetAllTimekeepingSummary")]
        [Authorize(Roles = "OAS")]
        [ProducesResponseType(typeof(IEnumerable<TimekeepingSummary>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllTimekeepingSummary()
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null)
                {
                    return Forbid();
                }

                var tkSummaries = await _timekeepingSummaryService.GetAllTimekeepingSummaryAsync();
                if (tkSummaries == null)
                {
                    return NotFound("No timekeeping summaries found.");
                }

                return Ok(tkSummaries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve Timekeeping Summaries.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        /// <summary>
        /// Retrieves a list of Timekeeping Summary entries by NAS id.
        /// </summary>
        /// <param name="nasId">NAS unique identifier</param>
        /// <returns>List of Timekeeping Summary entries by NAS id.</returns>
        [HttpGet("{nasId}", Name = "GetAllTimekeepingSummaryByNASId")]
        [Authorize(Roles = "OAS, Superior, NAS")]
        [ProducesResponseType(typeof(IEnumerable<TimekeepingSummary>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllTimekeepingSummaryByNASId(int nasId)
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null)
                {
                    return Forbid();
                }

                var tkSummary = await _timekeepingSummaryService.GetAllTimekeepingSummaryByNASIdAsync(nasId);

                if (tkSummary == null)
                {
                    return NotFound($"No timekeeping summary found for NAS ID #{nasId}.");
                }

                return Ok(tkSummary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve Timekeeping Summary.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        /// <summary>
        /// Retrieves a Timekeeping Summary entry by NAS id, semester, and year.
        /// </summary>
        /// <param name="nasId">NAS unique identifier</param>
        /// <param name="semester">NAS semester</param>
        /// <param name="year">NAS school year</param>
        /// <returns>Requested timekeeping summary entry.</returns>
        [HttpGet("{nasId}/{year}/{semester}", Name = "GetTimekeepingSummaryByNASIdSemesterYear")]
        [Authorize(Roles = "OAS, NAS, Superior")]
        [ProducesResponseType(typeof(TimekeepingSummary), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTimekeepingSummaryByNASIdSemesterYear(int nasId, int semester, int year)
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null) return Forbid();

                if (!(Enum.IsDefined(typeof(Semester), semester)))
                {
                    return UnprocessableEntity("Invalid semester input.");
                }

                var tkSummary = await _timekeepingSummaryService.GetTimekeepingSummaryByNASIdSemesterYearAsync(nasId, (Semester)semester, year);

                if (tkSummary == null)
                {
                    return NotFound($"No timekeeping summary found for NAS ID #{nasId}.");
                }

                return Ok(tkSummary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve Timekeeping Summary.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        #endregion

        #region UpdateTKSummary

        /// <summary>
        /// Updates Timekeeping Summary entry based on NAS id, year, and semester.
        /// </summary>
        /// <param name="nasId">NAS unique identifier</param>
        /// <param name="year">NAS school year</param>
        /// <param name="semester">NAS semester</param>
        /// <param name="tkUpdate">New Timekeeping Summary information</param>
        /// <returns>Newly updated Timekeeping Summary entry.</returns>
        [HttpPut("{nasId}/{year}/{semester}", Name = "UpdateTimekeepingSummary")]
        [Authorize(Roles = "OAS")]
        [ProducesResponseType(typeof(TimekeepingSummary), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateTimekeepingSummary(int nasId, int year, int semester, [FromBody] TimekeepingSummaryUpdateDto tkUpdate)
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

                var tkSummary = await _timekeepingSummaryService.UpdateTimekeepingSummaryAsync(nasId, year, (Semester)semester, tkUpdate);

                if (tkSummary == null)
                {
                    return BadRequest("Failed to update timekeeping summary");
                }

                return Ok(tkSummary);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating timekeeping summary.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        #endregion
    }
}
