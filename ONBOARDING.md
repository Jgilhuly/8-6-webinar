# RestaurantOps Legacy Application - Onboarding Guide

## Welcome to RestaurantOps! 🍽️

This guide will help you get up and running with the RestaurantOps legacy application, a comprehensive restaurant management system built with ASP.NET Core MVC.

## Table of Contents

1. [Project Overview](#project-overview)
2. [Prerequisites](#prerequisites)
3. [Getting Started](#getting-started)
4. [Project Structure](#project-structure)
5. [Key Features](#key-features)
6. [Development Workflow](#development-workflow)
7. [Database Setup](#database-setup)
8. [Testing](#testing)
9. [Common Tasks](#common-tasks)
10. [Troubleshooting](#troubleshooting)

## Project Overview

RestaurantOps is a legacy ASP.NET Core MVC application that manages restaurant operations including:

- **Order Management**: Create, view, and process customer orders
- **Menu Management**: Manage menu items and categories
- **Inventory Tracking**: Monitor ingredients and generate reorder reports
- **Table Management**: Track table occupancy and reservations
- **Employee Scheduling**: Manage shifts and time-off requests
- **Payment Processing**: Handle payment transactions
- **Kitchen Display**: Real-time order display for kitchen staff

## Prerequisites

Before you begin, ensure you have the following installed:

### Required Software
- **.NET 8.0 SDK** - [Download here](https://dotnet.microsoft.com/download/dotnet/8.0)
- **SQL Server** (2019+) or **SQL Server Express** - [Download here](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- **Visual Studio 2022** or **VS Code** with C# extension
- **Git** for version control

### Optional Tools
- **SQL Server Management Studio (SSMS)** for database management
- **Postman** or similar for API testing

## Getting Started

### 1. Clone the Repository
```bash
git clone <repository-url>
cd 8-6-webinar
```

### 2. Database Setup
1. Open SQL Server Management Studio or Azure Data Studio
2. Connect to your SQL Server instance
3. Run the database initialization script:
   ```sql
   -- Execute the contents of Database/init.sql
   ```

### 3. Configure Connection String
1. Open `RestaurantOps.Legacy/appsettings.json`
2. Update the connection string to match your SQL Server instance:
   ```json
   {
     "ConnectionStrings": {
       "Default": "Server=localhost;Database=RestaurantOps;Trusted_Connection=true;TrustServerCertificate=true;"
     }
   }
   ```

### 4. Build and Run
```bash
# Navigate to the main project
cd RestaurantOps.Legacy

# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run
```

The application will be available at `https://localhost:5001` or `http://localhost:5000`

## Project Structure

```
RestaurantOps.Legacy/
├── Controllers/          # MVC Controllers
│   ├── HomeController.cs
│   ├── OrderController.cs
│   ├── MenuController.cs
│   ├── InventoryController.cs
│   ├── ScheduleController.cs
│   ├── TablesController.cs
│   ├── PaymentGatewayController.cs
│   └── KitchenDisplayController.cs
├── Data/                # Data Access Layer
│   ├── SqlHelper.cs     # Database connection utility
│   ├── IOrderRepository.cs
│   ├── OrderRepository.cs
│   ├── IMenuRepository.cs
│   ├── MenuRepository.cs
│   ├── IInventoryRepository.cs
│   ├── InventoryRepository.cs
│   ├── IEmployeeRepository.cs
│   ├── EmployeeRepository.cs
│   ├── IShiftRepository.cs
│   ├── ShiftRepository.cs
│   ├── ITableRepository.cs
│   ├── TableRepository.cs
│   ├── IIngredientRepository.cs
│   └── IngredientRepository.cs
├── Models/              # Data Models
│   ├── Order.cs
│   ├── OrderLine.cs
│   ├── MenuItem.cs
│   ├── Category.cs
│   ├── RestaurantTable.cs
│   ├── Ingredient.cs
│   ├── InventoryTx.cs
│   ├── Employee.cs
│   ├── Shift.cs
│   └── TimeOff.cs
├── Views/               # Razor Views
│   ├── Home/
│   ├── Order/
│   ├── Menu/
│   ├── Inventory/
│   ├── Schedule/
│   ├── Tables/
│   └── Shared/
└── wwwroot/            # Static Files
    ├── css/
    ├── js/
    └── lib/
```

## Database Setup

### Initial Setup
The database schema is defined in `Database/init.sql`. This script:

1. Creates the RestaurantOps database
2. Creates all necessary tables
3. Seeds initial data (categories, menu items, tables)

### Database Schema Overview

**Core Tables:**
- `Orders` - Restaurant orders with status tracking
- `OrderLines` - Individual items in orders
- `MenuItems` - Menu offerings with pricing
- `Categories` - Menu categorization
- `RestaurantTables` - Physical dining tables

**Inventory Tables:**
- `Ingredients` - Raw materials tracking
- `InventoryTx` - Inventory transaction history

**Employee Tables:**
- `Employees` - Staff information
- `Shifts` - Work schedule management
- `TimeOff` - Time-off request tracking

### Adding New Tables
1. Add the table creation script to `Database/init.sql`
2. Create a corresponding model in `Models/`
3. Create a repository in `Data/`
4. Update the controller if needed

## Dependency Injection

The application uses ASP.NET Core's built-in DI container. All repositories are registered as scoped services in `Program.cs`:

### Adding New Repositories
1. Create an interface (e.g., `INewRepository.cs`)
2. Create the implementation (e.g., `NewRepository.cs`)
3. Register the service in `Program.cs`:
   ```csharp
   builder.Services.AddScoped<INewRepository, NewRepository>();
   ```
4. Inject the interface into controllers via constructor injection

### Repository Interfaces
- `IOrderRepository` - Order management operations
- `IMenuRepository` - Menu item operations  
- `ITableRepository` - Table management
- `IIngredientRepository` - Ingredient tracking
- `IInventoryRepository` - Inventory management
- `IEmployeeRepository` - Employee data operations
- `IShiftRepository` - Shift scheduling

## Testing

The solution includes a comprehensive test suite in `RestaurantOps.Tests/`:

### Running Tests
```bash
cd RestaurantOps.Tests
dotnet test
```

### Test Structure
- **UnitTests/**: Individual component testing
- **IntegrationTests/**: End-to-end testing
- **Builders/**: Test data builders

### Writing Tests
1. Create test classes in the appropriate folder
2. Use the existing builders for test data
3. Follow the Arrange-Act-Assert pattern
4. Test both success and failure scenarios

## Common Tasks

### Adding a New Menu Item
1. Navigate to `/Menu/Create`
2. Fill in the form with item details
3. Select a category
4. Set price and availability
5. Save the item

### Creating an Order
1. Go to `/Order/Create`
2. Select a table
3. Add menu items to the order
4. Set quantities
5. Submit the order

### Checking Inventory
1. Navigate to `/Inventory`
2. View current ingredient levels
3. Check reorder reports
4. Update quantities as needed

### Managing Employee Schedules
1. Go to `/Schedule`
2. View current shifts
3. Create new shifts
4. Handle time-off requests

## Troubleshooting

### Common Issues

**1. Database Connection Error**
- Verify SQL Server is running
- Check connection string in `appsettings.json`
- Ensure database exists and is accessible

**2. Build Errors**
- Run `dotnet restore` to restore packages
- Check for missing dependencies
- Verify .NET 8.0 SDK is installed

**3. Runtime Errors**
- Check application logs in Visual Studio Output window
- Verify database schema matches expectations
- Ensure all required tables exist

**4. Static Files Not Loading**
- Check `wwwroot` folder structure
- Verify file permissions
- Ensure files are included in project

### Getting Help

1. Check the application logs for detailed error messages
2. Review the `ARCHITECTURE.md` file for system overview
3. Examine existing code patterns for guidance
4. Use the test suite to understand expected behavior

## Next Steps

After completing this onboarding:

1. **Explore the Application**: Navigate through all features to understand the workflow
2. **Review the Codebase**: Examine controllers, repositories, and models
3. **Run Tests**: Ensure all tests pass and understand test coverage
4. **Make a Small Change**: Try adding a simple feature to get familiar with the development process
5. **Read Architecture Docs**: Review `ARCHITECTURE.md` for detailed system information

## Additional Resources

- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Documentation](https://docs.microsoft.com/en-us/ef/)
- [SQL Server Documentation](https://docs.microsoft.com/en-us/sql/sql-server/)

---

**Happy Coding! 🚀**

If you have any questions or need assistance, don't hesitate to reach out to the development team. 