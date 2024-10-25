using BL.DataGenerators;
using BL.Models;
using BL.Services;
using CLI;
using CLI.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Core;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using Serilog;

class Program
{
    static void Main(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .AddCommandLine(args)
            .Build();


        string filePath = config["FilePath"] ?? "undefined";
        string logPath = config["LogPath"] ?? "undefined";
        string outPath = config["OutPath"] ?? "undefined";
        string filterDistrict = config["FilterDistrict"] ?? "undefined";
        DateTime deliveryTimeStart = DateTime.Parse(config["DeliveryTimes:Start"] ?? DateTime.Now.ToString());
        DateTime? deliveryTimeEnd = null;

        string endDateString = config["DeliveryTimes:End"];
        if (!string.IsNullOrWhiteSpace(endDateString))
        {
            if (DateTime.TryParse(endDateString, out var parsedEndDate))
            {
                if (parsedEndDate < deliveryTimeStart)
                {
                    Console.WriteLine("Incorrect end date: cannot be earlier than start date. Filtering only by start date.");
                }
                else
                {
                    deliveryTimeEnd = parsedEndDate;
                }
            }
            else
            {
                Console.WriteLine($"Invalid end date format: '{endDateString}'. Filtering only by start date.");
            }
        }
        else
        {
            Console.WriteLine("No end date provided. Filtering only by start date.");
        }


        Console.WriteLine($"\nFile Path: {filePath}");
        Console.WriteLine($"Log Path: {logPath}");
        Console.WriteLine($"Output Path: {outPath}");
        Console.WriteLine($"Filter District: {filterDistrict}");
        Console.WriteLine($"Delivery Time Start: {deliveryTimeStart}");
        Console.WriteLine($"Delivery Time End: {deliveryTimeEnd}\n");
        
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IOrderService, OrderService>()
            .AddSingleton<OrderController>()
            .AddSingleton<BL.Logging.ILogger>(sp => new SerilogLogger(logPath))
            .BuildServiceProvider();

        var controller = serviceProvider.GetRequiredService<OrderController>();

        controller.ReadOrders(filePath);
        controller.ProcessOrders(filterDistrict, deliveryTimeStart, deliveryTimeEnd);
        controller.WriteFilteredOrders(outPath);

        Log.CloseAndFlush();
    }
}
