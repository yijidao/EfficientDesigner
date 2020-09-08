﻿using EfficientDesigner_Control.Controls.Editors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EfficientDesigner_Control.Controls
{
    public class PropertyItem : Control
    {


        public string DisplayName
        {
            get { return (string)GetValue(DisplayNameProperty); }
            set { SetValue(DisplayNameProperty, value); }
        }

        public static readonly DependencyProperty DisplayNameProperty = DependencyProperty.Register("DisplayName", typeof(string), typeof(PropertyItem), new PropertyMetadata(default(string)));

        public FrameworkElement EditorElement
        {
            get { return (FrameworkElement)GetValue(EditorElementProperty); }
            set { SetValue(EditorElementProperty, value); }
        }

        public static readonly DependencyProperty EditorElementProperty =
            DependencyProperty.Register("EditorElement", typeof(FrameworkElement), typeof(PropertyItem), new PropertyMetadata(default(string)));

        public static double GetDisplayNameWidth(DependencyObject obj)
        {
            return (double)obj.GetValue(DisplayNameWidthProperty);
        }

        public static void SetDisplayNameWidth(DependencyObject obj, double value)
        {
            obj.SetValue(DisplayNameWidthProperty, value);
        }

        public static readonly DependencyProperty DisplayNameWidthProperty =
            DependencyProperty.RegisterAttached("DisplayNameWidth", typeof(double), typeof(PropertyItem), new PropertyMetadata(150d));

        public string Category
        {
            get { return (string)GetValue(CategoryProperty); }
            set { SetValue(CategoryProperty, value); }
        }

        public static readonly DependencyProperty CategoryProperty =
            DependencyProperty.Register("Category", typeof(string), typeof(PropertyItem), new PropertyMetadata(default));


        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(PropertyItem), new PropertyMetadata(default));

        public string PropertyTypeName { get; set; }

        public PropertyEditorBase Editor { get; set; }
    }
}