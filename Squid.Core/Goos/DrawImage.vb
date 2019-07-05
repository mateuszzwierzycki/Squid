Imports Rhino.Geometry
Imports Grasshopper
Imports Grasshopper.Kernel
Imports Grasshopper.Kernel.Types
Imports GH_IO
Imports System.Drawing
Imports System.Drawing.Drawing2D

''' <summary>
''' Holds bitmap, rectangle and interpolation mode. Disposing affects bitmap only.
''' </summary>
''' <remarks></remarks>
Public Class DrawImage

    Private bmp As New Bitmap(1, 1)
    Private rect As New Rhino.Geometry.Rectangle3d(Plane.WorldXY, 1, 1)
    Private inter As Integer = 0

    ''' <summary>
    ''' Empty constructor disposes the internal bitmap.
    ''' </summary>
    Sub New()
        bmp.Dispose()
    End Sub

    ''' <summary>
    ''' A proper constructor in this case. 
    ''' </summary>
    ''' <param name="B">Bitmap.</param>
    ''' <param name="R">Has to be constrained to Plane.WorldXY plane.</param>
    ''' <param name="I">Important for resizing the image.</param>

    Sub New(B As Bitmap, R As Rectangle3d, I As InterpolationMode)
        bmp = B.Clone
        rect = R
        inter = I
    End Sub

    public property Image As Bitmap
        Get
            Return bmp
        End Get
        Set(value As Bitmap)
            bmp = value.Clone
        End Set
    End Property

    public property Rectangle As Rectangle3d
        Get
            Return rect
        End Get
        Set(value As Rectangle3d)
            rect = value
        End Set
    End Property

    public property Interpolation As Integer
        Get
            Return inter
        End Get
        Set(value As Integer)
            inter = value
        End Set
    End Property

    ''' <summary>
    ''' Disposes the internal bitmap, leaving the rest of the job to GC.
    ''' </summary>
    Friend Sub Dispose()
        bmp.Dispose()
    End Sub

    Public Function Duplicate() As DrawImage
        Dim n As New DrawImage(bmp.Clone, rect, inter)
        Return n
    End Function

End Class

