using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ApplicationCore.Config
{
    public class SourceDestMappingStepConfig
    {
        public string ConnectionString { get; set; }
        public string TableNameWithSchema { get; set; }
        public List<List<string>> ForeignKeyMapping { get; set; }

        //[OnDeserialized]
        //private void OnDeserialized(StreamingContext context)
        //{
        //    if (ForeignKeyMapping.Any(l => l.Count != 2))
        //    {
        //        throw new SerializationException("Foreign key mapping elements must consist of 2 column names.");
        //    }
        //}
    }
}
