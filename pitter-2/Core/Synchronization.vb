Imports Newtonsoft.Json.Linq
Imports System.Threading

Public Class Synchronization
    Dim Networking_super As Networking
    Dim Settings_super As Settings
    Dim client As New Net.WebClient
    Dim stringtool As New StringTool
    Dim parent As WebApp

    Public Sub New(ByVal p_parent As WebApp, ByVal networking As Networking, ByVal settings As Settings)
        parent = p_parent
        Networking_super = networking
        Settings_super = settings
    End Sub

    Public Sub sync_settings()
        Dim json_string As String
        Try
            json_string = Networking_super.get_settings()
            Dim l_json As JObject = JObject.Parse(json_string)
            Dim updated As Boolean = False

            Dim keys As IList(Of String) = l_json.Properties().[Select](Function(p) p.Name).ToList()

            For Each key As String In keys

                If Settings_super.getValue(key.Replace("_", " ")) <> l_json.GetValue(key).ToString Then
                    Settings_super.setValue(key.Replace("_", " "), l_json.GetValue(key).ToString)
                    updated = True

                End If
            Next
            If stringtool.parse_boolean(Settings_super.getValue("synchronization notification")) Then
                If updated Then parent.notification("Settings Synchronized", "Settings from the cloud have been saved to this machine.", 5000, ToolTipIcon.Info, False)
            End If
        Catch ex As Exception
            'Well, TRY AGAIN!

        End Try

    End Sub

    Public Sub sync_files()
        Try
            Dim files_json As String = Networking_super.get_files
            Dim parsed_parent As JArray = JArray.Parse(files_json)
            For Each row As JObject In parsed_parent

                If My.Computer.FileSystem.FileExists(parent.save_location + row.GetValue("filename").ToString) = False Then
                    client.DownloadFile("https://i.pitter.us/" + row.GetValue("filename").ToString, parent.save_location + row.GetValue("filename").ToString)
                End If


            Next
        Catch ex As Exception

        End Try
    End Sub

    Private Sub thr_loop_settings()
        While True
            Threading.Thread.Sleep(30000)
            sync_settings()
        End While
    End Sub

    Public Sub updateThread_Settings()
        Dim thr = New Thread(AddressOf thr_loop_settings)
        thr.IsBackground = True
        thr.Start()
    End Sub

    Private Sub thr_loop_files()
        While True
            Threading.Thread.Sleep(10 * (60 * 1000))
            sync_files()
        End While
    End Sub

    Public Sub updateThread_files()
        Dim thr = New Thread(AddressOf thr_loop_files)
        thr.IsBackground = True
        thr.Start()
    End Sub
End Class