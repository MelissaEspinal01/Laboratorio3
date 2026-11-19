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
    public partial class frmDetalleVentaModal : Form
    {
        private readonly int idVenta;

        public frmDetalleVentaModal(int idVenta)
        {
            InitializeComponent();

            this.idVenta = idVenta;

            btnCerrar.Click += btnCerrar_Click;

            CargarDetalleVenta();
        }

        private void CargarDetalleVenta()
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
                            Clientes.apellido
                        FROM Ventas
                        INNER JOIN Clientes
                            ON Ventas.id_cliente = Clientes.id_cliente
                        WHERE Ventas.id_venta = @idVenta";

                    using (SqlCommand comando = new SqlCommand(consultaVenta, conexion))
                    {
                        comando.Parameters.AddWithValue("@idVenta", idVenta);

                        using (SqlDataReader lector = comando.ExecuteReader())
                        {
                            if (lector.Read())
                            {
                                txtNVenta.Text = lector["id_venta"].ToString();

                                txtCliente.Text =
                                    lector["nombre"].ToString() + " " +
                                    lector["apellido"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("No se encontró la venta.");
                                Close();
                                return;
                            }
                        }
                    }

                    string consultaDetalle = @"
                        SELECT
                            Productos.nombre AS Producto,
                            DetalleVenta.cantidad AS Cantidad,
                            DetalleVenta.precio_unitario AS Precio,
                            (DetalleVenta.cantidad *
                             DetalleVenta.precio_unitario) AS Subtotal
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
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al cargar el detalle de la venta: " + ex.Message
                );
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
