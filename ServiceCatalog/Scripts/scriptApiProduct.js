//IndexCallByItemManual
//search partno
function GetProductApiCount(Stkcode) {
    console.log("GetProductApiCount");
    try {
        $.ajax({
            type: 'POST',
            url: '@Url.Action("GetProductApiCount", "ApiProduct")',
            data: {
                Stkcode: Stkcode,
            },
            dataType: 'json',
            success: function (data) {
                console.log(data[0]);
                //return false
                $('#countProductDes').text(data[0].countProductDes);
                $('#countProductSpec').text(data[0].countProductSpec);
                $('#countProductImage').text(data[0].countProductImage);
                $('#countProductOem').text(data[0].countProductOem);
                $('#countProductCom').text(data[0].countProductCom);
                $('#countProductLinkage').text(data[0].countProductLinkage);
            }
        }).fail(function (error) {
            Swal.fire({
                icon: 'error',
                title: 'ระบบทำงานผิดพลาด()',
                html: 'กรุณาแจ้งผู้ดูแลระบบ<br>โดยการแคปหน้าจอและก๊อปปี้ Error ด่านล่างนี้<br>GetProductApiCount<br>' + error.responseText + '<br><br>',
                confirmButtonColor: '#d33',
                confirmButtonText: 'ปิด',
                allowOutsideClick: false,
                focusConfirm: false
            });
        });
    } catch (error) {
        Swal.fire({
            icon: 'error',
            title: 'ระบบทำงานผิดพลาด()',
            html: 'กรุณาแจ้งผู้ดูแลระบบ<br>โดยการแคปหน้าจอและก๊อปปี้ Error ด่านล่างนี้<br>GetProductApiCount<br>' + erroพ + '<br><br>',
            confirmButtonColor: '#d33',
            confirmButtonText: 'ปิด',
            allowOutsideClick: false,
            focusConfirm: false
        });
    }
}
//call api
function getArticlesByPartno() {
    var txtPartNo = $("#txtPartNo").val();
    var txtBrandId = $("#txtBrandId option:selected").val();
    var msg = '';
    var error = 0;
    if (txtPartNo == "" || txtBrandId == "") {
        ++error;
        msg += '<br>' + error + '. กรุณากรอก Partno';
    }
    if (error) {
        Swal.fire({
            icon: 'warning',
            title: 'กรุณาตรวจสอบข้อมูลต่อไปนี้',
            html: msg,
            confirmButtonColor: '#d33',
            confirmButtonText: 'Close',
            allowOutsideClick: false,
            focusConfirm: false,
        });
        return false;
    } else {
        //check call api
        $.ajax({
            url: '@Url.Action("checkApiDataByPartno", "ApiProduct")',
            data: {
                PartNo: txtPartNo.trim(),
                BrandId: txtBrandId
            },
            type: "POST",
            dataType: "JSON",
            beforeSend: function () {
                $('#showResultArticlesByPartno').empty();
                Swal.fire({
                    title: 'Searching...',
                    text: 'Please wait while we search for the product.',
                    allowOutsideClick: false,
                    didOpen: () => {
                        Swal.showLoading();
                    }
                });
            },
            success: function (data) {
                //console.log(data);
                //return false;
                if (data.statusCallApi) { //call success
                    if (data.MfrNames[0] != null) { //call find data
                        $.ajax({
                            url: '@Url.Action("getArticlesByPartno", "ApiProduct")',
                            type: 'POST',
                            data: {
                                PartNo: txtPartNo.trim(),
                                BrandId: txtBrandId
                            },
                            dataType: 'html',
                            cache: false,
                            Async: true,
                            beforeSend: function () {
                                $('#showResultArticlesByPartno').empty();
                                //$('#showResultArticlesByPartno').html('<div style="padding: 30px; font-size: 14px;"> <i class="fa fa-spinner fa-spin" style="font-size: 24px;" aria-hidden="true"></i> กำลังโหลดข้อมูล Api Product ... </div>');
                                //LoadingShow();
                                Swal.fire({
                                    title: 'Searching...',
                                    text: 'Please wait while we search for the product.',
                                    allowOutsideClick: false,
                                    didOpen: () => {
                                        Swal.showLoading();
                                    }
                                });
                            },
                            success: function (res) {
                                Swal.close();
                                $('#showResultArticlesByPartno').html(res);
                                setTimeout(function () {
                                    GetProductApiCount(txtPartNo.trim());
                                }, 1000);
                                //LoadingHide();
                            }
                        });
                    } else {    ////call not find data
                        Swal.fire({
                            icon: 'warning',
                            //title: res.msg,
                            html: 'Product information not found',
                            confirmButtonColor: '#d33',
                            confirmButtonText: 'Close',
                            allowOutsideClick: false,
                            focusConfirm: false,
                        });
                    }
                } else { //call fail api | not run
                    Swal.fire({
                        icon: 'error',
                        title: 'API service not run',
                        html: data.statusCode,
                        confirmButtonColor: '#d33',
                        confirmButtonText: 'Close',
                        allowOutsideClick: false,
                        focusConfirm: false,
                    });
                }
            }
        });
    }
}
//_listCallByItemManual
//btn save
SaveProductApiToDb = async () => {
    var msg = '';
    var error = 0;
    var txtPartNo = $("#txtPartNo").val();
    var txtBrandId = $("#txtBrandId option:selected").val();
    if (txtPartNo == "" || txtBrandId == "") {
        ++error;
        msg += '<br>' + error + '. กรุณากรอก Partno';
    }
    console.log("txtPartNo = " + txtPartNo);
    console.log("txtBrandId = " + txtBrandId);
    //return false;

    if (error) {
        Swal.fire({
            icon: 'warning',
            title: 'กรุณาตรวจสอบข้อมูลต่อไปนี้',
            html: msg,
            confirmButtonColor: '#d33',
            confirmButtonText: 'Close',
            allowOutsideClick: false,
            focusConfirm: false,
        });
        return false;
    } else {
        Swal.fire({
            title: 'Do you confirm save to db',
            // html: html,
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#00a65a',
            cancelButtonColor: '#d33',
            confirmButtonText: '<i class="fa fa-check"></i> Yes',
            cancelButtonText: '<i class="fa fa-times"></i> No',
            allowOutsideClick: false,
            focusConfirm: false,
        }).then((result) => {
            if (result.value) {
                $.ajax({
                    type: 'POST',
                    cache: false,
                    url: '@Url.Action("SaveProductApiToDb", "ApiProduct")',
                    data: {
                        PartNo: txtPartNo,
                        BrandId: txtBrandId
                    },
                    //contentType: "application/json; charset=utf-8",
                    //enctype: 'multipart/form-data',
                    beforeSend: function () {
                        LoadingShow();
                    },
                    success: function (result) {
                        if (result != null) {
                            Swal.fire({
                                title: 'Saved successfully',
                                //html: '',
                                icon: 'success',
                                confirmButtonColor: '#3085d6',
                                confirmButtonText: 'Close',
                                allowOutsideClick: false,
                                focusConfirm: false,
                                timer: 6000,
                                timerProgressBar: true,
                                onClose: () => {
                                    //$(window).unbind('beforeunload');
                                    //LoadingHide();
                                    //$('form[id="budgetPmForm"]').submit();
                                    ////location.reload();
                                }
                            });
                        } else {
                            Swal.fire({
                                icon: 'error',
                                //title: res.msg,
                                html: 'Error not saved',
                                confirmButtonColor: '#d33',
                                confirmButtonText: 'Close',
                                allowOutsideClick: false,
                                focusConfirm: false,
                            });
                        }
                    },
                    error: function (ex) {
                        //notify(ex, 'danger');
                    },
                    complete: function (res) {
                        LoadingHide();
                    }
                }).fail(function (error) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error not saved',
                        html: 'กรุณาแจ้งผู้ดูแลระบบ<br>โดยการแคปหน้าจอและก๊อปปี้ Error ด่านล่างนี้<br>saveExcess<br><br>',
                        confirmButtonColor: '#d33',
                        confirmButtonText: 'ปิด',
                        allowOutsideClick: false,
                        focusConfirm: false,
                    });
                });
            }
        });
    }

}