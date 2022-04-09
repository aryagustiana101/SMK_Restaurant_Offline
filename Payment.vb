Imports System.Data.SqlClient
Public Class Payment
    Dim Connection As SqlConnection
    Dim Database As String
    Dim DataSet As DataSet
    Dim DataAdapter As SqlDataAdapter
    Dim DataReader As SqlDataReader
    Dim Command As SqlCommand
    Dim DataTable As DataTable

    Public Property UserId

    Sub Connect()
        Database = "Data Source = " & Environment.MachineName.ToString() & "\SQLEXPRESS; Initial Catalog = DB_SMK_RESTAURANT_INFORMATION_SYSTEM; Integrated Security = True"
        Connection = New SqlConnection(Database)
        If Connection.State = ConnectionState.Closed Then Connection.Open()
    End Sub

    Sub ComboBoxOrder()
        ComboBox1.DataSource = Nothing
        Call Connect()
        DataAdapter = New SqlDataAdapter("SELECT DISTINCT Id As OrderId FROM OrderHeader  WHERE PaymentType IS NULL", Connection)
        DataTable = New DataTable
        DataAdapter.Fill(DataTable)
        ComboBox1.DataSource = DataTable
        ComboBox1.DisplayMember = "OrderId"
        ComboBox1.ValueMember = "OrderId"
    End Sub

    Sub ComboBoxBank()
        ComboBox3.Items.Clear()
        ComboBox3.Items.Add("BCA")
        ComboBox3.Items.Add("BRI")
        ComboBox3.Items.Add("BNI")
        ComboBox3.Items.Add("Mandiri")
        ComboBox3.Items.Add("Permata Bank")
        ComboBox3.Items.Add("CIMB Niaga")
        ComboBox3.Items.Add("BTN")
        ComboBox3.Items.Add("BJB")
        ComboBox3.Items.Add("Mega")
    End Sub

    Sub StartCondition()
        ComboBox2.Visible = False
        ComboBox3.Visible = False
        Label5.Visible = False
        Button1.Visible = False

        ComboBox2.Items.Add("Cash")
        ComboBox2.Items.Add("Credit")
        Label3.Text = ""

        Label6.Visible = False
        Label7.Visible = False
        TextBox1.Visible = False
        TextBox2.Visible = False
        Button2.Visible = False

        Label6.Text = ""
        Label7.Text = ""

        TextBox1.MaxLength = 16
        Call ComboBoxOrder()
        Call ComboBoxBank()
        DataGridView1.DataSource = Nothing

        Button4.Visible = False
    End Sub

    Private Sub Payment_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call StartCondition()
    End Sub

    Private Sub ComboBox1_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles ComboBox1.SelectionChangeCommitted
        Call Connect()
        Dim OrderIdSelected As String = ComboBox1.SelectedValue.ToString
        DataAdapter = New SqlDataAdapter("Select MsMenu.Name As MenuName, OrderDetail.Qty As Qty, MsMenu.Price As Price, OrderDetail.Qty * MsMenu.Price As Total  FROM OrderDetail JOIN OrderHeader ON OrderId = OrderHeader.Id JOIN MsMenu ON MenuId = MsMenu.Id WHERE OrderId = '" & OrderIdSelected & "'", Connection)
        DataSet = New DataSet
        DataSet.Clear()
        DataAdapter.Fill(DataSet, "Order")
        DataGridView1.DataSource = (DataSet.Tables("Order"))
        DataGridView1.ClearSelection()
        Dim OrderRowCount As Integer = DataSet.Tables("Order").Rows.Count
        Dim Total As Integer
        For i = 0 To OrderRowCount - 1
            Total += DataSet.Tables("Order").Rows(i).Item("Total")
        Next
        Label3.Text = Total
        ComboBox2.Visible = True
        Label5.Visible = True

        Label6.Visible = False
        Label7.Visible = False
        TextBox1.Visible = False
        TextBox2.Visible = False
        Button2.Visible = False

        ComboBox2.Items.Clear()
        ComboBox2.Items.Add("Cash")
        ComboBox2.Items.Add("Credit")

        TextBox1.Text = ""
        TextBox2.Text = ""

        TextBox1.ReadOnly = False
        Button2.Text = "Input Cash"

        Button1.Visible = False

        ComboBox3.Visible = False
        Call ComboBoxBank()
        Button4.Visible = True
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        Dim SelectedPayment As String = ComboBox2.SelectedItem.ToString
        If SelectedPayment = "Credit" Then
            Label6.Text = "Card Number"
            Label7.Text = "Bank Name"
            Label6.Visible = True
            Label7.Visible = True
            TextBox1.Visible = True
            TextBox2.Visible = False
            Button2.Visible = False
            TextBox1.Text = ""
            ComboBox3.Visible = True
            Call ComboBoxBank()
            Button1.Visible = True
            TextBox1.MaxLength = 16
        Else
            Label6.Text = "Cash"
            Label6.Visible = True
            Label7.Visible = False
            TextBox1.Visible = True
            TextBox1.Text = ""
            TextBox2.Visible = False
            Button2.Visible = True
            ComboBox3.Visible = False
            Button1.Visible = False
            TextBox1.MaxLength = 32767
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim SelectedPayment As String = ComboBox2.SelectedItem.ToString
        Dim OrderId As String = ComboBox1.SelectedValue
        If SelectedPayment = "Credit" Then
            If TextBox1.Text = "" Or String.IsNullOrEmpty(ComboBox3.SelectedItem) Then
                MessageBox.Show("Payment Field Empty!", "Payment Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                Dim PaymentType As String = SelectedPayment
                Dim CardNumber As String = TextBox1.Text
                Dim Bank As String = ComboBox3.SelectedItem.ToString
                If IsNumeric(CardNumber) = True Then
                    Call Connect()
                    Dim QueryUpdateHeader As String = "UPDATE OrderHeader SET PaymentType = '" & PaymentType & "', CardNumber = '" & CardNumber & "', Bank = '" & Bank & "' WHERE Id = '" & OrderId & "'"
                    Command = New SqlCommand(QueryUpdateHeader, Connection)
                    Command.ExecuteNonQuery()
                    Connection.Close()
                    Call Connect()
                    Dim QueryUpdateDetail As String = "UPDATE OrderDetail SET Status = 'PAID' WHERE OrderId = '" & OrderId & "'"
                    Command = New SqlCommand(QueryUpdateDetail, Connection)
                    Command.ExecuteNonQuery()
                    MessageBox.Show("Payment Success And Complete", "Payment Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Connection.Close()
                    Call StartCondition()
                Else
                    MessageBox.Show("Card Number Must Number!", "Payment Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If
        Else
            Dim PaymentType As String = SelectedPayment
            Call Connect()
            Dim QueryUpdateHeader As String = "UPDATE OrderHeader SET PaymentType = '" & PaymentType & "' WHERE Id = '" & OrderId & "'"
            Command = New SqlCommand(QueryUpdateHeader, Connection)
            Command.ExecuteNonQuery()
            Connection.Close()
            Call Connect()
            Dim QueryUpdateDetail As String = "UPDATE OrderDetail SET Status = 'PAID' WHERE OrderId = '" & OrderId & "'"
            Command = New SqlCommand(QueryUpdateDetail, Connection)
            Command.ExecuteNonQuery()
            MessageBox.Show("Payment Success And Complete", "Payment Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Connection.Close()
            Call StartCondition()
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If Button2.Text = "Input Cash" Then
            Dim Cash As Integer
            Dim TotalPay As Integer = Label3.Text
            If TextBox1.Text = "" Then
                MessageBox.Show("Input Cash!", "Payment Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                If IsNumeric(TextBox1.Text) = True Then
                    Cash = TextBox1.Text
                    If Cash < TotalPay Then
                        MessageBox.Show("Cash Amount Less Than Total Payment!", "Payment Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    ElseIf Cash > TotalPay Then
                        Label7.Visible = True
                        TextBox2.Visible = True
                        Label7.Text = "Change"
                        TextBox2.Text = Cash - TotalPay
                        Button1.Visible = True
                        TextBox1.ReadOnly = True
                        Button2.Text = "Cancel"
                    Else
                        TextBox1.ReadOnly = True
                        Button2.Text = "Cancel"
                        Button1.Visible = True
                    End If
                Else
                    MessageBox.Show("Cash Field Must Number!", "Payment Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If
        Else
            Button1.Visible = False
            Label7.Visible = False
            TextBox2.Visible = False
            TextBox2.Text = ""
            TextBox1.ReadOnly = False
            TextBox1.Text = ""
            Button2.Text = "Input Cash"
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        CashierNavigation.UserId = UserId
        Me.Hide()
        CashierNavigation.Show()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Call StartCondition()
    End Sub

End Class