using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TDQ.Models
{
    public class CoachUser
    {
        [JsonProperty("coach_id")]
        public string ID { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("gender")]
        public string Gender { get; set; }
        [JsonProperty("dob")]
        public string Dob { get; set; }
    }
}

