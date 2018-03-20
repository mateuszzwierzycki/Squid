Imports Rhino.Geometry
Imports Grasshopper
Imports Grasshopper.Kernel
Imports GH_IO
Imports System.Drawing
Imports Grasshopper.Kernel.Data

Public Class CompQuickPreview
    Inherits GH_Component

    Sub New()
        MyBase.New("Quick Preview", "QPreview", "Quick bitmap preview", "Squid", "Util")
        DrawEmpty()
    End Sub

    Dim att As AttQuickPreview
    Public Overrides Sub CreateAttributes()
        m_attributes = New AttQuickPreview(Me)
        att = m_attributes
    End Sub

    Public Overrides ReadOnly Property ComponentGuid As Guid
        Get
            Return New Guid("{F8C2F9FC-B840-4A30-977D-BBB8DD173297}")
        End Get
    End Property

    Public Overrides ReadOnly Property Exposure As GH_Exposure
        Get
            Return GH_Exposure.primary
        End Get
    End Property


    Protected Overrides ReadOnly Property Icon As Bitmap
        Get
            Return My.Resources.qprev
        End Get
    End Property

    Protected Overrides Sub RegisterInputParams(pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
        pManager.AddParameter(New ParamSquidBitmap, "Bitmap", "B", "Squid bitmap or file path", GH_ParamAccess.tree)
        pManager.Param(0).Optional = True
    End Sub

    Protected Overrides Sub RegisterOutputParams(pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)

    End Sub

    Dim emptybmp As New Bitmap(200, 200)

    Private Sub DrawEmpty()
        Using g As Graphics = Graphics.FromImage(emptybmp)
            Using b As TextureBrush = New TextureBrush(My.Resources.backgroundSmall)
                g.FillRectangle(b, emptybmp.GetBounds(GraphicsUnit.Pixel))
            End Using
        End Using
    End Sub

    Protected Overrides Sub SolveInstance(DA As IGH_DataAccess)
        Me.Message = ""

        Dim sqi As New GH_Structure(Of DrawBitmap)
        If Not (DA.GetDataTree(0, sqi)) And Me.Params.Input(0).SourceCount > 0 Then
            att.DisplayImage = emptybmp
            Return
        End If

        If sqi.AllData(True).Count < 1 Then
            att.DisplayImage = emptybmp
            Return
        End If

        Dim sqb As DrawBitmap = sqi.AllData(True)(0)

        att.DisplayImage = sqb.Image
        Me.Message = sqb.Image.Width & "x" & sqb.Image.Height & " px"
    End Sub
End Class
