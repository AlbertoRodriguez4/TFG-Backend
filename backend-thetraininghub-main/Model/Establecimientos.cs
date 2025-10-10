namespace AA2_CS.Model
{
    public class Establecimientos
    {
        public string Nombre { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public string Direccion { get; set; }
        public string Tipo { get; set; }

        public Establecimientos() { }

        public Establecimientos(string nombre, string direccion, double lat, double lon)
        {
            Nombre = nombre;
            Direccion = direccion;
            Lat = lat;
            Lon = lon;
        }
    }
}
