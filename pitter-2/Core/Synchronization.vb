Imports Newtonsoft.Json.Linq

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
        MsgBox(json_string)
        Dim l_json As JObject = JObject.Parse(json_string)

        Dim keys As IList(Of String) = l_json.Properties().[Select](Function(p) p.Name).ToList()

        For Each key As String In keys
            Settings_super.setValue(key.Replace("_", " "), l_json.GetValue(key))
        Next

        WebApp.notification("Settings Synchronized", "Settings from thee cloud have been saved to this machine.", 5000, ToolTipIcon.Info, True)
    End Sub
End Class