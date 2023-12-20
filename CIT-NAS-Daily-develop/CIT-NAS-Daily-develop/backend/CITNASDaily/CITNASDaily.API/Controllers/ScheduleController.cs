using CITNASDaily.Entities.Dtos.ScheduleDtos;
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
    public class ScheduleController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IScheduleService _scheduleService;
        private readonly ILogger<ScheduleController> _logger;

        public ScheduleController(IAuthService authService, IScheduleService scheduleService, ILogger<ScheduleController> logger)
        {
            _authService = authService;
            _scheduleService = scheduleService;
            _logger = logger;
        }

        #region CreateSchedule

        /// <summary>
        /// Creates a new Schedule
        /// </summary>
        /// <param name="scheduleCreate">Information of new schedule</param>
        /// <returns>Newly created schedule</returns>
        [HttpPost]
        [Authorize(Roles = "NAS")]
        [ProducesResponseType(typeof(ScheduleDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateNASSchedule([FromBody] ScheduleCreateDto scheduleCreate)
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null)
                {
                    return Forbid();
                }

                var createdSchedule = await _scheduleService.CreateScheduleAsync(scheduleCreate);

                if (createdSchedule == null)
                {
                    return BadRequest("NAS Schedule creation failed.");
                }

                return CreatedAtRoute("GetSchedulesByNASId", new { nasId = createdSchedule.NASId }, createdSchedule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Schedule.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        #endregion

        #region GetSchedule

        /// <summary>
        /// Retrieves list of Schedules by NAS id
        /// </summary>
        /// <param name="nasId">NAS unique identifier</param>
        /// <returns>List of schedules</returns>
        [HttpGet("{nasId}", Name = "GetSchedulesByNASId")]
        [Authorize(Roles = "OAS, NAS, Superior")]
        [ProducesResponseType(typeof(IEnumerable<Schedule>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSchedulesByNASId(int nasId)
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null)
                {
                    return Forbid();
                }

                var schedules = await _scheduleService.GetSchedulesByNASIdAsync(nasId);

                if (schedules == null)
                {
                    return NotFound($"No Schedules found for NAS ID #{nasId}");
                }

                return Ok(schedules);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve Schedules.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        /// <summary>
        /// Retrieves schedules by NAS id, year, and semester
        /// </summary>
        /// <param name="nasId">NAS unique identifier</param>
        /// <param name="year">Year of NAS</param>
        /// <param name="semester">Semester of NAS</param>
        /// <returns>Requested Schedule entry</returns>
        [HttpGet("{nasId}/{year}/{semester}", Name = "GetSchedulesByNASIdSYSemester")]
        [Authorize(Roles = "OAS, NAS, Superior")]
        [ProducesResponseType(typeof(ScheduleListDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSchedulesByNASIdSYSemester(int nasId, int year, int semester)
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

                var schedule = await _scheduleService.GetSchedulesByNASIdSYSemesterAsync(nasId, year, (Semester)semester);

                if (schedule == null)
                {
                    return NotFound($"No schedule found for NAS ID #{nasId} from the specified year and semester.");
                }

                return Ok(schedule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve Schedule.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        #endregion

        #region DeleteSchedule

        /// <summary>
        /// Deletes Schedule with the specified NAS id
        /// </summary>
        /// <param name="nasId">NAS unique identifier</param>
        /// <returns>Successful deletion message</returns>
        [HttpDelete]
        [Authorize(Roles = "OAS, NAS")]
        [ProducesResponseType(typeof(String), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteScheduleByNASIdAsync(int nasId)
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);

                if (currentUser == null)
                {
                    return Forbid();
                }

                var checkSched = await _scheduleService.GetSchedulesByNASIdAsync(nasId);

                if (checkSched == null)
                {
                    return NotFound($"No Schedule found for NAS ID #{nasId}");
                }

                await _scheduleService.DeleteSchedulesByNASIdAsync(nasId);
                return Ok($"Schedules with NAS Id {nasId} deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete NAS Schedule.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        #endregion
    }
}
