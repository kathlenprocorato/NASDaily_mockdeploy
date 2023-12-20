using CITNASDaily.Entities.Dtos.NASDtos;
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
    public class NASController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly INASService _nasService;
        private readonly ISuperiorEvaluationRatingService _superiorEvaluationRatingService;
        private readonly ISummaryEvaluationService _summaryEvaluationService;
        private readonly ILogger<NASController> _logger;

        public NASController(IAuthService authService, INASService nasService, ISuperiorEvaluationRatingService superiorEvaluationRatingService, ISummaryEvaluationService summaryEvaluationService, ILogger<NASController> logger)
        {
            _authService = authService;
            _nasService = nasService;
            _superiorEvaluationRatingService = superiorEvaluationRatingService;
            _summaryEvaluationService = summaryEvaluationService;
            _logger = logger;
        }

        #region CreateNAS

        /// <summary>
        /// Creates new NAS entry
        /// </summary>
        /// <param name="nasCreate"></param>
        /// <returns>Newly created NAS</returns>
        [HttpPost]
        [Authorize(Roles = "OAS")]
        [ProducesResponseType(typeof(NASDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateNAS([FromBody] NASCreateDto nasCreate)
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null)
                {
                    return Forbid();
                }

                var createdNas = await _nasService.CreateNASAsync(nasCreate.Username, nasCreate);

                if (createdNas == null)
                {
                    return BadRequest("NAS creation failed");
                }

                return CreatedAtRoute("GetNAS", new { nasId = createdNas.Id }, createdNas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating NAS.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        #endregion

        #region GetNAS

        /// <summary>
        /// Retrieves NAS by id
        /// </summary>
        /// <param name="nasId"></param>
        /// <returns>Requested NAS</returns>
        [HttpGet("{nasId}", Name = "GetNAS")]
        [Authorize(Roles = "OAS, NAS, Superior")]
        [ProducesResponseType(typeof(NASDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetNAS(int nasId)
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null) return Forbid();

                // Pass the username from the API request
                var nas = await _nasService.GetNASAsync(nasId);

                if (nas == null)
                {
                    return NotFound("NAS not found");
                }

                return Ok(nas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting NAS.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        /// <summary>
        /// Retrieves NAS by id without image
        /// </summary>
        /// <param name="nasId"></param>
        /// <returns>Requested NAS</returns>
        [HttpGet("{nasId}/noimg", Name = "GetNASNoImage")]
        [Authorize(Roles = "OAS, NAS, Superior")]
        [ProducesResponseType(typeof(NASDtoNoImage), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetNASNoImage(int nasId)
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null) return Forbid();

                // Pass the username from the API request
                var nas = await _nasService.GetNASNoImageAsync(nasId);

                if (nas == null)
                {
                    return NotFound("NAS not found");
                }

                return Ok(nas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting NAS.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        /// <summary>
        /// Retrieves all NAS
        /// </summary>
        /// <returns>List of all NAS</returns>
        [HttpGet(Name = "GetAllNAS")]
        [Authorize(Roles = "OAS")]
        [ProducesResponseType(typeof(IEnumerable<NASDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllNAS()
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null) return Forbid();

                var nas = await _nasService.GetAllNASAsync();

                if (nas.IsNullOrEmpty())
                {
                    return NotFound("There is no registered NAS.");
                }

                return Ok(nas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting list of NAS.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        /// <summary>
        /// Retrieves all NAS without image
        /// </summary>
        /// <returns>List of all NAS without image</returns>
        [HttpGet("noimg", Name = "GetAllNASNoImage")]
        [Authorize(Roles = "OAS, Superior")]
        [ProducesResponseType(typeof(IEnumerable<NASDtoNoImage>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllNASNoImage()
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null) return Forbid();

                var nas = await _nasService.GetAllNASNoImageAsync();

                if (nas.IsNullOrEmpty())
                {
                    return NotFound("There is no registered NAS.");
                }

                return Ok(nas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting list of NAS.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        /// <summary>
        /// Retrieves all NAS by SY and semester
        /// </summary>
        /// <param name="year"></param>
        /// <param name="semester"></param>
        /// <returns>List of NAS by SY and Semester</returns>
        [HttpGet("{year}/{semester}/noimg", Name = "GetAllNASBySYSemester")]
        [Authorize(Roles = "OAS")]
        [ProducesResponseType(typeof(IEnumerable<NASDtoNoImage>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllNASBySYSemester(int year, int semester)
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null) return Forbid();

                var nas = await _nasService.GetAllNasBySYSemesterAsync(year, (Semester) semester);

                if (nas.IsNullOrEmpty())
                {
                    return NotFound("There is no registered NAS.");
                }

                return Ok(nas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting list of NAS.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        /// <summary>
        /// Retrieves NAS id by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Requested NAS id</returns>
        [HttpGet("{username}/id", Name = "GetNASId")]
        [Authorize(Roles = "OAS, NAS")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetNASIdAsync(string username)
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null) return Forbid();

                var nasId = await _nasService.GetNASIdByUsernameAsync(username);

                if (nasId == 0)
                {
                    return NotFound("NAS does not exist.");
                }

                return Ok(nasId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting NAS Id");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        /// <summary>
        /// Retrieves NAS by office id
        /// </summary>
        /// <param name="officeId"></param>
        /// <param name="year"></param>
        /// <param name="semester"></param>
        /// <returns>List of NAS by office id</returns>
        [HttpGet("{officeId}/{year}/{semester}", Name = "GetNASByOfficeIdSYSemester")]
        [Authorize(Roles = "OAS, Superior")]
        [ProducesResponseType(typeof(IEnumerable<NasByOfficeIdListDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetNASByOfficeIdSYSemester(int officeId, int year, int semester)
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null) return Forbid();

                var nas = await _nasService.GetNASByOfficeIdSYSemesterAsync(officeId, year, (Semester)semester);

                if (nas == null)
                {
                    return NotFound("No NAS under your office yet.");
                }

                return Ok(nas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting list of NAS.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        /// <summary>
        /// Retrieves Superior Evaluation Rating of NAS
        /// </summary>
        /// <param name="nasId"></param>
        /// <param name="semester"></param>
        /// <param name="year"></param>
        /// <returns>Requested superior evaluation rating</returns>
        [HttpGet("{nasId}/rating", Name = "GetNASSuperiorEvaluationRating")]
        [Authorize(Roles = "OAS, NAS, Superior")]
        [ProducesResponseType(typeof(SuperiorEvaluationRating), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetNASSuperiorEvaluationRatingAsync(int nasId, Semester semester, int year)
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null) return Forbid();

                var superiorEval = await _superiorEvaluationRatingService.GetSuperiorEvaluationRatingByNASIdAndSemesterAndSchoolYearAsync(nasId, semester, year);

                if (superiorEval == null)
                {
                    return NotFound("No Superior Evaluation Rating Yet.");
                }

                return Ok(superiorEval);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting superior evaluation");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        /// <summary>
        /// Retrieves all SY and Semester
        /// </summary>
        /// <returns>List of SY and semester</returns>
        [HttpGet("sysem", Name = "GetAllSYAndSem")]
        [Authorize(Roles = "OAS, NAS, Superior")]
        [ProducesResponseType(typeof(List<NASSYSemOnly>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllSYAndSem()
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null) return Forbid();

                var sysem = await _nasService.GetAllSYAndSem();

                if (sysem == null)
                {
                    return NotFound("No school year and sem.");
                }

                return Ok(sysem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting school year and sem");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        /// <summary>
        /// Retrieves NAS by nas id, year, and semester without image
        /// </summary>
        /// <param name="nasId"></param>
        /// <param name="year"></param>
        /// <param name="semester"></param>
        /// <returns>Requested NAS without image</returns>
        [HttpGet("{nasId}/{year}/{semester}/noimg", Name = "GetNASByNASIdSYSemesterNoImg")]
        [Authorize(Roles = "OAS, NAS, Superior")]
        [ProducesResponseType(typeof(NASDtoNoImage), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetNASByNASIdSYSemesterNoImg(int nasId, int year, int semester)
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null) return Forbid();

                var nas = await _nasService.GetNASByNASIdSYSemesterNoImgAsync(nasId, year, (Semester)semester);

                if(nas == null)
                {
                    return NotFound($"NAS #{nasId} is not enrolled for the specified semester of the school year {year}");
                }

                return Ok(nas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting NAS");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        /// <summary>
        /// Retrieves a list of enrolled SY and semester by NAS id
        /// </summary>
        /// <param name="nasId"></param>
        /// <returns>List of semester and SY</returns>
        [HttpGet("sysem/{nasId}", Name = "GetSYSemByNASId")]
        [Authorize(Roles = "OAS, NAS, Superior")]
        [ProducesResponseType(typeof(IEnumerable<NASSYSemOnly>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSYSemByNASId(int nasId)
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null) return Forbid();

                var check = await _nasService.GetNASAsync(nasId);
                if(check == null)
                {
                    return BadRequest($"NAS #{nasId} does not exist.");
                }

                var nas = await _nasService.GetSYSemByNASIdAsync(nasId);
                if (nas.IsNullOrEmpty())
                {
                    return NotFound($"NAS #{nasId} is not enrolled.");
                }

                return Ok(nas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting NAS");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        #endregion

        #region UpdateUpload

        /// <summary>
        /// Upload grades by year and semester
        /// </summary>
        /// <param name="nasId"></param>
        /// <param name="year"></param>
        /// <param name="semester"></param>
        /// <param name="file"></param>
        /// <returns>NAS Grade</returns>
        [HttpPut("grades/{nasId}/{year}/{semester}", Name = "UploadGrades")]
        [Authorize(Roles = "OAS, NAS")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UploadGrades(int nasId, int year, int semester, [FromForm] IFormFile file)
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null)
                {
                    return Forbid();
                }

                var nasGrades = await _summaryEvaluationService.UploadGrades(nasId, year, (Semester)semester, file);

                if (nasGrades == null)
                {
                    return BadRequest("Upload Failed");
                }

                return Ok(new { Grade = nasGrades.AcademicPerformance });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading grades.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        /// <summary>
        /// Uploads photo of NAS
        /// </summary>
        /// <param name="nasId"></param>
        /// <param name="file"></param>
        /// <returns>NAS image</returns>
        [HttpPut("photo/{nasId}", Name = "UploadPhoto")]
        [Authorize(Roles = "OAS, NAS")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UploadPhoto(int nasId, [FromForm] IFormFile file)
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null)
                {
                    return Forbid();
                }

                var result = await _nasService.UploadPhotoAsync(nasId, file);

                if(result == null)
                {
                    return BadRequest("Upload Failed");
                }

                return Ok(new { Image = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading photo.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        /// <summary>
        /// Updates NAS entry
        /// </summary>
        /// <param name="nasId"></param>
        /// <param name="nasUpdate"></param>
        /// <returns>Newly updated NAS</returns>
        [HttpPut("{nasId}", Name = "UpdateNAS")]
        [Authorize(Roles = "OAS")]
        [ProducesResponseType(typeof(NASDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateNAS(int nasId, [FromBody] NASUpdateDto nasUpdate)
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null)
                {
                    return Forbid();
                }
                var nas = await _nasService.UpdateNASAsync(nasId, nasUpdate);
                if (nas == null)
                {
                    return BadRequest("Failed to Update NAS");
                }
                return Ok(nas);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating nas.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        /// <summary>
        /// Change NAS password
        /// </summary>
        /// <param name="nasId"></param>
        /// <param name="currentPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns>A boolean whether the password change was successful or not</returns>
        [HttpPut("changepassword/{nasId}", Name = "ChangePassword")]
        [Authorize(Roles = "NAS")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangePassword(int nasId, string currentPassword, string newPassword)
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null)
                {
                    return Forbid();
                }
                var change = await _nasService.ChangePasswordAsync(nasId, currentPassword, newPassword);
                if (change == false)
                {
                    return BadRequest("Failed to Change Password.");
                }
                return Ok(true);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing password.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        #endregion
    }
}
