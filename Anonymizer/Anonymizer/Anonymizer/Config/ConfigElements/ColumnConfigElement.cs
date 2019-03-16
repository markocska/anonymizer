using System.Configuration;

namespace Anonymizer.ConfigElements
{
    public abstract class ColumnConfigElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return base["name"] as string; }
        }

    }
}
