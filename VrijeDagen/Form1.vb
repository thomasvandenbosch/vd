'*****************************************************************************************************************
'*Window met 1 textbox. Geeft bij het opstarten een lijst van de komende 15 vrije dagen (met circulerend regime).*
'*****************************************************************************************************************

'TODO Weeknummers tonen bij overzicht

Imports System.Globalization
Imports System.IO

Public Class Form1

    Friend Const txtPath = "vd.txt"

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        MinimizeBox = False
        MaximizeBox = False

        Dim data As String()


        If File.Exists(txtPath) Then
            Using fileReader As StreamReader = My.Computer.FileSystem.OpenTextFileReader(txtPath)
                Dim stringReader As String = fileReader.ReadLine()
                data = stringReader.Split(" ")
            End Using
        Else
            data = {"19/11/2018", "V"}  'Standaardwaarden gebruiken als bestand niet bestaat
        End If


        Dim datum As Date = CType(data(0), Date)
        Dim ploeg As String = data(1)

        Do While datum < LaatsteMaandag()
            datum = VolgendeDatum(datum)
            ploeg = NieuwePloeg(ploeg)
        Loop

        File.WriteAllText(txtPath, $"{datum.Day.ToString().PadLeft(2, "0")}/{datum.Month.ToString().PadLeft(2, "0")}/{datum.Year} {ploeg}")

        PrintLijn(datum, ploeg)
        txtResultaat.Text += New String("-", 22) & vbCrLf

        For i = 1 To 14
            datum = VolgendeDatum(datum)
            ploeg = NieuwePloeg(ploeg)
            PrintLijn(datum, ploeg)
        Next i
        txtResultaat.Text = txtResultaat.Text.Substring(0, txtResultaat.TextLength - 2)
    End Sub

    Private Function VolgendeDatum(ByVal datum As Date) As Date
        If datum.DayOfWeek = DayOfWeek.Friday Then
            Return datum.AddDays(3)
        Else
            Return datum.AddDays(8)
        End If
    End Function

    Private Function NieuwePloeg(ByVal ploeg As String) As String
        If ploeg = "V" Then
            Return "L"
        Else
            Return "V"
        End If
    End Function

    Private Sub PrintLijn(ByVal datum As Date, ByVal ploeg As String)
        txtResultaat.Text += $"W {Weeknummer(datum).ToString("00")} | {datum.ToString("ddd dd/MM/yyyy")} {ploeg}" & vbCrLf
    End Sub

    Private Function LaatsteMaandag() As Date
        Dim vandaag As Date = Date.Today
        Dim weekdag As Integer = vandaag.DayOfWeek

        If weekdag = 1 Then 'vandaag is maandag
            Return vandaag
        ElseIf weekdag = 0 Then 'vandaag is zondag
            Return vandaag.AddDays(-6)
        Else
            Return vandaag.AddDays(1 - weekdag)
        End If
    End Function

    Private Function Weeknummer(ByVal datum As Date) As Integer
        Return My.Application.Culture.Calendar.GetWeekOfYear(datum, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)
    End Function
End Class
