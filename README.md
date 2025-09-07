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