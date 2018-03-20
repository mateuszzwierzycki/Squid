Imports Rhino.Geometry
Imports Grasshopper
Imports Grasshopper.Kernel
Imports GH_IO
Imports System.Drawing

Public Class CompOver
    Inherits GH_Component

    Public Sub New()
        MyBase.New("Overwrite", "Over", "Overwrites color", "Squid", "Modifiers")
    End Sub

    Public Overrides ReadOnly Property ComponentGuid As Guid
        Get
            Return New Guid("{DB6CAC86-0CC0-490B-BA96-6C09710C09E3}")
        End Get
    End Property

    Protected Overrides ReadOnly Property Icon As Bitmap
        Get
            Return My.Resources.over2
        End Get
    End Property

    Protected Overrides Sub RegisterInputParams(pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
        pManager.AddParameter(New ParamSquidInstr)
        pManager.AddBooleanParameter("Over", "O", "Enable overwriting", GH_ParamAccess.item, True)
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

        If bool Then sicopy.OverWrite = True

        DA.SetData(0, sicopy)
    End Sub
End Class
