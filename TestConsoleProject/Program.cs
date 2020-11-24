using System;
using EfficientDesigner_Service;
using EfficientDesigner_Service.Models;

namespace TestConsoleProject
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");

            Console.WriteLine("添加 DataSource...");

            //ServiceFactory.GetLayoutService().UpdateDataSource(new DataSource{Api = "https://localhost:5001/efficientdesignapi/getBaiDu" });
            //ServiceFactory.GetLayoutService().UpdateDataSource(new DataSource{Api = "https://localhost:5001/efficientdesignapi/getGoogle" });
            //ServiceFactory.GetLayoutService().UpdateDataSource(new DataSource{Api = "https://localhost:5001/efficientdesignapi/GetMap" });
            //ServiceFactory.GetLayoutService().UpdateDataSource(new DataSource{Api = "https://localhost:5001/efficientdesignapi/GetMetro" });
            //ServiceFactory.GetLayoutService().UpdateDataSource(new DataSource{Api = "https://localhost:5001/efficientdesignapi/GetPCI" });
            //ServiceFactory.GetLayoutService().UpdateDataSource(new DataSource{Api = "https://localhost:5001/efficientdesignapi/GetImage" });
            //ServiceFactory.GetLayoutService().UpdateDataSource(new DataSource { Api = "https://localhost:5001/efficientdesignapi/GetPassengerFlow" });
            ServiceFactory.GetLayoutService().UpdateDataSource(new DataSource { Api = "https://localhost:5001/efficientdesignapi/GetInOutPassengerFlow" });

            Console.WriteLine("添加完毕...");
        }
    }
}
