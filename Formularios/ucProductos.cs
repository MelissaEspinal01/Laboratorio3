using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SistemaVentasElectronicas.Entidades;
using SistemaVentasElectronicas.Servicios;

namespace SistemaVentasElectronicas.Formularios
{
    public partial class ucProductos : UserControl
    {
        private readonly ProductoServicio servicio = new ProductoServicio();

        public ucProductos()
        {
            InitializeComponent();

            ConfigurarTabla();

            btnRegistrar.Click += btnRegistrar_Click;
            btnActualizar.Click += btnActualizar_Click;
            btnLimpiar.Click += btnLimpiar_Click;

            CargarProductos();
        }

        private void ConfigurarTabla()
        {
            dgvProductos.AutoGenerateColumns = true;
            dgvProductos.ReadOnly = true;
            dgvProductos.AllowUserToAddRows = false;
            dgvProductos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProductos.MultiSelect = false;
            dgvProductos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void CargarProductos()
        {
            try
            {
                dgvProductos.DataSource = null;
                dgvProductos.DataSource = servicio.ObtenerProductos().ToList();

                if (dgvProductos.Columns["id_producto"] != null)
                    dgvProductos.Columns["id_producto"].Visible = false;

                if (dgvProductos.Columns["nombre"] != null)
                    dgvProductos.Columns["nombre"].HeaderText = "Nombre";

                if (dgvProductos.Columns["descripcion"] != null)
                    dgvProductos.Columns["descripcion"].HeaderText = "Descripción";

                if (dgvProductos.Columns["precio"] != null)
                    dgvProductos.Columns["precio"].HeaderText = "Precio";

                if (dgvProductos.Columns["stock"] != null)
                    dgvProductos.Columns["stock"].HeaderText = "Stock";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar productos: " + ex.Message);
            }
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtDescripcion.Text) ||
                string.IsNullOrWhiteSpace(txtPrecio.Text) ||
                string.IsNullOrWhiteSpace(txtStock.Text))
            {
                MessageBox.Show("Completa todos los campos.");
                return;
            }

            if (!decimal.TryParse(txtPrecio.Text, out decimal precio) || precio <= 0)
            {
                MessageBox.Show("Ingresa un precio válido mayor que cero.");
                return;
            }

            if (!int.TryParse(txtStock.Text, out int stock) || stock < 0)
            {
                MessageBox.Show("Ingresa una cantidad de stock válida.");
                return;
            }

            try
            {
                Producto producto = new Producto
                {
                    nombre = txtNombre.Text.Trim(),
                    descripcion = txtDescripcion.Text.Trim(),
                    precio = precio,
                    stock = stock
                };

                servicio.RegistrarProducto(producto);

                MessageBox.Show("Producto registrado correctamente.");

                CargarProductos();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar: " + ex.Message);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (dgvProductos.CurrentRow == null)
            {
                MessageBox.Show("Selecciona un producto de la tabla.");
                return;
            }

            if (!decimal.TryParse(txtPrecio.Text, out decimal precio) || precio <= 0)
            {
                MessageBox.Show("Ingresa un precio válido mayor que cero.");
                return;
            }

            if (!int.TryParse(txtStock.Text, out int stock) || stock < 0)
            {
                MessageBox.Show("Ingresa un stock válido.");
                return;
            }

            try
            {
                Producto producto =
                    dgvProductos.CurrentRow.DataBoundItem as Producto;

                if (producto == null) return;

                producto.nombre = txtNombre.Text.Trim();
                producto.descripcion = txtDescripcion.Text.Trim();
                producto.precio = precio;
                producto.stock = stock;

                servicio.ActualizarProducto(producto);

                MessageBox.Show("Producto actualizado correctamente.");

                CargarProductos();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar: " + ex.Message);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtDescripcion.Clear();
            txtPrecio.Clear();
            txtStock.Clear();
            dgvProductos.ClearSelection();
        }
    }
}
