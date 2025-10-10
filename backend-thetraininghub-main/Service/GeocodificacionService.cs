using System;
using System.Threading.Tasks;
using AA2_CS.Model;



public class GeocodificacionService
{
    private readonly GeocodificacionRepository _repository;


    public GeocodificacionService(GeocodificacionRepository repository)
    {
        _repository = repository;
    }

    public async Task<CoordenadasDto?> ObtenerCoordenadasAsync(string direccion)
    {
        return await _repository.ObtenerCoordenadasAsync(direccion);
    }
    public async Task<List<Establecimientos>> BuscarEstablecimientosAsync(double lat, double lon, string tipo, int radio = 1000)
    {
        return await _repository.BuscarEstablecimientosAsync(lat, lon, tipo, radio);
    }
}
