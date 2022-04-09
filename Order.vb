Imports System.Data.SqlClient
Public Class Order

    Dim Database As String
    Dim Connection As SqlConnection
    Dim DataAdapter As SqlDataAdapter
    Dim DataReader As SqlDataReader
    Dim DataSet As DataSet
    Dim Command As SqlCommand
    Dim DataTable As DataTable

    Public Property UserId
    Public Property MemberIdSet

    Sub Connect()
        Database = "Data Source = " & Environment.MachineName.ToString() & "\SQLEXPRESS; Initial Catalog = DB_SMK_RESTAURANT_INFORMATION_SYSTEM; Integrated Security = True"
        Connection = New SqlConnection(Database)
        If Connection.State = ConnectionState.Closed Then Connection.Open()
    End Sub

    Sub StartDataGridView()
        DataGridView1.DataSource = Nothing
        Call Connect()
        DataAdapter = New SqlDataAdapter("SELECT * FROM MsMenu", Connection)
        DataSet = New DataSet
        DataSet.Clear()
        DataAdapter.Fill(DataSet, "MsMenu")
        DataGridView1.DataSource = (DataSet.Tables("MsMenu"))
        DataGridView1.Columns(0).Visible = False
        DataGridView1.Columns(3).Visible = False
    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        Call TextBoxEnabler()
        Call TextBoxClear()
        TextBox1.ReadOnly = True
        Dim i As Integer = DataGridView1.SelectedCells(0).RowIndex
        TextBox1.Text = DataGridView1.Rows(i).Cells(1).Value.ToString()
        TextBox3.Text = DataGridView1.Rows(i).Cells(0).Value.ToString()
        TextBox4.Text = DataGridView1.Rows(i).Cells(2).Value.ToString()
        TextBox5.Text = DataGridView1.Rows(i).Cells(4).Value.ToString()
        TextBox6.Text = DataGridView1.Rows(i).Cells(5).Value.ToString()
        PictureBox1.Image = Image.FromFile(DataGridView1.Rows(i).Cells(3).Value.ToString())
        Button9.Visible = True
    End Sub

    Sub ComboBoxMember()
        Call Connect()
        DataAdapter = New SqlDataAdapter("SELECT * FROM MsMember ORDER BY Name ASC", Connection)
        DataTable = New DataTable
        DataAdapter.Fill(DataTable)
        ComboBox1.DataSource = DataTable
        ComboBox1.DisplayMember = "Name"
        ComboBox1.ValueMember = "Id"
    End Sub

    Sub StartCondition()
        Call Connect()
        Call StartDataGridView()
        Call ComboBoxMember()
        TextBox1.ReadOnly = True
        NumericUpDown1.Enabled = False

        DataGridView3.ColumnCount = 7
        DataGridView3.Columns(0).Name = "Menu"
        DataGridView3.Columns(1).Name = "Qty"
        DataGridView3.Columns(2).Name = "Carbo"
        DataGridView3.Columns(3).Name = "Protein"
        DataGridView3.Columns(4).Name = "Price"
        DataGridView3.Columns(5).Name = "Total"
        DataGridView3.Columns(6).Name = "Id"

        DataGridView3.Columns(6).Visible = False

        DataGridView1.Columns(0).Visible = False
        DataGridView1.Columns(3).Visible = False
        PictureBox1.Image = Nothing

        DataGridView3.Rows.Clear()

        DataGridView3.Enabled = False

        DataGridView1.ClearSelection()
        DataGridView3.ClearSelection()

        Label3.Text = "Carbo: "
        Label4.Text = "Protein: "
        Label5.Text = "Total: "

        Button7.Visible = False
        Button9.Visible = False

        Button6.Text = "Set"
        ComboBox1.Enabled = True
        MemberIdSet = ""
    End Sub
    Private Sub Order_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call StartCondition()
    End Sub

    Sub TextBoxEnabler()
        TextBox1.ReadOnly = True
        NumericUpDown1.Enabled = True
    End Sub

    Sub TextBoxClear()
        TextBox1.Text = ""
        NumericUpDown1.Value = 1
        TextBox3.Text = ""
        TextBox4.Text = ""
        TextBox5.Text = ""
        TextBox6.Text = ""
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs)
        AdminNavigation.UserId = UserId
        Me.Hide()
        AdminNavigation.Show()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        Dim Name As String = TextBox1.Text
        Dim Qty As Integer = NumericUpDown1.Value
        Dim Id As String = TextBox3.Text

        If Id = "" Or TextBox4.Text = "" Or TextBox5.Text = "" Or TextBox6.Text = "" Then
            MessageBox.Show("Please Pick Menu!", "Order Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            If Name = "" Then
                MessageBox.Show("Please Pick Menu!", "Order Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                Dim DataGridRows As Integer = DataGridView3.Rows.Count
                Dim Price As Integer = TextBox4.Text
                Dim Carbo As Integer = TextBox5.Text
                Dim Protein As Integer = TextBox6.Text
                If DataGridRows = 0 Then
                    Dim row As String() = New String() {Name, Qty, Carbo, Protein, Price, Qty * Price, Id}
                    DataGridView3.Rows.Add(row)
                    Call TextBoxClear()
                    PictureBox1.Image = Nothing
                    DataGridView1.Columns(0).Visible = False
                    DataGridView1.Columns(3).Visible = False
                    DataGridView1.Columns(0).Visible = False
                    DataGridView1.Columns(3).Visible = False
                    Call StartDataGridView()
                    NumericUpDown1.Enabled = False
                    DataGridView1.ClearSelection()
                    DataGridView3.ClearSelection()
                    Button7.Visible = False
                    TextBox7.Text = ""
                    Button9.Visible = False
                    Call OrderSummary()
                Else
                    Dim IdExist As String
                    Dim QtyExist As Integer
                    For i As Integer = 0 To DataGridRows - 1
                        IdExist = DataGridView3.Rows(i).Cells(6).Value
                        If IdExist = Id Then
                            QtyExist = DataGridView3.Rows(i).Cells(1).Value
                            Dim QtyAdd As Integer = QtyExist + Qty
                            DataGridView3.Rows(i).Cells(1).Value = QtyAdd
                            DataGridView3.Rows(i).Cells(5).Value = QtyAdd * Price
                            Call TextBoxClear()
                            Call StartDataGridView()
                            NumericUpDown1.Enabled = False
                            DataGridView1.Columns(0).Visible = False
                            DataGridView1.Columns(3).Visible = False
                            PictureBox1.Image = Nothing
                            DataGridView1.ClearSelection()
                            DataGridView3.ClearSelection()
                            Button7.Visible = False
                            Button9.Visible = False
                            TextBox7.Text = ""
                            Call OrderSummary()
                            Exit Sub
                        End If
                    Next
                    Dim row As String() = New String() {Name, Qty, Carbo, Protein, Price, Qty * Price, Id}
                    DataGridView3.Rows.Add(row)
                    Call TextBoxClear()
                    PictureBox1.Image = Nothing
                    Call StartDataGridView()
                    NumericUpDown1.Enabled = False
                    DataGridView1.Columns(0).Visible = False
                    DataGridView1.Columns(3).Visible = False
                    DataGridView1.ClearSelection()
                    DataGridView3.ClearSelection()
                    Button7.Visible = False
                    Button9.Visible = False
                    TextBox7.Text = ""
                    Call OrderSummary()
                End If
            End If
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)
        Dim DataGridRows As Integer = DataGridView3.Rows.Count
        If DataGridRows = 0 Then
            MessageBox.Show("Order Table Is Empty!", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            If Button2.Text = "Delete" Then
                DataGridView1.Enabled = False
                DataGridView3.Enabled = True
                Button2.Text = "Cancel"
                Button1.Enabled = False
                DataGridView1.ClearSelection()
                DataGridView3.ClearSelection()
            Else
                DataGridView1.Enabled = True
                DataGridView3.Enabled = False
                Button2.Text = "Delete"
                Button1.Enabled = Enabled
                DataGridView1.ClearSelection()
                DataGridView3.ClearSelection()
            End If
        End If
    End Sub

    Private Sub DataGridView3_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        Dim ConfirmDelete = MessageBox.Show("Are You Sure?", "Delete Order Menu", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If ConfirmDelete = vbYes Then
            DataGridView3.Rows.Remove(DataGridView3.SelectedRows(0))
            DataGridView3.Enabled = False
            Button2.Text = "Delete"
            Button1.Enabled = Enabled
            DataGridView1.Enabled = True
            DataGridView1.ClearSelection()
            DataGridView3.ClearSelection()
            Call OrderSummary()
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs)
        Dim DataGridRows As Integer = DataGridView3.Rows.Count
        If DataGridRows = 0 Then
            MessageBox.Show("Order Table Is Empty!", "Order Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            Dim OrderId As String
            Dim MenuId As Integer
            Dim Qty As Integer
            Dim Status As String = "UNPAID"

            Dim EmployeeId As String = UserId
            Dim MemberId As String = MemberIdSet

            If MemberId = "" Then
                MessageBox.Show("Member Not Set!", "Order Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                Dim ConfirmOrder = MessageBox.Show("Make Order?", "Make Order", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If ConfirmOrder = vbYes Then
                    Dim TodayDate As String = String.Format("{0:yyyyMMdd}", DateTime.Now)

                    Dim Increment As Int32

                    Dim LatestOrderIdInt As Int32
                    Dim LatestOrderIdDate As String

                    Call Connect()
                    Dim QuerySearch As String = "SELECT * FROM OrderHeader Where Id = (SELECT MAX(Id)  FROM OrderHeader)"
                    Command = New SqlCommand(QuerySearch, Connection)
                    DataReader = Command.ExecuteReader()

                    If DataReader.HasRows Then
                        While DataReader.Read()
                            Dim LatestOrderId As String = DataReader("Id").ToString
                            Dim ChrIncrement As Char
                            Dim ChrDate As Char
                            Dim StrIncrement As String
                            Dim StrDate As String
                            For i = 8 To 12 - 1
                                ChrIncrement = LatestOrderId(i).ToString
                                StrIncrement += ChrIncrement.ToString
                            Next
                            For i = 0 To 8 - 1
                                ChrDate = LatestOrderId(i).ToString
                                StrDate += ChrDate.ToString
                            Next
                            LatestOrderIdDate = StrDate
                            LatestOrderIdInt = StrIncrement
                        End While

                        If LatestOrderIdDate = TodayDate Then
                            Connection.Close()
                            Call Connect()
                            Increment = LatestOrderIdInt + 1
                            OrderId = TodayDate & Increment.ToString("D4")
                            Dim QueryInput As String = "INSERT INTO OrderHeader (Id, EmployeeId, MemberId) VALUES ('" & OrderId & "','" & EmployeeId & "','" & MemberId & "')"
                            Command = New SqlCommand(QueryInput, Connection)
                            Command.ExecuteNonQuery()
                        Else
                            Connection.Close()
                            Call Connect()
                            Increment = 1
                            OrderId = TodayDate & Increment.ToString("D4")
                            Dim QueryInput As String = "INSERT INTO OrderHeader (Id, EmployeeId, MemberId) VALUES ('" & OrderId & "','" & EmployeeId & "','" & MemberId & "')"
                            Command = New SqlCommand(QueryInput, Connection)
                            Command.ExecuteNonQuery()
                        End If
                    Else
                        Connection.Close()
                        Call Connect()
                        Increment = 1
                        OrderId = TodayDate & Increment.ToString("D4")
                        Dim QueryInput As String = "INSERT INTO OrderHeader (Id, EmployeeId, MemberId) VALUES ('" & OrderId & "','" & EmployeeId & "','" & MemberId & "')"
                        Command = New SqlCommand(QueryInput, Connection)
                        Command.ExecuteNonQuery()
                    End If

                    Connection.Close()
                    Call Connect()
                    For i As Integer = 0 To DataGridRows - 1
                        MenuId = DataGridView3.Rows(i).Cells(6).Value
                        Qty = DataGridView3.Rows(i).Cells(1).Value
                        Dim QueryInput As String = "INSERT INTO OrderDetail (OrderId, MenuId, Qty, Status) VALUES ('" & OrderId & "','" & MenuId & "','" & Qty & "','" & Status & "')"
                        Command = New SqlCommand(QueryInput, Connection)
                        Command.ExecuteNonQuery()
                    Next
                    MessageBox.Show("Order Success", "Order Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    StartCondition()
                Else
                End If
            End If
        End If
    End Sub

    Sub OrderSummary()
        Dim DataGridRows As Integer = DataGridView3.Rows.Count
        For i As Integer = 0 To DataGridRows - 1
            Dim Total As Integer
            Dim Carbo As Integer
            Dim Protein As Integer
            Total += DataGridView3.Rows(i).Cells(5).Value
            Carbo += DataGridView3.Rows(i).Cells(2).Value * DataGridView3.Rows(i).Cells(1).Value
            Protein += DataGridView3.Rows(i).Cells(3).Value * DataGridView3.Rows(i).Cells(1).Value
            Label3.Text = "Carbo: " & Carbo
            Label4.Text = "Protein: " & Protein
            Label5.Text = "Total: " & Total
        Next
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs)
        DataGridView3.Rows.Clear()
        DataGridView3.ClearSelection()
        Label3.Text = "Carbo: "
        Label4.Text = "Protein: "
        Label5.Text = "Total: "
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs)
        If Button6.Text = "Set" Then
            Dim SelectedMemberId As String = ComboBox1.SelectedValue
            Dim SelectedMemberName As String = ComboBox1.Text

            If SelectedMemberName = "" Then
                MessageBox.Show("Please Pick Member!", "Order Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                If SelectedMemberId = "" Then
                    MessageBox.Show("Member Is Not Registered!", "Order Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Else
                    Button6.Text = "Unset"
                    ComboBox1.Enabled = False
                    MemberIdSet = SelectedMemberId
                End If
            End If
        Else
            Button6.Text = "Set"
            ComboBox1.Enabled = True
            MemberIdSet = ""
        End If
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs)
        TextBox1.Text = ""
        NumericUpDown1.Value = 1
        NumericUpDown1.Enabled = False
        DataGridView1.ClearSelection()
        Button9.Visible = False
        PictureBox1.Image = Nothing
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs)
        Dim Instructions As String
        Instructions = "Order Instructions:" & vbNewLine & vbNewLine & "● Pick menu in menu table then fill quantity field." & vbNewLine & vbNewLine & "● Click add button to insert menu to order table." & vbNewLine & vbNewLine & "● To delete menu from order table, click delete button then click menu from order table to delete it." & vbNewLine & vbNewLine & "● Select and set member in dropdownmenu before click order button" & vbNewLine & vbNewLine & "● Click order button to process and perform order."
        MessageBox.Show(Instructions, "Order Instructions", MessageBoxButtons.OK)
    End Sub

End Class