using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Bson;
using MongoDB.Driver.Core;
using System.Configuration;
using MVCWebApp.App_Start;
using MongoDB.Driver;
using MVCWebApp.Models;

namespace MVCWebApp.Controllers
{
    public class PeopleController : Controller
    {
        private MongoDBContext dbcontext;
        private IMongoCollection<PeopleModel> peopleCollection;

        public PeopleController()
        {
            dbcontext = new MongoDBContext();
            peopleCollection = dbcontext.database.GetCollection<PeopleModel>("people");
        }
        // GET: People
        public ActionResult Index()
        {
            List<PeopleModel> peoples = peopleCollection.AsQueryable<PeopleModel>().ToList();

            return View(peoples);
        }

        // GET: People/Details/5
        public ActionResult Details(string id)
        {
            var peopleId = new ObjectId(id);
            var people = peopleCollection.AsQueryable<PeopleModel>().SingleOrDefault(x => x.Id == peopleId);
            return View(people);
        }

        // GET: People/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: People/Create
        [HttpPost]
        public ActionResult Create(PeopleModel people)
        {
            
            try
            {
                peopleCollection.InsertOne(people);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: People/Edit/5
        public ActionResult Edit(string id)
        {
            var peopleId = new ObjectId(id);
            var people = peopleCollection.AsQueryable<PeopleModel>().SingleOrDefault(x => x.Id == peopleId);
            return View(people);
        }

        // POST: People/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, PeopleModel people)
        {
            try
            {

                var filter = Builders<PeopleModel>.Filter.Eq("_id", ObjectId.Parse(id));
                var update = Builders<PeopleModel>.Update
                    .Set("Person", people.Person)
                    .Set("Region", people.Region);

                var result = peopleCollection.UpdateOne(filter, update);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: People/Delete/5
        public ActionResult Delete(string id)
        {
            var peopleId = new ObjectId(id);
            var people = peopleCollection.AsQueryable<PeopleModel>().SingleOrDefault(x => x.Id == peopleId);
            return View(people);
        }

        // POST: People/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, PeopleModel people)
        {
            try
            {
                peopleCollection.DeleteOne(Builders<PeopleModel>.Filter.Eq("_id", ObjectId.Parse(id)));

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
