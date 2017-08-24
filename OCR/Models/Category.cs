using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
namespace OCR.Models
{
    public class Category
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}