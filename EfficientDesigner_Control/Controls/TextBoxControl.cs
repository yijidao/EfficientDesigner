﻿using EfficientDesigner_Control.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EfficientDesigner_Control.Controls
{
    public class TextBoxControl : IControl
    {
        public string Title => "TextBox";

        public FrameworkElement GetElement()
        {
            return new TextBox
            {
                Text = Title,
                Height = 40,
                Width = 100
            };
        }
    }
}