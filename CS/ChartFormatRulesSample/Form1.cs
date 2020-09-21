using DevExpress.DashboardCommon;
using DevExpress.XtraEditors;
using System.Drawing;
using WindowsFormsAppCustomProperties;

namespace ScatterChartFormatRulesSample
{
    public partial class Form1 : XtraForm {
        public Form1() {
            InitializeComponent();
            dashboardDesigner1.CreateRibbon();
            dashboardDesigner1.LoadDashboard(@"../../nwindDashboard.xml");
            ScatterChartDashboardItem scatterChart1 = (ScatterChartDashboardItem)dashboardDesigner1.Dashboard.Items["scatterChartDashboardItem1"];
            AddFormatRulesToBarSeries(scatterChart1);
            new ScatterChartConstantLineUserValueModule(dashboardDesigner1, svgImageCollection1["chartrangearea"]);
        }
        public void AddFormatRulesToBarSeries(ScatterChartDashboardItem scatterChart) {
            double yValue = 7000;
            double xValue = 18;
            ScatterChartItemFormatRule valueRule1 = new ScatterChartItemFormatRule();
            valueRule1.DataItem = scatterChart.AxisYMeasure;
            FormatConditionExpression formatCondition = new FormatConditionExpression();
            formatCondition.Expression = $"{scatterChart.AxisYMeasure.UniqueId} > {yValue} && {scatterChart.AxisXMeasure.UniqueId} > {xValue}";
            formatCondition.StyleSettings = new ColorStyleSettings(ColorTranslator.FromHtml("#14abb7"));
            valueRule1.Condition = formatCondition;
            valueRule1.ShowInLegend = true;
            valueRule1.DisplayName = "Discount amount from the quantity of products sold";
            scatterChart.FormatRules.Add(valueRule1);

            ScatterChartConstantLineUserData moduleData = new ScatterChartConstantLineUserData() { Enabled = true, VerticalAxisValue = xValue, HorizontalAxisValue = yValue };
            scatterChart.CustomProperties.SetValue(ScatterChartConstantLineUserValueModule.PropertyName, moduleData.GetStringFromData());
        }
    }
}
