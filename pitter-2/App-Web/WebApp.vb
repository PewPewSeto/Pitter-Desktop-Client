﻿Imports System.Runtime.InteropServices
Imports System.Environment
Imports System.IO
Imports System.Net
Imports System.Security.Principal
Imports Microsoft.Win32
Imports System.ComponentModel

Public Class WebApp
    Dim hashengine As New HashEngine
    Dim Encryption_ As New Encryption
    Dim Settings_ As New Settings
    Dim Auth_ As New Authentication(Me, Settings_)
    Dim Character As New CharacterMapping
    Dim StringTool As New StringTool
    Dim Networking As Networking 'REDIM
    Dim Capture_ As Capture
    Dim Synchronization As Synchronization
    Dim updater As New Updater(Me, Settings_)

    Public listeningForInput As Boolean = False
    Dim listeningEnabled As Boolean = True
    Public isCurrentlyUploading As Boolean = False
    Dim auth_init As Boolean = False
    Public settings_locked As Boolean

    Public save_location As String = My.Computer.FileSystem.SpecialDirectories.MyDocuments + "\Pitter\"
    Dim osInfo As System.OperatingSystem = System.Environment.OSVersion
    Public passbackSettingsUpdated As Boolean = False
    Dim pil_modes As String() = {"Pause Keybind Listener", "Listen for Keybinds"}

    Dim identity = WindowsIdentity.GetCurrent()
    Dim principal = New WindowsPrincipal(identity)
    Dim isElevated As Boolean = principal.IsInRole(WindowsBuiltInRole.Administrator)

    Dim parentProcess As System.Diagnostics.Process = System.Diagnostics.Process.GetCurrentProcess()
    Dim clicked_link As String

    <DllImport("user32.dll")> Shared Function GetAsyncKeyState(ByVal vKey As System.Windows.Forms.Keys) As Short
    End Function
    Public Sub notification(ByVal title As String, ByVal message As String, ByVal time As Integer, ByVal icon As ToolTipIcon, ByVal chime As Boolean)
        TaskbarIcon.BalloonTipIcon = icon 'Icon
        TaskbarIcon.BalloonTipTitle = title 'Title
        TaskbarIcon.BalloonTipText = message 'Body
        TaskbarIcon.ShowBalloonTip(time) 'Time we should dispaly for

        'Check if we should Chime.
        If chime And StringTool.parse_boolean(Settings_.getValue("chime")) = True Then 'Chime was true
            My.Computer.Audio.Play(My.Resources.complete, AudioPlayMode.Background) 'Play the chime
        End If

        'Reset the relog timer after uploading.
        BrowserRelogEvent.Stop()
        BrowserRelogEvent.Start()

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
            Select Case Settings_.getValue("image format").ToString.ToLower

                Case "png"
                    Return System.Drawing.Imaging.ImageFormat.Png
                Case "jpg", "jpeg"
                    Return System.Drawing.Imaging.ImageFormat.Jpeg
                Case "bmp"
                    Return System.Drawing.Imaging.ImageFormat.Bmp
                Case "tiff"
                    Return System.Drawing.Imaging.ImageFormat.Tiff
                Case "gif"
                    Return System.Drawing.Imaging.ImageFormat.Gif
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
        Me.Location = New Size(My.Computer.Screen.WorkingArea.Width * 0.065, My.Computer.Screen.WorkingArea.Height * 0.065)
    End Sub
    Public Function login_routine()
        If Settings_.getValue("username") <> "" And Settings_.getValue("password") <> "" Then
            'Credentials are stored
            Dim decrpyted_username = Encryption_.DPAPI_decrpyt(Settings_.getValue("username"))
            Dim decrpyted_password = Encryption_.DPAPI_decrpyt(Settings_.getValue("password"))

            Dim auth_token As String = ""

            'Attempt to get authentication token
            Try
                auth_token = Auth_.get_auth_token(
                    decrpyted_username,
                    decrpyted_password,
                    auth_init
                    )
            Catch ex As Exception
                MsgBox("Failed to gather authentication token", MsgBoxStyle.Critical, "Pitter")
                Application.Exit()
            End Try

            'Check to make sure that the token is not false
            If auth_token <> "false" Then
                'we have a valid auth token

                WebBrowser1.Navigate("https://panel.pitter.us/api/auth/token/" + auth_token)

                listeningForInput = True

                'REDIM
                Networking = New Networking(Me, Encryption_.DPAPI_decrpyt(Settings_.getValue("username")), Encryption_.DPAPI_decrpyt(Settings_.getValue("password")), Settings_)
                Capture_ = New Capture(Me, Encryption_, Settings_, Networking, StringTool)
                Synchronization = New Synchronization(Me, Networking, Settings_)

                'Start Settings Sync Thread
                Synchronization.updateThread_Settings()

                'Start Files Sync Thread
                Synchronization.updateThread_files()


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
        Dim g_username As String = WebBrowser1.Document.GetElementById("email").GetAttribute("value")
        Dim g_password As String = WebBrowser1.Document.GetElementById("password").GetAttribute("value")

        'Encrypt
        Dim e_g_username = Encryption_.DPAPI_encrypt(g_username)
        Dim e_g_password = Encryption_.DPAPI_encrypt(g_password)

        'Store
        Settings_.setValue("username", e_g_username)
        Settings_.setValue("password", e_g_password)

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
        If (e.CloseReason = CloseReason.WindowsShutDown) Then
            killproc()
        Else
            Me.Hide()
            Me.ShowInTaskbar = False
            e.Cancel = True
        End If
    End Sub

    Public Sub dropProcessPriority()
        'Throttle the process to help make resource conservative.
        Try
            parentProcess.PriorityClass = System.Diagnostics.ProcessPriorityClass.BelowNormal 'step
        Catch ex As Exception
            'This might fail if we're administrator, but we tried.
        End Try

        'Drop affinity.
        Try
            parentProcess.ProcessorAffinity = 1
        Catch ex As Exception
            'This might fail if we're administrator, but we tried.
        End Try
    End Sub

    Private Sub Pitter_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'Throttle process
        dropProcessPriority()

        'check if jsonTool Exists
        If My.Computer.FileSystem.FileExists(Environment.CurrentDirectory + "/Newtonsoft.Json.dll") = False Then
            Dim client As New Net.WebClient
            client.DownloadFile("https://raw.githubusercontent.com/Elycin/Pitter-Desktop-Client/master/pitter-2/bin/Debug/Newtonsoft.Json.dll", Environment.CurrentDirectory + "/Newtonsoft.Json.dll")
            Application.Restart()
        End If

        'Use Newest IE
        iefix()
        WebBrowser1.ScriptErrorsSuppressed = True

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
            WebBrowser1.Navigate("https://panel.pitter.us/login")
        Else
            BrowserRelogEvent.Start()
            'hide
            Me.Close()
        End If

        'Check if we updated
        If StringTool.parse_boolean(Settings_.getValue("updated")) = True Then
            Settings_.setValue("updated", "false")
            notification("Pitter has been updated", "You are now running build " + hashengine.hash_generator("sha1", Settings_.working_directory + "pitter.exe"), 5000, ToolTipIcon.Info, False)
        End If

        'Check if we should run as admin.
        If StringTool.parse_boolean(Settings_.getValue("run as administrator")) Then
            If isElevated Then
                'do nothing but remove context element
                RunAsAdministratorToolStripMenuItem.Enabled = False
                RunAsAdministratorToolStripMenuItem.Text = "Running as Admin"
            Else
                'We're not running as admin

                Try
                    Dim procStartInfo As New ProcessStartInfo
                    Dim procExecuting As New Process

                    With procStartInfo
                        .UseShellExecute = True
                        .FileName = Settings_.working_directory + "pitter.exe"
                        .WindowStyle = ProcessWindowStyle.Normal
                        .Verb = "runas" 'add this to prompt for elevation
                    End With
                    procExecuting = Process.Start(procStartInfo)
                    killproc()
                Catch ex As Exception
                    Settings_.setValue("run as administrator", "false")
                    killproc()
                End Try

            End If
        End If

        auth_init = True

    End Sub
    Private Sub DesktopEventListener_Tick(sender As Object, e As EventArgs) Handles DesktopEventListener.Tick

        'Todo: HEAVILY MODIFY

        If listeningForInput = True And isCurrentlyUploading = False And listeningEnabled = True Then
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

                                If GetAsyncKeyState(Character.keyValidator(Integer.Parse(Settings_.getValue("key 1")), True)) <> 0 Then
                                    Capture_.uploadFile()
                                End If
                                If GetAsyncKeyState(Character.keyValidator(Integer.Parse(Settings_.getValue("key 2")), True)) <> 0 Then
                                    Capture_.CaptureWindow(Nothing)
                                End If
                                If GetAsyncKeyState(Character.keyValidator(Integer.Parse(Settings_.getValue("key 3")), True)) <> 0 And StringTool.parse_boolean(Settings_.getValue("printscreen key means fullscreen")) = False Then
                                    'fullscreen
                                    Capture_.captureFullScreen()
                                End If
                                If GetAsyncKeyState(Character.keyValidator(Integer.Parse(Settings_.getValue("key 4")), True)) <> 0 And StringTool.parse_boolean(Settings_.getValue("printscreen key means selection")) = False Then
                                    'Selector
                                    If StringTool.parse_boolean(Settings_.getValue("use old selector")) = False Then
                                        newSelector()
                                    Else
                                        newLegacySelector()
                                    End If
                                End If

                                If GetAsyncKeyState(Character.keyValidator(Integer.Parse(Settings_.getValue("key 5")), True)) <> 0 Then
                                    Capture_.captureClipboard()
                                End If
                            End If
                        Else
                            'We do not need CTRL and SHIFT
                            If GetAsyncKeyState(Character.keyValidator(Integer.Parse(Settings_.getValue("key 1")), False)) <> 0 Then
                                Capture_.uploadFile()
                            End If
                            If GetAsyncKeyState(Character.keyValidator(Integer.Parse(Settings_.getValue("key 2")), False)) <> 0 Then
                                Capture_.CaptureWindow(Nothing)
                            End If
                            If GetAsyncKeyState(Character.keyValidator(Integer.Parse(Settings_.getValue("key 3")), False)) <> 0 And StringTool.parse_boolean(Settings_.getValue("printscreen key means fullscreen")) = False Then
                                'fullscreen
                                Capture_.captureFullScreen()
                            End If
                            If GetAsyncKeyState(Character.keyValidator(Integer.Parse(Settings_.getValue("key 4")), False)) <> 0 And StringTool.parse_boolean(Settings_.getValue("printscreen key means selection")) = False Then
                                'Selector
                                If StringTool.parse_boolean(Settings_.getValue("use old selector")) = False Then
                                    newSelector()
                                Else
                                    newLegacySelector()
                                End If
                            End If

                            If GetAsyncKeyState(Character.keyValidator(Integer.Parse(Settings_.getValue("key 5")), False)) <> 0 Then
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
                                    newSelector()
                                Else
                                    newLegacySelector()
                                End If
                            End If
                        End If
                    Catch ex As Exception
                        Synchronization.sync_settings() 'We might not have settings yet, download them.
                    End Try

                    '1 END
                End If
            Next
        End If
    End Sub
    Private Sub Passive_Tick(sender As Object, e As EventArgs) Handles Passive.Tick
        'This timer should only be used for passive variables or any other form of background processes
        settings_locked = Settings_.locked

        'Pause Listener Button
        If listeningEnabled Then
            If PauseInputListenerToolStripMenuItem.Text <> pil_modes(0) Then
                PauseInputListenerToolStripMenuItem.Text = pil_modes(0)
            End If
        Else
            If PauseInputListenerToolStripMenuItem.Text <> pil_modes(1) Then
                PauseInputListenerToolStripMenuItem.Text = pil_modes(1)
            End If
        End If
    End Sub

    Private Sub Cleaner_Tick(sender As Object, e As EventArgs) Handles Cleaner.Tick
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        killproc()
    End Sub
    Public Sub restore()
        Me.WindowState = FormWindowState.Normal
        Me.ShowInTaskbar = True
        Me.Show()
        AppActivate(System.Diagnostics.Process.GetCurrentProcess.Id)
    End Sub
    Private Sub TaskbarIcon_MouseClick(sender As Object, e As MouseEventArgs) Handles TaskbarIcon.MouseClick
        If StringTool.parse_boolean(Settings_.getValue("tray click twice to open")) = False And e.Button = MouseButtons.Left Then
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
            newSelector()
        Else
            newLegacySelector()
        End If
    End Sub

    Private Sub ForceUpdateToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ForceUpdateToolStripMenuItem.Click
        updater.update(True)
    End Sub

    Private Sub SynchronizeSettingsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SynchronizeSettingsToolStripMenuItem.Click
        Synchronization.sync_settings()
    End Sub
    Public Sub check_for_other_pitters()
        Dim current_pid = Process.GetCurrentProcess.Id

        For Each p As Process In Process.GetProcesses
            If p.ProcessName = "pitter" And p.Id <> current_pid Then
                p.Kill()
            End If
        Next
    End Sub

    Private Sub PauseInputListenerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PauseInputListenerToolStripMenuItem.Click
        If listeningEnabled = False Then
            listeningEnabled = True
            GoTo end1
        ElseIf listeningEnabled = True Then
            listeningEnabled = False
            GoTo end1
        End If
