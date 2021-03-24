using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using EfficientDesigner_Common.ServiceImps;
using EfficientDesigner_Common.Services;
using Prism.Ioc;
using Prism.Unity;

namespace TextProject
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //var settings = new CefSettings();
            //settings.BrowserSubprocessPath = System.IO.Path.GetFullPath(@"x86\CefSharp.BrowserSubprocess.exe");
            //Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);


        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<ILayoutService, LayoutService>();
        }

        protected override Window CreateShell() => new MainWindow();
    }
}
