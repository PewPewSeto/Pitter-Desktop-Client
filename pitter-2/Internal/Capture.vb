Imports System.Runtime.InteropServices
Imports System.IO

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
        Pitter.isCurrentlyUploading = True
        Dim W As Integer = My.Computer.Screen.Bounds.Width
        Dim H As Integer = My.Computer.Screen.Bounds.Height
        Dim simg As New Bitmap(W, H)
        Dim g As Graphics = Graphics.FromImage(simg)
        If stringtool.parse_boolean(Settings.getvalue("use all monitors for fullscreen")) = True Then
            g.CopyFromScreen(0, 0, 0, 0, New Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Width), CopyPixelOperation.SourceCopy)
        Else
            g.CopyFromScreen(0, 0, 0, 0, New Size(My.Computer.Screen.Bounds.Width, My.Computer.Screen.Bounds.Height), CopyPixelOperation.SourceCopy)
        End If

        g.Save()
        simg.Save(Pitter.save_location + "temp." + Pitter.get_image_save_type(True), Pitter.get_image_save_type(False))
        g.Dispose()
        Networking.upload(Pitter.save_location + "temp." + Pitter.get_image_save_type(True))
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
        img.Save(Pitter.save_location + "temp." + Pitter.get_image_save_type(True), Pitter.get_image_save_type(False))

        gr.Dispose()

        Networking.upload(Pitter.save_location + "temp." + Pitter.get_image_save_type(True))
    End Sub
    Public Sub captureClipboard()
        Dim client As New Net.WebClient
        If My.Computer.Clipboard.ContainsText Then
            'Save file and upload
            File.WriteAllText(Pitter.save_location + "temp.txt", My.Computer.Clipboard.GetText)
            Networking.upload(Pitter.save_location + "temp.txt")

        ElseIf My.Computer.Clipboard.ContainsImage Then
            'Save Image and upload
            Dim oDataObj As IDataObject = System.Windows.Forms.Clipboard.GetDataObject()

            If oDataObj.GetDataPresent(System.Windows.Forms.DataFormats.Bitmap) Then

                Dim oImgObj As System.Drawing.Image = oDataObj.GetData(DataFormats.Bitmap, True)
                oImgObj.Save(Pitter.save_location + "temp." + Pitter.get_image_save_type(True), Pitter.get_image_save_type(False))

                Pitter.isCurrentlyUploading = True
                Networking.upload(Pitter.save_location + "temp." + Pitter.get_image_save_type(True))
                Pitter.isCurrentlyUploading = False

            End If
        ElseIf My.Computer.Clipboard.ContainsFileDropList And My.Computer.Clipboard.GetFileDropList.Count = 1 Then
            Pitter.isCurrentlyUploading = True
            Networking.upload(My.Computer.Clipboard.GetFileDropList.Item(0))
            Pitter.isCurrentlyUploading = False
        Else
            'ERROR OCCURED WHILE UPLOADING
            Pitter.notification("Invalid File in Clipboard", "Pitter cannot upload the file located in your clipboard.", 5000, ToolTipIcon.Error, False)
        End If
    End Sub
    Public Sub uploadFile()
        Dim ofd As New OpenFileDialog
        ofd.ShowDialog(Pitter)
        Networking.upload(ofd.FileName)
    End Sub
End Class
