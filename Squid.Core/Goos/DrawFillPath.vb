Imports System.Drawing
Imports Rhino.Geometry

Public Class DrawFillPath
    Inherits DrawFill

    Private MyColors As New List(Of Color)
    Private MyParams As New List(Of Double)
    Private MyPoints As New List(Of Point3d)
    Private MyCenter As New Point3d()
    Private ChangedCenter As Boolean = False

    Sub New()
        MyBase.FillType = 4
        MyColors.Add(Color.White)
        MyColors.Add(Color.Black)
        MyParams.Add(0)
        MyParams.Add(1)
        MyPoints.Add(New Point3d(0, 0, 0))
        MyPoints.Add(New Point3d(100, 0, 0))
        MyPoints.Add(New Point3d(100, 100, 0))
        MyPoints.Add(New Point3d(0, 100, 0))
    End Sub

    Sub New(Colors As List(Of Color), Params As List(Of Double), Pts As List(Of Point3d))
        MyBase.FillType = 4
        MyColors.AddRange(Colors)
        MyParams.AddRange(Params)
        MyPoints.AddRange(Pts)
    End Sub

    Sub New(Colors As List(Of Color), Params As List(Of Double), Pts As List(Of Point3d), Cnt As Point3d)
        MyBase.FillType = 4
        MyColors.AddRange(Colors)
        MyParams.AddRange(Params)
        MyPoints.AddRange(Pts)
        MyCenter = Cnt
        ChangedCenter = True
    End Sub

    public property Colors As List(Of Color)
        Get
            Return MyColors
        End Get
        Set(value As List(Of Color))
            MyColors.Clear()
            MyColors.AddRange(value)
        End Set
    End Property

    public property Params As List(Of Double)
        Get
            Return MyParams
        End Get
        Set(value As List(Of Double))
            MyParams.Clear()
            MyParams.AddRange(value)
        End Set
    End Property

    public property GradientPoints As List(Of Point3d)
        Get
            Return MyPoints
        End Get
        Set(value As List(Of Point3d))
            MyPoints.Clear()
            MyPoints.AddRange(value)
        End Set
    End Property

    public property Center As Point3d
        Get
            Return MyCenter
        End Get
        Set(value As Point3d)
            MyCenter = value
        End Set
    End Property

    public property IsCenterChanged As Boolean
        Get
            Return ChangedCenter
        End Get
        Set(value As Boolean)
            ChangedCenter = value
        End Set
    End Property

End Class
