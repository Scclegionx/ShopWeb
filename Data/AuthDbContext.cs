using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopWeb.Models.Domain;

namespace ShopWeb.Data
{
    public class AuthDbContext : IdentityDbContext<ApplicationUser>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {

        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.Address)
                .IsRequired()
                .HasMaxLength(255);
                entity.Property(e => e.Avatar)
                .IsRequired();
            });

            var userRoleId = "7b77a51f-5bff-479a-af59-27d84c9257a9";
            var adminRoleId = "51ccd6c8-6c80-4852-bf8e-d4dc98516dc1";
            var superAdminRoleId = "d21169e5-d672-4720-845d-14dd6278c740";
            var shipperRoleId = "a37d654b-9771-4d90-b9d4-e6b88509c394";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "User",
                    Id = userRoleId,
                    ConcurrencyStamp = userRoleId
                },
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "Admin",
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId,
                },
                new IdentityRole
                {
                    Name = "SuperAdmin",
                    NormalizedName = "SuperAdmin",
                    Id = superAdminRoleId,
                    ConcurrencyStamp = superAdminRoleId,
                },
                new IdentityRole
                {
                    Name = "Shipper",
                    NormalizedName = "Shipper",
                    Id = shipperRoleId,
                    ConcurrencyStamp = shipperRoleId,
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
            var superAdminId = "b1932f74-fcf6-4c60-83a0-236c9cd76e13";
            var superAdminUser = new ApplicationUser
            {
                UserName = "superadmin",
                Email = "superadmin@gmail.com",
                NormalizedEmail = "superadmin@gmail.com".ToUpper(),
                NormalizedUserName = "superadmin".ToUpper(),
                Id = superAdminId,
                Address = "123 Main Street",
                Avatar = "test"
            };

            superAdminUser.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(superAdminUser, "Superadmin@123");

            builder.Entity<ApplicationUser>().HasData(superAdminUser);

            var superAdminRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId = adminRoleId,
                    UserId = superAdminId,
                },
                new IdentityUserRole<string>
                {
                    RoleId = userRoleId,
                    UserId = superAdminId,
                },
                new IdentityUserRole<string>
                {
                    RoleId = superAdminRoleId,
                    UserId = superAdminId,
                }
            };
            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);

        }
    }
}
