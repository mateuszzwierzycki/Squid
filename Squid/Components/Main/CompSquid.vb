Imports Rhino
Imports Grasshopper
Imports Grasshopper.Kernel
Imports GH_IO
Imports Grasshopper.Kernel.Data
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports Rhino.Geometry
Imports Grasshopper.Kernel.Types
Imports System.Windows.Forms

Public Class CompSquid
    Inherits GH_Component

    Private Shared autohide As Boolean = False

    Public Sub New()
        MyBase.New("Squid", "Squid", "Draws the picture", "Squid", "Squid")
        draw.OwnerComponent = Me
        draw.Picture = bmp
        draw.TransformPicture()
    End Sub

    Public Overrides ReadOnly Property ComponentGuid As Guid
        Get
            Return New Guid("{B1D172B1-1D78-422F-B711-C69C03AAA0BF}")
        End Get
    End Property

    Public Overrides Sub CreateAttributes()
        m_attributes = New AttSquid(Me)
    End Sub

    Public Overrides Sub AddedToDocument(document As GH_Document)
        draw.AutoTransform()
        MyBase.AddedToDocument(document)
    End Sub

    Protected Overrides Sub AppendAdditionalComponentMenuItems(menu As System.Windows.Forms.ToolStripDropDown)
        MyBase.AppendAdditionalComponentMenuItems(menu)

        GH_DocumentObject.Menu_AppendItem(menu, "Autohide (affects all Squids)", AddressOf AutohideSwitch, True, autohide)
        GH_DocumentObject.Menu_AppendItem(menu, "Save as...", AddressOf Me.draw.saveAction)

    End Sub

    Friend ReadOnly Property AutohideState
        Get
            Return autohide
        End Get
    End Property

    Private Sub AutohideSwitch()
        autohide = Not autohide
        Me.draw.AutohideSwitch = autohide
        Me.draw.FormStatus()
        If Not Me.draw.Focused Then
            Me.draw.Visible = False
            Grasshopper.Instances.DocumentEditor.Focus()
        End If
    End Sub

    Public Overrides Function Read(reader As Serialization.GH_IReader) As Boolean
        autohide = reader.GetBoolean("autohide")
        Return MyBase.Read(reader)
    End Function

    Public Overrides Function Write(writer As Serialization.GH_IWriter) As Boolean
        writer.SetBoolean("autohide", autohide)
        Return MyBase.Write(writer)
    End Function


    Protected Overrides ReadOnly Property Icon As Bitmap
        Get
            Return My.Resources.squid
        End Get
    End Property

    Protected Overrides Sub RegisterInputParams(pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
        pManager.AddRectangleParameter("BitmapR", "R", "Bitmap rectangle", GH_ParamAccess.tree, New Rhino.Geometry.Rectangle3d(Plane.WorldXY, 500, 300))
        pManager.AddParameter(New ParamSquidInstr, "SquidInstr", "SI", "Squid drawing instruction", GH_ParamAccess.tree)
        Me.Params.Input(1).Optional = True
        pManager.AddNumberParameter("PPU", "P", "Pixels per unit", GH_ParamAccess.tree, 1)
        Me.Params.Input(2).Optional = True
        pManager.AddBooleanParameter("AA", "A", "Enable AntiAliasing", GH_ParamAccess.tree, True)
    End Sub

    Protected Overrides Sub RegisterOutputParams(pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
        pManager.AddParameter(New ParamSquidBitmap, "Bitmap", "B", "Squid bitmap", GH_ParamAccess.item)
    End Sub

    Friend draw As New FormSquidPicture()
    Dim bmp As New Bitmap(500, 300)
    Dim pix As Double = 1
    Dim prepix As Double = 1
    Dim sqbmp As New DrawBitmap()

    Dim pw As Double
    Dim ph As Double
    Dim prew As Double
    Dim preh As Double

    Protected Overrides Sub SolveInstance(DA As IGH_DataAccess)

        'Gather all inputs
        Dim aa As New GH_Structure(Of GH_Boolean)
        If Not (DA.GetDataTree(3, aa)) Then Return

        Dim antialias As Boolean = True
        If Not aa.AllData(True)(0).CastTo(antialias) Then Return

        Dim sqi As New GH_Structure(Of InstrBase)
        If Not (DA.GetDataTree(1, sqi)) And Me.Params.Input(1).SourceCount > 0 Then Return

        Dim rects As New GH_Structure(Of GH_Rectangle)
        If Not (DA.GetDataTree(0, rects)) Then Return

        Dim rect As New Grasshopper.Kernel.Types.GH_Rectangle
        rect = rects.AllData(True)(0)
        If Not rect.Value.ToNurbsCurve.IsInPlane(Plane.WorldXY, Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance) Then
            AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Rectangle has to lie on the WorldXY plane.")
            Return
        End If
        Dim bbox As New BoundingBox(rect.Boundingbox.GetCorners)

        'Orientation rectangle (different coordinate system)
        Dim orientrect As New Rhino.Geometry.Rectangle3d(New Plane(bbox.GetCorners(0), New Vector3d(0, 0, 1)), bbox.GetCorners(0).DistanceTo(bbox.GetCorners(1)), bbox.GetCorners(0).DistanceTo(bbox.GetCorners(3)))

        'To detect size change
        pw = orientrect.Width
        ph = orientrect.Height

        'To detect resolution change
        Dim ppu As New GH_Structure(Of GH_Number)
        prepix = pix

        If DA.GetDataTree(2, ppu) Then
            Dim ghn As New GH_Number(ppu.AllData(False)(0))
            Dim val As Double
            If ghn.CastTo(val) Then
                If val < 0.1 Then pix = 0.1
                pix = ghn.Value
            Else
                pix = 1
            End If
        End If

        'new bitmap
        bmp = New Bitmap(CInt(orientrect.Width * pix), CInt(orientrect.Height * pix))

        'fill with transparent background
        Using g As Graphics = Graphics.FromImage(bmp)

            g.Clear(Color.FromArgb(0, 0, 0, 0))

        End Using

        'draw using instructions
        Using g As Graphics = Graphics.FromImage(bmp)
            'graphics setup
            g.SmoothingMode = SmoothingMode.None
            g.TextRenderingHint = Text.TextRenderingHint.AntiAliasGridFit

            If antialias Then g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

            For Each si As InstrBase In sqi.AllData(True)

                If si.OverWrite Then
                    g.CompositingMode = CompositingMode.SourceCopy
                Else
                    g.CompositingMode = CompositingMode.SourceOver
                End If

                Select Case si.InstructionType

                    'Case "Update"

                    '    draw.Picture = bmp
                    '    draw.PicWindow.Refresh()

                    '    'Dim st As New System.Diagnostics.Stopwatch
                    '    'st.Start()

                    '    'While st.ElapsedMilliseconds < 100
                    '    'End While

                    '    'st.Reset()

                    Case "Text" '=====================================================================================================================

                        Dim itext As InstrText = si

                        '===SET EVERYTHING

                        Using ColorPen As New Pen(Brushes.Black)

                            SetPen(ColorPen, itext.Outline)

                            Dim FillBrush As Brush = Nothing

                            Select Case itext.Fill.FillType
                                Case 0
                                    Dim cf As DrawFillColor = itext.Fill
                                    FillBrush = New SolidBrush(cf.FillColor)

                                    '===================================== not supported because transformations.
                                    'Case 1

                                    '    Dim tf As DrawFillTexture = itext.Fill
                                    '    Dim ntex As New TextureBrush(tf.Texture)

                                    '    ntex.WrapMode = tf.Wrap

                                    '    ' FixTextTransforms(ntex, tf, orientrect)

                                    '    FillBrush = ntex.Clone
                                    '    ntex.Dispose()

                                    'Case 2
                                    '    Dim lf As DrawFillLGradient = itext.Fill
                                    '    Dim nlin As New LinearGradientBrush(OrientPoint(lf.GradientLine.From, orientrect, pix) _
                                    '                                        , OrientPoint(lf.GradientLine.To, orientrect, pix), _
                                    '                                        Color.White, Color.Black)

                                    '    Dim nblend As New ColorBlend(lf.Colors.Count)
                                    '    nblend.Colors = lf.Colors.ToArray

                                    '    Dim pos(lf.Params.Count - 1) As Single
                                    '    For j As Integer = 0 To lf.Params.Count - 1 Step 1
                                    '        pos(j) = CSng(lf.Params(j))
                                    '    Next

                                    '    nblend.Positions = pos
                                    '    nlin.InterpolationColors = nblend

                                    '    FillBrush = nlin.Clone

                                    '    nlin.Dispose()
                                    'Case 3
                                    '    Dim lf As DrawFillCGradient = itext.Fill
                                    '    Dim cir As New GraphicsPath()
                                    '    cir.AddEllipse(LineToCircle(lf.GradientLine, orientrect, pix))
                                    '    Dim pthGrBrush As New PathGradientBrush(cir)

                                    '    Dim posi(lf.Params.Count - 1) As Single
                                    '    Dim coli(lf.Params.Count - 1) As Color
                                    '    For i As Integer = 0 To lf.Params.Count - 1 Step 1
                                    '        posi(i) = CSng(lf.Params(i))
                                    '        coli(i) = lf.Colors(i)
                                    '    Next

                                    '    Dim colorBlend As New ColorBlend()
                                    '    colorBlend.Colors = coli
                                    '    colorBlend.Positions = posi
                                    '    pthGrBrush.InterpolationColors = colorBlend

                                    '    FillBrush = pthGrBrush.Clone
                                    '    cir.Dispose()
                                    '    pthGrBrush.Dispose()

                                Case 255

                            End Select


                            FixText(itext, orientrect, g)

                            Using nstr As New StringFormat
                                Using textpath As New GraphicsPath

                                    nstr.Trimming = StringTrimming.Word
                                    nstr.FormatFlags = StringFormatFlags.NoClip
                                    If itext.Paragraph.RightToLeft Then nstr.FormatFlags += StringFormatFlags.DirectionRightToLeft

                                    Select Case itext.Paragraph.HAlign
                                        Case 0
                                            nstr.Alignment = StringAlignment.Near
                                        Case 1
                                            nstr.Alignment = StringAlignment.Center
                                        Case 2
                                            nstr.Alignment = StringAlignment.Far
                                    End Select

                                    Select Case itext.Paragraph.VAlign
                                        Case 0
                                            nstr.LineAlignment = StringAlignment.Near
                                        Case 1
                                            nstr.LineAlignment = StringAlignment.Center
                                        Case 2
                                            nstr.LineAlignment = StringAlignment.Far
                                    End Select

                                    Dim textrec As New Drawing.RectangleF(0, 0, itext.LayoutRectangle.Width * pix, itext.LayoutRectangle.Height * pix)
                                    Dim fstyle As FontStyle = itext.Font.FontStyle

                                    Dim ffam As New FontFamily(itext.Font.FontFamily)
                                    textpath.FillMode = FillMode.Winding
                                    textpath.AddString(itext.Text, ffam, itext.Font.FontStyle, CSng(itext.Font.FontSize * pix), textrec, nstr)

                                    If itext.FlipOrder Then
                                        g.DrawPath(ColorPen, textpath)
                                        g.FillPath(FillBrush, textpath)
                                    Else
                                        g.FillPath(FillBrush, textpath)
                                        g.DrawPath(ColorPen, textpath)
                                    End If

                                    ffam.Dispose()
                                    textpath.Dispose()

                                End Using
                            End Using


                            g.ResetTransform()

                            FillBrush.Dispose()
                        End Using




                    Case "Image" '=====================================================================================================================

                        Dim insim As InstrImage = si

                        Select Case insim.DrawImage.Interpolation
                            Case 0
                                g.InterpolationMode = InterpolationMode.Bilinear
                            Case 1
                                g.InterpolationMode = InterpolationMode.Bicubic
                            Case 2
                                g.InterpolationMode = InterpolationMode.HighQualityBilinear
                            Case 3
                                g.InterpolationMode = InterpolationMode.HighQualityBicubic
                            Case 4
                                g.InterpolationMode = InterpolationMode.NearestNeighbor
                            Case Else
                                g.InterpolationMode = InterpolationMode.Bilinear
                        End Select

                        FixImage(insim.DrawImage, orientrect, g)

                        g.PixelOffsetMode = PixelOffsetMode.HighQuality
                        g.CompositingQuality = CompositingQuality.HighQuality

                        g.DrawImage(insim.DrawImage.Image, 0, 0, CSng(insim.DrawImage.Rectangle.Width * pix), CSng(insim.DrawImage.Rectangle.Height * pix))

                        g.PixelOffsetMode = PixelOffsetMode.Default
                        g.CompositingQuality = CompositingQuality.Default

                        g.ResetTransform()

                        g.InterpolationMode = InterpolationMode.Default


                    Case "Mask"

                        Dim insim As InstrMask = si

                        Using maskbmp As New Bitmap(bmp.Width, bmp.Height)
                            Using mg As Graphics = Graphics.FromImage(maskbmp)
                                mg.Clear(Color.Black)

                                Select Case insim.DrawImage.Interpolation
                                    Case 0
                                        mg.InterpolationMode = InterpolationMode.Bilinear
                                    Case 1
                                        mg.InterpolationMode = InterpolationMode.Bicubic
                                    Case 2
                                        mg.InterpolationMode = InterpolationMode.HighQualityBilinear
                                    Case 3
                                        mg.InterpolationMode = InterpolationMode.HighQualityBicubic
                                    Case 4
                                        mg.InterpolationMode = InterpolationMode.NearestNeighbor
                                    Case Else
                                        mg.InterpolationMode = InterpolationMode.Bilinear
                                End Select

                                FixImage(insim.DrawImage, orientrect, mg)

                                mg.PixelOffsetMode = PixelOffsetMode.HighQuality
                                mg.CompositingQuality = CompositingQuality.HighQuality

                                g.PixelOffsetMode = PixelOffsetMode.HighQuality
                                g.CompositingQuality = CompositingQuality.HighQuality

                                mg.DrawImage(insim.DrawImage.Image, 0, 0, CSng(insim.DrawImage.Rectangle.Width * pix), CSng(insim.DrawImage.Rectangle.Height * pix))
                                ApplyMask(bmp, maskbmp)


                                g.PixelOffsetMode = PixelOffsetMode.Default
                                g.CompositingQuality = CompositingQuality.Default


                            End Using
                        End Using

                    Case "Draw" '=====================================================================================================================

                        '===SET EVERYTHING

                        Dim dc As InstrCurve = si

                        Using ColorPen As New Pen(Brushes.Black)

                            SetPen(ColorPen, dc.DrawCurve.Outline)

                            Dim FillBrush As Brush = Nothing

                            Select Case dc.DrawCurve.Fill.FillType
                                Case 0
                                    Dim cf As DrawFillColor = dc.DrawCurve.Fill
                                    FillBrush = New SolidBrush(cf.FillColor)
                                Case 1

                                    Dim tf As DrawFillTexture = dc.DrawCurve.Fill
                                    Dim ntex As New TextureBrush(tf.Texture)

                                    ntex.WrapMode = tf.Wrap

                                    FixTransforms(ntex, tf, orientrect)

                                    FillBrush = ntex.Clone
                                    ntex.Dispose()

                                Case 2
                                    Dim lf As DrawFillLGradient = dc.DrawCurve.Fill
                                    Dim nlin As New LinearGradientBrush(OrientPoint(lf.GradientLine.From, orientrect, pix) _
                                                                        , OrientPoint(lf.GradientLine.To, orientrect, pix), _
                                                                        Color.White, Color.Black)

                                    Dim nblend As New ColorBlend(lf.Colors.Count)
                                    nblend.Colors = lf.Colors.ToArray

                                    Dim pos(lf.Params.Count - 1) As Single
                                    For j As Integer = 0 To lf.Params.Count - 1 Step 1
                                        pos(j) = CSng(lf.Params(j))
                                    Next

                                    nblend.Positions = pos
                                    nlin.InterpolationColors = nblend

                                    FillBrush = nlin.Clone

                                    nlin.Dispose()
                                Case 3

                                    Dim lf As DrawFillCGradient = dc.DrawCurve.Fill
                                    Dim cir As New GraphicsPath()
                                    cir.AddEllipse(LineToCircle(lf.GradientLine, orientrect, pix))
                                    Dim pthGrBrush As New PathGradientBrush(cir)

                                    Dim posi(lf.Params.Count - 1) As Single
                                    Dim coli(lf.Params.Count - 1) As Color
                                    For i As Integer = 0 To lf.Params.Count - 1 Step 1
                                        posi(i) = CSng(lf.Params(i))
                                        coli(i) = lf.Colors(i)
                                    Next

                                    Dim colorBlend As New ColorBlend()
                                    colorBlend.Colors = coli
                                    colorBlend.Positions = posi
                                    pthGrBrush.InterpolationColors = colorBlend

                                    FillBrush = pthGrBrush.Clone
                                    cir.Dispose()
                                    pthGrBrush.Dispose()

                                Case 4

                                    Dim pg As DrawFillPath = dc.DrawCurve.Fill

                                    Dim gpt As New List(Of Drawing.PointF)
                                    Dim gpar As New List(Of Byte)

                                    For i As Integer = 0 To pg.GradientPoints.Count - 1 Step 1
                                        gpt.Add(OrientPoint(pg.GradientPoints(i), orientrect, pix))
                                    Next

                                    For i As Integer = 0 To pg.GradientPoints.Count - 1 Step 1
                                        If i = 0 Then
                                            gpar.Add(Drawing2D.PathPointType.Start)
                                        ElseIf i = pg.GradientPoints.Count - 1 Then
                                            gpar.Add(Drawing2D.PathPointType.Line)
                                        Else
                                            gpar.Add(Drawing2D.PathPointType.Line)
                                        End If
                                    Next

                                    Dim gpath As New GraphicsPath(gpt.ToArray, gpar.ToArray)
                                    Dim pathg As New PathGradientBrush(gpath)

                                    Dim colorBlend As New ColorBlend()

                                    Dim posi(pg.Params.Count - 1) As Single
                                    Dim coli(pg.Params.Count - 1) As Color
                                    For i As Integer = 0 To pg.Params.Count - 1 Step 1
                                        posi(i) = CSng(pg.Params(i))
                                        coli(i) = pg.Colors(i)
                                    Next

                                    colorBlend.Colors = coli
                                    colorBlend.Positions = posi
                                    pathg.InterpolationColors = colorBlend

                                    If pg.IsCenterChanged Then pathg.CenterPoint = OrientPoint(pg.Center, orientrect, pix)

                                    FillBrush = pathg.Clone

                                    gpath.Dispose()
                                    pathg.Dispose()

                                Case 255

                            End Select

                            '===NOW DRAW

                            Dim nc As Polyline = dc.DrawCurve.Curve
                            Dim np As New List(Of Drawing.PointF)
                            Dim npt As New List(Of Byte)

                            For i As Integer = 0 To nc.Count - 1 Step 1
                                np.Add(OrientPoint(nc(i), orientrect, pix))
                            Next

                            If nc.Count = 1 Then
                                Dim pt As New Point3d(nc(0))
                                If pt.Z > 0 Then
                                    Dim r As RectangleF = PointToRect(nc(0), orientrect, pix)
                                    g.FillEllipse(FillBrush, r)
                                    g.DrawEllipse(ColorPen, r)
                                    Exit Select
                                End If
                            End If

                            If nc(0).DistanceTo(nc(nc.Count - 1)) <= Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance Then

                                If dc.FlipOrder Then
                                    g.DrawPolygon(ColorPen, np.ToArray)
                                    g.FillPolygon(FillBrush, np.ToArray, Drawing2D.FillMode.Winding)
                                Else
                                    g.FillPolygon(FillBrush, np.ToArray, Drawing2D.FillMode.Winding)
                                    g.DrawPolygon(ColorPen, np.ToArray)
                                End If

                                Exit Select
                            End If

                            For i As Integer = 0 To nc.Count - 1 Step 1
                                If i = 0 Then
                                    npt.Add(Drawing2D.PathPointType.Start)
                                ElseIf i = nc.Count - 1 Then
                                    npt.Add(Drawing2D.PathPointType.Line)
                                Else
                                    npt.Add(Drawing2D.PathPointType.Line)
                                End If
                            Next

                            Dim ddd As New Drawing2D.GraphicsPath()

                            Dim gp As New Drawing2D.GraphicsPath(np.ToArray, npt.ToArray)
                            g.DrawPath(ColorPen, gp)

                            gp.Dispose()
                            FillBrush.Dispose()
                        End Using

                    Case "DrawEx" '=====================================================================================================================

                        Dim dex As InstrCurveEx = si

                        '===SET EVERYTHING

                        Using ColorPen As New Pen(Brushes.Black)

                            SetPen(ColorPen, dex.DrawCurveEx.Outline)

                            Dim FillBrush As Brush = Nothing

                            Select Case dex.DrawCurveEx.Fill.FillType
                                Case 0
                                    Dim cf As DrawFillColor = dex.DrawCurveEx.Fill
                                    FillBrush = New SolidBrush(cf.FillColor)
                                Case 1
                                    Dim tf As DrawFillTexture = dex.DrawCurveEx.Fill
                                    Dim ntex As New TextureBrush(tf.Texture)

                                    ntex.WrapMode = tf.Wrap

                                    FixTransforms(ntex, tf, orientrect)

                                    FillBrush = ntex.Clone
                                    ntex.Dispose()
                                Case 2
                                    Dim lf As DrawFillLGradient = dex.DrawCurveEx.Fill
                                    Dim nlin As New LinearGradientBrush(OrientPoint(lf.GradientLine.From, orientrect, pix) _
                                                                        , OrientPoint(lf.GradientLine.To, orientrect, pix), _
                                                                        Color.White, Color.Black)

                                    Dim nblend As New ColorBlend(lf.Colors.Count)
                                    nblend.Colors = lf.Colors.ToArray

                                    Dim pos(lf.Params.Count - 1) As Single
                                    For j As Integer = 0 To lf.Params.Count - 1 Step 1
                                        pos(j) = CSng(lf.Params(j))
                                    Next

                                    nblend.Positions = pos
                                    nlin.InterpolationColors = nblend

                                    FillBrush = nlin.Clone

                                    nlin.Dispose()
                                Case 3

                                    Dim lf As DrawFillCGradient = dex.DrawCurveEx.Fill
                                    Dim cir As New GraphicsPath()
                                    cir.AddEllipse(LineToCircle(lf.GradientLine, orientrect, pix))
                                    Dim pthGrBrush As New PathGradientBrush(cir)

                                    Dim posi(lf.Params.Count - 1) As Single
                                    Dim coli(lf.Params.Count - 1) As Color
                                    For i As Integer = 0 To lf.Params.Count - 1 Step 1
                                        posi(i) = CSng(lf.Params(i))
                                        coli(i) = lf.Colors(i)
                                    Next

                                    Dim colorBlend As New ColorBlend()
                                    colorBlend.Colors = coli
                                    colorBlend.Positions = posi
                                    pthGrBrush.InterpolationColors = colorBlend

                                    FillBrush = pthGrBrush.Clone
                                    cir.Dispose()
                                    pthGrBrush.Dispose()

                                Case 4

                                    Dim pg As DrawFillPath = dex.DrawCurveEx.Fill

                                    Dim gpt As New List(Of Drawing.PointF)
                                    Dim gpar As New List(Of Byte)

                                    For i As Integer = 0 To pg.GradientPoints.Count - 1 Step 1
                                        gpt.Add(OrientPoint(pg.GradientPoints(i), orientrect, pix))
                                    Next

                                    For i As Integer = 0 To pg.GradientPoints.Count - 1 Step 1
                                        If i = 0 Then
                                            gpar.Add(Drawing2D.PathPointType.Start)
                                        ElseIf i = pg.GradientPoints.Count - 1 Then
                                            gpar.Add(Drawing2D.PathPointType.Line)
                                        Else
                                            gpar.Add(Drawing2D.PathPointType.Line)
                                        End If
                                    Next

                                    Dim gpath As New GraphicsPath(gpt.ToArray, gpar.ToArray)
                                    Dim pathg As New PathGradientBrush(gpath)

                                    Dim colorBlend As New ColorBlend()

                                    Dim posi(pg.Params.Count - 1) As Single
                                    Dim coli(pg.Params.Count - 1) As Color
                                    For i As Integer = 0 To pg.Params.Count - 1 Step 1
                                        posi(i) = CSng(pg.Params(i))
                                        coli(i) = pg.Colors(i)
                                    Next

                                    colorBlend.Colors = coli
                                    colorBlend.Positions = posi
                                    pathg.InterpolationColors = colorBlend

                                    If pg.IsCenterChanged Then pathg.CenterPoint = OrientPoint(pg.Center, orientrect, pix)
                                    FillBrush = pathg.Clone

                                    gpath.Dispose()
                                    pathg.Dispose()

                                Case 255

                            End Select

                            '===NOW DRAW

                            Dim nc As List(Of Polyline) = dex.DrawCurveEx.Curves


                            Dim ngr As New GraphicsPath()
                            Dim np As New List(Of Drawing.PointF)
                            Dim npt As New List(Of Byte)

                            For Each pl As Polyline In nc

                                np.Clear()
                                npt.Clear()

                                For i As Integer = 0 To pl.Count - 1 Step 1
                                    np.Add(OrientPoint(pl(i), orientrect, pix))
                                Next

                                For i As Integer = 0 To pl.Count - 1 Step 1
                                    If i = 0 Then
                                        npt.Add(Drawing2D.PathPointType.Start)
                                    ElseIf i = pl.Count - 1 Then
                                        npt.Add(Drawing2D.PathPointType.Line)

                                        Dim npath As New GraphicsPath(np.ToArray, npt.ToArray)
                                        ngr.AddPath(npath, False)
                                        npath.Dispose()

                                        If pl(0).DistanceTo(pl(pl.Count - 1)) < Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance Then
                                            ngr.CloseFigure()
                                        Else
                                            ngr.StartFigure()
                                        End If

                                    Else
                                        npt.Add(Drawing2D.PathPointType.Line)
                                    End If
                                Next

                            Next


                            Select Case dex.DrawCurveEx.FillMode
                                Case True
                                    ngr.FillMode = FillMode.Winding
                                Case False
                                    ngr.FillMode = FillMode.Alternate
                            End Select

                            If dex.FlipOrder Then
                                g.DrawPath(ColorPen, ngr)
                                g.FillPath(FillBrush, ngr)
                            Else
                                g.FillPath(FillBrush, ngr)
                                g.DrawPath(ColorPen, ngr)
                            End If

                            ngr.Dispose()
                            FillBrush.Dispose()
                        End Using

                    Case "Save" '=====================================================================================================================

                        Dim isave As InstrSave = si

                        Dim savepth As New String("")
                        savepth += isave.Folder

                        If (Not System.IO.Directory.Exists(savepth)) Then System.IO.Directory.CreateDirectory(savepth)

                        Select Case isave.SaveAsTiff
                            Case True
                                savepth += "\" & isave.FileName & ".tif"
                                SaveTiffImage(bmp, savepth)
                            Case False
                                savepth += "\" & isave.FileName & ".png"
                                bmp.Save(savepth, Imaging.ImageFormat.Png)
                        End Select

                        'SaveTiff(bmp, savepth)

                    Case "ApplyEffect" '=====================================================================================================================

                    Case "Clear" '=====================================================================================================================

                        Dim ic As InstrClear = si
                        g.Clear(ic.Clear)

                End Select

            Next

        End Using

        draw.Picture = bmp

        If prepix <> pix Or pw <> prew Or ph <> preh Then
            prew = pw
            preh = ph
            prepix = pix
            draw.AutoTransform()
        End If

        sqbmp.Image = bmp
        DA.SetData(0, sqbmp)

    End Sub

    Sub ScalePattern(ByRef p As Pen, WithDash As Boolean)

        Dim pat() As Single = Nothing

        pat = p.DashPattern

        For i As Integer = 0 To pat.Length - 1 Step 1
            If p.Width >= 1 Then
                pat(i) *= pix
                pat(i) /= p.Width
            Else
                pat(i) *= pix
            End If
        Next
        p.DashPattern = pat

    End Sub

    Sub SetPen(ByRef p As Pen, ByRef dout As DrawOutline)

        p.Width = CSng(pix * dout.Width)
        If p.Width <= 0 Then p.Width = 1
        p.LineJoin = dout.LineJoin
        p.DashCap = dout.DashCap
        p.EndCap = dout.EndCap
        p.StartCap = dout.StartCap
        p.Color = dout.Color

        If dout.Pattern.Length > 1 Then
            Dim str As New String(dout.Pattern)
            Dim spl() As String = str.Split(",")
            Dim pl As New List(Of Single)
            Dim thissingle As Single = 1
            For Each s As String In spl
                Single.TryParse(s, thissingle)
                pl.Add(thissingle)
            Next

            If pl.Count > 1 Then
                p.DashPattern = pl.ToArray
                ScalePattern(p, True)
            End If
        Else
            p.DashStyle = Drawing2D.DashStyle.Solid
        End If


    End Sub


    Sub FixTransforms(ByRef ntex As TextureBrush, ByRef tf As DrawFillTexture, ByRef picrect As Rectangle3d)
        Dim transpt As PointF = OrientPoint(tf.Rectangle.PointAt(0, 1), picrect, pix)

        Dim rx As New Vector3d(tf.Rectangle.PointAt(1, 0) - tf.Rectangle.PointAt(0, 0))
        Dim ry As New Vector3d(tf.Rectangle.PointAt(0, 1) - tf.Rectangle.PointAt(0, 0))

        If rx.Length < Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance Or ry.Length < Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance Then
            Me.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid rectangle")
            Return
        End If

        Dim xv As New Vector3d(Vector3d.CrossProduct(rx, ry))

        Dim ra As Double = Vector3d.VectorAngle(rx, New Vector3d(1, 0, 0), Plane.WorldXY)

        ra = Rhino.RhinoMath.ToDegrees(ra)

        ntex.TranslateTransform(transpt.X, transpt.Y)

        Select Case xv.Z
            Case Is < 0
                ntex.RotateTransform(180 + ra)
                ntex.ScaleTransform(-pix * (tf.Rectangle.Width / tf.Texture.Width), pix * (tf.Rectangle.Height / tf.Texture.Height))
            Case Is > 0
                ntex.RotateTransform(ra)
                ntex.ScaleTransform(pix * (tf.Rectangle.Width / tf.Texture.Width), pix * (tf.Rectangle.Height / tf.Texture.Height))
        End Select



    End Sub

    Sub FixImage(ByRef ti As DrawImage, ByRef picrect As Rectangle3d, ByRef G As Graphics)

        Dim transpt As Drawing.PointF = OrientPoint(ti.Rectangle.PointAt(0, 1), picrect, pix)
        Dim angle As Single = 0

        G.TranslateTransform(transpt.X, transpt.Y)

        Dim rx As New Vector3d(ti.Rectangle.PointAt(1, 0) - ti.Rectangle.PointAt(0, 0))
        Dim ry As New Vector3d(ti.Rectangle.PointAt(0, 1) - ti.Rectangle.PointAt(0, 0))

        If rx.Length < Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance Or ry.Length < Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance Then
            Me.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid rectangle")
            Return
        End If

        Dim xv As New Vector3d(Vector3d.CrossProduct(rx, ry))

        Dim ra As Double = Vector3d.VectorAngle(rx, New Vector3d(1, 0, 0), Plane.WorldXY)

        ra = Rhino.RhinoMath.ToDegrees(ra)

        Select Case xv.Z
            Case Is < 0
                G.RotateTransform(180 + ra)
                G.ScaleTransform(-1, 1)
            Case Is > 0
                G.RotateTransform(ra)
        End Select

    End Sub

    Sub FixImage(ByRef ti As DrawMask, ByRef picrect As Rectangle3d, ByRef G As Graphics)

        Dim transpt As Drawing.PointF = OrientPoint(ti.Rectangle.PointAt(0, 1), picrect, pix)
        Dim angle As Single = 0

        G.TranslateTransform(transpt.X, transpt.Y)

        Dim rx As New Vector3d(ti.Rectangle.PointAt(1, 0) - ti.Rectangle.PointAt(0, 0))
        Dim ry As New Vector3d(ti.Rectangle.PointAt(0, 1) - ti.Rectangle.PointAt(0, 0))

        If rx.Length < Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance Or ry.Length < Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance Then
            Me.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid rectangle")
            Return
        End If

        Dim xv As New Vector3d(Vector3d.CrossProduct(rx, ry))

        Dim ra As Double = Vector3d.VectorAngle(rx, New Vector3d(1, 0, 0), Plane.WorldXY)

        ra = Rhino.RhinoMath.ToDegrees(ra)

        Select Case xv.Z
            Case Is < 0
                G.RotateTransform(180 + ra)
                G.ScaleTransform(-1, 1)
            Case Is > 0
                G.RotateTransform(ra)
        End Select

    End Sub

    Sub FixText(ByRef ti As InstrText, ByRef picrect As Rectangle3d, ByRef G As Graphics)
        Dim transpt As Drawing.PointF = (OrientPoint(ti.LayoutRectangle.PointAt(0, 1), picrect, pix))
        Dim angle As Single = 0

        G.TranslateTransform(transpt.X, transpt.Y)

        Dim rx As New Vector3d(ti.LayoutRectangle.PointAt(1, 0) - ti.LayoutRectangle.PointAt(0, 0))
        Dim ry As New Vector3d(ti.LayoutRectangle.PointAt(0, 1) - ti.LayoutRectangle.PointAt(0, 0))

        If rx.Length < Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance Or ry.Length < Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance Then
            Me.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid rectangle")
            Return
        End If

        Dim xv As New Vector3d(Vector3d.CrossProduct(rx, ry))

        Dim ra As Double = Vector3d.VectorAngle(rx, New Vector3d(1, 0, 0), Plane.WorldXY)

        ra = Rhino.RhinoMath.ToDegrees(ra)

        Select Case xv.Z
            Case Is < 0
                G.RotateTransform(180 + ra)
                G.ScaleTransform(-1, 1)
            Case Is > 0
                G.RotateTransform(ra)
        End Select

    End Sub



End Class
