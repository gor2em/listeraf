

$(function () {

    //menu bg
    $(window).scroll(function () {
        if ($(this).scrollTop() > 50)
            $(".navbar-inverse").css("background-color", "#000")
                .css("transition-duration", "1s");
        else
            $(".navbar-inverse").css("background-color", "rgba(0, 0, 0, 0.57)");
    });


    ////how to work thumbnail & icon hover
    //$(".thumbnail i").mouseover(function () {
    //    $("i").css("border-bottom", "1px solid #ededed");
    //})

});