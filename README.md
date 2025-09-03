## **E-CommerceSystem**


---

#  Order Management System – Part 2 (E-Commerce Backend)

A production-ready **ASP.NET Core Web API** backend for an E-Commerce system.
This phase extends the base code with new **models, services, security**, stronger **business rules**, and **analytics**.

---

##  Goal of the Project

Deliver a **secure, scalable, and extensible** backend that supports:

* **Customers**: browse products, place/cancel orders, write reviews.
* **Admins/Managers**: manage catalog, categories, suppliers; track sales; run reports.
* **Developers**: clean layering (Controllers → Services → Repositories → EF Core), DTOs & AutoMapper, logs, tests.

---

##  Tech Stack

* **.NET / ASP.NET Core Web API**
* **Entity Framework Core** (Migrations, LINQ, SQL Server)
* **AutoMapper** (DTO ↔ Entity mapping)
* **JWT Auth** with **Refresh Tokens** (tokens stored in Cookies)
* **BCrypt** (password hashing)
* **Serilog / ILogger** (structured logging)
* **Mail (SMTP)** for notifications
* (Optional) **PDF/Invoice**: QuestPDF or iText (implementation choice)

---

##  Database Schema



### Tables & Key Fields

**Users**

* `UID (PK)`, `UName`, `Email`, `PasswordHash`, `Phone`, `Role [Admin|Customer|Manager]`, `CreatedAt`, `RefreshToken`, `RefreshTokenExpiresAt`

**Products**

* `PID (PK)`, `ProductName`, `Description`, `Price`, `Stock`, `OverallRating`,
  `CategoryId (FK)`, `SupplierId (FK)`, `ImageUrl` 

**Orders**

* `OID (PK)`, `UID (FK to Users)`, `OrderDate`, `TotalAmount`,
  `Status [Pending|Paid|Shipped|Delivered|Cancelled]`, `RowVersion (Concurrency)`

**OrderProducts** *(join table)*

* `OID (FK)`, `PID (FK)`, `Quantity`, `UnitPrice`

**Reviews**

* `ReviewID (PK)`, `UID (FK)`, `PID (FK)`, `Rating`, `Comment`, `ReviewDate`

**Categories** 

* `CategoryId (PK)`, `Name`, `Description`

**Suppliers** 

* `SupplierId (PK)`, `Name`, `ContactEmail`, `Phone`

---

##  Project Structure

```
E-CommerceSystem/
│
├─ Controllers/             # Web API controllers (Products, Orders, Users, Reviews, Categories, Suppliers, Reports, Auth)
├─ Services/                # Business services (interfaces + implementations)
├─ Repository/              # Repositories (data access / EF Core)
├─ DTOs/
│  ├─ Requests/             # Create/Update input DTOs
│  └─ Responses/            # Output DTOs (what APIs return)
├─ Models/                  # EF Core entities
├─ Mapping/                 # AutoMapper profiles
├─ Migrations/              # EF Core migrations
├─ Middleware/              # Error handling, logging, etc.
├─ Infrastructure/          # Auth, Email, Pdf/Invoice helpers, FileStorage
├─ appsettings*.json        # Configuration (DB, JWT, Mail, Serilog)
├─ Program.cs               # Composition root (DI, middleware, endpoints)
└─ README.md
```

---

##  Features 

### 1) New Models + CRUD + DTOs + AutoMapper

* **Category** and **Supplier** added with full CRUD.
* All controllers use **DTOs** and **AutoMapper** (no manual mapping).

### 2) Services Enhancements

* **Products: Pagination & Filtering**

  * `GET /api/products?search=iphone&minPrice=100&maxPrice=1500&page=1&pageSize=20`
* **Order Summary Service**

  * Aggregates Orders + Users + Products (totals, lines, amounts).
* **Product Image Upload**

  * `POST /api/products/{id}/image` *(multipart/form-data)*.
* **Order Cancellation**

  * `POST /api/orders/{id}/cancel` (restores stock, sends email).
* **Order Status Tracking**

  * Lifecycle: `Pending → Paid → Shipped → Delivered` (or `Cancelled`).
* **Email Notifications**

  * Sent when an order is **placed** or **canceled**.
* **Invoice Generation (PDF)**

  * `GET /api/orders/{id}/invoice` .

### 3) Security & Authentication

* **JWT + Refresh Tokens** (refresh stored in DB; tokens stored in **Cookies**).
* Passwords stored as **BCrypt hashes**.
* **Role-based authorization**: `Admin`, `Customer`, `Manager`.

