
$(document).ready(function () {
    var modal = new bootstrap.Modal("#noticeModal");
    modal.show();
});
$("#category").change(function () {
    var idCategory = $(this).val();
    $("#product").load("/Product/LoadProduct?idCategory=" + idCategory);
});
function onCreateProduct(form) {
    e.preventDefault();
    var formData = FormData(form);
    $.ajax({
        type: form.type,
        url: form.action,
        data: formData,
        success: function (mess) {
            alert('Thêm thành công');
            var idCategory = $("#category").val();
            $("#product").load("/Product/LoadProduct?idCategory=" + idCategory);
        },
        error: function () {
            alert('Thêm thất bại');
        }
    });
    return false;
}
