using CITNASDaily.Entities.Dtos.SummaryEvaluationDtos;
using CITNASDaily.Entities.Models;
using CITNASDaily.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using static CITNASDaily.Entities.Enums.Enums;

namespace CITNASDaily.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SummaryEvaluationController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ISummaryEvaluationService _summaryEvaluationService;
        private readonly ILogger<SuperiorEvaluationRatingController> _logger;

        public SummaryEvaluationController(IAuthService authService, ISummaryEvaluationService summaryEvaluationService, ILogger<SuperiorEvaluationRatingController> logger)
        {
            _authService = authService;
            _summaryEvaluationService = summaryEvaluationService;
            _logger = logger;
        }

        #region CreateSummaryEvaluation

        /// <summary>
        /// Creates a new summary evaluation entry
        /// </summary>
        /// <param name="summary"></param>
        /// <returns>Newly created summary evaluation</returns>
        [HttpPost]
        [Authorize(Roles = "OAS")]
        [ProducesResponseType(typeof(SummaryEvaluation), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateSummaryEvaluation([FromBody] SummaryEvaluationCreateDto summary)
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null)
                {
                    return Forbid();
                }

                var summaryEval = await _summaryEvaluationService.CreateSummaryEvaluationAsync(summary);

                if (summaryEval == null)
                {
                    return BadRequest("Summary Evaluation creation failed.");
                }

                return CreatedAtRoute("GetSummaryEvaluationByNASId", new { nasId = summaryEval.nasId }, summaryEval);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating summary evaluation.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        #endregion

        #region GetSummaryEvaluation

        /// <summary>
        /// Retrieves all summary evaluation
        /// </summary>
        /// <returns>List of summary evaluation</returns>
        [HttpGet]
        [Authorize(Roles = "OAS")]
        [ProducesResponseType(typeof(IEnumerable<SummaryEvaluationDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSummaryEvaluations()
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null)
                {
                    return Forbid();
                }

                var summaryEval = await _summaryEvaluationService.GetSummaryEvaluationsAsync();
                if (summaryEval.IsNullOrEmpty())
                {
                    return NotFound("No Summary Evaluations Found");
                }

                return Ok(summaryEval);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve summary evaluations.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        /// <summary>
        /// Retrieves summary evaluation by nas ID, semester, and year
        /// </summary>
        /// <param name="nasId"></param>
        /// <param name="semester"></param>
        /// <param name="year"></param>
        /// <returns>Requested summary evaluation</returns>
        [HttpGet("{year}/{semester}/{nasId}", Name = "GetSummaryEvaluationByNASIdSemesterYear")]
        [Authorize(Roles = "OAS, Superior, NAS")]
        [ProducesResponseType(typeof(SummaryEvaluationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSummaryEvaluationByNASIdSemesterYear(int nasId, Semester semester, int year)
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null)
                {
                    return Forbid();
                }

                var summaryEval = await _summaryEvaluationService.GetSummaryEvaluationByNASIdSemesterYearAsync(nasId, semester, year);

                if (summaryEval == null)
                {
                    return NotFound($"No Summary Evaluation found for NAS ID #{nasId} with the specified semester and year.");
                }

                return Ok(summaryEval);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve summary evaluation.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        /// <summary>
        /// Retrieves NAS grade picture by nas ID, semester, and year
        /// </summary>
        /// <param name="nasId"></param>
        /// <param name="year"></param>
        /// <param name="semester"></param>
        /// <returns>Requested grade picture</returns>
        [HttpGet("grades/{nasId}/{year}/{semester}", Name = "GetNASGradePicture")]
        [Authorize(Roles = "OAS, Superior")]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetNASGradePicture(int nasId, int year, int semester)
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

                var nasGrades = await _summaryEvaluationService.GetNASGradePicture(nasId, year, (Semester)semester);
                if (nasGrades == null)
                {
                    return NotFound($"No grades found for NAS ID #{nasId} with the specified semester and year");
                }

                return Ok(nasGrades);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retreiving grades.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        #endregion

        #region PutSummaryEvaluation

        /// <summary>
        /// Updates summary evaluation
        /// </summary>
        /// <param name="summary"></param>
        /// <returns>Newly updated summary evaluation</returns>
        [HttpPut]
        [Authorize(Roles = "OAS")]
        [ProducesResponseType(typeof(SummaryEvaluation), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateSummaryEvaluation([FromBody] SummaryEvaluationUpdateDto summary)
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null)
                {
                    return Forbid();
                }

                var summaryEval = await _summaryEvaluationService.UpdateSummaryEvaluationAsync(summary);

                if (summaryEval == null)
                {
                    return BadRequest("Summary Evaluation Update Failed.");
                }

                return Ok(summaryEval);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating summary evaluation.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        #endregion
    }
}
