Imports Rhino
Imports Rhino.Geometry
Imports Grasshopper
Imports Grasshopper.Kernel
Imports Grasshopper.Kernel.Types
Imports GH_IO
Imports System.Drawing

Public Class ParamSquidFill
    Inherits GH_Param(Of DrawFill)

    Public Sub New()
        MyBase.New(New GH_InstanceDescription("Fill", "F", "Squid fill", "Params", "Geometry"))
    End Sub

    Public Overrides ReadOnly Property ComponentGuid As Guid
        Get
            Return New Guid("{445C027A-FE26-4BA6-8FBA-7A027427B537}")
        End Get
    End Property

    Public Overrides ReadOnly Property Exposure As GH_Exposure
        Get
            Return GH_Exposure.hidden
        End Get
    End Property

    Protected Overrides ReadOnly Property Icon As Drawing.Bitmap
        Get
            Return My.Resources.fillparam
        End Get
    End Property

    Protected Overrides Function PreferredCast(data As Object) As DrawFill

        If TypeOf data Is GH_Colour Then
            Dim GHC As GH_Colour = data
            Return (New DrawFillColor(GHC.Value))
        End If

        If TypeOf data Is DrawBitmap Then
            Dim GHC As DrawBitmap = data
            Return (New DrawFillTexture(GHC.Image, New Rectangle3d(Plane.WorldXY, GHC.Image.Width, GHC.Image.Height), Drawing.Drawing2D.WrapMode.Tile))
        End If

        If TypeOf data Is GH_String Then
            Dim GHC As GH_String = data
            Dim str As New String(GHC.Value)

            If System.IO.File.Exists(str) Then
                Dim nbmp As New Bitmap(str)
                Return (New DrawFillTexture(nbmp, New Rectangle3d(Plane.WorldXY, nbmp.Width, nbmp.Height), Drawing.Drawing2D.WrapMode.Tile))
            End If
        End If

        Return Nothing

    End Function

End Class
