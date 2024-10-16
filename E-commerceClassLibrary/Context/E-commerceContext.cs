using E_commerceClassLibrary.Models.Production;
using E_commerceClassLibrary.Models.Sales;
using Microsoft.EntityFrameworkCore;



namespace E_commerceClassLibrary.Context
{
    public class E_commerceContext : DbContext
    {
        public E_commerceContext()
        {

        }
        public E_commerceContext(DbContextOptions<E_commerceContext> options) : base(options)
        {

        }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderStaffAssignment> OrderStaffAssignments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Stock> Stocks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Använd för att aktivera känslig datalogging vid felsökning
                optionsBuilder.EnableSensitiveDataLogging();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Definiera relationer
            #region HasOneWithMany

            #region Prodction
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BrandId);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Color)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.ColorId);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Size)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.SizeId);

            modelBuilder.Entity<Stock>()
                .HasOne(s => s.Product)
                .WithOne(p => p.Stock)
                .HasForeignKey<Stock>(s => s.ProductId);
            #endregion

            #region Sales

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Order)
                .WithMany(o => o.CartItems)
                .HasForeignKey(ci => ci.OrderId);

            modelBuilder.Entity<OrderStaffAssignment>()
                .HasOne(osa => osa.Order)
                .WithMany(o => o.OrderStaffAssignments)
                .HasForeignKey(osa => osa.OrderId);

            modelBuilder.Entity<OrderStaffAssignment>()
                .HasOne(osa => osa.Staff)
                .WithMany(s => s.OrderStaffAssignments)
                .HasForeignKey(osa => osa.StaffId);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany()
                .HasForeignKey(ci => ci.ProductId);

            #endregion

            #endregion

            // Valideringar på fält
            #region IsRequiredHasMaxLength

            #region Production
            modelBuilder.Entity<Brand>()
                .Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Category>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Color>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Product>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Size>()
                .Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            #endregion

            #region Sales

            modelBuilder.Entity<Customer>()
                .Property(c => c.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Customer>()
                .Property(c => c.LastName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Customer>()
                .Property(c => c.Phone)
                .IsRequired()
                .HasMaxLength(20);

            modelBuilder.Entity<Customer>()
                .Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Customer>()
                .Property(c => c.Street)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Customer>()
                .Property(c => c.City)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Order>()
                .Property(o => o.OrderStatus)
                .IsRequired()
                .HasMaxLength(20);

            modelBuilder.Entity<Staff>()
                .Property(s => s.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Staff>()
                .Property(s => s.LastName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Staff>()
                .Property(s => s.Email)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Staff>()
                .Property(s => s.Phone)
                .IsRequired()
                .HasMaxLength(20);

            #endregion

            #endregion

            // Lägg till unika constraints. visa property i modell måste vara unikt
            #region IsUniqueName

            #region Production

            modelBuilder.Entity<Brand>()
                .HasIndex(b => b.Name)
                .IsUnique();

            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique();

            modelBuilder.Entity<Color>()
                .HasIndex(c => c.Name)
                .IsUnique();

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Name)
                .IsUnique();

            modelBuilder.Entity<Size>()
                .HasIndex(s => s.Name)
                .IsUnique();

            #endregion

            #region Sales

            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.Email)
                .IsUnique();

            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.Phone)
                .IsUnique();

            modelBuilder.Entity<Staff>()
                .HasIndex(s => s.Email)
                .IsUnique();

            modelBuilder.Entity<Staff>()
                .HasIndex(s => s.Phone)
                .IsUnique();

            #endregion

            #endregion

            // Lägg till index på främmande nycklar för bättre prestanda
            #region IndexOnForeignKey

            #region Production

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.BrandId);

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.CategoryId);

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.ColorId);

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.SizeId);

            modelBuilder.Entity<Stock>()
                .HasIndex(s => s.ProductId);

            #endregion

            #region Sales

            modelBuilder.Entity<CartItem>()
                .HasIndex(ci => ci.OrderId);

            modelBuilder.Entity<CartItem>()
                .HasIndex(ci => ci.ProductId);

            modelBuilder.Entity<Order>()
                .HasIndex(o => o.CustomerId);

            modelBuilder.Entity<OrderStaffAssignment>()
                .HasIndex(osa => osa.OrderId);

            modelBuilder.Entity<OrderStaffAssignment>()
                .HasIndex(osa => osa.StaffId);

            #endregion

            #endregion

            // SeedData
            #region SeedData

            #region Production

            modelBuilder.Entity<Brand>()
                .HasData(new Brand
                {
                    Id = 1,
                    Name = "Brand 1"
                });

            modelBuilder.Entity<Category>()
                .HasData(new Category
                {
                    Id = 1,
                    Name = "Category 1"
                });

            modelBuilder.Entity<Color>()
                .HasData(new Color
                {
                    Id = 1,
                    Name = "Color 1"
                });

            modelBuilder.Entity<Size>()
                .HasData(new Size
                {
                    Id = 1,
                    Name = "Size 1"
                });

            modelBuilder.Entity<Product>()
                .HasData(new Product
                {
                    Id = 1,
                    Name = "Product 1",
                    BrandId = 1,
                    CategoryId = 1,
                    ColorId = 1,
                    SizeId = 1,
                    Price = 1499.99m
                });

            modelBuilder.Entity<Stock>()
                .HasData(new Stock
                {
                    Id = 1,
                    ProductId = 1,
                    Quantity = 100,
                });

            #endregion

            #region Sales

            modelBuilder.Entity<Customer>()
                .HasData(new Customer
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Phone = "0123456789",
                    Email = "john.doe@example.com",
                    Street = "Main Street 1",
                    City = "Stockholm"
                });

            modelBuilder.Entity<Order>()
                .HasData(new Order
                {
                    Id = 1,
                    CustomerId = 1,
                    OrderStatus = "Pending",
                    OrderDate = DateTime.Now,
                    TotalPrice = 1499.99m
                });

            modelBuilder.Entity<CartItem>()
                .HasData(new CartItem
                {
                    Id = 1,
                    OrderId = 1,
                    ProductId = 1,
                    Quantity = 1,
                });

            modelBuilder.Entity<Staff>()
                .HasData(new Staff
                {
                    Id = 1,
                    FirstName = "Anna",
                    LastName = "Smith",
                    Email = "anna.smith@example.com",
                    Phone = "9876543210"
                });

            modelBuilder.Entity<OrderStaffAssignment>()
                .HasData(new OrderStaffAssignment
                {
                    Id = 1,
                    OrderId = 1,
                    StaffId = 1,
                });

            #endregion

            #endregion

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalPrice)
                .HasColumnType("decimal(18, 2)");
        }
    }
}
