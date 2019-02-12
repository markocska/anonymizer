using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anonymizer.Config
{
    [ConfigurationCollection(typeof(TableConfigElement),AddItemName = "Table", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class TableConfigCollection : ConfigurationElementCollection
    {
        public TableConfigElement this[int index]
        {
            get
            {
                return (TableConfigElement)BaseGet(index);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new TableConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TableConfigElement)element).NameWithSchema;
        }
    }
}
