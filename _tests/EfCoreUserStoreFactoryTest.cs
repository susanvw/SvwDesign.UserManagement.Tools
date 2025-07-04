using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SvwDesign.UserManagement.Data;
using SvwDesign.UserManagement.Enums;
using SvwDesign.UserManagement.Models;
using Xunit;

namespace SvwDesign.UserManagement._tests;

public class EfCoreUserStoreFactoryTest
{
    [Fact]
    public void CreateUserStore_WithSqlServer_ReturnsUserStore()
    {
        // Arrange
        var factory = new EfCoreUserStoreFactory();
        var connectionString = "Server=(localdb)\\mssqllocaldb;Database=TestDb;Trusted_Connection=True;";

        // Act
        var userStore = factory.CreateUserStore(DataSourceType.SqlServer, connectionString);

        // Assert
        Assert.NotNull(userStore);
        Assert.IsType<UserStore<ApplicationUser, ApplicationRole, IdentityDbContext<ApplicationUser, ApplicationRole, string>>>(userStore);
    }

    [Fact]
    public void CreateUserStore_WithMongoDB_ReturnsUserStore()
    {
        // Arrange
        var factory = new EfCoreUserStoreFactory();
        var connectionString = "mongodb://localhost:27017";

        // Act
        var userStore = factory.CreateUserStore(DataSourceType.MongoDB, connectionString);

        // Assert
        Assert.NotNull(userStore);
        Assert.IsType<UserStore<ApplicationUser, ApplicationRole, IdentityDbContext<ApplicationUser, ApplicationRole, string>>>(userStore);
    }

    [Fact]
    public void CreateUserStore_WithInvalidDataSource_ThrowsArgumentException()
    {
        // Arrange
        var factory = new EfCoreUserStoreFactory();
        var connectionString = "dummy";

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() =>
            factory.CreateUserStore((DataSourceType)999, connectionString));
        Assert.Equal("Invalid data source type (Parameter 'dataSourceType')", ex.Message);
    }
}