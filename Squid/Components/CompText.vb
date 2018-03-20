Imports Rhino.Geometry
Imports Grasshopper
Imports Grasshopper.Kernel
Imports GH_IO
Imports System.Drawing
Imports System.Windows.Forms

Public Class CompText
    Inherits GH_Component

    Public Sub New()
        MyBase.New("Text", "Text", "Draw text", "Squid", "Instructions")
    End Sub

    Public Overrides ReadOnly Property ComponentGuid As Guid
        Get
            Return New Guid("{8B5E230B-EB20-4134-BD2D-829810DE93FD}")
        End Get
    End Property

    Protected Overrides Sub RegisterInputParams(pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
        pManager.AddRectangleParameter("Rectangle", "L", "Layout rectangle", GH_ParamAccess.item)
        pManager.AddTextParameter("Text", "T", "Text to draw", GH_ParamAccess.item, "Hello world!")

        pManager.AddParameter(New ParamSquidFont)
        pManager.Param(2).Optional = True
        pManager.AddParameter(New ParamSquidParagraph)
        pManager.Param(3).Optional = True
        pManager.AddParameter(New ParamSquidOutline)
        pManager.Param(4).Optional = True
        pManager.AddColourParameter("Color", "C", "Text fill color", GH_ParamAccess.item, Color.Black)
        pManager.Param(5).Optional = True

    End Sub

    Protected Overrides ReadOnly Property Icon As Bitmap
        Get
            Return My.Resources.text
        End Get
    End Property

    Protected Overrides Sub RegisterOutputParams(pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
        pManager.AddParameter(New ParamSquidInstr)
    End Sub

    Protected Overrides Sub SolveInstance(DA As IGH_DataAccess)

        Dim rect As Rectangle3d
        Dim str As New String("")
        Dim fnt As DrawFont = Nothing
        Dim par As DrawParagraph = Nothing
        Dim outl As DrawOutline = Nothing

        Dim col As Color = Color.Black
        Dim fil As DrawFillColor = Nothing 'drawfill, but doesnt support... 

        If Not (DA.GetData(0, rect)) Then Return

        If Not (DA.GetData(1, str)) Then Return

        If Not (DA.GetData(2, fnt)) And Me.Params.Input(2).SourceCount > 0 Then
            Me.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid font")
            Return
        End If

        If Not (DA.GetData(3, par)) And Me.Params.Input(3).SourceCount > 0 Then
            Me.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid paragraph formatting")
            Return
        End If

        If Not (DA.GetData(4, outl)) And Me.Params.Input(4).SourceCount > 0 Then
            Me.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid outline")
            Return
        End If

        If Not (DA.GetData(5, col)) And Me.Params.Input(5).SourceCount > 0 Then
            Me.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid color")
            Return
        End If

        fil = New DrawFillColor(col)

        If fnt Is Nothing Then fnt = New DrawFont("Arial", 0, 25)
        If par Is Nothing Then par = New DrawParagraph(0, 0, False)
        If outl Is Nothing Then outl = New DrawOutline(Color.Transparent, 1)
        If fil Is Nothing Then fil = New DrawFillColor(Color.Black)

        Dim si As New InstrText(str, fnt, par, fil, outl, rect)

        DA.SetData(0, si)

    End Sub


End Class