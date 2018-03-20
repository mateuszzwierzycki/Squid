Imports Rhino.Geometry
Imports Grasshopper
Imports Grasshopper.Kernel
Imports Grasshopper.Kernel.Types
Imports GH_IO
Imports System.Drawing

''' <summary>
''' Holds polyline, fill and outline specifications. Dispose only when the fill is set to the DrawFillTexture.
''' </summary>
''' <remarks></remarks>
Public Class DrawCurveEx

    Private poly As New List(Of Polyline)
    Private fil As DrawFill
    Private otl As DrawOutline
    Private fillmod As Boolean = True

    Sub New()
        poly.Add(New Polyline())
        fill = New DrawFill()
        outline = New DrawOutline()
    End Sub

    Sub New(Other As DrawCurveEx)
        poly.AddRange(Other.Curves)
        Fill = Other.Fill.Duplicate
        Outline = Other.Outline.Duplicate
        FillMode = Other.FillMode
    End Sub

    Sub New(CrvIn As List(Of Polyline), CrvOutline As DrawOutline, CrvFill As DrawFill, Winding As Boolean)
        poly.AddRange(CrvIn)
        Fill = CrvFill.Duplicate
        Outline = CrvOutline.Duplicate
        FillMode = Winding
    End Sub

    Friend Property Outline As DrawOutline
        Set(value As DrawOutline)
            otl = value.Duplicate
        End Set
        Get
            Return otl
        End Get
    End Property

    Friend Property Fill As DrawFill
        Set(value As DrawFill)
            fil = value.Duplicate
        End Set
        Get
            Return fil
        End Get
    End Property


    ''' <summary>
    ''' True = Winding, False = Alternate
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Property FillMode As Boolean
        Set(value As Boolean)
            fillmod = value
        End Set
        Get
            Return fillmod
        End Get
    End Property

    Friend Property Curves As List(Of Polyline)
        Set(value As List(Of Polyline))
            poly.Clear()
            poly.AddRange(value)
        End Set
        Get
            Return poly
        End Get
    End Property

    Public Function Duplicate() As DrawCurveEx
        Return New DrawCurveEx(Me.Curves, Me.Outline, Me.Fill, Me.FillMode)
    End Function

    ''' <summary>
    ''' In case DrawCurve holds TextureFill
    ''' </summary>
    Friend Sub Dispose()
        If fil.FillType = 1 Then
            Dim filcast As DrawFillTexture = fil
            filcast.Texture.Dispose()
        End If
    End Sub

End Class
