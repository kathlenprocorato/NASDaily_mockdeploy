using CITNASDaily.Entities.Dtos.OfficeDtos;
using CITNASDaily.Entities.Models;
using CITNASDaily.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CITNASDaily.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficesController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IOfficeService _officeService;
        private readonly ILogger<OfficesController> _logger;

        public OfficesController(IAuthService authService, IOfficeService officeService, ILogger<OfficesController> logger)
        {
            _authService = authService;
            _officeService = officeService;
            _logger = logger;
        }

        #region CreateOffice

        /// <summary>
        /// Creates Office entry.
        /// </summary>
        /// <param name="officeCreate">Information of new Office</param>
        /// <returns>Newly created Office</returns>
        [HttpPost]
        [Authorize(Roles = "OAS")]
        [ProducesResponseType(typeof(OfficeDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateOffice([FromBody] OfficeCreateDto officeCreate)
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null)
                {
                    return Forbid();
                }

                var createdOffice = await _officeService.CreateOfficeAsync(officeCreate);

                if (createdOffice == null)
                {
                    return BadRequest("Office creation failed.");
                }

                return CreatedAtRoute("GetOffices", new { officeId = createdOffice.Id }, createdOffice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Office.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        #endregion

        #region GetOffice
        
        /// <summary>
        /// Retrieves a list of Offices.
        /// </summary>
        /// <returns>List of offices</returns>
        [HttpGet(Name = "GetOffices")]
        [Authorize(Roles = "OAS")]
        [ProducesResponseType(typeof(Office), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOffices()
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null)
                {
                    return Forbid();
                }

                var offices = await _officeService.GetOfficesAsync();
                if (offices == null)
                {
                    return NotFound("No offices found.");
                }

                return Ok(offices);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve Offices.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        /// <summary>
        /// Retrieve Office by id
        /// </summary>
        /// <param name="id">Office unique identifier</param>
        /// <returns>Requested office entry</returns>
        [HttpGet("{id}", Name = "GetOfficeById")]
        [Authorize(Roles = "OAS, Superior")]
        public async Task<IActionResult> GetOfficeById(int id)
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null)
                {
                    return Forbid();
                }

                var office = await _officeService.GetOfficeByIdAsync(id);
                if (office == null)
                {
                    return NotFound($"Office #{id} does not exist.");
                }

                return Ok(office);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve Office.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        /// <summary>
        /// Retrieves Office by Superior id.
        /// </summary>
        /// <param name="superiorId">Superior unique identifier</param>
        /// <returns>Requested Office entry</returns>
        [HttpGet("superior/{superiorId}", Name = "GetOfficeBySuperiorId")]
        [Authorize(Roles = "OAS, Superior")]
        [ProducesResponseType(typeof(Office), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOfficeBySuperiorId(int superiorId)
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null)
                {
                    return Forbid();
                }

                var offices = await _officeService.GetOfficeBySuperiorIdAsync(superiorId);
                if (offices == null)
                {
                    return NotFound($"No Office found for Superior id #{superiorId}");
                }

                return Ok(offices);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve Office.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        /// <summary>
        /// Retrieves Office by NAS id
        /// </summary>
        /// <param name="nasId">NAS unique identifier</param>
        /// <returns>Requested Office entry</returns>
        [HttpGet("NAS/{nasId}", Name = "GetOfficeByNASId")]
        [Authorize(Roles = "OAS, Superior, NAS")]
        [ProducesResponseType(typeof(Office), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOfficeByNASId(int nasId)
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null)
                {
                    return Forbid();
                }

                var offices = await _officeService.GetOfficeByNASIdAsync(nasId);
                if (offices == null)
                {
                    return NotFound($"No Office found for NAS id #{nasId}");
                }

                return Ok(offices);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve Office.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        #endregion

        #region UpdateOffice

        /// <summary>
        /// Updates Office entry
        /// </summary>
        /// <param name="office">New information of Office</param>
        /// <returns>Newly updated office</returns>
        [HttpPut]
        [Authorize(Roles = "OAS")]
        [ProducesResponseType(typeof(Office), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateOffice([FromBody] OfficeUpdateDto office)
        {
            try
            {
                var currentUser = _authService.GetCurrentUser(HttpContext.User.Identity as ClaimsIdentity);
                if (currentUser == null)
                {
                    return Forbid();
                }

                var existingOffice = await _officeService.GetOfficeByIdAsync(office.Id);

                if(existingOffice == null)
                {
                    return NotFound($"Office #{office.Id} does not exist.");
                }

                var updatedOffice = await _officeService.UpdateOfficeAsync(office);

                if (updatedOffice == null)
                {
                    return BadRequest("Office update failed.");
                }

                return Ok(updatedOffice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Office.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }

        #endregion
    }
}
