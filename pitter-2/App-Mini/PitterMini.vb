Imports System.Runtime.InteropServices
Imports System.Environment
Imports System.IO
Imports System.Net
Public Class PitterMini

    Dim Auth_ As New Authentication
    Dim Encryption_ As New Encryption
    Dim Settings_ As New Settings
    Dim Character As New CharacterMapping
    Dim StringTool As New StringTool
    Dim Networking As New Networking(Encryption_.DPAPI_decrpyt(Settings_.getValue("username")), Encryption_.DPAPI_decrpyt(Settings_.getValue("password")), Settings_)
    Dim Capture_ As New Capture(Encryption_, Settings_, Networking, StringTool)
    Dim Synchronization As New Synchronization(Networking, Settings_)

    Public listeningForInput As Boolean = False
    Dim listeningEnabled As Boolean = True
    Public isCurrentlyUploading As Boolean = False

    Public settings_locked As Boolean

    Public save_location As String = "C:\Users\" + Environment.UserName + "\My Documents\Pitter\"
    Dim osInfo As System.OperatingSystem = System.Environment.OSVersion
    <DllImport("user32.dll")> Shared Function GetAsyncKeyState(ByVal vKey As System.Windows.Forms.Keys) As Short
    End Function
    Public Sub notification(ByVal title As String, ByVal message As String, ByVal time As Integer, ByVal icon As ToolTipIcon, ByVal chime As Boolean)
        TaskbarIcon.BalloonTipIcon = icon 'Icon
        TaskbarIcon.BalloonTipTitle = title 'Title
        TaskbarIcon.BalloonTipText = message 'Body
        TaskbarIcon.ShowBalloonTip(time) 'Time we should dispaly for
        If chime Then 'Chime was true
            If osInfo.Version.Major <> 10 Then 'Check if we're running 10
                'If we're not 10, then we should chime.
                'Windows 10 has a built in chime, and would be bad if we used redundancy.
                My.Computer.Audio.Play(My.Resources.complete, AudioPlayMode.Background) 'Play the chime
            End If
        End If
    End Sub

    Public Function get_image_save_type(ByVal format_only As Boolean)

        If format_only Then
            Dim t_r As String = Settings_.getValue("image format")
            If t_r = "" Then
                'Not defined in settings, append it and retry.
                Settings_.setValue("image format", "jpg") 'By default for now, we should use jpg
                Return get_image_save_type(True) 'Redirection loop, will grab the function in an iteration and grab the new value after saving the settings.
            Else
                Return t_r
            End If
        Else
            Select Case Settings_.getValue("image format")

                Case "png", "PNG"
                    Return System.Drawing.Imaging.ImageFormat.Png
                Case "jpg", "JPG", "jpeg", "JPEG"
                    Return System.Drawing.Imaging.ImageFormat.Jpeg
                Case Else
                    'Not defined in settings, append it and retry.
                    Settings_.setValue("image format", "jpg") 'By default for now, we should use jpg
                    Return get_image_save_type(False) 'Redirection loop, will grab the function in an iteration and grab the new value after saving the settings.
            End Select
        End If

    End Function
    Public Sub directory_check()
        If My.Computer.FileSystem.DirectoryExists(save_location) = False Then
            MkDir(save_location)
        End If
    End Sub
    Private Sub PitterMini_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Change Icon
        Me.Icon = My.Resources.norm
        listeningForInput = True
        Me.Show()

    End Sub

    Private Sub ContextMenuStrip1_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip1.Opening

    End Sub
    Public Sub killproc()
        Settings_.locked = True
        'kill proc
        Dim location As String = System.Environment.GetCommandLineArgs()(0)
        Dim appName As String = System.IO.Path.GetFileName(location)
        Dim proc = Process.GetProcessesByName(appName.Substring(0, appName.Length - 4))
        For i As Integer = 0 To proc.Count - 1
            proc(i).Kill()
        Next i
    End Sub
    Private Sub Pitter_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        'PROCESS CHECK

        Dim p = Process.GetProcessesByName("explorer")
        If p.Length = 0 Then
            killproc()
        Else
            Me.WindowState = FormWindowState.Minimized
            Me.ShowInTaskbar = False
            e.Cancel = True
        End If

    End Sub
    Private Sub DesktopEventListener_Tick(sender As Object, e As EventArgs) Handles DesktopEventListener.Tick
        'Todo: HEAVILY MODIFY

        If listeningForInput = True And isCurrentlyUploading = False And listeningForInput = True Then
            Dim Result As Integer
            For i = 1 To 255
                Result = GetAsyncKeyState(i)
                If Result = -32767 Then

                    '1 START
                    'Check if we need to check for CTRL and SHIFT
                    If StringTool.parse_boolean(Settings_.getValue("use control and shift")) = True Then
                        'We need CTRL and SHIFT
                        If (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown) Then

                            If GetAsyncKeyState(Character.formkeys(Character.ASCIIDESCtokey(Integer.Parse(Settings_.getValue("key 1"))))) <> 0 Then
                                Capture_.uploadFile()
                            End If
                            If GetAsyncKeyState(Character.formkeys(Character.ASCIIDESCtokey(Integer.Parse(Settings_.getValue("key 2"))))) Then
                                Capture_.CaptureWindow(Nothing)
                            End If
                            If GetAsyncKeyState(Character.formkeys(Character.ASCIIDESCtokey(Integer.Parse(Settings_.getValue("key 3"))))) And StringTool.parse_boolean(Settings_.getValue("printscreen key means fullscreen")) = False Then
                                'fullscreen
                                Capture_.captureFullScreen()
                            End If
                            If GetAsyncKeyState(Character.formkeys(Character.ASCIIDESCtokey(Integer.Parse(Settings_.getValue("key 4"))))) And StringTool.parse_boolean(Settings_.getValue("printscreen key means selection")) = False Then
                                'Selector
                                If StringTool.parse_boolean(Settings_.getValue("use old selector")) = False Then
                                    Modern.Show()
                                Else
                                    Legacy.Show()
                                End If
                            End If

                            If GetAsyncKeyState(Character.formkeys(Character.ASCIIDESCtokey(Integer.Parse(Settings_.getValue("key 5"))))) Then
                                Capture_.captureClipboard()
                            End If
                        End If
                    Else
                        'We do not need CTRL and SHIFT
                        If GetAsyncKeyState(Character.formkeys(Character.ASCIIDESCtokey(Integer.Parse(Settings_.getValue("key 1"))))) <> 0 Then
                            Capture_.uploadFile()
                        End If
                        If GetAsyncKeyState(Character.formkeys(Character.ASCIIDESCtokey(Integer.Parse(Settings_.getValue("key 2"))))) <> 0 Then
                            Capture_.CaptureWindow(Nothing)
                        End If
                        If GetAsyncKeyState(Character.formkeys(Character.ASCIIDESCtokey(Integer.Parse(Settings_.getValue("key 3"))))) <> 0 And StringTool.parse_boolean(Settings_.getValue("printscreen key means fullscreen")) = False Then
                            'fullscreen
                            Capture_.captureFullScreen()
                        End If
                        If GetAsyncKeyState(Character.formkeys(Character.ASCIIDESCtokey(Integer.Parse(Settings_.getValue("key 4"))))) <> 0 And StringTool.parse_boolean(Settings_.getValue("printscreen key means selection")) = False Then
                            'Selector
                            If StringTool.parse_boolean(Settings_.getValue("use old selector")) = False Then
                                Modern.Show()
                            Else
                                Legacy.Show()
                            End If
                        End If

                        If GetAsyncKeyState(Character.formkeys(Character.ASCIIDESCtokey(Integer.Parse(Settings_.getValue("key 5"))))) <> 0 Then
                            Capture_.captureClipboard()
                        End If
                    End If

                    If Integer.Parse(Settings_.getValue("printscreen key means")) = 1 Then
                        'Fullscreen
                        If GetAsyncKeyState(Keys.PrintScreen) Then 'PrintScreen Key
                            'Fullscreen
                            Capture_.captureFullScreen()
                        End If
                    ElseIf Integer.Parse(Settings_.getValue("printscreen key means")) = 2 Then
                        'Selector
                        If GetAsyncKeyState(Keys.PrintScreen) Then 'PrintScreen Key
                            'Selector
                            If StringTool.parse_boolean(Settings_.getValue("use old selector")) = False Then
                                Modern.Show()
                            Else
                                Legacy.Show()
                            End If
                        End If
                    End If
                    '1 END
                End If
            Next
        End If
    End Sub

End Class