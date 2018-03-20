Imports Rhino.Geometry
Imports Grasshopper
Imports Grasshopper.Kernel
Imports GH_IO
Imports System.Drawing


Public Class CompReadBitmap
    Inherits GH_Component

    Sub New()
        MyBase.New("Read", "Read", "Read a bitmap from file, then use it as Squid bitmap.", "Squid", "Util")
    End Sub

    Public Overrides ReadOnly Property ComponentGuid As Guid
        Get
            Return New Guid("{CD6FC5C0-E53C-4CB2-AD60-3939D6CA9B48}")
        End Get
    End Property

    Public Overrides ReadOnly Property Exposure As GH_Exposure
        Get
            Return GH_Exposure.quarternary
        End Get
    End Property

    Protected Overrides Sub RegisterInputParams(pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
        pManager.AddTextParameter("Path", "F", "File Path", GH_ParamAccess.item)
    End Sub

    Protected Overrides Sub RegisterOutputParams(pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
        pManager.AddParameter(New ParamSquidBitmap, "Bitmap", "B", "Squid bitmap", GH_ParamAccess.item)
        pManager.AddRectangleParameter("Bounds", "R", "Bitmap rectangle", GH_ParamAccess.item)
    End Sub

    Protected Overrides ReadOnly Property Icon As Bitmap
        Get
            Return My.Resources.read
        End Get
    End Property

    Protected Overrides Sub SolveInstance(DA As IGH_DataAccess)
        Dim fil As New String("")

        DA.GetData(0, fil)

        If Not System.IO.File.Exists(fil) Then
            AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "File does not exist.")
            Return
        End If

        Dim nbmp As New Bitmap(fil)

        DA.SetData(0, New DrawBitmap(nbmp))
        DA.SetData(1, New Rhino.Geometry.Rectangle3d(Plane.WorldXY, nbmp.Width, nbmp.Height))
        nbmp.Dispose()

    End Sub


End Class
