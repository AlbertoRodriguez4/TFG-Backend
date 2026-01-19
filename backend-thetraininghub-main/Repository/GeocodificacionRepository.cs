using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using AA2_CS.Model; // Asumo que aquí están 'CoordenadasDto' y 'Establecimientos'

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
        
        try 
        {
            var response = await _http.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            
            // Usamos opciones para asegurar que lea mayúsculas/minúsculas sin problema
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = JsonSerializer.Deserialize<List<NominatimResult>>(json, options);

            if (result == null || result.Count == 0) return null;

            return new CoordenadasDto
            {
                // Usamos CultureInfo para evitar errores con puntos y comas decimales
                Lat = double.Parse(result[0].Lat, System.Globalization.CultureInfo.InvariantCulture),
                Lon = double.Parse(result[0].Lon, System.Globalization.CultureInfo.InvariantCulture)
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error geocodificando: " + ex.Message);
            return null;
        }
    }

    public async Task<List<Establecimientos>> BuscarEstablecimientosAsync(double lat, double lon, string tipo, int radio = 1000)
    {
        string latStr = lat.ToString(System.Globalization.CultureInfo.InvariantCulture);
        string lonStr = lon.ToString(System.Globalization.CultureInfo.InvariantCulture);

        string query = $@"[out:json];
        (
          node[""leisure""=""{tipo}""](around:{radio},{latStr},{lonStr});
          way[""leisure""=""{tipo}""](around:{radio},{latStr},{lonStr});
          relation[""leisure""=""{tipo}""](around:{radio},{latStr},{lonStr});
        );
        out center;";

        var url = "https://overpass-api.de/api/interpreter?data=" + Uri.EscapeDataString(query);

        try
        {
            var response = await _http.GetAsync(url);
            if (!response.IsSuccessStatusCode) return new List<Establecimientos>();

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = JsonSerializer.Deserialize<OverpassResult>(json, options);

            var lista = new List<Establecimientos>();

            if (result?.Elements != null)
            {
                foreach (var el in result.Elements)
                {
                    double elLat = el.Lat ?? el.Center?.Lat ?? 0;
                    double elLon = el.Lon ?? el.Center?.Lon ?? 0;

                    // --- LOGICA DE DIRECCIÓN ---
                    string direccionFinal = "Dirección no disponible";
                    if (el.Tags != null)
                    {
                        var partesDireccion = new List<string>();

                        if (!string.IsNullOrWhiteSpace(el.Tags.Street))
                            partesDireccion.Add(el.Tags.Street);

                        if (!string.IsNullOrWhiteSpace(el.Tags.HouseNumber))
                            partesDireccion.Add(el.Tags.HouseNumber);

                         // Si tiene código postal y ciudad, también se pueden añadir:
                         if (!string.IsNullOrWhiteSpace(el.Tags.Postcode) && !string.IsNullOrWhiteSpace(el.Tags.City))
                             partesDireccion.Add($"({el.Tags.Postcode} {el.Tags.City})");

                        if (partesDireccion.Count > 0)
                            direccionFinal = string.Join(", ", partesDireccion);
                    }
                    // --------------------------

                    lista.Add(new Establecimientos
                    {
                        Nombre = el.Tags?.Name ?? "Sin nombre",
                        Lat = elLat,
                        Lon = elLon,
                        Direccion = direccionFinal,
                        Tipo = tipo
                    });
                }
            }

            return lista;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al buscar establecimientos: {ex.Message}");
            return new List<Establecimientos>();
        }
    }
} 
// FIN DE LA CLASE REPOSITORY. Las clases modelo van FUERA.

// --- CLASES MODELO PARA OVERPASS ---
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

    [JsonPropertyName("addr:street")]
    public string Street { get; set; }

    [JsonPropertyName("addr:housenumber")]
    public string HouseNumber { get; set; }

    [JsonPropertyName("addr:postcode")]
    public string Postcode { get; set; }

    [JsonPropertyName("addr:city")]
    public string City { get; set; }
}

// --- CLASE MODELO FALTANTE PARA NOMINATIM ---
public class NominatimResult
{
    [JsonPropertyName("lat")]
    public string Lat { get; set; }

    [JsonPropertyName("lon")]
    public string Lon { get; set; }

    [JsonPropertyName("display_name")]
    public string DisplayName { get; set; }
}