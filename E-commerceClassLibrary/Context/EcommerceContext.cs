using E_commerceClassLibrary.Models.Production;
using E_commerceClassLibrary.Models.Sales;
using Microsoft.EntityFrameworkCore;



namespace E_commerceClassLibrary.Context
{
    public class EcommerceContext : DbContext
    {
        public EcommerceContext()
        {

        }
        public EcommerceContext(DbContextOptions<EcommerceContext> options) : base(options)
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
                .HasIndex(p => new { p.Name, p.BrandId, p.ColorId, p.SizeId })
                .IsUnique();

            modelBuilder.Entity<Size>()
                .HasIndex(s => s.Name)
                .IsUnique();

            modelBuilder.Entity<Stock>()
                .HasIndex(s => s.ProductId)
                .IsUnique();

            #endregion

            #region Sales

            modelBuilder.Entity<CartItem>()
                .HasIndex(ci => new { ci.OrderId, ci.ProductId })
                .IsUnique();

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
                .HasData(
                new Brand
                {
                    Id = 1,
                    Name = "Line Art"
                },
                new Brand
                {
                    Id = 2,
                    Name = "Dalle"
                }
                );

            modelBuilder.Entity<Category>()
                .HasData(
                new Category
                {
                    Id = 1,
                    Name = "T-Shirt"
                },
                new Category
                {
                    Id = 2,
                    Name = "Pants"
                },
                new Category
                {
                    Id = 3,
                    Name = "Jacket"
                },
                new Category
                {
                    Id = 4,
                    Name = "Hoodie"
                },
                new Category
                {
                    Id = 5,
                    Name = "Underwear"
                }
                );

            modelBuilder.Entity<Color>()
                .HasData(
                new Color
                {
                    Id = 1,
                    Name = "Red"
                },
                new Color
                {
                    Id = 2,
                    Name = "Green"
                },
                new Color
                {
                    Id = 3,
                    Name = "Blue"
                },
                new Color
                {
                    Id = 4,
                    Name = "White"
                }
                );

            modelBuilder.Entity<Size>()
                .HasData(
                new Size
                {
                    Id = 1,
                    Name = "S"
                },
                new Size
                {
                    Id = 2,
                    Name = "M"
                },
                new Size
                {
                    Id = 3,
                    Name = "L"
                },
                new Size
                {
                    Id = 4,
                    Name = "XL"
                }
                );

