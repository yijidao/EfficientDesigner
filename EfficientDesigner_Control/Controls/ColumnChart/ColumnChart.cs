using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using EfficientDesigner_Control.Interfaces;
using EfficientDesigner_Service.Models;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using Newtonsoft.Json;

namespace EfficientDesigner_Control.Controls
{
    public class ColumnChart : Control, IHasDisplayMode, IAutoRequest
    {
        private const string TextBlockTitleName = "PART_Title";
        private const string ChartName = "PART_Chart";

        static ColumnChart()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColumnChart), new FrameworkPropertyMetadata(typeof(ColumnChart)));
        }

        public ControlDisplayMode DisplayMode
        {
            get => (ControlDisplayMode)GetValue(DisplayModeProperty);
            set => SetValue(DisplayModeProperty, value);
        }

        public static readonly DependencyProperty DisplayModeProperty =
            DependencyProperty.Register("DisplayMode", typeof(ControlDisplayMode), typeof(ColumnChart), new PropertyMetadata(ControlDisplayMode.Runtime));

        public ControlDisplayMode GetDisplayMode() => DisplayMode;

        public void SetDisplayMode(ControlDisplayMode mode) => DisplayMode = mode;

        public int Interval { get; set; } = 30;

        public CartesianChart Chart
        {
            get => _chart;
            set
            {
                _chart = value;
                if (_chart != null)
                {
                    // 设置横轴
                    _chart.AxisX.Add(new Axis
                    {
                        Title = "时间",
                    });
                    // 设置纵轴
                    _chart.AxisY.Add(new Axis
                    {
                        Title = "人次",
                        LabelFormatter = d => $"{d}万"
                    });
                }
            }
        }

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(ColumnChart), new PropertyMetadata("标题"));

        [BindingApi]
        public string DataSource
        {
            get => (string)GetValue(DataSourceProperty);
            set => SetValue(DataSourceProperty, value);
        }

        public static readonly DependencyProperty DataSourceProperty =
            DependencyProperty.Register("DataSource", typeof(string), typeof(ColumnChart), new PropertyMetadata("", DataSourceChangedCallback));

        private CartesianChart _chart;

        private static void DataSourceChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var value = e.NewValue.ToString();
            var ctl = (ColumnChart)d;

            SetChartData(ctl.Chart, value);
        }

        static void SetChartData(CartesianChart chart, string dataSource)
        {
            if (chart == null) return;

            var chartDates = JsonConvert.DeserializeObject<List<ColumnChartData>>(dataSource);
            var series = new SeriesCollection();
            series.AddRange(chartDates.Select(data => new ColumnSeries
            {
                Title = data.Name,
                Values = new ChartValues<double>(data.DataModels.Select(model => model.Value))
            }));
            var lables = new HashSet<string>();

            foreach (var model in chartDates.SelectMany(data => data.DataModels))
            {
                lables.Add(model.Lable);
            }

            var axisX = chart.AxisX.FirstOrDefault();
            if (axisX != null)
            {
                axisX.Labels = lables.ToList();
            }

            chart.Series = series;

        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var textBlock = GetTemplateChild(TextBlockTitleName) as TextBlock ?? throw new ArgumentException();

            if (DisplayMode == ControlDisplayMode.Runtime)
            {
                // 一定要在运行模式中才给Chart赋值，不然会出现序列化到xaml出错的Bug
                Chart = GetTemplateChild(ChartName) as CartesianChart ?? throw new ArgumentException();
                SetChartData(Chart, DataSource);
            }

            textBlock.SetBinding(TextBlock.TextProperty, new Binding(nameof(ColumnChart.Title))
            {
                Source = this,
            });

        }
    }
}
