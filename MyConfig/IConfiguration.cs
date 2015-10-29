using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyConfig
{

    /// <summary>
    /// Configuration information
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        /// Get the configuration name
        /// </summary>
        string ConfigurationName { get; }

        /// <summary>
        /// Get the configuration values
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> GetConfigurationValues();

        /// <summary>
        /// Add new property/value to the current configuration
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        void Add(string propertyName, object propertyValue);
    }
}
