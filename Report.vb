Imports System.Data.SqlClient

Public Class Report

    Dim Command As SqlCommand
    Dim Database As String
    Dim Connection As SqlConnection
    Dim DataReader As SqlDataReader
    Dim DataSet As DataSet
    Dim DataAdapter As SqlDataAdapter

    Public Property UserId

    Sub Connect()
        Database = "Data Source = " & Environment.MachineName.ToString() & "\SQLEXPRESS; Initial Catalog = DB_SMK_RESTAURANT_INFORMATION_SYSTEM; Integrated Security = True"
        Connection = New SqlConnection(Database)
        If Connection.State = ConnectionState.Closed Then Connection.Open()
    End Sub

    Sub ComboBoxYear()
        ComboBox1.Items.Clear()
        Call Connect()
        Dim OldestYear As String
        Dim YearNow As String = String.Format("{0:yyyy}", DateTime.Now)
        Dim QueryRead As String = "SELECT * FROM OrderHeader WHERE Id = (SELECT MIN(Id) FROM OrderHeader)"
        Command = New SqlCommand(QueryRead, Connection)
        DataReader = Command.ExecuteReader()
        If DataReader.HasRows Then
            While DataReader.Read()
                OldestYear = String.Format("{0:yyyy}", DataReader("Date"))
            End While
            Dim IntYearOld As Integer = OldestYear
            Dim IntYearNow As Integer = YearNow
            For i = IntYearOld To IntYearNow
                ComboBox1.Items.Add(i)
            Next
        Else
            ComboBox1.Items.Add(" ")
        End If
    End Sub

    Sub ComboBoxMonthFrom()
        ComboBox2.Items.Clear()
        ComboBox2.Items.Add("January")
        ComboBox2.Items.Add("February")
        ComboBox2.Items.Add("March")
        ComboBox2.Items.Add("April")
        ComboBox2.Items.Add("May")
        ComboBox2.Items.Add("June")
        ComboBox2.Items.Add("July")
        ComboBox2.Items.Add("August")
        ComboBox2.Items.Add("September")
        ComboBox2.Items.Add("October")
        ComboBox2.Items.Add("November")
        ComboBox2.Items.Add("December")
    End Sub

    Sub StartCondition()
        Call Connect()
        Call ComboBoxYear()
        ComboBox2.Enabled = False
        ComboBox2.Items.Clear()
        ComboBox3.Enabled = False
        ComboBox3.Items.Clear()
        Button2.Visible = False
        Button1.Enabled = False

        DataGridView1.ColumnCount = 2
        DataGridView1.Columns(0).Name = "Month"
        DataGridView1.Columns(1).Name = "Income"
        DataGridView1.Rows.Clear()

        Chart1.ChartAreas(0).AxisX.Interval = 1
        Chart1.Series("Income").Points.Clear()

    End Sub

    Private Sub Report_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call StartCondition()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Button2.Visible = True
        ComboBox2.Enabled = True
        ComboBox2.Items.Clear()
        Call ComboBoxMonthFrom()
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        ComboBox3.Enabled = True
        ComboBox3.Items.Clear()
        Dim SelectedIndex As Integer = ComboBox2.SelectedIndex + 1
        For i = SelectedIndex To 12
            ComboBox3.Items.Add(MonthName(i))
        Next
    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox3.SelectedIndexChanged
        Button1.Enabled = True
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        DataGridView1.Rows.Clear()
        Chart1.Series("Income").Points.Clear()
        Dim YearSelected As Integer = ComboBox1.SelectedItem
        Dim FromIndex As Integer = ComboBox2.SelectedIndex + 1
        Dim ToIndex As Integer = Format(Month(DateValue("1 " & ComboBox3.SelectedItem & " " & YearSelected)))
        Dim TotalIncomeMonth As Integer = 0
        For i = FromIndex To ToIndex
            Call Connect()
            DataAdapter = New SqlDataAdapter("SELECT OrderDetail.Qty * MsMenu.Price As Total FROM OrderHeader JOIN OrderDetail ON OrderHeader.Id = OrderDetail.OrderId JOIN MsMenu ON OrderDetail.MenuId = MsMenu.Id WHERE MONTH(OrderHeader.Date) = " & i & " AND YEAR(OrderHeader.Date) = " & YearSelected & " AND OrderHeader.PaymentType IS NOT NULL", Connection)
            DataSet = New DataSet
            DataSet.Clear()
            DataAdapter.Fill(DataSet, "TotalSet")
            If DataSet.Tables("TotalSet").Rows.Count > 0 Then
                TotalIncomeMonth = 0
                For j = 0 To DataSet.Tables("TotalSet").Rows.Count - 1
                    TotalIncomeMonth += DataSet.Tables("TotalSet").Rows(j).Item("Total")
                Next
                Dim row As String() = New String() {MonthName(i).ToString, TotalIncomeMonth / 1000000}
                DataGridView1.Rows.Add(row)
                Chart1.Series("Income").Points.AddXY(MonthName(i).ToString, TotalIncomeMonth / 1000000)
            Else
                Dim row As String() = New String() {MonthName(i).ToString, 0}
                DataGridView1.Rows.Add(row)
                Chart1.Series("Income").Points.AddXY(MonthName(i).ToString, 0)
            End If
        Next
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Call StartCondition()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        AdminNavigation.UserId = UserId
        Me.Hide()
        AdminNavigation.Show()
    End Sub
End Class