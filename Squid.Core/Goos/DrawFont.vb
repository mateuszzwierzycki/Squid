Imports System.Drawing
Imports Rhino.Geometry

Public Class DrawFont
    Inherits Grasshopper.Kernel.Types.GH_Goo(Of DrawFont)

    Private fon As New String("Arial")
    Private sty As Integer = 0
    Private siz As Single = 0

    Public Sub New()

    End Sub

    Public Sub New(Family As String, FontStyle As Integer, FontSize As Single)
        fon = String.Empty
        fon += Family

        sty = FontStyle
        siz = FontSize
    End Sub

    public property FontFamily As String
        Get
            Return fon
        End Get
        Set(value As String)
            fon = String.Empty
            fon += value
        End Set
    End Property

    public property FontStyle As Integer
        Get
            Return sty
        End Get
        Set(value As Integer)
            fon = String.Empty
            sty = value
        End Set
    End Property

    public property FontSize As Single
        Get
            Return siz
        End Get
        Set(value As Single)
            siz = value
        End Set
    End Property

    Public Overrides Function Duplicate() As Grasshopper.Kernel.Types.IGH_Goo
        Return New DrawFont(Me.FontFamily, Me.fontstyle, Me.fontsize)
    End Function

    Public Overrides ReadOnly Property IsValid As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return "SquidFont"
    End Function

    Public Overrides ReadOnly Property TypeDescription As String
        Get
            Return "SquidFont"
        End Get
    End Property

    Public Overrides ReadOnly Property TypeName As String
        Get
            Return "SquidFont"
        End Get
    End Property

End Class
