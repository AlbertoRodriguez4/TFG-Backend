using Microsoft.AspNetCore.Mvc;
using AA2_CS.Model;
using AA2_CS.Service;
using Microsoft.AspNetCore.Authorization;

namespace AA2_CS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanController : ControllerBase
    {
        private readonly PlanService _planService;

        public PlanController(PlanService planService)
        {
            _planService = planService;
        }

        // Obtener todos los planes
        [HttpGet]
        [Authorize]
        public IActionResult GetAllPlans()
        {
            try
            {
                var plans = _planService.FindAll();
                return Ok(plans);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener los planes: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetPlanById(int id)
        {
            try
            {
                var plan = _planService.FindById(id);
                if (plan == null)
                    return NotFound();

                return Ok(plan);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener el plan: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize(Roles = Roles.userStaff)]
        public IActionResult AddPlan([FromBody] Plan plan)
        {
            try
            {
                if (plan == null || string.IsNullOrEmpty(plan.description) || plan.userid <= 0)
                    return BadRequest("Datos inválidos para crear un plan");

                int newPlanId = _planService.Add(plan);
                return CreatedAtAction(nameof(GetPlanById), new { id = newPlanId }, plan);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear el plan: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Roles.userStaff)]
        public IActionResult UpdatePlan(int id, [FromBody] Plan plan)
        {
            try
            {
                if (plan == null || id != plan.id)
                    return BadRequest("Los datos del plan no coinciden con el ID proporcionado");

                var updatedPlan = _planService.Update(plan);
                if (updatedPlan > 0)
                    return Ok(plan);

                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar el plan: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.userMaster)]
        public IActionResult DeletePlan(int id)
        {
            try
            {
                var plan = _planService.FindById(id);
                if (plan == null)
                    return NotFound();

                int result = _planService.Delete(plan);
                if (result > 0)
                    return Ok();

                return BadRequest("No se pudo eliminar el plan");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar el plan: {ex.Message}");
            }
        }

    }
}
