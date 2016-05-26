Public Class Legacy
    Dim Encryption_ As New Encryption
    Dim Settings_ As New Settings
    Dim Networking As New Networking(Encryption_.DPAPI_decrpyt(Settings_.getValue("username")), Encryption_.DPAPI_decrpyt(Settings_.getValue("password")))
    Dim fd As New FormDecision

    Private Sub selector_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Return Then
            Dim x1 As Integer = Me.Location.X
            Dim y1 As Integer = Me.Location.Y

            Dim wx As Integer = Me.Size.Width
            Dim wy As Integer = Me.Size.Height

            Dim client As New Net.WebClient

            Dim simg As New Bitmap(wx, wy)
            Dim g As Graphics = Graphics.FromImage(simg)

            Me.Visible = False
            Me.Opacity = 0.0

            g.CopyFromScreen(x1, y1, 0, 0, New Size(wx, wy), CopyPixelOperation.SourceCopy)
            simg.Save(fd.choice.save_location + "temp." + fd.choice.get_image_save_type(True), fd.choice.get_image_save_type(False))
            g.Dispose()

            fd.choice.isCurrentlyUploading = True

            Networking.upload(fd.choice.save_location + "temp." + fd.choice.get_image_save_type(True))

            fd.choice.listeningForInput = True
            fd.choice.isCurrentlyUploading = False

            Me.Close()

        ElseIf e.KeyCode = Keys.Escape Then

            fd.choice.listeningForInput = True
            fd.choice.isCurrentlyUploading = False
            Me.Close()

        End If
    End Sub

    Private Sub selector_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        fd.choice.listeningForInput = False

        Me.Size = New Size(470, 300)
        Me.Opacity = 0.6

    End Sub

    Private Sub selector_Resize(sender As Object, e As EventArgs) Handles Me.Resize

    End Sub
End Class