Imports System.IO
Imports System.Threading

Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        NomeFiletto = Server.MapPath(".") & "\Appoggio\ListaImmagini.Dat"
        Allargata = False

        If Page.IsPostBack = False And Not Page.IsCallback Then
            Dim sb1 As System.Text.StringBuilder = New System.Text.StringBuilder()

            sb1.Append("<script type='text/javascript' language='javascript'>")
            sb1.Append("     PrendeDimensioniSchermo();")
            sb1.Append("</script>")

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "prendeDS", sb1.ToString(), False)

            'ControllaDB()

            Try
                MkDir(Server.MapPath(".") & "\Log")
            Catch ex As Exception

            End Try

            ScriveLog("Page postback")

            Try
                MkDir(Server.MapPath(".") & "\Appoggio")
            Catch ex As Exception

            End Try
            divPopup.Visible = False

            If Not File.Exists(Server.MapPath(".") & "\PathImmagini.txt") Then
                Dim H As HttpApplication = HttpContext.Current.ApplicationInstance
                Dim StringaPassaggio As String

                StringaPassaggio = "?SQL=Page_Load"
                StringaPassaggio = StringaPassaggio & "&Utente=" & H.Session("idUtente")
                StringaPassaggio = StringaPassaggio & "&Chiamante=" & H.Request.CurrentExecutionFilePath.ToUpper.Trim
                StringaPassaggio = StringaPassaggio & "&Errore=File di percorso immagini non trovato (PathImmagini.txt nella root)"
                H.Response.Redirect("Errore.aspx" & StringaPassaggio)
            End If

            Dim gf As New GestioneFilesDirectory
            Percorso = gf.LeggeFileIntero(Server.MapPath(".") & "\PathImmagini.txt")
            PercorsoIIS = Server.MapPath(".") & "\Immagini"
            If Not Directory.Exists(Percorso) Then
                Dim H As HttpApplication = HttpContext.Current.ApplicationInstance
                Dim StringaPassaggio As String

                StringaPassaggio = "?SQL=Page_Load"
                StringaPassaggio = StringaPassaggio & "&Utente=" & H.Session("idUtente")
                StringaPassaggio = StringaPassaggio & "&Chiamante=" & H.Request.CurrentExecutionFilePath.ToUpper.Trim
                StringaPassaggio = StringaPassaggio & "&Errore=Percorso immagini non trovato: " & Percorso
                H.Response.Redirect("Errore.aspx" & StringaPassaggio)
            End If

            'CaricaImmagini()

            divThumbs.Style.Add("display", "none")
            'divFotografia.Style.Add("display", "visible")

            ' Timer1.Enabled = True

            CaricaImmagini()
            If Progressivi Is Nothing = True Then
                PrendeImmagine()
            Else
                PrendeImmagine(Progressivi(Progressivo), True)
            End If

            PulisceCartellaAppoggio
        End If
    End Sub

    Private Sub PulisceCartellaAppoggio()
        Dim gf As New GestioneFilesDirectory
        gf.ScansionaDirectorySingola(Server.MapPath(".") & "\Appoggio")
        Dim filetti() As String = gf.RitornaFilesRilevati
        Dim q As Integer = gf.RitornaQuantiFilesRilevati
        For i As Integer = 1 To q
            If filetti(i).Contains(Server.MapPath(".") & "\Appoggio") Then
                File.Delete(filetti(i))
            End If
        Next
    End Sub

    Private Sub GeneraThumbs()
        Dim x As Random = New Random()
        Dim xx As Long
        Dim Ancora As Boolean
        Dim imme(quanteThumbs) As Integer
        ' Dim QuanteFatte As Integer = 0
        Dim Nome As String = ""
        Dim NomeDest As String
        Dim gf As New GestioneFilesDirectory

        Dim s As New SQLSERVER
        Dim connectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnectionStringLOC").ToString()
        Dim ConnSQL As Object = s.ApreDB(connectionString)
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String

        'Dim imm(quanteThumbsX * quanteThumbsY) As ImageButton
        'Dim sNome(quanteThumbs) As String
        'Dim indiceImm(quanteThumbs) As Integer

        Try
            MkDir(Server.MapPath(".") & "\Thumbs")
        Catch ex As Exception

        End Try

        'Dim width As Integer = (90 / quanteThumbsX)
        'Dim height As Integer = (90 / quanteThumbsY)

        Try
            File.Delete(Server.MapPath(".") & "\Thumbs\Imme.dat")
        Catch ex As Exception

        End Try

        gf.ApreFileDiTestoPerScrittura(Server.MapPath(".") & "\Thumbs\Imme.dat")

        For i As Integer = 1 To quanteThumbs
            'For k As Integer = 1 To quanteThumbsX
            Ancora = True
            While Ancora
                Ancora = False

                xx = x.Next(qImmagini)
                If xx > qImmagini Then xx = qImmagini
                If xx < 1 Then xx = 1

                For z = 1 To quanteThumbs
                    If imme(z) = xx Then
                        Ancora = True
                        Exit For
                    End If
                Next

                If Not Ancora Then
                    Sql = "Select Immagine From Immagini Where idRiga=" & xx
                    Rec = s.LeggeQuery(ConnSQL, Sql)
                    If Not Rec.Eof Then
                        If Rec("Immagine").Value.ToString.ToUpper.Contains(".DB") Then
                            Ancora = False
                        Else
                            Nome = Percorso & "\" & Rec("Immagine").Value
                            NomeDest = gf.TornaNomeFileDaPath(Rec("Immagine").Value)

                            gf.ScriveTestoSuFileAperto(i & Chr(249) & xx & Chr(250) & Nome & Chr(251))

                            Do While File.Exists(Server.MapPath(".") & "\Thumbs\" & i.ToString & ".jpg")
                                Try
                                    File.Delete(Server.MapPath(".") & "\Thumbs\" & i.ToString & ".jpg")
                                Catch ex As Exception

                                End Try

                                Thread.Sleep(100)
                            Loop

                            Do While Not File.Exists(Server.MapPath(".") & "\Thumbs\" & i.ToString & ".jpg")
                                Try
                                    FileCopy(Nome, Server.MapPath(".") & "\Thumbs\" & i.ToString & ".jpg")
                                Catch ex As Exception

                                End Try

                                Thread.Sleep(100)
                            Loop
                        End If
                    End If
                    Rec.Close()
                End If
            End While

            'Dim div As HtmlGenericControl
            'div = New HtmlGenericControl
            'div.Style.Add("margin", "3px auto")
            'div.Style.Add("float", "left")
            'div.Style.Add("width", width & "%")
            'div.Style.Add("height", height & "%")

            'imm(QuanteFatte) = New ImageButton
            'imm(QuanteFatte).ImageUrl = "Thumbs\" & QuanteFatte.ToString & ".jpg?id=" & Now
            'imm(QuanteFatte).ID = "img" & QuanteFatte.ToString
            'imm(QuanteFatte).ToolTip = sNome(QuanteFatte)
            'imm(QuanteFatte).Style.Add("width", "95%")
            'imm(QuanteFatte).Style.Add("height", "95%")
            'AddHandler imm(QuanteFatte).Click, AddressOf CaricaImmagineDaThumbs

            'str.Append("<div style=""width:" & width & "%; height: " & height & "%; float: left; margin: 3px auto; background-color: #aca9a3;"" OnClick=""MostraImmagineDaThumbs('" & sNome(QuanteFatte).Replace("\", "§") & "'); return false;"">")
            'str.Append("<Image ID=""img" & QuanteFatte.ToString & """ runat=""server"" Src=""Thumbs\" & QuanteFatte.ToString & ".jpg?id=" & Now & """ ToolTip=""" & gf.TornaNomeFileDaPath(sNome(QuanteFatte)) & """ Width=""95%"" Height=""80%"" />")
            'str.Append("</div>")

            'div.Controls.Add(imm(QuanteFatte))

            'divThumbsDetail.Controls.Add(div)

            'QuanteFatte += 1
            'Next
        Next

        gf.ChiudeFileDiTestoDopoScrittura()
    End Sub

    Protected Sub clickimm(sender As Object, e As ImageClickEventArgs)
        Dim i As ImageButton = DirectCast(sender, ImageButton)
        Dim indice As String = i.ToolTip
        Dim a As Integer = indice.IndexOf("-")
        indice = Mid(indice, 1, a)

        divThumbs.Style.Add("display", "none")
        'divFotografia.Style.Add("display", "visible")
        Allargata = False

        CaricaImmagine(indice)
    End Sub

    'Protected Sub CaricaImmagineDaThumbs(sender As Object, e As EventArgs) Handles btnRefreshThumbs.Click
    '    Dim Nome As String = hdnImmaginePass.Value.ToString.Replace("§", "\")

    '    Nome = Nome.Replace(Percorso & "\", "")

    '    Dim s As New SQLSERVER
    '    Dim connectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnectionStringLOC").ToString()
    '    Dim ConnSQL As Object = s.ApreDB(connectionString)
    '    Dim Rec As Object = CreateObject("ADODB.Recordset")
    '    Dim Sql As String = "Select idRiga From Immagini Where Immagine='" & Nome & "'"
    '    Dim idRiga As Long = -1

    '    Rec = s.LeggeQuery(ConnSQL, Sql)
    '    If Not Rec.Eof Then
    '        idriga = Rec(0).Value
    '    End If
    '    Rec.Close()

    '    If idRiga > -1 Then
    '        divThumbs.Style.Add("display", "none")
    '        Allargata = False

    '        PrendeImmagine(idRiga, False)
    '    End If
    'End Sub

    Private Sub ControllaDB()
        Dim s As New SQLSERVER
        Dim connectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnectionStringLOC").ToString()
        Dim ConnSQL As Object = s.ApreDB(connectionString)
        Dim Sql As String

        Sql = "Update DettagliImmagini Set Numimmagini=NumImmagini Where NumImmagini=0"
        Try
            s.EsegueSqlSenzaTRY(ConnSQL, Sql)
        Catch ex As Exception
            Sql = "CREATE TABLE [dbo].[DettagliImmagini](" &
                    "[NumImmagini] [int] NULL" &
                    ") ON [PRIMARY]"
            s.EsegueSql(ConnSQL, Sql)

            Sql = "Insert Into DettagliImmagini Values (0)"
            s.EsegueSql(ConnSQL, Sql)
        End Try

        Sql = "Update Memoria Set id=id"
        Try
            s.EsegueSqlSenzaTRY(ConnSQL, Sql)
        Catch ex As Exception
            Sql = "CREATE TABLE [dbo].[Memoria](" &
                "[id] [int] NOT NULL," &
                "[Progressivo] [int] NOT NULL," &
                "[Numero] [int] NULL," &
                "CONSTRAINT [PK_Memoria] PRIMARY KEY CLUSTERED " &
                "(" &
                "[id] ASC," &
                "[Progressivo] Asc" &
                ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" &
                ") ON [PRIMARY]"
            s.EsegueSql(ConnSQL, Sql)
        End Try

        ConnSQL.Close()
        s = Nothing
    End Sub

    Private Sub CaricaImmagini()
        ScriveLog("Entrato in CaricaImmagini")

        Dim gf As New GestioneFilesDirectory
        Dim s As New SQLSERVER
        Dim connectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnectionStringLOC").ToString()
        Dim ConnSQL As Object = s.ApreDB(connectionString)
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String = ""

        ScriveLog("Conteggio immagini")

        Sql = "Select NumImmagini From DettagliImmagini"
        Rec = s.LeggeQuery(ConnSQL, Sql)
        If Rec(0).Value Is DBNull.Value = False Then
            qImmagini = Rec(0).Value
        Else
            qImmagini = -1
        End If
        Rec.Close()

        If qImmagini < 1 Then
            Sql = "Select Count(*) From Immagini"
            Rec = s.LeggeQuery(ConnSQL, Sql)
            If Rec(0).Value Is DBNull.Value = False Then
                qImmagini = Rec(0).Value

                Sql = "Update DettagliImmagini Set NumImmagini=" & qImmagini
                s.EsegueSql(ConnSQL, Sql)
            Else
                qImmagini = -1
            End If
        End If

        Dim r As New RoutineVarie
        ScriveLog("Immagini rilevate: " & r.FormattaNumero(qImmagini, False))

        If qImmagini < 1 Then
            EffettuaRefresh(True)
        End If

        Dim Numero As Integer = 0

        Progressivo = -1
        Sql = "Select * From Memoria Where id=1 Order By Progressivo"
        Rec = s.LeggeQuery(ConnSQL, Sql)
        Erase Progressivi
        Do Until Rec.Eof
            ReDim Preserve Progressivi(Numero)
            Progressivi(Numero) = Rec("Numero").Value
            Progressivo = Numero
            numero += 1

            Rec.MoveNext()
        Loop
        Rec.Close()

        ConnSQL.Close()
        r = Nothing
        s = Nothing
        gf = Nothing

        ScriveLog("Uscito da CaricaImmagine")
    End Sub

    Private Sub PrendeImmagine(Optional NumeroImm As Integer = -1, Optional Indietro As Boolean = False)
        ScriveLog("Entrato in PrendeImmagine")

        Dim x As Long

        If NumeroImm > -1 Then
            x = NumeroImm
        Else
            Dim x1 As Random

            x1 = New Random()
            x = x1.Next(qImmagini)
            If x > qImmagini Then x = qImmagini
            If x < 1 Then x = 1
        End If

        NumeroImmagine = x

        CaricaImmagine(NumeroImmagine)

        Dim nn As Integer = -1
        If Progressivi Is Nothing = False Then
            nn = Progressivi.Length - 1
        End If
        If Indietro = False And Progressivo = nn Then
            AggiungeNumeroAMemoria(NumeroImmagine)

            Dim n As Integer = 0
            If Progressivi Is Nothing = False Then
                n = Progressivi.Length
            End If
            ReDim Preserve Progressivi(n)
            Progressivi(Progressivi.Length - 1) = NumeroImmagine
            Progressivo = n
        End If

        Dim r As New RoutineVarie
        ScriveLog("Uscito da PrendeImmagine. Progressivo: " & r.FormattaNumero(Progressivo, False))
        r = Nothing
    End Sub

    Private Sub AggiungeNumeroAMemoria(Numero As Long)
        ScriveLog("Entrato in AggiungeNumeroAMemoria")

        Dim s As New SQLSERVER
        Dim connectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnectionStringLOC").ToString()
        Dim ConnSQL As Object = s.ApreDB(connectionString)
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String = "Select Max(Progressivo)+1 From Memoria Where id=1"
        Dim Massimo As Long

        Rec = s.LeggeQuery(ConnSQL, Sql)
        If Rec(0).Value Is DBNull.Value Then
            Massimo = 1
        Else
            Massimo = Rec(0).Value
        End If
        Rec.Close()

        Sql = "Insert Into Memoria Values (1, " & Massimo & ", " & Numero & ")"
        s.EsegueSql(ConnSQL, Sql)

        ConnSQL.Close()
        s = Nothing

        ScriveLog("Uscito da AggiungeNumeroAMemoria")
    End Sub

    Private Sub CaricaImmagine(NumeroImmagine As Long)
        Dim r As New RoutineVarie
        ScriveLog("Entrato in CaricaImmagine. Immagine da caricare: " & r.FormattaNumero(NumeroImmagine, False))

        Dim s As New SQLSERVER
        Dim connectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnectionStringLOC").ToString()
        Dim ConnSQL As Object = s.ApreDB(connectionString)
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String = "Select Immagine From Immagini Where idRiga=" & NumeroImmagine

        Rec = s.LeggeQuery(ConnSQL, Sql)
        ScriveLog("Query eseguita")
        If Rec.Eof = False Then
            Dim Immaginella As String = Rec("Immagine").Value

            ScriveLog("Nome immagine da caricare: " & Immaginella)

            ' hdnImmagine.Value = Immaginella

            Dim Imm As String = Percorso.Replace("\", "/") & "/" & Immaginella.Replace("\", "/")
            Dim immFisico As String = Percorso & "\" & Immaginella
            'If Not File.Exists(immFisico) Then
            '    PrendeImmagine()
            'End If
            hdnNomeImmagine.Value = Percorso & "\" & Immaginella

            'Try
            '    MkDir(Server.MapPath(".") & "\Appoggio")
            'Catch ex As Exception

            'End Try

            If Not File.Exists(immFisico) Then
                s.EsegueSql(ConnSQL, "Delete From Immagini")

                Dim H As HttpApplication = HttpContext.Current.ApplicationInstance
                Dim StringaPassaggio As String

                StringaPassaggio = "?SQL=CaricamentoImmagine"
                StringaPassaggio = StringaPassaggio & "&Utente=" & H.Session("idUtente")
                StringaPassaggio = StringaPassaggio & "&Chiamante=" & H.Request.CurrentExecutionFilePath.ToUpper.Trim
                StringaPassaggio = StringaPassaggio & "&Errore=File non esistente: " & immFisico & ": Provare a riavviare il sito"
                H.Response.Redirect("Errore.aspx" & StringaPassaggio)
            End If

            Dim gi As New GestioneImmagini
            'Dim sDime As String = gi.RitornaDimensioneImmagine(immFisico)
            'Dim Ancora As Boolean = True
            'Dim Conta As Integer = 0

            'If sDime.IndexOf("ERRORE") > -1 Then
            '    ScriveLog("Errore su lettura dimensioni file")
            '    lblNomeImmagine2.Text = "&nbsp;ERRORE: Errore su lettura dimensioni file&nbsp;"
            '    imgImmagine.Visible = False
            'Else
            '    Dim Dime() As String = sDime.Split("x")
            '    If hdnDimensioniSchermo.Value = "" Then
            '    Else
            'Dim dimeSchermo() As String = hdnDimensioniSchermo.Value.Split(";")
            'Dim dimX As Integer = dimeSchermo(0)
            'Dim dimY As Integer = dimeSchermo(1)

            'ScriveLog("Dimensioni file: " & r.FormattaNumero(Dime(0), False) & " x " & r.FormattaNumero(Dime(1), False))

            'If Val(Dime(0)) > dimX Or Val(Dime(1)) > dimY Then
            '    Dim x As Integer
            '    Dim y As Integer
            '    Dim a As Single
            '    Dim a1 As Single
            '    Dim a2 As Single

            '    a1 = Val(Dime(0)) / dimX
            '    a2 = Val(Dime(1)) / dimY
            '    If a1 > a2 Then
            '        a = a1
            '    Else
            '        a = a2
            '    End If
            '    x = Val(Dime(0)) / a
            '    y = Val(Dime(1)) / a

            'If File.Exists(Server.MapPath(".") & "\Appoggio\Imma.jpg") Then
            '    While Ancora
            '        ScriveLog("Eliminazione file di appoggio Appoggio\Imma.jpg. Tentativo " & Conta)
            '        Try
            '            File.Delete(Server.MapPath(".") & "\Appoggio\Imma.jpg")
            '            Ancora = False
            '        Catch ex As Exception
            '            ScriveLog("Eliminazione file di appoggio Appoggio\Imma.jpg. Errore: " & ex.Message)
            '        End Try

            '        Thread.Sleep(1000)

            '        Conta += 1
            '        If Conta > 10 Then
            '            Dim H As HttpApplication = HttpContext.Current.ApplicationInstance
            '            Dim StringaPassaggio As String

            '            StringaPassaggio = "?SQL=CaricamentoImmagine"
            '            StringaPassaggio = StringaPassaggio & "&Utente=" & H.Session("idUtente")
            '            StringaPassaggio = StringaPassaggio & "&Chiamante=" & H.Request.CurrentExecutionFilePath.ToUpper.Trim
            '            StringaPassaggio = StringaPassaggio & "&Errore=Problemi nell'eliminazione dell'immagine di appoggio: Appoggio\Imma.jpg"
            '            H.Response.Redirect("Errore.aspx" & StringaPassaggio)
            '        End If
            '    End While
            'End If

            '    ScriveLog("Ridimensionamento immagine in file di appoggio")

            '    gi.Ridimensiona(immFisico, Server.MapPath(".") & "\Appoggio\Imma.jpg", x, y)
            'Else
            '    While Ancora
            '        ScriveLog("Eliminazione file di appoggio Appoggio\Imma.jpg. Tentativo " & Conta)
            '        Thread.Sleep(1000)

            '        Conta += 1
            '        If Conta > 10 Then
            '            Dim H As HttpApplication = HttpContext.Current.ApplicationInstance
            '            Dim StringaPassaggio As String

            '            StringaPassaggio = "?SQL=CaricamentoImmagine"
            '            StringaPassaggio = StringaPassaggio & "&Utente=" & H.Session("idUtente")
            '            StringaPassaggio = StringaPassaggio & "&Chiamante=" & H.Request.CurrentExecutionFilePath.ToUpper.Trim
            '            StringaPassaggio = StringaPassaggio & "&Errore=Problemi nel caricamento dell'immagine: " & immFisico
            '            H.Response.Redirect("Errore.aspx" & StringaPassaggio)
            '        End If
            '    End While

            '    'Try
            '    '    FileIO.FileSystem.DeleteFile(Server.MapPath(".") & "\Appoggio\Imma.jpg")
            '    'Catch ex As Exception

            '    'End Try

            '    'FileIO.FileSystem.CopyFile(immFisico, Server.MapPath(".") & "\Appoggio\Imma.jpg")

            '    Dim x As Integer
            '    Dim y As Integer
            '    Dim a As Single
            '    Dim a1 As Single
            '    Dim a2 As Single

            '    a1 = Val(Dime(0)) / dimX
            '    a2 = Val(Dime(1)) / dimY
            '    If a1 > a2 Then
            '        a = a1
            '    Else
            '        a = a2
            '    End If
            '    x = Val(Dime(0)) / a
            '    y = Val(Dime(1)) / a

            '    gi.Ridimensiona(immFisico, Server.MapPath(".") & "\Appoggio\Imma.jpg", x, y)
            '    Dime(0) = x
            '    Dime(1) = y
            'End If

            'Dim x As Integer
            'Dim y As Integer
            'Dim a As Single
            'Dim a1 As Single
            'Dim a2 As Single

            'a1 = Val(Dime(0)) / dimX
            'a2 = Val(Dime(1)) / dimY
            'If a1 > a2 Then
            '    a = a1
            'Else
            '    a = a2
            'End If
            'x = Val(Dime(0)) / a
            'y = Val(Dime(1)) / a

            'gi.Ridimensiona(immFisico, Server.MapPath(".") & "\Appoggio\Imma.jpg", x, y)
            'Dime(0) = x
            'Dime(1) = y

            ' immFisico = Server.MapPath(".") & "\Appoggio\Imma.jpg"
            Dim Ora As String = Now.Year & Format(Now.Month, "00") & Format(Now.Day, "00") & Format(Now.Hour, "00") & Format(Now.Minute, "00") & Format(Now.Second, "00")
            Imm = "Appoggio/" & Ora & ".Jpg"

            'hdnImmagine.Value = Ora

            Try
                File.Delete(Server.MapPath(".") & "\Appoggio\" & Ora & ".jpg")
            Catch ex As Exception

            End Try

            '        ScriveLog("Copia immagine nel file di appoggio Appoggio\Imma.jpg. Tentativo " & Conta)
            Try
                FileCopy(immFisico, Server.MapPath(".") & "\Appoggio\" & Ora & ".jpg")
            Catch ex As Exception

            End Try

            'If File.Exists(immFisico) = True Then
            '    ScriveLog("Impostazione campi di testo")

            Dim gf As New GestioneFilesDirectory
            lblNomeImmagine2.Text = "Nome: " & gf.TornaNomeFileDaPath(Immaginella).Replace(gf.TornaEstensioneFileDaPath(Immaginella), "") & "<br />"
            lblNomeImmagine2.Text += "Tipologia: " & gf.TornaEstensioneFileDaPath(Immaginella).ToUpper.Replace(".", "") & "<br />"
            lblNomeImmagine2.Text += "Path: " & gf.TornaNomeDirectoryDaPath(Immaginella).Replace("Rows\", "") & "<br />"
            lblNomeImmagine2.Text += "N° " & r.FormattaNumero(NumeroImmagine, False) & "/" & r.FormattaNumero(qImmagini, False) & "<br />"
            'lblNomeImmagine2.Text += "Size: " & sDime & "<br />"
            lblNomeImmagine2.Text += "Kb.: " & r.FormattaNumero(FileLen(immFisico), False)
            Dim sPath As String = gf.TornaNomeDirectoryDaPath(Immaginella)
            gf = Nothing

            imgImmagine.Visible = True

            Dim sb1 As System.Text.StringBuilder = New System.Text.StringBuilder()

            sb1.Append("<script type='text/javascript' language='javascript'>")
            sb1.Append("     posizionaImm('" & Ora & "');")
            sb1.Append("</script>")

            ' ScriveLog("Posizionamento immagine")

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "posiImm", sb1.ToString(), False)
            'Else
            '    lblNomeImmagine2.Text = "&nbsp;ERRORE: File inesistente&nbsp;"
            '    imgImmagine.Visible = False
            'End If
            'End If

            gi = Nothing

                imgAvanti.Visible = True
                imgIndietro.Visible = True
                imgElimina.Visible = True
        'End If
        Else
            ScriveLog("Nessuna immagine")

            lblNomeImmagine2.Text = ""
            imgAvanti.Visible = False
            imgIndietro.Visible = False
            imgElimina.Visible = False
        End If
        Rec.Close()

        ConnSQL.Close()
        s = Nothing

        ScriveLog("Uscita da CaricaImmagine")
    End Sub

    Protected Sub imgAvanti_Click(sender As Object, e As ImageClickEventArgs) Handles imgAvanti.Click
        Dim nn As Integer = -1
        Dim Indietro As Boolean = False
        Dim n As Integer

        If Progressivi Is Nothing = False Then
            nn = Progressivi.Length - 1
        End If
        If Progressivo < nn Then
            Progressivo += 1
            Indietro = True
            n = Progressivi(Progressivo)
        Else
            n = -1
            Indietro = False
        End If

        PrendeImmagine(n, Indietro)
    End Sub

    Protected Sub imgIndietro_Click(sender As Object, e As ImageClickEventArgs) Handles imgIndietro.Click
        Dim x As Integer = -1

        For i As Integer = Progressivo To 1 Step -1
            If Progressivi(i) <> "" Then
                If Progressivi(i) = NumeroImmagine Then
                    x = Progressivi(i - 1)
                    Progressivo = i - 1
                    Exit For
                End If
            End If
        Next

        PrendeImmagine(x)
    End Sub

    Private Sub EffettuaRefresh(Optional DaPost As Boolean = False)
        ScriveLog("Entrata in refresh")

        Dim s As New SQLSERVER
        Dim connectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnectionStringLOC").ToString()
        Dim ConnSQL As Object = s.ApreDB(connectionString)
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim gf As New GestioneFilesDirectory
        Dim Sql As String

        Erase Progressivi
        Progressivo = -1

        ScriveLog("Scansionamento directory " & Percorso)

        gf.ScansionaDirectorySingola(Percorso)
        Dim Filetti() As String = gf.RitornaFilesRilevati
        Dim qFiletti As Integer = gf.RitornaQuantiFilesRilevati
        Dim r As New RoutineVarie

        ScriveLog("Files: " & r.FormattaNumero(qFiletti, False))
        ScriveLog("Pulizia tabella")

        Sql = "Truncate Table Immagini"
        s.EsegueSql(ConnSQL, Sql)

        ScriveLog("Tabella pulita")

        ScriveLog("Salvataggio immagini in tabella")

        For i As Integer = 1 To qFiletti
            If Filetti(i).ToUpper.IndexOf("LOCALI") = -1 And Filetti(i).ToUpper.IndexOf("PENNETTA") = -1 And Filetti(i).ToUpper.IndexOf(".INI") = -1 Then
                Sql = "Insert Into Immagini Values ('" & Filetti(i).Replace(Percorso & "\", "").Replace("'", "''") & "')"
                s.EsegueSql(ConnSQL, Sql)
            End If
        Next
        ScriveLog("Salvataggio immagini in tabella effettuato")

        ScriveLog("Conteggio immagini")

        Sql = "Select Count(*) From Immagini"
        Rec = s.LeggeQuery(ConnSQL, Sql)
        qImmagini = Rec(0).Value
        Rec.Close()

        Sql = "Update DettagliImmagini Set NumImmagini=" & qImmagini
        s.EsegueSql(ConnSQL, Sql)

        ScriveLog("Conteggio immagini effttuato: " & r.FormattaNumero(qImmagini, False))

        ConnSQL.Close()
        s = Nothing

        If qImmagini > 0 And Not DaPost Then
            CaricaImmagini()

            PrendeImmagine()
        End If

        ScriveLog("Uscita da refresh")
    End Sub

    Protected Sub imgRefresh_Click(sender As Object, e As ImageClickEventArgs) Handles imgRefresh2.Click
        EffettuaRefresh()
    End Sub

    Public Sub VisualizzaMessaggioInPopup(ByVal MessaggioPopup As String)
        Dim ulPopup As HtmlGenericControl = DirectCast(Me.FindControl("ulPopup"), HtmlGenericControl)
        Dim Ritorno As String = ""

        Ritorno = "<li><span class=""etichettapopup"">" & MessaggioPopup & "</span></li>"
        If Left(Ritorno, 4).ToUpper.Trim <> "<LI>" Then
            Ritorno = "<li>" & Ritorno
        End If
        If Right(Ritorno, 5).ToUpper.Trim <> "</LI>" Then
            Ritorno = Ritorno & "</li>"
        End If
        ulPopup.InnerHtml = Ritorno

        divPopup.Visible = True
    End Sub

    Protected Sub cmdOK_Click(sender As Object, e As EventArgs) Handles cmdOK.Click
        divPopup.Visible = False
        PrendeImmagine()
    End Sub

    Protected Sub imgElimina_Click(sender As Object, e As ImageClickEventArgs) Handles imgElimina2.Click
        Dim immFisico As String = hdnNomeImmagine.Value.ToString.Replace("\", "/")

        'Dim sb1 As System.Text.StringBuilder = New System.Text.StringBuilder()

        'sb1.Append("<script type='text/javascript' language='javascript'>")
        'sb1.Append("     messaggio('" & immFisico & "');")
        'sb1.Append("</script>")

        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "nomeImm", sb1.ToString(), False)

        File.Delete(immFisico)

        PrendeImmagine()
    End Sub

    Protected Sub imgRefreshThumbs_Click(sender As Object, e As ImageClickEventArgs) Handles imgRefreshThumbs.Click
        GeneraThumbs()
        CaricaThumbs()
    End Sub

    Private Sub CaricaThumbs()
        If File.Exists(Server.MapPath(".") & "\Thumbs\Imme.dat") Then
            Dim sNome(quanteThumbs) As String
            Dim indiceImm(quanteThumbs) As Integer
            Dim gf As New GestioneFilesDirectory

            Dim Tutto As String = gf.LeggeFileIntero(Server.MapPath(".") & "\Thumbs\Imme.dat")
            Dim Campi() As String = Tutto.Split(Chr(251))
            Dim c As Integer = 1
            For Each ss As String In Campi
                Dim Campi2() As String = ss.Split(Chr(250))
                If Campi2.Length > 1 Then
                    If Campi2(1) <> "" Then
                        Dim Campi3() As String = Campi2(0).Split(Chr(249))
                        sNome(Campi3(0)) = Campi2(1)
                        indiceImm(campi3(0)) = Campi3(1)
                        c += 1
                    End If
                End If
            Next

            For i As Integer = 1 To quanteThumbs
                Dim im As ImageButton = FindControl("image" & i.ToString)
                If Not im Is Nothing Then
                    ' im.ImageUrl = "Thumbs/" & (i - 1).ToString & ".jpg?id=" & Now
                    im.ToolTip = indiceImm(i) & "-" & gf.TornaNomeFileDaPath(sNome(i))
                    im.Attributes.Add("src", "Thumbs/" & i.ToString & ".jpg?id=" & Now)
                    'im.Attributes.Add("ToolTip", indiceImm(i) & "-" & gf.TornaNomeFileDaPath(i))
                End If
            Next
        Else
            GeneraThumbs()
            CaricaThumbs()
        End If
    End Sub

    Protected Sub imgLinguetta_Click(sender As Object, e As ImageClickEventArgs) Handles imgLinguetta2.Click
        If Allargata = False Then
            divThumbs.Style.Add("display", "visible")
            'divFotografia.Style.Add("display", "none")
            Allargata = True

            CaricaThumbs()
        Else
            divThumbs.Style.Add("display", "none")
            'divFotografia.Style.Add("display", "visible")
            Allargata = False
        End If
    End Sub

    Protected Sub imgChiudeThumbs_Click(sender As Object, e As ImageClickEventArgs) Handles imgChiudeThumbs.Click
        divThumbs.Style.Add("display", "none")
        'divFotografia.Style.Add("display", "visible")
        Allargata = False
        'Dim sb1 As New StringBuilder

        'sb1.Append("<script type='text/javascript' language='javascript'>")
        'sb1.Append("     posizionaImm('" & hdnImmagine.Value & "');")
        'sb1.Append("</script>")

        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "posiImm", sb1.ToString(), False)
    End Sub

    'Protected Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
    'Timer1.Enabled = False

    'If Progressivi Is Nothing = True Then
    '    PrendeImmagine()
    'Else
    '    PrendeImmagine(Progressivi(Progressivo), True)
    'End If
    'End Sub
End Class