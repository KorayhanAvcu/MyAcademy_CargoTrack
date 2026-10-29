using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CargoTrack.Business.Helpers
{
    public static class EnumDisplayExtensions
    {
        public static string GetDisplayName(this Enum value)
        {
            var member = value.GetType().GetMember(value.ToString()).FirstOrDefault();
            return member?.GetCustomAttribute<DisplayAttribute>()?.GetName() ?? value.ToString();
        }
    }
}
