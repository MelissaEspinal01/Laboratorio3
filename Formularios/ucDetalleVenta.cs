using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using SistemaVentasElectronicas.Datos;


namespace SistemaVentasElectronicas.Formularios
{
    public partial class ucDetalleVenta : UserControl
    {
        public ucDetalleVenta()
        {
            InitializeComponent();

            btnBuscar.Click += btnBuscar_Click;
        }

        public void CargarDetalleVenta(int idVenta)
        {
            txtNVenta.Text = idVenta.ToString();

            try
            {
                using (SqlConnection conexion = ConexionBD.ObtenerConexion())
                {
                    conexion.Open();

                    string consulta = @"
                SELECT Clientes.nombre, Ventas.fecha_venta
                FROM Ventas
                INNER JOIN Clientes
                    ON Ventas.id_cliente = Clientes.id_cliente
                WHERE Ventas.id_venta = @idVenta";

                    using (SqlCommand comando = new SqlCommand(consulta, conexion))
                    {
                        comando.Parameters.AddWithValue("@idVenta", idVenta);

                        using (SqlDataReader lector = comando.ExecuteReader())
                        {
                            if (lector.Read())
                            {
                                txtCliente.Text = lector["nombre"].ToString();
                                dtFecha.Value = Convert.ToDateTime(lector["fecha_venta"]);
                            }
                        }
                    }
                }

                BuscarVenta(idVenta);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la venta: " + ex.Message);
            }
        }
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNVenta.Text) ||
                string.IsNullOrWhiteSpace(txtCliente.Text))
            {
                MessageBox.Show("Ingresa el número de venta y el nombre del cliente.");
                return;
            }

            if (!int.TryParse(txtNVenta.Text, out int idVenta))
            {
                MessageBox.Show("Ingresa un número de venta válido.");
                return;
            }

            BuscarVenta(idVenta);
        }

        private void BuscarVenta(int idVenta)
        {
            try
            {
                using (SqlConnection conexion = ConexionBD.ObtenerConexion())
                {
                    conexion.Open();

                    string consultaVenta = @"
                        SELECT
                            Ventas.id_venta,
                            Clientes.nombre,
                            Ventas.fecha_venta
                        FROM Ventas
                        INNER JOIN Clientes
                            ON Ventas.id_cliente = Clientes.id_cliente
                        WHERE Ventas.id_venta = @idVenta
                        AND Clientes.nombre = @nombre
                        AND Ventas.fecha_venta = @fecha";

                    using (SqlCommand comando = new SqlCommand(consultaVenta, conexion))
                    {
                        comando.Parameters.AddWithValue("@idVenta", idVenta);
                        comando.Parameters.AddWithValue("@nombre", txtCliente.Text.Trim());
                        comando.Parameters.AddWithValue("@fecha", dtFecha.Value.Date);

                        using (SqlDataReader lector = comando.ExecuteReader())
                        {
                            if (!lector.Read())
                            {
                                MessageBox.Show("No se encontró una venta con esos datos.");
                                dgvDetalle.DataSource = null;
                                lblTotal.Text = "Total: $0.00";
                                return;
                            }

                            txtCliente.Text = lector["nombre"].ToString();
                        }
                    }

                    string consultaDetalle = @"
                        SELECT
                            Productos.nombre AS Producto,
                            DetalleVenta.cantidad AS Cantidad,
                            DetalleVenta.precio_unitario AS Precio,
                            (DetalleVenta.cantidad * DetalleVenta.precio_unitario) AS Subtotal
                        FROM DetalleVenta
                        INNER JOIN Productos
                            ON DetalleVenta.id_producto = Productos.id_producto
                        WHERE DetalleVenta.id_venta = @idVenta";

                    using (SqlCommand comando = new SqlCommand(consultaDetalle, conexion))
                    {
                        comando.Parameters.AddWithValue("@idVenta", idVenta);

                        DataTable tabla = new DataTable();

                        using (SqlDataAdapter adaptador = new SqlDataAdapter(comando))
                        {
                            adaptador.Fill(tabla);
                        }

                        dgvDetalle.DataSource = tabla;

                        decimal total = 0;

                        foreach (DataRow fila in tabla.Rows)
                        {
                            total += Convert.ToDecimal(fila["Subtotal"]);
                        }

                        lblTotal.Text = "Total: $" + total.ToString("0.00");

                        MessageBox.Show("Venta encontrada correctamente.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar la venta: " + ex.Message);
            }
        }
    }
}
