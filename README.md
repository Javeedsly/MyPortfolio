# MyPortfolio - ASP.NET Core Web API

This is the backend component for the "My Portfolio" application, built as an ASP.NET Core 8 Web API. It is designed to manage and serve portfolio project data, blog posts, handle user authentication, and manage file uploads.
The project is built using a clean **N-Layer Architecture** and is intended to be consumed by a frontend application, such as a Next.js app (for which CORS is pre-configured).

-----

## Key Features

* **N-Layer Architecture:** A clean separation of concerns into four distinct projects: `Core`, `Data`, `Business`, and `API` (MyPortfolio).
* **Authentication & Authorization:** Uses **ASP.NET Core Identity** for user management and **JWT Bearer tokens** for securing endpoints.
* **Repository & Unit of Work Patterns:** Implements the Generic Repository and Unit of Work patterns for robust and maintainable data access.
* **Database:** Uses **Entity Framework Core** with **SQL Server**.
* **File Uploads:** Handles image file uploads for projects and blogs, saving them to the server's `wwwroot/uploads` directory and storing a URL reference in the database.
* **DTO & Mapping:** Uses Data Transfer Objects (DTOs) and **AutoMapper** for clean data transfer between layers.
* **API Documentation:** Integrated **Swagger/OpenAPI** for easy API testing and documentation.
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

-----

## 📂 Project Structure

The solution follows an N-Layer architecture, separating responsibilities into the following projects:

| Project | Role | Description |
| :--- | :--- | :--- |
| **`MyPortfolio.Core`** | Domain / Shared Kernel | Contains the core domain entities (`Project`, `Blog`, `AppUser`), DTOs, and all shared interfaces (`IRepository`, `IUnitOfWork`, `IProjectService`, `IBlogService`, `ITokenService`). |
| **`MyPortfolio.Data`** | Data Access Layer (DAL) | Implements the interfaces from `Core`. Contains the `AppDbContext`, repository implementations (`Repository.cs`, `ProjectRepository.cs`, `BlogRepository.cs`, `UnitOfWork.cs`), and database migrations. |
| **`MyPortfolio.Business`** | Business Logic Layer (BLL) | Contains the business logic and service implementations (`ProjectService.cs`, `BlogService.cs`, `TokenService.cs`) and AutoMapper profiles (`MappingProfile.cs`). |
| **`MyPortfolio`** | Presentation Layer (API) | The main executable project. Contains the API Controllers (`ProjectsController.cs`, `BlogsController.cs`, `AuthController.cs`), `Program.cs` for service configuration, and `appsettings.json`. |

-----

## 🚀 Getting Started

### 1. Prerequisites

* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* A running instance of **SQL Server** (the default configuration targets `CAVID\SQLEXPRESS`).
* .NET EF Core tools (`dotnet tool install --global dotnet-ef`)

### 2. Configuration

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
       

### 3. Database Migration

Run the EF Core migrations from the solution's root directory to create the database and tables.
```bash
# Navigate to the API project folder
cd MyPortfolio

# Apply the migrations (Data project is referenced automatically)
dotnet ef database update
(Alternatively, from the root solution folder):Bashdotnet ef database update --project MyPortfolio
4. Running the ApplicationNavigate to the API project directory:Bashcd MyPortfolio
Run the project:Bashdotnet run
The API will start (typically on http://localhost:5247 as per launchSettings.json).Access the Swagger UI in your browser to test the endpoints: http://localhost:5247/swagger.🔐 Authentication FlowRegister: Send a POST request to /api/auth/register with a RegisterDto (username, email, password).Login: Send a POST request to /api/auth/login with a LoginDto (username, password).Receive Token: The API will return an AuthResponseDto containing the JWT token, expiration, and username.Access Protected Routes: To access protected endpoints (like creating a project), include the token in the Authorization header:Authorization: Bearer <your_token_here>_API EndpointsAuthentication (/api/auth)MethodRouteDescriptionRequest BodyResponsePOST/api/auth/registerRegisters a new user.RegisterDto200 OKPOST/api/auth/loginLogs in an existing user.LoginDtoAuthResponseDtoProjects (/api/projects)MethodRouteProtectedDescriptionRequest BodyResponseGET/api/projectsNoRetrieves all projects.-List<ProjectDto>GET/api/projects/{id}NoRetrieves a single project by its ID.-ProjectDtoPOST/api/projectsYes (Auth commented out)Creates a new project. (Uses multipart/form-data).[FromForm] CreateProjectDtoProjectDto (201 Created)PUT/api/projects/{id}Yes (Auth commented out)Updates an existing project. (Uses multipart/form-data).[FromForm] UpdateProjectDto204 NoContentDELETE/api/projects/{id}Yes (Auth commented out)Deletes a project by ID.-204 NoContentBlogs (/api/blogs)MethodRouteProtectedDescriptionRequest BodyResponseGET/api/blogsNoRetrieves all blogs.-List<BlogDto>GET/api/blogs/{id}NoRetrieves a single blog by its ID.-BlogDtoGET/api/blogs/slug/{slug}NoRetrieves a single blog by its Slug.-BlogDtoPOST/api/blogsYes (Auth commented out)Creates a new blog. (Uses multipart/form-data).[FromForm] CreateBlogDtoBlogDto (201 Created)PUT/api/blogs/{id}Yes (Auth commented out)Updates an existing blog. (Uses multipart/form-data).[FromForm] UpdateBlogDto204 NoContentDELETE/api/blogs/{id}Yes (Auth commented out)Deletes a blog by ID.-
