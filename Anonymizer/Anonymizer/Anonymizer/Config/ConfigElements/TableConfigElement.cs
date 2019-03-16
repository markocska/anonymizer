using Anonymizer.Config.ConfigCollections;
using Anonymizer.ConfigCollections;
using System;
using System.Configuration;

namespace Anonymizer.ConfigElements
{
    public class TableConfigElement : ConfigurationElement
    {

        [ConfigurationProperty("nameWithSchema", IsRequired = true, DefaultValue = "unknown", Options = ConfigurationPropertyOptions.IsRequired)]
        [StringValidator(MinLength = 1)]
        public string NameWithSchema
        {
            get
            {
                return base["nameWithSchema"] as string;
            }
        }

        [ConfigurationProperty("ScrambledColumns")]
        public ScrambledColumnConfigCollection ScrambledColumns
        {
            get
            {
                return base["ScrambledColumns"] as ScrambledColumnConfigCollection;
            }
        }

        [ConfigurationProperty("ConstantColumns")]
        public ConstantColumnConfigCollection ConstantColumns
        {
            get
            {
                return base["ConstantColumns"] as ConstantColumnConfigCollection;
            }
        }

    }
}
