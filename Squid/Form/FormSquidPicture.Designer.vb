<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormSquidPicture
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
        Me.DropShadow = New System.Windows.Forms.PictureBox()
        Me.PicWindow = New MyPic()
        CType(Me.DropShadow, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PicWindow, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DropShadow
        '
        Me.DropShadow.BackColor = System.Drawing.Color.Silver
        Me.DropShadow.Location = New System.Drawing.Point(197, 158)
        Me.DropShadow.Name = "DropShadow"
        Me.DropShadow.Size = New System.Drawing.Size(219, 108)
        Me.DropShadow.TabIndex = 1
        Me.DropShadow.TabStop = False
        '
        'PicWindow
        '
        Me.PicWindow.BackColor = System.Drawing.Color.White
        Me.PicWindow.BackgroundImage = My.Resources.Resources.background3
        Me.PicWindow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PicWindow.Location = New System.Drawing.Point(192, 153)
        Me.PicWindow.Name = "PicWindow"
        Me.PicWindow.Size = New System.Drawing.Size(219, 108)
        Me.PicWindow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PicWindow.TabIndex = 2
        Me.PicWindow.TabStop = False
        '
        'FormSquidPicture
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(236, Byte), Integer), CType(CType(236, Byte), Integer), CType(CType(236, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(592, 368)
        Me.Controls.Add(Me.PicWindow)
        Me.Controls.Add(Me.DropShadow)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(150, 100)
        Me.Name = "FormSquidPicture"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Squid"
        CType(Me.DropShadow, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PicWindow, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents DropShadow As System.Windows.Forms.PictureBox
    Friend WithEvents PicWindow As MyPic
End Class
