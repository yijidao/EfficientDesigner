using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EfficientDesigner.Views
{
    /// <summary>
    /// RibbonView.xaml 的交互逻辑
    /// </summary>
    public partial class RibbonView : UserControl
    {
        public RibbonView()
        {
            InitializeComponent();
        }

        public ICommand SaveCommand
        {
            get { return (ICommand)GetValue(SaveCommandProperty); }
            set { SetValue(SaveCommandProperty, value); }
        }

        public static readonly DependencyProperty SaveCommandProperty =
            DependencyProperty.Register("SaveCommand", typeof(ICommand), typeof(RibbonView), new PropertyMetadata(null));



        public ICommand OpenCommand
        {
            get { return (ICommand)GetValue(OpenCommandProperty); }
            set { SetValue(OpenCommandProperty, value); }
        }

        public static readonly DependencyProperty OpenCommandProperty =
            DependencyProperty.Register("OpenCommand", typeof(ICommand), typeof(RibbonView), new PropertyMetadata(null));




        public ICommand SaveAsCommand
        {
            get { return (ICommand)GetValue(SaveAsCommandProperty); }
            set { SetValue(SaveAsCommandProperty, value); }
        }

        public static readonly DependencyProperty SaveAsCommandProperty =
            DependencyProperty.Register("SaveAsCommand", typeof(ICommand), typeof(RibbonView), new PropertyMetadata(null));



        public ICommand PreviewCommand
        {
            get { return (ICommand)GetValue(PreviewCommandProperty); }
            set { SetValue(PreviewCommandProperty, value); }
        }

        public static readonly DependencyProperty PreviewCommandProperty =
            DependencyProperty.Register("PreviewCommand", typeof(ICommand), typeof(RibbonView), new PropertyMetadata(null));


        public ICommand PublishCommand
        {
            get { return (ICommand)GetValue(PublishCommandProperty); }
            set { SetValue(PublishCommandProperty, value); }
        }

        public static readonly DependencyProperty PublishCommandProperty =
            DependencyProperty.Register("PublishCommand", typeof(ICommand), typeof(RibbonView), new PropertyMetadata(null));


        public ICommand GetLayoutsCommand
        {
            get { return (ICommand)GetValue(GetLayoutsCommandProperty); }
            set { SetValue(GetLayoutsCommandProperty, value); }
        }

        public static readonly DependencyProperty GetLayoutsCommandProperty =
            DependencyProperty.Register("GetLayoutsCommand", typeof(ICommand), typeof(RibbonView), new PropertyMetadata(null));



        public ICommand NewCommand
        {
            get { return (ICommand)GetValue(NewCommandProperty); }
            set { SetValue(NewCommandProperty, value); }
        }

        public static readonly DependencyProperty NewCommandProperty =
            DependencyProperty.Register("NewCommand", typeof(ICommand), typeof(RibbonView), new PropertyMetadata(null));



    }
}
