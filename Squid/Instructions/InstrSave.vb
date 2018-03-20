Public Class InstrSave
    Inherits InstrBase

    Private fol As String
    Private fname As String
    Private tif As Boolean = False

    ''' <summary>
    ''' Saves files to the specified file path.
    ''' </summary>
    ''' <param name="FileName"></param>
    ''' <param name="Folder"></param>
    ''' <remarks></remarks>
    Sub New(ByVal FileName As String, ByVal Folder As String, SaveTiff As Boolean)
        MyBase.InstructionType = "Save"
        fname = FileName.Clone
        fol = Folder.Clone
        tif = SaveTiff
    End Sub

    Property FileName As String
        Set(value As String)
            fname = value.Clone
        End Set
        Get
            Return fname
        End Get
    End Property

    Property Folder As String
        Set(value As String)
            fol = value.Clone
        End Set
        Get
            Return fol
        End Get
    End Property

    Property SaveAsTiff As Boolean
        Set(value As Boolean)
            tif = value
        End Set
        Get
            Return tif
        End Get
    End Property

End Class
