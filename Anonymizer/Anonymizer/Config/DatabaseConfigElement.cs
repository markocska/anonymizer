using System.Configuration;

namespace Anonymizer.Config
{
    public class DatabaseConfigElement : ConfigurationElement
    {
        [ConfigurationProperty("connectionString", IsRequired = true)]
        public string connectionString
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
