Imports System.Environment
Imports System.Text.RegularExpressions
Imports Newtonsoft.Json.Linq
Imports System.IO
Imports Newtonsoft.Json

Public Class Settings
    Public working_directory = GetFolderPath(SpecialFolder.ApplicationData) + "\Pitter\"
    Public settings_location = working_directory + "pitter.config"

    Public locked As Boolean = False
    Private Sub lock(ByVal lockee As String)
recheck:
        'recheck will be used to check if the settings file is locked
        If locked Then
            Debugger.Log(1, 1, lockee + " is currently locking the settings..." + vbNewLine)
            GoTo recheck
        Else
            locked = True
        End If
    End Sub
    Public Function create()
        Dim created As Boolean = False
        lock("create")
        If My.Computer.FileSystem.FileExists(settings_location) = False Then
            File.WriteAllText(settings_location, "{}")
            created = True
        End If
        locked = False
        If created Then
            fill()
        End If

    End Function
    Public Function version_uodate()
        lock("version update") 'Hold the thread until the configuration file is available
        locked = False
    End Function
    Public Function setValue(ByVal key As String, ByVal value As String)
        create() 'Check to see if the settings file exists
        lock("set value key " + key) 'Hold the thread until the configuration file is available

        Dim json_object As JObject = JObject.Parse(File.ReadAllText(settings_location))
        Try
            'Junk remove function
            json_object.Remove(key)
        Catch ex As Exception
            'Function does not exist, graceful suppress.
        End Try
        json_object.Add(key, value)

        Dim json_out = JsonConvert.SerializeObject(json_object, Formatting.Indented)
        File.WriteAllText(settings_location, json_out)

        locked = False
        Return True

    End Function
    Public Function getValue(ByVal key)
        create() 'Check to see if the settings file exists
        lock("get value key " + key) 'Hold the thread until the configuration file is available
        Try
            Dim json_object As JObject = JObject.Parse(File.ReadAllText(settings_location))
            Try
                Dim response As String = json_object.GetValue(key).ToString
                locked = False
                Return response
            Catch ex As Exception
                locked = False
                Return ""
            End Try
        Catch ex As Exception
            locked = False
            My.Computer.FileSystem.DeleteFile(settings_location)
            create()
        End Try

    End Function
    Public Sub fill()
        'This sub should only be used for creation of config
        setValue("username", "")
        setValue("password", "")
        setValue("key 1", "49")
        setValue("key 2", "50")
        setValue("key 3", "51")
        setValue("key 4", "52")
        setValue("key 5", "53")
        setValue("use old selector", "false")
        setValue("printscreen key means selection", "false")
        setValue("printscreen key means fullscreen", "false")
        setValue("use control and shift", "true")
        setValue("image format", "jpg")
        setValue("config version", "0")
        setValue("chime", "true")
        setValue("fullscreen means all monitors", "true")
        setValue("synchronize files between clients", "true")
        setValue("custom server", "https://i.pitter.us/")
        setValue("use custom server", "")
        locked = False
    End Sub
End Class