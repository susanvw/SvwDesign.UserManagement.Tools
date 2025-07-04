// Data/IUserStoreFactory.cs

using Microsoft.AspNetCore.Identity;
using SvwDesign.UserManagement.Enums;
using SvwDesign.UserManagement.Models;

namespace SvwDesign.UserManagement.Interfaces;

public interface IUserStoreFactory
{
    /// <summary>
    /// Creates an instance of <see cref="IUserStore{ApplicationUser}"/> based on the specified data source type, name, and connection string.
    /// </summary>
    /// <param name="dataSourceType">The type of data source to use for the user store.</param>
    /// <param name="datasourceName">The name of the data source.</param>
    /// <param name="connectionString">The connection string used to connect to the data source.</param>
    /// <returns>An <see cref="IUserStore{ApplicationUser}"/> implementation for the specified data source.</returns>
    IUserStore<ApplicationUser> CreateUserStore(DataSourceType dataSourceType, string datasourceName, string connectionString);
}
