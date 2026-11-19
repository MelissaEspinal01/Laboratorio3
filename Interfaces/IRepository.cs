using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVentasElectronicas.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> ObtenerTodos();

        int Insertar(T entidad);

        int Actualizar(T entidad);
    }
}

