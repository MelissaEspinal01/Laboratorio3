using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVentasElectronicas.Entidades;
using SistemaVentasElectronicas.Repositorios;

namespace SistemaVentasElectronicas.Servicios
{
    public class DetalleVentaServicio
    {
        private readonly DetalleVentaRepositorio repositorio;

        public DetalleVentaServicio()
        {
            repositorio = new DetalleVentaRepositorio();
        }

        public IEnumerable<DetalleVenta> ObtenerDetalles()
        {
            return repositorio.ObtenerTodos();
        }

        public int RegistrarDetalle(DetalleVenta detalle)
        {
            return repositorio.Insertar(detalle);
        }

        public int ActualizarDetalle(DetalleVenta detalle)
        {
            return repositorio.Actualizar(detalle);
        }
    }
}
