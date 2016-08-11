Public Class Legacy
    Dim Encryption_ As New Encryption
    Dim Settings_ As New Settings
    Dim Networking As Networking
    Dim parent As WebApp

    Sub New(ByVal parent_form As WebApp, ByVal parent_networking As Networking)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        parent = parent_form
        Networking = parent_networking
    End Sub

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
            simg.Save(parent.save_location + "temp." + parent.get_image_save_type(True), parent.get_image_save_type(False))
            g.Dispose()

            parent.isCurrentlyUploading = True

            Networking.upload(parent.save_location + "temp." + parent.get_image_save_type(True), True)

            parent.listeningForInput = True
            parent.isCurrentlyUploading = False

            Me.Close()

        ElseIf e.KeyCode = Keys.Escape Then

            parent.listeningForInput = True
            parent.isCurrentlyUploading = False
            Me.Close()

        End If
    End Sub

    Private Sub selector_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        parent.listeningForInput = False

        Me.Size = New Size(470, 300)
        Me.Opacity = 0.6

    End Sub

    Private Sub selector_Resize(sender As Object, e As EventArgs) Handles Me.Resize

    End Sub
End Class