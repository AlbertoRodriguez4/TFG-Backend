using Microsoft.AspNetCore.Mvc;
using AA2_CS.Model;
using AA2_CS.Service;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = Roles.userMaster)]
        public async Task<ActionResult<User>> UpdateUser(int id, User user)
        {
            try
            {
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
    }
}