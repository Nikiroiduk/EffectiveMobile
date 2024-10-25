using BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public interface IOrderService
    {
        List<Order> FilterOrders(List<Order> orders, string filterDistrict, DateTime deliveryTimeStart, DateTime? deliveryTimeEnd);
        List<Order> ReadOrdersFromFile(string filePath);
        void WriteFilteredOrders(string filePath, List<Order> filteredOrders);
    }
}
