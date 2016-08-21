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
        Me.BrowserRelogEvent = New System.Windows.Forms.Timer(Me.components)
        Me.DesktopEventListener = New System.Windows.Forms.Timer(Me.components)
        Me.TaskbarIcon = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.PitterToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ForceUpdateToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SynchronizeSettingsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.RunAsAdministratorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.UploadToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ClipboardToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.CurrentWindowToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FullscreenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SelectionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.PauseInputListenerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Passive = New System.Windows.Forms.Timer(Me.components)
        Me.Cleaner = New System.Windows.Forms.Timer(Me.components)
        Me.WebBrowser1 = New System.Windows.Forms.WebBrowser()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'BrowserRelogEvent
        '
        Me.BrowserRelogEvent.Interval = 900000
        '
        'DesktopEventListener
        '
        Me.DesktopEventListener.Enabled = True
        Me.DesktopEventListener.Interval = 10
        '
        'TaskbarIcon
        '
        Me.TaskbarIcon.ContextMenuStrip = Me.ContextMenuStrip1
        Me.TaskbarIcon.Visible = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PitterToolStripMenuItem, Me.ToolStripSeparator1, Me.UploadToolStripMenuItem, Me.ToolStripSeparator2, Me.PauseInputListenerToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(181, 104)
        '
        'PitterToolStripMenuItem
        '
        Me.PitterToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ForceUpdateToolStripMenuItem, Me.SynchronizeSettingsToolStripMenuItem, Me.ToolStripSeparator4, Me.RunAsAdministratorToolStripMenuItem})
        Me.PitterToolStripMenuItem.Name = "PitterToolStripMenuItem"
        Me.PitterToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.PitterToolStripMenuItem.Text = "Pitter"
        '
        'ForceUpdateToolStripMenuItem
        '
        Me.ForceUpdateToolStripMenuItem.Name = "ForceUpdateToolStripMenuItem"
        Me.ForceUpdateToolStripMenuItem.Size = New System.Drawing.Size(185, 22)
        Me.ForceUpdateToolStripMenuItem.Text = "Force Update"
        '
        'SynchronizeSettingsToolStripMenuItem
        '
        Me.SynchronizeSettingsToolStripMenuItem.Name = "SynchronizeSettingsToolStripMenuItem"
        Me.SynchronizeSettingsToolStripMenuItem.Size = New System.Drawing.Size(185, 22)
        Me.SynchronizeSettingsToolStripMenuItem.Text = "Synchronize Settings"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(182, 6)
        '
        'RunAsAdministratorToolStripMenuItem
        '
        Me.RunAsAdministratorToolStripMenuItem.Name = "RunAsAdministratorToolStripMenuItem"
        Me.RunAsAdministratorToolStripMenuItem.Size = New System.Drawing.Size(185, 22)
        Me.RunAsAdministratorToolStripMenuItem.Text = "Run as Administrator"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(177, 6)
        '
        'UploadToolStripMenuItem
        '
        Me.UploadToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.ClipboardToolStripMenuItem, Me.ToolStripSeparator3, Me.CurrentWindowToolStripMenuItem, Me.FullscreenToolStripMenuItem, Me.SelectionToolStripMenuItem})
        Me.UploadToolStripMenuItem.Name = "UploadToolStripMenuItem"
        Me.UploadToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.UploadToolStripMenuItem.Text = "Upload"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(161, 22)
        Me.FileToolStripMenuItem.Text = "File Upload"
        '
        'ClipboardToolStripMenuItem
        '
        Me.ClipboardToolStripMenuItem.Name = "ClipboardToolStripMenuItem"
        Me.ClipboardToolStripMenuItem.Size = New System.Drawing.Size(161, 22)
        Me.ClipboardToolStripMenuItem.Text = "Clipboard"
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
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'Passive
        '
        Me.Passive.Enabled = True
        '
        'Cleaner
        '
        Me.Cleaner.Enabled = True
        Me.Cleaner.Interval = 60000
        '
        'WebBrowser1
        '
        Me.WebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.WebBrowser1.Location = New System.Drawing.Point(0, 0)
        Me.WebBrowser1.MinimumSize = New System.Drawing.Size(20, 20)
        Me.WebBrowser1.Name = "WebBrowser1"
        Me.WebBrowser1.Size = New System.Drawing.Size(935, 523)
        Me.WebBrowser1.TabIndex = 2
        Me.WebBrowser1.Url = New System.Uri("", System.UriKind.Relative)
        '
        'WebApp
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(935, 523)
        Me.Controls.Add(Me.WebBrowser1)
        Me.Name = "WebApp"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Pitter"
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
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
    Public WithEvents BrowserRelogEvent As System.Windows.Forms.Timer
    Public WithEvents DesktopEventListener As System.Windows.Forms.Timer
    Public WithEvents TaskbarIcon As System.Windows.Forms.NotifyIcon
    Public WithEvents Passive As System.Windows.Forms.Timer
    Public WithEvents Cleaner As System.Windows.Forms.Timer
    Public WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SynchronizeSettingsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As ToolStripSeparator
    Friend WithEvents RunAsAdministratorToolStripMenuItem As ToolStripMenuItem
    Public WithEvents WebBrowser1 As WebBrowser
End Class
