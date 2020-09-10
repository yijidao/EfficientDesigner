using EfficientDesigner_Control.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace EfficientDesigner_Control.Controls
{
    public class PropertyResolver
    {




        public string ResolveCategory(PropertyDescriptor descriptor) => descriptor.Attributes.OfType<CategoryAttribute>().FirstOrDefault()?.Category ?? default(string);

        public string ResolveDisplayName(PropertyDescriptor descriptor) => string.IsNullOrWhiteSpace(descriptor.DisplayName) ? descriptor.Name : descriptor.DisplayName;

        public string ResolveDescription(PropertyDescriptor descriptor) => descriptor.Description;

        public bool ResolveIsBrowsable(PropertyDescriptor descriptor) => descriptor.IsBrowsable;

        public bool ResolveIsReadOnly(PropertyDescriptor descriptor) => descriptor.IsReadOnly;

        public object ResolveDefaultValue(PropertyDescriptor descriptor) => descriptor.Attributes.OfType<DefaultValueAttribute>().FirstOrDefault()?.Value;

        public PropertyEditorBase ResolveEditor(PropertyDescriptor descriptor) => TypeCodeDic.TryGetValue(descriptor.PropertyType, out var code)
            ? code switch
            {
                EditorTypeCode.PlainText => new PlainTextPropertyEditor(),
                EditorTypeCode.SByteNumber => new NumberPropertyEditor(sbyte.MinValue, sbyte.MaxValue),
                EditorTypeCode.ByteNumber => new NumberPropertyEditor(byte.MinValue, byte.MaxValue),
                EditorTypeCode.Int16Number => new NumberPropertyEditor(short.MinValue, short.MaxValue),
                EditorTypeCode.UInt16Number => new NumberPropertyEditor(ushort.MinValue, ushort.MaxValue),
                EditorTypeCode.Int32Number => new NumberPropertyEditor(int.MinValue, int.MaxValue),
                EditorTypeCode.UInt32Number => new NumberPropertyEditor(uint.MinValue, uint.MaxValue),
                EditorTypeCode.Int64Number => new NumberPropertyEditor(long.MinValue, long.MaxValue),
                EditorTypeCode.UInt64Number => new NumberPropertyEditor(ulong.MinValue, ulong.MaxValue),
                EditorTypeCode.SingleNumber => new NumberPropertyEditor(float.MinValue, float.MaxValue),
                EditorTypeCode.DoubleNumber => new NumberPropertyEditor(double.MinValue, double.MaxValue),
                EditorTypeCode.Switch => new SwitchPropertyEditor(),
                EditorTypeCode.DateTime => new DateTimePropertyEditor(),
                //EditorTypeCode.HorizontalAlignment => new HorizontalAlignmentPropertyEditor(),
                //EditorTypeCode.VerticalAlignment => new VerticalAlignmentPropertyEditor(),
                EditorTypeCode.ImageSource => new ImagePropertyEditor(),
                EditorTypeCode.Brush => new BrushPropertyEditor(),
                EditorTypeCode.Thickness => new ThicknessPropertyEditor(),
                EditorTypeCode.FontFamily => new FontFamilyPropertyEditor(),
                EditorTypeCode.FontStretch => new FontStretchPropertyEditor(),
                EditorTypeCode.FontWeight => new FontWeightPropertyEditor(),
                _ => new ReadOnlyTextPropertyEditor()
            }
            : descriptor.PropertyType.IsSubclassOf(typeof(Enum))
                ? (PropertyEditorBase)new EnumPropertyEditor()
                : (PropertyEditorBase)new ReadOnlyTextPropertyEditor();
        private static readonly Dictionary<Type, EditorTypeCode> TypeCodeDic = new Dictionary<Type, EditorTypeCode>
        {
            [typeof(string)] = EditorTypeCode.PlainText,
            [typeof(sbyte)] = EditorTypeCode.SByteNumber,
            [typeof(byte)] = EditorTypeCode.ByteNumber,
            [typeof(short)] = EditorTypeCode.Int16Number,
            [typeof(ushort)] = EditorTypeCode.UInt16Number,
            [typeof(int)] = EditorTypeCode.Int32Number,
            [typeof(uint)] = EditorTypeCode.UInt32Number,
            [typeof(long)] = EditorTypeCode.Int64Number,
            [typeof(ulong)] = EditorTypeCode.UInt64Number,
            [typeof(float)] = EditorTypeCode.SingleNumber,
            [typeof(double)] = EditorTypeCode.DoubleNumber,
            [typeof(bool)] = EditorTypeCode.Switch,
            [typeof(DateTime)] = EditorTypeCode.DateTime,
            //[typeof(HorizontalAlignment)] = EditorTypeCode.HorizontalAlignment,
            //[typeof(VerticalAlignment)] = EditorTypeCode.VerticalAlignment,
            [typeof(ImageSource)] = EditorTypeCode.ImageSource,
            [typeof(Brush)] = EditorTypeCode.Brush,
            [typeof(Thickness)] = EditorTypeCode.Thickness,
            [typeof(FontFamily)] = EditorTypeCode.FontFamily,
            [typeof(FontStretch)] = EditorTypeCode.FontStretch,
            [typeof(FontWeight)] = EditorTypeCode.FontWeight,
        };

        private enum EditorTypeCode
        {
            PlainText,
            SByteNumber,
            ByteNumber,
            Int16Number,
            UInt16Number,
            Int32Number,
            UInt32Number,
            Int64Number,
            UInt64Number,
            SingleNumber,
            DoubleNumber,
            Switch,
            DateTime,
            //HorizontalAlignment,
            //VerticalAlignment,
            ImageSource,
            Brush,
            Thickness,
            FontFamily,
            FontStretch,
            FontWeight
        }
    }
}
