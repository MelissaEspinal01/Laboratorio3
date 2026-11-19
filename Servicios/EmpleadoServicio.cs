using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVentasElectronicas.Entidades;
using SistemaVentasElectronicas.Repositorios;

namespace SistemaVentasElectronicas.Servicios
{
    public class EmpleadoServicio
    {
        private readonly EmpleadoRepositorio repositorio;

        public EmpleadoServicio()
        {
            repositorio = new EmpleadoRepositorio();
        }

        public IEnumerable<Empleado> ObtenerEmpleados()
        {
            return repositorio.ObtenerTodos();
        }

        public int RegistrarEmpleado(Empleado empleado)
        {
            return repositorio.Insertar(empleado);
        }

        public int ActualizarEmpleado(Empleado empleado)
        {
            return repositorio.Actualizar(empleado);
        }
    }
}
