using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyConfig
{
    /// <summary>
    /// Parse a configuration string command into a IConfiguration
    /// </summary>
    public class ConfigurationCommandParser
    {
        /// <summary>
        /// Initialize a new instance of ConfigurationCommandParser with the given
        /// command configuration
        /// </summary>
        /// <param name="configCommand"></param>
        public ConfigurationCommandParser(string configCommand)
        {
            this.Command = configCommand;
        }

        /// <summary>
        /// Get the configuration command
        /// </summary>
        public readonly string Command;

        /// <summary>
        /// Get the category name of the configuration command
        /// </summary>
        public string CategoryName { get { throw new NotImplementedException(); } }

        /// <summary>
        /// Get the values defined in the string command config
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetValues()
        {

            throw new NotImplementedException();
        }


        public override string ToString()
        {
            return this.Command;
        }

    }

}
