using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MyConfig.Standars
{

    /// <summary>
    /// Configuration command parser utils
    /// </summary>
    public sealed class Parser
    {

        internal static readonly Regex CommandExpression = new Regex(@"[a-z]+[.][a-z0-9_]+\s*[=]\s*[a-z0-9\/.:_]+", RegexOptions.IgnoreCase);

        /// <summary>
        /// Parse a configuration command user.name='hola' user.password=what
        /// </summary>
        /// <param name="configCommand"></param>
        /// <returns></returns>
        public static IEnumerable<IConfiguration> ParseConfiguration(string configCommand)
        {

            Dictionary<string, IConfiguration> result = new Dictionary<string, IConfiguration>();
            MatchCollection matches = CommandExpression.Matches(configCommand);

            foreach (Match match in matches)
            {
                string config = match.Value;
                int firstDotIndex = config.IndexOf('.');

                // getting the category name of the command
                string category = config.Substring(0, firstDotIndex);

                //geting the property without the dot(.)
                string property = config.Substring(firstDotIndex + 1, (config.Length - firstDotIndex) - 1);

                var propertyInfo = ParseProperty(property);

                if (result.ContainsKey(category))
                {
                    result[category].Add(propertyInfo.Key, propertyInfo.Value);
                }
                else
                {
                    Configuration c = new Configuration(category);
                    c.Add(propertyInfo.Key, propertyInfo.Value);

                    result.Add(category, c);
                }
            }

            foreach (var item in result)
            {
                yield return item.Value;
            }
        }

        /// <summary>
        /// Parse a property/value
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static KeyValuePair<string, object> ParseProperty(string property)
        {
            var data = property.Split('=');

            if (data.Length != 2)
            {
                throw new FormatException(String.Format("The property '{0}' is in an invalid format.", property)); 
            }

            return new KeyValuePair<string, object>(data[0].Trim(), data[1].Trim());
        }
    }
}
