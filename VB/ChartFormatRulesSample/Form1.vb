Imports Microsoft.VisualBasic
Imports DevExpress.DashboardCommon
Imports DevExpress.XtraEditors
Imports System.Drawing
Imports WindowsFormsAppCustomProperties

Namespace ScatterChartFormatRulesSample
	Partial Public Class Form1
		Inherits XtraForm
		Public Sub New()
			InitializeComponent()
			dashboardDesigner1.CreateRibbon()
			dashboardDesigner1.LoadDashboard("../../nwindDashboard.xml")
			Dim scatterChart1 As ScatterChartDashboardItem = CType(dashboardDesigner1.Dashboard.Items("scatterChartDashboardItem1"), ScatterChartDashboardItem)
			AddFormatRulesToScatterChart(scatterChart1)
			Dim TempScatterChartConstantLineUserValueModule As ScatterChartConstantLineUserValueModule = New ScatterChartConstantLineUserValueModule(dashboardDesigner1, svgImageCollection1("chartrangearea"))

		End Sub
		Public Sub AddFormatRulesToScatterChart(ByVal scatterChart As ScatterChartDashboardItem)
			Dim unitCountThreshold As Double = 7000
			Dim discountThreshold As Double = 18
			Dim expressionRule1 As New ScatterChartItemFormatRule()
			expressionRule1.DataItem = scatterChart.AxisYMeasure
			Dim formatCondition As New FormatConditionExpression()
			formatCondition.Expression = $"{scatterChart.AxisYMeasure.UniqueId} > {unitCountThreshold} && {scatterChart.AxisXMeasure.UniqueId} > {discountThreshold}"
			formatCondition.StyleSettings = New ColorStyleSettings(ColorTranslator.FromHtml("#14abb7"))
			expressionRule1.Condition = formatCondition
			expressionRule1.ShowInLegend = True
			expressionRule1.DisplayName = "Discount amount from the quantity of products sold"
			scatterChart.FormatRules.Add(expressionRule1)
            Dim moduleData As New ScatterChartConstantLineUserData() With {.Enabled = True, .VerticalAxisValue = discountThreshold, .HorizontalAxisValue = unitCountThreshold}
            scatterChart.CustomProperties.SetValue(ScatterChartConstantLineUserValueModule.PropertyName, moduleData.GetStringFromData())
		End Sub
	End Class
End Namespace
