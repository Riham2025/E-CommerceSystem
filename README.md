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

Categories 1↔∞ Products (new)

Suppliers 1↔∞ Products (new)

Orders ∞→1 Users