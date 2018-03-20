
Public Class InstrCurve
    Inherits InstrBase

    Private dc As New DrawCurve()

    ''' <summary>
    ''' Draws curve using outline and fill.
    ''' </summary>
    ''' <param name="DrawCrv"></param>
    ''' <remarks></remarks>
    Sub New(ByVal DrawCrv As DrawCurve)
        MyBase.InstructionType = "Draw"
        dc = DrawCrv.Duplicate
    End Sub

    Property DrawCurve As DrawCurve
        Set(value As DrawCurve)
            dc = value.Duplicate
        End Set
        Get
            Return dc
        End Get
    End Property

End Class
