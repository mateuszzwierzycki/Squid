Imports Rhino.Geometry
Imports Grasshopper
Imports Grasshopper.Kernel
Imports GH_IO
Imports System.Drawing
Imports Grasshopper.Kernel.Data
Imports Grasshopper.Kernel.Types



Public Class CompFillLinear
    Inherits GH_Component


    Sub New()
        MyBase.New("Fill Linear", "FillL", "Create a linear gradient fill", "Squid", "Drawing palette")
    End Sub

    Public Overrides ReadOnly Property Exposure As GH_Exposure
        Get
            Return GH_Exposure.secondary
        End Get
    End Property

    Public Overrides ReadOnly Property ComponentGuid As Guid
        Get
            Return New Guid("{26368D50-C038-41F4-A28B-F61B908E1426}")
        End Get
    End Property

    Protected Overrides ReadOnly Property Icon As Bitmap
        Get
            Return My.Resources.filllinear2
        End Get
    End Property

    Protected Overrides Sub RegisterInputParams(pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
        pManager.AddLineParameter("Line", "L", "Gradient line", GH_ParamAccess.item)
        pManager.AddNumberParameter("Params", "P", "Color parameters", GH_ParamAccess.list)
        pManager.Param(1).Optional = True
        pManager.AddColourParameter("Colors", "C", "Colors", GH_ParamAccess.list)
        pManager.Param(2).Optional = True
    End Sub

    Protected Overrides Sub RegisterOutputParams(pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
        pManager.AddParameter(New ParamSquidFill)
        pManager.AddPointParameter("Points", "P", "Points", GH_ParamAccess.list)
    End Sub

    Dim l As New Line()
    Dim pts As New List(Of Double)
    Dim col As New List(Of Color)

    Protected Overrides Sub SolveInstance(DA As IGH_DataAccess)
        pts.Clear()
        col.Clear()
        If Not DA.GetData(0, l) Then Return

        If l.Length < Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance Then
            AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Line is too short.")
            Return
        End If

        If Not DA.GetDataList(1, pts) And Not DA.GetDataList(2, col) Then
            pts.Add(0)
            pts.Add(1)
            col.Add(Color.White)
            col.Add(Color.Black)
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

        ' MsgBox(pts.Count.ToString)

        'For i As Integer = 0 To pts.Count - 1 Step 1

        '    pts(i) = pts(i)

        '    If pts(i) < 0 Or pts(i) > 1 Then
        '        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Parameter value out of 0 to 1 domain.")
        '        Return
        '    End If

        'Next

        Dim pout As New List(Of Point3d)

        For Each p As Double In pts
            pout.Add(l.PointAt(p))
        Next

        DA.SetDataList(1, pout)

        Dim nfill As New DrawFillLGradient(col, pts, l)
        DA.SetData(0, nfill)
    End Sub

End Class
