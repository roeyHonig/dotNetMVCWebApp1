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


# Authentication, Authorization, JWT, OAuth2 and RBAC

## Overview

Modern web applications rarely expose APIs publicly.

Most APIs require multiple layers of security:

```text
REST API
    ↓
HTTP Verbs
    ↓
Controllers
    ↓
Swagger
    ↓
DTOs
    ↓
Validation
    ↓
Authentication (JWT)
    ↓
Authorization (RBAC)
    ↓
OAuth2 / OpenID Connect
    ↓
Blazor / Front-End
```

The goal is simple:

* Verify who the user is
* Verify what the user is allowed to do
* Protect sensitive endpoints

---

# Authentication vs Authorization

These concepts are often confused.

Authentication answers:

```text
Who are you?
```

Authorization answers:

```text
What are you allowed to do?
```

Example:

```text
User Login
     |
     v
Authentication
     |
     v
Identity Established
     |
     v
Authorization
     |
     v
Permission Check
```

A user may successfully authenticate but still be denied access to certain operations.

---

# JWT Authentication

JWT stands for:

```text
JSON Web Token
```

A JWT is a digitally signed token that proves the identity of a user.

Instead of storing session state on the server, the server generates a token and sends it to the client.

The client stores the token and sends it with every future request.

Advantages:

* Stateless
* Fast
* Scalable
* Commonly used in APIs

---

# JWT Authentication Flow

```text
Client
  |
  | 1. Login (username/password)
  v
Auth Server (your API)
  |
  | 2. Validate credentials
  |
  | 3. Generate JWT
  v
Client receives JWT
  |
  | 4. Store token
  |
  | 5. Send:
  |    Authorization: Bearer <JWT>
  v
API Middleware
  |
  | 6. Validate token
  |
  +--> Valid -> Continue
  |
  +--> Invalid -> Reject
  v
Controller
```

---

# Login Example

Client sends:

```http
POST /api/auth/login
```

Request body:

```json
{
  "username": "john",
  "password": "password123"
}
```

Server validates credentials and returns:

```json
{
  "token": "eyJhbGciOiJIUzI1NiIs..."
}
```

---

# JWT Structure

A JWT consists of three parts:

```text
HEADER.PAYLOAD.SIGNATURE
```

Example:

```text
xxxxx.yyyyy.zzzzz
```

---

# JWT Header

Contains metadata about the token.

Example:

```json
{
  "alg": "HS256",
  "typ": "JWT"
}
```

Meaning:

```text
Algorithm = HMAC SHA256
Type      = JWT
```

---

# JWT Payload

Contains claims.

Example:

```json
{
  "sub": "123",
  "name": "John",
  "role": "Admin",
  "exp": 1750000000
}
```

Common claims:

| Claim | Meaning    |
| ----- | ---------- |
| sub   | User ID    |
| name  | User Name  |
| email | Email      |
| role  | Role       |
| exp   | Expiration |

---

# JWT Signature

The signature prevents tampering.

```text
Header
   +
Payload
   +
Secret Key
   ↓
Signature
```

If someone modifies:

```text
role = User
```

to:

```text
role = Admin
```

the signature becomes invalid.

ASP.NET Core rejects the token.

---

# JWT Validation Flow

Every protected request passes through authentication middleware.

```text
Request
   |
Authorization: Bearer <JWT>
   |
   v
Authentication Middleware
   |
   +--> Validate Signature
   |
   +--> Validate Expiration
   |
   +--> Validate Claims
   |
   v
Controller
```

---

# ASP.NET Core JWT Configuration

Register JWT authentication:

```csharp
builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSecret))
            };
    });
```

Enable middleware:

```csharp
app.UseAuthentication();
app.UseAuthorization();
```

---

# Protecting Endpoints

Require authentication:

```csharp
[Authorize]
[HttpGet]
public IActionResult SecretData()
{
    return Ok("Protected Data");
}
```

Only authenticated users may access the endpoint.

---

# Authentication Results

```text
No Token
    ↓
401 Unauthorized
```

```text
Invalid Token
    ↓
401 Unauthorized
```

```text
Valid Token
    ↓
200 OK
```

---

# JWT Debugging

Useful website:

https://jwt.io

Paste a JWT token and inspect:

* Header
* Payload
* Claims
* Expiration

Never paste production secrets.

---

# Role Based Access Control (RBAC)

RBAC stands for:

```text
Role Based Access Control
```

