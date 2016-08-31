Imports Newtonsoft
Imports Newtonsoft.Json.Linq

Public Class Authentication
    Dim stringtool As New StringTool
    Dim settings As Settings
    Dim parent As WebApp

    Public Sub New(ByVal parent_p As WebApp, ByVal p_settings As Settings)
        parent = parent_p
        settings = p_settings
    End Sub
    Public Function get_auth_token(ByVal username As String, ByVal password As String, ByVal reinit As Boolean)
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
                responsebytes = client.UploadValues(("https://panel.pitter.us/api/auth"), "POST", reqparm)
                response = (New System.Text.UTF8Encoding).GetString(responsebytes)

                Dim parsed_response = JObject.Parse(response)

                Select Case parsed_response.GetValue("status")
                    Case "error"
                        Debugger.Log(1, 1, "Server did not respond with a 1TU token - Invalid Username or Password" + vbNewLine)
                        Throw New Exception(parsed_response.GetValue("message"))
                    Case "success"
                        Debugger.Log(1, 1, "Server has responded with 1TU token: " + response + vbNewLine)
                        Return parsed_response.GetValue("token")
                End Select
            End Using
        Catch ex As Exception
            If reinit = False Then
                MsgBox("There was an error while attempting to contact our servers. Please try launching pitter again." + vbNewLine + vbNewLine + ex.ToString, MsgBoxStyle.Information, "Pitter Server Status")
                parent.killproc()
            End If
        End Try
    End Function
End Class