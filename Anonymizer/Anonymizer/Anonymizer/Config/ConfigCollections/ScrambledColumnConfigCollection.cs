using Anonymizer.ConfigElements;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anonymizer.ConfigCollections
{
    [ConfigurationCollection(typeof(ScrambledColumnConfigElement), AddItemName = "Column", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class ScrambledColumnConfigCollection : ConfigurationElementCollection
    {
        public ScrambledColumnConfigElement this[int index]
        {
            get { return BaseGet(index) as ScrambledColumnConfigElement; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ScrambledColumnConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ScrambledColumnConfigElement) element).Name;
        }
    }
}
