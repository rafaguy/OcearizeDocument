using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Test.Entities;
using System.Threading.Tasks;

namespace Firebase.Test
{
    [TestClass]
    public class FirebasePathTests
    {
        public const string BasePath = "https://indexation-1b8f8.firebaseio.com/";
        public const string Token = "aBcEfgH";
        [TestMethod]
        public async Task TestAuthPath()
        {
            var firebase = new FirebaseClient(BasePath);
            var dino = await firebase.Child("dinosaurs").PostAsync(new Dinosaur());
            Console.WriteLine($"the key of dino: {dino.Key}");

        }
        [TestMethod]
        public void TestKeyGenerator()
        {
           // var key = new FirebaseKeyGenerator();
            var keygenerated = FirebaseKeyGenerator.Next();
        }
    }
}
