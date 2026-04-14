using Microsoft.AspNetCore.Mvc;
using AA2_CS.Model;
using AA2_CS.Service;
using Microsoft.AspNetCore.Authorization;
using AA2_CS.Services;
using System.Security.Claims; // Necesario para leer los Claims

namespace AA2_CS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly PurchaseService _purchaseService;
        private readonly AuthService _authService;
        private readonly NotificationService _notificationService;
        private readonly ItemService _itemService;

        // Constructor del controlador, se inyecta el servicio de compras
        public PurchaseController(PurchaseService purchaseService, AuthService authService, NotificationService notificationService, ItemService itemService)
        {
            _purchaseService = purchaseService;
            _authService = authService;
            _notificationService = notificationService;
            _itemService = itemService;
        }

        // Ruta para obtener todas las compras
        [HttpGet]
        [Authorize(Roles = Roles.userMaster)]
        public IActionResult GetAllPurchases()
        {
            try
            {
                var purchases = _purchaseService.FindAll(); // Obtiene todas las compras del servicio
                return Ok(purchases); // Devuelve las compras en formato JSON
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener las compras: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetPurchaseById(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_authService.HasAccessToResource(id, User))
            {
                return Forbid();
            }

            try
            {
                var purchase = _purchaseService.FindByUserId(id);
                if (purchase == null)
                    return NotFound();

                return Ok(purchase);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener la compra: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddPurchase([FromBody] Purchase purchase)
        {
            try
            {
                var item = _itemService.FindById(purchase.itemid);
                var id = _purchaseService.Add(purchase); // Agrega una nueva compra

                // Enviar recibo de compra por email (fire-and-forget)
                if (item != null)
                {
                    _ = System.Threading.Tasks.Task.Run(async () =>
                    {
                        try
                        {
                            await _notificationService.SendPurchaseReceiptIfNeeded(
                                purchase.userid, item.name, item.type, item.bonus, item.price);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error al enviar notificación de compra: {ex.Message}");
                        }
                    });
                }

                return Ok(new { id });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Roles.userMaster)]
        public IActionResult UpdatePurchase(int id, [FromBody] Purchase purchase)
        {
            try
            {
                if (id != purchase.id)
                    return BadRequest("Purchase ID mismatch.");

                int result = _purchaseService.Update(purchase); // Actualiza la compra
                if (result > 0)
                    return Ok(purchase);

                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar la compra: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.userMaster)]
        public IActionResult DeletePurchase(int id)
        {
            try
            {
                var purchase = _purchaseService.FindById(id); // Busca la compra por ID
                if (purchase == null)
                    return NotFound(); // Si no se encuentra, retorna 404

                int result = _purchaseService.Delete(purchase); // Elimina la compra
                if (result > 0)
                    return Ok();

                return BadRequest("Failed to delete purchase.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar la compra: {ex.Message}");
            }
        }

        [HttpGet("my-purchases")] // Ruta limpia, sin parámetros sensibles
        [Authorize] // Esto asegura que solo entre alguien con Token válido
        public IActionResult GetMyPurchases()
        {
            try
            {
                // 1. Extraer el ID del usuario directamente del Token
                // El ClaimTypes.NameIdentifier suele ser donde se guarda el ID en el JWT
                var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Si por alguna razón el token no tiene ID (raro si está autorizado)
                if (string.IsNullOrEmpty(userIdString))
                {
                    return Unauthorized("Token inválido: No contiene ID de usuario.");
                }

                int userId = int.Parse(userIdString);

                // 2. Llamar al servicio usando SOLO el ID (ya no hace falta password)
                var purchases = _purchaseService.FindByUserId(userId);

                return Ok(purchases);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener compras: {ex.Message}");
            }
        }
    }

}