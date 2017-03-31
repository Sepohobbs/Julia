Class MainWindow 
    Dim fractal_img As New WriteableBitmap(600, 600, 96, 96, PixelFormats.Rgb24, Nothing)

    Dim wdth As Integer = 600
    Dim hght As Integer = 600
    Dim rawStride As Integer = (wdth * 24 + 7) / 8
    Dim pxlData(rawStride * hght) As Byte

    Sub setPx(ByVal x As Integer, ByVal y As Integer, ByVal c As Color)
        For xp As Integer = x To x Step 1
            For yp As Integer = y To y Step 1
                Dim xindex As Integer = xp * 3
                Dim yindex As Integer = yp * rawStride
                If xindex + yindex + 2 < pxlData.Count Then
                    pxlData(xindex + yindex) = c.R
                    pxlData(xindex + yindex + 1) = c.G
                    pxlData(xindex + yindex + 2) = c.B
                End If
            Next
        Next
    End Sub

    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded

        setPx(300, 300, Colors.Azure)
        setPx(301, 300, Colors.Azure)
        setPx(302, 300, Colors.Azure)
        setPx(303, 300, Colors.Azure)
        setPx(304, 300, Colors.Azure)
        setPx(305, 300, Colors.Azure)

        updateimage()

    End Sub

    Sub updateimage()
        fractal_img.WritePixels(New Int32Rect(0, 0, wdth, hght), pxlData, rawStride, 0)
        main_canvas.Children.Clear()
        main_canvas.Background = New ImageBrush(fractal_img)
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click
       
    End Sub
End Class
