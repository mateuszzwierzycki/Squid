Imports Rhino.Geometry
Imports Grasshopper
Imports Grasshopper.Kernel
Imports GH_IO
Imports System.Drawing


Public Class CompOutlineEX
    Inherits GH_Component

    Sub New()
        MyBase.New("OutlineEx", "OutlineEx", "Create a complex outline", "Squid", "Drawing palette")
    End Sub

    Public Overrides ReadOnly Property ComponentGuid As Guid
        Get
            Return New Guid("{34D5A4B4-586A-4580-9332-D7F3E601BD75}")
        End Get
    End Property

    Public Overrides ReadOnly Property Exposure As GH_Exposure
        Get
            Return GH_Exposure.primary
        End Get
    End Property

    Protected Overrides ReadOnly Property Icon As Bitmap
        Get
            Return My.Resources.outlineex
        End Get
    End Property

    Protected Overrides Sub RegisterInputParams(pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
        pManager.AddColourParameter("Color", "C", "Outline color", GH_ParamAccess.item, Color.Black)

        pManager.AddNumberParameter("Width", "W", "Outline width", GH_ParamAccess.item, 1)

        pManager.AddIntegerParameter("LineJoin", "L", "0=Round 1=Miter 2=Bevel", GH_ParamAccess.item, 0)
        pManager.AddIntegerParameter("StartCap", "S", "0=Flat 1=Round 2=Square 3=Triangle", GH_ParamAccess.item, 1)
        pManager.AddIntegerParameter("EndCap", "E", "0=Flat 1=Round 2=Square 3=Triangle", GH_ParamAccess.item, 1)

        pManager.AddTextParameter("Dash", "D", "Dash pattern. Set to 1 for solid line.", GH_ParamAccess.item, "10,10")
        pManager.AddIntegerParameter("DashCap", "Dc", "0=Flat 1=Round 2=Triangle", GH_ParamAccess.item, 1)
    End Sub

    Protected Overrides Sub RegisterOutputParams(pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
        pManager.AddParameter(New ParamSquidOutline)
    End Sub

    Protected Overrides Sub SolveInstance(DA As IGH_DataAccess)

        Dim col As Color = Color.Black
        Dim wid As Double
        Dim lj As Integer
        Dim sc As Integer
        Dim ec As Integer
        Dim das As New String("")
        Dim dc As Integer

        DA.GetData(0, col)
        DA.GetData(1, wid)
        DA.GetData(2, lj)
        DA.GetData(3, sc)
        DA.GetData(4, ec)
        DA.GetData(5, das)
        DA.GetData(6, dc)

        If (lj <> 0 And lj <> 1) And lj <> 2 Then
            Me.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Incorrect LineJoin value.")
            Return
        End If

        If (sc <> 0 And sc <> 1) And (sc <> 2 And sc <> 3) Then
            Me.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Incorrect StartSap value.")
            Return
        End If

        If (ec <> 0 And ec <> 1) And (ec <> 2 And ec <> 3) Then
            Me.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Incorrect EndCap value.")
            Return
        End If

        If (dc <> 0 And dc <> 1) And dc <> 2 Then
            Me.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Incorrect DashCap value.")
            Return
        End If

        Dim str() As String = das.Split(",")
        Dim nlstr As New List(Of Single)

        For Each s As String In str
            Dim res As Single = 0.0
            If Single.TryParse(s, res) Then
                If res > 0 Then
                    nlstr.Add(res)
                Else
                    Me.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Incorrect Dash Pattern")
                    Return
                End If
            Else
                Me.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Incorrect Dash Pattern")
                Return
            End If
        Next

        If wid <= 0 Then wid = 1

        Dim nout As New DrawOutline()

        nout.Width = CSng(wid)
        nout.Pattern = das
        nout.Color = col

        Select Case lj
            Case 0
                nout.LineJoin = Drawing2D.LineJoin.Round
            Case 1
                nout.LineJoin = Drawing2D.LineJoin.Miter
            Case 2
                nout.LineJoin = Drawing2D.LineJoin.Bevel
        End Select

        Select Case sc
            Case 0
                nout.StartCap = Drawing2D.LineCap.Flat
            Case 1
                nout.StartCap = Drawing2D.LineCap.Round
            Case 2
                nout.StartCap = Drawing2D.LineCap.Square
            Case 3
                nout.StartCap = Drawing2D.LineCap.Triangle
        End Select

        Select Case ec
            Case 0
                nout.EndCap = Drawing2D.LineCap.Flat
            Case 1
                nout.EndCap = Drawing2D.LineCap.Round
            Case 2
                nout.EndCap = Drawing2D.LineCap.Square
            Case 3
                nout.EndCap = Drawing2D.LineCap.Triangle
        End Select

        Select Case dc
            Case 0
                nout.DashCap = Drawing2D.DashCap.Flat
            Case 1
                nout.DashCap = Drawing2D.DashCap.Round
            Case 2
                nout.DashCap = Drawing2D.DashCap.Triangle
        End Select


        DA.SetData(0, nout) '.Duplicate)


    End Sub



End Class
