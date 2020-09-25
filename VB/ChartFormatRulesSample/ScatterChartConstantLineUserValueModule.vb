Imports Microsoft.VisualBasic
Imports DevExpress.DashboardCommon
Imports DevExpress.DashboardWin
Imports DevExpress.Utils.Svg
Imports DevExpress.XtraBars
Imports DevExpress.XtraBars.Ribbon
Imports DevExpress.XtraCharts
Imports DevExpress.XtraDataLayout
Imports DevExpress.XtraEditors
Imports DevExpress.XtraReports.UI
Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Linq
Imports System.Windows.Forms

Namespace WindowsFormsAppCustomProperties
	Public Class ScatterChartConstantLineUserData
		Private privateEnabled As Boolean
		Public Property Enabled() As Boolean
			Get
				Return privateEnabled
			End Get
			Set(ByVal value As Boolean)
				privateEnabled = value
			End Set
		End Property
		Private privateVerticalAxisValue As Double
		Public Property VerticalAxisValue() As Double
			Get
				Return privateVerticalAxisValue
			End Get
			Set(ByVal value As Double)
				privateVerticalAxisValue = value
			End Set
		End Property
		Private privateHorizontalAxisValue As Double
		Public Property HorizontalAxisValue() As Double
			Get
				Return privateHorizontalAxisValue
			End Get
			Set(ByVal value As Double)
				privateHorizontalAxisValue = value
			End Set
		End Property

		Public Function GetStringFromData() As String
			Return Enabled.ToString() & "_" & VerticalAxisValue & "_" & HorizontalAxisValue
		End Function
	End Class
	Public Class ScatterChartConstantLineUserValueModule
		Public Shared ReadOnly PropertyName As String = "ConstantLine"
		Private ReadOnly designer As DashboardDesigner
		Private barItem As BarButtonItem
		Public Sub New(ByVal designer As DashboardDesigner, Optional ByVal barImage As SvgImage = Nothing)
			Me.designer = designer
			Dim barItem As BarButtonItem = AddBarItem("Constant line", barImage, designer.Ribbon)
			AddHandler barItem.ItemClick, AddressOf BarItem_Click
			AddHandler designer.DashboardItemControlUpdated, AddressOf Designer_DashboardItemControlUpdated
			AddHandler designer.CustomExport, AddressOf Designer_CustomExport
		End Sub
		Private Function AddBarItem(ByVal caption As String, ByVal barImage As SvgImage, ByVal ribbon As RibbonControl) As BarButtonItem
			Dim page = ribbon.GetDashboardRibbonPage(DashboardBarItemCategory.ScatterChartTools, DashboardRibbonPage.Design)
			Dim group As RibbonPageGroup = page.GetGroupByName("Custom Properties")
			If group Is Nothing Then
				group = New RibbonPageGroup("Custom Properties") With {.Name = "Custom Properties"}
				group.AllowTextClipping = False
				page.Groups.Add(group)
			End If
			barItem = New BarButtonItem(ribbon.Manager, caption)
			barItem.ImageOptions.SvgImage = barImage
			group.ItemLinks.Add(barItem)
			Return barItem
		End Function
		Private Sub BarItem_Click(ByVal sender As Object, ByVal e As ItemClickEventArgs)
			Dim chartItem = TryCast(designer.SelectedDashboardItem, ScatterChartDashboardItem)
			If chartItem IsNot Nothing Then
				Dim data = GetConstantLineModuleData(chartItem)
				If data.GetStringFromData() <> chartItem.CustomProperties.GetValue(PropertyName) Then
					designer.AddToHistory(New CustomPropertyHistoryItem(chartItem, PropertyName, data.GetStringFromData(), $"Constant lines state: {data.Enabled} - {data.VerticalAxisValue} - {data.VerticalAxisValue}"))
				End If
			End If
		End Sub
		Private Function GetConstantLineModuleData(ByVal dashboardItem As ScatterChartDashboardItem) As ScatterChartConstantLineUserData
			Dim data = GetDataFromString(dashboardItem.CustomProperties.GetValue(PropertyName))
			Using selector As New ValueSelectorControl(data)
				If XtraDialog.Show(selector, "Adjust constant lines") = DialogResult.OK Then
					Return selector.ConstantLineModuleData
				End If
			End Using
			Return Nothing
		End Function

		Private Sub Designer_CustomExport(ByVal sender As Object, ByVal e As CustomExportEventArgs)
			Dim controls As Dictionary(Of String, XRControl) = e.GetPrintableControls()
			For Each pair As KeyValuePair(Of String, XRControl) In controls
				Dim xrChart As XRChart = TryCast(pair.Value, XRChart)
				If xrChart IsNot Nothing Then
					Dim itemComponentName As String = pair.Key
					Dim chart As ScatterChartDashboardItem = TryCast(designer.Dashboard.Items(itemComponentName), ScatterChartDashboardItem)
					If chart IsNot Nothing Then
						UpdateChart(xrChart.Series, chart)
					End If
				End If
			Next pair
		End Sub
		Private Sub Designer_DashboardItemControlUpdated(ByVal sender As Object, ByVal e As DashboardItemControlEventArgs)
			If e.ChartControl IsNot Nothing Then
				Dim scatterDashboardItem As ScatterChartDashboardItem = TryCast(designer.Dashboard.Items(e.DashboardItemName), ScatterChartDashboardItem)
				If scatterDashboardItem IsNot Nothing Then
					UpdateChart(e.ChartControl.Series, scatterDashboardItem)
				End If
			End If
		End Sub
		Private Sub SetUpLine(ByVal chartAxis As Axis, ByVal color As Color, ByVal textColor As Color, ByVal value As Double)
			Dim line As New ConstantLine() With {.AxisValue = value}
			line.ShowInLegend = False
			line.Color = color
			line.LineStyle.Thickness = 2
			line.LineStyle.DashStyle = DashStyle.Dash
			line.Title.Text = "Value: " & value.ToString()
			line.Title.TextColor = textColor
			chartAxis.ConstantLines.Add(line)
		End Sub
		Private Sub UpdateChart(ByVal series As SeriesCollection, ByVal chartDashboardItem As ScatterChartDashboardItem)
			Dim moduleData As ScatterChartConstantLineUserData = GetDataFromString(chartDashboardItem.CustomProperties.GetValue(PropertyName))
			If moduleData IsNot Nothing AndAlso moduleData.Enabled Then
                Dim seriesView As BubbleSeriesView = series.OfType(Of Series)().Where(Function(s) TypeOf s.View Is BubbleSeriesView).Select(Function(s) TryCast(s.View, BubbleSeriesView)).FirstOrDefault()
                If seriesView IsNot Nothing Then
					seriesView.AxisX.ConstantLines.Clear()
					seriesView.AxisY.ConstantLines.Clear()
					SetUpLine(seriesView.AxisX, ColorTranslator.FromHtml("#14abb7"), ColorTranslator.FromHtml("#0e9ca9"), moduleData.VerticalAxisValue)
					SetUpLine(seriesView.AxisY, ColorTranslator.FromHtml("#14abb7"), ColorTranslator.FromHtml("#0e9ca9"), moduleData.HorizontalAxisValue)
				End If
			End If
		End Sub
		Private Function GetDataFromString(ByVal customPropertyValue As String) As ScatterChartConstantLineUserData
			If (Not String.IsNullOrEmpty(customPropertyValue)) Then
                Dim array = customPropertyValue.Split("_"c)
                Dim moduleData = New ScatterChartConstantLineUserData()
                moduleData.Enabled = Boolean.Parse(array(0))
                moduleData.VerticalAxisValue = Convert.ToDouble(array(1))
                moduleData.HorizontalAxisValue = Convert.ToDouble(array(2))
                Return moduleData
            End If
			Return Nothing
		End Function
	End Class
	Public Class ValueSelectorControl
		Inherits XtraUserControl
		Private privateConstantLineModuleData As ScatterChartConstantLineUserData
        Public Property ConstantLineModuleData() As ScatterChartConstantLineUserData
            Get
                Return privateConstantLineModuleData
            End Get
            Set(value As ScatterChartConstantLineUserData)
                privateConstantLineModuleData = value
            End Set
        End Property

        Public Sub New(ByVal data As ScatterChartConstantLineUserData)
			Dim dataLayoutControl As New DataLayoutControl()
			Dim source As New BindingSource()
			ConstantLineModuleData = If(data, New ScatterChartConstantLineUserData())
			source.DataSource = ConstantLineModuleData
			dataLayoutControl.DataSource = source
			dataLayoutControl.RetrieveFields()
			dataLayoutControl.Dock = DockStyle.Fill
			Controls.Add(dataLayoutControl)
			Dock = DockStyle.Top
		End Sub
	End Class
End Namespace
