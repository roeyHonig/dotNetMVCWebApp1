# Building Web Apps with ASP.NET Core & Razor Views

## Overview
A **server-rendered web application** where the server (ASP.NET Core) generates HTML on-demand and sends it to the browser. The browser displays the HTML; server retains business logic, database access, and secrets.

---

## Request Flow

```
Browser                                   ASP.NET Server
   │                                            │
   ├──── GET /home ────────────────────────────►│
   │                                            │
   │                       ┌───────────────────┐│
   │                       │ 1. Route to       ││
   │                       │    Controller     ││
   │                       │ 2. Query DB       ││
   │                       │ 3. Render Razor   ││
   │                       │    view + data    ││
   │                       └───────────────────┘│
   │◄────── HTML (complete page) ───────────────┤
   │                                            │
   ├──── Display page in browser ─────────────►│
```

---

## Architecture: Controllers → Models → Views

| Layer | Purpose | Example |
|-------|---------|---------|
| **Controller** (C#) | Handle HTTP requests, query DB, prepare data | `HomeController.Index()` returns `View(model)` |
| **Model** (C#) | Data classes, business logic | `HomeViewModel { Title, Items }` |
| **View** (Razor .cshtml) | HTML + embedded C# to render UI | `@Model.Title`, `@foreach` loops |

---

## Razor Syntax Essentials

### 1. Model Declaration
```razor
@model HomeViewModel
```

### 2. Displaying Data
```razor
<h1>@Model.Title</h1>
<p>@Model.Description</p>
```

### 3. Control Flow
```razor
@if (Model.IsAdmin)
{
    <button>Edit</button>
}

@foreach (var item in Model.Items)
{
    <li>@item.Name - $@item.Price</li>
}
```

### 4. Forms & Tag Helpers
```razor
<form asp-action="Submit" method="post">
    <input asp-for="Name" placeholder="Enter name" />
    <input asp-for="Email" type="email" />
    <button type="submit">Send</button>
</form>
```
Tag helpers automatically bind form inputs to model properties.

### 5. Shared Layout
```razor
<!-- _Layout.cshtml (wrapper for all views) -->
<html>
    <body>
        <nav>@Html.ActionLink("Home", "Index")</nav>
        @RenderBody()   <!-- Individual view renders here -->
    </body>
</html>
```

---

## Key Benefits of Server-Rendered Apps

### 1. **Security & Secrets**
- API keys, database credentials, auth tokens **never leave the server**.
- Client sees only safe HTML; sensitive logic stays on server.

### 2. **Database Access**
- Queries run on server; only safe data sent to client.
- Example: fetch user profile, filter by permissions, then render view.

### 3. **Authentication & Authorization**
- User roles, permissions checked server-side before rendering.
- Example: admin panel only renders if `User.IsInRole("Admin")`.

### 4. **SEO & Performance**
- Server sends complete, ready-to-display HTML (not blank JS bundle).
- Search engines see full content immediately.

### 5. **Simpler Frontend**
- No need for React/Vue/Angular.
- Plain HTML + light JS for forms and interactions.

### 6. **Server-Side Business Logic**
- Complex calculations, validations, workflows run on server.
- Client doesn't need to know internal logic.

---

## Trade-offs: Server-Rendered vs. Static

| Aspect | Server-Rendered (Razor) | Static (SPA/Firebase) |
|--------|-------------------------|----------------------|
| **Speed** | Slight latency per request (server renders) | Instant (pre-built HTML/JS) |
| **Scalability** | Requires server resources per request | Scales cheaply (CDN, no server) |
| **Security** | Secrets safe on server | Harder to hide logic/keys |
| **Interactivity** | Require page reload or AJAX for updates | Real-time, no reload |
| **Auth & DB** | Easy (server has access) | Complex (API needed) |
| **Deployment** | Server host (Cloud Run, VPS) | Static host (Firebase, S3) |

---

## Development Workflow

```bash
# 1. Run the app
dotnet run

# 2. Open browser → http://localhost:5000
# Server listens, renders views on-demand

# 3. Edit view file (e.g., Index.cshtml)
# Refresh browser → server re-renders instantly

# 4. For styling/static JS, edit files in wwwroot/
# Refresh → browser loads updated CSS/JS
```

---

## Deployment Options

### Option A: Keep Server-Rendered (Full ASP.NET)
1. Build & containerize app (`dotnet publish` → Docker image).
2. Deploy container to Cloud Run (GCP) or similar.
3. Traffic: Browser → Cloud Run service → renders Razor views.

### Option B: Static Frontend Only (Future Pivot)
1. Move UI to a static SPA (React/Vue/Blazor WASM) built to `wwwroot/`.
2. Deploy static files to Firebase Hosting.
3. If backend needed, keep a separate API (Cloud Functions/Cloud Run).

---

## Example: Strongly-Typed View with Model

### Controller (`HomeController.cs`)
```csharp
public IActionResult Index()
{
    var model = new HomeViewModel
    {
        Title = "Welcome",
        Items = new List<ItemViewModel>
        {
            new ItemViewModel { Name = "Item 1", Price = 29.99m },
            new ItemViewModel { Name = "Item 2", Price = 49.99m }
        }
    };
    return View(model);
}

[HttpPost]
public IActionResult Submit(HomeViewModel model)
{
    // Server-side validation
    if (string.IsNullOrEmpty(model.Name))
        ModelState.AddModelError("Name", "Name is required");
    
    if (!ModelState.IsValid)
        return View(model);
    
    // Save to DB, process, etc.
    return RedirectToAction("Success");
}
```

### Model (`HomeViewModel.cs`)
```csharp
public class HomeViewModel
{
    public string Title { get; set; }
    public string Name { get; set; }
    public List<ItemViewModel> Items { get; set; }
}

public class ItemViewModel
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}
```

### View (`Views/Home/Index.cshtml`)
```razor
@model HomeViewModel

<h1>@Model.Title</h1>

<form asp-action="Submit" method="post">
    <input asp-for="Name" required />
    <button type="submit">Submit</button>
</form>

<ul>
    @foreach (var item in Model.Items)
    {
        <li>@item.Name - $@item.Price</li>
    }
</ul>
```

---

## Summary
**Razor + ASP.NET** gives you a **full-stack C# development experience** where the server handles logic, security, and data, and the browser simply displays the rendered result. Ideal for business apps, admin panels, and sites requiring strong security and server-side processing.



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
