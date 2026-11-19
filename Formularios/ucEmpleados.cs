using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;
using SistemaVentasElectronicas.Entidades;
using SistemaVentasElectronicas.Servicios;

namespace SistemaVentasElectronicas.Formularios
{
    public partial class ucEmpleados : UserControl
    {
        private readonly EmpleadoServicio servicio = new EmpleadoServicio();

        public ucEmpleados()
        {
            InitializeComponent();

            ConfigurarTabla();

            btnRegistrar.Click += btnRegistrar_Click;
            btnActualizar.Click += btnActualizar_Click;
            btnLimpiar.Click += btnLimpiar_Click;

            CargarEmpleados();
        }

        private void ConfigurarTabla()
        {
            dgvEmpleados.AutoGenerateColumns = true;
            dgvEmpleados.ReadOnly = true;
            dgvEmpleados.AllowUserToAddRows = false;
            dgvEmpleados.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEmpleados.MultiSelect = false;
            dgvEmpleados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void CargarEmpleados()
        {
            try
            {
                dgvEmpleados.DataSource = null;

                dgvEmpleados.DataSource = servicio.ObtenerEmpleados().ToList();

                if (dgvEmpleados.Columns["id_empleado"] != null)
                    dgvEmpleados.Columns["id_empleado"].Visible = false;

                if (dgvEmpleados.Columns["nombre"] != null)
                    dgvEmpleados.Columns["nombre"].HeaderText = "Nombre";

                if (dgvEmpleados.Columns["apellido"] != null)
                    dgvEmpleados.Columns["apellido"].HeaderText = "Apellido";

                if (dgvEmpleados.Columns["cargo"] != null)
                    dgvEmpleados.Columns["cargo"].HeaderText = "Cargo";

                if (dgvEmpleados.Columns["usuario"] != null)
                    dgvEmpleados.Columns["usuario"].HeaderText = "Usuario";

                if (dgvEmpleados.Columns["password_hash"] != null)
                    dgvEmpleados.Columns["password_hash"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al cargar los empleados: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(txtCargo.Text) ||
                string.IsNullOrWhiteSpace(txtUsuario.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Completa todos los campos.");
                return;
            }

            try
            {
                Empleado empleado = new Empleado
                {
                    nombre = txtNombre.Text.Trim(),
                    apellido = txtApellido.Text.Trim(),
                    cargo = txtCargo.Text.Trim(),
                    usuario = txtUsuario.Text.Trim(),
                    password_hash = GenerarHash(txtPassword.Text)
                };

                servicio.RegistrarEmpleado(empleado);

                MessageBox.Show("Empleado registrado correctamente.");

                CargarEmpleados();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar: " + ex.Message);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (dgvEmpleados.CurrentRow == null)
            {
                MessageBox.Show("Selecciona un empleado de la tabla.");
                return;
            }

            try
            {
                Empleado empleado =
                    dgvEmpleados.CurrentRow.DataBoundItem as Empleado;

                if (empleado == null)
                    return;

                empleado.nombre = txtNombre.Text.Trim();
                empleado.apellido = txtApellido.Text.Trim();
                empleado.cargo = txtCargo.Text.Trim();
                empleado.usuario = txtUsuario.Text.Trim();

                if (!string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    empleado.password_hash = GenerarHash(txtPassword.Text);
                }

                servicio.ActualizarEmpleado(empleado);

                MessageBox.Show("Empleado actualizado correctamente.");

                CargarEmpleados();
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
            txtCargo.Clear();
            txtUsuario.Clear();
            txtPassword.Clear();

            dgvEmpleados.ClearSelection();
        }

        private string GenerarHash(string password)
        {
            byte[] salt = new byte[16];

            using (var generador = RandomNumberGenerator.Create())
            {
                generador.GetBytes(salt);
            }

            using (var derivador = new Rfc2898DeriveBytes(password, salt, 100000))
            {
                byte[] hash = derivador.GetBytes(32);

                return "PBKDF2$100000$" +
                       Convert.ToBase64String(salt) + "$" +
                       Convert.ToBase64String(hash);
            }
        }
    }
}
