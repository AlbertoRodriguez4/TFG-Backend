using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using AA2_CS.Model;
using System.Text.Json.Serialization;


public class GeocodificacionRepository
{
    private readonly HttpClient _http;

    public GeocodificacionRepository(HttpClient http)
    {
        _http = http;
        _http.DefaultRequestHeaders.UserAgent.ParseAdd(
            "MiAppGeocodificacion/1.0 (contacto@tudominio.com)"
        );
    }

    public async Task<CoordenadasDto?> ObtenerCoordenadasAsync(string direccion)
    {
        var url = $"https://nominatim.openstreetmap.org/search?format=json&q={Uri.EscapeDataString(direccion)}";
        var response = await _http.GetAsync(url);

        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync();
        Console.WriteLine("ESTE ES EL JSON: " + json);

        var result = JsonSerializer.Deserialize<List<NominatimResult>>(json);

        if (result == null || result.Count == 0) return null;

        return new CoordenadasDto
        {
            Lat = double.Parse(result[0].Lat, System.Globalization.CultureInfo.InvariantCulture),
            Lon = double.Parse(result[0].Lon, System.Globalization.CultureInfo.InvariantCulture)
        };
    }

    public async Task<List<Establecimientos>> BuscarEstablecimientosAsync(double lat, double lon, string tipo, int radio = 1000)
{
    // Convertir lat/lon a string con punto decimal
    string latStr = lat.ToString(System.Globalization.CultureInfo.InvariantCulture);
    string lonStr = lon.ToString(System.Globalization.CultureInfo.InvariantCulture);

    // Crear la consulta Overpass
    string query = $@"[out:json];
(
    node[""leisure""=""{tipo}""](around:{radio},{latStr},{lonStr});
    way[""leisure""=""{tipo}""](around:{radio},{latStr},{lonStr});
    relation[""leisure""=""{tipo}""](around:{radio},{latStr},{lonStr});
);
out center;";

    var url = "https://overpass-api.de/api/interpreter?data=" + Uri.EscapeDataString(query);
    var response = await _http.GetAsync(url);

    if (!response.IsSuccessStatusCode) return new List<Establecimientos>();

    var json = await response.Content.ReadAsStringAsync();
    var result = JsonSerializer.Deserialize<OverpassResult>(json);

    // Mapear a DTO
    var lista = new List<Establecimientos>();
    if (result?.Elements != null)
    {
        foreach (var el in result.Elements)
        {
            double elLat = el.Lat ?? el.Center?.Lat ?? 0;
            double elLon = el.Lon ?? el.Center?.Lon ?? 0;

            lista.Add(new Establecimientos
            {
                Nombre = el.Tags?.Name ?? "Sin nombre",
                Lat = elLat,
                Lon = elLon,
                Direccion = "", // Overpass no siempre devuelve dirección completa
                Tipo = tipo
            });
        }
    }

    return lista;
}


}
public class OverpassResult
{
    [JsonPropertyName("elements")]
    public List<Element> Elements { get; set; }
}

public class Element
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("lat")]
    public double? Lat { get; set; }

    [JsonPropertyName("lon")]
    public double? Lon { get; set; }

    [JsonPropertyName("center")]
    public Center Center { get; set; }

    [JsonPropertyName("tags")]
    public Tags Tags { get; set; }
}

public class Center
{
    [JsonPropertyName("lat")]
    public double Lat { get; set; }

    [JsonPropertyName("lon")]
    public double Lon { get; set; }
}

public class Tags
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("leisure")]
    public string Leisure { get; set; }
}
