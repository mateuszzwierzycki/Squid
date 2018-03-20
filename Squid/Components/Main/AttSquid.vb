Imports Grasshopper.GUI
Imports Grasshopper.GUI.Canvas
Imports System.Drawing
Imports Grasshopper.Kernel
Imports System.Drawing.Drawing2D
Imports Rhino.Geometry


Public Class AttSquid
    Inherits Grasshopper.Kernel.Attributes.GH_ComponentAttributes

    Public Sub New(ByVal owner As CompSquid)
        MyBase.New(owner)
    End Sub

    Public Overrides Function RespondToMouseDoubleClick(ByVal sender As GH_Canvas, ByVal e As GH_CanvasMouseEvent) As GH_ObjectResponse
        If (ContentBox.Contains(e.CanvasLocation)) Then
            Dim sqp As CompSquid = DirectCast(Owner, CompSquid)
            sqp.draw.AutohideSwitch = sqp.AutohideState
            sqp.draw.Show(Grasshopper.Instances.DocumentEditor)
            sqp.draw.AutoTransform()
    
            Return Grasshopper.GUI.Canvas.GH_ObjectResponse.Handled
        End If

        Return Grasshopper.GUI.Canvas.GH_ObjectResponse.Ignore
    End Function



End Class
