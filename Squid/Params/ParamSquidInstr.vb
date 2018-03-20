Imports Rhino
Imports Grasshopper
Imports Grasshopper.Kernel
Imports GH_IO

Public Class ParamSquidInstr
    Inherits GH_Param(Of InstrBase)

    Public Sub New()
        MyBase.New(New GH_InstanceDescription("SquidInstr", "SI", "Squid drawing instruction", "Params", "Geometry"))
    End Sub

    Public Overrides ReadOnly Property ComponentGuid As Guid
        Get
            Return New Guid("{E17E1A48-E1CF-451F-A282-1E1C26788B50}")
        End Get
    End Property

    Public Overrides ReadOnly Property Exposure As GH_Exposure
        Get
            Return GH_Exposure.hidden
        End Get
    End Property

    Protected Overrides ReadOnly Property Icon As Drawing.Bitmap
        Get
            Return Squid.My.Resources.instrparam
        End Get
    End Property
End Class
