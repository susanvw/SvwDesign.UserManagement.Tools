using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SvwDesign.UserManagement.Data;
using SvwDesign.UserManagement.Models;
using Xunit;

namespace SvwDesign.UserManagement.Tools.Data
{
    public class IdentityDbContextTest
    {
        private ModelBuilder CreateModelBuilder()
        {
            var options = new DbContextOptionsBuilder<IdentityDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;
            var context = new IdentityDbContext(options);
            var builder = new ModelBuilder();
            context.GetType()
                .GetMethod("OnModelCreating", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(context, new object[] { builder });
            return builder;
        }

        [Fact]
        public void OnModelCreating_ConfiguresUserTableNameAndCultureProperty()
        {
            var builder = CreateModelBuilder();
            var userEntity = builder.Model.FindEntityType(typeof(ApplicationUser));
            Assert.NotNull(userEntity);
            Assert.Equal("Users", userEntity.GetTableName());

            var cultureProp = userEntity.FindProperty(nameof(ApplicationUser.Culture));
            Assert.NotNull(cultureProp);
            Assert.Equal(10, cultureProp.GetMaxLength());
            Assert.False(cultureProp.IsNullable);
        }

        [Fact]
        public void OnModelCreating_ConfiguresRoleTableName()
        {
            var builder = CreateModelBuilder();
            var roleEntity = builder.Model.FindEntityType(typeof(ApplicationRole));
            Assert.NotNull(roleEntity);
            Assert.Equal("Roles", roleEntity.GetTableName());
        }

        // MongoDB-specific: ToCollection is an extension method and not directly testable via EF Core metadata.
        // You may want to test that the method does not throw and is called, but this is not easily verifiable in a unit test.
    }
}