using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anonymizer.ConfigElements
{
    public class ConstantColumnConfigElement : ColumnConfigElement
    {
        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get { return base["value"] as string; }
        }
    }
}
