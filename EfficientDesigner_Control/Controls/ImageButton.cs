using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace EfficientDesigner_Control.Controls
{
    public class ImageButton : Control
    {
        private const string TextBlockName = "PART_TextBlock";
        private const string ImageName = "PART_Image";

        public ImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton), new FrameworkPropertyMetadata(typeof(ImageButton)));
        }

        public ButtonType ButtonType
        {
            get { return (ButtonType)GetValue(ButtonTypeProperty); }
            set { SetValue(ButtonTypeProperty, value); }
        }

        public static readonly DependencyProperty ButtonTypeProperty =
            DependencyProperty.Register("ButtonType", typeof(ButtonType), typeof(ImageButton), new PropertyMetadata(ButtonType.查询));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(ImageButton), new PropertyMetadata(null));


        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(ImageButton), new PropertyMetadata(null));



        public string DisplayName
        {
            get { return (string)GetValue(DisplayNameProperty); }
            set { SetValue(DisplayNameProperty, value); }
        }

        public static readonly DependencyProperty DisplayNameProperty =
            DependencyProperty.Register("DisplayName", typeof(string), typeof(ImageButton), new PropertyMetadata(default(string)));

        public event EventHandler CanExecuteChanged;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var textBlock = GetTemplateChild(TextBlockName) as TextBlock;
            if (textBlock != null)
            {
                var binding = new Binding(nameof(DisplayName));
                binding.Source = this;
                binding.Mode = BindingMode.OneWay;
                textBlock.SetBinding(TextBlock.TextProperty, binding);

                if (string.IsNullOrWhiteSpace(DisplayName))
                {
                    DisplayName = ButtonType.ToString();
                }
            }

            var image = GetTemplateChild(ImageName) as Image;
            if(image != null)
            {
                image.Source = GetSource(ButtonType);
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (Command.CanExecute(CommandParameter))
            {
                Command.Execute(CommandParameter);
            }
        }

        private DrawingImage GetSource(ButtonType type)
        {
            switch (type)
            {
                case ButtonType.保存:
                case ButtonType.另存为:
                    return FindResource("Save") as DrawingImage;
                case ButtonType.打开:
                    return FindResource("Open") as DrawingImage;
                case ButtonType.查询:
                default:
                    return FindResource("Query") as DrawingImage;
            }
        }

    }

    public enum ButtonType
    {
        查询 = 0,
        保存,
        另存为,
        打开,
    }
}
