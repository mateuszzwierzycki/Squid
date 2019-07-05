Imports System.Drawing
Imports Rhino.Geometry

Public Class DrawFillLGradient
    Inherits DrawFill

    Private MyColors As New List(Of Color)
    Private MyParams As New List(Of Double)
    Private MyLine As New Line(0, 0, 0, 1, 0, 0)

    Sub New()
        MyBase.FillType = 2
        MyColors.Add(Color.White)
        MyColors.Add(Color.Black)
        MyParams.Add(0)
        MyParams.Add(1)
        MyLine = New Line(0, 0, 0, 100, 0, 0)
    End Sub

    Sub New(Colors As List(Of Color), Params As List(Of Double), L As Line)
        MyBase.FillType = 2
        MyColors.AddRange(Colors)
        MyParams.AddRange(Params)
        MyLine = L
    End Sub

    Public Property Colors As List(Of Color)
        Get
            Return MyColors
        End Get
        Set(value As List(Of Color))
            MyColors.Clear()
            MyColors.AddRange(value)
        End Set
    End Property

    Public Property Params As List(Of Double)
        Get
            Return MyParams
        End Get
        Set(value As List(Of Double))
            MyParams.Clear()
            MyParams.AddRange(value)
        End Set
    End Property

    Public Property GradientLine As Line
        Get
            Return MyLine
        End Get
        Set(value As Line)
            MyLine = value
        End Set
    End Property

End Class
