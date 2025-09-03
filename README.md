## **E-CommerceSystem**

# Overview
This project is an E-Commerce Backend System designed to manage products, users, orders, and reviews.
It provides RESTful APIs for order processing, product management, user authentication, and business analytics.

As part of Phase 2, new features, enhancements, and stricter business rules were added to extend the base system.

# Database Schema
Entities & Relationships :

Users :
Manages customers & admins (fields: UID, UName, Email, Password, Role, etc.)

One User → Many Orders

One User → Many Reviews

Products : 

Manages product catalog (fields: PID, ProductName, Description, Price, Stock, OverallRating)

One Product → Many Reviews

Many Products ↔ Many Orders (via OrderProducts)

