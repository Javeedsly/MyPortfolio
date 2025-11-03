# MyPortfolio - ASP.NET Core Web API

This is the backend component for the "My Portfolio" application, built as an ASP.NET Core 8 Web API. It is designed to manage and serve portfolio project data, blog posts, handle user authentication, and manage file uploads.
The project is built using a clean **N-Layer Architecture** and is intended to be consumed by a frontend application, such as a Next.js app (for which CORS is pre-configured).

-----

## 🚀 Key Features

  * **N-Layer Architecture:** A clean separation of concerns into four distinct projects: `Core`, `Data`, `Business`, and `API` (MyPortfolio).
  * **Authentication & Authorization:** Uses **ASP.NET Core Identity** for user management and **JWT Bearer tokens** for securing endpoints.
  * **Email Confirmation:** Includes an email confirmation flow for new user registrations.
  * **Email Service:** Features a swappable email service. By default, it uses `FileEmailService` which saves registration emails to the `MyPortfolio/emails` directory for easy development and testing.
  * **Repository & Unit of Work:** Implements the Generic Repository and Unit of Work patterns for robust and maintainable data access.
  * **Database Seeding:** Automatically seeds the database on application startup with user roles (`SuperAdmin`, `Admin`, `User`) and a default `superadmin` account.
  * **Database:** Uses **Entity Framework Core** with **SQL Server**.
  * **File Uploads:** Handles image file uploads for projects and blogs, saving them to the server's `wwwroot/uploads` directory and storing a URL reference in the database.
  * **DTO & Mapping:** Uses Data Transfer Objects (DTOs) and **AutoMapper** for clean data transfer between layers.
  * **API Documentation:** Integrated **Swagger/OpenAPI** for easy API testing and documentation.
  * **Containerization:** Includes a `Dockerfile` for easy containerization and deployment.
  * **CORS:** Configured with a policy named `"AllowNextApp"` to allow requests from `http://localhost:3000`.

-----

## 💻 Technology Stack

  * .NET 8.0
  * ASP.NET Core 8.0
  * Entity Framework Core 8.0
  * SQL Server
  * ASP.NET Core Identity (with EF Core Stores)
  * JWT Bearer Authentication
  * AutoMapper (DependencyInjection)
  * Swashbuckle (Swagger)
  * Docker

-----

## 📂 Project Structure

The solution follows an N-Layer architecture, separating responsibilities into the following projects:

| Project | Role | Description |
| :--- | :--- | :--- |
| **`MyPortfolio.Core`** | Domain / Shared Kernel | Contains the core domain entities (`Project`, `Blog`, `AppUser`), DTOs, and all shared interfaces (`IRepository`, `IUnitOfWork`, `IProjectService`, `IBlogService`, `ITokenService`, `IEmailService`). |
| **`MyPortfolio.Data`** | Data Access Layer (DAL) | Implements the interfaces from `Core`. Contains the `AppDbContext`, repository implementations (`Repository.cs`, `ProjectRepository.cs`, `BlogRepository.cs`, `UnitOfWork.cs`), database migrations, and the `IdentityDataSeeder`. |
| **`MyPortfolio.Business`** | Business Logic Layer (BLL) | Contains the business logic and service implementations (`ProjectService.cs`, `BlogService.cs`, `TokenService.cs`, `FileEmailService.cs`) and AutoMapper profiles (`MappingProfile.cs`). |
| **`MyPortfolio`** | Presentation Layer (API) | The main executable project. Contains the API Controllers (`ProjectsController.cs`, `BlogsController.cs`, `AuthController.cs`), `Program.cs` for service configuration, and `appsettings.json`. |

-----

## 🚀 Getting Started

### 1\. Prerequisites

  * [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
  * A running instance of **SQL Server** (the default configuration targets `db31307.public.databaseasp.net`).
  * .NET EF Core tools (`dotnet tool install --global dotnet-ef`)

### 2\. Configuration

1.  **Clone the repository:**

    ```bash
    git clone <your-repo-url>
    cd MyPortfolio
    ```

2.  **Configure `appsettings.json`:**
    Open `MyPortfolio/appsettings.json` and review (or change if needed) the following sections:

      * **Database Connection String:**
        *By default, the project is configured to connect to a public ASP.NET database. Replace this with your own local SQL Server instance if needed.*

        ```json
        "ConnectionStrings": {
          "DefaultConnection": "Server=db31307.public.databaseasp.net; Database=db31307; User Id=db31307; Password=*******; Encrypt=True; TrustServerCertificate=True; MultipleActiveResultSets=True; "
        },
        ```

      * **JWT Settings:**
        It is recommended to update the `Jwt:Key` with a strong, secret key.

        ```json
        "Jwt": {
          "Issuer": "http://localhost:5247",
          "Audience": "http://localhost:5247",
          "Key": "********"
        }
        ```

      * **Email Settings (for SMTP):**
        *Note: The `FileEmailService` is active by default. If you switch to `SmtpEmailService`, you must fill in these settings and change the registration in `Program.cs`.*

        ```json
        "EmailSettings": {
          "SmtpHost": "smtp.gmail.com",
          "SmtpPort": 587,
          "FromEmail": "cavidsly@gmail.com",
          "SmtpPass": "*********"
        },
        ```

### 3\. Database Migration

Run the EF Core migrations from the solution's root directory to create the database and tables.

```bash
# Navigate to the API project folder
cd MyPortfolio

# Apply the migrations (Data project is referenced automatically)
dotnet ef database update
```

### 4\. Running the App & Default Login

1.  Run the application from the `MyPortfolio` directory:
    ```bash
    dotnet run
    ```
2.  Once the application is running (e.g., at `http://localhost:5247`), open the Swagger UI (`/swagger`).
3.  Log in with the default superadmin account created by `IdentityDataSeeder`:
      * **Username:** `superadmin`
      * **Password:** `YourSuperStrongP@ssword1!`
4.  Use the `POST /api/Auth/login` endpoint to log in.
5.  Copy the resulting JWT token from the response and paste it into the "Authorize" button in Swagger to access protected endpoints.

-----

## 🧭 Core API Endpoints

Below are the main controllers and their endpoints:

### `Auth` Controller

Handles user registration, login, and email confirmation.

  * `POST /api/Auth/register` - Creates a new user and sends a confirmation email.
  * `GET /api/Auth/confirm-email` - Processes the email confirmation link.
  * `POST /api/Auth/login` - Authenticates a user and returns a JWT token.

### `Projects` Controller

Full CRUD operations for portfolio projects.

  * `GET /api/Projects`
  * `GET /api/Projects/{id}`
  * `POST /api/Projects` - Creates a new project, including an image, via `[FromForm]`.
  * `PUT /api/Projects/{id}` - Updates an existing project via `[FromForm]`.
  * `DELETE /api/Projects/{id}`

### `Blogs` Controller

Full CRUD operations for blog posts.

  * `GET /api/Blogs`
  * `GET /api/Blogs/{id}`
  * `GET /api/Blogs/slug/{slug}` - Retrieves a post by its unique slug.
  * `POST /api/Blogs` - Creates a new post, including an image, via `[FromForm]`.
  * `PUT /api/Blogs/{id}` - Updates an existing post via `[FromForm]`.
  * `DELETE /api/Blogs/{id}`
