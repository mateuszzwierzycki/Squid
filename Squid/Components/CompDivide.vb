Imports Rhino.Geometry
Imports Grasshopper
Imports Grasshopper.Kernel
Imports GH_IO
Imports System.Drawing

Public Class CompDivide
    Inherits GH_Component

    Public Sub New()
        MyBase.New("Divide Curve", "Divide", "Use this component as an alternative to the default Draw and DrawEx curve division.", "Squid", "Util")
    End Sub

    Public Overrides ReadOnly Property ComponentGuid As Guid
        Get
            Return New Guid("{97A75278-98DB-4F44-B3DD-B61EEA8BFA38}")
        End Get
    End Property

    Protected Overrides ReadOnly Property Icon As Bitmap
        Get
            Return My.Resources.divideicon
        End Get
    End Property

    Public Overrides ReadOnly Property Exposure As GH_Exposure
        Get
            Return GH_Exposure.tertiary
        End Get
    End Property

    Protected Overrides Sub RegisterInputParams(pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
        pManager.AddCurveParameter("Curve", "C", "Curve to divide", GH_ParamAccess.item)
        pManager.AddNumberParameter("Accuracy", "A", "Division accuracy", GH_ParamAccess.item, 0.1)
    End Sub

    Protected Overrides Sub RegisterOutputParams(pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
        pManager.AddCurveParameter("Polyline", "P", "Resulting polyline", GH_ParamAccess.item)
    End Sub

    Protected Overrides Sub SolveInstance(DA As IGH_DataAccess)
        Dim crv As Curve = Nothing
        Dim acc As Double

        If Not DA.GetData(0, crv) Then Return
        If Not DA.GetData(1, acc) Then Return

        DA.SetData(0, DivideAlternative(crv, acc))

    End Sub
End Class
