Imports System.Data.SqlClient
Public Class ManageMenu

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
        DataAdapter = New SqlDataAdapter("SELECT * FROM MsMenu", Connection)
        DataSet = New DataSet
        DataSet.Clear()
        DataAdapter.Fill(DataSet, "MsMenu")
        DataGridView1.DataSource = (DataSet.Tables("MsMenu"))
        DataGridView1.ClearSelection()
    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If Button1.Text = "Add" Then
        ElseIf Button2.Text = "Edit" Then
            Call TextBoxEnabler()
            TextBox7.ReadOnly = True
            Button5.Enabled = True
            Dim i As Integer = DataGridView1.SelectedCells(0).RowIndex
            TextBox1.Text = DataGridView1.Rows(i).Cells(0).Value.ToString()
            TextBox2.Text = DataGridView1.Rows(i).Cells(1).Value.ToString()
            TextBox3.Text = DataGridView1.Rows(i).Cells(2).Value.ToString()
            TextBox4.Text = DataGridView1.Rows(i).Cells(3).Value.ToString()
            PictureBox1.Image = Image.FromFile(DataGridView1.Rows(i).Cells(3).Value.ToString())
            TextBox5.Text = DataGridView1.Rows(i).Cells(4).Value.ToString()
            TextBox6.Text = DataGridView1.Rows(i).Cells(5).Value.ToString()
        Else
            Dim i As Integer = DataGridView1.SelectedCells(0).RowIndex
            TextBox1.Text = DataGridView1.Rows(i).Cells(0).Value.ToString()
            TextBox2.Text = DataGridView1.Rows(i).Cells(1).Value.ToString()
            TextBox3.Text = DataGridView1.Rows(i).Cells(2).Value.ToString()
            PictureBox1.Image = Image.FromFile(DataGridView1.Rows(i).Cells(3).Value.ToString())
            TextBox4.Text = DataGridView1.Rows(i).Cells(3).Value.ToString()
            TextBox5.Text = DataGridView1.Rows(i).Cells(4).Value.ToString()
            TextBox6.Text = DataGridView1.Rows(i).Cells(5).Value.ToString()
        End If
    End Sub

    Sub StartCondition()
        Call Connect()
        Call StartDataGridView()

        TextBox1.ReadOnly = True
        TextBox2.ReadOnly = True
        TextBox3.ReadOnly = True
        TextBox4.ReadOnly = True
        TextBox5.ReadOnly = True
        TextBox6.ReadOnly = True

        TextBox7.ReadOnly = False

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
        Button5.Visible = False
        Button7.Enabled = True
        Button8.Enabled = True

        PictureBox1.Image = Nothing

        UserId = UserId
    End Sub

    Private Sub ManageMenu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call StartCondition()
    End Sub

    Sub TextBoxEnabler()
        TextBox2.ReadOnly = False
        TextBox3.ReadOnly = False
        TextBox5.ReadOnly = False
        TextBox6.ReadOnly = False
        TextBox7.ReadOnly = False
    End Sub

    Sub TextBoxClear()
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""
        TextBox4.Text = ""
        TextBox5.Text = ""
        TextBox6.Text = ""
        TextBox7.Text = ""
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
            Button7.Enabled = False
            Call StartDataGridView()
            TextBox7.ReadOnly = True
            Button5.Visible = True
            Button5.Enabled = True
            PictureBox1.Image = Nothing
            Button8.Enabled = False
        Else
            Dim Name As String = TextBox2.Text
            Dim Price As String = TextBox3.Text
            Dim Photo As String = TextBox4.Text
            Dim Protein As String = TextBox5.Text
            Dim Carbo As String = TextBox6.Text
            If Name = "" Or Price = "" Or Photo = "" Or Protein = "" Or Carbo = "" Then
                MessageBox.Show("Text Field Empty!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                If IsNumeric(Price) = True Then
                    If IsNumeric(Protein) = True Then
                        If IsNumeric(Carbo) = True Then
                            Call Connect()
                            Dim QueryInput As String = "INSERT INTO MsMenu (Name, Price, Photo, Protein, Carbo) VALUES ('" & Name & "','" & Price & "','" & Photo & "','" & Protein & "','" & Carbo & "')"
                            Command = New SqlCommand(QueryInput, Connection)
                            Command.ExecuteNonQuery()
                            MessageBox.Show("Input Success", "Input Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Call StartCondition()
                        Else
                            MessageBox.Show("Carbo Must Number!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End If
                    Else
                        MessageBox.Show("Protein Must Number!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                Else
                    MessageBox.Show("Price Must Number!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
            Call TextBoxEnabler()
            Call TextBoxClear()
            Button7.Enabled = False
            Call StartDataGridView()
            TextBox7.ReadOnly = True
            Button5.Enabled = False
            Button5.Visible = True
            PictureBox1.Image = Nothing
            Button8.Enabled = False
        Else
            Dim Id As String = TextBox1.Text
            Dim Name As String = TextBox2.Text
            Dim Price As String = TextBox3.Text
            Dim Photo As String = TextBox4.Text
            Dim Protein As String = TextBox5.Text
            Dim Carbo As String = TextBox6.Text
            If Name = "" Or Price = "" Or Photo = "" Or Protein = "" Or Carbo = "" Then
                MessageBox.Show("Text Field Empty!", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                If IsNumeric(Price) = True Then
                    If IsNumeric(Protein) = True Then
                        If IsNumeric(Carbo) = True Then
                            Call Connect()
                            Dim QueryUpdate As String = "UPDATE MsMenu SET Name = '" & Name & "', Price = '" & Price & "', Photo = '" & Photo & "', Protein = '" & Protein & "', Carbo = '" & Carbo & "' WHERE Id = '" & Id & "'"
                            Command = New SqlCommand(QueryUpdate, Connection)
                            Command.ExecuteNonQuery()
                            MessageBox.Show("Update Success", "Update Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Call StartCondition()
                        Else
                            MessageBox.Show("Carbo Must Number!", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End If
                    Else
                        MessageBox.Show("Protein Must Number!", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                Else
                    MessageBox.Show("Price Must Number!", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If
        End If
    End Sub


    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If Button3.Text = "Delete" Then
            Button2.Enabled = False
            Button1.Enabled = False
            Button4.Visible = True
            Button3.Text = "Remove"
            Call TextBoxClear()
            Button7.Enabled = False
            TextBox7.ReadOnly = True
            Call StartDataGridView()
            Button5.Enabled = True
            PictureBox1.Image = Nothing
            Button8.Enabled = False
        Else
            Dim Id As String = TextBox1.Text
            Dim Name As String = TextBox2.Text
            Dim Price As String = TextBox3.Text
            Dim Photo As String = TextBox4.Text
            Dim Protein As String = TextBox5.Text
            Dim Carbo As String = TextBox6.Text
            If Name = "" Or Price = "" Or Photo = "" Or Protein = "" Or Carbo = "" Then
                MessageBox.Show("Pick Data To Delete!", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                If IsNumeric(Price) = True Then
                    If IsNumeric(Protein) = True Then
                        If IsNumeric(Carbo) = True Then
                            Call Connect()
                            Dim QueryDelete As String = "DELETE FROM MsMenu WHERE Id = '" & Id & "'"
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
                            MessageBox.Show("Carbo Must Number!", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End If
                    Else
                        MessageBox.Show("Protein Must Number!", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                Else
                    MessageBox.Show("Price Must Number!", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If
        End If
    End Sub


    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Call StartCondition()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.Filter = "Image files (.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png"
        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            PictureBox1.Image = Image.FromFile(OpenFileDialog1.FileName)
            TextBox4.Text = OpenFileDialog1.FileName
        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        AdminNavigation.UserId = UserId
        Me.Hide()
        AdminNavigation.Show()
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Button1.Enabled = False
        Button2.Enabled = False
        Button3.Enabled = False
        Button4.Visible = True

        Dim Keyword As String = TextBox7.Text

        If Keyword = "" Then
            MessageBox.Show("Search Field Empty", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Call StartCondition()
        Else
            DataAdapter = New SqlDataAdapter("SELECT * FROM MsMenu WHERE Id LIKE '%" & Keyword & "%' OR Name LIKE '%" & Keyword & "%' OR Price LIKE '%" & Keyword & "%' OR Carbo LIKE '%" & Keyword & "%' OR Protein LIKE '%" & Keyword & "%'", Connection)
            DataSet = New DataSet
            DataSet.Clear()
            DataAdapter.Fill(DataSet, "MsMenu")
            If DataSet.Tables("MsMenu").Rows.Count > 0 Then
                TextBox1.Text = ""
                TextBox2.Text = ""
                TextBox3.Text = ""
                TextBox4.Text = ""
                TextBox5.Text = ""
                TextBox6.Text = ""
                PictureBox1.Image = Nothing
                'MessageBox.Show(DataSet.Tables("MsMember").Rows(0).Item("Name"))
                DataGridView1.DataSource = (DataSet.Tables("MsMenu"))
            Else
                MessageBox.Show("Keyword Not Found!", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Call StartCondition()
            End If
        End If
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""
        TextBox4.Text = ""
        TextBox5.Text = ""
        TextBox6.Text = ""
        TextBox7.Text = ""
        PictureBox1.Image = Nothing
        Button5.Enabled = False
        Call StartDataGridView()
    End Sub

End Class
