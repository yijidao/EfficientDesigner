using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using EfficientDesigner_Service.ServiceImplements;
using Prism.Ioc;
using Prism.Unity;
using EfficientDesigner_Shell.Views;
using EfficientDesigner_Service.Services;

namespace EfficientDesigner_Shell
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<ILayoutService, LayoutService>();
        }

        protected override Window CreateShell() => new MainWindow();
    }
}
