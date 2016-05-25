<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class WebApp
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(WebApp))
        Me.WebControl1 = New Awesomium.Windows.Forms.WebControl(Me.components)
        Me.BrowserEventListener = New System.Windows.Forms.Timer(Me.components)
        Me.DesktopEventListener = New System.Windows.Forms.Timer(Me.components)
        Me.TaskbarIcon = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.PitterToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Passive = New System.Windows.Forms.Timer(Me.components)
        Me.Cleaner = New System.Windows.Forms.Timer(Me.components)
        Me.UploadToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.PauseInputListenerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ClipboardToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.CurrentWindowToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FullscreenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SelectionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ForceUpdateToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'WebControl1
        '
        Me.WebControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.WebControl1.Location = New System.Drawing.Point(0, 0)
        Me.WebControl1.Size = New System.Drawing.Size(935, 523)
        Me.WebControl1.Source = New System.Uri("http://panel.pitter.us", System.UriKind.Absolute)
        Me.WebControl1.TabIndex = 1
        Me.WebControl1.ViewType = Awesomium.Core.WebViewType.Offscreen
        '
        'BrowserEventListener
        '
        Me.BrowserEventListener.Enabled = True
        Me.BrowserEventListener.Interval = 10
        '
        'DesktopEventListener
        '
        Me.DesktopEventListener.Enabled = True
        Me.DesktopEventListener.Interval = 10
        '
        'TaskbarIcon
        '
        Me.TaskbarIcon.ContextMenuStrip = Me.ContextMenuStrip1
        Me.TaskbarIcon.Icon = CType(resources.GetObject("TaskbarIcon.Icon"), System.Drawing.Icon)
        Me.TaskbarIcon.Visible = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PitterToolStripMenuItem, Me.ToolStripSeparator1, Me.UploadToolStripMenuItem, Me.ToolStripSeparator2, Me.PauseInputListenerToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(181, 126)
        '
        'PitterToolStripMenuItem
        '
        Me.PitterToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ForceUpdateToolStripMenuItem})
        Me.PitterToolStripMenuItem.Name = "PitterToolStripMenuItem"
        Me.PitterToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.PitterToolStripMenuItem.Text = "Pitter"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(177, 6)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'Passive
        '
        Me.Passive.Enabled = True
        Me.Passive.Interval = 10
        '
        'Cleaner
        '
        Me.Cleaner.Enabled = True
        Me.Cleaner.Interval = 60000
        '
        'UploadToolStripMenuItem
        '
        Me.UploadToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.ClipboardToolStripMenuItem, Me.ToolStripSeparator3, Me.CurrentWindowToolStripMenuItem, Me.FullscreenToolStripMenuItem, Me.SelectionToolStripMenuItem})
        Me.UploadToolStripMenuItem.Name = "UploadToolStripMenuItem"
        Me.UploadToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.UploadToolStripMenuItem.Text = "Upload"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(177, 6)
        '
        'PauseInputListenerToolStripMenuItem
        '
        Me.PauseInputListenerToolStripMenuItem.Name = "PauseInputListenerToolStripMenuItem"
        Me.PauseInputListenerToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.PauseInputListenerToolStripMenuItem.Text = "Pause Input Listener"
        '
        'ClipboardToolStripMenuItem
        '
        Me.ClipboardToolStripMenuItem.Name = "ClipboardToolStripMenuItem"
        Me.ClipboardToolStripMenuItem.Size = New System.Drawing.Size(161, 22)
        Me.ClipboardToolStripMenuItem.Text = "Clipboard"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(161, 22)
        Me.FileToolStripMenuItem.Text = "File Upload"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(158, 6)
        '
        'CurrentWindowToolStripMenuItem
        '
        Me.CurrentWindowToolStripMenuItem.Name = "CurrentWindowToolStripMenuItem"
        Me.CurrentWindowToolStripMenuItem.Size = New System.Drawing.Size(161, 22)
        Me.CurrentWindowToolStripMenuItem.Text = "Current Window"
        '
        'FullscreenToolStripMenuItem
        '
        Me.FullscreenToolStripMenuItem.Name = "FullscreenToolStripMenuItem"
        Me.FullscreenToolStripMenuItem.Size = New System.Drawing.Size(161, 22)
        Me.FullscreenToolStripMenuItem.Text = "Fullscreen"
        '
        'SelectionToolStripMenuItem
        '
        Me.SelectionToolStripMenuItem.Name = "SelectionToolStripMenuItem"
        Me.SelectionToolStripMenuItem.Size = New System.Drawing.Size(161, 22)
        Me.SelectionToolStripMenuItem.Text = "Selection"
        '
        'ForceUpdateToolStripMenuItem
        '
        Me.ForceUpdateToolStripMenuItem.Name = "ForceUpdateToolStripMenuItem"
        Me.ForceUpdateToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.ForceUpdateToolStripMenuItem.Text = "Force Update"
        '
        'Pitter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(935, 523)
        Me.Controls.Add(Me.WebControl1)
        Me.Name = "Pitter"
        Me.Text = "Pitter"
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents WebControl1 As Awesomium.Windows.Forms.WebControl
    Friend WithEvents BrowserEventListener As System.Windows.Forms.Timer
    Friend WithEvents DesktopEventListener As System.Windows.Forms.Timer
    Friend WithEvents TaskbarIcon As System.Windows.Forms.NotifyIcon
    Friend WithEvents Passive As System.Windows.Forms.Timer
    Friend WithEvents Cleaner As System.Windows.Forms.Timer
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents PitterToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UploadToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ClipboardToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents CurrentWindowToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FullscreenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SelectionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents PauseInputListenerToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ForceUpdateToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
