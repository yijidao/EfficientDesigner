using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EfficientDesigner_Control.Controls
{
    public class ThicknessGrid : Control
    {
        private const string LeftName = "PART_Left";
        private const string RightName = "PART_Right";
        private const string TopName = "PART_Top";
        private const string BottomName = "PART_Bottom";

        public ThicknessGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ThicknessGrid), new FrameworkPropertyMetadata(typeof(ThicknessGrid)));
        }
        public Thickness Value
        {
            get { return (Thickness)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(Thickness), typeof(ThicknessGrid), new PropertyMetadata(null,
                (dp, e) =>
                {
                    var ctl = (ThicknessGrid)dp;
                    var value = (Thickness)e.NewValue;

                    if (ctl.Left == null || ctl.Right == null || ctl.Top == null || ctl.Bottom == null) return;

                    ctl.ValueChanging = true;
                    ctl.Left.Text = value.Left.ToString();
                    ctl.Right.Text = value.Right.ToString();
                    ctl.Top.Text = value.Top.ToString();
                    ctl.Bottom.Text = value.Bottom.ToString();
                    ctl.ValueChanging = false;
                }));

        public TextBox Left { get; set; }
        public TextBox Right { get; set; }
        public TextBox Top { get; set; }
        public TextBox Bottom { get; set; }

        public bool ValueChanging { get; set; }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Left = GetTemplateChild(LeftName) as TextBox ?? throw new ArgumentException();
            Right = GetTemplateChild(RightName) as TextBox ?? throw new ArgumentException();
            Top = GetTemplateChild(TopName) as TextBox ?? throw new ArgumentException();
            Bottom = GetTemplateChild(BottomName) as TextBox ?? throw new ArgumentException();

            Left.PreviewTextInput += PreviewTextInputHandle;
            Right.PreviewTextInput += PreviewTextInputHandle;
            Top.PreviewTextInput += PreviewTextInputHandle;
            Bottom.PreviewTextInput += PreviewTextInputHandle;

            DataObject.AddPastingHandler(Left, TextBoxPasting);
            DataObject.AddPastingHandler(Right, TextBoxPasting);
            DataObject.AddPastingHandler(Top, TextBoxPasting);
            DataObject.AddPastingHandler(Bottom, TextBoxPasting);

            Left.TextChanged += (sender, e) =>
            {
                if (ValueChanging) return;

                if (double.TryParse(Left.Text, out var d))
                    Value = new Thickness { Left = d, Bottom = Value.Bottom, Right = Value.Right, Top = Value.Top };
            };

            Right.TextChanged += (sender, e) =>
            {
                if (ValueChanging) return;

                if (double.TryParse(Right.Text, out var d))
                    Value = new Thickness { Left = Value.Left, Right = d, Bottom = Value.Bottom, Top = Value.Top };
            };

            Top.TextChanged += (sender, e) =>
            {
                if (ValueChanging) return;

                if (double.TryParse(Top.Text, out var d))
                    Value = new Thickness { Left = Value.Left, Top = d, Bottom = Value.Bottom, Right = Value.Right };
            };

            Bottom.TextChanged += (sender, e) =>
            {
                if (ValueChanging) return;

                if (double.TryParse(Bottom.Text, out var d))
                    Value = new Thickness { Left = Value.Left, Bottom = d, Right = Value.Right, Top = Value.Top };
            };
        }

        private bool IsTextAllowed(string text) => double.TryParse(text, out var d);

        /// <summary>
        /// 输入检测
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PreviewTextInputHandle(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        /// <summary>
        /// 粘贴检测
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                var text = (String)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }

        }
    }
}
