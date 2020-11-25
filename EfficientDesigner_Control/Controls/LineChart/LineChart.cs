using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using EfficientDesigner_Control.Interfaces;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using Newtonsoft.Json;

namespace EfficientDesigner_Control.Controls
{
    public class LineChart : Control, IHasDisplayMode, IAutoRequest
    {
        private const string TextBlockTitleName = "PART_Title";
        private const string ChartName = "PART_Chart";

        static LineChart()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LineChart), new FrameworkPropertyMetadata(typeof(LineChart)));
            Charting.For<DateValueModel>(
                Mappers.Xy<DateValueModel>()
                    .X(model => model.Date.Ticks)
                    .Y(model => model.Value));
        }

        [BindingApi]
        public string DataSource
        {
            get { return (string)GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }

        public static readonly DependencyProperty DataSourceProperty =
            DependencyProperty.Register("DataSource", typeof(string), typeof(LineChart), new PropertyMetadata("", DataSourceChangedCallback));

        private static void DataSourceChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var value = e.NewValue.ToString();
            var ctl = (LineChart)d;
            SetChartData(ctl.Chart, value);
        }

        static void SetChartData(CartesianChart chart, string dataSource)
        {
            if (chart == null) return;

            var chartDates = JsonConvert.DeserializeObject<List<LineChartData>>(dataSource);
            var series = new SeriesCollection();
            series.AddRange(chartDates.Select(data => new LineSeries
            {
                Title = data.Name,
                Values = new ChartValues<DateValueModel>(data.DataModels)
            }));

            
            chart.Series = series;
            
        }

        public ControlDisplayMode DisplayMode
        {
            get { return (ControlDisplayMode)GetValue(DisplayModeProperty); }
            set { SetValue(DisplayModeProperty, value); }
        }

        public static readonly DependencyProperty DisplayModeProperty =
            DependencyProperty.Register("DisplayMode", typeof(ControlDisplayMode), typeof(LineChart), new PropertyMetadata(ControlDisplayMode.Runtime));


        public ControlDisplayMode GetDisplayMode() => DisplayMode;

        public void SetDisplayMode(ControlDisplayMode mode) => DisplayMode = mode;

        /// <summary>
        /// 30分钟
        /// </summary>
        public int Interval { get; set; } = 60 * 30;

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
                        LabelFormatter = d => $"{new DateTime((long)d).Hour}:00",
                        MinValue = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 6, 0, 0).Ticks,
                        MaxValue = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 30, 0).Ticks,
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
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(LineChart), new PropertyMetadata("标题"));

        private CartesianChart _chart;


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

            textBlock.SetBinding(TextBlock.TextProperty, new Binding(nameof(LineChart.Title))
            {
                Source = this,
            });
        }

    }
}
