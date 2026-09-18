# Pokédex MVC - Aplicación Web ASP.NET Core

Aplicación web desarrollada con ASP.NET Core MVC para la gestión y visualización de pokémons mediante el consumo de la PokeAPI.

## 📋 Descripción

Esta aplicación implementa una Pokédex completa que permite:
- ✅ Visualizar una lista paginada de pokémons
- ✅ Ver detalles completos de cada pokémon (estadísticas, tipos, habilidades)
- ✅ Navegar entre pokémons (anterior/siguiente)
- ✅ Buscar pokémons por nombre
- ✅ Interfaz responsive y atractiva

## 🏗️ Arquitectura y Patrones de Diseño

### Arquitectura MVC (Model-View-Controller)
- **Models**: Representación de datos de pokémon
- **Views**: Interfaces de usuario con Razor
- **Controllers**: Lógica de control de flujo

### Patrones Implementados

1. **Repository Pattern**
   - `IPokemonRepository` / `PokemonRepository`
   - Abstracción del acceso a datos de la API externa

2. **Service Layer Pattern**
   - `IPokemonService` / `PokemonService`
   - Lógica de negocio separada del controlador

3. **Dependency Injection**
   - Inyección de dependencias en `Program.cs`
   - Inversión de control para mejor testabilidad

4. **ViewModels**
   - `PokemonListViewModel` para separar la lógica de presentación

## 🔒 Buenas Prácticas de Seguridad Implementadas

### 1. Headers de Seguridad
- `X-Content-Type-Options: nosniff` - Previene MIME-sniffing
- `X-Frame-Options: DENY` - Protección contra clickjacking
- `X-XSS-Protection` - Protección contra XSS
- `Content-Security-Policy` - Política de seguridad de contenido
- `Referrer-Policy` - Control de información de referencia

### 2. HTTPS y HSTS
- Redirección automática a HTTPS
- HSTS configurado con 1 año de duración
- Preload y subdominios incluidos

### 3. Validación de Entrada
- Validación de parámetros en Repository
- Sanitización de nombres con expresiones regulares
- Límites en rangos numéricos (offset, limit, id)

### 4. Manejo de Errores
- Try-catch en todas las operaciones de red
- Logging de errores sin exponer información sensible
- Páginas de error personalizadas

### 5. HttpClient Configurado
- Timeout definido (30 segundos)
- Base address configurada
- User-Agent establecido

### 6. Logging
- ILogger implementado en todos los servicios
- Registro de operaciones y errores
- Sin exposición de datos sensibles

## 🛠️ Tecnologías Utilizadas

- **ASP.NET Core 8.0** - Framework web
- **C# 12** - Lenguaje de programación
- **Razor Pages** - Motor de vistas
- **Newtonsoft.Json** - Serialización JSON
- **HttpClient** - Consumo de API REST
- **PokeAPI** - API pública de pokémons

## 📁 Estructura del Proyecto

```
PokemonMVC/
├── Controllers/
│   ├── PokemonController.cs    # Controlador principal
│   └── HomeController.cs       # Controlador de inicio
├── Models/
│   ├── Pokemon.cs              # Modelo principal
│   ├── PokemonListResponse.cs  # Modelo de respuesta API
│   └── PokemonViewModel.cs     # ViewModels
├── Repositories/
│   ├── IPokemonRepository.cs   # Interface del repositorio
│   └── PokemonRepository.cs    # Implementación del repositorio
├── Services/
│   ├── IPokemonService.cs      # Interface del servicio
│   └── PokemonService.cs       # Lógica de negocio
├── Views/
│   ├── Pokemon/
│   │   ├── Index.cshtml        # Lista de pokémons
│   │   └── Details.cshtml      # Detalles del pokémon
│   └── Shared/
│       ├── _Layout.cshtml      # Layout principal
│       └── Error.cshtml        # Página de error
├── wwwroot/
│   └── css/
│       └── site.css            # Estilos personalizados
├── Program.cs                  # Configuración de la aplicación
└── appsettings.json           # Configuración
```

## 🚀 Instalación y Ejecución

### Prerrequisitos
- .NET SDK 8.0 o superior
- Visual Studio 2022 (recomendado) o Visual Studio Code
- Git

### Pasos de Instalación

1. **Clonar el repositorio**
```bash
git clone <url-del-repositorio>
cd PokemonMVC
```

2. **Restaurar paquetes NuGet**
```bash
dotnet restore
```

3. **Compilar el proyecto**
```bash
dotnet build
```

4. **Ejecutar la aplicación**
```bash
dotnet run --project PokemonMVC
```

5. **Abrir en el navegador**
```
https://localhost:5001
```

### Ejecución con Visual Studio

1. Abrir `PokemonMVC.sln` en Visual Studio
2. Presionar F5 o hacer clic en "Start"
3. La aplicación se abrirá automáticamente en el navegador

## 📱 Funcionalidades

### Lista de Pokémons
- Visualización en grid responsive
- Paginación configurable (12, 20, 30, 50 pokémons por página)
- Navegación entre páginas (Primera, Anterior, Siguiente, Última)
- Imágenes de alta calidad
- Numeración oficial de Pokédex

### Detalles del Pokémon
- Imagen de alta resolución
- Información básica (altura, peso)
- Tipos con colores distintivos
- Lista de habilidades
- Estadísticas base con barras visuales
- Navegación rápida (anterior/siguiente)

### Búsqueda
- Búsqueda por nombre desde el navbar
- Redirección automática a los detalles
- Manejo de errores si no se encuentra

## 🎨 Características de Diseño

- **Responsive Design**: Adaptado a móviles, tablets y escritorio
- **Paleta de Colores**: Inspirada en Pokémon (rojo y amarillo)
- **Animaciones**: Transiciones suaves en hover
- **Tipografía**: Moderna y legible
- **Accesibilidad**: Semántica HTML correcta

## 📊 API Consumida

**PokeAPI v2**
- Endpoint base: `https://pokeapi.co/api/v2/`
- Endpoints utilizados:
  - `GET /pokemon?offset={offset}&limit={limit}` - Lista de pokémons
  - `GET /pokemon/{id}` - Detalles por ID
  - `GET /pokemon/{name}` - Detalles por nombre

## 🧪 Testing (Opcional)

Para agregar tests unitarios:

```bash
dotnet new xunit -n PokemonMVC.Tests
dotnet add PokemonMVC.Tests reference PokemonMVC
```

## 📝 Licencia

Este proyecto es de uso educativo para la materia de Calidad de Software.

## 👨‍💻 Autor

Desarrollado como proyecto académico para la evaluación de arquitectura MVC y buenas prácticas de seguridad.

## 🔗 Referencias

- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [PokeAPI Documentation](https://pokeapi.co/docs/v2)
- [OWASP Security Guidelines](https://owasp.org)

---

**Nota**: Este proyecto implementa las mejores prácticas de seguridad web, patrones de diseño reconocidos y una arquitectura MVC limpia y mantenible.
