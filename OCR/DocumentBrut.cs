using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OCR
{
     public class DocumentBrut
     {

         public String docId { get; set; }


         public string ocr { get; set; }

            public string state { get; set; }
     }

     public class ServerTimeStamp
     {
         [JsonProperty(".sv")]
         public string TimestampPlaceholder { get; } = "timestamp";
     }
   
}