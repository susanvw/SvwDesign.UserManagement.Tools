// Data/EfCoreUserStoreFactory.cs

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SvwDesign.UserManagement.Enums;
using SvwDesign.UserManagement.Interfaces;
using SvwDesign.UserManagement.Models;

namespace SvwDesign.UserManagement.Data;

public class EfCoreUserStoreFactory : IUserStoreFactory
{
    /// <summary>
    /// Creates an <see cref="IUserStore{TUser}"/> instance based on the specified <paramref name="dataSourceType"/> and <paramref name="connectionString"/>.
    /// </summary>
    /// <param name="dataSourceType">
    /// The type of data source to use for the user store (e.g., SqlServer, MongoDB).
    /// </param>
    /// <param name="connectionString">
    /// The connection string used to connect to the specified data source.
    /// </param>
    /// <returns>
    /// An <see cref="IUserStore{TUser}"/> implementation for the specified data source.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when an invalid <paramref name="dataSourceType"/> is provided.
    /// </exception>
    public IUserStore<ApplicationUser> CreateUserStore(DataSourceType dataSourceType, string datasourceName, string connectionString)
    {
        var optionsBuilder = new DbContextOptionsBuilder<IdentityDbContext<ApplicationUser, ApplicationRole, string>>();

        switch (dataSourceType)
        {
            case DataSourceType.SqlServer:
                optionsBuilder.UseSqlServer(connectionString);
                break;
            case DataSourceType.MongoDB:
                optionsBuilder.UseMongoDB(connectionString, datasourceName);
                break;
            default:
                throw new ArgumentException("Invalid data source type", nameof(dataSourceType));
        }

        var context = new IdentityDbContext<ApplicationUser, ApplicationRole, string>(optionsBuilder.Options);
        return new UserStore<ApplicationUser, ApplicationRole, IdentityDbContext<ApplicationUser, ApplicationRole, string>>(context);
    }
}
