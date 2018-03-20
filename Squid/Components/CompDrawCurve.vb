Imports Rhino.Geometry
Imports Grasshopper
Imports Grasshopper.Kernel
Imports GH_IO
Imports System.Drawing

Public Class CompDrawCurve
    Inherits GH_Component

    Public Sub New()
        MyBase.New("Draw", "Draw", "Draw any type of curve using fill and/or outline", "Squid", "Instructions")
    End Sub

    Public Overrides ReadOnly Property ComponentGuid As Guid
        Get
            Return New Guid("{C5A56EAB-238D-4127-BEB9-88B615AF0ADE}")
        End Get
    End Property

    Protected Overrides Sub RegisterInputParams(pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
        pManager.AddCurveParameter("Curve", "C", "Curve to draw", GH_ParamAccess.item)
        pManager.AddNumberParameter("Accuracy", "A", "Accuracy, set to -1 for default accuracy." & vbCrLf & "Polylines and lines are not affected.", GH_ParamAccess.item, -1)
        pManager.AddParameter(New ParamSquidOutline, "Outline", "O", "Outline", GH_ParamAccess.item)
        pManager.AddParameter(New ParamSquidFill, "Fill", "F", "Fill", GH_ParamAccess.item)
        pManager.Param(2).Optional = True
        pManager.Param(3).Optional = True
    End Sub

    Protected Overrides ReadOnly Property Icon As Bitmap
        Get
            Return My.Resources.drawicon
        End Get
    End Property

    Protected Overrides Sub RegisterOutputParams(pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
        pManager.AddParameter(New ParamSquidInstr)
        pManager.AddCurveParameter("Curve", "P", "Due to drawing limitations, almost all curves are transformed into polylines.", GH_ParamAccess.item)
    End Sub


    Protected Overrides Sub SolveInstance(DA As IGH_DataAccess)
        Dim crv As Curve = Nothing
        Dim acc As Double = -1

        Dim outl As New DrawOutline(Color.Transparent, 1)
        Dim fil As DrawFill = Nothing

        If Not (DA.GetData(0, crv)) Then Return
        If Not (DA.GetData(1, acc)) Then Return

        If Not (DA.GetData(2, outl)) And Me.Params.Input(2).SourceCount > 0 Then
            Me.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid outline")
            Return
        End If

        If Not (DA.GetData(3, fil)) And Me.Params.Input(3).SourceCount > 0 Then
            Me.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid fill")
            Return
        End If

        If fil Is Nothing Then fil = New DrawFillColor(Color.Transparent)

        If acc < Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance * 2 And acc <> -1 Then
            acc = -1
            Me.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Invalid accuracy, value set to default = -1")
        End If

        Dim cc As DrawCurve = Nothing

        Dim ptc As New Point3d(CheckCircle(crv))

        If ptc.Z > 0 Then
            Dim cirpl As New Polyline
            cirpl.Add(ptc)

            If fil.FillType = 2 Then


                Dim filtemp As DrawFillLGradient = fil

                Dim linear As New DrawFillLGradient(filtemp.Colors, filtemp.Params, filtemp.GradientLine)

                Dim circ As New Circle(New Point3d(ptc.X, ptc.Y, 0), ptc.Z)
                Dim ln As Line = linear.GradientLine

                Dim npl90 As New Plane(ln.From, ln.To - ln.From)

                Dim bbox As New Box(npl90, circ.ToNurbsCurve)

                bbox.Union(ln.From)
                bbox.Union(ln.To)

                Dim ngradline As New Line(bbox.GetCorners(0), bbox.GetCorners(4))
                ngradline.Extend(10, 10)

                Dim col As New List(Of Color)
                col.Add(linear.Colors(0))
                col.AddRange(linear.Colors)
                col.Add(linear.Colors(linear.Colors.Count - 1))

                Dim params As New List(Of Double)
                params.Add(0)
                For i As Integer = 0 To linear.Params.Count - 1 Step 1
                    params.Add(ngradline.ClosestParameter(linear.GradientLine.PointAt(linear.Params(i))))
                Next
                params.Add(1)

                linear.GradientLine = ngradline
                linear.Params = params
                linear.Colors = col

                cc = New DrawCurve(cirpl, outl, linear)
            Else
                cc = New DrawCurve(cirpl, outl, fil)
            End If


            DA.SetData(1, New Circle(New Rhino.Geometry.Point3d(cirpl(0).X, cirpl(0).Y, 0), cirpl(0).Z).ToNurbsCurve)
        Else

            Dim temppl As New Polyline(New Polyline(DivideCurve(crv, acc)))

            If fil.FillType = 2 Then
                Dim filtemp As DrawFillLGradient = fil

                Dim linear As New DrawFillLGradient(filtemp.Colors, filtemp.Params, filtemp.GradientLine)

                Dim ln As Line = linear.GradientLine

                Dim npl90 As New Plane(ln.From, ln.To - ln.From)

                Dim bbox As New Box(npl90, temppl)

                bbox.Union(ln.From)
                bbox.Union(ln.To)

                Dim ngradline As New Line(bbox.GetCorners(0), bbox.GetCorners(4))
                ngradline.Extend(10, 10)

                Dim col As New List(Of Color)
                col.Add(linear.Colors(0))
                col.AddRange(linear.Colors)
                col.Add(linear.Colors(linear.Colors.Count - 1))

                Dim params As New List(Of Double)
                params.Add(0)
                For i As Integer = 0 To linear.Params.Count - 1 Step 1
                    params.Add(ngradline.ClosestParameter(linear.GradientLine.PointAt(linear.Params(i))))
                Next
                params.Add(1)

                linear.GradientLine = ngradline
                linear.Params = params
                linear.Colors = col

                cc = New DrawCurve(temppl, outl, linear)
            Else
                cc = New DrawCurve(temppl, outl, fil)
            End If

            DA.SetData(1, cc.Curve.ToNurbsCurve)
        End If

        Dim si As New InstrCurve(cc)
        DA.SetData(0, si)
        cc.Dispose()

    End Sub

    Function CheckCircle(crv As Curve) As Point3d
        Dim np As New Point3d(0, 0, 0)

        If crv.IsCircle Then
            Dim cir As New Circle()
            crv.TryGetCircle(cir)

            If cir.IsInPlane(Plane.WorldXY, Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance) Then
                np = New Point3d(cir.Center)
                np.Z = cir.Radius
            End If

        End If

        Return np
    End Function


End Class
