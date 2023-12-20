using CITNASDaily.Entities.Models;
using CITNASDaily.Entities.Dtos.DailyTimeRecordDto;
using CITNASDaily.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using System.Security.Claims;
using static CITNASDaily.Entities.Enums.Enums;

namespace CITNASDaily.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DTRController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IDTRService _dtrService;
        private readonly ILogger<DTRController> _logger;

        public DTRController(IAuthService authService, IDTRService dtrService, ILogger<DTRController> logger)
        {
            _authService = authService;
            _logger = logger;
            _dtrService = dtrService;
        }

        /// <summary>
        /// Retrieves all Daily Time Records
        /// </summary>
        /// <returns>List of all Daily Time Records</returns>
        [HttpGet(Name = "GetAllDTR")]
        [Authorize(Roles = "OAS, Superior")]
        [ProducesResponseType(typeof(IEnumerable<DailyTimeRecord>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllDTR()
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null)
                {
                    return Forbid();
                }

                var dtr = await _dtrService.GetAllDTRAsync();

                if (dtr.IsNullOrEmpty())
                {
                    return NotFound("There are no DTRs.");
                }

                return Ok(dtr);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting DTR.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        /// <summary>
        /// Retrieves Daily Time Record by year, semester, first name, last name, and middle name
        /// </summary>
        /// <param name="year"></param>
        /// <param name="semester"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="middleName"></param>
        /// <returns>Requested Daily Time Record</returns>
        [HttpGet("{year}/{semester}/{firstName}/{lastName}", Name = "GetAllDTRBySYSem")]
        [Authorize(Roles = "OAS, NAS, Superior")]
        [ProducesResponseType(typeof(DailyTimeRecordListDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllDTRBySYSem(int year, int semester, string firstName, string lastName, [FromQuery] string middleName = "")
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null)
                {
                    return Forbid();
                }

                var dtr = await _dtrService.GetDTRsBySYSemesterAsync(year, (Semester)semester, firstName, lastName, middleName);

                if (dtr == null)
                {
                    return NotFound("There are no DTRs.");
                }

                return Ok(dtr);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting DTR.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        /// <summary>
        /// Uploads a Daily Time Record with year and semester
        /// </summary>
        /// <param name="file"></param>
        /// <param name="year"></param>
        /// <param name="semester"></param>
        /// <returns></returns>
        [HttpPost("UploadExcel/{year}/{semester}")]
        [Authorize(Roles = "OAS")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UploadExcel(IFormFile file, int year, int semester)
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null)
                {
                    return Forbid();
                }

                await _dtrService.SaveDTRs(file, year, (Semester)semester);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving DTR.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        /// <summary>
        /// Retrieves Daily Time Record by name
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="middleName"></param>
        /// <returns>Requested Daily Time Record</returns>
        [HttpGet("GetByNasName/{firstName}/{lastName}")]
        [Authorize(Roles = "OAS, Superior")]
        [ProducesResponseType(typeof(IEnumerable<DailyTimeRecord>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByNasName(string firstName, string lastName, [FromQuery] string middleName = "")
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null)
                {
                    return Forbid();
                }

                var fullName = string.IsNullOrEmpty(middleName) ? $"{firstName} {lastName}" : $"{firstName} {middleName} {lastName}";
                var dtr = await _dtrService.GetDTRByNasNameAsync(firstName, lastName, middleName);

                if (dtr == null || !dtr.Any())
                {
                    return NotFound($"There are no DTRs for NasName: {fullName}.");
                }

                return Ok(dtr);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting DTR by NasName.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

    }
}
