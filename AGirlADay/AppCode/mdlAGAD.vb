Module mdlAGAD
    'Public NomecartellaVirtuale As String = "Immagini"
    Public qImmagini As Long
    'Public Immagini() As String
    Public Progressivo As Long
    Public Progressivi() As String
    Public NumeroImmagine As Long
    'Public Percorso As String
    Public NomeFiletto As String
    ' Public NomeFilettoProgr As String
    Public Allargata As Boolean = False

    Public quanteThumbs As Integer = 24
    'Public quanteThumbsY As Integer = 4

    Public Percorso As String
    Public PercorsoIIS As String

    Private bScriveLog As Boolean = False

    Public Sub ScriveLog(Log As String)
        If bScriveLog Then
            Dim gf As New GestioneFilesDirectory
            gf.ApreFileDiTestoPerScrittura(HttpContext.Current.Server.MapPath(".") & "\Log\Log.txt")
            gf.ScriveTestoSuFileAperto(Now & ": " & Log)
            gf.ChiudeFileDiTestoDopoScrittura()
            gf = Nothing
        End If
    End Sub
End Module
