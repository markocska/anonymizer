using Anonymizer.ConfigCollections;
using System.Configuration;
using System.Data.SqlClient;

namespace Anonymizer.ConfigElements
{
    public class DatabaseConfigElement : ConfigurationElement
    {

        [ConfigurationProperty("connectionString", IsRequired = true, DefaultValue ="unknown", Options = ConfigurationPropertyOptions.IsRequired)]
        [StringValidator(MinLength = 1)]
        public string ConnectionString
        {
            get { return base["connectionString"] as string; }
        }

        [ConfigurationProperty("Tables")]
        public TableConfigCollection Tables
        {
            get { return base["Tables"] as TableConfigCollection; }
        }
    }
}
