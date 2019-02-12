using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anonymizer.Config
{
    [ConfigurationCollection(typeof(ColumnConfigElement), AddItemName = "Column", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class ColumnConfigCollection : ConfigurationElementCollection
    {
        public ColumnConfigElement this[int index]
        {
            get { return BaseGet(index) as ColumnConfigElement; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ColumnConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ColumnConfigElement) element).Name;
        }
    }
}
