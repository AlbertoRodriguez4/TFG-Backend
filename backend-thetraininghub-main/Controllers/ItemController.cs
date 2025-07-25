using Microsoft.AspNetCore.Mvc;
using AA2_CS.Model;
using AA2_CS.Service;
using Microsoft.AspNetCore.Authorization;

namespace AA2_CS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly ItemService _itemService;

        public ItemController(ItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpPost]
        [Authorize(Roles = Roles.userStaff)]
        public IActionResult AddItem([FromBody] Item item)
        {
            try
            {
                var result = _itemService.Add(item);
                return result > 0 ? Ok(item) : BadRequest("Failed to add item");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Roles.userMaster)]
        public async Task<ActionResult<Item>> UpdateItem(int id, Item updatedItem)
        {
            try
            {
                var result = await _itemService.UpdateById(id, updatedItem);
                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.userMaster)]
        public IActionResult DeleteItem(int id)
        {
            try
            {
                var item = _itemService.FindById(id);
                if (item == null) return NotFound();

                var result = _itemService.Delete(item);
                return result > 0 ? Ok() : BadRequest("Failed to delete item");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAllItems()
        {
            try
            {
                var items = _itemService.FindAll();
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = Roles.userMaster)]
        public IActionResult GetItemById(int id)
        {
            try
            {
                var item = _itemService.FindById(id);
                return item != null ? Ok(item) : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("search/{name}")]
        [Authorize(Roles = Roles.userMaster)]
        public IActionResult FindItemsByCharacteristic(string name)
        {
            try
            {
                var items = _itemService.FindByCharacteristic(name);
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("random-strength")]
        [Authorize]
        public IActionResult GetRandomStrengthItems()
        {
            try
            {
                var strengthItems = _itemService.GetRandomStrengthItems(2);
                return Ok(strengthItems);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("random-endurance")]
        [Authorize]
        public IActionResult GetRandomEnduranceItems()
        {
            try
            {
                var enduranceItems = _itemService.GetRandomEnduranceItems(2);
                return Ok(enduranceItems);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("random-items")]
        [Authorize]
        public IActionResult GetRandomItems()
        {
            try
            {
                var randomItems = _itemService.GetRandomItems(4);
                return Ok(randomItems);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
