# 🏠 RealEstate API

API RESTful para la gestión de propiedades inmobiliarias y propietarios, desarrollada con **.NET 8**, **MongoDB** y **Clean Architecture**, aplicando principios **SOLID**, **CQRS** y almacenamiento de imágenes en **Firebase Storage**.

---

## 🚀 Tecnologías principales

- .NET 8
- MongoDB
- Firebase Storage (para imágenes)
- Clean Architecture + DDD
- CQRS con MediatR
- FluentValidation
- Swagger / OpenAPI
- Variables de entorno para configuración

---


## 🧱 Arquitectura del proyecto

Estructura basada en Clean Architecture con separación de responsabilidades por capas:

```
RealEstate/
├── RealEstate.API/ → Proyecto ASP.NET Core (controllers, middlewares, Swagger)
├── RealEstate.Application/ → Lógica de negocio, casos de uso, CQRS, DTOs, validaciones
├── RealEstate.Domain/ → Entidades del dominio (Property, Owner), interfaces
├── RealEstate.Infrastructure/→ Implementaciones de base de datos, almacenamiento, servicios externos
├── RealEstate.Shared/ → Constantes, excepciones, helpers comunes
├── Application.Tests/ → Pruebas unitarias y de integración
└── README.md
```
---

## Configuración base de datos MongoDB

1. Restaura el backup de la base de datos contenida en el directorio /RealEstateDB.
2. edita el nodo MongoSettings.json en el archivo /API/appsettings.json con tu cadena de conexion.

## Variables de entorno requeridas
Crea en tu sistema una variable de entorno

1. Abre tu Windows PowerShell como adminitrador
2. Crea la variable de entorno FIREBASE_KEY_JSON
```bash
$json = 'copia y pega el contenido del archivo enviado como adjunto al buzón de correo'
[System.Environment]::SetEnvironmentVariable("FIREBASE_KEY_JSON", $json, "Machine")
```

## ▶️ Ejecución local

1. Clona el repositorio:

```bash
git clone https://gitlab.com/yeisermercado/realestate-api.git
cd realestate-api
```

2. Restaura paquetes y ejecuta:

```bash
dotnet restore
dotnet run --project API
```

3. Accede a la documentación Swagger en:

```bash
http://localhost:5001/swagger
```

4. Instalar y confiar el certificado de desarrollo ejecutando:

```bash
dotnet dev-certs https --clean
dotnet dev-certs https --trust
```
---

## ✅ Comandos útiles

```bash
# Ejecutar pruebas
dotnet test

# Limpiar y reconstruir
dotnet clean
dotnet build
```
---

## 📸 Subida de imágenes

Las imágenes de propiedades y propietarios se almacenan en Google Cloud Storage. Las URLs públicas se guardan en MongoDB como parte de las entidades.
---

## 🔧 Principales patrones implementados

- Clean Architecture
- CQRS con MediatR
- Inyección de dependencias
- FluentValidation para validación de datos
- Firebase Storage Adapter en capa de infraestructura
- Configuración desacoplada con variables de entorno
---

## ✍️ Autor
Desarrollado por Yeiser Smith Mercado.
---

## Licencia
Este proyecto está licenciado bajo la MIT License.