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
        Dim l_json As JObject = JObject.Parse(json_string)

        Settings_super.setValue("key 1", l_json.GetValue("key_1"))
        Settings_super.setValue("key 2", l_json.GetValue("key_2"))
        Settings_super.setValue("key 3", l_json.GetValue("key_3"))
        Settings_super.setValue("key 4", l_json.GetValue("key_4"))
        Settings_super.setValue("key 5", l_json.GetValue("key_5"))
        Settings_super.setValue("use old selector", l_json.GetValue("use_old_selector"))
        Settings_super.setValue("printscreen key means selection", l_json.GetValue("printscreen_selector"))
        Settings_super.setValue("printscreen key means fullscreen", l_json.GetValue("printscreen_fullscreen"))
        Settings_super.setValue("use control and shift", l_json.GetValue("ucas"))
        Settings_super.setValue("image format", l_json.GetValue("image_format"))

        Pitter.notification("Settings Synchronized", "Settings from thee cloud have been saved to this machine.", 5000, ToolTipIcon.Info, True)
    End Sub
End Class
