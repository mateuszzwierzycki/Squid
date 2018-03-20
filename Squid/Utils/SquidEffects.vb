Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO


Module SquidEffects



    Public Function ApplyEffect(M As ConvMatrix, Bmp As Bitmap, RepeatX As Integer) As Bitmap

        'Lock the bitmaps' bits.  
        Dim rect As New Rectangle(0, 0, Bmp.Width, Bmp.Height)
        Dim bmpData As System.Drawing.Imaging.BitmapData = Bmp.LockBits(rect, Drawing.Imaging.ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb)

        ' Get the address of the first line.
        Dim ptr As IntPtr = bmpData.Scan0

        ' Declare an array to hold the bytes of the bitmap.
        ' This code is specific to a bitmap with 24 bits per pixels.
        Dim bytes As Integer = Math.Abs(bmpData.Stride) * Bmp.Height
        Dim rgbValues(bytes - 1) As Byte
        Dim rgbValuesCopy(bytes - 1) As Byte

        ' Copy the RGB values into the array.
        System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes)
        rgbValuesCopy = rgbValues


        'loooop
        Dim bw As Integer = (Bmp.Width - 3)
        Dim bh As Integer = (Bmp.Height - 3)
        Dim bmpw4 As Integer = Bmp.Width * 4
        Dim bmpw8 As Integer = Bmp.Width * 8


        Dim i As Integer = 0
        Dim r As Integer = 0
        Dim g As Integer = 0
        Dim b As Integer = 0
        Dim a As Integer = 0
        Dim divideF As Double = 1 / M.Factor
        Dim tl As Integer = M.TopLeft
        Dim tm As Integer = M.TopMiddle
        Dim tr As Integer = M.TopRight
        Dim ml As Integer = M.MiddleLeft
        Dim p As Integer = M.Pixel
        Dim mr As Integer = M.MiddleRight
        Dim bl As Integer = M.BottomLeft
        Dim bm As Integer = M.BottomMiddle
        Dim br As Integer = M.BottomRight
        Dim mof As Integer = M.Offset



        For j As Integer = 0 To RepeatX - 1 Step 1

            i = 0

            For y As Integer = 0 To bh Step 1

                For x As Integer = 0 To bw Step 1

                    a = tl * rgbValuesCopy(i) + _
             tm * rgbValuesCopy(i + 4) + _
             tr * rgbValuesCopy(i + 8) + _
             ml * rgbValuesCopy(i + bmpw4) + _
             p * rgbValuesCopy(i + bmpw4 + 4) + _
             mr * rgbValuesCopy(i + bmpw4 + 8) + _
             bl * rgbValuesCopy(i + bmpw8) + _
             bm * rgbValuesCopy(i + bmpw8 + 4) + _
             br * rgbValuesCopy(i + bmpw8 + 8)


                    a = (a * divideF) + mof

                    If a < 0 Then a = 0
                    If a > 255 Then a = 255

                    rgbValues(i + 4 + bmpw4) = (a)

                    i += 1

                    b = tl * rgbValuesCopy(i) + _
tm * rgbValuesCopy(i + 4) + _
tr * rgbValuesCopy(i + 8) + _
ml * rgbValuesCopy(i + bmpw4) + _
p * rgbValuesCopy(i + bmpw4 + 4) + _
mr * rgbValuesCopy(i + bmpw4 + 8) + _
bl * rgbValuesCopy(i + bmpw8) + _
bm * rgbValuesCopy(i + bmpw8 + 4) + _
br * rgbValuesCopy(i + bmpw8 + 8)

                    b = (b * divideF) + mof

                    If b < 0 Then b = 0
                    If b > 255 Then b = 255
                    rgbValues(i + 4 + bmpw4) = (b)

                    i += 1

                    g = tl * rgbValuesCopy(i) + _
tm * rgbValuesCopy(i + 4) + _
tr * rgbValuesCopy(i + 8) + _
ml * rgbValuesCopy(i + bmpw4) + _
p * rgbValuesCopy(i + bmpw4 + 4) + _
mr * rgbValuesCopy(i + bmpw4 + 8) + _
bl * rgbValuesCopy(i + bmpw8) + _
bm * rgbValuesCopy(i + bmpw8 + 4) + _
br * rgbValuesCopy(i + bmpw8 + 8)

                    g = (g * divideF) + mof

                    If g < 0 Then g = 0
                    If g > 255 Then g = 255
                    rgbValues(i + 4 + bmpw4) = (g)

                    i += 1

                    r = tl * rgbValuesCopy(i) + _
