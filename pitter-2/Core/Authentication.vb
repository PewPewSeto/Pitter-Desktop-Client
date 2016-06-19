Public Class Authentication
    Dim stringtool As New StringTool
    Dim settings As Settings

    Public Sub New(ByVal p_settings As Settings)
        settings = p_settings
    End Sub
    Public Function get_auth_token(ByVal username As String, ByVal password As String)
        Try
            Dim response As String
            Using client As New Net.WebClient
                client.Headers("User-Agent") = ("PitterClient/1.0")
                Debugger.Log(1, 1, "Logging In..." + vbNewLine)
                Dim reqparm As New Specialized.NameValueCollection
                reqparm.Add("email", username)
                reqparm.Add("password", password)
                Debugger.Log(1, 1, "Username: " + (username) + vbNewLine)
                Debugger.Log(1, 1, "Password: " + (password) + vbNewLine)
                Debugger.Log(1, 1, "Posting to API Server..." + vbNewLine)
                Dim responsebytes As Byte()
                If stringtool.parse_boolean(settings.getValue("beta server")) Then
                    responsebytes = client.UploadValues(("https://panel.ieatass.club/api/auth"), "POST", reqparm)
                Else
                    responsebytes = client.UploadValues(("https://api.pitter.us/login.php"), "POST", reqparm)
                End If
                response = (New System.Text.UTF8Encoding).GetString(responsebytes).Replace(" ", "")

            End Using
            If response = "false" Or response = "" Then
                Debugger.Log(1, 1, "Server did not respond with a 1TU token - Invalid Username or Password" + vbNewLine)
                Return "false"
            Else
                Debugger.Log(1, 1, "Server has responded with 1TU token: " + response + vbNewLine)
                Return response
            End If
        Catch ex As Exception
            MsgBox("Pitter's frontend server's appear to be offline at this time." + vbNewLine + "We are sorry for the inconvenience and hope to be back up soon.", MsgBoxStyle.Information, "Pitter Server Status")
            MsgBox(ex.ToString)
            Application.Exit()

        End Try
    End Function
End Class