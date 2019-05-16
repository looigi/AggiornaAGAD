var oldImm;
var oldX;
var oldY;
var immAttuale;
var doit;
// var w;
// var h;

function posizionaImm(imm) {
    immAttuale = imm;
    //if (imm != null && imm!='') {
    //    oldImm = imm;
    //    oldX = x;
    //    oldY = y;
    //} else {
    //    imm = oldImm;
    //    x = oldX;
    //    y = oldY;
    //}

    var d = document.getElementById('divFotografia');
    var i = document.getElementById('imgImmagine');

    // var xi = d.offsetWidth; // - 30; //-250;
    // var yi = d.offsetHeight; // - 30; // -250;
    // 
    // var xd = w;
    // var yd = h;

    // alert('Imm: ' + xi + 'x' + yi + '\n\nDiv: ' + xd + 'x' + yd);

    // if (xi > xd || yi > yd) {
    //     var a1 = xi / xd;
    //     var a2 = yi / yd;
    //     var a;
    // 
    //     if (a1 > a2) {
    //         a = a1;
    //     } else {
    //         a = a2;
    //     }
    //     xi /= a;
    //     yi /= a;
    // }

    // var imm2 = imm;
    // while (imm2.indexOf("§") > -1) {
    //     imm2 = imm2.replace("§", "//");
    // }
    // alert(imm2);

    var suffix = Math.random(99999999);
    i.src = 'Appoggio/'+imm+'.Jpg?p=' + suffix;

    // i.style.width = xi + 'px';
    // i.style.height = yi + 'px';

    // var posx;
    // var posy;
    // 
    // posx = (xd / 2) - (xi / 2);
    // posy = (yd / 2) - (yi / 2);
    // 
    // i.style.marginLeft = posx + 'px';
    // i.style.marginTop = posy + 'px';
    PrendeDimensioniSchermo();
}

function ripristinacolore(i) {
    i.style.background = '#aca9a3';
}

function PrendeDimensioniSchermo() {
    var hdnDime = document.getElementById('hdnDimensioniSchermo');
    // var ratio = window.devicePixelRatio || 1;
    
    // ww = window.innerWidth; //  * ratio;
    // hh = window.innerHeight; //  * ratio;

    ww = window.innerWidth
    || document.documentElement.clientWidth
    || document.body.clientWidth;

    hh = window.innerHeight
    || document.documentElement.clientHeight
    || document.body.clientHeight;

    ww = Math.floor((ww * 98) / 100);
    hh = Math.floor((hh * 98) / 100);
    hh -= 80;

    // alert(ww + '-' + hh);
    hdnDime.value = ww + ';' + hh;

    clearTimeout(doit);
    doit = setTimeout(resizedw(ww,hh), 1000);
}

function resizedw(w,h) {
    var divCont = document.getElementById('divContainer');
    divCont.style.width = w + 'px !Important';
    divCont.style.height = h + 'px !Important';
    divCont.setAttribute('width', w);
    divCont.setAttribute('height', h);

    var img = document.getElementById('imgImmagine');
    img.style.width = w + 'px !Important';
    img.style.height = h + 'px !Important';
    img.setAttribute('width', w);
    img.setAttribute('height', h);

    //var mh = (h / 2) - (img.style.height / 2);

    // alert(h/2 + "-" + mh);

    //img.style.marginTop = mh + 'px;';
    // alert(w + "-" + h);
}

function changecolor(i) {
    i.style.backgroundColor = '#aaaa00';
}

function Schiarisce(chi) {
    var d = document.getElementById(chi);

    d.style.opacity = .45; //For real browsers;
    d.style.filter = "alpha(opacity=25)"; //For IE;
}

function Scurisce(chi) {
    var d = document.getElementById(chi);

    d.style.opacity = .95; //For real browsers;
    d.style.filter = "alpha(opacity=95)"; //For IE;
}

function messaggio(msg) {
    alert(msg);
}
