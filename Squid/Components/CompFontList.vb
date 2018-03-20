Imports Rhino.Geometry
Imports Grasshopper
Imports Grasshopper.Kernel
Imports GH_IO
Imports System.Drawing
Imports System.Drawing.Text

Public Class CompFontList
    Inherits GH_Component

    Public Sub New()
        MyBase.New("Font List", "FontList", "Lists all installed fonts", "Squid", "Util")
    End Sub

    Public Overrides ReadOnly Property ComponentGuid As Guid
        Get
            Return New Guid("{B41FE128-B6B4-46EC-96F0-BE6795E41F3B}")
        End Get
    End Property

    Protected Overrides Sub RegisterInputParams(pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)

    End Sub

    Protected Overrides ReadOnly Property Icon As Bitmap
        Get
            Return My.Resources.listfont
        End Get
    End Property

    Public Overrides ReadOnly Property Exposure As GH_Exposure
        Get
            Return GH_Exposure.secondary
        End Get
    End Property

    Protected Overrides Sub RegisterOutputParams(pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
        pManager.AddTextParameter("Families", "F", "List of all installed fonts", GH_ParamAccess.list)
    End Sub

    Protected Overrides Sub SolveInstance(DA As IGH_DataAccess)

        Dim str As New List(Of String)

        Dim fontFamilies As New List(Of String)

        Dim installedFontCollection As New InstalledFontCollection()

        For Each f As FontFamily In installedFontCollection.Families

            fontFamilies.Add(f.Name)

        Next

        DA.SetDataList(0, fontFamilies)

        installedFontCollection.Dispose()

    End Sub
End Class
