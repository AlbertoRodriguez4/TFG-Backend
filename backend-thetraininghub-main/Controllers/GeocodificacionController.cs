using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class GeocodificacionController : ControllerBase
{
    private readonly GeocodificacionService _service;

    public GeocodificacionController(GeocodificacionService service)
    {
        _service = service;
    }

    [HttpGet("{*direccion}")]
    public async Task<IActionResult> Get(string direccion)
    {
        var coordenadas = await _service.ObtenerCoordenadasAsync(direccion);
        if (coordenadas == null)
            return NotFound(new { mensaje = "No se encontraron coordenadas" });

        return Ok(coordenadas);
    }
    [HttpGet("establecimientos")]
    public async Task<IActionResult> GetEstablecimientos(
    [FromQuery] double lat,
    [FromQuery] double lon,
    [FromQuery] string tipo,
    [FromQuery] int radio = 1000)
    {
        var establecimientos = await _service.BuscarEstablecimientosAsync(lat, lon, tipo, radio);
        return Ok(establecimientos);
    }

}
