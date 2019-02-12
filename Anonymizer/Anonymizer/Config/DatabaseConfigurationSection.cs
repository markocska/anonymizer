using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anonymizer.Config
{
    public class DatabaseConfigurationSection : ConfigurationSection
    {
        public static string SectionName => "DatabasesSection";

        [ConfigurationProperty("Databases")]
        public DatabaseConfigCollection Databases
        {
            get
            {
                return base["Databases"] as DatabaseConfigCollection;
            }
        }
    }
}
