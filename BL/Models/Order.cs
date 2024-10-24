using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models
{
    public class Order
    {
        public required int OrderId { get; set; }
        public required decimal Weight { get; set; }
        public required string Area { get; set; }
        public required DateTime DeliveryTime { get; set; }

        public override string ToString()
        {
            //csv format
            //return $"{OrderId},{Math.Round(Weight, 2)},{Area},{DeliveryTime:yyyy-MM-dd HH:mm:ss}";
            return $"Id: {OrderId}, Weight: {Math.Round(Weight, 2)}, Area: {Area}, Delivary time: {DeliveryTime:yyyy-MM-dd HH:mm:ss}";
        }

        public static Order Parse(string inputData)
        {
            var parts = inputData.Split(',');

            if (parts.Length != 4)
            {
                throw new ArgumentException("Input data must contain exactly four fields.");
            }

            if (!int.TryParse(parts[0].Trim(), out int orderId))
            {
                throw new ArgumentException("Invalid OrderId.");
            }

            if (!decimal.TryParse(parts[1].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal weight))
            {
                throw new ArgumentException("Invalid Weight.");
            }

            var area = parts[2].Trim();
            if (string.IsNullOrWhiteSpace(area))
            {
                throw new ArgumentException("Area cannot be empty.");
            }

            if (!DateTime.TryParse(parts[3].Trim(), out DateTime deliveryTime))
            {
                throw new ArgumentException("Invalid DeliveryTime.");
            }

            return new Order
            {
                OrderId = orderId,
                Weight = weight,
                Area = area,
                DeliveryTime = deliveryTime
            };
        }
    }
}
