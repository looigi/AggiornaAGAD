<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="AGirlADay._Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>AGirlADay</title>

    <link rel="shortcut icon" href="App_Themes/Standard/Images/favicon.ico" />
    <link href="App_Themes/Standard/AGirlADay.css" rel="stylesheet" />
    <script src="Js/AGAD.js"></script>

<%--    <link href="App_Themes/Standard/Griglie.css" rel="stylesheet" />--%>

    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css" integrity="sha384-Gn5384xqQ1aoWXA+058RXPxPg6fy4IWvTNh0E263XmFcJlSAwiGgFAW/dAiS6JXm" crossorigin="anonymous" />

    <script src="https://code.jquery.com/jquery-3.2.1.slim.min.js" integrity="sha384-KJ3o2DKtIkvYIK3UENzmM7KCkRr/rE9/Qpg6aAZGJwFDMVNA/GpGFF93hXpG5KkN" crossorigin="anonymous"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.12.9/umd/popper.min.js" integrity="sha384-ApNbgh9B+Y1QKtv3Rn7W3mgPxhU9K/ScQsAP7hUibX39j7fakFPskvXusvfa0b4Q" crossorigin="anonymous"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js" integrity="sha384-JZR6Spejh4U02d8jOt6vLEHfe/JQGiRRSQQxSfFWpi1MquVdAyjUar5+76PVCmYl" crossorigin="anonymous"></script>
</head>

