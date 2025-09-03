## **E-CommerceSystem**

# Overview
This project is an E-Commerce Backend System designed to manage products, users, orders, and reviews.
It provides RESTful APIs for order processing, product management, user authentication, and business analytics.

As part of Phase 2, new features, enhancements, and stricter business rules were added to extend the base system.

# Database Schema
Entities & Relationships :

**Users :
Manages customers & admins (fields: UID, UName, Email, Password, Role, etc.)

One User → Many Orders

One User → Many Reviews

**Products : 

Manages product catalog (fields: PID, ProductName, Description, Price, Stock, OverallRating)

One Product → Many Reviews

Many Products ↔ Many Orders (via OrderProducts)

**Orders :

Contains order details (fields: OID, OrderDate, TotalAmount, UID, Status)

One Order → Many OrderProducts

**OrderProducts :

Join table for many-to-many (Order ↔ Product) with quantity

**Reviews :

Stores product reviews (fields: ReviewID, Rating, Comment, UID, PID)

**Category :

CategoryId, Name, Description

One Category → Many Products

Supplier

SupplierId, Name, ContactEmail, Phone

One Supplier → Many Products





