# ASP.NET Core MVC Web Server Overview

This document covers **ASP.NET Core MVC** for server-side development, including architecture patterns like **Clean Architecture** and **CQRS**.

---

## ASP.NET Core Overview

ASP.NET Core is a cross-platform, high-performance framework for building modern web applications. It allows developers to create:

- Web APIs
- MVC Web Applications
- Blazor Server Applications
- gRPC services

Key features:

- Cross-platform (Windows, Linux, macOS)
- Built-in dependency injection
- Middleware-based HTTP request pipeline
- Integration with Entity Framework Core

### Creating a New MVC Project

From a terminal (Codespaces, Bash, or PowerShell):

```bash
# Create a new ASP.NET Core MVC project
dotnet new mvc -n MyMvcApp
cd MyMvcApp

# Run the project
dotnet run
```

This generates the initial MVC project template.

---

## MVC Architecture

**MVC** stands for **Model-View-Controller**:

```
Request --> Controller --> Model --> View --> Response
```

- **Model**: Represents data and business logic.
- **View**: Handles UI rendering (HTML, Razor syntax).
- **Controller**: Handles HTTP requests, interacts with Models, and returns Views or API responses.

**Flow Example**:

1. Browser sends a request to `/Products/Index`.
2. `ProductsController` retrieves data from a Model or service.
3. Controller passes data to the `Index.cshtml` View.
4. Razor engine renders HTML.
5. Response is sent back to the browser.

---

## Project Structure

Typical **ASP.NET Core MVC** project tree:

```
MyMvcApp/
├─ Controllers/        # Handles requests
│   └─ HomeController.cs
├─ Models/             # Application data and business logic
│   └─ Product.cs
├─ Views/              # Razor pages (UI)
│   ├─ Home/
│   │   └─ Index.cshtml
│   └─ Shared/
│       └─ _Layout.cshtml
├─ wwwroot/            # Static files (JS, CSS, images)
├─ Program.cs          # Application entry point
├─ Startup.cs          # Configures services and middleware (if used)
├─ appsettings.json    # Configuration
└─ MyMvcApp.csproj     # Project file
```

**Key Points:**

- `Controllers` handle routing and HTTP verbs.
- `Models` encapsulate data and rules.
- `Views` render HTML dynamically using Razor.
- `wwwroot` serves static assets, e.g., JS/CSS/images.
- `Program.cs` sets up the server, middleware, and endpoints.

---

## Request Lifecycle

1. **Browser Request**: User navigates to a URL.
2. **Routing**: ASP.NET Core determines which Controller/action to execute.
3. **Middleware Pipeline**: Each middleware can modify the request or response.
4. **Controller Action Execution**: Logic and data retrieval.
5. **View Rendering**: Razor engine combines HTML and model data.
6. **Response Sent**: Browser receives the final HTML/JSON.

---

## Clean Architecture

**Clean Architecture** focuses on **separating concerns**:

- **Domain Layer**: Core business rules.
- **Application Layer**: Use cases, services, and interfaces.
- **Infrastructure Layer**: Database, external APIs.
- **Presentation Layer**: MVC Controllers and Views.

**Benefits**:

- Independent of frameworks and UI
- Easier testing
- Maintainable and scalable

```
Browser --> Presentation Layer --> Application Layer --> Domain Layer --> Infrastructure Layer
```

---

## CQRS (Command Query Responsibility Segregation)

CQRS separates operations into:

- **Commands**: Modify state (Create/Update/Delete)
- **Queries**: Retrieve data without side effects

**Benefits**:

- Clear separation of read vs. write concerns
- Scales well for complex applications
- Works well with event sourcing

```
Write (Command) --> Domain/Infrastructure --> Database
Read (Query)  --> Domain/Infrastructure --> Database
```

---

## MVC vs Clean Architecture vs CQRS

| Pattern                  | Focus                                         | Typical Use Case |
|---------------------------|-----------------------------------------------|-----------------|
| **MVC**                  | UI + request handling                         | Small to medium web apps |
| **Clean Architecture**    | Separation of concerns, testable core        | Large, maintainable apps |
| **CQRS**                  | Separating reads/writes for complex systems | High-performance, event-driven apps |

**Summary**:

- **MVC**: Best for straightforward request/response apps.
- **Clean Architecture**: Ensures independence and testability of business logic.
- **CQRS**: Ideal when reads and writes require different models or optimizations.

---

## References

- [Microsoft ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [Clean Architecture Principles](https://8thlight.com/blog/uncle-bob/2012/08/13/the-clean-architecture.html)
- [CQRS Basics](https://martinfowler.com/bliki/CQRS.html)