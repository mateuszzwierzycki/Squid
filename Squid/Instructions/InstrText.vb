Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports Rhino.Geometry
Imports System.Windows.Forms

Public Class InstrText

    Inherits InstrBase

    Private str As New String("")

    Private fnt As DrawFont
    Private otl As DrawOutline
    Private fil As DrawFill
    Private par As DrawParagraph

    Private loc As New Rectangle3d(Plane.WorldXY, 100, 100)

    Sub New()
        MyBase.InstructionType = "Text"
    End Sub

    Sub New(Text As String, TextFont As DrawFont, TextParagraph As DrawParagraph, TextFill As DrawFill, TextOutline As DrawOutline, LayoutRect As Rectangle3d)
        MyBase.InstructionType = "Text"

        str = String.Empty
        str += Text

        fnt = TextFont.Duplicate
        otl = TextOutline.Duplicate
        fil = TextFill.Duplicate
        par = TextParagraph.Duplicate

        loc = LayoutRect

    End Sub

    Friend Property Text As String
        Get
            Return str
        End Get
        Set(value As String)
            str = value.Clone
        End Set
    End Property

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

    Friend Property Font As DrawFont
        Set(value As DrawFont)
            fnt = value.Duplicate
        End Set
        Get
            Return fnt
        End Get
    End Property

    Friend Property Paragraph As DrawParagraph
        Set(value As DrawParagraph)
            par = value.Duplicate
        End Set
        Get
            Return par
        End Get
    End Property

    Friend Property LayoutRectangle As Rectangle3d
        Set(value As Rectangle3d)
            loc = value
        End Set
        Get
            Return loc
        End Get
    End Property

End Class