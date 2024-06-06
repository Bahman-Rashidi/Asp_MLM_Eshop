function getSelectedIds() {
    return $('.col-xs-4 .prod-id').map(function () { return $(this).text(); }).toArray();
}

function updateLinkAndCounter() {
    var ids = getSelectedIds().map(function (x, i) { return ['P', ++i, '=', x].join(''); });
    $('#comp > a').attr('href', '/AdminProducts/' + 'Compare?' + ids.join('&'));
    $(".count").text(ids.length == 1 ? '1 Item in list.' : ids.length + ' Item in list');
    $("#delete").removeAttr("href");
}

$(".more").click(function () {
    $("#container").show();
    var id = $(this).next('.ProdId').text();
    var img = $('.appendimg').attr('src');

    var selected = getSelectedIds();
    if (selected.length == 4) {
        alert("امکان add  کالای پنجم وجود ندارد");
        return;
    }
    if (selected.indexOf(id) != -1) {
        alert("آیتم قبلا اضافه شده است");
        return;
    }

    $('<div/>', { 'class': 'col-xs-4 col-md-2' })
       .append($('<span/>', { class: 'prod-id', text: id }))
       .append($('<a/>', { href: '#', class: 'close' }).append($('<i/>', { 'class': 'fa fa-times' })))
       .append($('<img/>', { src: img, class: 'img-thumbnail' }))
       .appendTo('#container');

    updateLinkAndCounter();
    $("#container").removeClass("hide");
});

$("body").on("click", ".close", function () {
    $(this).parent().remove();
    updateLinkAndCounter();
    var selected = getSelectedIds();
    if (selected.length == 0) {
        $("#container").hide();
    }
});

$("#compare").click(function () {
    var selected = getSelectedIds();
    if (selected.length == 1) {
        $("#compare").removeAttr("href");
        alert("برای مقایسه حداقل 2 کالا را انتخاب کنید");
    }
});

$("#delete").click(function () {
    $("* .close").parent().remove();
    updateLinkAndCounter();
    var selected = getSelectedIds();
    $("#container").hide();
});

$('#captcha').children('a').css('display', 'block');