Public Class ConvMatrix

    Private TL, TM, TR, ML, P, MR, BL, BM, BR, F, O As Integer

    Sub New()
        Reset()
    End Sub

    Public ReadOnly Property TopLeft As Integer
        Get
            Return TL
        End Get
    End Property

    Public ReadOnly Property TopMiddle As Integer
        Get
            Return TM
        End Get
    End Property

    Public ReadOnly Property TopRight As Integer
        Get
            Return TR
        End Get
    End Property

    Public ReadOnly Property MiddleLeft As Integer
        Get
            Return ML
        End Get
    End Property

    Public ReadOnly Property Pixel As Integer
        Get
            Return P
        End Get
    End Property

    Public ReadOnly Property MiddleRight As Integer
        Get
            Return MR
        End Get
    End Property

    Public ReadOnly Property BottomLeft As Integer
        Get
            Return BL
        End Get
    End Property

    Public ReadOnly Property BottomMiddle As Integer
        Get
            Return BM
        End Get
    End Property

    Public ReadOnly Property BottomRight As Integer
        Get
            Return BR
        End Get
    End Property

    Public ReadOnly Property Factor
        Get
            Return F
        End Get
    End Property

    Public ReadOnly Property Offset
        Get
            Return O
        End Get
    End Property

    Sub New(Pixel As Integer, Across As Integer, Diagonal As Integer, Factor As Integer, Offset As Integer)
        TL = Diagonal
        TM = Across
        TR = Diagonal
        ML = Across
        P = Pixel
        MR = Across
        BL = Diagonal
        BM = Across
        BR = Diagonal
        F = Factor
        O = Offset
    End Sub

    Public Function SetAll(TopLeft As Integer, TopMiddle As Integer, TopRight As Integer, _
         MiddleLeft As Integer, Pixel As Integer, MiddleRight As Integer, _
         BottomLeft As Integer, BottomMiddle As Integer, BottomRight As Integer, Factor As Integer, Offset As Integer) As Boolean

        TL = TopLeft
        TM = TopMiddle
        TR = TopRight
        ML = MiddleLeft
        P = Pixel
        MR = MiddleRight
        BL = BottomLeft
        BM = BottomMiddle
        BR = BottomRight
        F = Factor
        O = Offset

        Return True
    End Function

    Public Function SetAll(Pixel As Integer, Across As Integer, Diagonal As Integer, Factor As Integer, Offset As Integer) As Boolean
        P = Pixel
        TL = TR = BL = BR = Diagonal
        TM = ML = MR = BL = BR = Across
        F = Factor
        O = Offset

        Return True
    End Function

    Public Function SetAll(Pixel As Integer, All As Integer, Factor As Integer, Offset As Integer) As Boolean
        P = Pixel
        TL = TR = BL = BR = TM = ML = MR = BL = BR = All
        F = Factor
        O = Offset

        Return True
    End Function

    Public Sub Reset()
        TL = 0
        TM = 0
        TR = 0
        ML = 0
        P = 1
        MR = 0
        BL = 0
        BM = 0
        BR = 0
        F = 1
        O = 0
    End Sub



End Class
