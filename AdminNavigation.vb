Imports System.Data.SqlClient
Public Class AdminNavigation
    Dim Command As SqlCommand
    Dim Connection As SqlConnection
    Dim DataAdapter As SqlDataAdapter
    Dim DataReader As SqlDataReader
    Dim DataSet As DataSet
    Dim Database As String

    Public Property UserId

    Sub Connect()
        Database = "Data Source = " & Environment.MachineName.ToString() & "\SQLEXPRESS; Initial Catalog = DB_SMK_RESTAURANT_INFORMATION_SYSTEM; Integrated Security = True"
        Connection = New SqlConnection(Database)
        If Connection.State = ConnectionState.Closed Then Connection.Open()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Order.UserId = UserId
        Me.Hide()
        Order.Show()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ManageMenu.UserId = UserId
        Me.Hide()
        ManageMenu.Show()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ManageMember.UserId = UserId
        Me.Hide()
        ManageMember.Show()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Login.StartCondition()
        Me.Hide()
        Login.Show()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Report.UserId = UserId
        Me.Hide()
        Report.Show()
    End Sub

    Private Sub AdminNavigation_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call StartCondition()
    End Sub

    Sub StartCondition()
        Call Connect()
        Dim Id As String = UserId
        Dim QuerySearch As String = "SELECT * FROM MsEmployee WHERE Id = '" & Id & "'"
        Command = New SqlCommand(QuerySearch, Connection)
        DataReader = Command.ExecuteReader()

        If DataReader.HasRows Then
            While DataReader.Read()
                Label2.Text = "Welcome, " & DataReader("Name").ToString
            End While
        Else
        End If
    End Sub

End Class