
Imports Rhino.Geometry
Imports Grasshopper
Imports Grasshopper.Kernel
Imports GH_IO
Imports System.Drawing
Imports Grasshopper.Kernel.Data
Imports Grasshopper.Kernel.Types

Public Class CompScreenGrab

    Inherits GH_Component


    Sub New()
        MyBase.New("Screen Map", "Map", "Maps the points to an active viewport coordinates." & vbCrLf & "Update by double clicking OR boolean input change OR the timer.", "Squid", "Util")
    End Sub

    Public Overrides ReadOnly Property ComponentGuid As Guid
        Get
            Return New Guid("{53E90C83-730E-4A36-9B7D-A063C721CC6D}")
        End Get
    End Property

    Public Overrides Sub CreateAttributes()
        m_attributes = New AttGrab(Me)
    End Sub

    Public Overrides ReadOnly Property Exposure As GH_Exposure
        Get
            Return GH_Exposure.quarternary
        End Get
    End Property


    Protected Overrides ReadOnly Property Icon As Bitmap
        Get
            Return My.Resources.map
        End Get
    End Property

    Protected Overrides Sub RegisterInputParams(pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
        pManager.AddPointParameter("Points", "P", "Points to map", GH_ParamAccess.tree)
        pManager.AddBooleanParameter("Run", "R", "Change boolean value to restart", GH_ParamAccess.item, True)
        pManager.Param(0).Optional = True
    End Sub

    Protected Overrides Sub RegisterOutputParams(pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
        pManager.AddPointParameter("Mapped", "M", "Points in screen space. Z coordinate = distance to the camera location", GH_ParamAccess.tree)
        pManager.AddRectangleParameter("Bounds", "B", "Viewport rectangle", GH_ParamAccess.item)
    End Sub

    Protected Overrides Sub SolveInstance(DA As IGH_DataAccess)
        Dim pts As New GH_Structure(Of GH_Point)

        If Not DA.GetDataTree(0, pts) Then Return

        Dim dt As New DataTree(Of Point3d)

        Dim v As Rhino.Display.RhinoView = Rhino.RhinoDoc.ActiveDoc.Views.ActiveView

        Dim rect As Rectangle = v.ActiveViewport.Bounds
        DA.SetData(1, New Rectangle3d(Plane.WorldXY, rect.Width, rect.Height))

        For i As Integer = 0 To pts.PathCount - 1 Step 1
            For j As Integer = 0 To pts.Branch(pts.Path(i)).Count - 1 Step 1
                Dim thisgh As GH_Point = pts.Branch(pts.Path(i)).Item(j)
                Dim p As New Point2d(v.ActiveViewport.WorldToClient(thisgh.Value))
                Dim p3 As New Point3d(p.X, -p.Y + rect.Height, thisgh.Value.DistanceTo(v.ActiveViewport.CameraLocation))
                dt.Add(p3, New GH_Path(pts.Path(i)))
            Next
        Next

        DA.SetDataTree(0, dt)

    End Sub


End Class
