Imports System.Drawing
Imports Rhino.Geometry

Public Class DrawBitmap
    Inherits Grasshopper.Kernel.Types.GH_Goo(Of DrawBitmap)

    Private bmp As New Bitmap(10, 10)

    Public Sub New()

    End Sub

    Public Sub New(Image As Bitmap)
        bmp = Image.Clone
    End Sub

    public property Image As Bitmap
        Get
            Return bmp
        End Get
        Set(value As Bitmap)
            bmp = value.Clone
        End Set
    End Property

    Public Overrides Function Duplicate() As Grasshopper.Kernel.Types.IGH_Goo
        Return New DrawBitmap(bmp)
    End Function

    Public Overrides ReadOnly Property IsValid As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return "SquidBitmap"
    End Function

    Public Overrides ReadOnly Property TypeDescription As String
        Get
            Return "SquidBitmap"
        End Get
    End Property

    Public Overrides ReadOnly Property TypeName As String
        Get
            Return "SquidBitmap"
        End Get
    End Property

    Friend Sub Dispose()
        bmp.Dispose()
    End Sub

End Class
