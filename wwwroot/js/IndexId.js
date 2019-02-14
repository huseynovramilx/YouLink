$(document).ready(function () {
    $("#btnSkip").click(function () {
        console.log("ok");
        let link = $("#fullUrl").val();
        console.log(link);
        window.location = link;
    });

    function enableBtnSkip() {
        $("#btnSkip").removeAttr("disabled");
        $('#btnSkip').removeClass("disabled");
    }
    let sec = 10;
    let countDown = setInterval(function time() {
        
        if (sec === 0) {
            clearInterval(countDown);
            enableBtnSkip();
        }
        jQuery('#countdown #sec').html(sec);
        sec--;

    }, 1000); 

});

