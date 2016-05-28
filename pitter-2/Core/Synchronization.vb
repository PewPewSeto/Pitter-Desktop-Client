Imports Newtonsoft.Json.Linq
Imports System.Threading

Public Class Synchronization
    Dim Networking_super As Networking
    Dim Settings_super As Settings
    Dim client As New Net.WebClient


    Public Sub New(ByVal networking As Networking, ByVal settings As Settings)
        Networking_super = networking
        Settings_super = settings
    End Sub

    Public Sub sync()
        Dim json_string As String = Networking_super.get_settings()
        Dim l_json As JObject = JObject.Parse(json_string)
        Dim updated As Boolean = False


        Dim keys As IList(Of String) = l_json.Properties().[Select](Function(p) p.Name).ToList()

        For Each key As String In keys

            If Settings_super.getValue(key.Replace("_", " ")) <> l_json.GetValue(key).ToString Then
                Settings_super.setValue(key.Replace("_", " "), l_json.GetValue(key).ToString)
                updated = True

            End If
        Next
        If updated Then Settings_super.setValue("notify sync", "true")

    End Sub

    Private Sub thr_loop()
        While True
            Threading.Thread.Sleep(10000)
            sync()
        End While
    End Sub

    Public Sub updateThread()
        Dim thr = New Thread(AddressOf thr_loop)
        thr.IsBackground = True
        thr.Start()
    End Sub
End Class