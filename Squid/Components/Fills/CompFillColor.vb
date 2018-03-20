Imports Rhino.Geometry
Imports Grasshopper
Imports Grasshopper.Kernel
Imports GH_IO
Imports System.Drawing


Public Class CompFillColor
    Inherits GH_Component


    Sub New()
        MyBase.New("Fill Solid", "FillS", "Create a solid fill", "Squid", "Drawing palette")
    End Sub

    Public Overrides ReadOnly Property Exposure As GH_Exposure
        Get
            Return GH_Exposure.secondary
        End Get
    End Property

    Public Overrides ReadOnly Property ComponentGuid As Guid
        Get
            Return New Guid("{0454436C-F373-45B7-8CC7-591548E836EE}")
        End Get
    End Property

    Protected Overrides ReadOnly Property Icon As Bitmap
        Get
            Return My.Resources.hotpink
        End Get
    End Property

    Protected Overrides Sub RegisterInputParams(pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
        pManager.AddColourParameter("Color", "C", "Outline color", GH_ParamAccess.item, Color.HotPink)
    End Sub

    Protected Overrides Sub RegisterOutputParams(pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
        pManager.AddParameter(New ParamSquidFill)
    End Sub

    Protected Overrides Sub SolveInstance(DA As IGH_DataAccess)

        Dim col As Color = Color.HotPink

        If Not (DA.GetData(0, col)) Then Return

        Dim drw As New DrawFillColor(col)
        DA.SetData(0, drw)

    End Sub


End Class