            modelBuilder.Entity<Product>()
                .HasData(
                new Product
                {
                    Id = 1,
                    Name = "Line Art Shirt",
                    BrandId = 1,
                    CategoryId = 1,
                    ColorId = 1,
                    SizeId = 1,
                    Price = 1499.99m,
                    ImagePath = "images/ShirtRed.png"
                },
                new Product
                {
                    Id = 2,
                    Name = "Line Art Shirt",
                    BrandId = 1,
                    CategoryId = 1,
                    ColorId = 2,
                    SizeId = 1,
                    Price = 1499.99m,
                    ImagePath = "images/ShirtGreen.png"
                },
                new Product
                {
                    Id = 3,
                    Name = "Line Art Shirt",
                    BrandId = 1,
                    CategoryId = 1,
                    ColorId = 3,
                    SizeId = 1,
                    Price = 1499.99m,
                    ImagePath = "images/ShirtBlue.png"
                },
                new Product
                {
                    Id = 4,
                    Name = "Line Art Pants",
                    BrandId = 1,
                    CategoryId = 2,
                    ColorId = 1,
                    SizeId = 1,
                    Price = 1099.99m,
                    ImagePath = "images/PantsRed.png"
                },
                new Product
                {
                    Id = 5,
                    Name = "Line Art Pants",
                    BrandId = 1,
                    CategoryId = 2,
                    ColorId = 2,
                    SizeId = 1,
                    Price = 1099.99m,
                    ImagePath = "images/PantsGreen.png"
                },
                new Product
                {
                    Id = 6,
                    Name = "Line Art Pants",
                    BrandId = 1,
                    CategoryId = 2,
                    ColorId = 3,
                    SizeId = 1,
                    Price = 1099.99m,
                    ImagePath = "images/PantsBlue.png"
                },
                new Product
                {
                    Id = 7,
                    Name = "Line Art Jacket",
                    BrandId = 1,
                    CategoryId = 3,
                    ColorId = 1,
                    SizeId = 1,
                    Price = 2499.99m,
                    ImagePath = "images/JacketRed.png"
                },
                new Product
                {
                    Id = 8,
                    Name = "Line Art Jacket",
                    BrandId = 1,
                    CategoryId = 3,
                    ColorId = 2,
                    SizeId = 1,
                    Price = 2499.99m,
                    ImagePath = "images/JacketGreen.png"
                },
                new Product
                {
                    Id = 9,
                    Name = "Line Art Jacket",
                    BrandId = 1,
                    CategoryId = 3,
                    ColorId = 3,
                    SizeId = 1,
                    Price = 2499.99m,
                    ImagePath = "images/JacketBlue.png"
                },
                new Product
                {
                    Id = 10,
                    Name = "Line Art Hoodie",
                    BrandId = 1,
                    CategoryId = 4,
                    ColorId = 1,
                    SizeId = 1,
                    Price = 1899.99m,
                    ImagePath = "images/HoodieRed.png"
                },
                new Product
                {
                    Id = 11,
                    Name = "Line Art Hoodie",
                    BrandId = 1,
                    CategoryId = 4,
                    ColorId = 2,
                    SizeId = 1,
                    Price = 1899.99m,
                    ImagePath = "images/HoodieGreen.png"
                },
                new Product
                {
                    Id = 12,
                    Name = "Line Art Hoodie",
                    BrandId = 1,
                    CategoryId = 4,
                    ColorId = 3,
                    SizeId = 1,
                    Price = 1899.99m,
                    ImagePath = "images/HoodieBlue.png"
                },
                new Product
                {
                    Id = 13,
                    Name = "Dalle Hoodie",
                    BrandId = 2,
                    CategoryId = 4,
                    ColorId = 4,
                    SizeId = 1,
                    Price = 777m,
                    ImagePath = "images/DallHoodie.webp"
                },
                new Product
                {
                    Id = 14,
                    Name = "Dalle Hoodie",
                    BrandId = 2,
                    CategoryId = 4,
                    ColorId = 4,
                    SizeId = 2,
                    Price = 777m,
                    ImagePath = "images/DallHoodie.webp"
                },
                new Product
                {
                    Id = 15,
                    Name = "Dalle Hoodie",
                    BrandId = 2,
                    CategoryId = 4,
                    ColorId = 4,
                    SizeId = 3,
                    Price = 777m,
                    ImagePath = "images/DallHoodie.webp"
                },
                new Product
                {
                    Id = 16,
                    Name = "Dalle Hoodie",
                    BrandId = 2,
                    CategoryId = 4,
                    ColorId = 4,
                    SizeId = 4,
                    Price = 777m,
                    ImagePath = "images/DallHoodie.webp"
                },
                new Product
                {
                    Id = 17,
                    Name = "Dalle Jacket",
                    BrandId = 2,
                    CategoryId = 3,
                    ColorId = 4,
                    SizeId = 3,
                    Price = 777m,
                    ImagePath = "images/DallJacket.webp"
                },
                new Product
                {
                    Id = 18,
                    Name = "Dalle Pants",
                    BrandId = 2,
                    CategoryId = 2,
                    ColorId = 4,
                    SizeId = 2,
                    Price = 777m,
                    ImagePath = "images/DallPants.webp"
                },
                new Product
                {
                    Id = 19,
                    Name = "Dalle Shirt",
                    BrandId = 2,
                    CategoryId = 1,
                    ColorId = 4,
                    SizeId = 2,
                    Price = 777m,
                    ImagePath = "images/DallShirt.webp"
                },
                new Product
                {
                    Id = 20,
                    Name = "Fun Time",
                    BrandId = 2,
                    CategoryId = 5,
                    ColorId = 4,
                    SizeId = 2,
                    Price = 999m,
                    ImagePath = "images/pngwing.png"
                }, new Product
                {
                    Id = 21,
                    Name = "Line Art Shirt",
                    BrandId = 1,
                    CategoryId = 1,
                    ColorId = 1,
                    SizeId = 2,
                    Price = 1499.99m,
                    ImagePath = "images/ShirtRed.png"
                },
                new Product
                {
                    Id = 22,
                    Name = "Line Art Shirt",
                    BrandId = 1,
                    CategoryId = 1,
                    ColorId = 2,
                    SizeId = 2,
                    Price = 1499.99m,
                    ImagePath = "images/ShirtGreen.png"
                },
                new Product
                {
                    Id = 23,
                    Name = "Line Art Shirt",
                    BrandId = 1,
                    CategoryId = 1,
                    ColorId = 3,
                    SizeId = 2,
                    Price = 1499.99m,
                    ImagePath = "images/ShirtBlue.png"
                },
                new Product
                {
                    Id = 24,
                    Name = "Line Art Pants",
                    BrandId = 1,
                    CategoryId = 2,
                    ColorId = 1,
                    SizeId = 2,
                    Price = 1099.99m,
                    ImagePath = "images/PantsRed.png"
                }
                );

            modelBuilder.Entity<Stock>()
                .HasData(
                new Stock
                {
                    Id = 1,
                    ProductId = 1,
                    Quantity = 100,
                },
                new Stock
                {
                    Id = 2,
                    ProductId = 2,
                    Quantity = 100,
                }, new Stock
                {
                    Id = 3,
                    ProductId = 3,
                    Quantity = 100,
                }, new Stock
                {
                    Id = 4,
                    ProductId = 4,
                    Quantity = 100,
                }, new Stock
                {
                    Id = 5,
                    ProductId = 5,
                    Quantity = 100,
                }, new Stock
                {
                    Id = 6,
                    ProductId = 6,
                    Quantity = 100,
                }, new Stock
                {
                    Id = 7,
                    ProductId = 7,
                    Quantity = 100,
                }, new Stock
                {
                    Id = 8,
                    ProductId = 8,
                    Quantity = 100,
                },
                new Stock
                {
                    Id = 9,
                    ProductId = 9,
                    Quantity = 100,
                },
                new Stock
                {
                    Id = 10,
                    ProductId = 10,
                    Quantity = 100,
                },
                new Stock
                {
                    Id = 11,
                    ProductId = 11,
                    Quantity = 100,
                },
                new Stock
                {
                    Id = 12,
                    ProductId = 12,
                    Quantity = 100,
                },
                new Stock
                {
                    Id = 13,
                    ProductId = 13,
                    Quantity = 100,
                },
                new Stock
                {
                    Id = 14,
                    ProductId = 14,
                    Quantity = 100,
                },
                new Stock
                {
                    Id = 15,
                    ProductId = 15,
                    Quantity = 100,
                },
                new Stock
                {
                    Id = 16,
                    ProductId = 16,
                    Quantity = 100,
                },
                new Stock
                {
                    Id = 17,
                    ProductId = 17,
                    Quantity = 100,
                },
                new Stock
                {
                    Id = 18,
                    ProductId = 18,
                    Quantity = 100,
                },
                new Stock
                {
                    Id = 19,
                    ProductId = 19,
                    Quantity = 100,
                },
                new Stock
                {
                    Id = 20,
                    ProductId = 20,
                    Quantity = 100,
                }
                );

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
