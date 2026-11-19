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
    public partial class ucVentas : UserControl
    {
        private readonly VentaServicio ventaServicio = new VentaServicio();
        private readonly DetalleVentaServicio detalleServicio = new DetalleVentaServicio();
        private readonly ClienteServicio clienteServicio = new ClienteServicio();
        private readonly EmpleadoServicio empleadoServicio = new EmpleadoServicio();
        private readonly ProductoServicio productoServicio = new ProductoServicio();

        private List<DetalleTemporal> detalles = new List<DetalleTemporal>();

        public ucVentas()
        {
            InitializeComponent();

            ConfigurarTablas();
            CargarClientes();
            CargarEmpleados();
            CargarProductos();
            CargarVentas();

            dgvHistorial.CellDoubleClick += dgvHistorial_CellDoubleClick;

            btnAgregarProducto.Click += btnAgregarProducto_Click;
            btnRegistrar.Click += btnRegistrar_Click;
            btnActualizar.Click += btnActualizar_Click;
            btnLimpiar.Click += btnLimpiar_Click;
        }


        private void dgvHistorial_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            try
            {
                int idVenta = Convert.ToInt32(
                    dgvHistorial.Rows[e.RowIndex].Cells["id_venta"].Value
                );

                using (frmDetalleVentaModal formulario =
                    new frmDetalleVentaModal(idVenta))
                {
                    formulario.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al abrir el detalle de la venta: " + ex.Message
                );
            }
        }


        private void ConfigurarTablas()
        {
            dgvDetalles.AutoGenerateColumns = true;
            dgvDetalles.ReadOnly = true;
            dgvDetalles.AllowUserToAddRows = false;
            dgvDetalles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvHistorial.AutoGenerateColumns = true;
            dgvHistorial.ReadOnly = true;
            dgvHistorial.AllowUserToAddRows = false;
            dgvHistorial.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHistorial.MultiSelect = false;
            dgvHistorial.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void CargarClientes()
        {
            cmbCliente.DataSource = clienteServicio.ObtenerClientes().ToList();
            cmbCliente.DisplayMember = "nombre";
            cmbCliente.ValueMember = "id_cliente";
            cmbCliente.SelectedIndex = -1;
        }

        private void CargarEmpleados()
        {
            cmbEmpleado.DataSource = empleadoServicio.ObtenerEmpleados().ToList();
            cmbEmpleado.DisplayMember = "nombre";
            cmbEmpleado.ValueMember = "id_empleado";
            cmbEmpleado.SelectedIndex = -1;
        }

        private void CargarProductos()
        {
            cmbProducto.DataSource = productoServicio.ObtenerProductos().ToList();
            cmbProducto.DisplayMember = "nombre";
            cmbProducto.ValueMember = "id_producto";
            cmbProducto.SelectedIndex = -1;
        }

        private void CargarVentas()
        {
            try
            {
                dgvHistorial.DataSource = null;
                dgvHistorial.DataSource = ventaServicio.ObtenerVentas().ToList();

                if (dgvHistorial.Columns["id_venta"] != null)
                    dgvHistorial.Columns["id_venta"].HeaderText = "N.º Venta";

                if (dgvHistorial.Columns["id_cliente"] != null)
                    dgvHistorial.Columns["id_cliente"].HeaderText = "Código cliente";

                if (dgvHistorial.Columns["id_empleado"] != null)
                    dgvHistorial.Columns["id_empleado"].HeaderText = "Código empleado";

                if (dgvHistorial.Columns["fecha_venta"] != null)
                    dgvHistorial.Columns["fecha_venta"].HeaderText = "Fecha";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar ventas: " + ex.Message);
            }
        }

        private void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            if (cmbProducto.SelectedItem == null)
            {
                MessageBox.Show("Selecciona un producto.");
                return;
            }

            if (!int.TryParse(txtCantidad.Text, out int cantidad) || cantidad <= 0)
            {
                MessageBox.Show("Ingresa una cantidad válida.");
                return;
            }

            Producto producto = cmbProducto.SelectedItem as Producto;

            if (producto == null)
                return;

            if (cantidad > producto.stock)
            {
                MessageBox.Show("No hay suficiente stock disponible.");
                return;
            }

            DetalleTemporal existente = detalles.FirstOrDefault(
                d => d.id_producto == producto.id_producto
            );

            int cantidadAcumulada = cantidad;

            if (existente != null)
                cantidadAcumulada += existente.cantidad;

            if (cantidadAcumulada > producto.stock)
            {
                MessageBox.Show("La cantidad total supera el stock disponible.");
                return;
            }

            if (existente != null)
            {
                existente.cantidad += cantidad;
                existente.subtotal = existente.cantidad * existente.precio_unitario;
            }
            else
            {
                detalles.Add(new DetalleTemporal
                {
                    id_producto = producto.id_producto,
                    nombre = producto.nombre,
                    cantidad = cantidad,
                    precio_unitario = producto.precio,
                    subtotal = cantidad * producto.precio
                });
            }

            dgvDetalles.DataSource = null;
            dgvDetalles.DataSource = detalles.ToList();

            CalcularTotal();
            txtCantidad.Clear();
        }

        private void CalcularTotal()
        {
            decimal total = detalles.Sum(d => d.subtotal);
            lblTotalVenta.Text = total.ToString("C2");
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (cmbCliente.SelectedValue == null ||
                cmbEmpleado.SelectedValue == null)
            {
                MessageBox.Show("Selecciona un cliente y un empleado.");
                return;
            }

            if (detalles.Count == 0)
            {
                MessageBox.Show("Agrega al menos un producto a la venta.");
                return;
            }

            try
            {
                Venta venta = new Venta
                {
                    id_cliente = Convert.ToInt32(cmbCliente.SelectedValue),
                    id_empleado = Convert.ToInt32(cmbEmpleado.SelectedValue),
                    fecha_venta = dtpFecha.Value
                };

                int idVenta = ventaServicio.RegistrarVenta(venta);

                if (idVenta <= 0)
                {
                    MessageBox.Show("No se pudo obtener el número de la venta.");
                    return;
                }

                foreach (DetalleTemporal item in detalles)
                {
                    DetalleVenta detalle = new DetalleVenta
                    {
                        id_venta = idVenta,
                        id_producto = item.id_producto,
                        cantidad = item.cantidad,
                        precio_unitario = item.precio_unitario
                    };

                    detalleServicio.RegistrarDetalle(detalle);
                }

                MessageBox.Show("¡Venta registrada correctamente!");

                LimpiarCampos();
                CargarVentas();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar la venta: " + ex.Message);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (dgvHistorial.CurrentRow == null)
            {
                MessageBox.Show("Selecciona una venta de la tabla.");
                return;
            }

            if (cmbCliente.SelectedValue == null ||
                cmbEmpleado.SelectedValue == null)
            {
                MessageBox.Show("Selecciona un cliente y un empleado.");
                return;
            }

            try
            {
                Venta venta = dgvHistorial.CurrentRow.DataBoundItem as Venta;

                if (venta == null)
                    return;

                venta.id_cliente = Convert.ToInt32(cmbCliente.SelectedValue);
                venta.id_empleado = Convert.ToInt32(cmbEmpleado.SelectedValue);
                venta.fecha_venta = dtpFecha.Value;

                ventaServicio.ActualizarVenta(venta);

                MessageBox.Show("Venta actualizada correctamente.");

                CargarVentas();
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
            cmbCliente.SelectedIndex = -1;
            cmbEmpleado.SelectedIndex = -1;
            cmbProducto.SelectedIndex = -1;

            dtpFecha.Value = DateTime.Now;
            txtCantidad.Clear();

            detalles.Clear();
            dgvDetalles.DataSource = null;

            lblTotalVenta.Text = "$0.00";
            dgvHistorial.ClearSelection();
        }

        private class DetalleTemporal
        {
            public int id_producto { get; set; }
            public string nombre { get; set; }
            public int cantidad { get; set; }
            public decimal precio_unitario { get; set; }
            public decimal subtotal { get; set; }
        }

     
    }
}

