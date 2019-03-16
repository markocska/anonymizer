using Anonymizer.ConfigElements;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anonymizer.Config.ConfigCollections
{
    [ConfigurationCollection(typeof(ConstantColumnConfigElement), AddItemName = "Column", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class ConstantColumnConfigCollection : ConfigurationElementCollection
    {
        public ConstantColumnConfigElement this[int index]
        {
            get { return BaseGet(index) as ConstantColumnConfigElement; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ConstantColumnConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ConstantColumnConfigElement)element).Name;
        }
    }
}
