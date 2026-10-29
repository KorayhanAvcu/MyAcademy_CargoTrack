using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CargoTrack.WebUI.Extensions
{
    public static class EnumExtensions
    {
        public static string ToDisplayName(this Enum value)
        {
            var member = value.GetType().GetMember(value.ToString()).FirstOrDefault();
            var attribute = member?.GetCustomAttribute<DisplayAttribute>();
            return attribute?.GetName() ?? value.ToString();
        }
    }
}
