Public Class InstrMask
    Inherits InstrBase

    Private di As New DrawMask()

    ''' <summary>
    ''' Draws image.
    ''' </summary>
    ''' <param name="DrawI ">DrawImage to hold.</param>
    ''' <remarks></remarks>
    Sub New(DrawI As DrawMask)
        MyBase.InstructionType = "Mask"
        di.Image = DrawI.Image.Clone
        di.Rectangle = DrawI.Rectangle
        di.Interpolation = DrawI.Interpolation
    End Sub

    Property DrawImage As DrawMask
        Set(value As DrawMask)
            di = value.Duplicate
        End Set
        Get
            Return di
        End Get
    End Property


End Class
