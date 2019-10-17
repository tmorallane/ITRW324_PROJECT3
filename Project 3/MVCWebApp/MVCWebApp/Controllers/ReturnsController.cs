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
using MVCWebApp.ViewModels;

namespace MVCWebApp.Controllers
{
    public class ReturnsController : Controller
    {

        private MongoDBContext dbcontext;
        private IMongoCollection<ReturnsModel> returnsCollection;
        returnsViewModel returnsView = new returnsViewModel();

        public ReturnsController()
        {
            dbcontext = new MongoDBContext();
            returnsCollection = dbcontext.database.GetCollection<ReturnsModel>("returns");
        }

        [Authorize]
        public ActionResult Index()
        {
            List<ReturnsModel> returned = returnsCollection.AsQueryable<ReturnsModel>().ToList();

            int total_return = (from x in returned.Where(x => x.Returned.Contains("Yes")) select x.OrderID).Count();

            int africa_region = (from x in returned.Where(x => x.Region.Contains("Africa")) select x.OrderID).Count();
            int asia_region = (from x in returned.Where(x => x.Region.Contains("Asia")) select x.OrderID).Count();
            int europe_region = (from x in returned.Where(x => x.Region.Contains("Europe")) select x.OrderID).Count();
            int america_region = (from x in returned.Where(x => x.Region.Contains("US")) select x.OrderID).Count();

            returnsView.total_returns = total_return;

            returnsView.Africa_region = africa_region;
            returnsView.America_region = america_region;
            returnsView.Asia_region = asia_region;
            returnsView.Europe_region = europe_region;

            return View(returnsView);
        }

        // GET: Returns/Details/5
        public ActionResult Details(string id)
        {
            var returnsId = new ObjectId(id);
            var returns = returnsCollection.AsQueryable<ReturnsModel>().SingleOrDefault(x => x.Id == returnsId);
            return View(returns);
        }

        // GET: Returns/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Returns/Create
        [HttpPost]
        public ActionResult Create(ReturnsModel returns)
        {
            try
            {
                returnsCollection.InsertOne(returns);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Returns/Edit/5
        public ActionResult Edit(string id)
        {
            var returnsId = new ObjectId(id);
            var returns = returnsCollection.AsQueryable<ReturnsModel>().SingleOrDefault(x => x.Id == returnsId);
            return View(returns);
        }

        // POST: Returns/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, ReturnsModel returns)
        {
            try
            {
                var filter = Builders<ReturnsModel>.Filter.Eq("_id", ObjectId.Parse(id));
                var update = Builders<ReturnsModel>.Update
                    .Set("Returned", returns.Returned)
                    .Set("OrderId", returns.OrderID)
                    .Set("Region", returns.Region);

                var result = returnsCollection.UpdateOne(filter, update);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Returns/Delete/5
        public ActionResult Delete(string id)
        {
            var returnsId = new ObjectId(id);
            var returns = returnsCollection.AsQueryable<ReturnsModel>().SingleOrDefault(x => x.Id == returnsId);
            return View(returns);
        }

        // POST: Returns/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, ReturnsModel returns)
        {
            try
            {
                returnsCollection.DeleteOne(Builders<ReturnsModel>.Filter.Eq("_id", ObjectId.Parse(id)));
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
