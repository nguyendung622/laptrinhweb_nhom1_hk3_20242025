
$(document).ready(function () {
    var modal = new bootstrap.Modal("#noticeModal");
    modal.show();
});
$("#category").change(function () {
    var idCategory = $(this).val();
    $("#product").load("/Product/LoadProduct?idCategory=" + idCategory);
});
