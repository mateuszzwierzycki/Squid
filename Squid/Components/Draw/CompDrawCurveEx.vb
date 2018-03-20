Imports Rhino.Geometry
Imports Grasshopper
Imports Grasshopper.Kernel
Imports GH_IO
Imports System.Drawing

Public Class CompDrawCurveEx
    Inherits GH_Component

    Public Sub New()
        MyBase.New("DrawEx", "DrawEx", "Draw complex shapes using fill and/or outline", "Squid", "Instructions")
    End Sub

    Public Overrides ReadOnly Property ComponentGuid As Guid
        Get
            Return New Guid("{63E7DEF0-3892-4FDD-92B5-BACB417C4D74}")
        End Get
    End Property

    Protected Overrides Sub RegisterInputParams(pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
        pManager.AddCurveParameter("Curves", "C", "Curves to draw. List of curves will be transformed into complex shape.", GH_ParamAccess.list)
        pManager.AddNumberParameter("Accuracy", "A", "Accuracy, set to -1 for default accuracy." & vbCrLf & "Polylines and lines are not affected.", GH_ParamAccess.item, -1)
        pManager.AddParameter(New ParamSquidOutline, "Outline", "O", "Outline", GH_ParamAccess.item)
        pManager.AddParameter(New ParamSquidFill, "Fill", "F", "Fill", GH_ParamAccess.item)
        pManager.AddBooleanParameter("Mode", "M", "Fill mode. True=Winding False=Alternate", GH_ParamAccess.item, False)
        pManager.Param(2).Optional = True
        pManager.Param(3).Optional = True
        pManager.Param(4).Optional = True
    End Sub

    Protected Overrides Sub RegisterOutputParams(pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
        pManager.AddParameter(New ParamSquidInstr)
        pManager.AddCurveParameter("Curve", "P", "Due to drawing limitations, all curves are transformed into polylines.", GH_ParamAccess.item)
    End Sub

    Protected Overrides ReadOnly Property Icon As Bitmap
        Get
            Return My.Resources.drawexicon
        End Get
    End Property

    Protected Overrides Sub SolveInstance(DA As IGH_DataAccess)
        Dim crv As New List(Of Curve)
        Dim acc As Double = -1

        Dim outl As New DrawOutline(Color.Transparent, 1)
        Dim fil As DrawFill = Nothing
        Dim fillmod As Boolean = True

        If Not (DA.GetDataList(0, crv)) Then Return
        If Not (DA.GetData(1, acc)) Then Return

        If Not (DA.GetData(2, outl)) And Me.Params.Input(2).SourceCount > 0 Then
            Me.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid outline")
            Return
        End If

        If Not (DA.GetData(3, fil)) And Me.Params.Input(3).SourceCount > 0 Then
            Me.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid fill")
            Return
        End If

        If Not (DA.GetData(4, fillmod)) Then Return

        If fil Is Nothing Then fil = New DrawFillColor(Color.Transparent)

        If acc < Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance * 2 And acc <> -1 Then
            acc = -1
            Me.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Invalid accuracy, value set to default = -1")
        End If

        Dim divcrv As New List(Of Polyline)
        Dim nurbcrv As New List(Of Curve)
        For Each c As Curve In crv
            divcrv.Add(New Polyline(DivideCurve(c, acc)))
            nurbcrv.Add(divcrv(divcrv.Count - 1).ToNurbsCurve)
        Next


        Dim cc As DrawCurveEx = Nothing

        If fil.FillType = 2 Then
            Dim filtemp As DrawFillLGradient = fil

            Dim linear As New DrawFillLGradient(filtemp.Colors, filtemp.Params, filtemp.GradientLine)

            Dim ln As Line = linear.GradientLine

            Dim npl90 As New Plane(ln.From, ln.To - ln.From)

            Dim temppl As New Polyline

            For Each p As Polyline In divcrv
                temppl.AddRange(p)
            Next

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
            cc = New DrawCurveEx(divcrv, outl, linear, fillmod)
        Else
            cc = New DrawCurveEx(divcrv, outl, fil, fillmod)
        End If



        DA.SetDataList(1, nurbcrv)

        Dim si As New InstrCurveEx(cc)
        DA.SetData(0, si)
        cc.Dispose()

    End Sub


End Class
