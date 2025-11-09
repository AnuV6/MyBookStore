# MyBookStore - Book Store Billing System

## Overview
MyBookStore is a simple desktop billing system for a book store, built with C# .NET (WPF) and SQL Server. It features a login screen and a dashboard, and is designed for easy extensibility.

## Tech Stack
- C# .NET 8 (WPF)
- SQL Server (LocalDB or full instance)
- Entity Framework Core
- Visual Studio 2022 or later

## Features
- Secure login screen (demo uses plaintext password for simplicity)
- Dashboard window after login
- User authentication via SQL Server database

## Project Structure
- `Models/` - Entity classes (e.g., User)
- `Data/` - Database context (EF Core)
- `LoginWindow.xaml` - Login UI
- `DashboardWindow.xaml` - Dashboard UI
- `MainWindow.xaml` - App entry point

## Database Setup
1. Create a database named `MyBookDB` in SQL Server (or use LocalDB).
2. Run the following SQL to create the Users table and insert a test user:

```sql
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL
);

INSERT INTO Users (Username, PasswordHash) VALUES ('admin', 'admin123');
```

## Configuration
- Update the connection string in `Data/AppDbContext.cs` if needed:
  ```csharp
  optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MyBookDB;Integrated Security=True;");
  ```

## Required NuGet Packages
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.SqlServer

## How to Run
1. Open the solution in Visual Studio.
2. Restore NuGet packages.
3. Build the solution.
4. Ensure your SQL Server instance is running and the database is set up.
5. Run the application. Login with username `admin` and password `admin123`.

## Notes
- For demo purposes, passwords are stored in plaintext. For production, use hashed passwords (e.g., BCrypt).
- You can extend the system with more features like inventory, billing, and reporting.

---

Feel free to customize and extend this project for your needs!
