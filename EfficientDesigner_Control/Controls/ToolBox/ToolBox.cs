using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using EfficientDesigner_Control.Interfaces;

namespace EfficientDesigner_Control.Controls
{
    public class ToolBox : Control
    {
        private const string TreeViewName = "PART_TreeView";

        static ToolBox()
        {
            Add<Rectangle>();
            Add<Button>();
            Add<TextBox>();
            Add<WebBrowser>();
        }

        public ToolBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ToolBox), new FrameworkPropertyMetadata(typeof(ToolBox)));
        }

        private static List<Type> BoxItems { get; set; } = new List<Type>();

        public static void Add<T>() where T : FrameworkElement
        {
            if (!BoxItems.Contains(typeof(T)))
            {
                BoxItems.Add(typeof(T));
            }
        }

        public override void OnApplyTemplate()
        {
            ContentTreeView = GetTemplateChild(TreeViewName) as TreeView ?? throw new ArgumentException();

            ContentTreeView.AddHandler(MouseMoveEvent, new MouseEventHandler(ContentTreeView_MouseMove));

            var list = new List<ControlCategory>();
            var category = new ControlCategory("所有控件");
            category.Details.AddRange(BoxItems.Select(x => new ControlDetail(x)));
            list.Add(category);

            var executingAssembly = Assembly.GetExecutingAssembly();
            var category2 = new ControlCategory("自定义控件");
            category2.Details.AddRange(BoxItems.Where(x => x.Assembly == executingAssembly).Select(x => new ControlDetail(x)));
            list.Add(category2);

            ContentTreeView.ItemsSource = list;
        }

        private void ContentTreeView_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && ContentTreeView.SelectedItem is ControlDetail controlItem)
            {
                var data = new DataObject();
                data.SetData("control", controlItem);
                DragDrop.DoDragDrop(sender as DependencyObject, data, DragDropEffects.Copy);
            }
        }

        public TreeView ContentTreeView { get; set; }
    }

    internal class ControlCategory
    {
        public string DisplayName { get; set; }

        public List<ControlDetail> Details { get; set; } = new List<ControlDetail>();

        public ControlCategory(string displayName)
        {
            DisplayName = displayName;
        }
    }

    internal class ControlDetail
    {
        private string _displayName;
        public Type ControlType { get; set; }

        public string DisplayName
        {
            get => !string.IsNullOrWhiteSpace(_displayName) ? _displayName : ControlType?.Name;
            set => _displayName = value;
        }

        public ControlDetail(Type controlType, string displayName = null)
        {
            ControlType = controlType;
            DisplayName = displayName;
        }

        public FrameworkElement GetElement()
        {
            if (!(Activator.CreateInstance(ControlType) is FrameworkElement element))
            {
                throw new Exception($"{ControlType.Name} 无法转型为 {nameof(FrameworkElement)}");
            }

            //switch (ControlType)
            //{
            //    ControlType.is
            //}

            if (element is WebBrowser webBrowser)
            {
                webBrowser.Background = Brushes.CornflowerBlue;
                webBrowser.DisplayMode = ControlDisplayMode.Design;
            }
            else if (ControlType.IsSubclassOf(typeof(Shape)))
            {
                (element as Shape).Stroke = Brushes.Black;
                (element as Shape).StrokeThickness = 2;
                (element as Shape).Fill = Brushes.Transparent;
            }

            element.Height = 100;
            element.Width = 100;
            return element;
        }

        //private FrameworkElement GetElement(Type type) => type switch
        //{
            

        //};

    }
}
