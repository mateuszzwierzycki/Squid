Imports Grasshopper.GUI.Canvas
Imports System.Drawing
Imports System.Drawing.Drawing2D

Public Class AttQuickPreview

    Inherits Grasshopper.Kernel.Attributes.GH_ComponentAttributes

    Public Sub New(ByVal owner As CompQuickPreview)

        MyBase.New(owner)

    End Sub

    Private img As New Bitmap(100, 100)

    Friend Property DisplayImage As Bitmap
        Get
            Return img
        End Get
        Set(value As Bitmap)
            img.Dispose()
            img = Nothing
            img = value.Clone
            ProcessImage()
        End Set
    End Property

    Private Sub ProcessImage()

        Dim w As Integer = img.Width
        Dim h As Integer = img.Height

        Dim tw As Integer
        Dim th As Integer

        Dim max As Integer = 200

        Select Case w > h
            Case True
                tw = max
                th = Math.Ceiling((h * max) / w)
            Case False
                tw = Math.Ceiling((w * max) / h)
                th = max
        End Select

        Dim nimg As New Bitmap(tw, th)

        Using g As Graphics = Graphics.FromImage(nimg)

            g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality

            Using b As TextureBrush = New TextureBrush(My.Resources.backgroundSmall)
                g.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
                g.FillRectangle(b, nimg.GetBounds(GraphicsUnit.Pixel))
            End Using

            If w < tw Or h < th Then
                g.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
            Else
                g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            End If

            g.DrawImage(img, 0, 0, tw, th)
        End Using

        img.Dispose()
        img = Nothing
        img = nimg.Clone
        nimg.Dispose()

    End Sub

    Protected Overrides Sub Layout()

        Dim h As Integer = Me.Bounds.Height
        Dim th As Integer = img.Height + 12
        Me.Pivot = New PointF(Me.Pivot.X, (Me.Pivot.Y - ((th - h) / 2)))

        Me.Pivot = System.Drawing.Point.op_Implicit(Grasshopper.Kernel.GH_Convert.ToPoint(Me.Pivot))
        Me.Bounds = New System.Drawing.RectangleF(Me.Pivot.X, Me.Pivot.Y, img.Width + 29, img.Height + 12)
        Dim paramBox As System.Drawing.RectangleF = Me.Bounds
        paramBox.Inflate(-2, -2)
        Grasshopper.Kernel.Attributes.GH_ComponentAttributes.LayoutInputParams(Me.Owner, paramBox)
        paramBox.X = (paramBox.X + (Me.Owner.Params.Input.Item(0).Attributes.Bounds.Width + 4))
        Grasshopper.Kernel.Attributes.GH_ComponentAttributes.LayoutInputParams(Me.Owner, paramBox)
    End Sub


    Protected Overrides Sub Render(canvas As Grasshopper.GUI.Canvas.GH_Canvas, graphics As Drawing.Graphics, channel As Grasshopper.GUI.Canvas.GH_CanvasChannel)

        Dim cqp As CompQuickPreview = Me.Owner

        Select Case channel
            Case Grasshopper.GUI.Canvas.GH_CanvasChannel.Objects

                MyBase.Render(canvas, graphics, channel)

                Dim pixoff As PixelOffsetMode = graphics.PixelOffsetMode
                Dim intpol As InterpolationMode = graphics.InterpolationMode
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality
                graphics.DrawImage(img, Me.Bounds.Location.X + 23, Me.Bounds.Location.Y + 6, img.Width, img.Height)
                graphics.InterpolationMode = intpol
                graphics.DrawRectangle(Pens.Black, Me.Bounds.Location.X + 23, Me.Bounds.Location.Y + 6, img.Width, img.Height)
                graphics.PixelOffsetMode = pixoff

            Case Else
                MyBase.Render(canvas, graphics, channel)
        End Select


    End Sub


End Class
