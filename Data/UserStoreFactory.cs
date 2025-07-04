using Microsoft.AspNetCore.Identity;
using SvwDesign.UserManagement.Models;

namespace SvwDesign.UserManagement.Data;

public abstract class UserStoreFactory
{
    public abstract IUserStore<ApplicationUser> CreateUserStore(string connectionString);
    /// <summary>
    /// Trims leading and trailing whitespace from the specified connection string.
    /// </summary>
    /// <param name="connectionString">The connection string to normalize.</param>
    /// <returns>The trimmed connection string.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="connectionString"/> is <c>null</c>.</exception>
    protected static string NormalizeConnectionString(string connectionString)
    {
        return connectionString?.Trim() ?? throw new ArgumentNullException(nameof(connectionString));
    }
}