using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firebase.Test.Entities
{
  public  class Dinosaur
    {
        public Dinosaur(double height, double weight, double length)
        {
            this.Dimensions = new Dimensions { Height = height, Weight = weight, Length = length };
        }

        public Dinosaur()
        {
        }

        [JsonProperty(PropertyName = "ds")]
        public Dimensions Dimensions { get; set; }

        [JsonProperty(PropertyName = "weight")]
        public double Weight { get; set; }
    }
}
