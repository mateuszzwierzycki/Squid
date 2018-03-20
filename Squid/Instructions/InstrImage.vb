Public Class InstrImage
    Inherits InstrBase

    Private di As New DrawImage()

    ''' <summary>
    ''' Draws image.
    ''' </summary>
    ''' <param name="DrawI ">DrawImage to hold.</param>
    ''' <remarks></remarks>
    Sub New(DrawI As DrawImage)
        MyBase.InstructionType = "Image"
        di.Image = DrawI.Image.Clone
        di.Rectangle = DrawI.Rectangle
        di.Interpolation = DrawI.Interpolation
    End Sub

    Property DrawImage As DrawImage
        Set(value As DrawImage)
            di = value.Duplicate
        End Set
        Get
            Return di
        End Get
    End Property


End Class
