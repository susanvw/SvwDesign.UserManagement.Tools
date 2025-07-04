// Configuration/IdentityConfiguration.cs
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SvwDesign.UserManagement.Data;
using SvwDesign.UserManagement.Enums;
using SvwDesign.UserManagement.Interfaces;
using SvwDesign.UserManagement.Models;
using SvwDesign.UserManagement.Services;
using System.Text;

namespace SvwDesign.UserManagement.Configuration
{
    public static class IdentityConfiguration
    {
        public static IServiceCollection AddUserManagementAuth(
            this IServiceCollection services,
            DataSourceType dataSourceType,
            string connectionString,
            bool enableJwtAuth = false,
            string? jwtIssuer = null,
            string? jwtAudience = null,
            string? jwtSecretKey = null,
            bool enableGoogleAuth = false,
            string? googleClientId = null,
            string? googleClientSecret = null,
            bool enableAppleAuth = false,
            string? appleClientId = null,
            string? appleTeamId = null,
            string? appleKeyId = null,
            string? applePrivateKey = null,
            bool enableRoles = true)
        {
            services.AddScoped<IUserStoreFactory, EfCoreUserStoreFactory>();
            services.AddScoped<UserManagementService>();

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
            })
            .AddUserStore(factory => factory.CreateUserStore(dataSourceType, connectionString))
            .AddDefaultTokenProviders();

            if (enableRoles)
            {
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
                });
            }

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = enableJwtAuth ? JwtBearerDefaults.AuthenticationScheme : IdentityConstants.ApplicationScheme;
                options.DefaultChallengeScheme = enableJwtAuth ? JwtBearerDefaults.AuthenticationScheme : IdentityConstants.ApplicationScheme;
            })
            .AddCookie();

            if (enableJwtAuth && !string.IsNullOrEmpty(jwtIssuer) && !string.IsNullOrEmpty(jwtAudience) && !string.IsNullOrEmpty(jwtSecretKey))
            {
                services.AddAuthentication().AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtIssuer,
                        ValidAudience = jwtAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey)),
                        ClockSkew = TimeSpan.Zero
                    };
                });
            }

            if (enableGoogleAuth && !string.IsNullOrEmpty(googleClientId) && !string.IsNullOrEmpty(googleClientSecret))
            {
                services.AddAuthentication().AddGoogle(options =>
                {
                    options.ClientId = googleClientId;
                    options.ClientSecret = googleClientSecret;
                });
            }

            if (enableAppleAuth && !string.IsNullOrEmpty(appleClientId) && !string.IsNullOrEmpty(appleTeamId) &&
                !string.IsNullOrEmpty(appleKeyId) && !string.IsNullOrEmpty(applePrivateKey))
            {
                services.AddAuthentication().AddApple(options =>
                {
                    options.ClientId = appleClientId;
                    options.TeamId = appleTeamId;
                    options.KeyId = appleKeyId;
                    options.PrivateKey = _ => Task.FromResult(applePrivateKey);
                    options.UseTemporaryPrivateKey = true;
                });
            }

            return services;
        }

        public static IApplicationBuilder UseUserManagementAuth(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
            return app;
        }
    }
}