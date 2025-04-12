using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TP1Entities
{
    [Table("Clientes")]
    [Index(nameof(Cliente.Nombre), nameof(Cliente.Apellido), Name = "Clientes_Nombre_Apellido", IsUnique = true)]
    public class Cliente
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(50, ErrorMessage = "The field {0} must be between {2} and {1} characteres", MinimumLength = 3)]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(50, ErrorMessage = "The field {0} must be between {2} and {1} characteres", MinimumLength = 3)]
        public string Apellido { get; set; } = null!;

        public int Dni { get; set; }
        public ICollection<Orden>? Ordenes { get; set; }
        public override string ToString()
        {
            return $"ID: {Id} Nombre completo: {Apellido.ToUpper()}, {Nombre.ToUpper()}, DNI: {Dni}";
        }
    }
}
