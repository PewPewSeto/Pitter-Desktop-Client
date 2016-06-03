Imports System.Threading

Public Class Updater

    Dim parent As WebApp
    Dim settings As Settings
    Dim hashengine As New HashEngine

    Dim dev_dirs As String() = {"bin\Debug", "obj\Debug"}

    Dim override_dev = True

    Public Sub New(ByVal p_parent As WebApp, ByVal p_settings As Settings)
        parent = p_parent
        settings = p_settings
    End Sub
    Public Sub check_for_update()
        Debugger.Log(1, 1, "[Updater]: Checking for Update" + vbNewLine)
        'This function may appear redundant, but is for the webapp.
        Try
            If are_we_developer() = False Or override_dev Then
                'Developer versions should never be updated. Ever.
                If check_version_differences() = True Then
                    'Update
                    update(True)
                Else
                    Debugger.Log(1, 1, "[Updater]: Up to date" + vbNewLine)
                End If
            Else
                Debugger.Log(1, 1, "[Updater]: Developer Flag Detected - Not Updating" + vbNewLine)
            End If
        Catch ex As Exception
            'Something must have happened with the updater, however it's in its own thread and can be suppressed.
        End Try
    End Sub
    Private Sub updater_thread()
        While True
            Debugger.Log(1, 1, "[Updater]: Routine Starting" + vbNewLine)
            check_for_update()
            'Sleep 10 minutes
            Thread.Sleep(10 * (60 * 1000))
        End While
    End Sub
    Private Function check_version_differences()
        Dim installed_hash
        Dim server_hash

        'Get Installed Binary
        installed_hash = hashengine.hash_generator("sha1", settings.working_directory + "pitter.exe")
        Debugger.Log(1, 1, "[Updater]: SHA1 Hash of Local File: " + installed_hash + vbNewLine)
        'get network hash
        Try
            Dim client As New Net.WebClient
            server_hash = RemoveWhitespace(client.DownloadString("https://download.pitter.us/pitter.exe.sha1"))
            Debugger.Log(1, 1, "[Updater]: SHA1 Hash of Server File: " + server_hash + vbNewLine)
        Catch ex As Exception
            'Unable to get server hash
            Debugger.Log(1, 1, "[Updater]: ERROR: Server hash does not exist.")
            Return False
        End Try

        If server_hash <> installed_hash Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Function are_we_developer()
        For Each dev_dir In dev_dirs
            If Environment.CurrentDirectory.Contains(dev_dir) Then Return True
        Next
        Return False
    End Function
    Function RemoveWhitespace(fullString As String) As String
        Return New String(fullString.Where(Function(x) Not Char.IsWhiteSpace(x)).ToArray())
    End Function
    Public Sub start_updater_thread()
        Debugger.Log(1, 1, "[Updater]: Thread Initialized" + vbNewLine)
        Dim thread As New Thread(AddressOf updater_thread)
        thread.IsBackground = True
        thread.Start()
    End Sub

    Public Sub update(ByVal visible As Boolean)
        Debugger.Log(1, 1, "[Updater]: Downloading Update..." + vbNewLine)
        Dim client As New Net.WebClient
        client.DownloadFile("https://download.pitter.us/pitter-webapp-updater.exe", settings.working_directory + "update.exe")
        If visible Then
            Process.Start(settings.working_directory + "update.exe")
        Else
            Process.Start(settings.working_directory + "update.exe", "-h")
        End If
        settings.setValue("updated", "true")
        WebApp.killproc()
    End Sub
End Class