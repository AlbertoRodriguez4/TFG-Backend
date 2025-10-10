using System.Text.Json.Serialization;
namespace AA2_CS.Model
{
    public class NominatimResult
    {
        [JsonPropertyName("lat")]
        public string Lat { get; set; }

        [JsonPropertyName("lon")]
        public string Lon { get; set; }

        public NominatimResult() { }
        public NominatimResult(string lat, string lon)
        {
            Lat = lat;
            Lon = lon;
        }
    }
}