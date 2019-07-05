Imports Rhino
Imports Grasshopper
Imports Grasshopper.Kernel
Imports GH_IO

Public Class ParamSquidParagraph
    Inherits GH_Param(Of DrawParagraph)

    Public Sub New()
        MyBase.New(New GH_InstanceDescription("Paragraph", "P", "Squid paragraph formatting", "Params", "Geometry"))
    End Sub

    Public Overrides ReadOnly Property ComponentGuid As Guid
        Get
            Return New Guid("{82105459-4DDB-4A68-8286-011AF81A0A40}")
        End Get
    End Property

    Public Overrides ReadOnly Property Exposure As GH_Exposure
        Get
            Return GH_Exposure.hidden
        End Get
    End Property

    Protected Overrides ReadOnly Property Icon As Drawing.Bitmap
        Get
            Return My.Resources.paragraphparam
        End Get
    End Property

End Class
