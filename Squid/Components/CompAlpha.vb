
Imports Rhino.Geometry
Imports Grasshopper
Imports Grasshopper.Kernel
Imports GH_IO
Imports System.Drawing
Imports System.Drawing.Drawing2D

Public Class CompAlpha

    Inherits GH_Component

    Sub New()
        MyBase.New("Alpha", "Alpha", "Alters the alpha channel of the underlying image." & vbCrLf & "White=opaque, black=transparent.", "Squid", "Instructions")
    End Sub

    Public Overrides ReadOnly Property ComponentGuid As Guid
        Get
            Return New Guid("{32614E74-6357-4D81-935C-5B07C22B219E}")
        End Get
    End Property

    Protected Overrides ReadOnly Property Icon As Bitmap
        Get
            Return My.Resources.alpha
        End Get
    End Property

    Protected Overrides Sub RegisterInputParams(pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
        pManager.AddParameter(New ParamSquidBitmap)
        pManager.AddRectangleParameter("Bounds", "R", "Mask image bounds", GH_ParamAccess.item, New Rectangle3d(Plane.WorldXY, 100, 100))
        pManager.AddIntegerParameter("Interpolate", "I", InterpolationModes, GH_ParamAccess.item, 0)
    End Sub

    Protected Overrides Sub RegisterOutputParams(pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
        pManager.AddParameter(New ParamSquidInstr)
    End Sub

    Protected Overrides Sub SolveInstance(DA As IGH_DataAccess)
        Dim sqb As New DrawBitmap
        Dim rect As New Rectangle3d
        Dim inter As Integer = 0

        If Not DA.GetData(0, sqb) Then Return
        If Not DA.GetData(1, rect) Then Return
        If Not DA.GetData(2, inter) Then Return

        If Not rect.ToNurbsCurve.IsInPlane(Plane.WorldXY, Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance) Then
            AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Rectangle has to lie on the WorldXY plane.")
            Return
        End If

        DA.SetData(0, New InstrMask(New DrawMask(sqb.Image, rect, inter)))

    End Sub

    Function InterpolationModes() As String

        Dim nstr As New String("Interpolation mode :")
        nstr += vbCrLf
        nstr += "0 = Bilinear (Default)" & vbCrLf
        nstr += "1 = Bicubic" & vbCrLf
        nstr += "2 = HighQualityBilinear" & vbCrLf
        nstr += "3 = HighQualityBicubic" & vbCrLf
        nstr += "4 = NearestNeighbor"

        Return nstr
    End Function
End Class
