using System.ComponentModel.DataAnnotations.Schema;

namespace Parkner.Data.Entities
{
    public class Base
    {
        public string Id { get; set; }

        [NotMapped]
        public Respuesta Respuesta { get; set; }
    }
}
