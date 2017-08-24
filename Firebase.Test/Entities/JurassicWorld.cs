using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firebase.Test.Entities
{
  public  class JurassicWorld
    {
        public JurassicWorld()
        {
            this.Dinosaurs = new Dictionary<string, Dinosaur>();
        }

        public Dictionary<string, Dinosaur> Dinosaurs { get; set; }
    }
}
