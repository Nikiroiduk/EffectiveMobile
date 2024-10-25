using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.IO;
using BL.Models;
using BL.Services;
using CLI.Controllers;
using CLITests;

public class OrderControllerTests
{
    private readonly Mock<IOrderService> _mockOrderService;
    private readonly OrderController _controller;

    public OrderControllerTests()
    {
        _mockOrderService = new Mock<IOrderService>();
        _controller = new OrderController(_mockOrderService.Object);
    }

    [Fact]
    public void ReadOrders_FileNotFound_PrintsErrorMessage()
    {
        // Arrange
        string filePath = "nonexistentfile.csv";
        _mockOrderService
            .Setup(s => s.ReadOrdersFromFile(filePath))
            .Throws(new FileNotFoundException("File not found"));

        // Act and Assert
        using (var consoleOutput = new ConsoleOutput())
        {
            _controller.ReadOrders(filePath);
            Assert.Contains("File not found", consoleOutput.GetOutput());
        }
    }

    [Fact]
    public void ProcessOrders_NoMatchingOrders_PrintsZeroFilteredCount()
    {
        // Arrange
        var orders = new List<Order>();
        string district = "Meh";
        DateTime deliveryTimeStart = DateTime.Now;

        _mockOrderService
            .Setup(s => s.FilterOrders(orders, district, deliveryTimeStart, null))
            .Returns(new List<Order>());

        _controller.Orders = orders;

        // Act and Assert
        using (var consoleOutput = new ConsoleOutput())
        {
            _controller.ProcessOrders(district, deliveryTimeStart, null);
            Assert.Contains("Data filtered [0]", consoleOutput.GetOutput());
        }
    }

    [Fact]
    public void WriteFilteredOrders_InvalidFileFormat_PrintsErrorMessage()
    {
        // Arrange
        string filePath = "output.txt";
        _mockOrderService
            .Setup(s => s.WriteFilteredOrders(filePath, It.IsAny<List<Order>>()))
            .Throws(new InvalidOperationException("The output file format is not CSV"));

        // Act and Assert
        using (var consoleOutput = new ConsoleOutput())
        {
            _controller.WriteFilteredOrders(filePath);
            Assert.Contains("The output file format is not CSV", consoleOutput.GetOutput());
        }
    }

    [Fact]
    public void ReadOrders_ValidFilePath_LoadsOrders()
    {
        // Arrange
        string filePath = "orders.csv";
        var orders = new List<Order>
        {
            new Order { OrderId = 1, Weight = 10, Area = "MehA", DeliveryTime = DateTime.Now },
            new Order { OrderId = 2, Weight = 20, Area = "MehB", DeliveryTime = DateTime.Now }
        };
        _mockOrderService.Setup(s => s.ReadOrdersFromFile(filePath)).Returns(orders);

        // Act
        using (var consoleOutput = new ConsoleOutput())
        {
            _controller.ReadOrders(filePath);
            Assert.Contains($"Data loaded [{orders.Count}]", consoleOutput.GetOutput());
        }
    }

    [Fact]
    public void ProcessOrders_WithMatchingOrders_PrintsFilteredCount()
    {
        // Arrange
        var orders = new List<Order>
        {
            new Order { OrderId = 1, Weight = 10, Area = "Meh", DeliveryTime = DateTime.Now.AddHours(1) }
        };
        string district = "Meh";
        DateTime deliveryTimeStart = DateTime.Now;

        _mockOrderService
            .Setup(s => s.FilterOrders(orders, district, deliveryTimeStart, null))
            .Returns(orders);

        _controller.Orders = orders;

        // Act and Assert
        using (var consoleOutput = new ConsoleOutput())
        {
            _controller.ProcessOrders(district, deliveryTimeStart, null);
            Assert.Contains($"Data filtered [{orders.Count}]", consoleOutput.GetOutput());
        }
    }
}
