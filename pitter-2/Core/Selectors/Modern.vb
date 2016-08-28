Public Class Modern
    Dim Encryption_ As New Encryption
    Dim Settings_ As New Settings
    Dim Networking As Networking
    Dim click As Integer = 0
    Dim parent As WebApp
    Dim x1 As Integer
    Dim x2 As Integer
    Dim y1 As Integer
    Dim y2 As Integer
    Dim p1 As New Panel
    Dim move As Boolean = Nothing

    Sub New(ByVal parent_form As WebApp, parent_networking As Networking)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        parent = parent_form
        Networking = parent_networking
    End Sub


    Public Sub upload()
        parent.Passive.Start()
        Try
            parent.BrowserRelogEvent.Start()
        Catch ex As Exception
            'We're not using the webapp then
        End Try

        Try
            Dim tg As Graphics = Me.CreateGraphics

            Dim scale_x = tg.DpiX / 96.0F
            Dim scale_y = tg.DpiY / 96.0F

            Dim or_x1 = x1
            Dim or_x2 = x2

            Dim or_y1 = y1
            Dim or_y2 = y2

            x1 = Math.Round(x1 * scale_x)
            x2 = Math.Round(x2 * scale_x)

            y1 = Math.Round(y1 * scale_y)
            y2 = Math.Round(y2 * scale_y)

            Dim client As New Net.WebClient
            Dim simg As New Bitmap(or_x2 - or_x1, or_y2 - or_y1)
            Dim g As Graphics = Graphics.FromImage(simg)

            g.CopyFromScreen(x1, y1, 0, 0, New Size(x2 - x1, y2 - y1), CopyPixelOperation.SourceCopy)

            simg.Save(parent.save_location + "temp." + parent.get_image_save_type(True), parent.get_image_save_type(False))

            'upload
            Networking.upload(parent.save_location + "temp." + parent.get_image_save_type(True), True)
            parent.listeningForInput = True
            parent.isCurrentlyUploading = False
            Me.Close()

        Catch ex As Exception
            parent.notification("Invalid Selection Region", "You attempted to select an invalid region of the screen, please work diagnally down from the top left to the bottom right.", 5000, ToolTipIcon.Info, False)
            parent.listeningForInput = True
            parent.isCurrentlyUploading = False
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
                parent.listeningForInput = True
            End If
        End If
    End Sub
    Public Sub generalclickevent()
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
    Private Sub selector_MouseDown(sender As Object, e As MouseEventArgs) Handles MyBase.MouseDown
        generalclickevent()
        Label1.Visible = False
        Label2.Visible = False
    End Sub
    Private Sub selector_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then
            parent.listeningForInput = True
            parent.isCurrentlyUploading = False
            Me.Close()
        End If
    End Sub

    Private Sub selector_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        parent.listeningForInput = False
        Me.Show()
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Location = New Point(0, 0)
        Me.Size = SystemInformation.VirtualScreen.Size
        Me.TopMost = True
        Me.BringToFront()
        Me.Opacity = 0.5

        parent.Passive.Stop()
        Try
            parent.BrowserRelogEvent.Stop()
        Catch ex As Exception
            'We're not using the webapp then
        End Try


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

        If click = 0 Then
            Label2.Text = "Mouse Position: (" + Cursor.Position.X.ToString + ", " + Cursor.Position.Y.ToString + ")"
        Else
            Label2.Text = "Mouse Position: (" + Cursor.Position.X.ToString + ", " + Cursor.Position.Y.ToString + ")" + vbNewLine + "Region Size: (" + ((x1 - Cursor.Position.X) * -1).ToString + ", " + ((y1 - Cursor.Position.Y) * -1).ToString + ")"
        End If

    End Sub



    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If Me.Size <> SystemInformation.VirtualScreen.Size Then
            Me.Size = SystemInformation.VirtualScreen.Size
            Me.WindowState = FormWindowState.Normal
            Me.Refresh()
        End If
        If Me.Location <> New Point(0, 0) Then
            Me.Location = New Point(0, 0)
            Me.Refresh()
        End If

    End Sub


End Class