end1:
    End Sub

    Private Sub RunAsAdministratorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RunAsAdministratorToolStripMenuItem.Click
        Dim re = MsgBox("Certain processes on your system may be unable to be captured as a screenshot, due to the process running as the Administrator account on your system." + vbNewLine + "This feature will allow pitter to elevate itself and run as Administrator to overcome this issue, however is purely experimental." + vbNewLine + vbNewLine + "At any time if you wish to undo this change, you will need to manually change the configuration located at %appdata%\pitter\pitter.config" + vbNewLine + vbNewLine + "Do you wish to continue?", MsgBoxStyle.YesNo)

        If re = MsgBoxResult.Yes Then
            Settings_.setValue("run as administrator", "true")
            MsgBox("Please restart Pitter.")
            killproc()
        End If
    End Sub

    Private Const BrowserKeyPath As String = "\SOFTWARE\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_BROWSER_EMULATION"
    Public Sub iefix(Optional ByVal IgnoreIDocDirective As Boolean = False)
        Dim basekey As String = Microsoft.Win32.Registry.CurrentUser.ToString
        Dim value As Int32
        Dim thisAppsName As String = My.Application.Info.AssemblyName & ".exe"
        ' Value reference: http://msdn.microsoft.com/en-us/library/ee330730%28v=VS.85%29.aspx
        ' IDOC Reference:  http://msdn.microsoft.com/en-us/library/ms535242%28v=vs.85%29.aspx
        Select Case (New WebBrowser).Version.Major
            Case 8
                If IgnoreIDocDirective Then
                    value = 8888
                Else
                    value = 8000
                End If
            Case 9
                If IgnoreIDocDirective Then
                    value = 9999
                Else
                    value = 9000
                End If
            Case 10
                If IgnoreIDocDirective Then
                    value = 10001
                Else
                    value = 10000
                End If

            Case 11
                If IgnoreIDocDirective Then
                    value = 11001
                Else
                    value = 11000
                End If
            Case Else
                Exit Sub
        End Select
        Try
            If My.Computer.Registry.GetValue(Microsoft.Win32.Registry.CurrentUser.ToString & BrowserKeyPath, Process.GetCurrentProcess.ProcessName & ".exe", Nothing).ToString <> ((New WebBrowser).Version.Major.ToString + "000") Then
                Microsoft.Win32.Registry.SetValue(Microsoft.Win32.Registry.CurrentUser.ToString & BrowserKeyPath, Process.GetCurrentProcess.ProcessName & ".exe", value, Microsoft.Win32.RegistryValueKind.DWord)
                updater.update(False)
            End If
        Catch ex As Exception
            Microsoft.Win32.Registry.SetValue(Microsoft.Win32.Registry.CurrentUser.ToString & BrowserKeyPath, Process.GetCurrentProcess.ProcessName & ".exe", value, Microsoft.Win32.RegistryValueKind.DWord)
            updater.update(False)
        End Try

    End Sub

    Private Sub WebBrowser1_DocumentCompleted(ByVal sender As System.Object, ByVal e As Windows.Forms.WebBrowserDocumentCompletedEventArgs) Handles WebBrowser1.DocumentCompleted
        AddHandler WebBrowser1.Document.Click, AddressOf getClickedElement
    End Sub

    Private Sub getClickedElement(ByVal sender As Object, ByVal e As HtmlElementEventArgs)
        With WebBrowser1.Document.GetElementFromPoint(e.ClientMousePosition)
            Try
                Dim selectedHtmlElement_ID As String = .GetAttribute("id").ToLower
                Dim selectedHtmlElement_NAME As String = .GetAttribute("name").ToLower

                'Attempt to see if we're clicking a link
                'Grab the hfef
                Try
                    clicked_link = .GetAttribute("href")
                Catch ex As Exception

                End Try

                'Element action
                Try
                    Select Case selectedHtmlElement_ID
                        Case "submit-b"
                            If WebBrowser1.Url = New Uri("https://panel.pitter.us/login") Or WebBrowser1.Url = New Uri("https://panel.pitter.us/login?new=true") Or WebBrowser1.Url = New Uri("https://panel.ieatass.club/login") Or WebBrowser1.Url = New Uri("https://panel.ieatass.club/login?new=true") Then
                                grab_login_information()
                                login_routine()
                            End If
                    End Select
                Catch ex As Exception
                    Try
                        grab_login_information()
                        login_routine()
                    Catch ex2 As Exception

                    End Try
                End Try
            Catch ex As Exception
                notification("Please restart pitter", "You need to log back in.", 5000, ToolTipIcon.Info, False)
            End Try
        End With
    End Sub

    Private Sub WebBrowser1_NewWindow(sender As Object, e As CancelEventArgs) Handles WebBrowser1.NewWindow
        e.Cancel = True

        'Check if we're clicking a thunbnail.
        If clicked_link.Contains("thumb/") Then
            clicked_link = clicked_link.Replace("thumb/", "")
        End If
        Process.Start(clicked_link)
    End Sub

    Private Sub newSelector()
        Dim tmpSelector As New Modern(Me, Networking)
        tmpSelector.Show()
    End Sub
    Private Sub newLegacySelector()
        Dim tmpSelector As New Legacy(Me, Networking)
        tmpSelector.Show()
    End Sub

    Private Sub BrowserRelogEvent_Tick(sender As Object, e As EventArgs) Handles BrowserRelogEvent.Tick
        'This function will automatically login the user every 15 minutes to prevent the login screen from showing.
        Dim cur_url As String = WebBrowser1.Url.ToString

        'Restore the last position.
        If login_routine() = True Then
            WebBrowser1.Navigate(cur_url)
        End If
    End Sub

    Private Sub OpenStorageLocationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenStorageLocationToolStripMenuItem.Click
        System.Diagnostics.Process.Start(save_location)
    End Sub
End Class