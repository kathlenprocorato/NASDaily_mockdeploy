using CITNASDaily.Entities.Dtos.SummaryEvaluationDtos;
using CITNASDaily.Entities.Dtos.SuperiorEvaluationRatingDto;
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
    public class SuperiorEvaluationRatingController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ISuperiorEvaluationRatingService _superiorEvaluationRatingService;
        private readonly ISummaryEvaluationService _summaryEvaluationService;
        private readonly ILogger<SuperiorEvaluationRatingController> _logger;

        public SuperiorEvaluationRatingController(IAuthService authService, ISuperiorEvaluationRatingService superiorEvaluationRatingervice, ISummaryEvaluationService summaryEvaluationService, ILogger<SuperiorEvaluationRatingController> logger)
        {
            _authService = authService;
            _superiorEvaluationRatingService = superiorEvaluationRatingervice;
            _summaryEvaluationService = summaryEvaluationService;
            _logger = logger;
        }

        #region CreateSuperiorEvaluationRating

        /// <summary>
        /// Creates Superior Evaluation Rating
        /// </summary>
        /// <param name="superiorEvaluationRatingCreate">Information of Superior Evaluation Rating</param>
        /// <returns>Newly created Superior Evaluation Rating</returns>
        [HttpPost]
        [Authorize(Roles = "Superior")]
        [ProducesResponseType(typeof(SuperiorEvaluationRating), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateSuperiorEvaluationRating([FromBody] SuperiorEvaluationRatingCreateDto superiorEvaluationRatingCreate)
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null)
                {
                    return Forbid();
                }

                var rating = await _superiorEvaluationRatingService.CreateSuperiorEvaluationRatingAsync(superiorEvaluationRatingCreate);

                if (rating == null)
                {
                    return BadRequest("Superior Evaluation Rating creation failed.");
                }

                var summary = await _summaryEvaluationService.UpdateSuperiorRatingAsync(rating.NASId, rating.SchoolYear, rating.Semester, rating.OverallRating);

                if (summary == null)
                {
                    return BadRequest("Updating Overall Rating Failed.");
                }

                return CreatedAtRoute("GetSuperiorEvaluationRatingByNASIdAndSemesterAndSchoolYear", new { nasId = rating.NASId, semester = rating.Semester, year = rating.SchoolYear }, rating);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Superior Evaluation Rating.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        #endregion

        #region GetSuperiorEvaluationRating

        /// <summary>
        /// Retrieves Superior Evaluation Rating by NAS id, semester, and year
        /// </summary>
        /// <param name="nasId">nAS unique identifier</param>
        /// <param name="semester">Semester of NAS</param>
        /// <param name="year">Year of NAS</param>
        /// <returns>Requested Superior Evaluation Rating</returns>
        [HttpGet(Name = "GetSuperiorEvaluationRatingByNASIdAndSemesterAndSchoolYear")]
        [Authorize(Roles = "OAS, Superior,NAS")]
        [ProducesResponseType(typeof(SuperiorEvaluationRating), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSuperiorEvaluationRatingByNASIdAndSemesterAndSchoolYear(int nasId, Semester semester, int year)
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null)
                {
                    return Forbid();
                }

                var superiorEvaluationRating = await _superiorEvaluationRatingService.GetSuperiorEvaluationRatingByNASIdAndSemesterAndSchoolYearAsync(nasId, semester, year);
                if (superiorEvaluationRating == null)
                {
                    return NotFound($"No Superior Evaluation Rating found for NAS ID #{nasId}");
                }

                return Ok(superiorEvaluationRating);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve Superior Evaluation Rating.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        #endregion
    }
}
