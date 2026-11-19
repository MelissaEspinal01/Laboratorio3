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
    public partial class frmReporteInventario : Form
    {
        public frmReporteInventario()
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
                            id_producto,
                            nombre,
                            precio,
                            stock
                        FROM Productos
                        ORDER BY nombre";

                    DataTable tabla = new DataTable();

                    using (SqlDataAdapter adaptador = new SqlDataAdapter(consulta, conexion))
                    {
                        adaptador.Fill(tabla);
                    }

                    dgvInventario.DataSource = tabla;
                    dgvInventario.DefaultCellStyle.ForeColor = Color.Black;
                    dgvInventario.DefaultCellStyle.BackColor = Color.White;
                    dgvInventario.DefaultCellStyle.SelectionForeColor = Color.White;
                    dgvInventario.DefaultCellStyle.SelectionBackColor = Color.DodgerBlue;
                    dgvInventario.RowsDefaultCellStyle.ForeColor = Color.Black;
                    dgvInventario.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;

                    int totalProductos = tabla.Rows.Count;

                    lblTotalProductos.Text =
                        "Total de productos: " + totalProductos;

                    // Aplicar colores según las existencias.
                    foreach (DataGridViewRow fila in dgvInventario.Rows)
                    {
                        if (fila.IsNewRow)
                            continue;

                        int stock = Convert.ToInt32(fila.Cells["stock"].Value);

                        if (stock <= 5)
                        {
                            fila.DefaultCellStyle.BackColor = Color.LightCoral;
                            fila.DefaultCellStyle.ForeColor = Color.DarkRed;
                        }
                        else if (stock <= 10)
                        {
                            fila.DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                            fila.DefaultCellStyle.ForeColor = Color.Black;
                        }
                    }

                    if (tabla.Rows.Count == 0)
                    {
                        MessageBox.Show("No hay productos registrados.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al consultar el inventario: " + ex.Message,
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

