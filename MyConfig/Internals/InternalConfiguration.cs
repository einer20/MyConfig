using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyConfig.Internals
{
    internal class InternalConfiguration : IConfiguration, IEquatable<string>
    {

        private string name;
        private Dictionary<string, object> values = new Dictionary<string, object>();
        public InternalConfiguration( IConfiguration config )
        {
            this.name = config.ConfigurationName;
            this.values = config.GetConfigurations();
        }

        public string ConfigurationName
        {
            get { return this.name; }
        }


        public string ConfigurationNameStandard { get { return String.Format("[{0}]", this.ConfigurationName); } }

        public Dictionary<string, object> GetConfigurations()
        {
            return this.values;
        }

        public void Add(string propertyName, object propertyValue)
        {
            this.values.Add(propertyName, propertyValue);
        }

        public bool Equals(string categoryName)
        {
            return this.ConfigurationName == categoryName || this.ConfigurationNameStandard == categoryName;
        }


        public bool HasSettings
        {
            get { return this.values.Any(); }
        }
    }
}
