using BL.Models;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DataGenerators
{
    public static class MockDataGenerator
    {
        private readonly static string[] _areas = new[]
        {
            "Maplewood",
            "Riverbend",
            "Cedarview",
            "Lakeside",
            "Bayside"
        };

        public static List<Order> GenerateOrders(int count)
        {
            var faker = new Faker<Order>()
                .RuleFor(o => o.OrderId, f => f.IndexFaker + 1)
                .RuleFor(o => o.Weight, f => f.Random.Decimal(1, 300))
                .RuleFor(o => o.Area, f => f.PickRandom(_areas))
                .RuleFor(o => o.DeliveryTime, f => f.Date.Between(DateTime.Now, DateTime.Now.AddMonths(5)));
            return faker.Generate(count);
        }
    }
}
