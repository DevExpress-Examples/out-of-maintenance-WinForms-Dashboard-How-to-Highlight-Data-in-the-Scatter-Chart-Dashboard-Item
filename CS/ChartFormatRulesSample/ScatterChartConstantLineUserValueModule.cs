using DevExpress.DashboardCommon;
using DevExpress.DashboardWin;
using DevExpress.Utils.Svg;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraCharts;
using DevExpress.XtraDataLayout;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsAppCustomProperties
{
    public class ScatterChartConstantLineUserData
    {
        public bool Enabled { get; set; }
        public double VerticalAxisValue { get; set; }
        public double HorizontalAxisValue { get; set; }

        public string GetStringFromData()
        {
            return Enabled.ToString() + "_" + VerticalAxisValue + "_" + HorizontalAxisValue;
        }
    }
    public class ScatterChartConstantLineUserValueModule
    {
        public static readonly string PropertyName = "ConstantLine";
        readonly DashboardDesigner designer;
        BarButtonItem barItem;
        public ScatterChartConstantLineUserValueModule(DashboardDesigner designer, SvgImage barImage = null)
        {
            this.designer = designer;
            BarButtonItem barItem = AddBarItem("Constant line", barImage, designer.Ribbon);
            barItem.ItemClick += BarItem_Click;
            designer.DashboardItemControlUpdated += Designer_DashboardItemControlUpdated;
            designer.CustomExport += Designer_CustomExport;
        }
        BarButtonItem AddBarItem(string caption, SvgImage barImage, RibbonControl ribbon)
        {
            var page = ribbon.GetDashboardRibbonPage(DashboardBarItemCategory.ScatterChartTools, DashboardRibbonPage.Design);
            RibbonPageGroup group = page.GetGroupByName("Custom Properties");
            if (group == null)
            {
                group = new RibbonPageGroup("Custom Properties") { Name = "Custom Properties" };
                group.AllowTextClipping = false;
                page.Groups.Add(group);
            }
            barItem = new BarButtonItem(ribbon.Manager, caption);
            barItem.ImageOptions.SvgImage = barImage;
            group.ItemLinks.Add(barItem);
            return barItem;
        }
        void BarItem_Click(object sender, ItemClickEventArgs e)
        {
            var chartItem = designer.SelectedDashboardItem as ScatterChartDashboardItem;
            if (chartItem != null)
            {
                var data = GetConstantLineModuleData(chartItem);
                if (data.GetStringFromData() != chartItem.CustomProperties.GetValue(PropertyName))
                    designer.AddToHistory(new CustomPropertyHistoryItem(chartItem, PropertyName, data.GetStringFromData(), $"Constant lines state: {data.Enabled} - {data.VerticalAxisValue} - {data.VerticalAxisValue}"));
            }
        }
        ScatterChartConstantLineUserData GetConstantLineModuleData(ScatterChartDashboardItem dashboardItem)
        {
            var data = GetDataFromString(dashboardItem.CustomProperties.GetValue(PropertyName));
            using (ValueSelectorControl selector = new ValueSelectorControl(data))
            {
                if (XtraDialog.Show(selector, "Adjust constant lines") == DialogResult.OK)
                    return selector.ConstantLineModuleData;
            }
            return null;
        }

        void Designer_CustomExport(object sender, CustomExportEventArgs e)
        {
            Dictionary<string, XRControl> controls = e.GetPrintableControls();
            foreach (KeyValuePair<string, XRControl> pair in controls)
            {
                XRChart xrChart = pair.Value as XRChart;
                if (xrChart != null)
                {
                    string itemComponentName = pair.Key;
                    ScatterChartDashboardItem chart = designer.Dashboard.Items[itemComponentName] as ScatterChartDashboardItem;
                    if (chart != null)
                        UpdateChart(xrChart.Series, chart);
                }
            }
        }
        void Designer_DashboardItemControlUpdated(object sender, DashboardItemControlEventArgs e)
        {
            if (e.ChartControl != null)
            {
                ScatterChartDashboardItem scatterDashboardItem = designer.Dashboard.Items[e.DashboardItemName] as ScatterChartDashboardItem;
                if (scatterDashboardItem != null)
                    UpdateChart(e.ChartControl.Series, scatterDashboardItem);
            }
        }
        void SetUpLine(Axis chartAxis, Color color, Color textColor, double value)
        {
            ConstantLine line = new ConstantLine() { AxisValue = value };
            line.ShowInLegend = false;
            line.Color = color;
            line.LineStyle.Thickness = 2;
            line.LineStyle.DashStyle = DashStyle.Dash;
            line.Title.Text = "Value: " + value.ToString();
            line.Title.TextColor = textColor;
            chartAxis.ConstantLines.Add(line);
        }
        void UpdateChart(SeriesCollection series, ScatterChartDashboardItem chartDashboardItem)
        {
            ScatterChartConstantLineUserData moduleData = GetDataFromString(chartDashboardItem.CustomProperties.GetValue(PropertyName));
            if (moduleData != null && moduleData.Enabled)
            {
                BubbleSeriesView seriesView = series.OfType<Series>().Where(s => s.View is BubbleSeriesView).Select(s => s.View as BubbleSeriesView).FirstOrDefault();
                if (seriesView != null)
                {
                    seriesView.AxisX.ConstantLines.Clear();
                    seriesView.AxisY.ConstantLines.Clear();
                    SetUpLine(seriesView.AxisX, ColorTranslator.FromHtml("#14abb7"), ColorTranslator.FromHtml("#0e9ca9"), moduleData.VerticalAxisValue);
                    SetUpLine(seriesView.AxisY, ColorTranslator.FromHtml("#14abb7"), ColorTranslator.FromHtml("#0e9ca9"), moduleData.HorizontalAxisValue);
                }
            }
        }
        ScatterChartConstantLineUserData GetDataFromString(string customPropertyValue)
        {
            if (!string.IsNullOrEmpty(customPropertyValue))
            {
                var array = customPropertyValue.Split('_');
                return new ScatterChartConstantLineUserData()
                {
                    Enabled = bool.Parse(array[0]),
                    VerticalAxisValue = Convert.ToDouble(array[1]),
                    HorizontalAxisValue = Convert.ToDouble(array[2])
                };
            }
            return null;
        }
    }
    public class ValueSelectorControl : XtraUserControl
    {
        public ScatterChartConstantLineUserData ConstantLineModuleData { get; }

        public ValueSelectorControl(ScatterChartConstantLineUserData data)
        {
            DataLayoutControl dataLayoutControl = new DataLayoutControl();
            BindingSource source = new BindingSource();
            ConstantLineModuleData = data ?? new ScatterChartConstantLineUserData();
            source.DataSource = ConstantLineModuleData;
            dataLayoutControl.DataSource = source;
            dataLayoutControl.RetrieveFields();
            dataLayoutControl.Dock = DockStyle.Fill;
            Controls.Add(dataLayoutControl);
            Dock = DockStyle.Top;
        }
    }
}
