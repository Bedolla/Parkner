namespace Parkner.Mobile.Models
{
    public class Posicion
    {
        public Posicion(double latitud = 1, double longitud = 1, double distancia = 200)
        {
            this.Latitud = latitud;
            this.Longitud = longitud;
            this.Distancia = distancia;
        }

        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public double Distancia { get; set; }
    }
}
