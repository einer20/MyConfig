using System; 
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MyConfig.Internals;
using MyConfig.Standars;

namespace MyConfig
{

    /// <summary>
    /// Manage the configuration data saved as the git configuration format(user.name=einer).
    /// <para>Created by @einersantanar</para>
    /// </summary>
    public sealed class MyConfigurationManager
    {

        private const string COMMENT = "//";

        /// <summary>
        /// Initialize a new instance of MyConfigurationManager
        /// </summary>
        /// <param name="configFile"></param>
        public MyConfigurationManager(string configFile)
        {
            this.Path = configFile;
        }

        /// <summary>
        /// Get the path of the configuration file
        /// </summary>
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
                    if (FormatsStandars.IsValidCategoryName(rawConfig[i]))
                    {
                        // it is a category name, ignore it
                        continue;
                    }

                    string propertyString = rawConfig[i];
                    bool isComment = FormatsStandars.IsComment(propertyString);
                    bool isEmpty = String.IsNullOrWhiteSpace(propertyString);

                    if (isComment == false && isEmpty == false)
                    {
                        var property = MyConfigParser.ParseProperty(propertyString);

                        if (configuration.GetConfigurations().Any(x => x.Key == property.Key))
                        {
                            var newProperty = configuration.GetConfigurations().First(x => x.Key == property.Key);
                            // update the property
                            rawConfig[i] = "\t" + newProperty.Key + " = " + newProperty.Value;
                        }
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

                foreach (var item in internalModel.GetConfigurations())
                {
                    rawConfig[lastIndexOfArray] += Environment.NewLine + "\t" + item.Key + " = " + item.Value;
                }
            }

            File.WriteAllLines(this.Path, rawConfig);
        }

        /// <summary>
        /// Set a configuration by passing the configuration string
        /// </summary>
        /// <param name="configurationCommand"></param>
        public void SetConfiguration(string configurationCommand)
        {
            var config = MyConfigParser.ParseConfiguration(configurationCommand);

            foreach (var item in config)
            {
                SetConfiguration(item);
            }
        }

        /// <summary>
        /// Get a configuration by its category
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public IConfiguration Find(string categoryName)
        {
            return this.GetConfigurationByCategory(categoryName);
        }

        /// <summary>
        /// Get all configurations
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IConfiguration> GetConfigurations()
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

        internal IConfiguration GetConfigurationByCategory(string categoryName)
        {

            if (!FormatsStandars.IsValidCategoryName(categoryName))
            {
                categoryName = categoryName.Replace("[", string.Empty).Replace("]", string.Empty);
                categoryName = String.Format("[{0}]", categoryName);
            }

            Configuration conf = new Configuration(categoryName);

            var data = ReadFile();
            for (int i = 0, total = data.Length; i < total; i++)
            {
                if (data[i] == categoryName)
                {
                    
                    // reading the properties values
                    for (int y = i + 1; y < total; y++)
                    {
                        //|| String.IsNullOrEmpty(data[y]) ||
                        var trimed = data[y].Trim();
                        if (y >= data.Length || FormatsStandars.IsValidCategoryName(data[y])) 
                        {
                            break;
                        }
                        else if (trimed.StartsWith(COMMENT) || String.IsNullOrEmpty(trimed))
                        {
                            continue;
                        }
                        else{
                            var property = MyConfigParser.ParseProperty(data[y]);
                            conf.Add(property.Key, property.Value); 
                        }

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
                if (FormatsStandars.IsValidCategoryName(item.Trim()))
                {
                    yield return item;
                }
            }
        }
    }
}
