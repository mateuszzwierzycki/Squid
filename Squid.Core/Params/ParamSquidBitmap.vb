Imports Rhino
Imports Grasshopper
Imports Grasshopper.Kernel
Imports GH_IO
Imports Grasshopper.Kernel.Types
Imports System.Drawing
Imports Rhino.Geometry

Public Class ParamSquidBitmap
    Inherits GH_Param(Of DrawBitmap)

    Public Sub New()
        MyBase.New(New GH_InstanceDescription("Bitmap", "B", "Squid bitmap", "Params", "Geometry"))
    End Sub

    'Public Sub New(ChangeGrip As Boolean)
    '    MyBase.New(New GH_InstanceDescription("Bitmap", "B", "Squid bitmap or file path", "Params", "Geometry"))
    'End Sub


    Public Overrides ReadOnly Property ComponentGuid As Guid
        Get
            Return New Guid("{F0EF22EC-7445-4D31-907F-C94CFAFE4395}")
        End Get
    End Property

    Public Overrides ReadOnly Property Exposure As GH_Exposure
        Get
            Return GH_Exposure.hidden
        End Get
    End Property

    Protected Overrides ReadOnly Property Icon As Drawing.Bitmap
        Get
            Return Squid.Core.My.Resources.bitmapparam
        End Get
    End Property

    Protected Overrides Function PreferredCast(data As Object) As DrawBitmap

        If TypeOf data Is GH_String Then
            Dim GHC As GH_String = data
            Dim str As New String(GHC.Value)

            If System.IO.File.Exists(str) Then
                Dim nbmp As New Bitmap(str)
                Return (New DrawBitmap(nbmp))
            End If
        End If

        Return Nothing

    End Function

End Class