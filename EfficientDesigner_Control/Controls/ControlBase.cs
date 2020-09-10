using EfficientDesigner_Control.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EfficientDesigner_Control.Controls
{
    public abstract class ControlBase : IControl
    {
        public abstract string Title { get; }

        public abstract FrameworkElement GetElement();

        //private PropertyInfo PropertyInfos { get; set; }

        //private static PropertyInfo[] _PropertyInfos;

        //public virtual PropertyInfo[] PropertyInfos
        //{
        //    get
        //    {
        //        if (_PropertyInfos == null)
        //        {

        //        }

        //        return _PropertyInfos;
        //    }
        //    set
        //    {
        //        _PropertyInfos = value;
        //    }
        //}

        //public virtual PropertyInfo[] GetPropertyInfos()
        //{
        //    if()
        //}

        private PropertyInfo[] _PropertyInfos;

        public PropertyInfo[] PropertyInfos
        {
            get { return _PropertyInfos; }
            set { _PropertyInfos = value; }
        }

        private void GetPropertyInfos()
        {
            //var names = new string[]
            //{
            //    nameof(FrameworkElement.)
            //};

            //typeof(FrameworkElement).GetProperties

            //typeof(FrameworkElement).GetProperty(nameof())
            //FontStretch
            //FontWeight
            //FontStyles
            //Fonts.SystemFontFamilies;
        }

    }
}
