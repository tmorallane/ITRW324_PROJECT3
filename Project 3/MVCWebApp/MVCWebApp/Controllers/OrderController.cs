﻿using System;
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
using PagedList.Mvc;
using PagedList;
using MVCWebApp.ViewModels;

namespace MVCWebApp.Controllers
{
    public class OrderController : Controller
    {

        private MongoDBContext dbcontext;
        private IMongoCollection<OrderModel> orderCollection;
        ordersViewModel orderView = new ordersViewModel();

        public OrderController()
        {
            dbcontext = new MongoDBContext();
            orderCollection = dbcontext.database.GetCollection<OrderModel>("orders");
        }

        [Authorize]
        public ActionResult Index()
        {
            List<OrderModel> orders = orderCollection.AsQueryable<OrderModel>().ToList();

            int total_order = (from x in orders select x.OrderId).Count();
            //double total_sale = (from x in orders select Convert.ToDouble(x.Sales.Replace("$", string.Empty).Replace(".", ","))).Sum();
            int market_africa = (from x in orders.Where(x => x.Market.Contains("Africa")) select x.OrderId).Count();
            int market_asia = (from x in orders.Where(x => x.Market.Contains("Asia")) select x.OrderId).Count();
            int market_America = (from x in orders.Where(x => x.Market.Contains("US")) select x.OrderId).Count();
            int market_europe = (from x in orders.Where(x => x.Market.Contains("Europe")) select x.OrderId).Count();

            orderView.Africa_market = market_africa;
            orderView.America_market = market_America;
            orderView.Asia_market = market_asia;
            orderView.Europe_market = market_europe;

            orderView.total_orders = total_order;
            //orderView.total_sales = total_sale;

            return View(orderView);
        }

        // GET: Order/Details/5
        public ActionResult Details(string id)
        {
            var orderId = new ObjectId(id);
            var orders = orderCollection.AsQueryable<OrderModel>().SingleOrDefault(x => x.Id == orderId);
            return View(orders);
        }

        // GET: Order/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Order/Create
        [HttpPost]
        public ActionResult Create(OrderModel order)
        {
            try
            {

                orderCollection.InsertOne(order);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Order/Edit/5
        public ActionResult Edit(string id)
        {
            var orderId = new ObjectId(id);
            var order = orderCollection.AsQueryable<OrderModel>().SingleOrDefault(x => x.Id == orderId);
            return View(order);
        }

        // POST: Order/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, OrderModel order)
        {
            try
            {
                var filter = Builders<OrderModel>.Filter.Eq("_id", ObjectId.Parse(id));
                var update = Builders<OrderModel>.Update
                    .Set("Row ID", order.RowID)
                    .Set("Order ID", order.OrderId)
                    .Set("Order Date", order.OrderDate)
                    .Set("Ship Date", order.ShipDate)
                    .Set("Ship Mode", order.ShipMode)
                    .Set("Customer ID", order.CustomerId)
                    .Set("Segment", order.Segment)
                    .Set("Postal Code", order.PostalCode)
                    .Set("City", order.City)
                    .Set("State", order.State)
                    .Set("Coutry", order.Country)
                    .Set("Region", order.Region)
                    .Set("Market", order.Market)
                    .Set("Product ID", order.ProductId)
                    .Set("Category", order.Category)
                    .Set("Sub-Category", order.SubCategory)
                    .Set("Product Name", order.ProductName)
                    .Set("Sales", order.Sales)
                    .Set("Quantity", order.Quantity)
                    .Set("Discount", order.Discount)
                    .Set("Profit", order.Profit)
                    .Set("Shipping Cost", order.ShippingCost)
                    .Set("Order Priority", order.OrderPriority);

                var result = orderCollection.UpdateOne(filter, update);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Order/Delete/5
        public ActionResult Delete(string id)
        {
            var orderId = new ObjectId(id);
            var order = orderCollection.AsQueryable<OrderModel>().SingleOrDefault(x => x.Id == orderId);
            return View(order);
        }

        // POST: Order/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, OrderModel order)
        {
            try
            {

                orderCollection.DeleteOne(Builders<OrderModel>.Filter.Eq("_id", ObjectId.Parse(id)));
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
