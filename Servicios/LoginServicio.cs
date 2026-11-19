using RepoDb;
using SistemaVentasElectronicas.Datos;
using SistemaVentasElectronicas.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVentasElectronicas.Servicios
{
    public class LoginServicio
    {
        public bool ValidarUsuario(string usuario, string contraseña)
        {
            try
            {
                using (var conexion = ConexionBD.ObtenerConexion())
                {
                    string consulta = @"
                        SELECT *
                        FROM Empleados
                        WHERE usuario = @usuario";

                    var empleado = conexion.ExecuteQuery<Empleado>(
                        consulta,
                        new
                        {
                            usuario = usuario
                        }
                    ).FirstOrDefault();

                    if (empleado == null)
                        return false;

                    string hashIngresado =
                        Seguridad.ObtenerHashSHA256(contraseña);

                    return empleado.password_hash == hashIngresado;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Error al validar el usuario: " + ex.Message
                );
            }
        }
    }
}
