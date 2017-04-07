Imports System.Numerics

Class MainWindow
    Dim fractal_img As New WriteableBitmap(600, 600, 96, 96, PixelFormats.Rgb24, Nothing)

    Dim wdth As Integer = 600
    Dim hght As Integer = 600
    Dim rawStride As Integer = (wdth * 24 + 7) / 8
    Dim pxlData(rawStride * hght) As Byte

    Dim x_max As Double = 2
    Dim x_men As Double = -2
    Dim y_max As Double = 2
    Dim y_min As Double = -2

    Dim c As New Complex(0.279, 0)              '(0.279, 0)
    Dim max_calc As Integer = 50



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

    Sub updateimage()
        fractal_img.WritePixels(New Int32Rect(0, 0, wdth, hght), pxlData, rawStride, 0)
        main_canvas.Children.Clear()
        main_canvas.Background = New ImageBrush(fractal_img)
    End Sub


    Public Function screen_to_grid(ByVal x As Integer, ByVal y As Integer)
        Dim x_size As Double = x_max - x_men
        Dim x_step As Double = x_size / wdth

        Dim y_size As Double = y_max - y_min
        Dim y_step As Double = y_size / hght

        Return New Complex(x_men + (x * x_step), y_min + (y * y_step))
    End Function


    Public Sub draw_julia()

        For x = 0 To wdth - 1
            For y = 0 To hght - 1
                Dim current_point As Complex = screen_to_grid(x, y)

                Dim speed As Integer = explosion(current_point)
                Dim bright As Byte = (speed / max_calc) * 255

                Dim r, g, b As Byte
                HSVtoRGB(speed * 4, 255, 255, r, g, b)
                setPx(x, y, Color.FromRgb(r, g, b))
            Next
        Next
        updateimage()
    End Sub


    Public Function explosion(ByVal z As Complex)

        For i = 0 To max_calc
            z = (z * z) + c

            If z.Magnitude > 25 Then
                Return i
            End If
        Next
        Return max_calc
    End Function

    
    Private Sub main_canvas_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles main_canvas.MouseDown
        draw_julia()
    End Sub

    Private Sub main_canvas_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Me.KeyDown

        If e.Key = Key.W Then
            c = New Complex(c.Real, c.Imaginary + 0.005)
        End If

        If e.Key = Key.S Then
            c = New Complex(c.Real, c.Imaginary - 0.005)
        End If

        If e.Key = Key.D Then
            c = New Complex(c.Real + 0.005, c.Imaginary)
        End If

        If e.Key = Key.A Then
            c = New Complex(c.Real - 0.005, c.Imaginary)
        End If
        draw_julia()

    End Sub

    

    Public Sub HSVtoRGB(ByVal H As Byte, ByVal S As Byte, ByVal V As Byte, _
                    ByRef R As Byte, ByRef G As Byte, ByRef B As Byte)
        Dim MinVal As Byte
        Dim MaxVal As Byte
        Dim Chroma As Byte
        Dim TempH As Single

        If V = 0 Then
            R = 0
            G = 0
            B = 0
        Else
            If S = 0 Then
                R = V
                G = V
                B = V
            Else
                MaxVal = V
                Chroma = S / 255 * MaxVal
                MinVal = MaxVal - Chroma
                Select Case H
                    Case Is >= 170
                        TempH = (H - 170) / 43
                        If TempH < 1 Then
                            B = MaxVal
                            R = MaxVal * TempH
                        Else
                            R = MaxVal
                            B = MaxVal * (2 - TempH)
                        End If
                        G = 0
                    Case Is >= 85
                        TempH = (H - 85) / 43
                        If TempH < 1 Then
                            G = MaxVal
                            B = MaxVal * TempH
                        Else
                            B = MaxVal
                            G = MaxVal * (2 - TempH)
                        End If
                        R = 0
                    Case Else
                        TempH = H / 43
                        If TempH < 1 Then
                            R = MaxVal
                            G = MaxVal * TempH
                        Else
                            G = MaxVal
                            R = MaxVal * (2 - TempH)
                        End If
                        B = 0
                End Select
                R = R / MaxVal * (MaxVal - MinVal) + MinVal
                G = G / MaxVal * (MaxVal - MinVal) + MinVal
                B = B / MaxVal * (MaxVal - MinVal) + MinVal
            End If
        End If
    End Sub

End Class
