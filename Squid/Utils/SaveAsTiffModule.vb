Imports System.Drawing
Imports System.Windows.Media.Imaging
Imports System.IO
Imports System.Windows.Media
Imports System.Drawing.Imaging
Imports System.Windows

Module SaveAsTiffModule

    Sub SaveTiffImage(ByRef bmp As Bitmap, filepath As String)

        Dim cmyk32 As System.Windows.Media.PixelFormat

        cmyk32 = PixelFormats.Cmyk32

        Dim filestr As New MemoryStream()

        Dim nb As Bitmap = bmp.Clone

        Dim nba As Bitmap = RenderAlpha(nb)

        Dim nfp As New String(filepath)

        nfp = nfp.Remove(nfp.Length - 4, 4)
        nfp += "_Alpha.tif"

        nb.Save(filestr, ImageFormat.Tiff)
        nba.Save(nfp, ImageFormat.Tiff)

        Dim enc As New TiffBitmapEncoder()

        Dim bmpsrc As BitmapSource = BitmapFrame.Create(filestr)

        Dim nfbmp As FormatConvertedBitmap = New FormatConvertedBitmap()
        nfbmp.BeginInit()

        nfbmp.Source = bmpsrc
        nfbmp.DestinationFormat = cmyk32
        nfbmp.AlphaThreshold = 0

        nfbmp.EndInit()

        enc.Compression = TiffCompressOption.Lzw
        enc.Frames.Add(BitmapFrame.Create(nfbmp))

        Dim cmykstr As FileStream = New FileStream(filepath, FileMode.Create, FileAccess.Write, FileShare.Write)
        enc.Save(cmykstr)

        cmykstr.Close()
        filestr.Close()

        filestr.Dispose()
        cmykstr.Dispose()
        nb.Dispose()
        nba.Dispose()
    End Sub

End Module
