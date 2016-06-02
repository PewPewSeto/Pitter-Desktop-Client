Imports System.Runtime.InteropServices
Imports System.IO
Imports System.IO.Compression
Public Class Capture
    Dim Encryption As Encryption
    Dim Settings As Settings
    Dim Networking As Networking
    Dim stringtool As New StringTool

    Sub New(ByVal enc_c As Encryption, ByVal set_c As Settings, ByVal net_c As Networking, ByVal str_c As StringTool)
        Encryption = enc_c
        Settings = set_c
        Networking = net_c
        stringtool = str_c
    End Sub

    Public Structure RECT
        Public left As Integer
        Public top As Integer
        Public right As Integer
        Public bottom As Integer
    End Structure 'RECT

    Declare Function GetDesktopWindow Lib "user32.dll" () As IntPtr
    Declare Function GetWindowRect Lib "user32.dll" (ByVal hwnd As IntPtr, ByRef lpRect As RECT) As Int32
    Declare Function GetForegroundWindow Lib "user32.dll" Alias "GetForegroundWindow" () As IntPtr

    <DllImport("user32.dll", EntryPoint:="GetActiveWindow", SetLastError:=True,
    CharSet:=CharSet.Unicode, ExactSpelling:=True,
    CallingConvention:=CallingConvention.StdCall)>
    Public Shared Function GetActiveWindowHandle() As System.IntPtr
    End Function
    Public Sub captureFullScreen()
        WebApp.isCurrentlyUploading = True
        Dim W As Integer = My.Computer.Screen.Bounds.Width
        Dim H As Integer = My.Computer.Screen.Bounds.Height
        Dim simg As Bitmap
        Dim g As Graphics
        If stringtool.parse_boolean(Settings.getValue("fullscreen means all monitors")) = True Then
            simg = New Bitmap( _
                        Screen.AllScreens.Sum(Function(s As Screen) s.Bounds.Width),
                        Screen.AllScreens.Max(Function(s As Screen) s.Bounds.Height))
            g = Graphics.FromImage(simg)
            g.CopyFromScreen(SystemInformation.VirtualScreen.X,
                       SystemInformation.VirtualScreen.Y,
                       0, 0, SystemInformation.VirtualScreen.Size)
        Else
            simg = New Bitmap(W, H)
            g = Graphics.FromImage(simg)
            g.CopyFromScreen(0, 0, 0, 0, New Size(My.Computer.Screen.Bounds.Width, My.Computer.Screen.Bounds.Height), CopyPixelOperation.SourceCopy)
        End If

        g.Save()
        simg.Save(WebApp.save_location + "temp." + WebApp.get_image_save_type(True), WebApp.get_image_save_type(False))
        g.Dispose()
        Networking.upload(WebApp.save_location + "temp." + WebApp.get_image_save_type(True), True)
    End Sub

    Public Sub CaptureWindow(ByVal r As Rectangle)
        ' get the size

        Dim windowRect As New RECT

        Dim width As Integer
        Dim height As Integer

        If r = Nothing Then
            GetWindowRect(GetForegroundWindow, windowRect)
            windowRect.left -= 5
            windowRect.top -= 5
            width = windowRect.right - windowRect.left + 5
            height = windowRect.bottom - windowRect.top + 5
        Else
            windowRect.left = r.Left + 1
            windowRect.top = r.Top + 1
            width = r.Width - 1
            height = r.Height - 1
            windowRect.right = windowRect.left + width
            windowRect.bottom = windowRect.top + height
        End If

        Dim img As Bitmap = New Bitmap(width, height)
        Dim gr As Graphics = Graphics.FromImage(img)
        gr.CopyFromScreen(windowRect.left, windowRect.top, 0, 0, New Size(width, height))
        gr.Save()
        gr.Save()
        img.Save(WebApp.save_location + "temp." + WebApp.get_image_save_type(True), WebApp.get_image_save_type(False))

        gr.Dispose()

        Networking.upload(WebApp.save_location + "temp." + WebApp.get_image_save_type(True), True)
    End Sub
    Public Sub captureClipboard()
        Dim client As New Net.WebClient
        If My.Computer.Clipboard.ContainsText Then
            'Save file and upload
            File.WriteAllText(WebApp.save_location + "temp.txt", My.Computer.Clipboard.GetText)
            Networking.upload(WebApp.save_location + "temp.txt", True)

        ElseIf My.Computer.Clipboard.ContainsImage Then
            'Save Image and upload
            Dim oDataObj As IDataObject = System.Windows.Forms.Clipboard.GetDataObject()

            If oDataObj.GetDataPresent(System.Windows.Forms.DataFormats.Bitmap) Then

                Dim oImgObj As System.Drawing.Image = oDataObj.GetData(DataFormats.Bitmap, True)
                oImgObj.Save(WebApp.save_location + "temp." + WebApp.get_image_save_type(True), WebApp.get_image_save_type(False))

                WebApp.isCurrentlyUploading = True
                Networking.upload(WebApp.save_location + "temp." + WebApp.get_image_save_type(True), True)
                WebApp.isCurrentlyUploading = False

            End If
        ElseIf My.Computer.Clipboard.ContainsFileDropList Then
            If My.Computer.Clipboard.GetFileDropList.Count = 1 Then
                WebApp.isCurrentlyUploading = True
                Networking.upload(My.Computer.Clipboard.GetFileDropList.Item(0), True)
                WebApp.isCurrentlyUploading = False
            ElseIf My.Computer.Clipboard.GetFileDropList.Count > 1 Then
                WebApp.isCurrentlyUploading = True
                WebApp.notification("Files will be compressed", "You have multiple files in your clipboard. They will be compressed into a single .zip", 5000, ToolTipIcon.Info, False)
                If My.Computer.FileSystem.FileExists(WebApp.save_location + "temp.zip") Then My.Computer.FileSystem.DeleteFile(WebApp.save_location + "temp.zip")
                If My.Computer.FileSystem.FileExists(WebApp.save_location + "zipdir") = False Then MkDir(WebApp.save_location + "zipdir")
                For Each File In My.Computer.Clipboard.GetFileDropList
                    Dim fn() As String = File.ToString.Split("\")
                    Dim fnl As Integer = fn.Length - 1
                    My.Computer.FileSystem.CopyFile(File, WebApp.save_location + "zipdir\" + fn(fnl))
                Next
                ZipFile.CreateFromDirectory(WebApp.save_location + "zipdir\", WebApp.save_location + "temp.zip")
                Networking.upload(WebApp.save_location + "temp.zip", True)
                My.Computer.FileSystem.DeleteDirectory(WebApp.save_location + "zipdir\", FileIO.DeleteDirectoryOption.DeleteAllContents)
                WebApp.isCurrentlyUploading = False
            End If

        Else
            'ERROR OCCURED WHILE UPLOADING
            WebApp.notification("Invalid File in Clipboard", "Pitter cannot upload the file located in your clipboard.", 5000, ToolTipIcon.Error, False)
        End If
    End Sub
    Public Sub uploadFile()
        Dim ofd As New OpenFileDialog
        ofd.ShowDialog(WebApp)
        Networking.upload(ofd.FileName, False)
    End Sub
End Class