using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EfficientDesigner_Control.Controls
{
    public class WrapPanel : Panel
    {
        /// <summary>
        /// 为true的时候，立刻换行。
        /// </summary>
        public static readonly DependencyProperty LineBreakBeforeProperty = DependencyProperty.RegisterAttached(
            "LineBreakBefore", typeof(bool), typeof(WrapPanel), new PropertyMetadata(default(bool)));

        public static void SetLineBreakBefore(DependencyObject element, bool value)
        {
            element.SetValue(LineBreakBeforeProperty, value);
        }

        public static bool GetLineBreakBefore(DependencyObject element)
        {
            return (bool)element.GetValue(LineBreakBeforeProperty);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var currentSize = new Size(); // 当前行size
            var panelSize = new Size(); // 最终要返回的面板大小

            foreach (UIElement element in InternalChildren)
            {
                element.Measure(availableSize);
                var desiredSize = element.DesiredSize;

                if (GetLineBreakBefore(element) || currentSize.Width + desiredSize.Width > availableSize.Width)
                {
                    panelSize.Width = Math.Max(currentSize.Width, desiredSize.Width);
                    panelSize.Height += currentSize.Height;
                    currentSize = desiredSize;
                    if (currentSize.Width > availableSize.Width)
                    {
                        panelSize.Width = Math.Max(panelSize.Width, currentSize.Width);
                        panelSize.Height += currentSize.Height;
                        currentSize = new Size();
                    }
                }
                else
                {
                    currentSize.Width += desiredSize.Width;
                    currentSize.Height = Math.Max(currentSize.Height, desiredSize.Height);
                }
            }
            return base.MeasureOverride(availableSize);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var firstInLine = 0; // 当前行第一个元素的索引
            var currentSize = new Size(); // 当前行的Size
            var accumulateHeight = 0d; // 当前行之前的累计行高

            for (int i = 0; i < InternalChildren.Count; i++)
            {
                var element = InternalChildren[i];
                var desiredSize = element.DesiredSize;
                if (GetLineBreakBefore(element) || currentSize.Width + desiredSize.Width > finalSize.Width)
                {
                    ArrageLine(accumulateHeight, currentSize.Height, firstInLine++, i);
                    accumulateHeight += currentSize.Height;
                    currentSize = desiredSize;

                    if (desiredSize.Width > finalSize.Width)
                    {
                        ArrageLine(accumulateHeight, currentSize.Height, firstInLine++, ++i);
                        accumulateHeight += currentSize.Height;
                        currentSize = new Size();
                    }
                }
                else
                {
                    currentSize.Height = Math.Max(currentSize.Height, desiredSize.Height);
                    currentSize.Width += desiredSize.Width;
                }
            }

            return finalSize;
        }

        private void ArrageLine(double y, double lineHeight, int start, int end)
        {
            // y 是当前的Y轴点，lineHeight 是当前行高，start 是当前行第一个元素的索引，end 是当前行最后一个元素的索引
            double x = 0;

            for (var i = start; i <= end; i++)
            {
                var element = InternalChildren[i];
                element.Arrange(new Rect(x, y, element.DesiredSize.Width, lineHeight));
                x += element.DesiredSize.Width;
            }

        }
    }
}
