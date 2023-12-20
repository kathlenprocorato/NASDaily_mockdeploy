using CITNASDaily.Entities.Dtos.SuperiorDtos;
using CITNASDaily.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace CITNASDaily.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperiorsController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ISuperiorService _superiorService;
        private readonly ILogger<SuperiorsController> _logger;

        public SuperiorsController(IAuthService authService, ISuperiorService superiorService, ILogger<SuperiorsController> logger)
        {
            _authService = authService;
            _superiorService = superiorService;
            _logger = logger;
        }

        #region CreateSuperior

        /// <summary>
        /// Creates a new Superior entry.
        /// </summary>
        /// <param name="superiorCreate">Information of Superior</param>
        /// <returns>Newly created Superior</returns>
        [HttpPost]
        [Authorize(Roles = "OAS")]
        [ProducesResponseType(typeof(SuperiorDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateSuperior([FromBody] SuperiorCreateDto superiorCreate)
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null)
                {
                    return Forbid();
                }

                var createdSuperior = await _superiorService.CreateSuperiorAsync(superiorCreate.Username, superiorCreate);

                if (createdSuperior == null)
                {
                    return BadRequest("Superior creation failed.");
                }

                return CreatedAtRoute("GetSuperior", new { superiorId = createdSuperior.Id }, createdSuperior);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Superior.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        #endregion

        #region GetSuperior

        /// <summary>
        /// Retrieves all Superior entries.
        /// </summary>
        /// <returns>List of Superior entries.</returns>
        [HttpGet]
        [Authorize(Roles = "OAS")]
        [ProducesResponseType(typeof(IEnumerable<SuperiorDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSuperiors()
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null)
                {
                    return Forbid();
                }

                var superiors = await _superiorService.GetSuperiorsAsync();
                if (superiors.IsNullOrEmpty())
                {
                    return NotFound("No Superiors found.");
                }

                return Ok(superiors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve superiors.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        /// <summary>
        /// Retrieve Superior by id
        /// </summary>
        /// <param name="superiorId">Superior unique identifier</param>
        /// <returns>Requested Superior entry.</returns>
        [HttpGet("{superiorId}", Name = "GetSuperior")]
        [Authorize(Roles = "OAS, Superior")]
        [ProducesResponseType(typeof(SuperiorDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSuperior(int superiorId)
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null)
                {
                    return Forbid();
                }

                var superior = await _superiorService.GetSuperiorAsync(superiorId);

                if (superior == null)
                {
                    return NotFound($"Superior #{superiorId} does not exist.");
                }

                return Ok(superior);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve Superior.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        /// <summary>
        /// Retrieves Superior by Office id.
        /// </summary>
        /// <param name="officeId">Office unique identifier</param>
        /// <returns>Requested Superior entry.</returns>
        [HttpGet("{officeId}/office", Name = "GetSuperiorByOfficeId")]
        [Authorize(Roles = "OAS, Superior")]
        [ProducesResponseType(typeof(SuperiorDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSuperiorByOfficeId(int officeId)
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null)
                {
                    return Forbid();
                }

                var superior = await _superiorService.GetSuperiorByOfficeId(officeId);

                if (superior == null)
                {
                    return NotFound("Superior not found");
                }

                return Ok(superior);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve Superior.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        /// <summary>
        /// Retrieves Superior id by username.
        /// </summary>
        /// <param name="username">Username of Superior</param>
        /// <returns>Requested Superior Id</returns>
        [HttpGet("{username}/id", Name = "GetSuperiorIdByUsername")]
        [Authorize(Roles = "OAS, Superior")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSuperiorIdByUsername(string username)
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null)
                {
                    return Forbid();
                }

                var nasId = await _superiorService.GetSuperiorIdByUsernameAsync(username);
                if (nasId == 0)
                {
                    return NotFound("Superior not found.");
                }

                return Ok(nasId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve Superior id.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        #endregion

        #region UpdateSuperior

        /// <summary>
        /// Updates Superior Password
        /// </summary>
        /// <param name="superiorId"></param>
        /// <param name="currentPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns>A boolean whether the password change was successful or not</returns>
        [HttpPut("changepassword/{superiorId}", Name = "ChangeSuperiorPassword")]
        [Authorize(Roles = "Superior")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangePassword(int superiorId, string currentPassword, string newPassword)
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null)
                {
                    return Forbid();
                }
                var change = await _superiorService.ChangePasswordAsync(superiorId, currentPassword, newPassword);
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
