Imports Rhino.Geometry
Imports Grasshopper
Imports Grasshopper.Kernel
Imports GH_IO
Imports System.Drawing


Public Class CompFillTexture
    Inherits GH_Component


    Sub New()
        MyBase.New("Fill Texture", "FillT", "Create a texture fill", "Squid", "Drawing palette")
    End Sub

    Public Overrides ReadOnly Property Exposure As GH_Exposure
        Get
            Return GH_Exposure.secondary
        End Get
    End Property

    Public Overrides ReadOnly Property ComponentGuid As Guid
        Get
            Return New Guid("{F91A2F93-4B1F-4359-A918-9B7044F02DCD}")
        End Get
    End Property

    Protected Overrides ReadOnly Property Icon As Bitmap
        Get
            Return My.Resources.filltex
        End Get
    End Property

    Protected Overrides Sub RegisterInputParams(pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
        pManager.AddParameter(New ParamSquidBitmap, "Bitmap", "B", "Squid bitmap", GH_ParamAccess.item)
        pManager.AddRectangleParameter("Rectangle", "R", "Boundary rectangle", GH_ParamAccess.item, New Rhino.Geometry.Rectangle3d(Plane.WorldXY, 100, 100))
        pManager.AddIntegerParameter("Tiling", "T", "0=Clamp 1=Tile 2=TileFlipX 3=TileFlipXY 4=TileFlipY", GH_ParamAccess.item, 1)
    End Sub

    Protected Overrides Sub RegisterOutputParams(pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
        pManager.AddParameter(New ParamSquidFill)
    End Sub

    Protected Overrides Sub SolveInstance(DA As IGH_DataAccess)


        Dim fil As New DrawBitmap()
        Dim rect As New Rhino.Geometry.Rectangle3d(Plane.WorldXY, 100, 100)
        Dim til As Integer = 1



        If Not (DA.GetData(0, fil)) Then Return
        If Not (DA.GetData(1, rect)) Then Return
        If Not (DA.GetData(2, til)) Then Return


        If Not rect.ToNurbsCurve.IsInPlane(Plane.WorldXY) Then
            AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Rectangle has to lie on the WorldXY plane.")
            Return
        End If

        Dim wrapm As Drawing2D.WrapMode = Drawing2D.WrapMode.Tile

        Select Case til
            Case 0
                wrapm = Drawing2D.WrapMode.Clamp
            Case 1
                wrapm = Drawing2D.WrapMode.Tile
            Case 2
                wrapm = Drawing2D.WrapMode.TileFlipX
            Case 3
                wrapm = Drawing2D.WrapMode.TileFlipXY
            Case 4
                wrapm = Drawing2D.WrapMode.TileFlipY
            Case Else
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Invalid tiling value, tiling set to default -> Tile")
                wrapm = Drawing2D.WrapMode.Tile
        End Select

        Dim nfil As New DrawFillTexture(fil.Image, rect, wrapm)

        DA.SetData(0, nfil)

    End Sub


End Class
