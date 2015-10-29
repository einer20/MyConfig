using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MyConfig.Standars
{
    public sealed class CategoryStandars
    {


        internal static readonly Regex CATEGORY_NAME_EXPRESSION = new Regex("^\\[\\w+\\]$", RegexOptions.IgnoreCase);

        /// <summary>
        /// Indicates if the given raw configuration is valid tree level
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static bool IsValidDeepTreeLevel(string configuration)
        {

            var empty = Enumerable.Empty<string>().ToArray();
            var result = IsValidDeepTreeLevel(configuration, out empty);
            empty = null;

            return result;
        }

        public static bool IsValidDeepTreeLevel(string command, out string[] data)
        {
            data = command.Split('.');

            if (data.Length < 2)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool IsValidCategoryName(string name)
        {
            bool isValid = CATEGORY_NAME_EXPRESSION.IsMatch(name);
            return isValid;
        }
    }
}
