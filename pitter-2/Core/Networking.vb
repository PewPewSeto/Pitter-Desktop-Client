Imports System.Net
Imports System.Collections.Specialized
Imports System.IO
Imports System.Text
Imports Pitter.norvanco.http

Public Class Networking
    Dim Encryption As New Encryption
    Dim fd As New FormDecision

    Dim username As String
    Dim password As String

    Sub New(ByVal passed_username As String, ByVal passed_password As String)
        username = passed_username
        password = passed_password
    End Sub
    Public Function get_settings()

    End Function
    Public Sub upload(ByVal filepath As String)
        If filepath <> "" Then
            Dim infoReader As System.IO.FileInfo
            infoReader = My.Computer.FileSystem.GetFileInfo(filepath)
            If infoReader.Length < 104857600 Then
                Try
                    Dim mpf As New MultipartForm("https://api.pitter.us/scalar.php")
                    mpf.setField("username", Encryption.base64_encode(username))
                    mpf.setField("password", Encryption.base64_encode(password))
                    mpf.setField("command", "upload")
                    mpf.setField("filename", Path.GetFileName(filepath))
                    mpf.sendFile(filepath)

                    Dim resp = mpf.ResponseText.ToString

                    Dim response_split As String() = resp.Split(":")

                    Select Case response_split(0)
                        Case "success"
                            My.Computer.Clipboard.SetText(response_split(1) + ":" + response_split(2))

                            Dim fns1 As String() = response_split(2).Split("/")
                            Dim fnc = fns1.Length - 1

                            Dim bfn = Path.GetFileName(filepath)
                            Dim wrkdir = filepath.Substring(0, filepath.Length - bfn.Length)

                            My.Computer.FileSystem.MoveFile(filepath, wrkdir + fns1(fnc))

                            fd.choice.notification("Upload Complete", "A link to the uploaded file has been added to your clipboard.", 5000, ToolTipIcon.Info, True)

                        Case "failed"
                            fd.choice.notification("Upload Failed", "An unknown error occured while uploading the file.", 5000, ToolTipIcon.Error, False)

                        Case "restricted"
                            fd.choice.notification("Account Suspended", "The file you attempted to upload has been discarded.", 5000, ToolTipIcon.Warning, False)

                        Case "invalid"
                            fd.choice.notification("Invalid Credentials", "Pitter failed to exchange credentials to the server.", 5000, ToolTipIcon.Error, False)
                    End Select

                Catch ex As Exception
                    Dim ex_f As String() = ex.ToString.Split(vbNewLine)
                    fd.choice.notification("Ambigious Error while Uploading", ex_f(0), 5000, ToolTipIcon.Error, False)
                    MsgBox(ex.ToString)
                End Try
            Else
                fd.choice.notification("Max Filesize Exceeded", "The uploaded file exceeds 100MB in size, and cannot be processed.", 5000, ToolTipIcon.Error, False)
            End If
        End If
        fd.choice.isCurrentlyUploading = False
    End Sub
End Class