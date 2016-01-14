using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoAspTemplate.Controllers
{
    public class DefaultController : Controller
    {
        protected IMongoClient _client;
        protected IMongoDatabase _database;

        public DefaultController() {
            _client = new MongoClient(new MongoClientSettings
            {
                Server = new MongoServerAddress("localhost", 27017)
            });
            _database = _client.GetDatabase("test");
        }

        //
        // GET: /Default/
        public async Task<ActionResult> Index()
        {
            var collection = _database.GetCollection<BsonDocument>("restaurants");
            var resourants = await collection.Find(new BsonDocument()).ToListAsync();

            return View(resourants);
        }

        public async Task<RedirectToRouteResult> New() {
            var document = new BsonDocument() {
                {
                    "address", new BsonDocument {
                        {"street", "2 Avenue"},
                        {"zipcode", "10075"},
                        {"building", "1480"},
                        {"coord", new BsonArray {73.9557413, 40.7720266}}
                    }
                },
                { "borough", "Manhattan" },
                { "cuisine", "Italian" }
            };

            var collection = _database.GetCollection<BsonDocument>("restaurants");
            await collection.InsertOneAsync(document);

            return RedirectToAction("Index");
        }
	}
}