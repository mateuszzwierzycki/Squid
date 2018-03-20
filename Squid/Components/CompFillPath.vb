Imports Rhino.Geometry
Imports Grasshopper
Imports Grasshopper.Kernel
Imports GH_IO
Imports System.Drawing

Public Class CompFillPath
    Inherits GH_Component

    Public Sub New()
        MyBase.New("Fill Path", "FillP", "Create a gradient fill based on a curve", "Squid", "Drawing palette")
    End Sub

    Public Overrides ReadOnly Property ComponentGuid As Guid
        Get
            Return New Guid("{742A6320-5C6A-4912-A419-322C5B0E13B5}")
        End Get
    End Property

    Protected Overrides Sub RegisterInputParams(pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
        pManager.AddCurveParameter("Curve", "C", "Curve to follow", GH_ParamAccess.item)
        pManager.AddNumberParameter("Accuracy", "A", "Accuracy, set to -1 for default accuracy." & vbCrLf & "Polylines and lines are not affected.", GH_ParamAccess.item, -1)
        pManager.AddNumberParameter("Params", "P", "Color parameters", GH_ParamAccess.list)
        pManager.Param(2).Optional = True
        pManager.AddColourParameter("Colors", "C", "Colors", GH_ParamAccess.list)
        pManager.Param(3).Optional = True
        pManager.AddPointParameter("Center", "M", "Optional gradient center", GH_ParamAccess.item)
        pManager.Param(4).Optional = True
    End Sub

    Public Overrides ReadOnly Property Exposure As GH_Exposure
        Get
            Return GH_Exposure.secondary
        End Get
    End Property

    Protected Overrides ReadOnly Property Icon As Bitmap
        Get
            Return My.Resources.fillpath
        End Get
    End Property

    Protected Overrides Sub RegisterOutputParams(pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
        pManager.AddParameter(New ParamSquidFill)
        pManager.AddCurveParameter("Curve", "P", "Due to drawing limitations, almost all curves are transformed into polylines.", GH_ParamAccess.item)
    End Sub

    Dim ptl As New List(Of Point3d)
    Dim pts As New List(Of Double)
    Dim col As New List(Of Color)


    Protected Overrides Sub SolveInstance(DA As IGH_DataAccess)
        ptl.Clear()
        pts.Clear()
        col.Clear()

        Dim cen As New Point3d
        Dim crv As Curve = Nothing
        Dim acc As Double = -1
        Dim changecen As Boolean = False

        If Not (DA.GetData(0, crv)) Then Return
        If Not (DA.GetData(1, acc)) Then Return

        If DA.GetData(4, cen) Then changecen = True

        If Not cen.IsValid Then
            Me.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid center point.")
            Return
        End If

            If Not DA.GetDataList(2, pts) And Not DA.GetDataList(3, col) Then
                pts.Add(0)
                pts.Add(1)
                col.Add(Color.White)
                col.Add(Color.Black)
            End If

            If acc < Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance * 2 And acc <> -1 Then
                acc = -1
                Me.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Invalid accuracy, value set to default = -1")
            End If

            If pts.Count <> col.Count Then
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Please provide equal amount of parameters and colors.")
                Return
            End If

            If pts.Count < 2 Then
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Please provide at least 2 values for parameters and colors.")
                Return
            End If

            Dim ptsarr() As Double = pts.ToArray
            Dim colarr() As Color = col.ToArray

            Array.Sort(ptsarr, colarr)

            pts.Clear()
            col.Clear()
            pts.AddRange(ptsarr)
            col.AddRange(colarr)

            For i As Integer = 0 To pts.Count - 1 Step 1

                pts(i) = 1 - pts(i)

                If pts(i) < 0 Or pts(i) > 1 Then
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Parameter value out of 0 to 1 domain.")
                    Return
                End If

            Next

            pts.Reverse()
            col.Reverse()

            Dim cc As DrawFillPath = Nothing

            Dim npl As New Polyline(DivideCurve(crv, acc))

            DA.SetData(1, npl.ToNurbsCurve)

        ptl.AddRange(npl.ToArray)

        If npl(0).DistanceTo(npl(npl.Count - 1)) <= Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance Then
            ptl.RemoveAt(0)
        End If

        Select Case changecen
            Case True
                cc = New DrawFillPath(col, pts, ptl, cen)
            Case False
                cc = New DrawFillPath(col, pts, ptl)
        End Select

        DA.SetData(0, cc)

    End Sub

End Class
