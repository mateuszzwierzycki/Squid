Imports System.Drawing
Imports Rhino.Geometry
Imports Grasshopper.Kernel.Types


Public Class DrawFillTexture
    Inherits DrawFill

    Private MyTex As New Bitmap(1, 1)
    Private MyRect As New Rhino.Geometry.Rectangle3d(Plane.WorldXY, 100, 100)
    Private MyWrap As Byte = 0

    Sub New()
        MyBase.FillType = 1
    End Sub

    Public Overrides Function Duplicate() As IGH_Goo
        Return New DrawFillTexture(MyTex, MyRect, MyWrap)
    End Function

    Sub New(Bmp As Bitmap, Rect As Rhino.Geometry.Rectangle3d, Wrap As Drawing2D.WrapMode)
        MyBase.FillType = 1
        MyTex = Bmp.Clone
        MyRect = Rect
        MyWrap = Wrap

    End Sub

    public property Texture As Bitmap
        Get
            Return MyTex
        End Get
        Set(value As Bitmap)
            MyTex = value.Clone
        End Set
    End Property

    public property Rectangle As Rhino.Geometry.Rectangle3d
        Get
            Return MyRect
        End Get
        Set(value As Rhino.Geometry.Rectangle3d)
            MyRect = value
        End Set
    End Property

    public property Wrap As Drawing2D.WrapMode
        Get
            Return MyWrap
        End Get
        Set(value As Drawing2D.WrapMode)
            MyWrap = value
        End Set
    End Property

    ' ''' <summary>
    ' ''' Disposes the internal bitmap, leaving the rest of the job to GC.
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Friend Sub Dispose()
    '    'MyTex.Dispose()
    'End Sub


End Class
