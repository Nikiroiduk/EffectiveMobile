using BL.Logging;
using BL.Models;
using BL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI.Controllers
{
    public class OrderController
    {
        private readonly IOrderService _orderService;
        private List<Order> _orders = new List<Order>();
        public List<Order> Orders
        {
            get => _orders;
            set => _orders = value;
        }

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public void ReadOrders(string filePath)
        {
            try
            {
                var orders = _orderService.ReadOrdersFromFile(filePath);
                _orders = orders;
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
            Console.WriteLine($"Data loaded [{_orders.Count}]");
        }

        public void ProcessOrders(string district, DateTime deliveryTimeStart, DateTime? deliveryTimeEnd)
        {
            try
            {
                _orders = _orderService.FilterOrders(_orders, district, deliveryTimeStart, deliveryTimeEnd);
                Console.WriteLine($"Data filtered [{_orders.Count}]");
            }
            catch(InvalidDataException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
        }

        public void WriteFilteredOrders(string filePath)
        {
            try
            {
                _orderService.WriteFilteredOrders(filePath, _orders);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
            Console.WriteLine($"Data saved [{_orders.Count}]");
        }
    }
}
