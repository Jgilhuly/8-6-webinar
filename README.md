# Restaurant Operations System

A comprehensive .NET Core web application for restaurant back-office operations. The system handles menu management, inventory tracking, employee scheduling, and reporting to streamline restaurant operations.

## Overview

This project provides a complete restaurant management system built with ASP.NET Core MVC. It serves as a robust solution for restaurant owners and managers to handle daily operations efficiently.

## Features

- **Menu Management** - CRUD operations for menu items, categories, pricing, and availability
- **Table & Order Management** - Floor plan view, table seating, order creation and modification
- **Inventory Tracking** - Ingredient management, stock adjustments, and reorder thresholds
- **Employee Scheduling** - Role definitions, shift planning, and time-off requests
- **Daily Reports** - Sales summaries, labor analysis, and inventory variance
- **Integration Stubs** - Receipt printer, payment gateway, and kitchen display system

## Architecture

Built as a monolithic ASP.NET Core MVC application with:
- **Frontend**: Server-rendered Razor views with Bootstrap 4 and jQuery
- **Data Layer**: Entity Framework Core with SQL Server
- **Services**: Repository pattern with dependency injection
- **Authentication**: Cookie-based auth with role-based access control
- **Logging**: Structured logging with Serilog

## Setup

1. **Prerequisites**:
   - .NET 6+ SDK
   - SQL Server or SQL Server Express
   - Docker (optional)

2. **Database Setup**:
   ```bash
   # Initialize the database
   dotnet run --project RestaurantOps.Legacy -- --setup-db
   ```

3. **Run the application**:
   ```bash
   cd RestaurantOps.Legacy
   dotnet run
   ```

4. **Access the application**:
   - Open `http://localhost:5000`
   - Default credentials: admin/password

## Development

For development with auto-reload:
```bash
dotnet watch run --project RestaurantOps.Legacy
```

## Technology Stack

- **Backend**: ASP.NET Core 6, Entity Framework Core
- **Frontend**: Razor Views, Bootstrap 4, jQuery
- **Database**: SQL Server with EF Core migrations
- **Testing**: xUnit with integration and unit tests
- **Logging**: Serilog with structured logging

## Database Schema

Core entities include:
- **Menu**: Categories, MenuItems, Pricing
- **Orders**: Tables, Orders, OrderLines
- **Inventory**: Ingredients, InventoryTransactions
- **Staff**: Employees, Shifts, TimeOff
- **Reports**: Sales, Labor, Inventory summaries 