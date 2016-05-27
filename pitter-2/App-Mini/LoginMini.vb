Public Class LoginMini
    Dim client As New Net.WebClient
    Dim settings_ As New Settings
    Dim encryption_ As New Encryption
    Dim authentication_ As New Authentication

    Private Sub LoginMini_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Change the icon
        Me.Icon = My.Resources.norm

        'Clear the error message
        em.Text = ""

        'Check if we already have credentials
        If settings_.getValue("username") <> "" And settings_.getValue("password") <> "" Then
            Dim loginResponse = login(encryption_.DPAPI_decrpyt(settings_.getValue("username")), encryption_.DPAPI_decrpyt(settings_.getValue("password")))
            If loginResponse Then
                'Load form
                PitterMini.Show()
                Me.Close()
            Else
                'Invalid Saved Session
                em.Text = "The username and password you previously have saved is now invalid"
            End If
        End If

    End Sub
    Public Function login(ByVal username As String, ByVal password As String)
        'Credentials are stored

        Dim auth_token As String

        'Attempt to get authentication token
        Try
            auth_token = authentication_.get_auth_token( _
                encryption_.base64_encode( _
                    username _
                    ), _
                encryption_.base64_encode( _
                    password _
                    ) _
                )
        Catch ex As Exception
            MsgBox("Failed to gather authentication token", MsgBoxStyle.Critical, "Pitter")
            Application.Exit()
        End Try

        If auth_token <> "false" Then
            'Logged in succeessfully
            Return True
        Else
            Return False
        End If

    End Function
    Public Sub save_login()
        settings_.setValue("username", encryption_.DPAPI_encrypt(TextBox1.Text))
        settings_.setValue("password", encryption_.DPAPI_encrypt(TextBox1.Text))
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        If TextBox1.Text.Contains("@") = False Or TextBox1.Text.Contains(".") = False Then
            em.Text = "Invalid Email Address"
            GoTo end1
        End If

        If TextBox2.Text.Length < 1 Then
            em.Text = "Invalid Password" + vbNewLine
            GoTo end1
        End If

        Dim loginResult = login(TextBox1.Text, TextBox2.Text)
        If loginResult Then
            'We logged in
            'Save Login Information
            save_login()
            PitterMini.Show()
            Me.Close()
        Else
            'Invalid username or password
            em.Text = "Invalid Username or Password"
        End If

end1:

    End Sub
End Class