Instead of assigning permissions directly to users, permissions are assigned to roles.

Example:

```text
Admin
   |
   +--> Create Products
   +--> Update Products
   +--> Delete Products

User
   |
   +--> View Products

Guest
   |
   +--> View Public Data
```

---

# RBAC Flow

```text
User Login
     |
     v
JWT Contains Role Claim
     |
     v
Request Arrives
     |
     v
Authorization Middleware
     |
     v
Role Check
     |
     +--> Allowed
     |
     +--> Denied
```

---

# JWT with Role Claims

Example JWT payload:

```json
{
  "name": "john",
  "role": "Admin"
}
```

---

# ASP.NET Core Role Authorization

Restrict endpoint access:

```csharp
[Authorize(Roles = "Admin")]
[HttpDelete("{id}")]
public IActionResult DeleteProduct(int id)
{
    return Ok();
}
```

Results:

```text
Admin -> Allowed

User -> Forbidden
```

---

# Authorization Status Codes

Authentication failed:

```text
401 Unauthorized
```

Meaning:

```text
You are not logged in
```

Authorization failed:

```text
403 Forbidden
```

Meaning:

```text
You are logged in
but do not have permission
```

---

# OAuth2

JWT answers:

```text
How do we prove identity?
```

OAuth2 answers:

```text
Who manages identity?
```

OAuth2 allows users to authenticate using external providers:

* Google
* Microsoft
* Facebook
* GitHub
* Okta
* Auth0

Instead of storing passwords ourselves.

---

# OAuth2 Authentication Flow

```text
Client
  |
  | Login with Google
  v
Google
  |
  | Authenticate User
  |
  | Issue Token
  v
Client
  |
  | Send Token
  v
API
```

---

# Login with Google Example

User clicks:

```text
Login with Google
```

Browser redirects:

```text
Application
      |
      v
Google Login Page
```

User authenticates.

Google returns:

```text
ID Token
Access Token
```

---

# OpenID Connect

OAuth2 primarily handles authorization.

OpenID Connect (OIDC) adds identity information.

Modern applications typically use:

```text
OAuth2
    +
OpenID Connect
```

Together.

---

# External Authentication Flow

```text
User
   |
   | Login with Google
   v
Google
   |
   | Verify Identity
   |
   | Generate Token
   v
Client
   |
   | Send Token
   v
API
   |
   | Validate Token
   v
Controller
```

---

# How OAuth2 Validation Works

Most systems validate tokens locally.

```text
Google
   |
Publishes Public Keys
   |
   v
Your API Downloads Keys
   |
   v
Local Validation
```

No network call is required for every request.

---

# OAuth2 + RBAC

OAuth2 identifies the user.

RBAC determines permissions.

Typical flow:

```text
Google Login
       |
       v
User Identity
       |
       v
Lookup Local User
       |
       v
Determine Role
       |
       v
Generate Local JWT
       |
       v
Role Based Authorization
```

---

# OAuth2 + RBAC Example

Local database:

```text
GoogleId       Email               Role
--------------------------------------------
123456         john@gmail.com      Admin
987654         jane@gmail.com      User
```

User authenticates using Google.

Server finds matching record.

Server generates local JWT:

```json
{
  "email": "john@gmail.com",
  "role": "Admin"
}
```

ASP.NET Core can now enforce role-based permissions.

---

# Blazor Authentication Flow

```text
Blazor Client
      |
      | Login
      v
ASP.NET Core API
      |
      | Generate JWT
      v
Blazor Stores Token
(LocalStorage)
      |
      v
HttpClient
      |
Authorization: Bearer <JWT>
      |
      v
Protected API
```

---

# Blazor HttpClient Example

```csharp
var token =
    await JS.InvokeAsync<string>(
        "localStorage.getItem",
        "jwt");

Http.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue(
        "Bearer",
        token);

var products =
    await Http.GetFromJsonAsync<List<ProductDto>>(
        "/api/products");
```

---

# Summary

JWT:

```text
Authentication
Stateless
Digitally Signed
```

RBAC:

```text
Authorization
Roles
Permissions
```

OAuth2 / OpenID Connect:

```text
External Identity Provider
Google Login
Microsoft Login
Single Sign-On (SSO)
```

Complete Flow:

```text
User Login
      |
      v
JWT Authentication
      |
      v
RBAC Authorization
      |
      v
Protected Controller
      |
      v
Response Returned
```
