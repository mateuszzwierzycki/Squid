Imports Rhino.Geometry
Imports Grasshopper
Imports Grasshopper.Kernel
Imports GH_IO
Imports System.Drawing




Public Class CompFillRadial
    Inherits GH_Component


    Sub New()
        MyBase.New("Fill Radial", "FillR", "Create a radial gradient fill", "Squid", "Drawing palette")
    End Sub

    Public Overrides ReadOnly Property ComponentGuid As Guid
        Get
            Return New Guid("{634C93AE-D2F0-414C-9FD3-5AC34164D150}")
        End Get
    End Property

    Public Overrides ReadOnly Property Exposure As GH_Exposure
        Get
            Return GH_Exposure.secondary
        End Get
    End Property

    Protected Overrides ReadOnly Property Icon As Bitmap
        Get
            Return My.Resources.fillradial2
        End Get
    End Property

    Protected Overrides Sub RegisterInputParams(pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
        pManager.AddCircleParameter("Circle", "R", "Gradient circle", GH_ParamAccess.item)
        pManager.AddNumberParameter("Params", "P", "Color parameters", GH_ParamAccess.list)
        pManager.Param(1).Optional = True
        pManager.AddColourParameter("Colors", "C", "Colors", GH_ParamAccess.list)
        pManager.Param(2).Optional = True
    End Sub

    Protected Overrides Sub RegisterOutputParams(pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
        pManager.AddParameter(New ParamSquidFill)
        pManager.AddCircleParameter("Circles", "C", "Circles", GH_ParamAccess.list)
    End Sub

    Dim l As New Line()
    Dim pts As New List(Of Double)
    Dim col As New List(Of Color)
    Dim cir As New List(Of Circle)

    Protected Overrides Sub SolveInstance(DA As IGH_DataAccess)
        pts.Clear()
        col.Clear()
        cir.Clear()

        Dim cnr As New Circle()

        If Not DA.GetData(0, cnr) Then Return

        If Not cnr.IsInPlane(Plane.WorldXY, Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance) Then
            AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Circle has to lie on the WorldXY plane.")
            Return
        End If

        If Not cnr.IsValid Then
            AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid circle.")
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

        If pts(0) <> 0 Then
            pts.Insert(0, 0)
            col.Insert(0, col(0))
        End If

        If pts(pts.Count - 1) <> 1 Then
            pts.Add(1)
            col.Add(col(col.Count - 1))
        End If

        cir.Clear()

        l = New Line(cnr.Center, cnr.PointAt(0))

        For Each p As Double In pts
            cir.Add(New Circle(l.From, l.From.DistanceTo(l.PointAt(p))))
        Next

        DA.SetDataList(1, cir)

        Dim nfill As New DrawFillCGradient(col, pts, l)
        DA.SetData(0, nfill)

    End Sub


End Class
