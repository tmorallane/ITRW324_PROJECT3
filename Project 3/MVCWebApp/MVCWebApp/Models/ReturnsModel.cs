using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MVCWebApp.Models
{
    public class ReturnsModel
    {
        [BsonId]

        public ObjectId Id { get; set; }

        [BsonElement("Returned")]

        public string Returned { get; set; }

        [BsonElement("Order ID")]

        public string OrderID { get; set; }

        [BsonElement("Region")]

        public string Region { get; set; }
    }
}