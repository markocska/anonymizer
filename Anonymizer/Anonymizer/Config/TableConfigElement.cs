using System.Configuration;

namespace Anonymizer.Config
{
    public class TableConfigElement : ConfigurationElement
    {
        [ConfigurationProperty("nameWithSchema", IsRequired = true)]
        public string NameWithSchema
        {
            get
            {
                return base["nameWithSchema"] as string;
            }
        }

        [ConfigurationProperty("index", IsRequired = true)]
        public string Index
        {
            get
            {
                return base["index"] as string;
            }
        }

        [ConfigurationProperty("Columns")]
        public ColumnConfigCollection Columns
        {
            get
            {
                return base["Columns"] as ColumnConfigCollection;
            }
        }
    }
}
