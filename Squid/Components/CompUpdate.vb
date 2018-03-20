Imports Grasshopper.Kernel

Public Class CompUpdate

    Inherits GH_Component

    Sub New()
        MyBase.New("Update", "Update", "Updates the Squid window while still processing the solution", "Squid", "Instructions")
    End Sub

    Public Overrides ReadOnly Property ComponentGuid As Guid
        Get
            Return New Guid("{00CD6066-31E9-4E7D-9581-D52473BAC596}")
        End Get
    End Property

    'Protected Overrides ReadOnly Property Icon As BitMap
    '    Get
    '        Return My.Resources.alpha
    '    End Get
    'End Property

    Protected Overrides Sub RegisterInputParams(pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)

    End Sub

    Protected Overrides Sub RegisterOutputParams(pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
        pManager.AddParameter(New ParamSquidInstr)
    End Sub

    Protected Overrides Sub SolveInstance(DA As IGH_DataAccess)

        DA.SetData(0, New InstrUpdate)

    End Sub

End Class
