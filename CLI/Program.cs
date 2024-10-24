using BL.DataGenerators;
using BL.Models;
using BL.Services;
using CLI;
using CLI.Controllers;
using Serilog.Core;
using System.Runtime.CompilerServices;

class Program
{
    static void Main(string[] args)
    {
        string filePath = "undefined";
        string logPath = "undefined";
        string outPath = "undefined";
        string filterArea = "undefined";
        DateTime deliveryTimeStart = DateTime.Now;
        DateTime? deliveryTimeEnd = null;

        for (int i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "-d":
                case "-dist":
                case "-district":
                    if (i + 1 < args.Length)
                    {
                        filterArea = args[++i];
                    }
                    break;

                case "-t":
                case "-time":
                case "-deliveryTime":
                    List<DateTime> deliveryTimes = new List<DateTime>();

                    while (i + 1 < args.Length)
                    {
                        string dateTimeString = $"{args[i + 1]} {args[i + 2]}";

                        if (DateTime.TryParse(dateTimeString, out var parsedTime))
                        {
                            deliveryTimes.Add(parsedTime);
                            i += 2;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (deliveryTimes.Count > 0)
                    {
                        deliveryTimeStart = deliveryTimes[0];
                    }
                    if (deliveryTimes.Count > 1)
                    {
                        deliveryTimeEnd = deliveryTimes[1];
                    }
                    break;

                case "-f":
                case "-file":
                case "-filePath":
                    if (i + 1 < args.Length)
                    {
                        filePath = args[++i];
                    }
                    break;

                case "-l":
                case "-log":
                case "-logFile":
                    if (i + 1 < args.Length)
                    {
                        logPath = args[++i];
                    }
                    break;

                case "-o":
                case "-out":
                case "-output":
                    if (i + 1 < args.Length)
                    {
                        outPath = args[++i];
                    }
                    break;
            }
        }

        SerilogLogger logger = new SerilogLogger(logPath);
        OrderController controller = new OrderController(logger);
        controller.ReadOrders(filePath);
        controller.ProcessOrders(filterArea, deliveryTimeStart, deliveryTimeEnd);
        controller.WriteFilteredOrders(outPath);
    }
}
