# 📦 Sistema de Gestión de Stock

Un sistema web moderno y profesional desarrollado en **.NET 9** bajo el patrón de **Clean Architecture** (Arquitectura Limpia) para la administración eficiente de inventarios (productos, categorías y movimientos de stock).

---

## ✨ Características Principales

### 🏗️ Arquitectura Limpia (Clean Architecture)
El sistema ha sido estructurado en 4 capas físicas totalmente desacopladas, lo que garantiza una separación clara de responsabilidades, alta mantenibilidad y facilidad para pruebas unitarias:
* **Dominio (`Dominio/`)**: Contiene las entidades puras del negocio (`Producto`, `Categoria`, `Usuario`, etc.). Sin dependencias externas.
* **Aplicación (`Aplicacion/`)**: Alberga las reglas de aplicación y los modelos de vista compartidos (`ViewModels`).
* **Infraestructura (`Infraestructura/`)**: Encargada del acceso a datos, migraciones de Entity Framework Core y persistencia física en base de datos.
* **Presentación (`Presentacion/`)**: Aplicación web ASP.NET Core MVC con controladores, filtros de autorización, recursos estáticos y vistas.

### 🎨 Interfaz de Usuario Premium
* **Login y Registro Renovados**: Pantallas con estética premium basada en **Glassmorphic Design** (efecto de vidrio esmerilado, desenfoque de fondo y sombras suaves), degradados animados y un botón interactivo para mostrar/ocultar contraseñas.
* **Responsive Design**: Adaptado por completo para visualizarse en computadoras, tablets y teléfonos móviles con transiciones fluidas.

### 🔔 Módulo de Notificaciones de Stock
* **Insignia Dinámica en Menú Lateral**: Un contador numérico rojo indica en tiempo real cuántos productos requieren atención urgente (con stock agotado o por debajo del stock mínimo parametrizado).
* **Bandeja de Entrada de Alertas**: Un panel dedicado que clasifica las advertencias visualmente:
  * 🔴 **Agotado**: Stock en 0 (acción recomendada urgente).
  * 🟡 **Stock Bajo**: Productos que alcanzan o están por debajo del límite mínimo.
* **Acción de Reabastecimiento Rápido**: Botón directo de *"Registrar Entrada"* en cada alerta que permite sumar unidades de stock al producto con un solo clic.

---

## 🛠️ Tecnologías Utilizadas

* **Framework Principal**: .NET 9.0 (ASP.NET Core MVC)
* **ORM / Base de Datos**: Entity Framework Core 9.0, SQL Server (LocalDB)
* **Maquetado**: HTML5, Vanilla CSS (Estilos personalizados e interactividad CSS/JS)
* **Framework CSS**: Bootstrap 5.3 + Bootstrap Icons
* **Validación**: Client-Side Validation (jQuery Validation)

---

## 📂 Estructura del Repositorio

```text
/ (Raíz)
├── Dominio/              # Entidades puras y constantes del sistema
├── Aplicacion/           # ViewModels de lógica e intermediarios
├── Infraestructura/      # DbContext y Migraciones de base de datos
├── Presentacion/         # Controladores, Vistas, Filtros y archivos estáticos (wwwroot)
└── SistemaStock.sln      # Archivo de solución general
```

---

## ⚙️ Configuración y Requisitos

### Requisitos Previos
* .NET 9.0 SDK
* SQL Server (LocalDB o instancia de base de datos externa)
* Visual Studio 2022 o Visual Studio Code

### 1. Conexión a Base de Datos
Edita la cadena de conexión en el archivo [Presentacion/appsettings.json](file:///c:/Users/EDGARDO/Desktop/itla-materias/2026-C2/Programacion%203/SistemaGestionStock-P3-main/Presentacion/appsettings.json) para apuntar a tu base de datos SQL Server:

```json
"ConnectionStrings": {
  "ConexionSQL": "Server=(localdb)\\MSSQLLocalDB;Database=SistemaStock;Trusted_Connection=True;TrustServerCertificate=True"
}
```

### 2. Ejecutar Migraciones (Base de Datos)
Para crear las tablas de la base de datos a partir del historial de migraciones de la capa de Infraestructura, ejecuta los siguientes comandos desde tu terminal en la raíz del proyecto:

```bash
# Instalar herramientas de EF si no las tienes
dotnet tool install --global dotnet-ef

# Aplicar las migraciones a la base de datos
dotnet ef database update --project Infraestructura --startup-project Presentacion
```

---

## ▶️ Ejecución del Proyecto

### Opción A: Desde Terminal
Dirígete a la raíz del espacio de trabajo y ejecuta el proyecto de presentación:

```bash
dotnet run --project Presentacion
```

Una vez iniciado, abre tu navegador en las rutas configuradas en la consola:
* **HTTP**: `http://localhost:5258`
* **HTTPS**: `https://localhost:7066`

### Opción B: Desde Visual Studio
1. Abre la solución `SistemaStock.sln`.
2. Establece el proyecto **`SistemaStock.Web`** (ubicado en la carpeta `Presentacion/`) como **Proyecto de Inicio (Startup Project)**.
3. Presiona `F5` o el botón de Iniciar.

---

## 👤 Primeros Pasos
1. Abre la aplicación y haz clic en **"Crear cuenta"** en la pantalla de registro.
2. Regístrate e inicia sesión con las nuevas credenciales.
3. Se te redirigirá al inventario, donde podrás crear categorías, gestionar productos y llevar el control de entradas y salidas de stock desde los detalles de cada producto.
