using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
namespace OCR.Models
{
    public class Question
    {
        [JsonProperty("categoryId")]
        public string CategoryId { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}