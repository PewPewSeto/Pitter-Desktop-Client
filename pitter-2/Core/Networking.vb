Imports System.Net
Imports System.Collections.Specialized
Imports System.IO
Imports System.Text
Imports Pitter.norvanco.http
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class Networking
    Dim parent As WebApp
    Dim Encryption As New Encryption
    Dim settings_parent As Settings
    Dim st As New StringTool

    Dim username As String
    Dim password As String

    Sub New(ByVal passedparent As WebApp, ByVal passed_username As String, ByVal passed_password As String, ByVal settings As Settings)
        parent = passedparent
        username = passed_username
        password = passed_password
        settings_parent = settings
    End Sub
    Public Function get_settings()

        Using client As New Net.WebClient
            client.Headers("User-Agent") = ("PitterClient/1.0")
            Dim reqparm As New Specialized.NameValueCollection
            reqparm.Add("username", username)
            reqparm.Add("password", password)
            Dim responsebytes = client.UploadValues("https://panel.pitter.us/api/user/settings/client", "POST", reqparm)
            Return (New System.Text.UTF8Encoding).GetString(responsebytes)
        End Using

    End Function
    Public Sub upload(ByVal filepath As String, ByVal rename As Boolean)

        If My.Computer.Network.IsAvailable Then
            If filepath <> "" And My.Computer.FileSystem.FileExists(filepath) Then
                Dim infoReader As System.IO.FileInfo
                infoReader = My.Computer.FileSystem.GetFileInfo(filepath)
                If infoReader.Length < 104857600 Then
                    Try
                        Dim mpf As New MultipartForm("https://api.pitter.us/upload")
                        mpf.setField("email", username)
                        mpf.setField("password", password)
                        mpf.setField("original_filename", Path.GetFileName(filepath))
                        mpf.sendFile(filepath)
                        Dim resp = mpf.ResponseText.ToString
                        responseparser(filepath, rename, resp)

                    Catch ex As Exception
                        Dim ex_f As String() = ex.ToString.Split(vbNewLine)
                        parent.notification("Ambigious Error while Uploading", ex_f(0), 5000, ToolTipIcon.Error, False)
                    End Try
                Else
                    parent.notification("Max Filesize Exceeded", "The uploaded file exceeds 100MB in size, and cannot be processed.", 5000, ToolTipIcon.Error, False)
                End If
            End If
        Else
            parent.notification("Internet Conntection Unavailable", "Pitter is unable to reach our servers. Please check that you are connected to the internet.", 5000, ToolTipIcon.Error, False)
        End If
        WebApp.isCurrentlyUploading = False
    End Sub
    Public Sub responseparser(ByVal filepath As String, ByVal rename As Boolean, ByVal resp As String)
        If resp = Nothing Or resp = "" Then
            parent.notification("Error getting data from server", "Pitter was unable to get a response from the upload server.", 5000, ToolTipIcon.Error, False)
        Else
            My.Computer.Clipboard.SetText(resp)
            Dim parsed_json As JObject = JObject.Parse(resp)

            'Grab basic data.
            Dim header As String = parsed_json.GetValue("title")
            Dim message As String = parsed_json.GetValue("body")

            Select Case parsed_json.GetValue("status")
                Case "success"
                    'Information tracking
                    Dim bfn = Path.GetFileName(filepath)
                    Dim wrkdir = filepath.Substring(0, filepath.Length - bfn.Length)

                    'Grab the new filename from the json response
                    Dim returned_filename As String = parsed_json.GetValue("file")

                    'Endpoint Decision - Clipboard Setter
                    If (st.parse_boolean(settings_parent.getValue("endpoint caching"))) Then
                        'Cache
                        My.Computer.Clipboard.SetText("https://c.pitter.us/" + returned_filename)
                    Else
                        'No Cache
                        If (st.parse_boolean(settings_parent.getValue("use custom server"))) Then
                            My.Computer.Clipboard.SetText(settings_parent.getValue("custom server address") + returned_filename)
                        Else
                            'don't use custom server
                            My.Computer.Clipboard.SetText("https://i.pitter.us/" + returned_filename)
                        End If
                    End If

                    'Should we rename the file?
                    If rename = True Then
                        My.Computer.FileSystem.MoveFile(filepath, wrkdir + returned_filename)
                    End If

                    parent.notification(header, message, 5000, ToolTipIcon.Info, False)
                Case "warning"
                    parent.notification(header, message, 5000, ToolTipIcon.Warning, False)
                Case "error"
                    parent.notification(header, message, 5000, ToolTipIcon.Error, False)
            End Select
        End If
    End Sub

End Class