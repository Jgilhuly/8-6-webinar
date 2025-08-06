using Microsoft.EntityFrameworkCore;
using RestaurantOps.Legacy.Models;

namespace RestaurantOps.Legacy.Data
{
    public class RestaurantOpsContext : DbContext
    {
        public RestaurantOpsContext(DbContextOptions<RestaurantOpsContext> options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<RestaurantTable> RestaurantTables { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<InventoryTx> InventoryTransactions { get; set; }
        public DbSet<TimeOff> TimeOffs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Order entity
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderId);
                entity.Property(e => e.Status).HasMaxLength(20);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
                entity.HasOne(e => e.Table)
                    .WithMany()
                    .HasForeignKey(e => e.TableId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure OrderLine entity
            modelBuilder.Entity<OrderLine>(entity =>
            {
                entity.HasKey(e => e.OrderLineId);
                entity.Property(e => e.PriceEach).HasColumnType("decimal(10,2)");
                entity.HasOne(e => e.Order)
                    .WithMany(o => o.Lines)
                    .HasForeignKey(e => e.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.MenuItem)
                    .WithMany()
                    .HasForeignKey(e => e.MenuItemId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure MenuItem entity
            modelBuilder.Entity<MenuItem>(entity =>
            {
                entity.HasKey(e => e.MenuItemId);
                entity.Property(e => e.Price).HasColumnType("decimal(10,2)");
                entity.Property(e => e.Name).HasMaxLength(150);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.HasOne(e => e.Category)
                    .WithMany(c => c.MenuItems)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Category entity
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryId);
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
            });

            // Configure RestaurantTable entity
            modelBuilder.Entity<RestaurantTable>(entity =>
            {
                entity.HasKey(e => e.TableId);
                entity.Property(e => e.Name).HasMaxLength(50);
            });

            // Configure Employee entity
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmployeeId);
                entity.Property(e => e.FirstName).HasMaxLength(50);
                entity.Property(e => e.LastName).HasMaxLength(50);
                entity.Property(e => e.Role).HasMaxLength(30);
            });

            // Configure Shift entity
            modelBuilder.Entity<Shift>(entity =>
            {
                entity.HasKey(e => e.ShiftId);
                entity.HasOne(e => e.Employee)
                    .WithMany()
                    .HasForeignKey(e => e.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Ingredient entity
            modelBuilder.Entity<Ingredient>(entity =>
            {
                entity.HasKey(e => e.IngredientId);
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.Unit).HasMaxLength(20);
                entity.Property(e => e.QuantityOnHand).HasColumnType("decimal(10,2)");
                entity.Property(e => e.ReorderThreshold).HasColumnType("decimal(10,2)");
            });

            // Configure InventoryTx entity
            modelBuilder.Entity<InventoryTx>(entity =>
            {
                entity.HasKey(e => e.TxId);
                entity.Property(e => e.QuantityChange).HasColumnType("decimal(10,2)");
                entity.HasOne(e => e.Ingredient)
                    .WithMany()
                    .HasForeignKey(e => e.IngredientId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure TimeOff entity
            modelBuilder.Entity<TimeOff>(entity =>
            {
                entity.HasKey(e => e.TimeOffId);
                entity.HasOne(e => e.Employee)
                    .WithMany()
                    .HasForeignKey(e => e.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
} 