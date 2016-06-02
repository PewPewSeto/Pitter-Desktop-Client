Imports System.Runtime.InteropServices
Imports System.Environment
Imports System.IO
Imports System.Net

Public Class WebApp

    Dim Auth_ As New Authentication
    Dim Encryption_ As New Encryption
    Dim Settings_ As New Settings
    Dim Character As New CharacterMapping
    Dim StringTool As New StringTool
    Dim Networking As New Networking(Encryption_.DPAPI_decrpyt(Settings_.getValue("username")), Encryption_.DPAPI_decrpyt(Settings_.getValue("password")), Settings_)
    Dim Capture_ As New Capture(Encryption_, Settings_, Networking, StringTool)
    Dim Synchronization As New Synchronization(Networking, Settings_)
    Dim updater As New Updater(Me, Settings_)

    Public listeningForInput As Boolean = False
    Dim listeningEnabled As Boolean = True
    Public isCurrentlyUploading As Boolean = False

    Public settings_locked As Boolean

    Public save_location As String = My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\Pitter\"
    Dim osInfo As System.OperatingSystem = System.Environment.OSVersion
    Public passbackSettingsUpdated As Boolean = False

    <DllImport("user32.dll")> Shared Function GetAsyncKeyState(ByVal vKey As System.Windows.Forms.Keys) As Short
    End Function
    Public Sub notification(ByVal title As String, ByVal message As String, ByVal time As Integer, ByVal icon As ToolTipIcon, ByVal chime As Boolean)
        TaskbarIcon.BalloonTipIcon = icon 'Icon
        TaskbarIcon.BalloonTipTitle = title 'Title
        TaskbarIcon.BalloonTipText = message 'Body
        TaskbarIcon.ShowBalloonTip(time) 'Time we should dispaly for
        If chime And StringTool.parse_boolean(Settings_.getValue("chime")) = True Then 'Chime was true
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

    Public Sub init_resize()
        Me.Size = New Size(My.Computer.Screen.WorkingArea.Width * 0.85, My.Computer.Screen.WorkingArea.Height * 0.85)
        Me.Location = New Size(My.Computer.Screen.WorkingArea.Width * 0.05, My.Computer.Screen.WorkingArea.Height * 0.05)
    End Sub
    Public Function login_routine()
        If Settings_.getValue("username") <> "" And Settings_.getValue("password") <> "" Then
            'Credentials are stored
            Dim decrpyted_username = Encryption_.DPAPI_decrpyt(Settings_.getValue("username"))
            Dim decrpyted_password = Encryption_.DPAPI_decrpyt(Settings_.getValue("password"))

            Dim auth_token As String

            'Attempt to get authentication token
            Try
                auth_token = Auth_.get_auth_token( _
                    Encryption_.base64_encode( _
                        decrpyted_username _
                        ), _
                    Encryption_.base64_encode( _
                        decrpyted_password _
                        ) _
                    )
            Catch ex As Exception
                MsgBox("Failed to gather authentication token", MsgBoxStyle.Critical, "Pitter")
                Application.Exit()
            End Try

            'Check to make sure that the token is not false
            If auth_token <> "false" Then
                'we have a valid auth token
                WebControl1.Source = New Uri("https://panel.pitter.us/login.php?token=" + auth_token)
                listeningForInput = True

                'Start Sync Thread
                Synchronization.sync()
                Synchronization.updateThread()

                'Start updater thread
                updater.start_updater_thread()

                Return True
            End If
        Else
            'Credentials are NOT stored
            Return False
        End If
    End Function

    Public Sub grab_login_information()

        'Grab email and password
        Dim g_username As String = WebControl1.ExecuteJavascriptWithResult("$(""#email"").val()")
        Dim g_password As String = WebControl1.ExecuteJavascriptWithResult("$(""#password"").val()")

        'Encrypt
        Dim e_g_username = Encryption_.DPAPI_encrypt(g_username)
        Dim e_g_password = Encryption_.DPAPI_encrypt(g_password)

        'Store
        Settings_.setValue("username", e_g_username)
        Settings_.setValue("password", e_g_password)

    End Sub

    Private Sub Pitter_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed

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

    Private Sub Pitter_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Check for other pitters
        check_for_other_pitters()

        'check for updates before continuing
        updater.check_for_update()

        'Check to see if the save directory exists
        directory_check()

        'Set icon
        Me.Icon = My.Resources.norm
        TaskbarIcon.Icon = My.Resources.norm

        'Resize the window
        init_resize()

        'Check if we need to prompt for login
        If login_routine() = False Then
            WebControl1.Source = New Uri("https://panel.pitter.us/dashboard")
        End If

        Me.Close()

    End Sub

    Private Sub WebControl1_MouseClick(sender As Object, e As MouseEventArgs) Handles WebControl1.MouseClick
        Try
            Dim element_id = (WebControl1.ExecuteJavascriptWithResult("document.elementFromPoint(parseInt(" + e.X.ToString + "), parseInt(" + e.Y.ToString + ")).id;").ToString)
            Select Case element_id 'Conditional based on ID
                Case "submit-b" 'Element with ID was clicked
                    If WebControl1.Source = New Uri("https://panel.pitter.us/") Or WebControl1.Source = New Uri("https://panel.pitter.us/login.php") Then 'Check for login url
                        grab_login_information()
                    End If

            End Select
        Catch ex As Exception
            'simple suppress
        End Try

    End Sub

    Private Sub EventListener_Tick(sender As Object, e As EventArgs) Handles BrowserEventListener.Tick
        Try
            'Check if loading
            If WebControl1.IsLoading = False Then 'Make sure we are not loading
                'Clear memory footprint

                'Hidden Login Check
                If WebControl1.Source = New Uri("https://panel.pitter.us/") Or WebControl1.Source = New Uri("https://panel.pitter.us/login.php") Then 'Check for login url
                    Try
                        'Check for hidden variable
                        Dim h_var As String = WebControl1.ExecuteJavascriptWithResult("$(""#pressed"").val()")
                        'Check if h_var is true
                        If h_var = "true" Then
                            grab_login_information()
                            login_routine()
                        End If
                    Catch ex As Exception
                        'suppress
                    End Try
                End If
            End If
        Catch ex As Exception

        End Try
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
                    Try
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
                    Catch ex As Exception
                        Synchronization.sync() 'We might not have settings yet, download them.
                    End Try

                    '1 END
                End If
            Next
        End If
    End Sub
    Private Sub Passive_Tick(sender As Object, e As EventArgs) Handles Passive.Tick
        'This timer should only be used for passive variables or any other form of background processes
        settings_locked = Settings_.locked

        If StringTool.parse_boolean(Settings_.getValue("notify sync")) Then
            Settings_.setValue("notify sync", "false")
            notification("Settings Synchronized", "Settings from the cloud have been saved to this machine.", 5000, ToolTipIcon.Info, False)
        End If
    End Sub

    Private Sub Cleaner_Tick(sender As Object, e As EventArgs) Handles Cleaner.Tick
        Try
            WebControl1.ReduceMemoryUsage()
        Catch ex As Exception
            'This function will sometimes be called, but it is safe to suppress.
        End Try
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        killproc()
    End Sub
    Public Sub restore()
        Me.WindowState = FormWindowState.Normal
        Me.ShowInTaskbar = True
        Me.BringToFront()
    End Sub
    Private Sub TaskbarIcon_MouseClick(sender As Object, e As MouseEventArgs) Handles TaskbarIcon.MouseClick
        If StringTool.parse_boolean(Settings_.getValue("tray click twice to open")) = False Then
            restore()
        End If
    End Sub

    Private Sub TaskbarIcon_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles TaskbarIcon.MouseDoubleClick
        If StringTool.parse_boolean(Settings_.getValue("tray click twice to open")) Then
            restore()
        End If
    End Sub
    Private Sub FileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FileToolStripMenuItem.Click
        Capture_.uploadFile()
    End Sub

    Private Sub ClipboardToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ClipboardToolStripMenuItem.Click
        Capture_.captureClipboard()
    End Sub

    Private Sub CurrentWindowToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CurrentWindowToolStripMenuItem.Click
        Capture_.CaptureWindow(Nothing)
    End Sub

    Private Sub FullscreenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FullscreenToolStripMenuItem.Click
        Capture_.captureFullScreen()
    End Sub

    Private Sub SelectionToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SelectionToolStripMenuItem.Click
        If StringTool.parse_boolean(Settings_.getValue("use old selector")) = False Then
            Modern.Show()
        Else
            Legacy.Show()
        End If
    End Sub

    Private Sub ForceUpdateToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ForceUpdateToolStripMenuItem.Click
        
    End Sub

    Private Sub SynchronizeSettingsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SynchronizeSettingsToolStripMenuItem.Click
        Synchronization.sync()
    End Sub
    Public Sub check_for_other_pitters()
        Dim current_pid = Process.GetCurrentProcess.Id

        For Each p As Process In Process.GetProcesses
            If p.ProcessName = "pitter" And p.Id <> current_pid Then
                p.Kill()
            End If
        Next
    End Sub
End Class