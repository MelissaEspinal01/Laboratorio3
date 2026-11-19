using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVentasElectronicas.Entidades;
using SistemaVentasElectronicas.Repositorios;

namespace SistemaVentasElectronicas.Servicios
{
    public class VentaServicio
    {
        private readonly VentaRepositorio repositorio;

        public VentaServicio()
        {
            repositorio = new VentaRepositorio();
        }

        public IEnumerable<Venta> ObtenerVentas()
        {
            return repositorio.ObtenerTodos();
        }

        public int RegistrarVenta(Venta venta)
        {
            return repositorio.Insertar(venta);
        }

        public int ActualizarVenta(Venta venta)
        {
            return repositorio.Actualizar(venta);
        }
    }
}
