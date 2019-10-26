using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Scrambler.Config
{
    public class PairedColumnsOutsideTableConfig
    {
        [JsonProperty(Required = Required.Always)]
        public List<List<string>> ColumnMapping { get; set; }
        [JsonProperty(Required = Required.Always)]
        public List<SourceDestMappingStepConfig> SourceDestMapping { get; set; }

        //[OnDeserialized]
        //private void OnDeserialized(StreamingContext context)
        //{
        //    if (ColumnMapping.Any(l => l.Count != 2))
        //    {
        //        new SerializationException("Column mapping elements must consist of 2 columns.");
        //    }
        //}
    }
}
