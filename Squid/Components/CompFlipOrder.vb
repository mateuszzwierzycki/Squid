Imports Rhino.Geometry
Imports Grasshopper
Imports Grasshopper.Kernel
Imports GH_IO
Imports System.Drawing

Public Class CompFlipOrder
    Inherits GH_Component

    Public Sub New()
        MyBase.New("Flip Order", "Flip", "Draw fill over outline", "Squid", "Modifiers")
    End Sub

    Public Overrides ReadOnly Property ComponentGuid As Guid
        Get
            Return New Guid("{639284A1-6B9C-41A6-8E17-8A764E487D62}")
        End Get
    End Property

    Protected Overrides ReadOnly Property Icon As Bitmap
        Get
            Return My.Resources.fliporder
        End Get
    End Property

    Protected Overrides Sub RegisterInputParams(pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
        pManager.AddParameter(New ParamSquidInstr)
        pManager.AddBooleanParameter("Flip", "F", "Flip order boolean", GH_ParamAccess.item, True)
    End Sub

    Protected Overrides Sub RegisterOutputParams(pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
        pManager.AddParameter(New ParamSquidInstr)
    End Sub

    Protected Overrides Sub SolveInstance(DA As IGH_DataAccess)
        Dim si As InstrBase = Nothing

        If Not (DA.GetData(0, si)) Then Return

        Dim bool As Boolean = True
        If Not (DA.GetData(1, bool)) Then Return

        Dim sicopy As New InstrBase
        sicopy = si.Duplicate

        If bool Then sicopy.FlipOrder = True

        DA.SetData(0, sicopy)
    End Sub
End Class
