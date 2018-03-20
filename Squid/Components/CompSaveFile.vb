Imports Rhino.Geometry
Imports Grasshopper
Imports Grasshopper.Kernel
Imports GH_IO
Imports System.Drawing

Public Class CompSaveFile
    Inherits GH_Component

    Public Sub New()
        MyBase.New("Save", "Save", "Create a Save File instruction", "Squid", "Instructions")
    End Sub

    Public Overrides ReadOnly Property ComponentGuid As Guid
        Get
            Return New Guid("{CEA43670-61E6-486E-9FAA-0EA59F99B331}")
        End Get
    End Property

    Protected Overrides ReadOnly Property Icon As Bitmap
        Get
            Return My.Resources.save2
        End Get
    End Property

    Protected Overrides Sub RegisterInputParams(pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
        pManager.AddTextParameter("Folder", "D", "Folder", GH_ParamAccess.item, Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) & "\SquidOutput")
        pManager.AddTextParameter("Filename", "F", "Filename", GH_ParamAccess.item, "SquidPrint")
        pManager.AddBooleanParameter("Tiff", "T", "Save as CMYK Tiff file." & vbCrLf & "Creates additional Alpha channel file.", GH_ParamAccess.item, False)
    End Sub

    Protected Overrides Sub RegisterOutputParams(pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
        pManager.AddParameter(New ParamSquidInstr)
        pManager.AddTextParameter("Path", "F", "Complete file path", GH_ParamAccess.item)
    End Sub

    Protected Overrides Sub SolveInstance(DA As IGH_DataAccess)
        Dim fil As New String("")
        Dim fol As New String("")
        Dim tif As Boolean = False

        If Not (DA.GetData(0, fol)) Then Return
        If Not (DA.GetData(1, fil)) Then Return
        If Not (DA.GetData(2, tif)) Then Return


        Dim ninstr As New InstrSave(fil, fol, tif)

        DA.SetData(0, ninstr)

        Select Case tif
            Case True
                DA.SetData(1, fol & "\" & fil & ".tif")
            Case False
                DA.SetData(1, fol & "\" & fil & ".png")
        End Select


    End Sub
End Class
