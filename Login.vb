Imports System.Data.SqlClient
Imports System.Text.RegularExpressions

Public Class Login
    Dim Command As SqlCommand
    Dim Connection As SqlConnection
    Dim DataAdapter As SqlDataAdapter
    Dim DataReader As SqlDataReader
    Dim DataSet As DataSet
    Dim Database As String

    Sub Connect()
        Database = "Data Source = " & Environment.MachineName.ToString() & "\SQLEXPRESS; Initial Catalog = DB_SMK_RESTAURANT_INFORMATION_SYSTEM; Integrated Security = True"
        Connection = New SqlConnection(Database)
        If Connection.State = ConnectionState.Closed Then Connection.Open()
    End Sub

    Sub StartCondition()
        TextBox1.Text = ""
        TextBox2.Text = ""
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim Email As String = TextBox1.Text
        Dim Password As String = TextBox2.Text
        If Email = "" Or Password = "" Then
            MessageBox.Show("Email And Password Field Cannot Be Empty!", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            If EmailAddressCheck(Email) = True Then
                Call Connect()
                Dim QuerySearch As String = "SELECT * FROM MsEmployee WHERE Email = '" & Email & "'"
                Command = New SqlCommand(QuerySearch, Connection)
                DataReader = Command.ExecuteReader()
                If DataReader.HasRows Then
                    While DataReader.Read()
                        Dim UserId As String = DataReader("Id").ToString
                        Dim UserEmail As String = DataReader("Email").ToString
                        Dim UserPassword As String = DataReader("Password").ToString
                        Dim UserPosition As String = DataReader("Position").ToString

                        If Password = UserPassword.Trim() Then
                            If UserPosition = "admin" Then
                                Dim AdminNavigation As New AdminNavigation
                                AdminNavigation.UserId = UserId
                                Me.Hide()
                                AdminNavigation.Show()
                            ElseIf UserPosition = "cashier" Then
                                Dim CashierNavigation As New CashierNavigation
                                CashierNavigation.UserId = UserId
                                Me.Hide()
                                CashierNavigation.Show()
                            Else
                                MessageBox.Show("Position Is Not Authorization!", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End If
                        Else
                            MessageBox.Show("Wrong Password!", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End If

                    End While
                Else
                    MessageBox.Show("Email Is Not Registered!", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            Else
                MessageBox.Show("Inputted Email Not Valid!", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

    Private Sub Login_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call StartCondition()
    End Sub

End Class