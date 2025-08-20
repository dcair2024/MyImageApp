using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyImageApp.Models;


namespace MyImageApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ImageModel> Images { get; set; }
        public DbSet<ImageHistory> ImageHistories { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configurações específicas para SQLite - corrige o erro nvarchar(max)
            builder.Entity<IdentityRole>().Property(r => r.ConcurrencyStamp).HasColumnType("TEXT");
            builder.Entity<IdentityRole>().Property(r => r.Id).HasColumnType("TEXT");
            builder.Entity<IdentityRole>().Property(r => r.Name).HasColumnType("TEXT");
            builder.Entity<IdentityRole>().Property(r => r.NormalizedName).HasColumnType("TEXT");

            builder.Entity<ApplicationUser>().Property(u => u.ConcurrencyStamp).HasColumnType("TEXT");
            builder.Entity<ApplicationUser>().Property(u => u.SecurityStamp).HasColumnType("TEXT");
            builder.Entity<ApplicationUser>().Property(u => u.Id).HasColumnType("TEXT");
            builder.Entity<ApplicationUser>().Property(u => u.UserName).HasColumnType("TEXT");
            builder.Entity<ApplicationUser>().Property(u => u.NormalizedUserName).HasColumnType("TEXT");
            builder.Entity<ApplicationUser>().Property(u => u.Email).HasColumnType("TEXT");
            builder.Entity<ApplicationUser>().Property(u => u.NormalizedEmail).HasColumnType("TEXT");
            builder.Entity<ApplicationUser>().Property(u => u.PasswordHash).HasColumnType("TEXT");
            builder.Entity<ApplicationUser>().Property(u => u.PhoneNumber).HasColumnType("TEXT");

            // Outras tabelas do Identity
            builder.Entity<IdentityUserClaim<string>>().Property(c => c.ClaimType).HasColumnType("TEXT");
            builder.Entity<IdentityUserClaim<string>>().Property(c => c.ClaimValue).HasColumnType("TEXT");

            builder.Entity<IdentityUserLogin<string>>().Property(l => l.LoginProvider).HasColumnType("TEXT");
            builder.Entity<IdentityUserLogin<string>>().Property(l => l.ProviderKey).HasColumnType("TEXT");
            builder.Entity<IdentityUserLogin<string>>().Property(l => l.ProviderDisplayName).HasColumnType("TEXT");

            builder.Entity<IdentityUserToken<string>>().Property(t => t.LoginProvider).HasColumnType("TEXT");
            builder.Entity<IdentityUserToken<string>>().Property(t => t.Name).HasColumnType("TEXT");
            builder.Entity<IdentityUserToken<string>>().Property(t => t.Value).HasColumnType("TEXT");

            builder.Entity<IdentityRoleClaim<string>>().Property(c => c.ClaimType).HasColumnType("TEXT");
            builder.Entity<IdentityRoleClaim<string>>().Property(c => c.ClaimValue).HasColumnType("TEXT");
        }
    }
}