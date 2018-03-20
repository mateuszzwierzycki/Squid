Imports Rhino
Imports Grasshopper
Imports Grasshopper.Kernel.Types
Imports Grasshopper.Kernel
Imports GH_IO

Public Class DrawCurveGoo
    Inherits Grasshopper.Kernel.Types.GH_Goo(Of DrawCurveClass)

    Sub New()

    End Sub

    Public Overrides Function Duplicate() As IGH_Goo
        Dim returnDup As New DrawCurveGoo
        MsgBox("here")
        Return returnDup
    End Function

    Public Overrides ReadOnly Property IsValid As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return "Squid.DrawCurve"
    End Function

    Public Overrides ReadOnly Property TypeDescription As String
        Get
            Return "Stores curve with fill and outline."
        End Get
    End Property

    Public Overrides ReadOnly Property TypeName As String
        Get
            Return "DrawCurve"
        End Get
    End Property
End Class
