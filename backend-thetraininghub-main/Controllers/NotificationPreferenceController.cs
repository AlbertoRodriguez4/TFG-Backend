using AA2_CS.Model;
using AA2_CS.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AA2_CS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationPreferenceController : ControllerBase
    {
        private readonly NotificationPreferenceService _prefService;

        public NotificationPreferenceController(NotificationPreferenceService prefService)
        {
            _prefService = prefService;
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        }

        [HttpGet("my-preferences")]
        public async Task<IActionResult> GetMyPreferences()
        {
            try
            {
                var userId = GetUserId();
                if (userId == 0) return Unauthorized("Token inválido.");

                var dto = await _prefService.GetByUserIdAsync(userId);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener preferencias: {ex.Message}");
            }
        }

        [HttpPut("my-preferences")]
        public async Task<IActionResult> UpdateMyPreferences([FromBody] NotificationPreferenceDTO dto)
        {
            try
            {
                var userId = GetUserId();
                if (userId == 0) return Unauthorized("Token inválido.");

                dto.UserId = userId;
                var result = await _prefService.UpdateAsync(userId, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar preferencias: {ex.Message}");
            }
        }

        [HttpPost("reset")]
        public async Task<IActionResult> ResetDefaults()
        {
            try
            {
                var userId = GetUserId();
                if (userId == 0) return Unauthorized("Token inválido.");

                var result = await _prefService.ResetDefaultsAsync(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al resetear preferencias: {ex.Message}");
            }
        }
    }
}