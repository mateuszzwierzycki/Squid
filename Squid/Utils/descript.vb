Imports Grasshopper.Kernel

Public Class descript
    Inherits Grasshopper.Kernel.GH_AssemblyInfo
    Public Overrides ReadOnly Property AuthorContact As String
        Get
            Return "mateuszzwierzycki@gmail.com"
        End Get
    End Property
    Public Overrides ReadOnly Property Description As String
        Get
            Return "GDI+ based bitmap drawing."
        End Get
    End Property
    Public Overrides ReadOnly Property AuthorName As String
        Get
            Return "Mateusz Zwierzycki"
        End Get
    End Property

    Public Overrides ReadOnly Property Icon As Drawing.Bitmap
        Get
            Return My.Resources.squid
        End Get
    End Property

    Public Overrides ReadOnly Property Id As Guid
        Get
            Return New Guid("{618B9650-F4E6-4F31-9713-7C72E6CC4E31}")
        End Get
    End Property
    Public Overrides ReadOnly Property Name As String
        Get
            Return "Squid"
        End Get
    End Property
    Public Overrides ReadOnly Property Version As String
        Get
            Return "0.110"
        End Get
    End Property
End Class
