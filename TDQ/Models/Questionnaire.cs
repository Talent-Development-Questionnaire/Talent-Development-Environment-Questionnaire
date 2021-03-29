using System;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace TDQ.Models
{
    public class Questionnaire
    {
        [JsonProperty ("questionnaire_id")]
        public string ID { get; set; }

        [JsonProperty ("name")]
        public string Name { get; set; }

        [JsonProperty ("type")]
        public string Type { get; set; }

        [JsonProperty ("coach_id")]
        public string Coach_ID { get; set; }

        [JsonProperty ("completions")]
        public string Completions { get; set; }
    }
}
