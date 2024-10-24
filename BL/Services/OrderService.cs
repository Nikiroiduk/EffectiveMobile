using BL.Logging;
using BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class OrderService
    {
        private readonly ILogger _logger;

        public OrderService(ILogger logger)
        {
            _logger = logger;
        }
        public List<Order> FilterOrders(List<Order> orders, string filterArea, DateTime deliveryTimeStart, DateTime? deliveryTimeEnd)
        {
            _logger.LogInformation($"Filtering orders according to: {filterArea}, {deliveryTimeStart}, {(deliveryTimeEnd.HasValue ? deliveryTimeEnd.Value.ToString() : "No end time")}");

            if (orders.Count == 0)
            {
                throw new InvalidDataException($"There is no records in a collection");
            }

            var filteredOrders = orders.FindAll(o =>
                o.Area.Equals(filterArea, StringComparison.OrdinalIgnoreCase) &&
                o.DeliveryTime >= deliveryTimeStart);

            if (deliveryTimeEnd.HasValue)
            {
                filteredOrders = filteredOrders.FindAll(fo => fo.DeliveryTime <= deliveryTimeEnd);
            }

            _logger.LogInformation($"{filteredOrders.Count} mathicng records were found.");

            return filteredOrders;
        }

        public List<Order> ReadOrdersFromFile(string filePath)
        {
            _logger.LogInformation($"Reading data from file: '{filePath}'");

            var orders = new List<Order>();

            try
            {
                foreach (var line in File.ReadLines(filePath))
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        try
                        {
                            Order order = Order.Parse(line);
                            orders.Add(order);
                        }
                        catch (ArgumentException ex)
                        {
                            _logger.LogError($"Error parsing line: '{line}'. {ex.Message}");
                            continue;
                        }
                    }
                }

                _logger.LogInformation($"Successfully read: {orders.Count} lines");
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError($"File not found: '{filePath}'. {ex.Message}");
                throw new FileNotFoundException($"File not found: '{filePath}'. {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError($"Access denied to file: '{filePath}'. {ex.Message}");
                throw new UnauthorizedAccessException($"Access denied to file: '{filePath}'. {ex.Message}");
            }
            catch (IOException ex)
            {
                _logger.LogError($"IO error while reading file: '{filePath}'. {ex.Message}");
                throw new IOException($"IO error while reading file: '{filePath}'. {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred while reading the file: '{filePath}'. {ex.Message}");
                throw new Exception($"An unexpected error occurred while reading the file: '{filePath}'. {ex.Message}");
            }

            return orders;
        }

        public void WriteFilteredOrders(string filePath, List<Order> filteredOrders)
        {
            _logger.LogInformation($"Writing {filteredOrders.Count} filtered orders to file: '{filePath}'");

            try
            {
                if (!Path.GetExtension(filePath).Equals(".csv", StringComparison.CurrentCultureIgnoreCase))
                {
                    throw new InvalidOperationException("The output file format is not CSV. Please save the file with a .csv extension.");
                }

                using (var writer = new StreamWriter(filePath))
                {
                    writer.WriteLine("OrderId,Weight,Area,DeliveryTime");

                    foreach (var order in filteredOrders)
                    {
                        writer.WriteLine($"{order.OrderId},{order.Weight},{order.Area},{order.DeliveryTime:yyyy-MM-dd HH:mm:ss}");
                    }
                }

                _logger.LogInformation("Filtered orders successfully written to file.");
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError($"Access to the file {filePath} denied: {ex.Message}");
                throw new UnauthorizedAccessException($"Access to the file {filePath} denied: {ex.Message}");
            }
            catch (IOException ex)
            {
                _logger.LogError($"An I/O error occurred while writing to the file {filePath}: {ex.Message}");
                throw new IOException($"An I/O error occurred while writing to the file {filePath}: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                throw new Exception($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
