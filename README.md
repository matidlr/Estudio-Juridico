# ⚖️ Estudio Jurídico - Sistema de Gestión Legal

Sistema web fullstack para la gestión de causas judiciales de un estudio jurídico argentino. Permite a abogados administrar expedientes, y a clientes hacer seguimiento de sus causas en tiempo real.

---

## 🚀 Stack tecnológico

**Backend:** .NET 8 · Entity Framework Core · MySQL · JWT · Serilog · FluentValidation  
**Frontend:** Angular 18 · SCSS · Standalone Components  
**Base de datos:** MySQL 8  
**Docker:** Docker Compose

---

## 📋 Funcionalidades principales

### Panel del abogado
- Gestión completa de causas (crear, editar, eliminar)
- Sistema de fojas/expediente con paginación y versionado
- Archivos y pruebas adjuntas por causa
- Consultas y mensajería con clientes
- Recordatorios y vencimientos
- Datos económicos (honorarios, gastos, pagos)
- Estadísticas del estudio
- Calendario de eventos
- Sistema de permisos entre abogados
- Notificaciones de nuevas consultas

### Portal del cliente
- Ver estado de sus causas
- Leer el expediente (fojas paginadas) con aclaraciones en lenguaje simple
- Descargar archivos adjuntos
- Enviar consultas al abogado
- Ver cuenta corriente (honorarios y pagos)
- Gestionar perfil y preferencias de notificación

---

## 🔐 Credenciales de acceso

### Administrador / SuperAdmin
| Campo | Valor |
|-------|-------|
| URL | `https://estudio-juridico-rosy.vercel.app/admin-estudio` |
| Email | `admin@estudio.com` |
| Contraseña | `Admin123` |

### Abogados
| Nombre | Email | Contraseña |
|--------|-------|------------|
| María González | `maria@estudio.com` | `Abogado123` |
| Santiago Vega | `svega@estudio.com` | `Abogado123` |
| Laura Acosta | `lacosta@estudio.com` | `Abogado123` |
| Carlos Romero | `cromero@estudio.com` | `Abogado123` |

### Clientes
| URL | `https://estudio-juridico-rosy.vercel.app/login` |
| Nombre | Email | Contraseña |
|--------|-------|------------|
| Matías De la Rosa | `matidlr@mail.com` | `Cliente123` |
| Ana Martínez | `amartinez@gmail.com` | `Cliente123` |
| Juan López | `jlopez@gmail.com` | `Cliente123` |

> El login de clientes es desde `/login`  
> El login de abogados/admin es desde `/admin-estudio`

---

## ⚙️ Instalación y configuración

### Requisitos
- .NET 8 SDK
- Node.js 18+
- MySQL 8
- Angular CLI

### Backend

```bash
cd EstudioJuridico.API2

# Crear archivo de variables de entorno
cp .env.example .env
# Editar .env con tus valores

# Restaurar dependencias
dotnet restore

# Aplicar migraciones
dotnet ef database update

# Correr el servidor
dotnet run
```

El backend corre en `https://estudio-juridico-production-0c74.up.railway.app/`
Swagger disponible en `https://estudio-juridico-production-0c74.up.railway.app/swagger/index.html`

### Frontend

```bash
cd EstudioJuridico.App

# Instalar dependencias
npm install

# Correr el servidor de desarrollo
ng serve
```

El frontend corre en `https://estudio-juridico-rosy.vercel.app/`

### Docker (alternativa)

```bash
# En la carpeta raíz del proyecto
docker-compose up -d
```

---

## 🏗️ Arquitectura

```
EstudioJuridico.API2/
├── Base/               ← BaseController, BaseService, BaseRepository
├── Controllers/        ← Endpoints REST
├── Services/           ← Lógica de negocio
├── Repositories/       ← Acceso a datos
├── Models/             ← Entidades de base de datos
├── Middleware/         ← Error handling, Token blacklist
├── Validators/         ← FluentValidation
├── Events/             ← Eventos del sistema
├── Observers/          ← Pattern Observer (notificaciones)
└── Data/               ← DbContext

EstudioJuridico.App/
├── pages/
│   ├── admin/          ← Panel del abogado
│   └── cliente/        ← Portal del cliente
├── shared/             ← Navbar, Footer, Sidebars
└── services/           ← Servicios Angular
```

### Patrones de diseño implementados
- **Repository Pattern** → Abstracción de acceso a datos
- **Observer Pattern** → Sistema de notificaciones
- **Base Classes** → BaseController, BaseService, BaseRepository

---

## 🔒 Seguridad implementada

- JWT con expiración de 2 horas
- Token blacklist para logout real
- Rate limiting (10 req/min login)
- Validación XSS con HtmlSanitizer
- Headers de seguridad HTTP
- BCrypt para contraseñas
- Logs de auditoría
- Sistema de permisos por causa

---

## 📡 Endpoints principales

```
POST /api/auth/login
POST /api/auth/logout
GET  /api/casos
GET  /api/casos/{id}
GET  /api/casos/{id}/fojas?pagina=1&porPagina=1
PUT  /api/casos/actualizacion/{id}
GET  /api/archivos/descargar/{id}
POST /api/consultas-publicas
``
