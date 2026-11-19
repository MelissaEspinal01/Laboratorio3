using SistemaVentasElectronicas.Entidades;
using SistemaVentasElectronicas.Formularios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaVentasElectronicas
{
    public partial class frmPrincipal : Form
    {
        public frmPrincipal()
        {
            InitializeComponent();

            PruebaConexion.Probar();

            btnEmpleados.Click += btnEmpleados_Click;
            btnClientes.Click += btnClientes_Click;
            btnProductos.Click += btnProductos_Click;
            btnVentas.Click += btnVentas_Click;
            btnDetalleVenta.Click += btnDetalleVenta_Click;
            btnReportes.Click += btnReportes_Click;


        }

        private void btnEmpleados_Click(object sender, EventArgs e)
        {
            MostrarFormulario(new ucEmpleados());
        }

        private void MostrarFormulario(UserControl formulario)
        {
            pnlContenido.Controls.Clear();

            formulario.Dock = DockStyle.Fill;

            pnlContenido.Controls.Add(formulario);
        }

        private void btnClientes_Click(object sender, EventArgs e)
        {
            MostrarFormulario(new ucClientes());
        }

        private void btnProductos_Click(object sender, EventArgs e)
        {
            MostrarFormulario(new ucProductos());
        }

        private void btnVentas_Click(object sender, EventArgs e)
        {
            MostrarFormulario(new ucVentas());
        }

        
        private void btnDetalleVenta_Click(object sender, EventArgs e)
        {
            MostrarFormulario(new ucDetalleVenta());
        }

        public void MostrarDetalleVenta(int idVenta)
        {
            ucDetalleVenta detalle = new ucDetalleVenta();

            detalle.CargarDetalleVenta(idVenta);

            MostrarFormulario(detalle);
        }

        private void btnReportes_Click(object sender, EventArgs e)
        {
            using (frmReporteVentas reporte = new frmReporteVentas())
            {
                reporte.ShowDialog();
            }
        }

        private void btnInventario_Click(object sender, EventArgs e)
        {
            using (frmReporteInventario inventario = new frmReporteInventario())
            {
                inventario.ShowDialog();
            }
        }
    }
}
