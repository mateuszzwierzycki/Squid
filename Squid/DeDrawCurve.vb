Imports Rhino.Geometry
Imports Grasshopper
Imports Grasshopper.Kernel
Imports GH_IO

Public Class DEDrawCurve
    Inherits GH_Component

    Public Sub New()
        MyBase.New("DEDraw Curve", "DEDrawCurve", "Draw any type of curve", "Squid", "Draw")
    End Sub

    Public Overrides ReadOnly Property ComponentGuid As Guid
        Get
            Return New Guid("{A58EDB91-3CDA-4177-BFED-6E1F0B071B5E}")
        End Get
    End Property

    Protected Overrides Sub RegisterInputParams(pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
        pManager.AddParameter(New DrawCurveParam)
    End Sub

    Protected Overrides Sub RegisterOutputParams(pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
        pManager.AddNumberParameter("L", "L", "Length", GH_ParamAccess.item)
        pManager.AddTextParameter("C", "C", "Color", GH_ParamAccess.item)
        pManager.AddTextParameter("T", "T", "Type", GH_ParamAccess.item)
    End Sub

    Dim nCurveDraw As New DrawCurveClass()

    Protected Overrides Sub SolveInstance(DA As IGH_DataAccess)
        Dim crv As DrawCurveClass = Nothing

        If Not (DA.GetData(0, crv)) Then Return



        DA.SetData(0, crv.GetCurve)
        DA.SetData(1, crv.OutlineColor)
        DA.SetData(2, crv.BrushType)
    End Sub

End Class
