using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVentasElectronicas.Entidades;
using SistemaVentasElectronicas.Repositorios;

namespace SistemaVentasElectronicas.Servicios
{
    public class ClienteServicio
    {
        private readonly ClienteRepositorio repositorio;

        public ClienteServicio()
        {
            repositorio = new ClienteRepositorio();
        }

        public IEnumerable<Cliente> ObtenerClientes()
        {
            return repositorio.ObtenerTodos();
        }

        public int RegistrarCliente(Cliente cliente)
        {
            return repositorio.Insertar(cliente);
        }

        public int ActualizarCliente(Cliente cliente)
        {
            return repositorio.Actualizar(cliente);
        }
    }
}
