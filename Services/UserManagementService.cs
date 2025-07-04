// Services/UserManagementService.cs
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SvwDesign.UserManagement.Models;

namespace SvwDesign.UserManagement.Services;

public class UserManagementService(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    string? jwtIssuer = null,
    string? jwtAudience = null,
    string? jwtSecretKey = null)
{
    /// <summary>
    /// Registers a new user with the specified email, password, and culture.
    /// </summary>
    /// <param name="email">The email address of the user to register.</param>
    /// <param name="password">The password for the new user.</param>
    /// <param name="culture">The culture information for the user.</param>
    /// <returns>
    /// A tuple containing the <see cref="IdentityResult"/> of the registration operation and a JWT token string if registration succeeded; otherwise, the token is <c>null</c>.
    /// </returns>
    public async Task<(IdentityResult Result, string? Token)> RegisterUserAsync(string email, string password, string culture)
    {
        var user = new ApplicationUser(email, culture);

        var result = await userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
            await signInManager.SignInAsync(user, isPersistent: false);
            var token = await GenerateJwtTokenAsync(user);
            return (result, token);
        }
        return (result, null);
    }

    public async Task<(SignInResult Result, string? Token)> LoginUserAsync(string email, string password, bool isPersistent)
    {
        var result = await signInManager.PasswordSignInAsync(email, password, isPersistent, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            var user = await userManager.FindByEmailAsync(email);
            var token = await GenerateJwtTokenAsync(user);
            return (result, token);
        }
        return (result, null);
    }

    public async Task SignOutAsync()
    {
        await signInManager.SignOutAsync();
    }

    public async Task<IdentityResult> AddToRoleAsync(string email, string role)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return IdentityResult.Failed(new IdentityError { Description = "User not found" });
        }
        return await userManager.AddToRoleAsync(user, role);
    }

    public async Task<bool> IsInRoleAsync(string email, string role)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return false;
        }
        return await userManager.IsInRoleAsync(user, role);
    }

    public async Task<IdentityResult> UpdateCultureAsync(string email, string culture)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return IdentityResult.Failed(new IdentityError { Description = "User not found" });
        }
        user.Culture = culture;
        return await userManager.UpdateAsync(user);
    }

    public async Task<ExternalLoginInfo> GetExternalLoginInfoAsync()
    {
        return await signInManager.GetExternalLoginInfoAsync();
    }

    public async Task<(SignInResult Result, string? Token)> ExternalLoginSignInAsync(string provider, string providerKey, bool isPersistent)
    {
        var loginInfo = await signInManager.GetExternalLoginInfoAsync();
        if (loginInfo == null)
        {
            return (SignInResult.Failed, null);
        }
        var result = await signInManager.ExternalLoginSignInAsync(loginInfo.LoginProvider, loginInfo.ProviderKey, isPersistent);
        if (result.Succeeded)
        {
            var user = await userManager.FindByLoginAsync(loginInfo.LoginProvider, loginInfo.ProviderKey);
            var token = await GenerateJwtTokenAsync(user);
            return (result, token);
        }
        return (result, null);
    }

    public async Task<(IdentityResult Result, string? Token)> CreateExternalUserAsync(ExternalLoginInfo loginInfo, string culture)
    {
        var user = new ApplicationUser
        {
            UserName = loginInfo.Principal.Identity?.Name ?? loginInfo.Principal.FindFirst("email")?.Value,
            Email = loginInfo.Principal.FindFirst("email")?.Value,
            Culture = culture
        };

        var result = await userManager.CreateAsync(user);
        if (result.Succeeded)
        {
            result = await userManager.AddLoginAsync(user, loginInfo);
            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
                var token = await GenerateJWTTokenAsync(user);
                return (result, token);
            }
        }
        return (result, null);
    }

    private async Task<string?> GenerateJwtTokenAsync(ApplicationUser user)
    {
        if (jwtIssuer == null || jwtAudience == null || jwtSecretKey == null)
        {
            return null;
        }

        var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("culture", user.Culture)
            };

        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtIssuer,
            audience: _jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}