Imports System.Drawing

Public Class InstrClear
    Inherits InstrBase

    Private clearcolor As Color = Color.White

    ''' <summary>
    ''' Clears image.
    ''' </summary>
    ''' <param name="Col"></param>
    ''' <remarks></remarks>
    Sub New(Col As Color)
        MyBase.InstructionType = "Clear"
        clearcolor = Col
    End Sub

    Property Clear As Color
        Set(value As Color)
            clearcolor = value
            MyBase.InstructionType = "Clear"
        End Set
        Get
            Return clearcolor
        End Get
    End Property

End Class
