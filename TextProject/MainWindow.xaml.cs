using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using Prism.Commands;
using Prism.Mvvm;

namespace TextProject
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();

            var assembly = Assembly.LoadFrom("EfficientDesigner_Control.dll");
            var list = new List<ToTestViewItem>();
            foreach (var type in assembly.GetTypes())
            {
                if (Attribute.GetCustomAttribute(type, typeof(ToTestAttribute)) is ToTestAttribute toTestAttribute)
                {
                    list.Add(new ToTestViewItem(type, toTestAttribute));
                }
            }

            viewList.ItemsSource = list;
        }
    }


    class ToTestViewItem : BindableBase
    {
        private readonly Type _type;
        private readonly ToTestAttribute _toTestAttribute;

        public string Name { get; }


        public ToTestViewItem(Type type, ToTestAttribute toTestAttribute)
        {
            _type = type;
            _toTestAttribute = toTestAttribute;
            Name = toTestAttribute != null && toTestAttribute.Name == string.Empty ? _type.Name : _toTestAttribute.Name;

            OpenViewCommand = new DelegateCommand(OpenView);

            void OpenView()
            {
                var window = new Window { Content = Activator.CreateInstance(_type), Background = Brushes.White };
                window.Show();
            }
        }

        public ICommand OpenViewCommand { get; }
    }
}
