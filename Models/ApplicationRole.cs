using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SvwDesign.UserManagement.Models;

/// <summary>
/// Represents an application-specific role that extends the <see cref="IdentityRole"/> class,
/// allowing for additional properties such as a description.
/// </summary>
/// <param name="roleName">The name of the role.</param>
public class ApplicationRole(string roleName) : IdentityRole(roleName)
{
    /// Gets or sets the description of the application role.
    /// </summary>
    /// <remarks>
    /// The description can be up to 50 characters long.
    /// </remarks>
    [MaxLength(50, ErrorMessage = "Description can only by 50 characters long.", ErrorMessageResourceName = "ApplicationRole")]
    public string? Description { get; set; }

}