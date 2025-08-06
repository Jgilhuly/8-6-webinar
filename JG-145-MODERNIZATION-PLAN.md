# JG-145: RestaurantOps Legacy Application Modernization Plan

## Overview

**Ticket**: [JG-145](https://linear.app/jg-org/issue/JG-145/modernize-restaurantops-legacy-application)  
**Status**: In Progress (25% Complete)  
**Priority**: High  
**Project**: DOTNET  

Refactor the legacy ASP.NET Core MVC application to implement modern development patterns and improve maintainability, performance, and testability.

## Current State Assessment

### ✅ **COMPLETED MODERNIZATION TASKS**

#### JG-146: Implement Dependency Injection Infrastructure ✅ **DONE**
- **Status**: Complete
- **Description**: Replace direct repository instantiation with proper DI container registration
- **Completed Tasks**:
  - ✅ Created repository interfaces (IOrderRepository, IMenuRepository, etc.)
  - ✅ Registered services in Program.cs with proper lifetimes
  - ✅ Updated controllers to use constructor injection
  - ✅ Removed static SqlHelper initialization
  - ✅ Updated Program.cs to use DI container

**Implementation Details**:
```csharp
// Repository interfaces created
public interface IOrderRepository
{
    Order Create(int tableId);
    Order? GetById(int orderId);
    // ... other methods
}

// Services registered in Program.cs
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
// ... other repositories

// Controllers use constructor injection
public class OrderController : Controller
{
    private readonly IOrderRepository _orderRepo;
    
    public OrderController(IOrderRepository orderRepo)
    {
        _orderRepo = orderRepo;
    }
}
```

#### JG-147: Migrate to Entity Framework Core ✅ **DONE**
- **Status**: Complete
- **Description**: Successfully migrated from raw ADO.NET to EF Core for type-safe data access
- **Completed Tasks**:
  - ✅ Install EF Core packages (Microsoft.EntityFrameworkCore.SqlServer)
  - ✅ Create DbContext with proper entity configurations
  - ✅ Define entity relationships and constraints
  - ✅ Migrate existing SQL queries to LINQ expressions
  - ✅ Add database migrations
  - ✅ Update connection string configuration
  - ✅ Replace SqlHelper usage with DbContext

**Implementation Details**:
```csharp
// DbContext Configuration
public class RestaurantOpsContext : DbContext
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderLine> OrderLines { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }
    // ... other entities
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure relationships and constraints
    }
}

// Repository Updates
public class OrderRepository : IOrderRepository
{
    private readonly RestaurantOpsContext _context;
    
    public OrderRepository(RestaurantOpsContext context)
    {
        _context = context;
    }
    
    public Order? GetById(int id)
    {
        return _context.Orders
            .Include(o => o.Lines)
            .FirstOrDefault(o => o.OrderId == id);
    }
}
```

**Entity Relationships Established**:
- Order ↔ OrderLine: One-to-many
- Order ↔ RestaurantTable: Many-to-one
- MenuItem ↔ Category: Many-to-one
- Employee ↔ Shift: One-to-many
- Employee ↔ TimeOff: One-to-many
- Ingredient ↔ InventoryTx: One-to-many

---

## 🔄 **REMAINING MODERNIZATION TASKS**

### JG-148: Implement Async/Await Patterns ⏳ **PENDING**
- **Priority**: Medium
- **Description**: Convert synchronous operations to asynchronous for better performance and scalability
- **Current State**: All database operations are synchronous
- **Tasks**:
  - [ ] Update all repository methods to return Task<T>
  - [ ] Convert controller actions to async
  - [ ] Ensure proper exception handling in async contexts
  - [ ] Add cancellation token support where appropriate
  - [ ] Update service layer methods to be async

**Implementation Plan**:
```csharp
// Update repository methods
public async Task<Order?> GetByIdAsync(int orderId)
{
    return await _context.Orders
        .Include(o => o.Lines)
        .FirstOrDefaultAsync(o => o.OrderId == orderId);
}

// Update controller actions
public async Task<IActionResult> Details(int id)
{
    var order = await _orderRepo.GetByIdAsync(id);
    if (order == null)
    {
        return NotFound();
    }
    return View(order);
}
```

### JG-149: Add Service Layer Architecture ⏳ **PENDING**
- **Priority**: Medium
- **Description**: Introduce business logic layer between controllers and repositories
- **Current State**: Business logic mixed in controllers and repositories
- **Tasks**:
  - [ ] Create service interfaces and implementations
  - [ ] Move business logic from controllers to services
  - [ ] Implement proper error handling and validation
  - [ ] Add logging and monitoring capabilities
  - [ ] Create DTOs for complex operations
  - [ ] Update controllers to use services instead of repositories

**Implementation Plan**:
```csharp
// Create service interfaces
public interface IOrderService
{
    Task<OrderDto> CreateOrderAsync(int tableId);
    Task<OrderDto> GetOrderDetailsAsync(int orderId);
    Task<bool> AddItemToOrderAsync(int orderId, int menuItemId, int quantity);
    Task<bool> SubmitOrderAsync(int orderId);
    Task<bool> CloseOrderAsync(int orderId);
}

// Implement services
public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepo;
    private readonly IMenuRepository _menuRepo;
    private readonly ILogger<OrderService> _logger;
    
    public OrderService(IOrderRepository orderRepo, IMenuRepository menuRepo, ILogger<OrderService> logger)
    {
        _orderRepo = orderRepo;
        _menuRepo = menuRepo;
        _logger = logger;
    }
}
```

### JG-150: Enhance Model Validation ⏳ **PENDING**
- **Priority**: Medium
- **Description**: Add comprehensive validation to domain models and improve data integrity
- **Current State**: Minimal validation on models
- **Tasks**:
  - [ ] Implement data annotations for validation
  - [ ] Add custom validation attributes where needed
  - [ ] Create view models for complex forms
  - [ ] Implement client-side validation
  - [ ] Add business rule validation
  - [ ] Create validation error messages

**Implementation Plan**:
```csharp
// Add validation to models
public class MenuItem
{
    public int MenuItemId { get; set; }
    
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Description is required")]
    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string Description { get; set; } = string.Empty;
    
    [Range(0.01, 1000.00, ErrorMessage = "Price must be between $0.01 and $1000.00")]
    public decimal Price { get; set; }
}

// Create view models for complex operations
public class CreateOrderViewModel
{
    [Required(ErrorMessage = "Table selection is required")]
    public int TableId { get; set; }
    
    [Required(ErrorMessage = "At least one item is required")]
    [MinLength(1, ErrorMessage = "Order must contain at least one item")]
    public List<OrderLineViewModel> Items { get; set; } = new();
}
```

### JG-151: Improve Error Handling and Logging ⏳ **PENDING**
- **Priority**: Medium
- **Description**: Implement comprehensive error handling and logging throughout the application
- **Current State**: Basic exception handling only
- **Tasks**:
  - [ ] Add global exception handling middleware
  - [ ] Implement structured logging with Serilog
  - [ ] Add health checks for monitoring
  - [ ] Create custom error pages
  - [ ] Add request/response logging
  - [ ] Implement proper error responses

**Implementation Plan**:
```csharp
// Global exception handling middleware
public class GlobalExceptionHandler : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    
    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }
}

// Configure Serilog in Program.cs
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/restaurantops-.txt", rollingInterval: RollingInterval.Day));
```

### JG-152: Add Unit Testing Infrastructure ⏳ **PENDING**
- **Priority**: Medium
- **Description**: Implement comprehensive unit testing for the modernized components
- **Current State**: Limited unit testing
- **Tasks**:
  - [ ] Set up test project with proper mocking
  - [ ] Create unit tests for services and repositories
  - [ ] Add integration tests for data access
  - [ ] Implement test data builders
  - [ ] Add test coverage reporting
  - [ ] Create test utilities and helpers

**Implementation Plan**:
```csharp
// Test data builders
public class OrderBuilder
{
    private Order _order = new()
    {
        OrderId = 1,
        TableId = 1,
        Status = "Open",
        CreatedAt = DateTime.UtcNow
    };
    
    public OrderBuilder WithTableId(int tableId)
    {
        _order.TableId = tableId;
        return this;
    }
    
    public Order Build() => _order;
}

// Unit tests for services
[TestFixture]
public class OrderServiceTests
{
    private Mock<IOrderRepository> _mockOrderRepo;
    private Mock<IMenuRepository> _mockMenuRepo;
    private Mock<ILogger<OrderService>> _mockLogger;
    private OrderService _service;
    
    [SetUp]
    public void Setup()
    {
        _mockOrderRepo = new Mock<IOrderRepository>();
        _mockMenuRepo = new Mock<IMenuRepository>();
        _mockLogger = new Mock<ILogger<OrderService>>();
        _service = new OrderService(_mockOrderRepo.Object, _mockMenuRepo.Object, _mockLogger.Object);
    }
    
    [Test]
    public async Task CreateOrderAsync_ValidTableId_ReturnsOrder()
    {
        // Arrange
        var tableId = 1;
        var expectedOrder = new OrderBuilder().WithTableId(tableId).Build();
        _mockOrderRepo.Setup(r => r.CreateAsync(tableId)).ReturnsAsync(expectedOrder);
        
        // Act
        var result = await _service.CreateOrderAsync(tableId);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.TableId, Is.EqualTo(tableId));
        _mockOrderRepo.Verify(r => r.CreateAsync(tableId), Times.Once);
    }
}
```

### JG-153: Update Documentation and Onboarding ⏳ **PENDING**
- **Priority**: Low
- **Description**: Update project documentation to reflect the modernized architecture
- **Current State**: Documentation reflects legacy patterns
- **Tasks**:
  - [ ] Update ARCHITECTURE.md with new patterns
  - [ ] Revise ONBOARDING.md for new setup process
  - [ ] Add API documentation if applicable
  - [ ] Create development guidelines
  - [ ] Update README with modern setup instructions
  - [ ] Add troubleshooting section

**Implementation Plan**:
```markdown
# Updated Documentation Structure

## Modern Architecture
- Dependency Injection for all services
- Entity Framework Core for data access
- Async/await patterns throughout
- Service layer for business logic
- Comprehensive error handling and logging

## Setup Instructions
1. Install .NET 8.0 SDK
2. Run `dotnet restore`
3. Update connection string in appsettings.json
4. Run EF Core migrations: `dotnet ef database update`
5. Start application: `dotnet run`

## Development Guidelines
- Use async/await for all I/O operations
- Implement interfaces for testability
- Add validation to all models
- Write unit tests for new features
- Follow SOLID principles
```

---

## Success Criteria Progress

### ✅ **COMPLETED CRITERIA**
- [x] All data access uses EF Core
- [x] Controllers use constructor injection
- [x] Proper entity relationships defined
- [x] Database migrations in place
- [x] Type-safe query operations

### ⏳ **REMAINING CRITERIA**
- [ ] All operations are async
- [ ] Comprehensive unit test coverage
- [ ] Proper error handling and logging
- [ ] Updated documentation
- [ ] Service layer architecture implemented
- [ ] Enhanced model validation

---

## Technical Debt Addressed

### ✅ **RESOLVED**
- Legacy ADO.NET patterns → Entity Framework Core
- Tight coupling between components → Dependency Injection
- Direct repository instantiation → Constructor injection

### 🔄 **STILL NEEDS ADDRESSING**
- Synchronous I/O operations → Async/await patterns
- Lack of proper separation of concerns → Service layer
- Minimal error handling → Comprehensive logging
- Limited testability → Unit testing infrastructure

---

## Next Steps Priority

1. **High Priority**: Implement async/await patterns (JG-148)
2. **Medium Priority**: Add service layer architecture (JG-149)
3. **Medium Priority**: Enhance model validation (JG-150)
4. **Medium Priority**: Improve error handling and logging (JG-151)
5. **Medium Priority**: Add unit testing infrastructure (JG-152)
6. **Low Priority**: Update documentation and onboarding (JG-153)

---

## Files Modified During Modernization

### ✅ **COMPLETED CHANGES**
- Added: `RestaurantOpsContext.cs`
- Updated: All repository classes
- Updated: All model classes with navigation properties
- Updated: `Program.cs` with EF Core services
- Removed: `SqlHelper.cs`
- Updated: `ARCHITECTURE.md`

### 🔄 **PENDING CHANGES**
- Service layer interfaces and implementations
- Async repository method signatures
- Enhanced model validation attributes
- Global exception handling middleware
- Comprehensive unit test suite
- Updated documentation files

---

*Last Updated: January 2025*  
*Progress: 2/8 sub-tickets complete (25%)* 