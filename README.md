# Sistema de Ventas Electrónicas

## Descripción del proyecto

Sistema de escritorio desarrollado en C# Windows Forms para administrar las operaciones de una empresa dedicada a la venta de productos electrónicos.

El sistema permite gestionar empleados, clientes, productos, ventas y detalles de ventas, además de consultar reportes de productos vendidos e inventario.

El proyecto utiliza SQL Server como sistema de gestión de bases de datos y RepoDb como ORM para facilitar las operaciones de acceso a datos.

## Tecnologías utilizadas

- C#
- Windows Forms
- SQL Server
- RepoDb
- Microsoft.Data.SqlClient
- SHA-256 para la protección de contraseñas
- Git y GitHub para el control de versiones

## Funcionalidades principales

### 1. Inicio de sesión
- Autenticación mediante usuario y contraseña.
- Comparación de contraseñas utilizando hashes SHA-256.

### 2. Gestión de empleados
- Consulta de empleados registrados.
- Registro y actualización de información.

### 3. Gestión de clientes
- Consulta de clientes.
- Registro y actualización de información.

### 4. Gestión de productos
- Registro de productos.
- Consulta de productos.
- Actualización de precios y existencias.

### 5. Gestión de ventas
- Selección de clientes y empleados.
- Agregar productos y cantidades.
- Cálculo del total de la venta.
- Registro de ventas.

### 6. Detalle de ventas
- Consulta de productos incluidos en una venta.
- Visualización de cantidades, precios y subtotales.
- Consulta mediante número de venta, cliente y fecha.
- Visualización del detalle en una ventana modal.

### 7. Reportes
- Reporte de productos vendidos, cantidades y montos.
- Reporte general de inventario.
- Identificación visual de productos con existencias bajas.

## Arquitectura del proyecto

El proyecto está organizado en diferentes carpetas para separar las responsabilidades:

- **Datos:** conexión con SQL Server.
- **Entidades:** clases que representan los datos del sistema.
- **Interfaces:** definición del repositorio genérico.
- **Repositorios:** operaciones de acceso a datos mediante RepoDb.
- **Servicios:** lógica de negocio y autenticación.
- **Formularios:** interfaces gráficas del sistema.

## Base de datos

Nombre de la base de datos: `VentasElectronicas`.

Entre las tablas principales se encuentran:

- Empleados
- Clientes
- Productos
- Ventas
- DetalleVenta

## Configuración y ejecución

1. Descargar o clonar el repositorio.
2. Abrir la solución en Visual Studio.
3. Crear o restaurar la base de datos `VentasElectronicas` en SQL Server.
4. Configurar la cadena de conexión en la clase `ConexionBD.cs`, utilizando el nombre de servidor correspondiente a la computadora.
5. Restaurar los paquetes NuGet necesarios.
6. Ejecutar el proyecto desde Visual Studio.


Melissa Abigail Espinal Mejía

Proyecto académico desarrollado en C# Windows Forms.
