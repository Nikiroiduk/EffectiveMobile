using BL.Models;

namespace BLTests
{
    public class OrderTests
    {
        [Fact]
        public void Parse_ValidInput_ReturnsOrder()
        {
            // Arrange
            var input = "123, 12.34, Meh, 2024-10-25 23:30:00";

            // Act
            var order = Order.Parse(input);

            // Assert
            Assert.Equal(123, order.OrderId);
            Assert.Equal(12.34m, order.Weight);
            Assert.Equal("Meh", order.Area);
            Assert.Equal(new DateTime(2024, 10, 25, 23, 30, 0), order.DeliveryTime);
        }

        [Fact]
        public void Parse_InvalidInput_ThrowsArgumentException()
        {
            // Arrange
            var input = "123, 12.34, Meh";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => Order.Parse(input));
        }

        [Fact]
        public void Parse_InvalidOrderId_ThrowsArgumentException()
        {
            // Arrange
            var input = "abc, 12.34, Meh, 2024-10-25 23:30:00";

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => Order.Parse(input));
            Assert.Equal("Invalid OrderId.", ex.Message);
        }

        [Fact]
        public void Parse_InvalidWeight_ThrowsArgumentException()
        {
            // Arrange
            var input = "123, abc, Meh, 2024-10-25 23:30:00";

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => Order.Parse(input));
            Assert.Equal("Invalid Weight.", ex.Message);
        }

        [Fact]
        public void Parse_EmptyArea_ThrowsArgumentException()
        {
            // Arrange
            var input = "123, 12.34, , 2024-10-25 23:30:00";

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => Order.Parse(input));
            Assert.Equal("Area cannot be empty.", ex.Message);
        }

        [Fact]
        public void Parse_InvalidDeliveryTime_ThrowsArgumentException()
        {
            // Arrange
            var input = "123, 12.34, Meh, abc";

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => Order.Parse(input));
            Assert.Equal("Invalid DeliveryTime.", ex.Message);
        }

        [Fact]
        public void ToString_ReturnsFormattedString()
        {
            // Arrange
            var order = new Order
            {
                OrderId = 123,
                Weight = 12.34m,
                Area = "Meh",
                DeliveryTime = new DateTime(2024, 10, 25, 23, 30, 0)
            };

            // Act
            var result = order.ToString();

            // Assert
            Assert.Equal("Id: 123, Weight: 12.34, Area: Meh, Delivary time: 2024-10-25 23:30:00", result);
        }
    }
}