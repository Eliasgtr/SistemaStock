# Sistema de Gestión de Stock (P3)

Proyecto web en ASP.NET Core MVC para gestionar inventario (stock). Incluye login/registro, y módulos de Productos y Categorías, conectados a SQL Server usando Entity Framework Core.

📌 Funcionalidades

✅ Módulo de Cuenta

Registro de usuarios

Inicio de sesión

Cierre de sesión

✅ Módulo de Inventario

Gestión de Productos (CRUD)

Gestión de Categorías (CRUD)

Relación Producto → Categoría

✅ Base de datos operativa

Conexión a SQL Server

Contexto EF Core (SistemaStockContext)

🧱 Tecnologías usadas

.NET (ASP.NET Core MVC)

Entity Framework Core

SQL Server

Razor Views (Front-end MVC)

📂 Estructura del proyecto

Controllers/ → Controladores (Cuenta, Productos, Categorías)

Models/ → Entidades y DbContext

Views/ → Vistas Razor (UI)

wwwroot/ → CSS, JS, librerías estáticas

Program.cs → Configuración (DI, sesión, rutas)

✅ Requisitos

Antes de correr el proyecto necesitas:

Visual Studio 2022 (o VS Code)

.NET SDK instalado

SQL Server (LocalDB o Instancia)

(Opcional) SQL Server Management Studio (SSMS)

⚙️ Configuración de la Base de Datos

1) Crear la base de datos
   
En SQL Server crea una base de datos con el nombre que vas a usar, por ejemplo:

SistemaStock (recomendado)

3) Configurar el Connection String
   
En el archivo appsettings.json, cambia tu conexión según tu SQL Server.

Ejemplo:

"ConnectionStrings": {
  "ConexionSQL": "Server=TU_SERVIDOR;Database=SistemaStock;Trusted_Connection=True;TrustServerCertificate=True"
}

📌 Nota: Si tu servidor es local, puede verse así:

Server=localhost

Server=DESKTOP-NOMBRE\\SQLEXPRESS

Server=TU_PC\\MSSQLSERVER

▶️ Cómo ejecutar el proyecto

Opción A: Desde Visual Studio

Abre la solución en Visual Studio

Selecciona el proyecto como startup

Dale a Run (IIS Express o HTTPS)

Opción B: Desde terminal (CLI)

En la carpeta del proyecto ejecuta:

dotnet restore

dotnet run

Luego abre el link que salga en consola, normalmente:

https://localhost:xxxx

http://localhost:xxxx

🗃️ Migraciones

Si el proyecto usa migraciones EF Core, puedes correr:

dotnet ef migrations add InitialCreate

dotnet ef database update

Si ya tienes la base creada y las tablas existen, no hace falta.

👤 Login

Primero crea un usuario en Registro

Luego inicia sesión en Login

Al iniciar sesión te manda al módulo de productos

📝 Estado del proyecto

✅ Funcionalidades base completadas:

Login/Registro (Cuenta)

CRUD Productos

CRUD Categorías

Conexión a SQL Server


