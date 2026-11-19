using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using RepoDb;

namespace SistemaVentasElectronicas.Datos
{
    public static class ConexionBD
    {
        private static readonly string cadenaConexion =
            "Server=MELYEM;Database=VentasElectronicas;Trusted_Connection=True;TrustServerCertificate=True;";

        static ConexionBD()
        {
            GlobalConfiguration
                .Setup()
                .UseSqlServer();
        }

        public static SqlConnection ObtenerConexion()
        {
            return new SqlConnection(cadenaConexion);
        }
    }
}

