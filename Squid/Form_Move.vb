Imports System.Drawing

Public Class Form_Move

    Private Sub PictureBox1_Visi(sender As Object, e As EventArgs) Handles Me.VisibleChanged
        If Not Me.Visible Then
            Grasshopper.Instances.DocumentEditor.Focus()
        End If
        Me.Parent.Focus()

    End Sub
End Class