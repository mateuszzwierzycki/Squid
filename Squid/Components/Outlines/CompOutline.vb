Imports Rhino.Geometry
Imports Grasshopper
Imports Grasshopper.Kernel
Imports GH_IO
Imports System.Drawing


Public Class CompOutline
    Inherits GH_Component


    Sub New()
        MyBase.New("Outline", "Outline", "Create an outline", "Squid", "Drawing palette")
    End Sub

    Public Overrides ReadOnly Property ComponentGuid As Guid
        Get
            Return New Guid("{5E6D68AF-0DC6-462C-BA7D-E717EA6CA192}")
        End Get
    End Property

    Public Overrides ReadOnly Property Exposure As GH_Exposure
        Get
            Return GH_Exposure.primary
        End Get
    End Property

    Protected Overrides ReadOnly Property Icon As Bitmap
        Get
            Return My.Resources.outline3
        End Get
    End Property

    Protected Overrides Sub RegisterInputParams(pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
        pManager.AddColourParameter("Color", "C", "Outline color", GH_ParamAccess.item, Color.Black)
        pManager.AddNumberParameter("Width", "W", "Outline width", GH_ParamAccess.item, 1)
    End Sub

    Protected Overrides Sub RegisterOutputParams(pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
        pManager.AddParameter(New ParamSquidOutline)
    End Sub

    Protected Overrides Sub SolveInstance(DA As IGH_DataAccess)
        Dim col As Color = Nothing
        Dim wid As Double = 1

        If Not (DA.GetData(0, col)) Then Return
        If Not (DA.GetData(1, wid)) Then Return

        Dim nout As New DrawOutline(col, CSng(wid))

        DA.SetData(0, nout) '.Duplicate)

    End Sub



End Class
