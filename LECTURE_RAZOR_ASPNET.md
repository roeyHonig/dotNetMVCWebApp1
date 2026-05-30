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
