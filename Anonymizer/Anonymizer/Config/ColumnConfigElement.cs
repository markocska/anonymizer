using System.Configuration;

namespace Anonymizer.Config
{
    public class ColumnConfigElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return base["name"] as string; }
        }
    }
}
