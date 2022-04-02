using System;
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace Models
{
    public class Product
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("metafields")]
        public IEnumerable<Metafield> Metafields { get; set; }

        [JsonPropertyName("toUpdate")]
        public bool ToUpdate { get; set; }

    }

    public class Metafield
    {
        [JsonPropertyName("key")]
        public int? Key { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}
