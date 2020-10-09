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

            // 对Winfrom控件启用系统自带得现代化样式
            //System.Windows.Forms.Application.EnableVisualStyles();

            //var settings = new CefSettings();
            //settings.BrowserSubprocessPath = System.IO.Path.GetFullPath(@"x86\CefSharp.BrowserSubprocess.exe");
            //Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //throw new NotImplementedException();
        }
    }
}
