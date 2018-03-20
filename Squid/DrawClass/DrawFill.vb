Imports Rhino.Geometry
Imports Grasshopper
Imports Grasshopper.Kernel
Imports Grasshopper.Kernel.Types
Imports GH_IO
Imports System.Drawing
Imports System.Drawing.Drawing2D

Public Class DrawFill
    Inherits Grasshopper.Kernel.Types.GH_Goo(Of DrawFill)

    Private MyType As Byte '0 = color 1=texture 2=linear gradient 3=radial gradient 4=path gradient 255=empty

    ''' <summary>
    ''' 0 =Color 1=Texture 2=Linear gradient 3=Radial gradient 255=Empty
    ''' </summary>
    ''' 
    Friend Property FillType As Byte
        Get
            Return MyType
        End Get
        Set(value As Byte)
            MyType = value
        End Set
    End Property

    Public Overrides Function Duplicate() As IGH_Goo
        Return Me.MemberwiseClone
    End Function

    Public Overrides ReadOnly Property IsValid As Boolean
        Get
            If ResolveMyName() <> "SquidFill.Invalid" Then Return True
            Return False
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return ResolveMyName()
    End Function

    Public Overrides ReadOnly Property TypeDescription As String
        Get
            Return ResolveMyName()
        End Get
    End Property

    Public Overrides ReadOnly Property TypeName As String
        Get
            Return ResolveMyName()
        End Get
    End Property

    Private Function ResolveMyName() As String
        Select Case MyType
            Case 0
                Return "SquidFill.Solid"
            Case 1
                Return "SquidFill.Texture"
            Case 2
                Return "SquidFill.LinearGradient"
            Case 3
                Return "SquidFill.RadialGradient"
            Case 4
                Return "SquidFill.PathGradient"
            Case 255
                Return "SquidFill.Empty"
        End Select
        Return "SquidFill.Invalid"
    End Function

End Class
