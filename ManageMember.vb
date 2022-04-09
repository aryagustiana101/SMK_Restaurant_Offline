Imports System.Data.SqlClient
Imports System.Text.RegularExpressions

Public Class ManageMember

    Dim Connection As SqlConnection
    Dim DataAdapter As SqlDataAdapter
    Dim Command As SqlCommand
    Dim DataReader As SqlDataReader
    Dim DataSet As DataSet
    Dim Database As String

    Public Property UserId

    Sub Connect()
        Database = "Data Source = " & Environment.MachineName.ToString() & "\SQLEXPRESS; Initial Catalog = DB_SMK_RESTAURANT_INFORMATION_SYSTEM; Integrated Security = True"
        Connection = New SqlConnection(Database)
        If Connection.State = ConnectionState.Closed Then Connection.Open()
    End Sub

    Sub StartDataGridView()
        Call Connect()
        DataAdapter = New SqlDataAdapter("SELECT * FROM MsMember", Connection)
        DataSet = New DataSet
        DataSet.Clear()
        DataAdapter.Fill(DataSet, "MsMember")
        DataGridView1.DataSource = (DataSet.Tables("MsMember"))
        DataGridView1.ClearSelection()
    End Sub

    Sub StartCondition()
        Call Connect()
        Call StartDataGridView()

        TextBox1.ReadOnly = True
        TextBox2.ReadOnly = True
        TextBox3.ReadOnly = True
        TextBox4.ReadOnly = True

        TextBox5.ReadOnly = False

        Call TextBoxClear()

        Label2.Visible = True
        TextBox1.Visible = True

        Button1.Text = "Insert"
        Button1.Enabled = True
        Button2.Text = "Update"
        Button2.Enabled = True
        Button3.Text = "Delete"
        Button3.Enabled = True
        Button4.Visible = False

        Button6.Enabled = True
        Button7.Enabled = True

        Label6.Text = "Click Data In Table To See Detail."
        UserId = UserId
    End Sub

    Private Sub ManageMember_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call StartCondition()
    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If Button1.Text = "Add" Then
        ElseIf Button2.Text = "Edit" Then
            Call TextBoxEnabler()
            TextBox5.ReadOnly = True
            Dim i As Integer = DataGridView1.SelectedCells(0).RowIndex
            TextBox1.Text = DataGridView1.Rows(i).Cells(0).Value.ToString()
            TextBox2.Text = DataGridView1.Rows(i).Cells(1).Value.ToString()
            TextBox3.Text = DataGridView1.Rows(i).Cells(2).Value.ToString()
            TextBox4.Text = DataGridView1.Rows(i).Cells(3).Value.ToString()
        Else
            Dim i As Integer = DataGridView1.SelectedCells(0).RowIndex
            TextBox1.Text = DataGridView1.Rows(i).Cells(0).Value.ToString()
            TextBox2.Text = DataGridView1.Rows(i).Cells(1).Value.ToString()
            TextBox3.Text = DataGridView1.Rows(i).Cells(2).Value.ToString()
            TextBox4.Text = DataGridView1.Rows(i).Cells(3).Value.ToString()
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        AdminNavigation.UserId = UserId
        Me.Hide()
        AdminNavigation.Show()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Button1.Text = "Insert" Then
            Button2.Enabled = False
            Button3.Enabled = False
            Button4.Visible = True
            Button1.Text = "Add"
            Call TextBoxClear()
            Call TextBoxEnabler()
            Label2.Visible = False
            TextBox1.Visible = False
            Button6.Enabled = False
            TextBox5.ReadOnly = True
            Button7.Enabled = False
            Label6.Text = "Fill Text Field With Appropriate Data"
        Else
            Dim Name As String = TextBox2.Text
            Dim Email As String = TextBox3.Text
            Dim Handphone As String = TextBox4.Text
            If Name = "" Or Email = "" Or Handphone = "" Then
                MessageBox.Show("Text Field Empty!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                If EmailAddressCheck(Email) = True Then
                    If IsNumeric(Handphone) = True Then
                        Call Connect()
                        Dim QueryInput As String = "INSERT INTO MsMember (Name, Email, Handphone) VALUES ('" & Name & "','" & Email & "','" & Handphone & "')"
                        Command = New SqlCommand(QueryInput, Connection)
                        Command.ExecuteNonQuery()
                        MessageBox.Show("Input Success", "Input Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Call StartCondition()
                    Else
                        MessageBox.Show("Handphone Must Number!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                Else
                    MessageBox.Show("Email Not Valid!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If Button2.Text = "Update" Then
            Button1.Enabled = False
            Button3.Enabled = False
            Button4.Visible = True
            Button2.Text = "Edit"
            Call TextBoxClear()
            Button6.Enabled = False
            TextBox5.ReadOnly = True
            Button7.Enabled = False
            Label6.Text = "Pick Data In Table To Edit"
            Call StartDataGridView()
        Else
            Dim Id As String = TextBox1.Text
            Dim Name As String = TextBox2.Text
            Dim Email As String = TextBox3.Text
            Dim Handphone As String = TextBox4.Text
            If Id = "" Or Name = "" Or Email = "" Or Handphone = "" Then
                MessageBox.Show("Text Field Empty!", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                If EmailAddressCheck(Email) = True Then
                    If IsNumeric(Handphone) = True Then
                        Call Connect()
                        Dim QueryUpdate As String = "UPDATE MsMember SET Name = '" & Name & "', Email = '" & Email & "', Handphone = '" & Handphone & "' WHERE Id = '" & Id & "'"
                        Command = New SqlCommand(QueryUpdate, Connection)
                        Command.ExecuteNonQuery()
                        MessageBox.Show("Update Success!", "Update Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Call StartCondition()
                    Else
                        MessageBox.Show("Handphone Must Number!", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                Else
                    MessageBox.Show("Email Not Valid!", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If Button3.Text = "Delete" Then
            Button1.Enabled = False
            Button2.Enabled = False
            Button4.Visible = True
            Button3.Text = "Remove"
            Call TextBoxClear()
            Button6.Enabled = False
            TextBox5.ReadOnly = True
            Button7.Enabled = False
            Label6.Text = "Pick Data In Table To Delete"
            Call StartDataGridView()
        Else
            Dim Id As String = TextBox1.Text
            Dim Name As String = TextBox2.Text
            Dim Email As String = TextBox3.Text
            Dim Handphone As String = TextBox4.Text
            If Id = "" Or Name = "" Or Email = "" Or Handphone = "" Then
                MessageBox.Show("Pick Data To Delete!", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                If EmailAddressCheck(Email) = True Then
                    If IsNumeric(Handphone) = True Then
                        Call Connect()
                        Dim QueryDelete As String = "DELETE FROM MsMember WHERE Id = '" & Id & "'"
                        Dim ConfirmDelete = MessageBox.Show("Are You Sure?", "Delete Member", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                        If ConfirmDelete = vbYes Then
                            Command = New SqlCommand(QueryDelete, Connection)
                            Command.ExecuteNonQuery()
                            MessageBox.Show("Delete Success", "Delete Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Call StartCondition()
                        Else
                            Call StartCondition()
                        End If
                    Else
                        MessageBox.Show("Handphone Must Number", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                Else
                    MessageBox.Show("Email Not Valid!", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Call StartCondition()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Button1.Enabled = False
        Button2.Enabled = False
        Button3.Enabled = False
        Button4.Visible = True

        Dim Keyword As String = TextBox5.Text

        If Keyword = "" Then
            MessageBox.Show("Search Field Empty", "Search Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Call StartCondition()
        Else
            DataAdapter = New SqlDataAdapter("SELECT * FROM MsMember WHERE Id LIKE '%" & Keyword & "%' OR Name LIKE '%" & Keyword & "%' OR Email LIKE '%" & Keyword & "%' OR Handphone LIKE '%" & Keyword & "%' OR JoinDate LIKE '%" & Keyword & "%'", Connection)
            DataSet = New DataSet
            DataSet.Clear()
            DataAdapter.Fill(DataSet, "MsMember")
            If DataSet.Tables("MsMember").Rows.Count > 0 Then
                TextBox1.Text = ""
                TextBox2.Text = ""
                TextBox3.Text = ""
                TextBox4.Text = ""
                'MessageBox.Show(DataSet.Tables("MsMember").Rows(0).Item("Name"))
                DataGridView1.DataSource = (DataSet.Tables("MsMember"))
            Else
                MessageBox.Show("Keyword Not Found!", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Call StartCondition()
            End If
        End If
    End Sub

    Function EmailAddressCheck(ByVal emailAddress As String) As Boolean
        Dim pattern As String = "^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"
        Dim emailAddressMatch As Match = Regex.Match(emailAddress, pattern)
        If emailAddressMatch.Success Then
            EmailAddressCheck = True
        Else
            EmailAddressCheck = False
        End If
    End Function

    Sub TextBoxEnabler()
        TextBox2.ReadOnly = False
        TextBox3.ReadOnly = False
        TextBox4.ReadOnly = False
        TextBox5.ReadOnly = False
    End Sub

    Sub TextBoxClear()
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""
        TextBox4.Text = ""
        TextBox5.Text = ""
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""
        TextBox4.Text = ""
        TextBox5.Text = ""
        Call StartDataGridView()
    End Sub
End Class