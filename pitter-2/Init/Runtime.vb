Imports System.IO
Imports System.Environment
Public Class Runtime
    Public working_directory = GetFolderPath(SpecialFolder.ApplicationData) + "\Pitter\"
    Private Sub Runtime_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dircheck()
        processcheck()
        decision()
        Me.Close()

    End Sub
    Public Sub decision()
        If My.Computer.FileSystem.FileExists(Environment.CurrentDirectory + "\Awesomium.dll") Then
            WebApp.Show()
        Else
            'Mini
            MsgBox("The Mini Version of Pitter is still in development, and is not accessable at this time.")
        End If
    End Sub

    Public Sub processcheck()
        'Process Check
        Dim p = Process.GetProcessesByName("pitter")
        If p.Length > 1 Then
            Dim di As New NotifyIcon
            di.BalloonTipIcon = ToolTipIcon.Error
            di.BalloonTipTitle = "Pitter is already running"
            di.BalloonTipText = "Your system has indicated that pitter is already running in the background."
            di.Icon = My.Resources.norm
            di.ShowBalloonTip(5000)
            Threading.Thread.Sleep(5000)
            Application.Exit()
        End If

    End Sub

    Public Sub dircheck()
        If My.Computer.FileSystem.DirectoryExists(working_directory) = False Then
            MkDir(working_directory)
        End If
    End Sub
End Class