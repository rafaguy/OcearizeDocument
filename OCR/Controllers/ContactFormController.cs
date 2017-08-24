using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace OCR.Controllers
{
    public class ContactFormController : SurfaceController
    {
        // GET: ContactForm
        public ActionResult Index()
        {
            return View();
        }
       
        [HttpPost]
        public ActionResult Submit()
        {
            if(!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }
            return RedirectToCurrentUmbracoPage();
        }
    }
}