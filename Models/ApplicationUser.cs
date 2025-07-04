
using Microsoft.AspNetCore.Identity;
using SvwDesign.UserManagement.Enums;
using SvwDesign.UserManagement.Utils;
using System.ComponentModel.DataAnnotations;

namespace SvwDesign.UserManagement.Models;

public class ApplicationUser : IdentityUser
{
    [Required]
    [StringLength(10)]
    public string Culture { get; set; } = CultureType.UK.GetDescription();

    public ApplicationUser(string email, string? cultureType = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        UserName = email;

        if (cultureType != null) Culture = cultureType;
    }
}