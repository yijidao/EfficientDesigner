using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using CefSharp;
using CefSharp.WinForms;

namespace EfficientDesigner_Control.Controls
{
    public class WebBrowser : Control
    {
        private const string HostName = "PART_Host";

        static WebBrowser()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WebBrowser), new FrameworkPropertyMetadata(typeof(WebBrowser)));
            Cef.EnableHighDPISupport();

            var settings = new CefSettings()
            {
                //By default CefSharp will use an in-memory cache, you need to specify a Cache Folder to persist data
                CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Cache")
            };
            settings.CefCommandLineArgs.Add("enable-media-stream");
            //https://peter.sh/experiments/chromium-command-line-switches/#use-fake-ui-for-media-stream
            settings.CefCommandLineArgs.Add("use-fake-ui-for-media-stream");
            //For screen sharing add (see https://bitbucket.org/chromiumembedded/cef/issues/2582/allow-run-time-handling-of-media-access#comment-58677180)
            settings.CefCommandLineArgs.Add("enable-usermedia-screen-capturing");
            Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Host = GetTemplateChild(HostName) as System.Windows.Forms.Integration.WindowsFormsHost ?? throw new ArgumentException();
            //InitCef();
            Host.Child = new ChromiumWebBrowser(Url);
        }

        //public static bool CefInitialized { get; set; }

        /// <summary>
        /// AnyCPU时初始化配置
        /// </summary>
        //private void InitCef()
        //{
        //    if (CefInitialized) return;
        //    var settings = new CefSettings
        //    {
        //        BrowserSubprocessPath = System.IO.Path.GetFullPath(@"x86\CefSharp.BrowserSubprocess.exe")
        //    };
        //    Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);
        //    CefInitialized = true;
        //}

        private WindowsFormsHost Host { get; set; }

        public string Url
        {
            get => (string)GetValue(UrlProperty);
            set => SetValue(UrlProperty, value);
        }

        public static readonly DependencyProperty UrlProperty =
            DependencyProperty.Register("Url", typeof(string), typeof(WebBrowser), new PropertyMetadata("www.baidu.com", UrlChangedCallback));

        private static void UrlChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is string url)) return;
            var ctl = d as WebBrowser;
            if (!(ctl?.Host.Child is ChromiumWebBrowser browser)) return;
            browser.Load(url);
        }
    }
}
