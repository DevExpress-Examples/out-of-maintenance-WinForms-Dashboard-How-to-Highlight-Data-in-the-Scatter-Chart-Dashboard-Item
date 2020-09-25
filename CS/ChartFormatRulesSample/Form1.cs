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
            AddFormatRulesToScatterChart(scatterChart1);
            new ScatterChartConstantLineUserValueModule(dashboardDesigner1, svgImageCollection1["chartrangearea"]);
           
        }
        public void AddFormatRulesToScatterChart(ScatterChartDashboardItem scatterChart)
        {
            double unitCountThreshold = 7000;
            double discountThreshold = 18;
            ScatterChartItemFormatRule expressionRule1 = new ScatterChartItemFormatRule();
            expressionRule1.DataItem = scatterChart.AxisYMeasure;
            FormatConditionExpression formatCondition = new FormatConditionExpression();
            formatCondition.Expression = $"{scatterChart.AxisYMeasure.UniqueId} > {unitCountThreshold} && {scatterChart.AxisXMeasure.UniqueId} > {discountThreshold}";
            formatCondition.StyleSettings = new ColorStyleSettings(ColorTranslator.FromHtml("#14abb7"));
            expressionRule1.Condition = formatCondition;
            expressionRule1.ShowInLegend = true;
            expressionRule1.DisplayName = "Discount amount from the quantity of products sold";
            scatterChart.FormatRules.Add(expressionRule1);
            ScatterChartConstantLineUserData moduleData = new ScatterChartConstantLineUserData() { Enabled = true, VerticalAxisValue = discountThreshold, HorizontalAxisValue = unitCountThreshold };
            scatterChart.CustomProperties.SetValue(ScatterChartConstantLineUserValueModule.PropertyName, moduleData.GetStringFromData());
        }
    }
}
