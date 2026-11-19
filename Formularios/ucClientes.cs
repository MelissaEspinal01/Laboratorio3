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
    public partial class ucClientes : UserControl
    {
        private readonly ClienteServicio servicio = new ClienteServicio();

        public ucClientes()
        {
            InitializeComponent();

            ConfigurarTabla();

            btnRegistrar.Click += btnRegistrar_Click;
            btnActualizar.Click += btnActualizar_Click;
            btnLimpiar.Click += btnLimpiar_Click;

            CargarClientes();
        }

        private void ConfigurarTabla()
        {
            dgvClientes.AutoGenerateColumns = true;
            dgvClientes.ReadOnly = true;
            dgvClientes.AllowUserToAddRows = false;
            dgvClientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvClientes.MultiSelect = false;
            dgvClientes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void CargarClientes()
        {
            try
            {
                dgvClientes.DataSource = null;
                dgvClientes.DataSource = servicio.ObtenerClientes().ToList();

                if (dgvClientes.Columns["id_cliente"] != null)
                    dgvClientes.Columns["id_cliente"].Visible = false;

                if (dgvClientes.Columns["nombre"] != null)
                    dgvClientes.Columns["nombre"].HeaderText = "Nombre";

                if (dgvClientes.Columns["apellido"] != null)
                    dgvClientes.Columns["apellido"].HeaderText = "Apellido";

                if (dgvClientes.Columns["telefono"] != null)
                    dgvClientes.Columns["telefono"].HeaderText = "Teléfono";

                if (dgvClientes.Columns["correo"] != null)
                    dgvClientes.Columns["correo"].HeaderText = "Correo electrónico";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar clientes: " + ex.Message);
            }
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(txtTelefono.Text) ||
                string.IsNullOrWhiteSpace(txtCorreo.Text))
            {
                MessageBox.Show("Completa todos los campos.");
                return;
            }

            try
            {
                Cliente cliente = new Cliente
                {
                    nombre = txtNombre.Text.Trim(),
                    apellido = txtApellido.Text.Trim(),
                    telefono = txtTelefono.Text.Trim(),
                    correo = txtCorreo.Text.Trim()
                };

                servicio.RegistrarCliente(cliente);

                MessageBox.Show("Cliente registrado correctamente.");

                CargarClientes();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar: " + ex.Message);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (dgvClientes.CurrentRow == null)
            {
                MessageBox.Show("Selecciona un cliente de la tabla.");
                return;
            }

            try
            {
                Cliente cliente =
                    dgvClientes.CurrentRow.DataBoundItem as Cliente;

                if (cliente == null)
                    return;

                cliente.nombre = txtNombre.Text.Trim();
                cliente.apellido = txtApellido.Text.Trim();
                cliente.telefono = txtTelefono.Text.Trim();
                cliente.correo = txtCorreo.Text.Trim();

                servicio.ActualizarCliente(cliente);

                MessageBox.Show("Cliente actualizado correctamente.");

                CargarClientes();
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
            txtApellido.Clear();
            txtTelefono.Clear();
            txtCorreo.Clear();

            dgvClientes.ClearSelection();
        }

        
    }
}
