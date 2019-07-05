Imports Rhino.Geometry
Imports Grasshopper
Imports Grasshopper.Kernel
Imports Grasshopper.Kernel.Types
Imports GH_IO
Imports System.Drawing
Imports System.Drawing.Drawing2D


Public Class DrawParagraph
    Inherits Grasshopper.Kernel.Types.GH_Goo(Of DrawParagraph)

    Private halg As Integer = 0
    Private valg As Integer = 0
    Private rtl As Boolean = False

    Public Sub New()

    End Sub

    Public Sub New(HAlign As Integer, VAlign As Integer, RightToLeft As Boolean)
        halg = HAlign
        valg = VAlign
        rtl = RightToLeft
    End Sub

    public property HAlign As Integer
        Get
            Return halg
        End Get
        Set(value As Integer)
            halg = value
        End Set
    End Property

    public property VAlign As Integer
        Get
            Return valg
        End Get
        Set(value As Integer)
            valg = value
        End Set
    End Property

    public property RightToLeft As Boolean
        Get
            Return rtl
        End Get
        Set(value As Boolean)
            rtl = value
        End Set
    End Property


    Public Overrides Function Duplicate() As Grasshopper.Kernel.Types.IGH_Goo
        Return New DrawParagraph(Me.HAlign, Me.VAlign, Me.RightToLeft)
    End Function

    Public Overrides ReadOnly Property IsValid As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return "SquidParagraph"
    End Function

    Public Overrides ReadOnly Property TypeDescription As String
        Get
            Return "SquidParagraph"
        End Get
    End Property

    Public Overrides ReadOnly Property TypeName As String
        Get
            Return "SquidParagraph"
        End Get
    End Property

End Class
