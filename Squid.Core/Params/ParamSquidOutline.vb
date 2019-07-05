Imports Rhino
Imports Grasshopper
Imports Grasshopper.Kernel
Imports GH_IO
Imports Grasshopper.Kernel.Types
Imports System.Drawing

Public Class ParamSquidOutline
    Inherits GH_Param(Of DrawOutline)

    Public Sub New()
        MyBase.New(New GH_InstanceDescription("Outline", "O", "Squid outline", "Params", "Geometry"))
    End Sub

    Public Overrides ReadOnly Property ComponentGuid As Guid
        Get
            Return New Guid("{4C9299B6-0067-4B96-9FA5-D65AC5D2D0DA}")
        End Get
    End Property

    Public Overrides ReadOnly Property Exposure As GH_Exposure
        Get
            Return GH_Exposure.hidden
        End Get
    End Property

    Protected Overrides ReadOnly Property Icon As Drawing.Bitmap
        Get
            Return My.Resources.outlineparam
        End Get
    End Property

    Protected Overrides Function PreferredCast(data As Object) As DrawOutline

        If TypeOf data Is GH_Colour Then
            Dim GHC As GH_Colour = data
            Return (New DrawOutline(GHC.Value, 1))
        End If

        If TypeOf data Is GH_Number Then
            Dim GHC As GH_Number = data
            Return (New DrawOutline(Color.Black, GHC.Value))
        End If

        Return Nothing

    End Function

End Class
