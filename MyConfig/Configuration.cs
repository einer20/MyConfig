using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using MyConfig.Standars;

namespace MyConfig
{
    [DebuggerDisplay("{ConfigurationName} with {ConfigurationCount} {ConfigurationQuantities}")]
    public sealed class Configuration : IConfiguration
    {

        private string name;

        private Dictionary<string, object> values;

        public Configuration(string name)
        {
            this.name = name;
            this.values = new Dictionary<string, object>();
        }

        public Configuration(IConfiguration config)
        {
            this.name = config.ConfigurationName;
            this.values = this.GetConfigurationValues();
        }

        public string ConfigurationName
        {
            get { return this.name; }
        }

        private int ConfigurationCount { get { return this.values.Count(); } }

        private string ConfigurationQuantities { get { return this.values.Count() <= 1 ? " configuration" : "configurations"; } }

        /// <summary>
        /// Get the config index in file
        /// </summary>
        internal int Index { get; set; }

        public Dictionary<string, object> GetConfigurationValues()
        {
            return this.values;
        }

        public void Add(string propertyName, object propertyValue)
        {
            this.values.Add(propertyName, propertyValue);
        }

    }
}
