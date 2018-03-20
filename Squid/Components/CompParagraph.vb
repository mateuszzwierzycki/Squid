Imports Rhino.Geometry
Imports Grasshopper
Imports Grasshopper.Kernel
Imports GH_IO
Imports System.Drawing

Public Class CompParagraph
    Inherits GH_Component

    Public Sub New()
        MyBase.New("Paragraph", "Paragraph", "Define paragraph formatting", "Squid", "Drawing palette")
    End Sub

    Public Overrides ReadOnly Property ComponentGuid As Guid
        Get
            Return New Guid("{83694490-EDA2-47E4-AD9B-2BB2E5E7C0AF}")
        End Get
    End Property

    Public Overrides ReadOnly Property Exposure As GH_Exposure
        Get
            Return GH_Exposure.tertiary
        End Get
    End Property

    Protected Overrides ReadOnly Property Icon As Bitmap
        Get
            Return My.Resources.paragraphicon
        End Get
    End Property

    Protected Overrides Sub RegisterInputParams(pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
        pManager.AddIntegerParameter("HAlign", "H", "Horizontal alignment. 0=Left 1=Center 2=Right", GH_ParamAccess.item, 0)
        pManager.AddIntegerParameter("VAlign", "V", "Vertical alignment. 0=Top 1=Center 2=Bottom", GH_ParamAccess.item, 0)
        pManager.AddBooleanParameter("RightLeft", "RL", "Right to left", GH_ParamAccess.item, False)
    End Sub

    Protected Overrides Sub RegisterOutputParams(pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
        pManager.AddParameter(New ParamSquidParagraph)
    End Sub

    Protected Overrides Sub SolveInstance(DA As IGH_DataAccess)
        Dim halg As Integer
        Dim valg As Integer
        Dim rtl As Boolean

        If Not DA.GetData(0, halg) Then Return
        If Not DA.GetData(1, valg) Then Return
        If Not DA.GetData(2, rtl) Then Return

        Dim npar As New DrawParagraph(halg, valg, rtl)

        DA.SetData(0, npar)
    End Sub
End Class
