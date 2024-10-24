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
        private OrderService _orderService;
        private List<Order> _orders = new List<Order>();

        public OrderController(ILogger logger)
        {
            _orderService = new OrderService(logger);
        }

        public OrderController(List<Order> orders, ILogger logger)
        {
            _orderService = new OrderService(logger);
            _orders = orders;
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

        public void ProcessOrders(string area, DateTime deliveryTimeStart, DateTime? deliveryTimeEnd)
        {
            try
            {
                _orders = _orderService.FilterOrders(_orders, area, deliveryTimeStart, deliveryTimeEnd);
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
