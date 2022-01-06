$(function () {
    //load lan

    var vi = getCookie("Language");
    if (vi !== "" ) {
        console.log(vi);
        GetLanguage();
        if (vi === "en-GB") {
            $('#menucontrol p.langMenu').addClass('hasLeft');

        } else {
            $('#menucontrol p.langMenu').removeClass('hasLeft');
        }
    }
    else {
        setCookie("Language", "en-GB", 365);
        GetLanguage();

        $('#menucontrol p.langMenu').addClass('hasLeft');
    }


    //setTimeout(function () {
    //    $('.detail-category').find('.number1').removeClass('top_double');
    //}, 1500);
    //setTimeout(function () {
    //    $('.detail-category').find('.number2').removeClass('top_double');
    //}, 3000);
    var action = $('#hdAction').val();
    if (action === "Index") {

        $('body').addClass('index');
    } else {
        $('body').attr('id', 'lifestyle_detail');
    }

});



function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}
function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

$('.btn-language').on('click', function () {

    var lang = $(this).attr('data-language');
    setCookie("Language", lang, 365);

    location.reload();
});
$('#lang_button').on('click', function () {
    var time = 15;


    var t = setInterval(function () {

        time = time - 1;
        if (time === 0) {
            $('#lang_button').removeClass('active');
            clearInterval(t);
        };
        console.log(time);
    }, 1000);


});

function GetLanguage() {
    var url = "/Home/GetLanguage";
    $.ajax
   ({
       type: "POST",
       url: url, //LyricsloadMore
       data: '',
       dataType: "json",
       contentType: "application/json;charset=utf-8",
       success: function (data) {
           console.log(data);

           $.each(data, function (i, o) {
               if (o.Code.lastIndexOf("ip_") != -1)
                   $('.' + o.Code).val(o.Name);
               else if (o.Code.lastIndexOf("pl_") != -1)
                   $('.' + o.Code).attr("placeholder", o.Name);
               else
                   $('.' + o.Code).html(o.Name);

           });


       }
   });
}

var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
/*$(document).on('click', '.btnCross', function () {
    var t = $(this);
    if ($('.menu_panel').hasClass('opened') == true) {
        setTimeout(function () {
            
            $('.opened').not(t.parents()).eq(0).find('.btnCrossHide').click();
        }, 800);
           
    }
});*/
