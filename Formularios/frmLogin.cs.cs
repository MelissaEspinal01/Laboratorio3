using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SistemaVentasElectronicas.Servicios;
using SistemaVentasElectronicas;

namespace SistemaVentasElectronicas.Formularios
{
    public partial class frmLogin : Form
    {
        private readonly LoginServicio loginServicio =
            new LoginServicio();

        public frmLogin()
        {
            InitializeComponent();
            btnIngresar.Click += btnIngresar_Click;
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) ||
                string.IsNullOrWhiteSpace(txtContraseña.Text))
            {
                MessageBox.Show(
                    "Ingresa tu usuario y contraseña.",
                    "Campos obligatorios",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            try
            {
                bool acceso = loginServicio.ValidarUsuario(
                    txtUsuario.Text.Trim(),
                    txtContraseña.Text
                );

                if (acceso)
                {
                    MessageBox.Show(
                        "¡Bienvenido al sistema!",
                        "Acceso correcto",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    this.Hide();

                    using (frmPrincipal principal = new frmPrincipal())
                    {
                        principal.ShowDialog();
                    }

                    this.Close();
                }
                else
                {
                    MessageBox.Show(
                        "Usuario o contraseña incorrectos.",
                        "Acceso denegado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );

                    txtContraseña.Clear();
                    txtContraseña.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error de inicio de sesión",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}

