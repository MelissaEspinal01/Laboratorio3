using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVentasElectronicas.Entidades;
using SistemaVentasElectronicas.Repositorios;

namespace SistemaVentasElectronicas.Servicios
{
    public class ProductoServicio
    {
        private readonly ProductoRepositorio repositorio;

        public ProductoServicio()
        {
            repositorio = new ProductoRepositorio();
        }

        public IEnumerable<Producto> ObtenerProductos()
        {
            return repositorio.ObtenerTodos();
        }

        public int RegistrarProducto(Producto producto)
        {
            return repositorio.Insertar(producto);
        }

        public int ActualizarProducto(Producto producto)
        {
            return repositorio.Actualizar(producto);
        }
    }
}
