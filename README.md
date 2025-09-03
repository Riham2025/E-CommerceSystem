# E-CommerceSystem (Order Management System – Part 2)


---

## Table of Contents
- [Architecture](#architecture)
- [Database Schema](#database-schema)
- [Entities & Relationships](#entities--relationships)
- [Project Structure](#project-structure)
- [DTOs & Mapping](#dtos--mapping)
- [Services & Business Rules](#services--business-rules)
- [Security](#security)
- [API Surface (high level)](#api-surface-high-level)
- [Run Locally](#run-locally)

---

## Architecture

Layered architecture with clear separation of concerns:

- **Models (Entities)** → EF Core entities that map to tables.
- **Repositories** → Data access (CRUD, queries).
- **Services** → Business logic, validation, transactions, cross-aggregate rules.
- **DTOs** → Shape request/response payloads; avoid exposing EF entities directly.
- **Controllers** → HTTP endpoints calling services.

Cross-cutting:
- **AutoMapper** for DTO ↔ Entity mapping.
- **FluentValidation / DataAnnotations** for input validation.
- **Middleware** for error handling & logging.

---

## Database Schema



    USERS {
      int UID PK
      string UName
      string Email
      string Password
      string Phone
      string Role
      datetime CreatedAt
    }

    PRODUCTS {
      int PID PK
      string ProductName
      string Description
      decimal Price
      int Stock
      decimal OverallRating
    }

    REVIEWS {
      int ReviewID PK
      int UID FK
      int PID FK
      int Rating
      string Comment
      datetime ReviewDate
    }

    ORDERS {
      int OID PK
      datetime OrderDate
      decimal TotalAmount
      int UID FK
      string Status
    }

    ORDERPRODUCTS {
      int OID FK
      int PID FK
      int Quantity
    }


    CATEGORY {
      int CategoryId PK
      string Name
      string Description
    }

    SUPPLIER {
      int SupplierId PK
      string Name
      string ContactEmail
      string Phone
    }
The new Category and Supplier are one-to-many parents for Products (a product belongs to one category and one supplier). 

Entities & Relationships

Users ⟶ Orders (1-to-many)

Orders ⟷ Products (many-to-many via OrderProducts)

Products ⟶ Reviews (1-to-many)

Users ⟶ Reviews (1-to-many)

Category ⟶ Products (1-to-many) (new)

Supplier ⟶ Products (1-to-many) (new)

Key business notes:

Orders.Status: Pending | Paid | Shipped | Delivered | Cancelled. (new) 

On cancel, stock is restored. (new) 

Reviews allowed only for purchased products; one review per product per user. (new) 

Project Structure



E-CommerceSystem/

├─ Controllers/
│  ├─ ProductsController.cs
│  ├─ OrdersController.cs
│  ├─ ReviewsController.cs
│  ├─ UsersController.cs
│  ├─ CategoriesController.cs        
│  └─ SuppliersController.cs  
├─ Services/
│  ├─ Interfaces/...
│  ├─ Implementations/...
│  ├─ OrderSummaryService.cs         #  (aggregates Orders + Products + Users)
│  └─ FileStorage/ImageService.cs    #  (product images)
├─ Repositories/
│  ├─ Interfaces/...
│  ├─ Ef/...
│  ├─ CategoryRepository.cs          
│  └─ SupplierRepository.cs          
├─ DTO/
│  ├─ ProductDto.cs / CreateProductDto.cs / UpdateProductDto.cs
│  ├─ CategoryDto.cs (Create/Update) 
│  ├─ SupplierDto.cs (Create/Update) 
│  ├─ OrderDtos.cs (incl. summary)   
│  └─ AuthDtos.cs
├─ Mapping/
│  └─ MappingProfile.cs              # AutoMapper profiles for all DTOs ↔ Entities
├─ Persistence/
│  ├─ ApplicationDbContext.cs        # DbSets + Fluent config
│  └─ Migrations/                    # EF Core migrations & seed
├─ Middleware/
│  ├─ ErrorHandlingMiddleware.cs     # centralized exception to ProblemDetails
│  └─ Logging (Serilog) setup
└─ Program.cs                         # DI, JWT, Swagger, CORS, Serilog
DTOs & Mapping
Why DTOs?

Keep controllers thin and safe: never expose EF entities directly.

Shape responses (pagination, filtering, HATEOAS if needed).

Validate inputs at the boundary.





public record ProductDto(int PID, string ProductName, string Description,
                         decimal Price, int Stock, decimal OverallRating,
                         int CategoryId, int SupplierId);

public record CreateProductDto(string ProductName, string Description,
                               decimal Price, int Stock,
                               int CategoryId, int SupplierId);

public record UpdateProductDto(string? ProductName, string? Description,
                               decimal? Price, int? Stock,
                               int? CategoryId, int? SupplierId);
AutoMapper configuration:



CreateMap<Product, ProductDto>();
CreateMap<CreateProductDto, Product>();
CreateMap<UpdateProductDto, Product>()
    .ForAllMembers(opt => opt.Condition((src, _, val) => val != null));

Services & Business Rules
Enhancements (Part 2): 

Products

Get-all with pagination & filtering (name, price range).

Image upload (store path or blob; validate type/size).

Orders

Status tracking: Pending → Paid → Shipped → Delivered; with Cancelled branch.

Cancellation restores product stock.

Invoice generation (PDF/report).

Order summary service (aggregates Orders+Products+Users).

Email notifications on placed / canceled orders.

Reviews

Only purchasers can review; max one review per product+user.

Concurrency

Use rowversion/timestamp on Product/Order to prevent lost updates (409 on mismatch).

Security
JWT authentication with refresh tokens; tokens stored in cookies. 

Roles: Admin, Customer, Manager (policy-based authorization). 

Password hashing using BCrypt or ASP.NET Core Identity’s PBKDF2. 

API Surface (high level)
POST /api/auth/register, POST /api/auth/login, POST /api/auth/refresh

GET /api/products?name=&minPrice=&maxPrice=&page=&pageSize=

POST /api/products/{id}/image (multipart/form-data)

GET /api/orders/{id} , POST /api/orders , POST /api/orders/{id}/cancel

GET /api/orders/{id}/invoice (PDF)

POST /api/reviews (enforces purchase + single review rule)

CRUD /api/categories (new) , CRUD /api/suppliers (new)

GET /api/admin/reports/best-sellers | revenue | top-rated | active-customers (new)

Run Locally
Configure connection string in appsettings.json.

Apply migrations & seed:



dotnet ef database update



