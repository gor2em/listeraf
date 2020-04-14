//izlenenler

let id = $(".userId").text();
function izlediklerim() {

    let url = "/User/Izlediklerim/" + id;
    $("div#partial-izlenenler").html();
    var thead = "<div class='text-center center-block'>";
    $('div#partial-izlenenler').append(thead);
    $('div#partial-izlenenler').html("<h2>İzlenenler</h2>");
    $.getJSON(url, function (data) {
        for (let item in data.Result) {
            let deger = "<div class='col-lg-1  col-sm-1 col-xs-2 img-margin-movies kapsayici'><a href=/Filmler/Detay/" + data.Result[item].id + " > <img src=" + data.Result[item].poster_path + " class='img-circle img-profil-movies' width='110' height='110' /></a><div class='overlay'><p>" + data.Result[item].title + "</p><br><a href='#'><i class='fas fa-minus-circle mDel modalAc' data-imdb_id='" + data.Result[item].imdb_id + "'></i></a></div></div>";
            $('div#partial-izlenenler').append(deger);
        }
    });
};
function begendiklerim() {
    let url = "/User/Begendiklerim/" + id;
    $("div#partial-begenilenler").html();
    var thead = "<div class='text-center center-block'>";
    $('div#partial-begenilenler').append(thead);
    $('div#partial-begenilenler').html("<h2>Beğenilenler</h2>");
    $.getJSON(url, function (data) {
        for (let item in data.Result) {
            let deger = "<div class='col-lg-1  img-margin-movies kapsayici'><a href=/Filmler/Detay/" + data.Result[item].id + " > <img src=" + data.Result[item].poster_path + " class='img-circle img-profil-movies' width='110' height='110' /></a><div class='overlay'><p>" + data.Result[item].title + "</p><br><a href='#'><i class='fas fa-minus-circle mDel modalAc' data-imdb_id='" + data.Result[item].imdb_id + "'></i></a></div></div>";
            $('div#partial-begenilenler').append(deger);
        }
    });
};
function DahaSonraIzle() {
    let url = "/User/DahaSonraIzle/" + id;
    $("div#partial-DHS").html();
    var thead = "<div class='text-center center-block'>";
    $('div#partial-DHS').append(thead);
    $('div#partial-DHS').html("<h2>Daha sonra izle</h2>");
    $.getJSON(url, function (data) {
        for (let item in data.Result) {
            let deger = "<div class='col-lg-1 img-margin-movies kapsayici'><a href=/Filmler/Detay/" + data.Result[item].id + " > <img src=" + data.Result[item].poster_path + " class='img-circle img-profil-movies' width='110' height='110' /></a><div class='overlay'><p>" + data.Result[item].title + "</p><br><a href='#'><i class='fas fa-minus-circle mDel modalAc' data-imdb_id='" + data.Result[item].imdb_id + "'></i></a></div></div>";
            $('div#partial-DHS').append(deger);
        }
    });
};
function Yorumlar() {
    let url = "/User/Yorumlar/" + id;
    $("div#partial-yorumlar").html();
    var thead = "<div class='text-center center-block'>";
    $('div#partial-yorumlar').append(thead);
    $('div#partial-yorumlar').html("<h2>Yorumlar</h2>");
    $.getJSON(url, function (data) {
        for (let item in data.Result) {
            let deger = "<div class='col-lg-12'>" +
                "<div class='col-lg-8'><h5>" + data.Result[item].UserName + "</h5></div>" +
                "<div class='col-lg-4 text-right'><p><a href='/Filmler/Detay/" + data.Result[item].id + "' class='label label-danger'>" + data.Result[item].title + "</a></p></div>" +
                "<div class='col-lg-12' style='text-align:justify;'><p style='border-bottom:1px solid #757575;color:#ededed;'>" + data.Result[item].YorumIcerik + "</p></div>" +
                "</div>";
            $('div#partial-yorumlar').append(deger);
        }
    });
};


/*menu-fixed*/
var nav = $('#profil-menu-container');
$(window).scroll(function () {
    if ($(this).scrollTop() > 80) {
        nav.addClass("f-nav");
    } else {
        nav.removeClass("f-nav");
    }
});
/***/



/*call the functions*/
$("#tog-izlenenler").click(function () {
    $('html, body').animate({
        scrollTop: $("#partial-izlenenler").offset().top - 70
    }, 1000);
    izlediklerim();
});

$("#tog-begenilenler").click(function () {

    $('html, body').animate({
        scrollTop: $("#partial-begenilenler").offset().top - 70
    }, 1000);
    begendiklerim();
});

$("#tog-dhs").click(function () {
    $('html, body').animate({
        scrollTop: $("#partial-DHS").offset().top - 70
    }, 1000);
    DahaSonraIzle();
});
$("#tog-yorumlar").click(function () {
    $('html, body').animate({
        scrollTop: $("#partial-yorumlar").offset().top - 70
    }, 1000);
    Yorumlar();
});


/***/



/*silme işlemi*/
let userId = $(".takeUserId").text();
$(function () {
    const usID = $(".userId").text();
    const tuID = $(".takeUserId").text();
    $(document).on("mouseover", ".overlay i", function () {
        if (usID != tuID) {
            $(this).hide();
        }
    });
    $(document).on("click", "#partial-izlenenler .overlay a i", function () {
        let imdb_id = $(this).data('imdb_id');/*important*/
        $.ajax({
            type: "POST",
            url: "/User/IzSil",
            data: { userId: userId, imdb_id: imdb_id },
            success: function (durum) {
                if (durum == "true") {
                    izlediklerim();
                } else {
                    alert("hata");
                }
            }

        });
    });
    $(document).on("click", "#partial-begenilenler .overlay a i", function () {
        let imdb_id = $(this).data('imdb_id');/*important*/

        $.ajax({
            type: "POST",
            url: "/User/BeSil",
            data: { userId: userId, imdb_id: imdb_id },
            success: function (durum) {
                if (durum == "true") {
                    begendiklerim();
                } else {
                    alert("hata");
                }
            }

        });
    });
    $(document).on("click", "#partial-DHS .overlay a i", function () {
        let imdb_id = $(this).data('imdb_id');/*important*/
        $.ajax({
            type: "POST",
            url: "/User/DHSSil",
            data: { userId: userId, imdb_id: imdb_id },
            success: function (durum) {
                if (durum == "true") {
                    DahaSonraIzle();
                } else {
                    alert("hata");
                }
            }

        });
    });

});
