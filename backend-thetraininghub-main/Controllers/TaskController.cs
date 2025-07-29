namespace AA2_CS.Controllers;

using AA2_CS.Model;
using AA2_CS.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
[Route("api/[controller]")]
[ApiController]
public class TaskController : ControllerBase
{
    private readonly TasksService _tasksService;

    public TaskController(TasksService tasksService)
    {
        _tasksService = tasksService;
    }

    [HttpPost]
    public IActionResult AddTask([FromBody] Task task)
    {
        try
        {
            var result = _tasksService.Add(task);
            return result > 0 ? Ok(task) : BadRequest("Failed to add task");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    [HttpPut("{id}")]
    public IActionResult UpdateTask([FromBody] Task updatedTask)
    {
        try
        {
            var result = _tasksService.Update(updatedTask);
            if (result == null)
            {
                return NotFound($"Task with ID {updatedTask.id} not found");
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    [HttpGet]
    public IActionResult GetAllTasks()
    {
        try
        {
            var tasks = _tasksService.FindAll();
            return Ok(tasks);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    [HttpGet("user/{userId}")]
    public IActionResult GetTasksByUserId(int userId)
    {
        try
        {
            var tasks = _tasksService.FindByUserId(userId);
            return Ok(tasks);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    [HttpGet("{id}")]
    public IActionResult GetTaskById(int id)
    {
        try
        {
            var task = _tasksService.FindById(id);
            if (task == null)
            {
                return NotFound($"Task with ID {id} not found");
            }
            return Ok(task);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    [HttpDelete("{id}")]
    public IActionResult DeleteTask(int id)
    {
        try
        {
            var task = _tasksService.FindById(id);
            if (task == null)
            {
                return NotFound($"Task with ID {id} not found");
            }
            var result = _tasksService.Delete(task);
            return result > 0 ? Ok($"Task with ID {id} deleted successfully") : BadRequest("Failed to delete task");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    [HttpPost("complete/{taskId}")]
    public IActionResult CompleteTask(int taskId)
    {
        try
        {
            var result = _tasksService.CompleteTask(taskId);
            if (result == "Task not found")
            {
                return NotFound(result);
            }
            else if (result == "Task is already completed")
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}