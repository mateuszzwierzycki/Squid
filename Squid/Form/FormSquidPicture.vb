Imports System.Drawing
Imports Grasshopper.Kernel
Imports System.Windows.Forms

Public Class FormSquidPicture

    Private pic As New Bitmap(500, 300)
    Private zoom As Double = 1.0
    Private trans As New Drawing.Point(0, 0)
    Private transdrop As New Drawing.Point(5, 5)
    Private autohide As Boolean
    Private mycomponent As CompSquid = Nothing
    Private fullscreen As Boolean = False
    Private fullscreensize As Size
    Private fullscreenloc As Point

    Friend WriteOnly Property OwnerComponent
        Set(value)
            mycomponent = value
        End Set
    End Property

    Friend Property Picture As Bitmap
        Set(value As Bitmap)
            pic = value.Clone
            PicWindow.Image = pic
            TransformPicture()
        End Set
        Get
            Return pic '.Clone
        End Get
    End Property

    Friend Property AutohideSwitch As Boolean
        Set(value As Boolean)
            autohide = value
        End Set
        Get
            Return autohide
        End Get
    End Property

    Friend Sub TransformPicture()
        Me.PicWindow.Size = New Size(pic.Width * zoom, pic.Height * zoom)
        Me.PicWindow.Location = trans
        Me.DropShadow.Size = Me.PicWindow.Size
        Me.DropShadow.Location = Me.PicWindow.Location + transdrop
        FormStatus()
    End Sub

    Friend Sub FormStatus()
        Select Case autohide
            Case True
                Me.Text = "Squid [zoom:" & Math.Round(zoom * 100, 2) & "% " & "size:" & pic.Width & "x" & pic.Height & "px autohide:true]"
            Case False
                Me.Text = "Squid [zoom:" & Math.Round(zoom * 100, 2) & "% " & "size:" & pic.Width & "x" & pic.Height & "px autohide:false]"
        End Select
    End Sub

    Private Sub PicWindow_Wheel(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseWheel

        Dim zoompre As Double = zoom

        If e.Delta < 0 Then
            zoom = 0.9 * Math.Round(zoom, 2)
        ElseIf e.Delta > 0 Then
            zoom = 1.1 * Math.Round(zoom, 2)
        End If

        If zoom > 0.9 And zoom < 1.1 Then zoom = 1
        If zoom < 0.1 Then zoom = 0.1
        If zoom > 20 Then zoom = 20

        trans.X += (e.X - PicWindow.Location.X) - ((e.X - PicWindow.Location.X) * (zoom / zoompre))
        trans.Y += (e.Y - PicWindow.Location.Y) - ((e.Y - PicWindow.Location.Y) * (zoom / zoompre))

        If zoom < 1.5 Then
            PicWindow.Interpolation = Drawing2D.InterpolationMode.HighQualityBicubic
        Else
            PicWindow.Interpolation = Drawing2D.InterpolationMode.NearestNeighbor
        End If

        TransformPicture()
    End Sub

    Private Sub Form_SquidPicture_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PicWindow.Show()
        PicWindow.Size = pic.Size
        Me.DropShadow.Size = Me.PicWindow.Size
        Me.DropShadow.Location = Me.PicWindow.Location + transdrop
        AutoTransform()
    End Sub

    Private Sub Form_SquidPicture_Resize(sender As Object, e As EventArgs) Handles Me.ResizeEnd
        TransformPicture()
    End Sub

    Dim temppt As New Drawing.Point
    Dim pres As Boolean

    Private Sub Pic_DClick(sender As Object, e As Windows.Forms.MouseEventArgs) Handles Me.MouseDoubleClick, PicWindow.MouseDoubleClick
        If e.Button = Windows.Forms.MouseButtons.Left Then
            If fullscreen Then
                AutoTransform(0)
            Else
                AutoTransform()
            End If
        End If
    End Sub

    Dim savef As New Windows.Forms.SaveFileDialog()

    Private Sub Pic_CtrlS(sender As Object, e As Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If Windows.Forms.Control.ModifierKeys = Windows.Forms.Keys.Control And e.KeyCode = Windows.Forms.Keys.S Then
            saveAction()
        End If

        If Windows.Forms.Control.ModifierKeys = Windows.Forms.Keys.Control And e.KeyCode = Windows.Forms.Keys.C Then

            Dim picClip As New Bitmap(pic.Width, pic.Height)
            Using g As Graphics = Graphics.FromImage(picClip)
                g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
                g.Clear(Color.White)
                g.DrawImage(pic, New Drawing.Point(0, 0))
            End Using

            System.Windows.Forms.Clipboard.SetImage(picClip)

        End If

        If e.KeyCode = Windows.Forms.Keys.Home Then

            Dim ownerrect As RectangleF = (mycomponent.Attributes.Bounds)

            Dim zo As Single

            Dim gap As Integer = 300
            Dim actc As Grasshopper.GUI.Canvas.GH_Canvas = Grasshopper.Instances.ActiveCanvas
            Dim zr As New RectangleF(ownerrect.X - gap, ownerrect.Y - gap, ownerrect.Width + (gap * 2), ownerrect.Height + (gap * 2))

            If zr.Height / zr.Width > actc.Height / actc.Width Then
                zo = actc.Height / zr.Height
            Else
                zo = actc.Width / zr.Width
            End If

            Dim nv As New Grasshopper.GUI.Canvas.GH_NamedView("OO", New PointF(zr.X + zr.Width / 2, zr.Y + zr.Height / 2), zo, Grasshopper.GUI.Canvas.GH_NamedViewType.center)

            nv.SetToViewport(Grasshopper.Instances.ActiveCanvas, 500)
        End If

        If e.KeyCode = Windows.Forms.Keys.F1 Then

            MsgBox(HelpText, MsgBoxStyle.OkOnly, "Squid Help")

        End If

        If e.KeyCode = Windows.Forms.Keys.F11 Then

            fullscreen = Not fullscreen

            If fullscreen Then
                PicWindow.BorderStyle = BorderStyle.None
                Me.BackColor = Color.Black
                DropShadow.Visible = False
                fullscreenloc = Me.Location
                fullscreensize = Me.Size
                Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
                'Me.Location = New Point(0, 0)
                'Me.Size = SystemInformation.PrimaryMonitorSize
                Me.WindowState = FormWindowState.Maximized
                Me.SizeGripStyle = Windows.Forms.SizeGripStyle.Hide
                AutoTransform(0)
            Else
                PicWindow.BorderStyle = BorderStyle.FixedSingle
                Me.WindowState = FormWindowState.Normal
                Me.SizeGripStyle = Windows.Forms.SizeGripStyle.Show
                Me.BackColor = Drawing.Color.FromArgb(236, 236, 236)
                DropShadow.Visible = True
                Me.FormBorderStyle = Windows.Forms.FormBorderStyle.SizableToolWindow
                Me.Location = fullscreenloc
                Me.Size = fullscreensize
                AutoTransform()
            End If

        End If

        If e.KeyCode = Windows.Forms.Keys.Escape Then

            If fullscreen Then
                PicWindow.BorderStyle = BorderStyle.FixedSingle
                Me.WindowState = FormWindowState.Normal
                Me.SizeGripStyle = Windows.Forms.SizeGripStyle.Show
                fullscreen = Not fullscreen
                Me.BackColor = Drawing.Color.FromArgb(236, 236, 236)
                DropShadow.Visible = True
                Me.FormBorderStyle = Windows.Forms.FormBorderStyle.SizableToolWindow
                Me.Location = fullscreenloc
                Me.Size = fullscreensize
                AutoTransform()
            Else
                Me.Visible = False
                Grasshopper.Instances.DocumentEditor.Focus()
            End If

        End If

    End Sub

    Function HelpText() As String

        Dim nstr As New String("")

        nstr += "Double click to zoom the image." & vbCrLf & vbCrLf
        nstr += "Press Ctrl+S to save the image." & vbCrLf & vbCrLf
        nstr += "Press Ctrl+C to copy the image to the clipboard." & vbCrLf
        nstr += "Clipboard can't handle transparency," & vbCrLf & "image will have a white background." & vbCrLf & vbCrLf
        nstr += "Press Home button to zoom to the owner component of this form." & vbCrLf & vbCrLf
        nstr += "Press F11 to enable the full screen mode." & vbCrLf
        nstr += "To quit the full screen mode, either press ESC or F11."
        Return nstr


    End Function

    Private Sub Pic_MouseMove(sender As Object, e As Windows.Forms.MouseEventArgs) Handles PicWindow.MouseMove

        If Not pres Then Return
        Me.PicWindow.Location += e.Location - temppt
        Me.DropShadow.Location = Me.PicWindow.Location + transdrop

    End Sub

    Private Sub Pic_MouseDown(sender As Object, e As Windows.Forms.MouseEventArgs) Handles PicWindow.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Left Or e.Button = Windows.Forms.MouseButtons.Right Then
            temppt.X = 0
            temppt.Y = 0
            temppt = e.Location
            pres = True
        End If
    End Sub

    Private Sub Pic_MouseUp(sender As Object, e As Windows.Forms.MouseEventArgs) Handles PicWindow.MouseUp
        If e.Button = Windows.Forms.MouseButtons.Left Or e.Button = Windows.Forms.MouseButtons.Right Then
            Me.PicWindow.Location += e.Location - temppt
            trans = Me.PicWindow.Location
            TransformPicture()
            pres = False
            Me.Invalidate(True)
        End If
    End Sub

    Friend Sub AutoTransform()
        Dim hz As Double = (((Me.ClientSize.Width - 20) / PicWindow.Width)) * zoom
        Dim vz As Double = (((Me.ClientSize.Height - 20) / PicWindow.Height)) * zoom

        zoom = Math.Min(hz, vz)

        trans.X = (ClientSize.Width / 2) - ((pic.Width * zoom) / 2)
        trans.Y = (ClientSize.Height / 2) - ((pic.Height * zoom) / 2)

        If zoom < 1 Then
            PicWindow.Interpolation = Drawing2D.InterpolationMode.HighQualityBicubic
        Else
            PicWindow.Interpolation = Drawing2D.InterpolationMode.NearestNeighbor
        End If

        TransformPicture()
    End Sub

    Friend Sub AutoTransform(Gap As Integer)
        Dim hz As Double = (((Me.ClientSize.Width - Gap) / PicWindow.Width)) * zoom
        Dim vz As Double = (((Me.ClientSize.Height - Gap) / PicWindow.Height)) * zoom

        zoom = Math.Min(hz, vz)

        trans.X = (ClientSize.Width / 2) - ((pic.Width * zoom) / 2)
        trans.Y = (ClientSize.Height / 2) - ((pic.Height * zoom) / 2)

        If zoom < 1 Then
            PicWindow.Interpolation = Drawing2D.InterpolationMode.HighQualityBicubic
        Else
            PicWindow.Interpolation = Drawing2D.InterpolationMode.NearestNeighbor
        End If

        TransformPicture()
    End Sub

    Private Sub Form_Close(sender As Object, e As Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If e.CloseReason = Windows.Forms.CloseReason.UserClosing Then
            e.Cancel = True
            Me.Visible = False
            Grasshopper.Instances.DocumentEditor.Focus()
        Else
            Me.Close()
        End If

    End Sub

    Private Sub Form_LostFocus(sender As Object, e As System.EventArgs) Handles Me.LostFocus
        If Me.Visible Then
            mycomponent.Message = "Opened"
        Else
            mycomponent.Message = ""
        End If

        mycomponent.OnDisplayExpired(True)
        If autohide Then
            Me.Visible = False
            Grasshopper.Instances.DocumentEditor.Focus()
        End If

    End Sub

    Private Sub Form_GotFocus(sender As Object, e As System.EventArgs) Handles Me.GotFocus
        mycomponent.Message = "Active"
        mycomponent.OnDisplayExpired(True)
    End Sub

    Friend Sub saveAction()
        savef.Filter = "PNG (RGB) (*.png)|*.png|TIFF (CMYK+Alpha) (*.tif)|*.tif"
        savef.Title = "Save as..."

        If savef.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim formstr As New String(savef.FileName)

            formstr = formstr.Substring(formstr.Length - 3, 3)

            Select Case formstr
                Case "png"
                    pic.Save(savef.FileName, Imaging.ImageFormat.Png)
                Case "tif"
                    SaveTiffImage(pic, savef.FileName)
            End Select

        End If
    End Sub

End Class