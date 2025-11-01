# MyPortfolio - ASP.NET Core Web API

This is the backend component of the "My Portfolio" application, implemented as an ASP.NET Core Web API project. The project is built upon a robust **N-Layer Architecture (Core, Data, Business, API)** and utilizes the **Generic Repository** design pattern to manage personal portfolio data, specifically the "Project" entries.

This API is designed to be consumed by external frontend applications, such as your Next.js project.

## ⚙️ Key Technologies and Architecture

* **Platform:** ASP.NET Core 8.0.
* **Language:** C#.
* **Architecture:** N-Layer (Multi-Layer) Architecture (Core, Data, Business, API).
* **Data Access:** Entity Framework Core (EF Core).
* **Database:** SQL Server (configured with a connection string targeting `CAVID\SQLEXPRESS`).
* **Data Patterns:** **Generic Repository** and **Unit of Work** patterns are implemented.
* **Mapping:** **AutoMapper** is used for data transfer between Entity and DTO (Data Transfer Object) classes.
* **Documentation:** **Swagger/OpenAPI** is configured for automatic documentation and interactive testing.

## 📂 Project Structure (N-Layer)

| Project | Role | Description |
| :--- | :--- | :--- |
| **MyPortfolio.Core** | Domain/Shared | Contains the fundamental domain entities (`Project`), DTOs, and layer-agnostic interfaces (`IRepository`, `IUnitOfWork`, `IProjectService`). |
| **MyPortfolio.Data** | Data Access Layer (DAL) | Implements the repository pattern using EF Core. Contains the `AppDbContext`, `Repository`, and `UnitOfWork` classes. |
| **MyPortfolio.Business** | Business Logic Layer (BLL) | Contains the business logic (`ProjectService`) and AutoMapper profiles (`MappingProfile`). It coordinates between the Data layer and the API layer. |
| **MyPortfolio** | API/Presentation | The entry point of the application. It hosts the API controllers (`ProjectsController`), configures DI, CORS, and Swagger. |

## 🔗 Main API Endpoints (CRUD)

The primary controller for project management is `ProjectsController`. All endpoints are built on the base path `api/projects`.

| HTTP Method | Route | Description | DTO (Body/Response) |
| :--- | :--- | :--- | :--- |
| `GET` | `/api/projects` | Retrieves all portfolio projects. | `List<ProjectDto>` |
| `GET` | `/api/projects/{id}` | Retrieves a single project by its ID. | `ProjectDto` |
| `POST` | `/api/projects` | Creates a new project. | `CreateProjectDto` |
| `PUT` | `/api/projects/{id}` | Updates an existing project by ID. | `UpdateProjectDto` |
| `DELETE` | `/api/projects/{id}` | Deletes a project by ID. | |

## 🛠️ Configuration and Setup

### 1. Database Setup

The project is configured to work with **SQL Server**.

* **Connection String:** The connection details are defined in the `MyPortfolio/appsettings.json` file:
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=CAVID\\SQLEXPRESS;Database=MyPortfolioDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
    }
    ```
    ***Note:*** Ensure that your SQL Server instance (`CAVID\SQLEXPRESS`) is running and accessible.

* **Migrations:** You must run Entity Framework Core Migrations to create the database and tables.

    ```bash
    # (In the solution directory)
    dotnet ef migrations add InitialCreate --project MyPortfolio.Data
    dotnet ef database update --project MyPortfolio.Data
    ```

### 2. Running the API

1.  Set the `MyPortfolio` project as the **Startup Project**.
2.  Run the application (e.g., `dotnet run` or F5 in Visual Studio).
3.  The API will typically start on `http://localhost:5247` (or another port).
4.  Navigate to the `/swagger` path to access the interactive documentation and testing interface (e.g., `http://localhost:5247/swagger`).

### 3. CORS Policy

The API is configured to allow requests from your Next.js frontend application:

* **Allowed Origin:** `http://localhost:3000`.
* **Policy Name:** `"AllowNextApp"`.
