Imports System.Drawing.Drawing2D
Imports System.ComponentModel
Imports System.Windows.Forms

'CREDITS : snarfblam 
'http://www.xtremedotnettalk.com/showthread.php?t=97904

Public Class MyPic
    Inherits PictureBox

    Dim _interpolation As InterpolationMode = InterpolationMode.Default

    <DefaultValue(GetType(InterpolationMode), "Default"), _
    Description("The interpolation used to render the image.")> _
    Public Property Interpolation() As InterpolationMode
        Get
            Return _interpolation
        End Get
        Set(ByVal value As InterpolationMode)
            If value = InterpolationMode.Invalid Then _
                Throw New ArgumentException("""Invalid"" is not a valid value.") '(duh.)

            _interpolation = value
            Invalidate()
        End Set
    End Property

    Protected Overrides Sub OnPaint(ByVal pe As System.Windows.Forms.PaintEventArgs)
        pe.Graphics.InterpolationMode = _interpolation
        pe.Graphics.PixelOffsetMode = PixelOffsetMode.Half
        MyBase.OnPaint(pe)
    End Sub
End Class