using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Management;
using System.Threading.Tasks;
using System.Windows;
using CefSharp;
using CefSharp.WinForms;
using HandyControl.Data;
using HandyControl.Tools;
using Prism.Ioc;
using Prism.Unity;
using MessageBox = HandyControl.Controls.MessageBox;

namespace EfficientDesigner
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : PrismApplication
    {

        protected override Window CreateShell()
        {
            var w = Container.Resolve<MainWindow>();
            return w;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConfigHelper.Instance.SetWindowDefaultStyle();
            DispatcherUnhandledException += (s, args) =>
            {
                MessageBox.Show(args.Exception.ToString());
                args.Handled = true;
            };
        }


        protected override void ConfigureViewModelLocator()
        {
            //base.ConfigureViewModelLocator();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //throw new NotImplementedException();
        }
    }
}
