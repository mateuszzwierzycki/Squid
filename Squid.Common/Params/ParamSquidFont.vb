Imports Rhino
Imports Grasshopper
Imports Grasshopper.Kernel
Imports GH_IO

Public Class ParamSquidFont
    Inherits GH_Param(Of DrawFont)

    Public Sub New()
        MyBase.New(New GH_InstanceDescription("Font", "F", "Squid font", "Params", "Geometry"))
    End Sub

    Public Overrides ReadOnly Property ComponentGuid As Guid
        Get
            Return New Guid("{ACC2C814-AEEA-4D0A-A4E9-56D60B0AC69F}")
        End Get
    End Property

    Public Overrides ReadOnly Property Exposure As GH_Exposure
        Get
            Return GH_Exposure.hidden
        End Get
    End Property

    Protected Overrides ReadOnly Property Icon As Drawing.Bitmap
        Get
            Return Squid.My.Resources.fontparam
        End Get
    End Property

End Class