using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
namespace OCR.Models
{
    public class Reponse
    {
        [JsonProperty("questId")]
        public string QuestionId { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("isTrue")]
        public bool IsTrue { get; set; }
    }
}