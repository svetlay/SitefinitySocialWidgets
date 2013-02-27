$(document).ready(function () {
    var el = kendo.fx($("#album")),
    flip = el.flip("horizontal", $("#registrationButton"), $("#registrationWidget")),
    zoom = el.zoomIn().startValue(1).endValue(0.7);

    flip.add(zoom).duration(200);

    $("#registrationButton").click(function () {

        flip.stop().play();
    });

    $("#registrationWidget").click(function () {
        flip.stop().reverse();
    });

    $(".submitButton").click(function () {
        $.ajax({
            type: 'POST',
            url: '/mvc/SubmitResgistration',
            data: { "referrer": window.location },
            dataType: "json"

        });
    });

});