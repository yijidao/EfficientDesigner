using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using EfficientDesigner_Control.Common;
using EfficientDesigner_Control.ViewModels;
using EfficientDesigner_Service;

namespace EfficientDesigner_Control.Views
{
    /// <summary>
    /// PostListView.xaml 的交互逻辑
    /// </summary>
    [ToTest("帖子列表")]
    public partial class PostListView
    {
        public PostListView()
        {
            
            InitializeComponent();
            
            //if (DesignerProperties.GetIsInDesignMode(this))
            //{
            //    DataContext = new PostListViewModel
            //    {
            //        PostItems = new ObservableCollection<PostItenViewModel>
            //        {
            //            new PostItenViewModel("测试数据1"),
            //            new PostItenViewModel("测试数据2"),
            //            new PostItenViewModel("测试数据3"),
            //        }
            //    };
            //}
        }
    }
}
