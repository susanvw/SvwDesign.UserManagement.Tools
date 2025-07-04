using System.ComponentModel;
using Microsoft.OpenApi.Extensions;
using SvwDesign.UserManagement.Enums;

namespace SvwDesign.UserManagement.Utils
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Retrieves the description associated with a <see cref="CultureType"/> enumeration value.
        /// If the <see cref="DescriptionAttribute"/> is not present, returns "en-UK" as the default value.
        /// </summary>
        /// <param name="type">The <see cref="CultureType"/> enumeration value.</param>
        /// <returns>
        /// The description specified by the <see cref="DescriptionAttribute"/> if present; otherwise, "en-UK".
        /// </returns>
        public static string GetDescription(this CultureType type)
        {
            var description = type.GetAttributeOfType<DescriptionAttribute>();

            if (description == null)
                return "en-UK";

            return description.Description;
        }
    }
}