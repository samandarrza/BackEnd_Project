$(document).on("click", ".add-to-basket", function (e) {
    e.preventDefault();

    let url = $(this).attr("href");

    fetch(url)
        .then(response => {
            if (!response.ok) {
                toastr["error"]("Sold!")
                return;
            }
            else {
                toastr["error"]("Added!")
                return response.text();
            }
        }).then(html => {
            $("#basket-block").html(html)
        }).then(() => window.location.reload())
})
//$(document).on("click", ".book-modal-btn", function (e) {
//    e.preventDefault()

//    let url = $(this).attr("href");

//    fetch(url).then(response => response.text())
//        .then(data => {
//            $("#quickModal .modal-dialog").html(data)
//        })
//    $("#quickModal").modal("show")
//})