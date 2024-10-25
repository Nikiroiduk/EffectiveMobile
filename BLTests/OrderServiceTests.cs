using BL.Logging;
using BL.Models;
using BL.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTests
{
    public class OrderServiceTests
    {
        private readonly Mock<ILogger> _loggerMock;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _loggerMock = new Mock<ILogger>();
            _orderService = new OrderService(_loggerMock.Object);
        }

        [Fact]
        public void FilterOrders_ValidFilter_ReturnsFilteredOrders()
        {
            // Arrange
            var orders = new List<Order>
        {
            new Order { OrderId = 1, Weight = 10, Area = "MehA", DeliveryTime = new DateTime(2024, 10, 25, 14, 0, 0) },
            new Order { OrderId = 2, Weight = 15, Area = "MehB", DeliveryTime = new DateTime(2024, 10, 25, 15, 0, 0) },
            new Order { OrderId = 3, Weight = 20, Area = "MehA", DeliveryTime = new DateTime(2024, 10, 25, 16, 0, 0) }
        };

            // Act
            var result = _orderService.FilterOrders(orders, "MehA", new DateTime(2024, 10, 25, 13, 0, 0), new DateTime(2024, 10, 25, 15, 0, 0));

            // Assert
            Assert.Single(result);
            Assert.Equal(1, result[0].OrderId);
            _loggerMock.Verify(l => l.LogInformation(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Fact]
        public void FilterOrders_NoMatchingOrders_ReturnsEmptyList()
        {
            // Arrange
            var orders = new List<Order>
        {
            new Order { OrderId = 1, Weight = 10, Area = "MehA", DeliveryTime = new DateTime(2024, 10, 25, 14, 0, 0) }
        };

            // Act
            var result = _orderService.FilterOrders(orders, "MehB", new DateTime(2024, 10, 25, 13, 0, 0), null);

            // Assert
            Assert.Empty(result);
            _loggerMock.Verify(l => l.LogInformation(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Fact]
        public void FilterOrders_EmptyOrderList_ThrowsInvalidDataException()
        {
            // Arrange
            var orders = new List<Order>();

            // Act & Assert
            Assert.Throws<InvalidDataException>(() => _orderService.FilterOrders(orders, "MehA", DateTime.Now, null));
        }

        [Fact]
        public void ReadOrdersFromFile_ValidFile_ReturnsOrders()
        {
            // Arrange
            var filePath = "validFile.txt";
            File.WriteAllLines(filePath, new[]
            {
            "1,10,MehA,2024-10-25 14:00:00",
            "2,15,MehB,2024-10-25 15:00:00"
        });

            // Act
            var result = _orderService.ReadOrdersFromFile(filePath);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(1, result[0].OrderId);
            Assert.Equal("MehA", result[0].Area);
            _loggerMock.Verify(l => l.LogInformation(It.IsAny<string>()), Times.AtLeastOnce);

            // Cleanup
            File.Delete(filePath);
        }

        [Fact]
        public void ReadOrdersFromFile_FileNotFound_ThrowsFileNotFoundException()
        {
            // Arrange
            var filePath = "nonexistentFile.txt";

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => _orderService.ReadOrdersFromFile(filePath));
            _loggerMock.Verify(l => l.LogError(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void WriteFilteredOrders_ValidFilePath_WritesOrders()
        {
            // Arrange
            var filePath = "filteredOrders.csv";
            var orders = new List<Order>
        {
            new Order { OrderId = 1, Weight = 10, Area = "MehA", DeliveryTime = new DateTime(2024, 10, 25, 14, 0, 0) }
        };

            // Act
            _orderService.WriteFilteredOrders(filePath, orders);

            // Assert
            Assert.True(File.Exists(filePath));
            var lines = File.ReadAllLines(filePath);
            Assert.Equal(2, lines.Length);
            Assert.Contains("OrderId,Weight,Area,DeliveryTime", lines[0]);
            Assert.Contains("1,10,MehA,2024-10-25 14:00:00", lines[1]);
            _loggerMock.Verify(l => l.LogInformation(It.IsAny<string>()), Times.AtLeastOnce);

            // Cleanup
            File.Delete(filePath);
        }

        [Fact]
        public void WriteFilteredOrders_InvalidFileExtension_ThrowsInvalidOperationException()
        {
            // Arrange
            var filePath = "filteredOrders.txt";
            var orders = new List<Order>
        {
            new Order { OrderId = 1, Weight = 10, Area = "MehA", DeliveryTime = DateTime.Now }
        };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _orderService.WriteFilteredOrders(filePath, orders));
        }
    }
}
