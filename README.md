# E-commerce App

A personal e-commerce project built in 2025 with a React frontend and an ASP.NET Core Web API backend.

The project was an attempt to build a larger online store, including product browsing, filtering, product details, a shopping cart, and backend data management. As the scope grew, the project became too large to complete within the available time, so development was stopped before the application was finished.

**Project status: Incomplete / discontinued**

## Project Overview

The application is structured as two main parts:

- **Frontend:** A React and TypeScript single-page application.
- **Backend:** An ASP.NET Core Web API that exposes endpoints for product and sales-related data.
- **Class library:** Shared models, DTOs, Entity Framework Core context, service interfaces, service implementations, and AutoMapper configuration.

The project uses a clothing-store concept, with products organized by attributes such as brand, category, color, and size.

## Features Implemented

The repository contains implementations for the following areas:

### Frontend

- Home page with product-category navigation
- Product listing page
- Product detail page
- Product image slider
- Product variants by color and size
- Product filtering UI
- Shopping cart UI with quantity updates and item removal
- Responsive desktop and mobile navigation
- React Router navigation between pages

### Backend API

- Product CRUD endpoints
- Brand, category, color, and size endpoints
- Stock management endpoints
- Customer CRUD endpoints
- Cart item CRUD endpoints
- Order CRUD endpoints
- Staff management endpoints
- Order-to-staff assignment endpoints
- Swagger/OpenAPI setup for development

### Data and Architecture

- Entity Framework Core database context
- SQL Server integration
- DTOs for transferring data between the API and frontend
- Service and interface separation
- AutoMapper configuration
- Entity relationships for products, stock, customers, orders, cart items, and staff assignments

## Technologies Used

### Frontend

- React 18
- TypeScript
- React Router
- Axios
- CSS Modules

### Backend

- C#
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core 8
- Microsoft SQL Server
- AutoMapper
- Swagger / Swashbuckle

## Project Structure

```text
E-commerceApp/
├── E-commerce/
│   ├── Controllers/
│   │   ├── Production/
│   │   └── Sales/
│   ├── Program.cs
│   └── appsettings.json
│
├── E-commerceClassLibrary/
│   ├── AutoMapper/
│   ├── Context/
│   ├── DTO/
│   ├── Interfaces/
│   ├── Models/
│   └── Services/
│
└── ecommercefront/
    ├── public/
    └── src/
        ├── api/
        ├── components/
        └── pages/
```

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/)
- [SQL Server LocalDB or another SQL Server instance](https://www.microsoft.com/sql-server)
- An IDE such as [Visual Studio](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)

### 1. Clone the repository

```bash
git clone https://github.com/TacoKrydda/E-commerceApp.git
cd E-commerceApp
```

### 2. Configure the database

The backend reads its SQL Server connection string from:

`E-commerce/appsettings.json`

The checked-in configuration uses SQL Server LocalDB:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ECommerceDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

Change the connection string if you are using a different SQL Server instance. Do not commit private credentials or production connection strings.

### 3. Run the backend

Open a terminal in the `E-commerce` directory:

```bash
cd E-commerce
dotnet restore
dotnet run
```

The frontend's Axios configuration currently points to:

```text
https://localhost:7100/api
```

If the backend starts on a different HTTPS port, update `baseURL` in `ecommercefront/src/api/axiosInstance.tsx` to match.

Swagger is configured for the development environment. Open the Swagger URL printed by the application when it starts.

### 4. Run the frontend

Open a second terminal in the `ecommercefront` directory:

```bash
cd ecommercefront
npm install
npm start
```

The React development server normally runs at `http://localhost:3000`. The backend CORS policy is configured to allow this origin.

## Current Limitations

This project was discontinued before it reached a complete, production-ready state. The repository represents work in progress rather than a finished online store.

Examples of unfinished areas visible in the current code:

- The login page is a UI placeholder; authentication is not implemented.
- The checkout/confirm-purchase button is disabled.
- The frontend shopping cart is currently managed in React state; a complete checkout flow connecting it to customer and order handling is not finished.
- Some backend sales endpoints and data models exist, but that does not mean the complete customer purchase journey is implemented.
- The application has not been completed or prepared as a production deployment.

These limitations are intentional to document: the project grew beyond its original scope and was stopped early.

## What I Explored

Through this project, I worked with a larger application structure that separates a React frontend from an ASP.NET Core Web API. I explored how to organize product and sales domains, model database relationships with Entity Framework Core, use DTOs and service layers, and connect a frontend to backend endpoints with Axios.

## Author

[TacoKrydda](https://github.com/TacoKrydda)

---

*Personal project developed in 2025. Development was discontinued before completion because the project scope became too large.*
