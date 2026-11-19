using RepoDb.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVentasElectronicas.Entidades
{
    public class Venta
    {
        [Primary]
        public int id_venta { get; set; }
        public int id_cliente { get; set; }
        public int id_empleado { get; set; }
        public DateTime fecha_venta { get; set; }
    }
}

