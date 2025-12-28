<div align="center">
  <h1>🧭 EinarTask Management</h1>
  <p>A simple yet powerful Task Management System built with ASP.NET Core MVC & Entity Framework Core</p>
  
  <div>
    <img src="https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet" alt=".NET 8.0">
    <img src="https://img.shields.io/badge/ASP.NET_Core-MVC-512BD4?logo=dotnet" alt="ASP.NET Core MVC">
    <img src="https://img.shields.io/badge/Entity_Framework-Core-6DB33F?logo=nuget" alt="Entity Framework Core">
    <img src="https://img.shields.io/badge/SQL_Server-2022-CC2927?logo=microsoft-sql-server" alt="SQL Server">
    <img src="https://img.shields.io/badge/Bootstrap-5-7952B3?logo=bootstrap" alt="Bootstrap 5">
    <img src="https://img.shields.io/badge/License-MIT-green" alt="MIT License">
  </div>
</div>

---

## ✨ Key Features

### 👤 User Management
<ul>
  <li>🔐 User registration and login via ASP.NET Identity</li>
  <li>👥 Profile creation and management</li>
  <li>🔄 Secure authentication and session handling</li>
</ul>

### 📋 Task Management
<ul>
  <li>📝 Create, edit, and delete personal tasks</li>
  <li>📅 Track task status (In Progress, Completed)</li>
  <li>🎯 User-specific task lists</li>
</ul>

### ⚙️ Technical Highlights
<ul>
  <li>📦 Code-First approach with Entity Framework Core</li>
  <li>🧱 Model–View–Controller (MVC) architectural pattern</li>
  <li>💾 MSSQL database integration via appsettings.json</li>
</ul>

---

## 🛠️ Technical Stack

<table>
  <tr>
    <th>Component</th>
    <th>Technology</th>
  </tr>
  <tr>
    <td>Backend Framework</td>
    <td>ASP.NET Core 8.0 MVC</td>
  </tr>
  <tr>
    <td>Database</td>
    <td>Microsoft SQL Server (EF Core Code-First)</td>
  </tr>
  <tr>
    <td>Frontend</td>
    <td>Bootstrap 5 + Razor Views</td>
  </tr>
  <tr>
    <td>Authentication</td>
    <td>ASP.NET Core Identity</td>
  </tr>
  <tr>
    <td>Architecture</td>
    <td>Model–View–Controller (MVC)</td>
  </tr>
</table>

---

## 🚀 Installation

```bash
# 1️⃣ Clone the repository
git clone https://github.com/kullaniciadi/EinarTask-Management.git
cd EinarTask-Management

# 2️⃣ Open appsettings.json and configure your database connection:
# "ConnectionStrings": {
#   "DefaultConnection": "Server=.;Database=EinarTaskDB;Trusted_Connection=True;TrustServerCertificate=True;"
# }

# 3️⃣ Apply migrations and create the database
update-database

# 4️⃣ Run the application
dotnet run
