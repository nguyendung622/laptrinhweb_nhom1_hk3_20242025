var idCategory = 0;
var keyWord = "";
var pageIndex = 1;
$(document).ready(function () {
    var modal = new bootstrap.Modal("#noticeModal");
    modal.show();
});
$("#category").change(function () {
    idCategory = $(this).val();
    onLoad();
});
function onCreateProduct(event, form) {
    event.preventDefault();
    var formData = new FormData(form);
    console.log("Type: " + form.type);
    $.ajax({
        type: "post",
        url: form.action,
        data: formData,
        contentType: false,
        processData: false,
        success: function (mess) {
            //var modal = new bootstrap.Modal("#noticeModal");
            const modal = bootstrap.Modal.getInstance('#addModal');
            modal.hide();
            $("#toastMessage").html(mess.message);
            const myToast = bootstrap.Toast.getOrCreateInstance("#liveToast");
            myToast.show();
            onLoad();
        },
        error: function () {
            alert('Thêm thất bại');
        }
    });
    return false;
}

function loadViewUpdate(idProduct) {
    $("#contentViewUpdate").load("/Product/Update?id=" + idProduct);
}

function onSearch() {
    pageIndex = 1;
    keyWord = $("#keyWord").val();
    onLoad();
}
function onPaging(index) {
    pageIndex = index;
    onLoad();
}
function onLoad() {
    $("#product").load("/Product/LoadProduct?pageIndex=" + pageIndex
        + "&keyWord=" + keyWord + "&idCategory=" + idCategory);
}
function onEnterSearch(event) {
    if (event.key === "Enter") {
        event.preventDefault();
        onSearch();
    }
}