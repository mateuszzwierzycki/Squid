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
Public Class DrawCurve

    Private poly As Polyline
    Private fil As DrawFill
    Private otl As DrawOutline

    Sub New()
        poly = New Polyline()
        fill = New DrawFill()
        outline = New DrawOutline()
    End Sub

    Sub New(Other As DrawCurve)
        poly = New Polyline(Other.Curve.ToArray)
        Fill = Other.Fill.Duplicate
        Outline = Other.Outline.Duplicate
    End Sub

    Sub New(CrvIn As Polyline, CrvOutline As DrawOutline, CrvFill As DrawFill)
        poly = New Polyline(CrvIn.ToArray)
        Fill = CrvFill.Duplicate
        Outline = CrvOutline.Duplicate
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

    Friend Property Curve As Polyline
        Set(value As Polyline)
            poly = New Polyline(value.ToArray)
        End Set
        Get
            Return poly
        End Get
    End Property

    Public Function Duplicate() As DrawCurve
        Return New DrawCurve(Me.Curve, Me.Outline, Me.Fill)
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
