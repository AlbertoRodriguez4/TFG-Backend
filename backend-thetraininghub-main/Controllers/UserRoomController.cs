using Microsoft.AspNetCore.Mvc;
using AA2_CS.Model;
using AA2_CS.Service;
using System;
using System.Threading.Tasks;

namespace AA2_CS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoomController : ControllerBase
    {
        private readonly UserRoomService _userRoomService;

        public UserRoomController(UserRoomService userRoomService)
        {
            _userRoomService = userRoomService;
        }

        // GET: api/UserRoom
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var result = _userRoomService.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/UserRoom/user/5
        // Devuelve todas las salas donde está el usuario 5
        [HttpGet("user/{userId}")]
        public IActionResult GetByUserId(int userId)
        {
            try
            {
                var result = _userRoomService.FindByUserId(userId);
                // Retornamos OK aunque la lista esté vacía (significa que el usuario no tiene salas)
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/UserRoom/room/10
        // Devuelve todos los usuarios que están en la sala 10
        [HttpGet("room/{roomId}")]
        public IActionResult GetByRoomId(int roomId)
        {
            try
            {
                var result = _userRoomService.FindUsersByRoomId(roomId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/UserRoom
        [HttpPost]
        public IActionResult Add([FromBody] UserRoom userRoom)
        {
            try
            {
                // Opcional: Validar si la relación ya existe antes de agregar
                var existing = _userRoomService.FindByCompositeKey(userRoom.userid, userRoom.roomid);
                if (existing != null)
                {
                    return BadRequest("User is already in this room.");
                }

                var result = _userRoomService.Add(userRoom);
                return result > 0 ? Ok(userRoom) : BadRequest("Failed to add user to room");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/UserRoom/5/10
        // Necesitamos ambos IDs para identificar qué registro actualizar
        [HttpPut("{userId}/{roomId}")]
        public async Task<IActionResult> Update(int userId, int roomId, [FromBody] UserRoom updatedUserRoom)
        {
            try
            {
                // Validar que los IDs de la URL coincidan con el cuerpo (opcional, pero recomendado)
                if (userId != updatedUserRoom.userid || roomId != updatedUserRoom.roomid)
                {
                    return BadRequest("IDs in URL do not match IDs in body.");
                }

                var result = await _userRoomService.Update(userId, roomId, updatedUserRoom);
                
                if (result == null)
                {
                    return NotFound("UserRoom relationship not found.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/UserRoom/5/10
        [HttpDelete("{userId}/{roomId}")]
        public IActionResult Delete(int userId, int roomId)
        {
            try
            {
                var result = _userRoomService.Delete(userId, roomId);
                
                if (result == 0)
                {
                    return NotFound("UserRoom relationship not found or could not be deleted.");
                }

                return Ok("Deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}