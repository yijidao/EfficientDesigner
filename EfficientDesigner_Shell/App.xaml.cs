using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CefSharp;
using CefSharp.WinForms;
using EfficientDesigner_Control.Controls;
using EfficientDesigner_Service.ServiceImplements;
using Prism.Ioc;
using Prism.Unity;
using EfficientDesigner_Shell.Views;
using EfficientDesigner_Service.Services;
using HandyControl.Tools;

namespace EfficientDesigner_Shell
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConfigHelper.Instance.SetWindowDefaultStyle();
           
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<ILayoutService, LayoutService>();
        }

        protected override Window CreateShell() => Container.Resolve<MainWindow>();
    }
}
