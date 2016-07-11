﻿Imports System.Net
Imports System.Collections.Specialized
Imports System.IO
Imports System.Text
Imports Pitter.norvanco.http
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class Networking
    Dim Encryption As New Encryption
    Dim settings_parent As Settings
    Dim st As New StringTool

    Dim username As String
    Dim password As String

    Sub New(ByVal passed_username As String, ByVal passed_password As String, ByVal settings As Settings)
        username = passed_username
        password = passed_password
        settings_parent = settings
    End Sub
    Public Function get_settings()

        Using client As New Net.WebClient
            Dim reqparm As New Specialized.NameValueCollection
            reqparm.Add("username", Encryption.base64_encode(username))
            reqparm.Add("password", Encryption.base64_encode(password))
            Dim responsebytes = client.UploadValues("https://api.pitter.us/settings.php", "POST", reqparm)
            Return (New System.Text.UTF8Encoding).GetString(responsebytes)
        End Using

    End Function
    Public Sub upload(ByVal filepath As String, ByVal rename As Boolean)
        If filepath <> "" And My.Computer.FileSystem.FileExists(filepath) Then
            Dim infoReader As System.IO.FileInfo
            infoReader = My.Computer.FileSystem.GetFileInfo(filepath)
            If infoReader.Length < 104857600 Then
                Try
                    If st.parse_boolean(settings_parent.getValue("beta server")) Then
                        Dim mpf As New MultipartForm("https://api.ieatass.club/upload")
                        mpf.setField("email", username)
                        mpf.setField("password", password)
                        mpf.setField("original_filename", Path.GetFileName(filepath))
                        mpf.sendFile(filepath)
                        Dim resp = mpf.ResponseText.ToString
                        responseparser(filepath, rename, resp)
                    Else
                        Dim mpf As New MultipartForm("https://api.pitter.us/scalar.php")
                        mpf.setField("username", Encryption.base64_encode(username))
                        mpf.setField("password", Encryption.base64_encode(password))
                        mpf.setField("command", "upload")
                        mpf.setField("filename", Path.GetFileName(filepath))
                        mpf.sendFile(filepath)
                        Dim resp = mpf.ResponseText.ToString
                        responseparser_legacy(filepath, rename, resp)
                    End If

                Catch ex As Exception
                    Dim ex_f As String() = ex.ToString.Split(vbNewLine)
                    WebApp.notification("Ambigious Error while Uploading", ex_f(0), 5000, ToolTipIcon.Error, False)
                End Try
            Else
                WebApp.notification("Max Filesize Exceeded", "The uploaded file exceeds 100MB in size, and cannot be processed.", 5000, ToolTipIcon.Error, False)
            End If
        End If
        WebApp.isCurrentlyUploading = False
    End Sub
    Public Sub responseparser(ByVal filepath As String, ByVal rename As Boolean, ByVal resp As String)
        If resp = Nothing Or resp = "" Then
            WebApp.notification("Error getting data from server", "Pitter was unable to get a response from the upload server.", 5000, ToolTipIcon.Error, False)
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
                        My.Computer.Clipboard.SetText("https://c.ieatass.club/" + returned_filename)
                    Else
                        'No Cache
                        If (st.parse_boolean(settings_parent.getValue("use custom server"))) Then
                            My.Computer.Clipboard.SetText(settings_parent.getValue("custom server address") + returned_filename)
                        Else
                            'don't use custom server
                            My.Computer.Clipboard.SetText("https://i.ieatass.club/" + returned_filename)
                        End If
                    End If

                    'Should we rename the file?
                    If rename = True Then
                        My.Computer.FileSystem.MoveFile(filepath, wrkdir + returned_filename)
                    End If


                    WebApp.notification(header, message, 5000, ToolTipIcon.Info, False)
                Case "warning"
                    WebApp.notification(header, message, 5000, ToolTipIcon.Warning, False)
                Case "error"
                    WebApp.notification(header, message, 5000, ToolTipIcon.Error, False)
            End Select
        End If
    End Sub
    Public Sub responseparser_legacy(ByVal filepath As String, ByVal rename As Boolean, ByVal resp As String)
        Dim response_split As String() = resp.Split(":")
        Dim fns1 As String() = response_split(2).Split("/")
        Dim fnc = fns1.Length - 1

        Dim bfn = Path.GetFileName(filepath)

        Dim wrkdir = filepath.Substring(0, filepath.Length - bfn.Length)
        Select Case response_split(0)
            Case "success"

                'Decide what URL

                'use custom server

                If (st.parse_boolean(settings_parent.getValue("endpoint caching"))) Then
                    'Cache
                    My.Computer.Clipboard.SetText("https://c.pitter.us/" + fns1(fnc))
                Else
                    'No Cache
                    If (st.parse_boolean(settings_parent.getValue("use custom server"))) Then
                        My.Computer.Clipboard.SetText(settings_parent.getValue("custom server address") + fns1(fnc))
                    Else
                        'don't use custom server
                        My.Computer.Clipboard.SetText(response_split(1) + ":" + response_split(2))
                    End If
                End If

                If rename Then
                    My.Computer.FileSystem.MoveFile(filepath, wrkdir + fns1(fnc))
                End If

                WebApp.notification("Upload Complete", "A link to the uploaded file has been added to your clipboard.", 5000, ToolTipIcon.Info, True)

            Case "failed"
                WebApp.notification("Upload Failed", "An unknown error occured while uploading the file.", 5000, ToolTipIcon.Error, False)

            Case "restricted", "suspended"
                WebApp.notification("Account Suspended", "The file you attempted to upload has been discarded.", 5000, ToolTipIcon.Warning, False)

            Case "invalid"
                WebApp.notification("Invalid Credentials", "Pitter failed to exchange credentials to the server.", 5000, ToolTipIcon.Error, False)
        End Select
    End Sub
End Class