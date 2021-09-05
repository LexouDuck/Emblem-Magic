using System.Text.RegularExpressions;

namespace Nintenlord.Utility.Strings
{
    static class RegexHelper
    {
        public static string[] Substrings(this Group group)
        {
            return Substrings(group.Captures);
        }

        public static string[] Substrings(this CaptureCollection collection)
        {
            string[] result = new string[collection.Count];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = collection[i].Value;
            }
            return result;
        }

    }
}
