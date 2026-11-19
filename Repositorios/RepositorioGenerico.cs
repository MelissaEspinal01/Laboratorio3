using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using RepoDb;
using SistemaVentasElectronicas.Datos;
using SistemaVentasElectronicas.Interfaces;

namespace SistemaVentasElectronicas.Repositorios
{
    public class RepositorioGenerico<T> : IRepository<T> where T : class
    {
        private readonly string tabla;

        public RepositorioGenerico(string nombreTabla)
        {
            tabla = nombreTabla;
        }

        public IEnumerable<T> ObtenerTodos()
        {
            using (SqlConnection conexion = ConexionBD.ObtenerConexion())
            {
                conexion.Open();

                return conexion.QueryAll<T>(tabla).ToList();
            }
        }

        public int Insertar(T entidad)
        {
            using (SqlConnection conexion = ConexionBD.ObtenerConexion())
            {
                conexion.Open();

                return conexion.Insert<T, int>(tabla, entidad);
            }
        }

        public int Actualizar(T entidad)
        {
            using (SqlConnection conexion = ConexionBD.ObtenerConexion())
            {
                conexion.Open();

                return conexion.Update<T>(tabla, entidad);
            }
        }
    }
}

