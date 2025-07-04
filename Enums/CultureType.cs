using System.ComponentModel;

namespace SvwDesign.UserManagement.Enums;

/// <summary>
/// Specifies the supported culture types for the application.
/// </summary>
public enum CultureType
{
    [Description("en-US")]
    US,

    [Description("en-GB")]
    UK
}
