Imports Rhino.Geometry
Imports Grasshopper
Imports Grasshopper.Kernel
Imports Grasshopper.Kernel.Types
Imports GH_IO
Imports System.Drawing
Imports System.Drawing.Drawing2D

Public Class DrawOutline
    Inherits Grasshopper.Kernel.Types.GH_Goo(Of DrawOutline)

    Private mycol As Color = Drawing.Color.Transparent
    Private mywid As Single = 1

    Private mylj As Byte = 2

    Private myec As Byte = 2
    Private mysc As Byte = 2

    Private mydc As Byte = 2
    Private mydas As New String("1")

    Sub New()

    End Sub

    Sub New(Col As Color, Wid As Double)
        mycol = Col
        mywid = Wid
    End Sub

    Sub New(Other As DrawOutline)
        mycol = Other.Color
        mywid = Other.Width
        mylj = Other.LineJoin
        myec = Other.EndCap
        mysc = Other.StartCap
        mydc = Other.DashCap
        mydas = Other.Pattern.Clone
    End Sub

    public property Color As Color
        Set(value As Color)
            mycol = value
        End Set
        Get
            Return mycol
        End Get
    End Property

    public property Width As Single
        Set(value As Single)
            mywid = value
        End Set
        Get
            Return mywid
        End Get
    End Property

    public property LineJoin As Byte
        Set(value As Byte)
            mylj = value
        End Set
        Get
            Return mylj
        End Get
    End Property

    public property EndCap As Byte
        Set(value As Byte)
            myec = value
        End Set
        Get
            Return myec
        End Get
    End Property

    public property StartCap As Byte
        Set(value As Byte)
            mysc = value
        End Set
        Get
            Return mysc
        End Get
    End Property

    public property DashCap As Byte
        Set(value As Byte)
            mydc = value
        End Set
        Get
            Return mydc
        End Get
    End Property

    public property Pattern As String
        Set(value As String)
            mydas = value.Clone
        End Set
        Get
            Return mydas
        End Get
    End Property

    Public Overrides Function Duplicate() As IGH_Goo
        Return New DrawOutline(Me)
    End Function

    Public Overrides ReadOnly Property IsValid As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return "Squid.Outline"
    End Function

    Public Overrides ReadOnly Property TypeDescription As String
        Get
            Return "Squid.Outline"
        End Get
    End Property

    Public Overrides ReadOnly Property TypeName As String
        Get
            Return "Squid.Outline"
        End Get
    End Property


End Class
