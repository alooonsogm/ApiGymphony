# 🏋️‍♂️ Gymphony API REST (.NET 10)

Backend principal de **Gymphony**, una plataforma de gestión integral para gimnasios. Esta API centraliza toda la lógica de negocio, acceso a datos y seguridad, diseñada para ser consumida de forma eficiente por cualquier aplicación cliente.

🌍 **[Explorar la API en Azure (Scalar)] https://apigymphony.azurewebsites.net/**

## 🚀 El Reto: Migración a la Nube
Este proyecto nació originalmente como una aplicación monolítica tradicional. Sin embargo, para mejorar su escalabilidad y seguridad, decidí refactorizarlo por completo y migrarlo a una arquitectura orientada a servicios 100% en la nube de Azure.

### 🛠️ Arquitectura y Decisiones Técnicas:
* **Desacoplamiento con NuGet:** Para evitar duplicar código entre la API y el Frontend, extraje todas las entidades y DTOs a una biblioteca de clases y creé **mi propio paquete NuGet** (publicado en nuget.org), el cual instalo en ambos proyectos.
* **Seguridad y JWT:** Implementé un sistema de autenticación basado en **JSON Web Tokens (JWT)**. Los endpoints están protegidos y autorizan acciones dependiendo del rol del usuario (Socio, Entrenador o Administrador).
* **Gestión de Archivos (Azure Blob Storage):** 
  * Los recursos estáticos como el logo se sirven desde un contenedor público.
  * Las fotos de perfil de los usuarios se suben a un **contenedor privado**. En la base de datos solo se guarda el nombre del archivo, y la API genera un **Token SAS (Shared Access Signature)** para otorgar acceso temporal y seguro a las imágenes.
* **Cero Secretos en Código (Azure Key Vault):** Toda la información sensible (cadenas de conexión a la BD, Connection Strings del Blob Storage, claves de encriptación y el Issuer/Secret del JWT) fue extraída del `appsettings.json` y se gestiona de forma segura a través de **Azure Key Vault**.
* **Base de Datos:** Migración completa a **Azure SQL Database**.
* **Despliegue:** API alojada en la nube mediante **Azure App Service**.

## ⚙️ Tecnologías Utilizadas
* C# y .NET 10 (Web API)
* Entity Framework Core & LINQ
* Azure App Service, Azure SQL, Azure Blob Storage, Azure Key Vault
* Autenticación JWT Bearer
* Scalar / OpenAPI

## 💻 Instalación Local
Si deseas levantar la API en tu entorno local:
1. Clona este repositorio.
2. Necesitarás configurar tus propias credenciales en un `appsettings.json` local (o conectar tu entorno a tu Key Vault de Azure).
3. Asegúrate de descargar el paquete NuGet de modelos.
4. Compila y ejecuta el proyecto. Se abrirá la interfaz de Scalar para probar los endpoints.
