using Microsoft.AspNetCore.Mvc;
using AA2_CS.Model;
using AA2_CS.Service;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
namespace AA2_CS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }


        [HttpPost]
        // [Authorize]
        public IActionResult AddUser([FromBody] User user)
        {
            try
            {
                var result = _userService.Add(user);
                return result > 0 ? Ok(user) : BadRequest("Failed to add user");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al agregar usuario: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [Authorize] 
            public async Task<ActionResult<User>> UpdateUser(int id, User user)
        {
            Console.WriteLine($"Received update request for user ID: {id}");
            Console.WriteLine($"User data: {System.Text.Json.JsonSerializer.Serialize(user)}");
            try
            {
                var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

                bool isSelfUpdate = currentUserIdClaim == id.ToString();
                bool isAdmin = currentUserRole == Roles.userMaster;

                if (!isSelfUpdate && !isAdmin)
                {
                    return Forbid(); 
                }

                var result = await _userService.UpdateById(id, user);
                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar usuario: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.userMaster)]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                var user = _userService.FindById(id);
                if (user == null) return NotFound();

                var result = _userService.Delete(user);
                return result > 0
                    ? Ok(new { message = "User deleted successfully" })
                    : BadRequest("Failed to delete user");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar usuario: {ex.Message}");
            }
        }

        [HttpGet]
        [Authorize(Roles = Roles.userMaster)]
        public IActionResult GetAllUsers()
        {
            try
            {
                Console.WriteLine("hola");
                var users = _userService.FindAll();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener usuarios: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetUserById(int id)
        {
            try
            {
                var user = _userService.FindById(id);
                return user != null ? Ok(user) : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener usuario: {ex.Message}");
            }
        }

        [HttpGet("search/{name}")]
        [Authorize(Roles = Roles.userMaster)]
        public IActionResult FindUsersByCharacteristic(string name)
        {
            try
            {
                var users = _userService.FindByCharacteristic(name);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al buscar usuarios: {ex.Message}");
            }
        }

        [HttpGet("getTopThreeUsers")]
        [Authorize]
        public IActionResult GetTopThreeUsers()
        {
            try
            {
                var users = _userService.GetTopThreeUsers();
                return users != null ? Ok(users) : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener los tres mejores usuarios: {ex.Message}");
            }
        }
        [HttpPost("equip/{userId}/{itemId}")]
        public IActionResult EquipItem(int userId, int itemId)
        {
            var result = _userService.EquipItem(userId, itemId);
            if (result.Contains("not found") || result.Contains("No posees"))
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("unequip/{userId}/{type}")]
        public IActionResult UnequipItem(int userId, string type)
        {
            var result = _userService.UnequipItem(userId, type);
            return Ok(result);
        }
        [HttpPost("change-password")]
        [Authorize] // Solo usuarios logueados
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            try
            {
                // Extraemos el ID del usuario directamente de su Token JWT por seguridad
                var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                
                if (!int.TryParse(userIdStr, out int userId))
                {
                    return Unauthorized("Token inválido o ID de usuario no encontrado.");
                }

                // Llamamos al servicio para hacer el cambio
                bool success = await _userService.ChangePassword(userId, request.CurrentPassword, request.NewPassword);

                if (success)
                {
                    return Ok(new { message = "Contraseña actualizada correctamente." });
                }
                else
                {
                    // Si devuelve false, es porque la contraseña actual era incorrecta (o el usuario no existe)
                    return BadRequest("La contraseña actual proporcionada es incorrecta.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno al cambiar la contraseña: {ex.Message}");
            }
        }
    }
}