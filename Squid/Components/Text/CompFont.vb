Imports Rhino.Geometry
Imports Grasshopper
Imports Grasshopper.Kernel
Imports GH_IO
Imports System.Drawing

Public Class CompFont
    Inherits GH_Component

    Public Sub New()
        MyBase.New("Font", "Font", "Create font", "Squid", "Drawing palette")
    End Sub

    Public Overrides ReadOnly Property ComponentGuid As Guid
        Get
            Return New Guid("{83C013DD-5E25-48AC-B1A7-68874C883FBE}")
        End Get
    End Property

    Public Overrides ReadOnly Property Exposure As GH_Exposure
        Get
            Return GH_Exposure.tertiary
        End Get
    End Property

    Protected Overrides ReadOnly Property Icon As Bitmap
        Get
            Return My.Resources.fonticon2
        End Get
    End Property

    Protected Overrides Sub RegisterInputParams(pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
        pManager.AddTextParameter("Family", "F", "Font family name", GH_ParamAccess.item, "Arial")
        pManager.AddNumberParameter("Size", "H", "Font size", GH_ParamAccess.item, 10)
        pManager.AddBooleanParameter("Bold", "B", "Bold", GH_ParamAccess.item, False)
        pManager.AddBooleanParameter("Italic", "I", "Italic", GH_ParamAccess.item, False)
        pManager.AddBooleanParameter("Underline", "U", "Underline", GH_ParamAccess.item, False)
    End Sub

    Protected Overrides Sub RegisterOutputParams(pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
        pManager.AddParameter(New ParamSquidFont)
    End Sub

    Protected Overrides Sub SolveInstance(DA As IGH_DataAccess)
        Dim fam As New String("")
        Dim siz As Double

        Dim bol As Boolean
        Dim ita As Boolean
        Dim und As Boolean

        If Not DA.GetData(0, fam) Then Return
        If Not DA.GetData(1, siz) Then Return

        If Not DA.GetData(2, bol) Then Return
        If Not DA.GetData(3, ita) Then Return
        If Not DA.GetData(4, und) Then Return

        Dim sty As Integer = 0

        If bol Then sty += FontStyle.Bold
        If ita Then sty += FontStyle.Italic
        If und Then sty += FontStyle.Underline

        Dim nfont As New DrawFont(fam, sty, CSng(siz))

        DA.SetData(0, nfont)
    End Sub
End Class
