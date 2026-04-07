using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AA2_CS.Model;
using AA2_CS.Services;

namespace AA2_CS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest login)
        {
            try
            {
                var token = _authService.Login(login.Email, login.Password);

                if (token == null)
                    return Unauthorized("Invalid credentials");

                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            try
            {
                var (registeredUser, token, emailSent) = await _authService.RegisterAsync(user);

                if (!emailSent)
                {
                    // El registro fue exitoso pero no se pudo enviar el email
                    return Ok(new {
                        token,
                        message = "Usuario registrado correctamente, pero no se pudo enviar el correo de verificación",
                        emailSent = false
                    });
                }

                return Ok(new {
                    token,
                    message = "Usuario registrado correctamente. Se ha enviado un código de verificación a tu correo.",
                    emailSent = true,
                    userId = registeredUser.id
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("verify-email")]
        [Authorize]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest request)
        {
            try
            {
                // Obtener el ID del usuario desde el token JWT
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim is null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized("Token inválido");
                }

                var isVerified = await _authService.VerifyEmailAsync(userId, request.Code);

                if (isVerified)
                {
                    return Ok(new { message = "Correo verificado exitosamente" });
                }
                else
                {
                    return BadRequest(new { message = "Código de verificación inválido o expirado" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("resend-verification")]
        [Authorize]
        public async Task<IActionResult> ResendVerification()
        {
            try
            {
                // Obtener el ID del usuario desde el token JWT
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim is null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized("Token inválido");
                }

                var emailSent = await _authService.ResendVerificationEmailAsync(userId);

                if (emailSent)
                {
                    return Ok(new { message = "Se ha reenviado el código de verificación a tu correo" });
                }
                else
                {
                    return BadRequest(new { message = "No se pudo reenviar el código de verificación. Es posible que tu correo ya esté verificado" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
