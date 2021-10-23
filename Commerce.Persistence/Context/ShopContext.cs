using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Commerce.Domain;
using Commerce.Domain.Identity;

namespace Commerce.Persistence.Context
{
    public class ShopContext : IdentityDbContext<User, Role, int, 
        IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>, 
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public ShopContext(DbContextOptions<ShopContext> options) : base(options){}
        public DbSet<ShoppingSession> ShoppingSessions { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Shop> Shops { get; set; }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Address { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>(userRole =>
                {
                    userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                    userRole.HasOne(ur => ur.Role)
                        .WithMany(r => r.UserRoles)
                        .HasForeignKey(ur => ur.RoleId)
                        .IsRequired();

                    userRole.HasOne(ur => ur.User)
                        .WithMany(r => r.UserRoles)
                        .HasForeignKey(ur => ur.UserId)
                        .IsRequired();
                }
            );

            modelBuilder.Entity<Category>()
                .HasMany(p => p.Products)
                .WithOne(c => c.Category);

            modelBuilder.Entity<Shop>()
                .HasMany(p => p.Products)
                .WithOne(c => c.Shop)
                .IsRequired();

            modelBuilder.Entity<Shop>()
                .HasMany(p => p.Categories)
                .WithOne(c => c.Shop)
                .IsRequired();

            modelBuilder.Entity<CartItem>()
                .HasKey(CP => new { CP.ShoppingSessionId, CP.ProductId });

            modelBuilder.Entity<OrderItem>()
                .HasKey(CP => new { CP.OrderId, CP.ProductId });


            InitilizeDB(modelBuilder);
        }

        public void InitilizeDB(ModelBuilder modelBuilder)
        {
            var hasher = new PasswordHasher<User>();

            Shop[] shops = new Shop[]
            {
                new Shop { Id = 1, Name = "Admin", Cnpj = "11111111111111", Desc ="Loja da adminstração", Email = "admin@admin.com" },
                new Shop { Id = 2, Name = "Shop2", Cnpj = "22222222222222", Desc = "Shop2", Email = "shop2@shop2.com" },
                new Shop { Id = 3, Name = "Shop3", Cnpj = "33333333333333", Desc = "Shop3", Email = "shop3@shop3.com" }
            };
            foreach (var loja in shops)
                modelBuilder.Entity<Shop>().HasData(loja);


            Role[] roles = new Role[]
            {
                new Role{Id = 1, Name = "admin", NormalizedName = "admin".ToUpper() },
                new Role{Id = 2, Name = "loja", NormalizedName = "loja".ToUpper() },
                new Role{Id = 3, Name = "cliente", NormalizedName = "cliente".ToUpper() },
            };
            foreach (var role in roles)
                modelBuilder.Entity<Role>().HasData(role);

            User[] users = new User[]
            {
                new User{Id = 1, UserName = shops[0].Name, NormalizedUserName = shops[0].Name.ToUpper(), Email = shops[0].Email, NormalizedEmail = shops[0].Email.ToUpper(), PasswordHash = hasher.HashPassword(null, "1234"), ShopId = shops[0].Id },
                new User{Id = 2, UserName = shops[1].Name, NormalizedUserName = shops[1].Name.ToUpper(), Email = shops[1].Email, NormalizedEmail = shops[1].Email.ToUpper(), PasswordHash = hasher.HashPassword(null, "1234"), ShopId = shops[1].Id },
                new User{Id = 3, UserName = shops[2].Name, NormalizedUserName = shops[2].Name.ToUpper(), Email = shops[2].Email, NormalizedEmail = shops[2].Email.ToUpper(), PasswordHash = hasher.HashPassword(null, "1234"), ShopId = shops[2].Id },
            };
            foreach (var usuario in users)
                modelBuilder.Entity<User>().HasData(usuario);

            UserRole[] userRoles = new UserRole[]
            {
                new UserRole { UserId = 1, RoleId = 1 },
                new UserRole { UserId = 2, RoleId = 2 },
                new UserRole { UserId = 3, RoleId = 2 },
            };
            foreach (var userRole in userRoles)
                modelBuilder.Entity<UserRole>().HasData(userRole);
        }
    }
}