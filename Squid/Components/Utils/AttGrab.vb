Imports Grasshopper.GUI
Imports Grasshopper.GUI.Canvas
Imports System.Drawing
Imports Grasshopper.Kernel
Imports System.Drawing.Drawing2D
Imports Rhino.Geometry


Public Class AttGrab
    Inherits Grasshopper.Kernel.Attributes.GH_ComponentAttributes

    Public Sub New(ByVal owner As CompScreenGrab)
        MyBase.New(owner)
    End Sub


    Public Overrides Function RespondToMouseDoubleClick(ByVal sender As GH_Canvas, ByVal e As GH_CanvasMouseEvent) As GH_ObjectResponse
        If (ContentBox.Contains(e.CanvasLocation)) Then
            Dim sqp As CompScreenGrab = DirectCast(Owner, CompScreenGrab)
            sqp.ExpireSolution(True)
            Return Grasshopper.GUI.Canvas.GH_ObjectResponse.Handled
        End If

        Return Grasshopper.GUI.Canvas.GH_ObjectResponse.Ignore
    End Function



End Class
