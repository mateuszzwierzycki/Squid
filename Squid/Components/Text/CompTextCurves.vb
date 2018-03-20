Imports Rhino.Geometry
Imports Grasshopper
Imports Grasshopper.Kernel
Imports GH_IO
Imports System.Drawing
Imports System.Windows.Forms

Public Class CompTextCurves
    Inherits GH_Component

    Public Sub New()
        MyBase.New("Text Curves", "TextC", "Text to curves." & vbCrLf & "Note: Rhino 4 doesn't support paragraph formatting." & vbCrLf & "Doesn't support underlined text.", "Squid", "Util")
    End Sub

    Public Overrides ReadOnly Property ComponentGuid As Guid
        Get
            Return New Guid("{71BF8474-AD52-4344-BDB4-EF809963779E}")
        End Get
    End Property

    Public Overrides ReadOnly Property Exposure As GH_Exposure
        Get
            Return GH_Exposure.secondary
        End Get
    End Property

    Protected Overrides Sub RegisterInputParams(pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
        pManager.AddPlaneParameter("Plane", "P", "Text plane", GH_ParamAccess.item, Plane.WorldXY)
        pManager.AddTextParameter("Text", "T", "Text", GH_ParamAccess.item, "Hello world!")

        pManager.AddParameter(New ParamSquidFont)
        pManager.Param(2).Optional = True
        pManager.AddParameter(New ParamSquidParagraph)
        pManager.Param(3).Optional = True

    End Sub

    Protected Overrides ReadOnly Property Icon As Bitmap
        Get
            Return My.Resources.textcurves
        End Get
    End Property

    Protected Overrides Sub RegisterOutputParams(pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
        pManager.AddCurveParameter("Curves", "C", "Curves", GH_ParamAccess.list)
    End Sub

    Protected Overrides Sub SolveInstance(DA As IGH_DataAccess)

        Dim plan As New Plane(Plane.WorldXY)
        Dim str As New String("")
        Dim fnt As DrawFont = Nothing
        Dim par As DrawParagraph = Nothing

        If Not (DA.GetData(0, plan)) Then Return

        If Not (DA.GetData(1, str)) Then Return

        If Not (DA.GetData(2, fnt)) And Me.Params.Input(2).SourceCount > 0 Then
            Me.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid font")
            Return
        End If

        If Not (DA.GetData(3, par)) And Me.Params.Input(3).SourceCount > 0 Then
            Me.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid paragraph formatting")
            Return
        End If


        If fnt Is Nothing Then fnt = New DrawFont("Arial", 0, 25)
        If par Is Nothing Then par = New DrawParagraph(0, 0, False)
     
        Dim nt As New Rhino.Geometry.TextEntity

        nt.Text = str
        nt.TextHeight = fnt.FontSize
        nt.Plane = plan

        Dim bol As Boolean = False
        Dim ita As Boolean = False

        Select Case fnt.FontStyle
            Case 1, 5
                bol = True
            Case 2, 6
                ita = True
            Case 3, 7
                bol = True
                ita = True
        End Select

        nt.FontIndex = Rhino.RhinoDoc.ActiveDoc.Fonts.FindOrCreate(fnt.FontFamily, bol, ita)

        Dim align As Integer = 0

        Select Case par.HAlign
            Case 0
                align += 1
            Case 1
                align += 2
            Case 2
                align += 4
        End Select

        Select Case par.VAlign
            Case 0
                align += 262144
            Case 1
                align += 131072
            Case 2
                align += 65536
        End Select

        nt.Justification = align

        Dim crvs As New List(Of Curve)(nt.Explode)

        Dim outcrv As New List(Of Curve)(Curve.JoinCurves(crvs, Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance, False))

        DA.SetDataList(0, outcrv)

    End Sub






End Class
