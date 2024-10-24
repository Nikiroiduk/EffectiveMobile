using BL.DataGenerators;
using BL.Models;
using BL.Services;
using CLI;
using CLI.Controllers;
using Microsoft.Extensions.Configuration;
using Serilog.Core;
using System.Runtime.CompilerServices;

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
        if (config["DeliveryTimes:End"] != null)
        {
            deliveryTimeEnd = DateTime.Parse(config["DeliveryTimes:End"]);
        }

        Console.WriteLine($"\nFile Path: {filePath}");
        Console.WriteLine($"Log Path: {logPath}");
        Console.WriteLine($"Output Path: {outPath}");
        Console.WriteLine($"Filter District: {filterDistrict}");
        Console.WriteLine($"Delivery Time Start: {deliveryTimeStart}");
        Console.WriteLine($"Delivery Time End: {deliveryTimeEnd}\n");

        SerilogLogger logger = new SerilogLogger(logPath);
        OrderController controller = new OrderController(logger);
        controller.ReadOrders(filePath);
        controller.ProcessOrders(filterDistrict, deliveryTimeStart, deliveryTimeEnd);
        controller.WriteFilteredOrders(outPath);
    }
}
