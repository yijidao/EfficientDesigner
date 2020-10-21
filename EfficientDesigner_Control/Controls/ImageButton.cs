using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;

namespace EfficientDesigner_Control.Controls
{
    public class ImageButton : Control
    {
        private const string TextBlockName = "PART_TextBlock";
        private const string ImageName = "PART_Image";

        static ImageButton()
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
            DependencyProperty.Register("Command", typeof(ICommand), typeof(ImageButton), new PropertyMetadata(null, OnCommandChanged));


        private static void OnCommandChanged(DependencyObject dp, DependencyPropertyChangedEventArgs args)
        {
            var b = (ImageButton)dp;
            b.OnCommandChanged((ICommand)args.OldValue, (ICommand)args.NewValue);
        }

        private void OnCommandChanged(ICommand oldCommand, ICommand newCommand)
        {
            if (oldCommand != null)
                UnHookCommand(oldCommand);
            if (newCommand != null)
                HookCommand(newCommand);

            UpdateCanExecute();
        }

        private void HookCommand(ICommand command)
        {
            CanExecuteChangedEventManager.AddHandler(command, OnCanExecuteChanged);
        }

        private void UnHookCommand(ICommand command)
        {
            CanExecuteChangedEventManager.RemoveHandler(command, OnCanExecuteChanged);
        }

        private void OnCanExecuteChanged(object sender, EventArgs e)
        {
            UpdateCanExecute();
        }

        /// <summary>
        /// 根据 Command 的 CanExecute 更新 IsEnaled
        /// </summary>
        private void UpdateCanExecute()
        {
            if (Command != null)
            {
                IsEnabled = Command.CanExecute(CommandParameter);
            }
        }

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
            if (image != null)
            {
                //var resources = new ResourceDictionary { Source = new Uri("/EfficientDesigner_Control;component/Themes/Icon.xaml", UriKind.Absolute) };
                image.Source = GetSource(ButtonType);
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            IsPressed = true;
            if (Command?.CanExecute(CommandParameter) == true)
            {
                Command.Execute(CommandParameter);
            }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            IsPressed = false;
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            IsPressed = false;
        }

        private DrawingImage GetSource(ButtonType type)
        {
            switch (type)
            {
                case ButtonType.保存:
                case ButtonType.另存:
                    return Application.Current.Resources["Save"] as DrawingImage;
                case ButtonType.打开:
                    return Application.Current.Resources["Open"] as DrawingImage;
                case ButtonType.预览:
                    return Application.Current.Resources["Preview"] as DrawingImage;
                case ButtonType.撤销:
                    return Application.Current.Resources["Cancel"] as DrawingImage;
                case ButtonType.重做:
                    return Application.Current.Resources["Reset"] as DrawingImage;
                case ButtonType.发布:
                    return Application.Current.Resources["Publish"] as DrawingImage;
                case ButtonType.查询:
                default:
                    return Application.Current.Resources["Query"] as DrawingImage;
            }
        }


        public bool IsPressed
        {
            get { return (bool)GetValue(IsPressedProperty); }
            set { SetValue(IsPressedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsPressed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsPressedProperty =
            DependencyProperty.Register("IsPressed", typeof(bool), typeof(ImageButton), new PropertyMetadata(false));


        //protected override void OnMouseEnter(MouseEventArgs e)
        //{
        //    base.OnMouseEnter(e);
        //    Debug.WriteLine($"{IsMouseOver}");
        //}
        //protected override void OnMouseLeave(MouseEventArgs e)
        //{
        //    base.OnMouseLeave(e);
        //    Debug.WriteLine($"{IsMouseOver}");
        //}
    }

    public enum ButtonType
    {
        查询 = 0,
        保存,
        另存,
        打开,
        预览,
        撤销,
        重做,
        发布,
    }
}
