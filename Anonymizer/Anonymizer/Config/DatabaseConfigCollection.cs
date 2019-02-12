using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anonymizer.Config
{   
    [ConfigurationCollection(typeof(DatabaseConfigElement),AddItemName = "Database", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class DatabaseConfigCollection : ConfigurationElementCollection
    {
        public DatabaseConfigElement this[int index]
        {
            get
            {
                return BaseGet(index) as DatabaseConfigElement;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new DatabaseConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DatabaseConfigElement)element).connectionString;
        }
    }
}
