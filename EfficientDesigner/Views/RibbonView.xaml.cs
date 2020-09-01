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

        public ICommand LoadCommand
        {
            get { return (ICommand)GetValue(LoadCommandProperty); }
            set { SetValue(LoadCommandProperty, value); }
        }

        public static readonly DependencyProperty LoadCommandProperty =
            DependencyProperty.Register("LoadCommand", typeof(ICommand), typeof(RibbonView), new PropertyMetadata(null));


    }
}
