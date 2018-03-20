Public Class InstrCurveEx
    Inherits InstrBase

    Private dcex As New DrawCurveEx()

    ''' <summary>
    ''' Draws complex curves using outline and fill.
    ''' </summary>
    ''' <param name="DrawCrvEx"></param>
    ''' <remarks></remarks>
    Sub New(ByVal DrawCrvEx As DrawCurveEx)
        MyBase.InstructionType = "DrawEx"
        dcex = DrawCrvEx.Duplicate
    End Sub

    Property DrawCurveEx As DrawCurveEx
        Set(value As DrawCurveEx)
            dcex = value.Duplicate
            MyBase.InstructionType = "DrawEx"
        End Set
        Get
            Return dcex
        End Get
    End Property

End Class