### 4) Business Rules (Enforced in Services)

* A user can **only review products they purchased**.
* A user **cannot add more than one review** to the same product.
* **Optimistic Concurrency** on products & orders via `RowVersion` (timestamp):

  * Clients must send the latest `RowVersion` when updating.

### 5) Reports & Analytics (Admin)

* **Best-selling products**: `/api/admin/reports/best-sellers?from=2025-01-01&to=2025-01-31`
* **Revenue reports** (daily/monthly): `/api/admin/reports/revenue?granularity=month&from=...&to=...`
* **Top-rated products**: `/api/admin/reports/top-rated`
* **Most active customers**: `/api/admin/reports/active-customers`

### 6) Code Quality

* **Centralized error handling** middleware (uniform problem details).
* **Serilog** (file/console sinks) + `ILogger` in services/controllers.

---

##  Getting Started

### 1) Configure

Update **`appsettings.json`** (or `appsettings.Development.json`):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=ECommerceDb;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "Jwt": {
    "Issuer": "ECommerce.Api",
    "Audience": "ECommerce.SPA",
    "Key": "SUPER-SECRET-KEY-CHANGE-ME",
    "AccessTokenMinutes": 15,
    "RefreshTokenDays": 7
  },
  "Mail": {
    "SmtpHost": "smtp.example.com",
    "SmtpPort": 587,
    "User": "no-reply@example.com",
    "Password": "CHANGE_ME",
    "FromName": "E-Commerce"
  },
  "Serilog": {
    "MinimumLevel": "Information",
    "WriteTo": [ { "Name": "Console" } ]
  }
}
```

### 2) Database

```bash
dotnet tool install --global dotnet-ef           
dotnet ef database update                         # apply migrations
```

### 3) Run

```bash
dotnet run
# Swagger UI will be available at /swagger
```

---

##  Authentication Flow

* **Register/Login** to receive **access token** (JWT) and **refresh token**.
* Tokens are stored in **HTTP-Only cookies**.
* When the access token expires, call `POST /api/auth/refresh` to get a new one.
* Logout clears cookies and revokes refresh token.

---

##  Common API Examples

### Products (Pagination & Filter)

```
GET /api/products?search=watch&minPrice=100&maxPrice=400&page=2&pageSize=12
```

### Product Image Upload

```
POST /api/products/{pid}/image
Content-Type: multipart/form-data
Body: file=@/path/to/image.jpg
```

### Create Order

```
POST /api/orders
{
  "userId": 42,
  "items": [
    { "productId": 1, "quantity": 2 },
    { "productId": 5, "quantity": 1 }
  ]
}
```

### Cancel Order (restores stock + email)

```
POST /api/orders/{oid}/cancel
```

### Add Review (Purchase-guarded)

```
POST /api/reviews
{
  "productId": 10,
  "rating": 5,
  "comment": "Excellent!"
}
```

> Service validates that the user has bought `productId` and hasn’t already reviewed it.

### Concurrency (RowVersion)

* Entities expose `rowVersion` (base64).
* Client must include the latest value when updating:

```
PUT /api/orders/{oid}
If-Match: "base64RowVersionHere"  # or send in body (depending on design)
```

---

##  AutoMapper & DTOs

* Incoming requests use **Request DTOs** (Create/Update).
* Responses use **Response DTOs** (hide internal fields such as hashes, rowversion).
* All mappings configured in `MappingProfiles`.

---

##  Logging & Errors

* **Serilog** records structured logs (request/response, exceptions).
* Centralized **Error Handling Middleware** returns consistent **ProblemDetails** payloads.

---

##  Admin Reports

* **Best Sellers**: quantity & revenue per product.
* **Revenue**: totals per day/month.
* **Top Rated**: average ratings & counts.
* **Active Customers**: orders & value per user.

---

##  Future Work

* External **payment gateway** integration (Stripe/PayPal).
* **Docker** & containerized deployment.
* **Redis** caching (catalog, reports).
* **CI/CD** (GitHub Actions/Azure DevOps).
* **Rate limiting** & **API Keys** for partners.
* **Search** improvements (EF.Functions.FreeText / Elastic).

---

##  Contribution

1. Create a feature branch: `feat/<short-name>`
2. Add unit/integration tests when possible.
3. Follow existing patterns (DTOs, Services, Repos, AutoMapper).
4. Submit PR with a clear description & screenshots (when relevant).

---





