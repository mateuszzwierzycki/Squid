Imports System.Drawing
Imports Rhino.Geometry

Public Class DrawFillColor
    Inherits DrawFill

    Private MyCol As Color = Color.Transparent

    Public Sub New()
        MyBase.FillType = 0
        MyCol = Color.Transparent
    End Sub

    Public Sub New(FillCol As Color)
        MyBase.FillType = 0
        MyCol = FillCol
    End Sub

    public property FillColor As Color
        Get
            Return MyCol
        End Get
        Set(value As Color)
            MyCol = value
        End Set
    End Property

End Class
