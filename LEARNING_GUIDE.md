# Learning .NET and C#: A Beginner's Guide

Welcome to the world of .NET and C#! This guide will help you understand the project structure and give you a roadmap for learning.

## 1. Project Overview

The project we created is an **ASP.NET Core Web API**. It's designed to handle HTTP requests and return data (usually in JSON format).

### Key Files:
- **`Program.cs`**: The entry point of your application. This is where services are configured and the middleware pipeline is defined.
- **`Controllers/`**: Contains classes that handle incoming HTTP requests.
- **`appsettings.json`**: Configuration file for things like connection strings and application settings.
- **`PaymentApi.csproj`**: The project file that lists dependencies and compiler settings.

## 2. Where to Start?

### C# Language Basics
Start by learning the syntax of C#. It's very similar to Java or C++.
- **Data Types**: `int`, `string`, `bool`, `double`, `DateTime`.
- **Control Flow**: `if`, `else`, `switch`, `for`, `foreach`, `while`.
- **Classes and Objects**: C# is object-oriented. Learn how to create classes, properties, and methods.
- **LINQ (Language Integrated Query)**: One of C#'s best features. It allows you to query collections (and databases) using a SQL-like syntax.

### .NET Framework Concepts
- **Dependency Injection (DI)**: Understanding how .NET manages object lifetimes and dependencies.
- **Middleware**: How requests flow through the application.
- **Entity Framework Core (EF Core)**: The "Object-Relational Mapper" (ORM) used to interact with databases.

## 3. Recommended Resources

- [Official .NET Tutorials](https://dotnet.microsoft.com/learn)
- [C# for Beginners (Video Series)](https://www.youtube.com/playlist?list=PLdo4fOcmZ0oVxKLQCHpbO2J99u9G5hA_V)
- [Microsoft Learn - C# Paths](https://learn.microsoft.com/en-us/training/paths/csharp-first-steps/)

## 4. Running the Project

1. **Start the Database**:
   - Ensure Docker Desktop is running.
   - Run:
     ```bash
     docker-compose up -d
     ```
2. **Apply Migrations**:
   The project uses Entity Framework Core. To create the database tables:
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```
3. **Run the API**:
   ```bash
   dotnet run
   ```
4. **Explore Swagger**:
   Open `http://localhost:5000/swagger` (check terminal for actual port) to see the interactive API documentation.

## 5. Working with the Database

- **Models**: Look at `Models/Payment.cs` to see how data is structured.
- **DbContext**: `Data/ApplicationDbContext.cs` is the bridge between your code and the database.
- **Controllers**: `Controllers/PaymentsController.cs` contains the logic for handling requests.

## 6. Authentication with JWT

We've added JWT (JSON Web Token) authentication to protect your API.
- See [JWT_IMPLEMENTATION_GUIDE.md](file:///Users/betopia/Desktop/Arafat/net/pay/JWT_IMPLEMENTATION_GUIDE.md) for a deep dive.
- Use the `AuthController` to login and get a token.
- Remember to use the "Authorize" button in Swagger!

- Implementation of a real user registration system.

## 8. Working with Files and Security

### File Uploads
We added a `Utilities/FileHelper.cs` to handle file uploads.
- **`IFormFile`**: Used in controller actions to receive uploaded files.
- **`wwwroot`**: The folder where static files (like images) are stored and served from.
- **`app.UseStaticFiles()`**: Middleware that enables serving files from `wwwroot`.

### Password Hashing
- **BCrypt.Net**: We use this library to hash passwords. **Never store plain-text passwords!**
- **`BCrypt.HashPassword`**: Creates a secure hash.
- **`BCrypt.Verify`**: Checks a plain-text password against a hash.

## 9. Testing Signup
1. Use **Postman** or **Swagger**.
2. For Signup, use `multipart/form-data`.
3. Provide `Username`, `Password`, and a `ProfilePicture` file.
4. Access the image at: `http://localhost:5xxx/uploads/profiles/your-file-name.jpg`
