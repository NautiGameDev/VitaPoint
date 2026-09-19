/* This class cleans up enum strings to be more readable before being sent to the client */

using System.Text.RegularExpressions;

namespace VitaPoint.Server.Helpers
{
    public static class EnumCleaner
    {
        public static string ToFormattedString(this Enum value)
        {
            return Regex.Replace(value.ToString(), "(?<!^)([A-Z])", " $1");
        }
    }
}
