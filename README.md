# E-Commerce API

A robust **ASP.NET Core Web API** built with **Clean Architecture** principles, designed for **scalability**, **security**, and **maintainability**. The project covers real-world eCommerce needs including authentication, payments, image uploads, and role-based access control.

---

## Features

*  **User Authentication & Authorization** using **JWT Tokens**
*  **Role-Based Access Control (RBAC)** for Users & Admins
*  **Unit of Work & Repository Pattern** for clean data access
*  **Generic Repository** for reusable CRUD operations
*  **OTP Verification** for secure user authentication
*  **Email Service** using **MailKit & MimeKit**
*  **Payment Integration** with **Stripe API**
*  **AutoMapper** for DTO ↔ Domain model mapping
*  **SQL Server** for persistent data storage
*  **Dependency Injection (DI)** for loosely coupled architecture
*  **Image Upload Service** using **Cloudinary API**

---

## Technologies Used

* ASP.NET Core Web API (.NET 8)
* SQL Server
* JWT Authentication
* Stripe Payment Gateway
* MailKit & MimeKit
* AutoMapper
* Unit of Work Pattern
* Generic Repository Pattern
* Dependency Injection
* OTP-based Authentication
* Cloudinary API (Image Uploads)

---

## Setup & Installation

### Prerequisites

Make sure you have the following installed:

* .NET 8 SDK
* SQL Server
* Visual Studio 
* Stripe Account
* Cloudinary Account

---

### Clone the Repository

```bash
git clone https://github.com/NdaYoussef/E-CommereceAPI.git
cd E-CommereceAPI
```

## Authentication Flow

1. User registers with email
2. OTP is sent via email
3. OTP verification confirms the account
4. JWT token is generated for secure access
5. Role-based authorization controls access to endpoints

---

## Payment Flow (Stripe)

* Create payment intent
* Process payment securely using Stripe
* Store transaction details in the database


