# MyPortfolio - ASP.NET Core Web API

This is the backend component for the "My Portfolio" application, built as an ASP.NET Core 8 Web API. It is designed to manage and serve portfolio project data, handle user authentication, and manage file uploads.

The project is built using a clean **N-Layer Architecture** and is intended to be consumed by a frontend application, such as a Next.js app (for which CORS is pre-configured).

-----

## Key Features

  * **N-Layer Architecture:** A clean separation of concerns into four distinct projects: `Core`, `Data`, `Business`, and `API` (MyPortfolio).
  * **Authentication & Authorization:** Uses **ASP.NET Core Identity** for user management and **JWT Bearer tokens** for securing endpoints.
  * **Repository & Unit of Work Patterns:** Implements the Generic Repository and Unit of Work patterns for robust and maintainable data access.
  * **Database:** Uses **Entity Framework Core** with **SQL Server**.
  * **File Uploads:** Handles image file uploads for projects, saving them to the server's `wwwroot/uploads` directory and storing a URL reference in the database.
  * **DTO & Mapping:** Uses Data Transfer Objects (DTOs) and **AutoMapper** for clean data transfer between layers.
  * **API Documentation:** Integrated **Swagger/OpenAPI** for easy API testing and documentation.
  * **CORS:** Configured with a policy named `"AllowNextApp"` to allow requests from `http://localhost:3000`.

## 💻 Technology Stack

  * **.NET 8.0**
  * **ASP.NET Core 8.0**
  * **Entity Framework Core 8.0**
  * **SQL Server**
  * **ASP.NET Core Identity** (with EF Core Stores)
  * **JWT Bearer Authentication**
  * **AutoMapper** (DependencyInjection)
  * **Swashbuckle (Swagger)**

-----

## 📂 Project Structure

The solution follows an N-Layer architecture, separating responsibilities into the following projects:

| Project | Role | Description |
| :--- | :--- | :--- |
| **`MyPortfolio.Core`** | Domain / Shared Kernel | Contains the core domain entities (`Project`, `AppUser`), DTOs, and all shared interfaces (`IRepository`, `IUnitOfWork`, `IProjectService`, `ITokenService`). |
| **`MyPortfolio.Data`** | Data Access Layer (DAL) | Implements the interfaces from `Core`. Contains the `AppDbContext`, repository implementations (`Repository.cs`, `ProjectRepository.cs`, `UnitOfWork.cs`), and database migrations. |
| **`MyPortfolio.Business`** | Business Logic Layer (BLL) | Contains the business logic and service implementations (`ProjectService.cs`, `TokenService.cs`) and AutoMapper profiles (`MappingProfile.cs`). |
| **`MyPortfolio`** | Presentation Layer (API) | The main executable project. Contains the API Controllers (`ProjectsController.cs`, `AuthController.cs`), `Program.cs` for service configuration, and `appsettings.json`. |

-----

## 🚀 Getting Started

### 1\. Prerequisites

  * [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
  * A running instance of **SQL Server** (the default configuration targets `CAVID\SQLEXPRESS`).
  * .NET EF Core tools (`dotnet tool install --global dotnet-ef`)

### 2\. Configuration

1.  **Clone the repository:**

    ```bash
    git clone <your-repo-url>
    cd MyPortfolio
    ```

2.  **Configure `appsettings.json`:**
    Open `MyPortfolio/appsettings.json` and update the following sections:

      * **Database Connection String:**

        ```json
        "ConnectionStrings": {
          "DefaultConnection": "Server=CAVID\\SQLEXPRESS;Database=MyPortfolioDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
        }
        ```

      * **JWT Settings:**
        Update the `Jwt:Key` with a strong, secret key. The `Issuer` and `Audience` should point to your API's URL.

        ```json
        "Jwt": {
          "Issuer": "http://localhost:5247",
          "Audience": "http://localhost:5247",
          "Key": "YOUR_SUPER_SECRET_STRONG_KEY_GOES_HERE"
        }
        ```

### 3\. Database Migration

Run the EF Core migrations from the solution's root directory to create the database and tables.

```bash
# Navigate to the API project folder
cd MyPortfolio

# Apply the migrations (Data project is referenced automatically)
dotnet ef database update
```

*(Alternatively, from the root solution folder)*:

```bash
dotnet ef database update --project MyPortfolio
```

### 4\. Running the Application

1.  Navigate to the API project directory:

    ```bash
    cd MyPortfolio
    ```

2.  Run the project:

    ```bash
    dotnet run
    ```

3.  The API will start (typically on `http://localhost:5247` as per `launchSettings.json`).

4.  Access the Swagger UI in your browser to test the endpoints: **`http://localhost:5247/swagger`**.

-----

## 🔐 Authentication Flow

1.  **Register:** Send a `POST` request to `/api/auth/register` with a `RegisterDto` (username, email, password).
2.  **Login:** Send a `POST` request to `/api/auth/login` with a `LoginDto` (username, password).
3.  **Receive Token:** The API will return an `AuthResponseDto` containing the JWT `token`, `expiration`, and `username`.
4.  **Access Protected Routes:** To access protected endpoints (like creating a project), include the token in the `Authorization` header:
    `Authorization: Bearer <your_token_here>`

## \_API Endpoints

### Authentication (`/api/auth`)

| Method | Route | Description | Request Body | Response |
| :--- | :--- | :--- | :--- | :--- |
| `POST` | `/api/auth/register` | Registers a new user. | `RegisterDto` | `200 OK` |
| `POST` | `/api/auth/login` | Logs in an existing user. | `LoginDto` | `AuthResponseDto` |

### Projects (`/api/projects`)

| Method | Route | Protected | Description | Request Body | Response |
| :--- | :--- | :--- | :--- | :--- | :--- |
| `GET` | `/api/projects` | No | Retrieves all projects. | - | `List<ProjectDto>` |
| `GET` | `/api/projects/{id}` | No | Retrieves a single project by its ID. | - | `ProjectDto` |
| `POST` | `/api/projects` | **Yes** | Creates a new project. (Uses `multipart/form-data`). | `[FromForm] CreateProjectDto` | `ProjectDto` |
| `PUT` | `/api/projects/{id}` | **Yes** | Updates an existing project. (Uses `multipart/form-data`). | `[FromForm] UpdateProjectDto` | `204 NoContent` |
| `DELETE` | `/api/projects/{id}` | **Yes** | Deletes a project by ID. | - | `204 NoContent` |
