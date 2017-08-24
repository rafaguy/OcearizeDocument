using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;




namespace OCR
{
    public class DataJson
    {
    }
    public class Rootobject
    {
        public Class1[] Property1 { get; set; }
    }

    public class Class1
    {
        public string question { get; set; }
        public string category { get; set; }
        public Listanswer[] listAnswer { get; set; }
        public string response { get; set; }
    }
    public class Listanswer
    {
        public string answer { get; set; }
        public bool isTrue { get; set; }
    }

}