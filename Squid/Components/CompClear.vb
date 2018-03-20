Imports Rhino.Geometry
Imports Grasshopper
Imports Grasshopper.Kernel
Imports GH_IO
Imports System.Drawing

Public Class CompClear
    Inherits GH_Component

    Public Sub New()
        MyBase.New("Clear", "Clear", "Fill the picture using one color", "Squid", "Instructions")
    End Sub

    Public Overrides ReadOnly Property ComponentGuid As Guid
        Get
            Return New Guid("{FD1BD15A-3343-46F4-A493-0CE755EC4B35}")
        End Get
    End Property

    Protected Overrides ReadOnly Property Icon As Bitmap
        Get
            Return My.Resources.clear
        End Get
    End Property

    Protected Overrides Sub RegisterInputParams(pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
        pManager.AddColourParameter("Color", "C", "Clear color", GH_ParamAccess.item, Color.White)
    End Sub

    Protected Overrides Sub RegisterOutputParams(pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
        pManager.AddParameter(New ParamSquidInstr)
    End Sub

    Protected Overrides Sub SolveInstance(DA As IGH_DataAccess)
        Dim col As Color = Color.White

        If Not DA.GetData(0, col) Then Return

        Dim si As New InstrClear(col)

        DA.SetData(0, si)
    End Sub
End Class
