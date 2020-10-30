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
            
            ServiceFactory.GetLayoutService().UpdateDataSource(new DataSource{Api = "www.baidu.com"});
            ServiceFactory.GetLayoutService().UpdateDataSource(new DataSource{Api = "www.google.com"});

            Console.WriteLine("添加完毕...");
        }
    }
}
