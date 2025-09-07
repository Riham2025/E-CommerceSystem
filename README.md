## E-CommerceSystem

# Overview

E-CommerceSystem is a backend for a small online store.
It covers product catalog, user accounts, orders, reviews, and (in this extension) categories and suppliers.
The API exposes secure endpoints for CRUD operations, order placement, reporting hooks, and future add-ons such as invoices and emails.

# Technologies Used

ASP.NET Core Web API – host HTTP endpoints.

Entity Framework Core – ORM for SQL Server.

SQL Server – relational database.

JWT (JSON Web Tokens) – authN/authZ.

Swagger / Swashbuckle – API docs & try-it-out.

Dependency Injection – service lifetime management.

AutoMapper – DTO ↔ Entity mapping (no manual mapping).

# App Settings

Central configuration lives in appsettings.json (+ environment overrides).

Typical keys

ConnectionStrings.DefaultConnection – server, db, credentials.

JwtSettings

SecretKey – HMAC signing key.

Issuer / Audience – token scope.

ExpiryInMinutes – token lifetime.

Change environment-specific values in appsettings.Development.json.

# Launch Settings

Properties/launchSettings.json controls how the app runs locally.

Profiles (Kestrel, IIS Express, Docker)

Application URLs (e.g. https://localhost:7147)

Environment variables (e.g. ASPNETCORE_ENVIRONMENT=Development)

Auto-launch browser & command-line args for debugging

Not used in production; for local development only.

# Models with Data Annotations & Validation

Data annotations enforce integrity and business rules:

[Key] primary keys

[Required], [MaxLength], [Range]

[EmailAddress], [RegularExpression]

[ForeignKey] relationships

[JsonIgnore] to break circular refs when needed

# Users

UID (PK)

UName, Email, Password (hashed), Phone, Role (Customer, Admin, Manager)

CreatedAt

# Products

PID (PK)

ProductName, Description, Price, Stock, OverallRating

CategoryId (FK → Categories) (new)

SupplierId (FK → Suppliers) (new)

# Reviews

ReviewID (PK)

UID (FK → Users)

PID (FK → Products)

Rating, Comment, ReviewDate

# Orders

OID (PK)

UID (FK → Users)

OrderDate, TotalAmount

# OrderProducts (Join Table)

OID (FK → Orders)

PID (FK → Products)

Quantity

# Categories 

CategoryId (PK)

Name, Description

Products (1 Category → Many Products)

# Suppliers 

SupplierId (PK)

Name, ContactEmail, Phone

Products (1 Supplier → Many Products)

# Relationships (from the ERD)

Users 1↔∞ Orders

Orders ∞↔∞ Products via OrderProducts

Users 1↔∞ Reviews

Products 1↔∞ Reviews

Categories 1↔∞ Products 

Suppliers 1↔∞ Products 

Orders ∞→1 Users

# Navigation Summary

Order → User, OrderProducts[]

OrderProduct → Order, Product

Product → Category, Supplier, Reviews[], OrderProducts[]

Review → User, Product

User → Orders[], Reviews[]

Category → Products[]

Supplier → Products[]

# DbContext

ApplicationDbContext : DbContext holds the session with SQL Server.

DbSet<T>: Users, Products, Reviews, Orders, OrderProducts, Categories, Suppliers

OnModelCreating configures:

Composite key for OrderProducts (OID,PID)

Required FKs

1→∞ for Category→Products and Supplier→Products

Optional unique indexes (e.g., Category.Name, Supplier.Name)

# Enable lazy loading (optional):

```  builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseLazyLoadingProxies()
       .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```
Install: Microsoft.EntityFrameworkCore.Proxies and make nav props virtual.

# Database Schema

The schema exactly matches the diagram you provided:

Core tables: Users, Products, Reviews, Orders, OrderProducts

New master data tables: Categories, Suppliers

FKs: Products.CategoryId, Products.SupplierId, Reviews.UID/PID, Orders.UID, OrderProducts.OID/PID

# Run:

``` dotnet ef migrations add Init_With_Category_Supplier
dotnet ef database update 
```

# Project Structure
```
E-CommerceSystem/
├─ Controllers/
│  ├─ ProductsController.cs
│  ├─ OrdersController.cs
│  ├─ ReviewsController.cs
│  ├─ UsersController.cs
│  ├─ CategoriesController.cs          # new
│  └─ SuppliersController.cs           # new
├─ Services/
│  ├─ Interfaces/
│  ├─ Implementations/
│  ├─ OrderSummaryService.cs           # aggregates Orders + Products + Users
│  └─ FileStorage/ImageService.cs      # product images (upload)
├─ Repositories/
│  ├─ Interfaces/
│  ├─ Ef/
│  ├─ CategoryRepository.cs            # new
│  └─ SupplierRepository.cs            # new
├─ DTO/
│  ├─ ProductDto.cs / CreateProductDto.cs / UpdateProductDto.cs
│  ├─ CategoryDto.cs (Create/Update/Read)   # new
│  ├─ SupplierDto.cs (Create/Update/Read)   # new
│  ├─ OrderDtos.cs (incl. summary)
│  └─ AuthDtos.cs
├─ Mapping/
│  └─ MappingProfile.cs                # AutoMapper profiles
├─ Persistence/
│  ├─ ApplicationDbContext.cs
│  └─ Migrations/
├─ Middleware/
│  ├─ ErrorHandlingMiddleware.cs       # centralized exception → ProblemDetails
│  └─ Serilog/                         # logging setup
└─ Program.cs

```

# Repositories

Each repository implements CRUD for its entity and hides EF details:

# Example : 
```
public class OrderProductsRepo
{
    private readonly ApplicationDbContext _context;
    public OrderProductsRepo(ApplicationDbContext context) => _context = context;
    // Add, Get, Update, Delete...
}

```

# New repos:

``` 
ICategoryRepo / CategoryRepo
```
```
ISupplierRepo / SupplierRepo
```

# Services with DTO (and AutoMapper)

Business logic lives in Services; controllers talk to Services using DTOs.
AutoMapper replaces manual mapping across the system.

# Register:
```
builder.Services.AddAutoMapper(typeof(Program));
```

# Inject:
```
public class ReviewService : IReviewService
{
    private readonly IMapper _mapper;
    public ReviewService(IMapper mapper) => _mapper = mapper;
}
```

# DTO Example
```
public class OrderProductsDto
{
    public int OID { get; set; }
    public int PID { get; set; }
    public int Quantity { get; set; }
}
```

# Category / Supplier DTOs (new)
```
public record CategoryCreateDto(string Name, string? Description);
public record CategoryReadDto(int CategoryId, string Name, string? Description);

public record SupplierCreateDto(string Name, string ContactEmail, string? Phone);
public record SupplierReadDto(int SupplierId, string Name, string ContactEmail, string? Phone);
```