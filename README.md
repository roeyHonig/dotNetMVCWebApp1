# MyModernWebApp

A modern ASP.NET Core MVC application built with **.NET 7** using **GitHub Codespaces**.

This setup allows you to build and run the project entirely in the cloud without installing .NET locally.

---

## 🚀 Prerequisites

* A GitHub account
* Access to GitHub Codespaces

No local .NET installation is required.

---

# Step 1: Create the GitHub Repository

1. Go to GitHub and create a new repository.
2. Name it something like:

```text
MyModernWebApp
```

3. Do **NOT** initialize the repository with:

   * README
   * `.gitignore`
   * License

4. Click **Create repository**.

---

# Step 2: Open a Codespace

1. Open your repository on GitHub.
2. Click:

```text
Code → Codespaces → New Codespace
```

3. GitHub will launch a browser-based VS Code environment.

You now have a full Linux development container running in the cloud.

---

# Step 3: Create the MVC Project

Open the terminal inside Codespaces and run:

```bash
# Create project folder
mkdir MyModernWebApp
cd MyModernWebApp

# Create a new ASP.NET Core MVC app targeting .NET 7
dotnet new mvc -o .
```

The `-o .` option creates the project directly in the current folder.

---

# Step 4: Configure Codespaces with a Dev Container

Create the `.devcontainer` folder:

```bash
mkdir -p .devcontainer
```

Create the file:

```text
.devcontainer/devcontainer.json
```

Add the following configuration:

```json
{
  "name": "dotnet-7 MVC App",
  "image": "mcr.microsoft.com/dotnet/sdk:7.0",
  "remoteUser": "root",
  "features": {
    "ghcr.io/devcontainers/features/dotnet:1": {}
  },
  "forwardPorts": [5000, 5001],
  "postCreateCommand": "dotnet restore",
  "extensions": [
    "ms-dotnettools.csharp",
    "ms-dotnettools.vscode-dotnet-runtime"
  ]
}
```

This configuration provides:

* .NET 7 SDK
* C# VS Code extensions
* Automatic package restore
* Port forwarding for local preview

---

# Step 5: Commit and Push the Project

Run the following commands in the Codespaces terminal:

```bash
git init
git add .
git commit -m "Initial .NET 7 MVC project with Codespaces support"
git branch -M main
git remote add origin https://github.com/<your-username>/MyModernWebApp.git
git push -u origin main
```

Replace:

```text
<your-username>
```

with your GitHub username.

---

# Step 6: Run the Application

Start the web application:

```bash
dotnet run
```

The app typically runs on:

```text
http://localhost:5000
```

Codespaces will automatically detect the port and allow you to open the application in your browser.

---

# ✅ Benefits of This Setup

* No local .NET installation required
* Fully cloud-hosted development environment
* Works from any machine or browser
* Consistent development environment across devices
* Easy collaboration through GitHub

---

# 💡 Notes

With GitHub Codespaces, your local machine acts only as a client interface.
The actual build tools, SDKs, runtime, and development environment run entirely in the cloud.

This makes it easy to develop .NET applications even on systems where installing the SDK locally is difficult or unsupported.

---