tm * rgbValuesCopy(i + 4) + _
tr * rgbValuesCopy(i + 8) + _
ml * rgbValuesCopy(i + bmpw4) + _
p * rgbValuesCopy(i + bmpw4 + 4) + _
mr * rgbValuesCopy(i + bmpw4 + 8) + _
bl * rgbValuesCopy(i + bmpw8) + _
bm * rgbValuesCopy(i + bmpw8 + 4) + _
br * rgbValuesCopy(i + bmpw8 + 8)

                    r = (r * divideF) + mof

                    If r < 0 Then r = 0
                    If r > 255 Then r = 255
                    rgbValues(i + 4 + bmpw4) = (r)

                    i += 1

                Next

                i += 8

            Next

            rgbValuesCopy = rgbValues

        Next

        ' Copy the RGB values back to the bitmap
        System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes)

        ' Unlock the bits.
        Bmp.UnlockBits(bmpData)

        Return Bmp

    End Function


    Public Sub ApplyMask(ByRef Bmp As Bitmap, ByRef Mask As Bitmap)

        'Lock the bitmaps' bits.  
        Dim rect As New Rectangle(0, 0, Bmp.Width, Bmp.Height)
        Dim bmpData As System.Drawing.Imaging.BitmapData = Bmp.LockBits(rect, Drawing.Imaging.ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb)
        Dim rectmask As New Rectangle(0, 0, Mask.Width, Mask.Height)
        Dim maskData As System.Drawing.Imaging.BitmapData = Mask.LockBits(rectmask, Drawing.Imaging.ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb)

        ' Get the address of the first line.
        Dim ptr As IntPtr = bmpData.Scan0
        Dim maskptr As IntPtr = maskData.Scan0

        ' Declare an array to hold the bytes of the bitmap.
        Dim bytes As Integer = bmpData.Stride * Bmp.Height
        Dim rgbValues(bytes) As Byte

        Dim maskBytes As Integer = maskData.Stride * Mask.Height
        Dim maskValues(maskBytes) As Byte

        ' Copy the RGB values into the array.
        System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes)
        System.Runtime.InteropServices.Marshal.Copy(maskptr, maskValues, 0, bytes)

        'loooop
        Dim bw As Integer = (Bmp.Width - 1)
        Dim bh As Integer = (Bmp.Height - 1)

        ' Dim a As Integer = 0
        Dim i As Integer = 0
        Dim a As Integer = 0

        For y As Integer = 0 To bh Step 1
            For x As Integer = 0 To bw Step 1
                ' rgbValues(i + 3) *= maskValues(i) / 255

                ' a = Math.Max((maskValues(i)), (maskValues(i + 1)))
                ' a = Math.Max((maskValues(i + 2)), a)
                a = 0
                a += maskValues(i)
                a += maskValues(i + 1)
                a += maskValues(i + 2)
                a = a / 3

                If a < 0 Then a = 0
                If a >= 255 Then a = 255
                'rgbValues(i + 3) = CByte((a / 255) * rgbValues(i + 3))

                rgbValues(i + 3) = CByte((a / 255) * rgbValues(i + 3))

                '0.21 R + 0.72 G + 0.07 B

                i += 4
            Next
        Next

        ' Copy the RGB values back to the bitmap
        System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes)
        System.Runtime.InteropServices.Marshal.Copy(maskValues, 0, maskptr, maskBytes)

        ' Unlock the bits.
        Bmp.UnlockBits(bmpData)
        Mask.UnlockBits(maskData)
    End Sub

    Public Function RenderAlpha(ByRef Bmp As Bitmap) As Bitmap

        Dim AlphaBmp As New Bitmap(Bmp.Width, Bmp.Height)

        'Lock the bitmaps' bits.  
        Dim rect As New Rectangle(0, 0, Bmp.Width, Bmp.Height)
        Dim bmpData As System.Drawing.Imaging.BitmapData = Bmp.LockBits(rect, Drawing.Imaging.ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb)
        Dim rectalpha As New Rectangle(0, 0, alphabmp.Width, alphabmp.Height)
        Dim alphaData As System.Drawing.Imaging.BitmapData = alphabmp.LockBits(rectalpha, Drawing.Imaging.ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb)

        ' Get the address of the first line.
        Dim ptr As IntPtr = bmpData.Scan0
        Dim alphaptr As IntPtr = alphaData.Scan0

        ' Declare an array to hold the bytes of the bitmap.
        Dim bytes As Integer = bmpData.Stride * Bmp.Height
        Dim rgbValues(bytes) As Byte

        Dim AlphaBytes As Integer = alphaData.Stride * AlphaBmp.Height
        Dim AlphaValues(AlphaBytes) As Byte

        ' Copy the RGB values into the array.
        System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes)
        System.Runtime.InteropServices.Marshal.Copy(alphaptr, AlphaValues, 0, bytes)

        'loooop
        Dim bw As Integer = (Bmp.Width - 1)
        Dim bh As Integer = (Bmp.Height - 1)


        Dim i As Integer = 0

        For y As Integer = 0 To bh Step 1
            For x As Integer = 0 To bw Step 1

                AlphaValues(i) = rgbValues(i + 3)
                AlphaValues(i + 1) = rgbValues(i + 3)
                AlphaValues(i + 2) = rgbValues(i + 3)
                AlphaValues(i + 3) = 255

                i += 4
            Next
        Next

        ' Copy the RGB values back to the bitmap
        System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes)
        System.Runtime.InteropServices.Marshal.Copy(AlphaValues, 0, alphaptr, AlphaBytes)

        ' Unlock the bits.
        Bmp.UnlockBits(bmpData)
        AlphaBmp.UnlockBits(alphaData)

        Return AlphaBmp
    End Function


End Module
