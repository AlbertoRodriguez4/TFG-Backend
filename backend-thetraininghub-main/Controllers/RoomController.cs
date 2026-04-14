using AA2_CS.Model;
using AA2_CS.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AA2_CS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly RoomService _roomService;
        private readonly NotificationService _notificationService;

        public RoomController(RoomService roomService, NotificationService notificationService)
        {
            _roomService = roomService;
            _notificationService = notificationService;
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateRoomWithUser([FromBody] UserRoomDTO request)
        {
            try
            {
                // request.room ya trae description y date automáticamente del JSON
                var roomId = _roomService.CreateRoomWithUser(request.room, request.userid);

                // Enviar notificación de creación de sala (fire-and-forget)
                _ = System.Threading.Tasks.Task.Run(async () =>
                {
                    try
                    {
                        await _notificationService.SendRoomActivityNotificationIfNeeded(
                            request.userid, request.room.name, "created");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al enviar notificación de sala: {ex.Message}");
                    }
                });

                return Ok(new { roomId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear la sala: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        // [Authorize(Roles = Roles.userStaff)] // Descomenta si usas roles
        public IActionResult UpdateRoom(int id, [FromBody] Room room)
        {
            try
            {
                if (room == null || room.id != id)
                    return BadRequest("Room data is invalid.");

                // room trae los nuevos campos actualizados
                var result = _roomService.Update(room);
                return result > 0 ? Ok(room) : NotFound("Room not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar la sala: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        // [Authorize(Roles = Roles.userStaff)]
        public IActionResult DeleteRoom(int id)
        {
            try
            {
                var room = _roomService.FindById(id);
                if (room == null)
                    return NotFound("Room not found.");

                var result = _roomService.Delete(room);
                return result > 0 ? NoContent() : BadRequest("Failed to delete room.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar la sala: {ex.Message}");
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAllRooms()
        {
            try
            {
                var rooms = _roomService.FindAll();
                return Ok(rooms);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener las salas: {ex.Message}");
            }
        }
        
        // Endpoint extra si quieres traer salas + usuarios en una sola llamada
        [HttpGet("with-users")]
        [Authorize]
        public IActionResult GetAllRoomsWithUsers()
        {
            try
            {
                var result = _roomService.FindAllWithUsers();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener salas con usuarios: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetRoomById(int id)
        {
            try
            {
                var room = _roomService.FindById(id);
                return room != null ? Ok(room) : NotFound("Room not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener la sala: {ex.Message}");
            }
        }

        [HttpGet("search/{name}")]
        [Authorize]
        public IActionResult FindRoomsByName(string name)
        {
            try
            {
                var rooms = _roomService.FindByCharacteristic(name);
                return Ok(rooms);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al buscar salas: {ex.Message}");
            }
        }

        [HttpGet("sort-level-asc")]
        [Authorize]
        public IActionResult SortRoomsByLevelAsc()
        {
            try
            {
                var rooms = _roomService.SortByLevelAsc();
                return Ok(rooms);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("sort-level-desc")]
        [Authorize]
        public IActionResult SortRoomsByLevelDesc()
        {
            try
            {
                var rooms = _roomService.SortByLevelDesc();
                return Ok(rooms);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("sort-stats-asc")]
        [Authorize]
        public IActionResult SortRoomsByStatsAsc()
        {
            try
            {
                var rooms = _roomService.SortByStatsAsc();
                return Ok(rooms);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("sort-stats-desc")]
        [Authorize]
        public IActionResult SortRoomsByStatsDesc()
        {
            try
            {
                var rooms = _roomService.SortByStatsDesc();
                return Ok(rooms);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}