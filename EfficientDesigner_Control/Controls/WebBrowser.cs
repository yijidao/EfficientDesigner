using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
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

            // 对Winfrom控件启用系统自带得现代化样式
            System.Windows.Forms.Application.EnableVisualStyles();

            Cef.EnableHighDPISupport();

            InitCef();
        }



        public ControlDisplayMode DisplayMode
        {
            get { return (ControlDisplayMode)GetValue(DisplayModeProperty); }
            set { SetValue(DisplayModeProperty, value); }
        }

        public static readonly DependencyProperty DisplayModeProperty =
            DependencyProperty.Register("DisplayMode", typeof(ControlDisplayMode), typeof(WebBrowser), new PropertyMetadata(ControlDisplayMode.Runtime));



        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            switch (DisplayMode)
            {
                case ControlDisplayMode.Runtime:
                    Host = GetTemplateChild(HostName) as System.Windows.Forms.Integration.WindowsFormsHost ?? throw new ArgumentException();
                    Host.Child = new ChromiumWebBrowser(Url);
                    break;
                case ControlDisplayMode.Design:
                    HostText = GetTemplateChild(HostName) as TextBlock ?? throw new ArgumentException();
                    HostText.Text = Url;
                    break;
            }
        }

        /// <summary>
        /// AnyCPU时初始化配置
        ///
        /// 1. 首先需要在项目的 csproj 文件中的第一个 <PropertyGroup> 中增加 <CefSharpAnyCpuSupport>true</CefSharpAnyCpuSupport>
        /// 2. 在 app.config 中添加
        /// <runtime>
        /// <assemblyBinding xmlns = "urn:schemas-microsoft-com:asm.v1" >
        ///     < probing privatePath="x86"/>
        ///     </assemblyBinding>
        /// </runtime>
        /// 3. 设置 32 位优先
        /// 4. 设置 settings.BrowserSubprocessPath，并且在之后调用 Cef.Initialize
        /// 如何配置 cefsharp 为 anycpu，请参考：https://github.com/cefsharp/CefSharp/issues/1714
        /// </summary>
        private static void InitCef()
        {
            var settings = new CefSettings();

            // Set BrowserSubProcessPath based on app bitness at runtime
            settings.BrowserSubprocessPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                Environment.Is64BitProcess ? "x64" : "x86",
                "CefSharp.BrowserSubprocess.exe");

            // Make sure you set performDependencyCheck false
            Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);
        }

        private WindowsFormsHost Host { get; set; }

        private TextBlock HostText { get; set; }

        public string Url
        {
            get => (string)GetValue(UrlProperty);
            set => SetValue(UrlProperty, value);
        }

        public static readonly DependencyProperty UrlProperty =
            DependencyProperty.Register("Url", typeof(string), typeof(WebBrowser), new PropertyMetadata("网页地址", UrlChangedCallback));

        private static void UrlChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is string url)) return;
            var ctl = d as WebBrowser;

            switch (ctl.DisplayMode)
            {
                case ControlDisplayMode.Design:
                    ctl.HostText.Text = url;
                    break;
                case ControlDisplayMode.Runtime:
                    if (!(ctl?.Host?.Child is ChromiumWebBrowser browser)) return;
                    browser.Load(url);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public enum ControlDisplayMode
    {
        Design,
        Runtime
    }
}
