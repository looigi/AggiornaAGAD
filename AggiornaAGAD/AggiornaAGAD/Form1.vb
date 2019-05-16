Imports System.IO

Public Class frmMain

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblAvanzamento.Text = ""
        Dim gf As New GestioneFilesDirectory
        Dim Cosa As String = ""

        If File.Exists(Application.StartupPath & "\Conf.dat") Then
            Cosa = gf.LeggeFileIntero(Application.StartupPath & "\Conf.dat")
        Else
            Cosa = "K:\Immagini\Rows;Provider=SQLNCLI11.1|Integrated Security=SSPI|Persist Security Info=False|User ID=|Initial Catalog=AGirlADay|Data Source=(local)|Initial File Name=|Server SPN=;C:\inetpub\wwwroot\AGirlADay\Appoggio;"
            gf.CreaAggiornaFile(Application.StartupPath & "\Conf.dat", cosa)
        End If
        gf = Nothing

        Dim c() As String = Cosa.Split(";")

        PathImmagini = c(0)
        ConnectionString = c(1).Replace("|", ";")
        PathProgressivi = c(2)
    End Sub

    'Private Function LeggeFileDiConfigurazione(gf As GestioneFilesDirectory) As String
    '    Dim Righe As String = gf.LeggeFileIntero(Application.StartupPath & "\Web.config")
    '    Dim Inizio As Integer = Righe.IndexOf("<connectionStrings>")
    '    Dim Fine As Integer = Righe.IndexOf("</connectionStrings>")
    '    Righe = Mid(Righe, Inizio, Fine - Inizio)
    '    Inizio = Righe.IndexOf("Provider")
    '    Righe = Mid(Righe, Inizio + 1, Righe.Length)
    '    Righe = Mid(Righe, 1, Righe.IndexOf("=""/> "))

    '    Return Righe
    'End Function

    Private Sub CaricaImmagini()
        Dim gf As New GestioneFilesDirectory
        Dim s As New SQLSERVER
        ' Dim connectionString As String = LeggeFileDiConfigurazione(gf)
        Dim ConnSQL As Object = s.ApreDB(ConnectionString)
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String = ""

        lblAvanzamento.Text = "Lettura immagini"
        Application.DoEvents()

        gf.ScansionaDirectorySingola(PathImmagini)
        Dim Filetti() As String = gf.RitornaFilesRilevati
        Dim qFiletti As Integer = gf.RitornaQuantiFilesRilevati

        lblAvanzamento.Text = "Immagini rilevate: " & qFiletti
        Application.DoEvents()

        Sql = "Truncate Table Immagini"
        s.EsegueSql(ConnSQL, Sql)

        For i As Integer = 1 To qFiletti
            ' If Filetti(i).ToUpper.IndexOf("LOCALI") = -1 Then
            If i / 100 = Int(i / 100) Then
                    lblAvanzamento.Text = "Scrittura: " & i & "/" & qFiletti
                    Application.DoEvents()
                End If

                Sql = "Insert Into Immagini Values ('" & Filetti(i).Replace(PathImmagini.Replace("Rows", ""), "").Replace("'", "''") & "')"
                s.EsegueSql(ConnSQL, Sql)
            ' End If
        Next

        ConnSQL.Close()
        s = Nothing
        gf = Nothing

        Try
            Kill(PathProgressivi & "\Progressivi.dat")
        Catch ex As Exception

        End Try
        End
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Enabled = False
        CaricaImmagini()
    End Sub
End Class
