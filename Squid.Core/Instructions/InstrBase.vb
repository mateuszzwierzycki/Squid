Imports Rhino
Imports Grasshopper
Imports Grasshopper.Kernel
Imports GH_IO
Imports System.Drawing

Public Class InstrBase
    Inherits Grasshopper.Kernel.Types.GH_Goo(Of InstrBase)

    Private desc As String
    Private flip As Boolean = False
    Private over As Boolean = False

    ''' <summary>
    ''' Determines instruction type (DrawCurve, DrawImage, SaveFile, Empty)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    public property InstructionType As String
        Get
            Return desc
        End Get
        Set(value As String)
            desc = value.Clone
        End Set
    End Property

    public property FlipOrder As Boolean
        Get
            Return flip
        End Get
        Set(value As Boolean)
            flip = value
        End Set
    End Property

    public property OverWrite As Boolean
        Get
            Return over
        End Get
        Set(value As Boolean)
            over = value
        End Set
    End Property

    Public Overrides Function Duplicate() As Grasshopper.Kernel.Types.IGH_Goo
        Return Me.MemberwiseClone
    End Function

    Public Overrides ReadOnly Property IsValid As Boolean
        Get
            If desc = String.Empty Then Return False
            Return True
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return ReturnType()
    End Function

    Public Overrides ReadOnly Property TypeDescription As String
        Get
            Return ReturnType()
        End Get
    End Property

    Public Overrides ReadOnly Property TypeName As String
        Get
            Return ReturnType()
        End Get
    End Property

    Private Function ReturnType() As String
        Select Case desc
            Case "Empty"
                Return "SquidInstr.Empty"
            Case "Save"
                Return "SquidInstr.Save"
            Case "Draw"
                Return "SquidInstr.Draw"
            Case "DrawEx"
                Return "SquidInstr.DrawEx"
            Case "Clear"
                Return "SquidInstr.Clear"
            Case "Image"
                Return "SquidInstr.Image"
            Case "Text"
                Return "SquidInstr.Text"
            Case "Mask"
                Return "SquidInstr.Mask"
            Case "Update"
                Return "SquidInstr.Update"
        End Select
        Return "None"
    End Function


End Class
