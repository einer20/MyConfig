using System; 
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MyConfig.Internals;
using MyConfig.Standars;

namespace MyConfig
{
    public sealed class MyConfigurationManager
    {
        public MyConfigurationManager(string configFile)
        {
            this.Path = configFile;
        }

        public readonly string Path;


        /// <summary>
        /// Set a configuration to the pointer file
        /// </summary>
        /// <param name="configuration"></param>
        public void SetConfiguration(IConfiguration configuration)
        {

            var rawConfig = ReadFile().ToList();

            var internalModel = new InternalConfiguration(configuration);

            var categoryIndex = rawConfig.IndexOf(internalModel.ConfigurationNameStandard);

            if (categoryIndex != -1)
            {

                for (int i = categoryIndex + 1; i < rawConfig.Count; i++)
                {
                    if (CategoryStandars.IsValidCategoryName(rawConfig[i]))
                    {
                        // it is a category name, ignore it
                        continue;
                    }

                    string propertyString = rawConfig[i];

                    var property = Parser.ParseProperty(propertyString);

                    if (configuration.GetConfigurationValues().Any(x => x.Key == property.Key))
                    {
                        var newProperty = configuration.GetConfigurationValues().First(x => x.Key == property.Key);
                        // update the property
                        rawConfig[i] = "\t" + newProperty.Key + "=" + newProperty.Value;
                    }
                }
            }
            else
            {
                int lastIndexOfArray = rawConfig.Count == 0 ? 0 : rawConfig.Count - 1;

                if (lastIndexOfArray == 0){
                    rawConfig.Add(internalModel.ConfigurationNameStandard);
                }
                else{
                    rawConfig[lastIndexOfArray] += Environment.NewLine + internalModel.ConfigurationNameStandard;
                }

                foreach (var item in internalModel.GetConfigurationValues())
                {
                    rawConfig[lastIndexOfArray] += Environment.NewLine + "\t" + item.Key + "=" + item.Value;
                }
            }

            File.WriteAllLines(this.Path, rawConfig);
        }

        public void SetConfiguration(string configurationCommand)
        {
            var config = Parser.ParseConfiguration(configurationCommand);
            //SetConfiguration(config);
        }

        /// <summary>
        /// Get a configuration by its category
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public Configuration Find(string categoryName)
        {
            return this.GetConfigurationByCategory(categoryName);
        }

        /// <summary>
        /// Get all configurations
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Configuration> GetConfigurations()
        {
            foreach (var item in this.GetCategoriesNames())
            {
                yield return GetConfigurationByCategory(item);
            }
        }

        internal string[] ReadFile()
        {
            return File.ReadAllLines(this.Path);
        }

        internal Configuration GetConfigurationByCategory(string categoryName)
        {

            if (!CategoryStandars.IsValidCategoryName(categoryName))
            {
                return null;
            }

            Configuration conf = new Configuration(categoryName);

            var data = ReadFile();
            for (int i = 0, total = data.Length; i < total; i++)
            {
                if (data[i] == categoryName)
                {
                    
                    conf.Index = i;

                    // reading the properties values
                    for (int y = i + 1; y < total; y++)
                    {
                        if (y >= data.Length || CategoryStandars.IsValidCategoryName(data[y])) 
                        {
                            break;
                        }

                        var property = Parser.ParseProperty(data[y]);
                        conf.Add(property.Key, property.Value);
                    }
                }
            }

            return conf;
        }

        internal IEnumerable<string> GetCategoriesNames()
        {
            var data = ReadFile();

            foreach (var item in data)
            {
                if (CategoryStandars.IsValidCategoryName(item.Trim()))
                {
                    yield return item;
                }
            }
        }
    }
}
