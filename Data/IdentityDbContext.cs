// Data/IdentityDbContext.cs (ensure this exists for clarity)

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;
using SvwDesign.UserManagement.Models;

namespace SvwDesign.UserManagement.Data;

/// <summary>
/// Represents the Entity Framework database context for identity management,
/// inheriting from <see cref="IdentityDbContext{TUser, TRole, TKey}"/> with custom configuration
/// for <see cref="ApplicationUser"/> and <see cref="ApplicationRole"/> entities.
/// </summary>
/// <param name="options">
/// The options to be used by the <see cref="DbContext"/>.
/// </param>
public class IdentityDbContext(DbContextOptions<IdentityDbContext> options)
    : IdentityDbContext<ApplicationUser, ApplicationRole, string>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Customize table/collection names
        builder.Entity<ApplicationUser>().ToTable("Users").Property(u => u.Culture).HasMaxLength(10).IsRequired();
        builder.Entity<ApplicationRole>().ToTable("Roles");

        // MongoDB-specific configuration
        builder.Entity<ApplicationUser>().ToCollection("Users");
        builder.Entity<ApplicationRole>().ToCollection("Roles");
    }
}