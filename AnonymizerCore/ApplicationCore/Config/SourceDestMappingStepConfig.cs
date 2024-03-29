﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Scrambler.Config
{
    public class SourceDestMappingStepConfig
    {
        public string DestinationConnectionString { get; set; }
        public string DestinationFullTableName { get; set; }
        public string DestinationLinkedInstance { get; set; }
        [JsonProperty(Required = Required.Always)]
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
