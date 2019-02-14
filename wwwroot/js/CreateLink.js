$(document).ready(function () {
    let fullUrl = $("#fullUrl");
    let btnShort = $("#btnShort");
    let form = $("#formFullUrl");
    btnShort.click(function () {
        if (form.valid()) {
            let link = fullUrl.val();
            jQuery.ajax({
                method: "POST",
                url: "Links/Create",
                data: {
                    "FullUrl": link
                },
                success: function (id) {
                    let shortLink = "https://localhost:5001/" + id;
                    fullUrl.val(shortLink);
                    btnShort.html("Copy");
                    btnShort.off("click");
                    btnShort.click(function () {
                        copyToClipboard(shortLink);
                        btnShort.html("Copied");
                        btnShort.attr("disabled");
                    });
                },
                error: function (error) {
                    var obj = JSON.parse(error.responseText);
                }
            });
        }
    });

    function copyToClipboard(Value) {
        var aux = document.createElement("input");
        aux.setAttribute("value", Value);
        document.body.appendChild(aux);
        aux.select();
        document.execCommand("copy");
        document.body.removeChild(aux);
    }

});