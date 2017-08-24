using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core;
using Umbraco.Web.Routing;
using Umbraco.Core.Events;
using Umbraco.Core.Services;
using OCR.Models;
using Firebase.Database.Query;
using Umbraco.Core.Models;

namespace OCR.Application
{
    public class MyApplication: ApplicationEventHandler
    {
        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
           // ContentFinderResolver.Current.InsertType<CustomContentFinder>();
          //  base.ApplicationStarting(umbracoApplication, applicationContext);
        }
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            //ContentService.Saved += ContentService_Saved;
            ContentService.Published += ContentService_Published;
            DataTypeService.Saved += DataTypeService_Saving;
            //base.ApplicationStarted(umbracoApplication, applicationContext);
        }

        private async void ContentService_Published(Umbraco.Core.Publishing.IPublishingStrategy sender, PublishEventArgs<IContent> e)
        {
            foreach (var content in e.PublishedEntities.Where(c => c.ContentType.Alias.Equals("question")).Where(c => c.WasPropertyDirty("Id")))
            {
                var umbracoHelper = new Umbraco.Web.UmbracoHelper(Umbraco.Web.UmbracoContext.Current);
                var node = umbracoHelper.TypedContent(content.Id);
               var question=(string)node.GetProperty("questionaire").Value;
                var category=(string)node.GetProperty("category").Value;
                var listAnswer = (IEnumerable<IPublishedContent>)node.GetProperty("listAnswer").Value;

                 var client = new Firebase.Database.FirebaseClient("https://qcmapp-4d7b3.firebaseio.com/");
                var categoryFirebase =await client.Child("category").OrderBy("name").EqualTo(category).LimitToFirst(1).OnceAsync<Category>();

                var catId = categoryFirebase.First().Key;

                var data =await client.Child("question").PostAsync(new Question { CategoryId = catId, Value = question });

                foreach (var list in listAnswer)
                {
                    //listAns = new List<Listanswer>();
                    var answer = (string)list.GetProperty("answer").Value;
                    var isTrue = (bool)list.GetProperty("isTrueAnswer").Value;
                    await   client.Child("reponse").PostAsync(new Reponse { QuestionId = data.Key, Value = answer,IsTrue=isTrue });
                }
            }
            
        }

        private void DataTypeService_Saving(IDataTypeService sender, SaveEventArgs<Umbraco.Core.Models.IDataTypeDefinition> e)
        {
           foreach(var dt in e.SavedEntities.Where(dt=>dt.Name.Equals("Question - category - Dropdown list")))
            {
                var dtService = ApplicationContext.Current.Services.DataTypeService;
                var list = dtService.GetPreValuesByDataTypeId(dt.Id);
                var client = new Firebase.Database.FirebaseClient("https://qcmapp-4d7b3.firebaseio.com/");
                var child = client.Child("category").PostAsync(new Category {  Name=list.Last()});
              // var reponse= child.PatchAsync<Question>();
              /*  foreach(var item in list)
                {
                    //item.Value.
                }*/
            }
        }

       /* private async void  ContentService_Saved(IContentService sender, SaveEventArgs<Umbraco.Core.Models.IContent> e)
        {
            foreach(var content in e.SavedEntities.Where(c=>c.ContentType.Alias.Equals("question")).Where(c=>c.WasPropertyDirty("Id")))
            {
                var umbracoHelper = new Umbraco.Web.UmbracoHelper(Umbraco.Web.UmbracoContext.Current);
                /*var node = umbracoHelper.TypedContent(content.Id);
               var question=(string)node.GetProperty("questionaire").Value;
                var category=(string)node.GetProperty("category").Value;
                var listAnswer = (IEnumerable<IPublishedContent>)node.GetProperty("listAnswer").Value;

                 var client = new Firebase.Database.FirebaseClient("https://indexation-1b8f8.firebaseio.com/");
                var categoryFirebase = client.Child("category").OrderBy("name").EqualTo(category).LimitToFirst(1).OnceAsync<Category>();
               var list= categoryFirebase.Result.ToList();*/
               
                
               
              //  var data=client.Child("question").PostAsync(new Question {  CategoryId="", Value=""})


          //  }
       // }
    }
}