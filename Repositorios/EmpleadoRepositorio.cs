using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVentasElectronicas.Entidades;

namespace SistemaVentasElectronicas.Repositorios
{
    public class EmpleadoRepositorio : RepositorioGenerico<Empleado>
    {
        public EmpleadoRepositorio()
            : base("Empleados")
        {
        }
    }
}
