using Microsoft.AspNetCore.Mvc;
using AA2_CS.Model;
using AA2_CS.Service;
using Microsoft.AspNetCore.Authorization;
using AA2_CS.Services;

namespace AA2_CS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly PurchaseService _purchaseService;
        private readonly AuthService _authService;

        // Constructor del controlador, se inyecta el servicio de compras
        public PurchaseController(PurchaseService purchaseService, AuthService authService)
        {
            _purchaseService = purchaseService;
            _authService = authService;
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
                var id = _purchaseService.Add(purchase); // Agrega una nueva compra
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

        [HttpGet("userEmail/{email}/userPassword/{password}")]
        [Authorize]
        public IActionResult GetPurchasesByUse(string email, string password)
        {
            try
            {
                var purchases = _purchaseService.FindByUser(email, password); // Busca compras por usuario
                return Ok(purchases);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener las compras del usuario: {ex.Message}");
            }
        }
    }

}