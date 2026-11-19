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
    public partial class frmReporteVentas : Form
    {
        public frmReporteVentas()
        {
            InitializeComponent();

            btnGenerar.Click += btnGenerar_Click;
            btnCerrar.Click += btnCerrar_Click;
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conexion = ConexionBD.ObtenerConexion())
                {
                    conexion.Open();

                    string consulta = @"
                        SELECT
                            Productos.nombre AS Producto,
                            SUM(DetalleVenta.cantidad) AS CantidadVendida,
                            SUM(DetalleVenta.cantidad * DetalleVenta.precio_unitario) AS TotalVendido
                        FROM DetalleVenta
                        INNER JOIN Productos
                            ON DetalleVenta.id_producto = Productos.id_producto
                        GROUP BY Productos.nombre
                        ORDER BY Productos.nombre";

                    DataTable tabla = new DataTable();

                    using (SqlDataAdapter adaptador = new SqlDataAdapter(consulta, conexion))
                    {
                        adaptador.Fill(tabla);
                    }

                    dgvReporte.DataSource = tabla;

                    if (tabla.Rows.Count == 0)
                    {
                        lblTotalGeneral.Text = "Total general: $0.00";

                        MessageBox.Show("Todavía no hay productos vendidos.");
                        return;
                    }

                    decimal totalGeneral = 0;

                    foreach (DataRow fila in tabla.Rows)
                    {
                        totalGeneral += Convert.ToDecimal(fila["TotalVendido"]);
                    }

                    lblTotalGeneral.Text =
                        "Total general: $" + totalGeneral.ToString("0.00");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al generar el reporte: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

