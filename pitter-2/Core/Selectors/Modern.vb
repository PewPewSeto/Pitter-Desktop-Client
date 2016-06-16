Public Class Modern
    Dim Encryption_ As New Encryption
    Dim Settings_ As New Settings
    Dim Networking As New Networking(Encryption_.DPAPI_decrpyt(Settings_.getValue("username")), Encryption_.DPAPI_decrpyt(Settings_.getValue("password")), Settings_)

    Dim click As Integer = 0

    Dim x1 As Integer
    Dim x2 As Integer
    Dim y1 As Integer
    Dim y2 As Integer
    Dim p1 As New Panel
    Dim move As Boolean = Nothing
    Public Sub upload()
        WebApp.Passive.Start()
        Try
            WebApp.BrowserEventListener.Start()
        Catch ex As Exception
            'We're not using the webapp then
        End Try

        Try
            Dim client As New Net.WebClient
            Dim simg As New Bitmap(x2 - x1, y2 - y1)
            Dim g As Graphics = Graphics.FromImage(simg)

            g.CopyFromScreen(x1, y1, 0, 0, New Size(x2 - x1, y2 - y1), CopyPixelOperation.SourceCopy)

            simg.Save(WebApp.save_location + "temp." + WebApp.get_image_save_type(True), WebApp.get_image_save_type(False))

            'upload
            Networking.upload(WebApp.save_location + "temp." + WebApp.get_image_save_type(True), True)
            WebApp.isCurrentlyUploading = False
            Me.Close()

        Catch ex As Exception
            WebApp.notification("Invalid Selection Region", "You attempted to select an invalid region of the screen, please work diagnally down from the top left to the bottom right.", 5000, ToolTipIcon.Info, False)
            WebApp.isCurrentlyUploading = False
            Me.Close()
        End Try

    End Sub
    Private Sub selector_Mouseup(sender As Object, e As MouseEventArgs) Handles MyBase.MouseUp
        If Cursor.Position.X > x1 + 5 Then
            If Cursor.Position.Y > y1 + 5 Then
                x2 = Cursor.Position.X 'x2 = e.X
                y2 = Cursor.Position.Y 'y2 = e.Y
                move = False
                Me.Hide()
                Me.Visible = False
                upload()
            End If
        End If
    End Sub
    Private Sub selector_MouseDown(sender As Object, e As MouseEventArgs) Handles MyBase.MouseDown
        If click = 0 Then
            x1 = Cursor.Position.X 'x1 = e.X
            y1 = Cursor.Position.Y 'y1 = e.Y

            'add panel
            p1.BackColor = Color.FromArgb(255, 255, 255, 255)
            p1.Location = New Point(x1, y1)
            Me.Controls.Add(p1)
            move = True
        ElseIf click = 1 Then
            x2 = Cursor.Position.X 'x2 = e.X
            y2 = Cursor.Position.Y 'y2 = e.Y
            move = False
            Me.Hide()
            Me.Visible = False

            upload()
        End If
        click = Val(click) + 1
    End Sub
    Private Sub selector_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then
            WebApp.listeningForInput = True
            WebApp.isCurrentlyUploading = False
            Me.Close()
        End If
    End Sub

    Private Sub selector_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Location = New Point(0, 0)
        Me.Size = SystemInformation.VirtualScreen.Size
        Me.TopMost = True
        Me.BringToFront()
        Me.Opacity = 0.5

        WebApp.Passive.Stop()
        Try
            WebApp.BrowserEventListener.Stop()
        Catch ex As Exception
            'We're not using the webapp then
        End Try

        Me.Show()
    End Sub
    Private Sub selector_resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        Me.Text = "Selector Window (" + Me.Size.Width.ToString + "," + Me.Size.Height.ToString + ")"
    End Sub

    Private Sub MovementTracker_Tick(sender As Object, e As EventArgs) Handles MovementTracker.Tick
        Me.Location = New Point(0, 0)
        Me.Size = SystemInformation.VirtualScreen.Size
        If move Then
            p1.Size = New Size(Cursor.Position.X - x1, Cursor.Position.Y - y1)
        End If
    End Sub
End Class