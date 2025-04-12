using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP1Entities
{
    public class Orden
    {
        public int Id { get; set; }
        public int NumeroOrden { get; set; }
        public DateOnly FechaOrden { get; set; }
        public double Valor { get; set; }
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
        public override string ToString()
        {
            return $"Número de pedido:{NumeroOrden} - Monto:{Valor} AR$ - Cliente: {Cliente}";
        }
    }
}
