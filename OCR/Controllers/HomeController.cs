using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using Tesseract;
using System.Drawing;
using Firebase.Database;
using System.Reactive.Linq;
using Umbraco.Core.Models;

namespace OCR.Controllers
{
    public class HomeController : SurfaceController
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
       
        public ActionResult GetQuestions()
        {
            var questions = Umbraco.TypedContentAtXPath("//questionContainer").FirstOrDefault().Children;
            var root = new Rootobject();
            var property =new List<Class1>();
            
            foreach (var question in questions)
            {
                
                var questionnaire= (string)question.GetProperty("questionaire").Value;
                var category = (string)question.GetProperty("category").Value;
                var class1 = new Class1 { question=questionnaire, category=category };
                var listAnswer =(IEnumerable<IPublishedContent>)question.GetProperty("listAnswer").Value;
                var listAns= new List<Listanswer>();
               foreach (var list in listAnswer)
                {
                     //listAns = new List<Listanswer>();
                    var answer =(string) list.GetProperty("answer").Value;
                    var isTrue =(bool) list.GetProperty("isTrueAnswer").Value;
                    listAns.Add(new Listanswer { answer = answer, isTrue = isTrue });
                }
                class1.listAnswer = listAns.ToArray();
                property.Add(class1);
            }
            root.Property1 = property.ToArray();
              return Json(root.Property1, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public async Task<JsonResult> UploadFile()
        {
            var files = Request.Files;
            var directory ="LOT"+ DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString().ToString() + DateTime.Now.Year.ToString().ToString() + DateTime.Now.Hour.ToString().ToString()+ DateTime.Now.Minute.ToString().ToString()+ DateTime.Now.Second.ToString().ToString();
            Directory.CreateDirectory(Server.MapPath("~/imageData/" + directory));
            try
            {
                foreach(string file in files)
                {
                    var fileContent = files[file];
                    if(fileContent !=null && fileContent.ContentLength>0)
                    {
                        var stream = fileContent.InputStream;
                        var fileName = fileContent.FileName;
                        var path = Path.Combine(Server.MapPath("~/imageData/"+directory), fileName);
                        using (var fileStream= System.IO. File.Create(path))
                        {
                            stream.CopyTo(fileStream);
                        }
                    }
                }
            }catch(Exception)
            {

            }
            await Ocearize(Server.MapPath("~/imageData/"+directory));

            return Json("Files Ocearized successfully");
        }
        public async System.Threading.Tasks.Task Ocearize(string directory)
        {
            //Process the list of files found in the directory
            var dir = directory.Split('\\').Last();
            string[] fileEntries = System.IO.Directory.GetFiles(directory);

            foreach(var file in fileEntries)
            {
                convertToPng(file);
                var result=  Do_Ocr(file);
                var client = new FirebaseClient("https://indexation-1b8f8.firebaseio.com/");
                var child = client.Child("docBrut");
                //await child.PostAsync(new DocumentBrut { docId = "/imageData/" + dir + "/" + Path.GetFileName(file).Split('.')[0]+".png", ocr = result,state="0" });
                await child.PostAsync($"docId=");
            }
        }

        public  string Do_Ocr(string imgPath)
        {
            var image = new Bitmap(imgPath);
            var ocrText = string.Empty;

            using (var engine = new TesseractEngine(Server.MapPath("~/tessdata"), "eng", EngineMode.Default))
            {
                using (var img = PixConverter.ToPix(image))
                {
                    using (var page = engine.Process(img))
                    {
                        ocrText = page.GetText();
                    }
                }
            }
            return ocrText;
        }
        public void convertToPng(string imagePath)
        {
            var extension = Path.GetExtension(imagePath);
            if(extension ==".png")
            {
                return;
            }
            var fileNoExt = imagePath.Replace(extension, "");
            System.Drawing.
            Bitmap.FromFile(imagePath)
              .Save(fileNoExt + ".png", System.Drawing.Imaging.ImageFormat.Png);
        }

        public void getContentId()
        {
            var contentType = Services.DataTypeService;
            var datatypeDef = contentType.GetDataTypeDefinitionByName("Question - category - Dropdown list");
            var lisPrevalue = contentType.GetPreValuesByDataTypeId(datatypeDef.Id);
            // var listPrev = new List<string> { "List1", "List2", "List3" };
            var dict = new Dictionary<string, PreValue>();
            dict.Add("123456", new PreValue(2, "ASDFASDTest3", 3));
            contentType.SavePreValues(datatypeDef.Id, dict);

          
        }

    }
}