<body style="background-color: #000000;" onload="PrendeDimensioniSchermo();" onresize="PrendeDimensioniSchermo();">
    <!-- -->
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <%--<asp:Timer ID="Timer1" runat="server" Enabled="False" Interval="500"></asp:Timer>--%>
        <asp:UpdatePanel id="uppConferma" runat="server" updatemode="Conditional">
            <ContentTemplate>
                <asp:HiddenField ID="hdnDimensioniSchermo" runat="server" />

                    <header>
                      <nav class="navbar navbar-expand-md navbar-dark fixed-top bg-dark">
                        <a class="navbar-brand" href="#">A Girl A Day</a>

                        <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarCollapse" aria-controls="navbarCollapse" aria-expanded="false" aria-label="Toggle navigation">
                          <span class="navbar-toggler-icon"></span>
                        </button>

                        <div class="collapse navbar-collapse" id="navbarCollapse">
                            <ul class="navbar-nav mr-auto">
                                <!-- <li class="nav-item active col-2">
                                    <asp:ImageButton ID="imgIndietro2" class="nav-link" runat="server" ImageUrl="App_Themes/Standard/Images/icona_INDIETRO.png" Width="60" Height="60" tooltip="Indietro"/>
                                </li> -->
                                <li class="nav-item active col-3">
                                    <asp:ImageButton ID="imgElimina2" class="nav-link" runat="server" ImageUrl="App_Themes/Standard/Images/icona_ELIMINA-TAG.png" Width="60" Height="60"  tooltip="Elimina l'immagine"/>
                                </li>
                                <li class="nav-item active col-3">
                                    <asp:ImageButton ID="imgRefresh2" class="nav-link" runat="server" ImageUrl="App_Themes/Standard/Images/icona_REFRESH.png"  Width="60" Height="60" tooltip="Refresh"/>
                                </li>
                                <li class="nav-item active col-3">
                                    <asp:ImageButton ID="imgLinguetta2" class="nav-link" runat="server" ImageUrl ="App_Themes/Standard/Images/icona_CERCA.png" Width="60" Height="60" tooltip="Thumbs" />
                                </li>
                                <!-- <li class="nav-item active col-2">
                                    <asp:ImageButton ID="imgAvanti2" class="nav-link" runat="server" ImageUrl="App_Themes/Standard/Images/icona_AVANTI.png" Width="60" Height="60" tooltip="Avanti"/>
                                </li> -->
                            </ul>
                        </div>
                      </nav>
                    </header>

                    <div id="divContainer" style="text-align: center; margin-top: 75px;">
                        <asp:Image ID="imgImmagine" style="object-fit: contain;" runat="server" />
                    </div>

                    <div style="position: absolute; top: 70px;">
                        <asp:Label ID="lblNomeImmagine2" class="nav-link" runat="server" Text="" Font-Names="Arial" Font-Size="Medium" ForeColor="#FFFFFF" ></asp:Label>
                    </div>
               <div style="width:80px; position: absolute; left: 1px; z-index: 1000; top: 50%; margin-top: -40px;">
                    <asp:ImageButton ID="imgIndietro" runat="server" ImageUrl="App_Themes/Standard/Images/icona_INDIETRO.png" Width="80" Height="80" tooltip="Indietro"/>
                </div>

                <!-- <div id="divTasti" runat="server" style="position: absolute; top: 1px; left: 50%; margin-left: -75px; height: 55px; opacity: .25;"  onmouseout ="Schiarisce('divTasti');" onmouseover ="Scurisce('divTasti');">
                    <asp:ImageButton ID="imgElimina" runat="server" ImageUrl="App_Themes/Standard/Images/icona_ELIMINA-TAG.png"  Width="50" Height="50" tooltip="Elimina l'immagine"/>
                    <asp:ImageButton ID="imgRefresh" runat="server" ImageUrl="App_Themes/Standard/Images/icona_REFRESH.png"  Width="50" Height="50" tooltip="Refresh"/>
                </div>-->
                <div style="width:80px; position: absolute; right: 1px; text-align:right; z-index: 1000; top: 50%; margin-top: -40px;">
                    <asp:ImageButton ID="imgAvanti" runat="server" ImageUrl="App_Themes/Standard/Images/icona_AVANTI.png" Width="80" Height="80" tooltip="Avanti"/>
                </div>

                <div id="divPopup" runat="server">
                    <div id="Div4" class="bloccafinestra" runat="server"></div>
    
                    <div class="popup draggable" runat="server">
                        <ul id="ulPopup" runat="server">
                        </ul>

                        <asp:Button id="cmdOK" CssClass="bottone" runat="server" Text="OK" />
                    </div>

                    <div class="clear"></div>
                </div>

                <asp:HiddenField ID="hdnNomeImmagine" runat="server" />

                <!-- <div id="divNome" runat="server" class="nomeimmagine doves" onmouseout ="Schiarisce('divNome');" onmouseover ="Scurisce('divNome');">
                    <asp:Label ID="lblNomeImmagine" runat="server" Text="Label" Font-Names="Arial" Font-Size="Medium" ForeColor="#FFFFFF" ></asp:Label>
                </div> -->

                <div id="divThumbs" runat="server" style="position: absolute; left: 5%; top: 5%; width: 90%; height: 90vh; background-color: #AAAAAA; border: 1px solid #777777; z-index: 9999;">
                    <div id="divThumbsDetail" runat="server" style="position: absolute; left: 3px; top: 3px; width: 99%; height: 99vh; overflow: auto;">
                        <%--<asp:Literal ID="ltlThumbs" runat="server"></asp:Literal>--%>

                        <div class="divvinoThumbs" onmouseover="changecolor(this);" onmouseout ="ripristinacolore(this);">
                            <asp:ImageButton ID="Image1" runat="server" Src="" ToolTip="" Width="95%" Height="80%" OnClick ="clickimm" class="mx-auto d-block" />
                        </div>
                        <div class="divvinoThumbs" onmouseover="changecolor(this);" onmouseout ="ripristinacolore(this);">
                            <asp:ImageButton ID="Image2" runat="server" Src="" ToolTip="" Width="95%" Height="80%" OnClick ="clickimm" class="mx-auto d-block" />
                        </div>
                        <div class="divvinoThumbs" onmouseover="changecolor(this);" onmouseout ="ripristinacolore(this);">
                            <asp:ImageButton ID="Image3" runat="server" Src="" ToolTip="" Width="95%" Height="80%" OnClick ="clickimm" class="mx-auto d-block" />
                        </div>
                        <div class="divvinoThumbs" onmouseover="changecolor(this);" onmouseout ="ripristinacolore(this);">
                            <asp:ImageButton ID="Image4" runat="server" Src="" ToolTip="" Width="95%" Height="80%" OnClick ="clickimm" class="mx-auto d-block" />
                        </div>
                        <div class="divvinoThumbs" onmouseover="changecolor(this);" onmouseout ="ripristinacolore(this);">
                            <asp:ImageButton ID="Image5" runat="server" Src="" ToolTip="" Width="95%" Height="80%" OnClick ="clickimm" class="mx-auto d-block" />
                        </div>
                        <div class="divvinoThumbs" onmouseover="changecolor(this);" onmouseout ="ripristinacolore(this);">
                            <asp:ImageButton ID="image6" runat="server" Src="" ToolTip="" Width="95%" Height="80%" OnClick ="clickimm" class="mx-auto d-block" />
                        </div>
                        <div class="divvinoThumbs" onmouseover="changecolor(this);" onmouseout ="ripristinacolore(this);">
                            <asp:ImageButton ID="Image7" runat="server" Src="" ToolTip="" Width="95%" Height="80%" OnClick ="clickimm" class="mx-auto d-block" />
                        </div>
                        <div class="divvinoThumbs" onmouseover="changecolor(this);" onmouseout ="ripristinacolore(this);">
                            <asp:ImageButton ID="Image8" runat="server" Src="" ToolTip="" Width="95%" Height="80%" OnClick ="clickimm" class="mx-auto d-block" />
                        </div>
                        <div class="divvinoThumbs" onmouseover="changecolor(this);" onmouseout ="ripristinacolore(this);">
                            <asp:ImageButton ID="Image9" runat="server" Src="" ToolTip="" Width="95%" Height="80%" OnClick ="clickimm" class="mx-auto d-block" />
                        </div>
                        <div class="divvinoThumbs" onmouseover="changecolor(this);" onmouseout ="ripristinacolore(this);">
                            <asp:ImageButton ID="Image10" runat="server" Src="" ToolTip="" Width="95%" Height="80%" OnClick ="clickimm" class="mx-auto d-block" />
                        </div>
                        <div class="divvinoThumbs" onmouseover="changecolor(this);" onmouseout ="ripristinacolore(this);">
                            <asp:ImageButton ID="Image11" runat="server" Src="" ToolTip="" Width="95%" Height="80%" OnClick ="clickimm" class="mx-auto d-block" />
                        </div>
                        <div class="divvinoThumbs" onmouseover="changecolor(this);" onmouseout ="ripristinacolore(this);">
                            <asp:ImageButton ID="Image12" runat="server" Src="" ToolTip="" Width="95%" Height="80%" OnClick ="clickimm" class="mx-auto d-block" />
                        </div>
                        <div class="divvinoThumbs" onmouseover="changecolor(this);" onmouseout ="ripristinacolore(this);">
                            <asp:ImageButton ID="Image13" runat="server" Src="" ToolTip="" Width="95%" Height="80%" OnClick ="clickimm" class="mx-auto d-block" />
                        </div>
                        <div class="divvinoThumbs" onmouseover="changecolor(this);" onmouseout ="ripristinacolore(this);">
                            <asp:ImageButton ID="Image14" runat="server" Src="" ToolTip="" Width="95%" Height="80%" OnClick ="clickimm" class="mx-auto d-block" />
                        </div>
                        <div class="divvinoThumbs" onmouseover="changecolor(this);" onmouseout ="ripristinacolore(this);">
                            <asp:ImageButton ID="Image15" runat="server" Src="" ToolTip="" Width="95%" Height="80%" OnClick ="clickimm" class="mx-auto d-block" />
                        </div>
                        <div class="divvinoThumbs" onmouseover="changecolor(this);" onmouseout ="ripristinacolore(this);">
                            <asp:ImageButton ID="Image16" runat="server" Src="" ToolTip="" Width="95%" Height="80%" OnClick ="clickimm" class="mx-auto d-block" />
                        </div>
                        <div class="divvinoThumbs" onmouseover="changecolor(this);" onmouseout ="ripristinacolore(this);">
                            <asp:ImageButton ID="Image17" runat="server" Src="" ToolTip="" Width="95%" Height="80%" OnClick ="clickimm" class="mx-auto d-block" />
                        </div>
                        <div class="divvinoThumbs" onmouseover="changecolor(this);" onmouseout ="ripristinacolore(this);">
                            <asp:ImageButton ID="Image18" runat="server" Src="" ToolTip="" Width="95%" Height="80%" OnClick ="clickimm" class="mx-auto d-block" />
                        </div>
                        <div class="divvinoThumbs" onmouseover="changecolor(this);" onmouseout ="ripristinacolore(this);">
                            <asp:ImageButton ID="Image19" runat="server" Src="" ToolTip="" Width="95%" Height="80%" OnClick ="clickimm" class="mx-auto d-block" />
                        </div>
                        <div class="divvinoThumbs" onmouseover="changecolor(this);" onmouseout ="ripristinacolore(this);">
                            <asp:ImageButton ID="Image20" runat="server" Src="" ToolTip="" Width="95%" Height="80%" OnClick ="clickimm" class="mx-auto d-block" />
                        </div>
                        <div class="divvinoThumbs" onmouseover="changecolor(this);" onmouseout ="ripristinacolore(this);">
                            <asp:ImageButton ID="Image21" runat="server" Src="" ToolTip="" Width="95%" Height="80%" OnClick ="clickimm" class="mx-auto d-block" />
                        </div>
                        <div class="divvinoThumbs" onmouseover="changecolor(this);" onmouseout ="ripristinacolore(this);">
                            <asp:ImageButton ID="Image22" runat="server" Src="" ToolTip="" Width="95%" Height="80%" OnClick ="clickimm" class="mx-auto d-block" />
                        </div>
                        <div class="divvinoThumbs" onmouseover="changecolor(this);" onmouseout ="ripristinacolore(this);">
                            <asp:ImageButton ID="Image23" runat="server" Src="" ToolTip="" Width="95%" Height="80%" OnClick ="clickimm" class="mx-auto d-block" />
                        </div>
                         <div class="divvinoThumbs" onmouseover="changecolor(this);" onmouseout ="ripristinacolore(this);">
                            <asp:ImageButton ID="image24" runat="server" Src="" ToolTip="" Width="95%" Height="80%" OnClick ="clickimm" class="mx-auto d-block" />
                        </div>
                   </div>
                    <div style ="position: absolute; top: 5px; right: 55px;">
                        <asp:ImageButton ID="imgRefreshThumbs" runat="server" ImageUrl="App_Themes/Standard/Images/icona_REFRESH.png"  Width="50" Height="50" tooltip="Refresh"/>
                    </div>
                    <div style ="position: absolute; top: 5px; right: 5px;">
                        <asp:ImageButton ID="imgChiudeThumbs" runat="server" ImageUrl="App_Themes/Standard/Images/elimina_quadrato.png"  Width="50" Height="50" tooltip="Indietro"/>
                    </div>
                </div>

             </ContentTemplate>
        </asp:UpdatePanel>
        
        <asp:UpdateProgress id="uppAttenderePrego" runat="server" AssociatedUpdatePanelid="uppConferma" DisplayAfter="1">
            <ProgressTemplate>
                <div id="Div1" class="bloccafinestra" runat="server"></div>
                    
                <div id="Div2" class="popup draggable" runat="server">
                    <p class="icona_loading">
                    <img src="App_Themes/Standard/Images/loading.gif" />&nbsp;
                    <span class="etichettapopup">Elaborazione in corso...</span>
                    </p>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>

        <!-- <div class="Linguetta" id="divLinguetta" runat="server">
            <div style="width: 30px; float: left; height: 50px;">
                <asp:ImageButton ID="imgLinguetta" runat="server" ImageUrl ="App_Themes/Standard/Images/icona_SINISTRA_mezza.png" style="height: 85px" />
            </div>
        </div>

        <asp:HiddenField ID="hdnImmaginePass" runat="server" />
        <div style="position: absolute; left: -100px; top: -100px;">
            <asp:Button ID="btnRefreshThumbs" runat="server" Text="Refresh Testo" />
        </div> -->
    </form>
</body>
</html>
