# 🚗 Premium Car Rental System 

A state-of-the-art, robust, and visually stunning Car Rental System built with **ASP.NET Core 8.0 MVC** and **MySQL**. This application features a premium "Glassmorphism" design and is specifically engineered for high performance and maximum compatibility with legacy database environments (MySQL 5.5+).

---

## ✨ Key Features & Improvements

### 🎨 Elite Visual Experience
- **Premium Glassmorphism Design**: Semi-transparent, blurred UI elements with high-end aesthetics.
- **Full Responsiveness**: Optimized for mobile, tablet, and desktop viewing across all pages.
- **Dynamic Micro-Animations**: Smooth transitions and hover effects using custom CSS (`animate-up`).
- **Modern Iconography**: Fully integrated with Bootstrap Icons for a professional feel.

### 🛡️ Robust Technical Foundation
- **Legacy MySQL Support**: Custom-tuned for MySQL 5.5+, resolving common batching and ID retrieval (`LAST_INSERT_ID`) issues.
- **Safe ID Retrieval**: Configured with `AllowMultiQueries=true` and `AllowUserVariables=true` for reliable operations.
- **Clean Build (Zero Warnings)**: 100% resolution of all nullability warnings and EF Core tracker issues.
- **Manual Data Scaffolding**: Integrated `SeedData` logic for automatic, error-free database initialization.

### 🚘 Comprehensive Car Management
- **Fleet Inventory Control**: Admins can easily Create, Edit (Rename), View Details, and Delete vehicles.
- **Live Search & Status**: Real-time visibility into car availability and rental rates.
- **Interactive UI**: Image-rich vehicle cards with intuitive action buttons.

### � Advanced Booking & Payments
- **Seamless Rental Flow**: Smart date selection with automated price calculations.
- **My Bookings Dashboard**: Personalized user views to track active and past rentals.
- **Secure Mock Payments**: Integrated payment processing simulation for a complete end-to-end flow.

### 📊 Administrative Power
- **Intuitive Dashboard**: Real-time metrics for Total Revenue, Total Bookings, and Recent Activity.
- **User Management**: Role-based access control (Admin vs. Regular User).

---

## 🚀 Deployment Guide (Setup on Any System)

### 1. Prerequisites
- **.NET 8.0 SDK** or later.
- **MySQL Server** (Version 5.5 or later).
- **Web Browser** (Modern browser recommended).

### 2. Database Configuration
1. Open `CarRentalSystem/appsettings.json`.
2. Update the `DefaultConnection` string with your local MySQL credentials:
   ```json
   "DefaultConnection": "server=localhost;user=root;password=root;database=car_rental_db;CharSet=utf8"
   ```

### 3. Build & Run
1. Open terminal in the project root.
2. Run the application:
   ```powershell
   dotnet run --project CarRentalSystem
   ```
3. Access the portal at: `http://localhost:5202`

### 4. Automatic Initialization
The system will automatically create the database and seed it with an **Admin Account** and **7 Premium Cars** on the first launch.

---

## 🛠️ Troubleshooting

### ❌ "The process cannot access the file... because it is being used by another process"
If you see this error during `dotnet run` or `dotnet build`, it means the application is already running. 
- **Fix**: Kill the existing process via Task Manager or terminal:
  ```powershell
  taskkill /F /IM CarRentalSystem.exe
  ```

### ❌ "MySQL Syntax Error" (Persistent)
If you encounter syntax errors during database operations (like Bookings), ensure your `Program.cs` is configured with:
```csharp
options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
    mySqlOptions => mySqlOptions.MaxBatchSize(1))
```
The `MaxBatchSize(1)` is critical for compatibility with certain MySQL environments where multi-statement queries are restricted.

---

## 🔑 Default Credentials

| Account | Email | Password |
|------|-------|----------|
| **Admin** | `admin@carrental.com` | `admin` |
| **User** | (Register a new account via the UI) | |

---

## 📁 System Architecture

- **`/Controllers`**: Lean, efficient logic for handling web requests.
- **`/Models`**: Well-structured Entity Framework entities with null-safety.
- **`/Views`**: Premium cshtml templates implementing the design system.
- **`/Data`**: Centralized context and robust seeding engine.
- **`/wwwroot`**: Optimized assets including the core `site.css` design tokens.

---

## 🛡️ Stability Commitment
This version has been stress-tested for SQL syntax accuracy and runtime stability. All "Possibly null reference" warnings have been systematically resolved using defensive programming patterns.
