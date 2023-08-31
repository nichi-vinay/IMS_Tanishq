$(function () {
    $('#Company').val('1');
    $('#JewelType').val('2');
    $('#Supplier').val('1003');
    $('#Category').val('1050');
    $('#Description').val('');
    $('#filterStatus').val('');
    loadInventoryData();
    inventoryFormSubmit();
    validateInventoryForm();
    inventoryDeleteForm();
    
});

function loadInventoryData() {
    
    $('#InventoryTable').DataTable({
        "bServerSide": true,
        "sAjaxSource": '/InventoryMaster/GetInventoryJsonData',

        "sAjaxDataProp": "aaData",
        "bProcessing": true,
        "dom": "<'row'<'col-sm-4'l><'col-sm-4'><'col-sm-4'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        "fnServerData": function (sSource, aoData, fnCallback) {
            aoData.push({ "name": "categorySearch", "value": $('#filterCategory').val() });
            aoData.push({ "name": "suplierSearch", "value": $('#filterSupplier').val() });
            aoData.push({ "name": "statusSearch", "value": $('#filterStatus').val() });
            aoData.push({ "name": "inventorySearch", "value": $('#filterInventoryStatus').val() });
            aoData.push({ "name": "statusSearchId", "value": $('#filterSearchId').val() });
            $.ajax({
                type: "POST",
                data: aoData,
                url: sSource,
                success: fnCallback
            })
        },
        "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]],

        "aoColumns": [
            { "data": "Id" },
            { "data": "JewelType" },
            { "data": "CaratWeight" },
            { "data": "GoldWeight" },
            { "data": "Pieces" },
            { "data": "Description" },
            { "data": "Company" },
            { "data": "Supplier" },
            { "data": "Price" },
            { "data": "DateReceived" },
            { "data": "InventoryStatus" },
            { "data": "StatusName" },
            {
                "data": null,
                "orderable": false,
                "mRender": function () {
                    return '<a id="tooltip" data-toggle="tooltip" data-placement="top" title="View Details"><button class="btn btn-primary btn-icon" onclick="findInventoryAjax(this)" data-toggle="modal" data-target="#viewimageModalPopup"><i class="fas fa-eye fa-xs"></i></button></a>&nbsp;<a id="tooltip" data-toggle="tooltip" data-placement="top" onclick="GetBarcodeId(this)" title="Display Barcode"><button class="btn btn-primary btn-icon" ><i class="fas fa-barcode"></i></button></a >&nbsp<a id="tooltip" data-toggle="tooltip" data-placement="top" title="Edit Inventory"><button class="btn btn-primary btn-icon" onclick="editInventoryFunction(this)" data-toggle="modal" data-target="#InventoryModalPopup"><i class="fas fa-edit fa-xs"></i></button></a>&nbsp<a id="tooltip" data-toggle="tooltip" data-placement="top" title="Delete Inventory"><button class="btn btn-danger" onclick="deleteInventoryFunction(this)" data-toggle="modal" data-target="#inventoryDeleteModalPopup"><i class="fas fa-trash fa-xs"></i></button></a>';
                }
            }
        ],
        
        "columnDefs": [
            { "width": "30px", "targets": 0 },
            { "width": "20px", "targets": 1 },
            { "width": "20px", "targets": 2 },
            { "width": "20px", "targets": 3 },
            { "width": "20px", "targets": 4 },
            { "width": "120px", "targets": 5 },
            { "width": "60px", "targets": 6 },
            { "width": "50px", "targets": 7 },
            { "width": "40px", "targets": 8 },
            { "width": "40px", "targets": 9 },
            { "width": "30px", "targets": 10 },
            { "width": "30px", "targets": 11 },
            { "width": "150px", "targets": 12 }
        ],
        "autoWidth": false,

        "order": [[0, 'desc']],
       
        "language": {
            "emptyTable": "No data found...",
            "processing":
                '<i class="fa fa-spinner fa-spin fa-3x fa-fw" style="color:#2a2b2b;"></i><span class="sr-only">Loading...</span> '
        },
        "pageLength": 10,
        "responsive": true, "lengthChange": true, "autoWidth": true,
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            if (aData.StatusName == "In-active") {
                $('td', nRow).css('color', 'Red');
            }
        },

      
    }).buttons().container().appendTo('#InventoryTable_wrapper .col-md-6:eq(0)');
    
}
$('#filterCategory').on('change', function () {
    $('#InventoryTable').dataTable().fnClearTable();
    $('#InventoryTable').dataTable().fnDestroy();
    loadInventoryData();
    
});
$('#filterSupplier').on('change', function () {
    $('#InventoryTable').dataTable().fnClearTable();
    $('#InventoryTable').dataTable().fnDestroy();
    loadInventoryData();
});
$('#filterStatus').on('change', function () {
    $('#InventoryTable').dataTable().fnClearTable();
    $('#InventoryTable').dataTable().fnDestroy();
    loadInventoryData();
});
$('#filterInventoryStatus').on('change', function () {
    $('#InventoryTable').dataTable().fnClearTable();
    $('#InventoryTable').dataTable().fnDestroy();
    loadInventoryData();
});
$('#filterSearchId').on('keydown', function () {
    if (event.keyCode === 13) {
        $('#InventoryTable').dataTable().fnClearTable();
        $('#InventoryTable').dataTable().fnDestroy();
        loadInventoryData();
    }
});

function findInventoryAjax(parm) {
    $('#Id').val($(parm).parents("tr")[0].children[0].outerText);
    var idValue = $('#Id').val();
    $('#detailsId').text(idValue);
    $('#detailsjewelType').text($(parm).parents("tr")[0].children[1].outerText);
    $('#detailsCaratWeight').text($(parm).parents("tr")[0].children[2].outerText);
    $('#detailsGoldWeight').text($(parm).parents("tr")[0].children[3].outerText);
    $('#detailsPieces').text($(parm).parents("tr")[0].children[4].outerText);
    $('#detailsDescription').text($(parm).parents("tr")[0].children[5].outerText);
    $('#detailsCompany').text($(parm).parents("tr")[0].children[6].outerText);
    $('#detailsSupplier').text($(parm).parents("tr")[0].children[7].outerText);
    $('#detailsPrice').text($(parm).parents("tr")[0].children[8].outerText);
    $('#detailsDateReceived').text($(parm).parents("tr")[0].children[9].outerText);
    $('#detailsInventoryStatus').text($(parm).parents("tr")[0].children[10].outerText);
    $('#detailsStatus').text($(parm).parents("tr")[0].children[11].outerText);
    if ($(parm).parents("tr")[0].children[11].outerText == 'In-active') {
        $("#detailsStatus").css({ "color": "red" });
    }
    else {
        $("#detailsStatus").css({ "color": "Black" });
    }
    $.ajax({
        url: '/InventoryMaster/FindInventoryImage',
        type: "POST",
        data: '{Id: ' + JSON.stringify(idValue) + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result.data != '') {
                $("#viewImage").attr("src", result.data)
            }
            else {
                $("#viewImage").attr("src", result.noImage);
            }
            if (result.values != null) {
                $('#detailsGoldContent').text(result.values.GoldContent);
            }
           
        },

        error: function (result) {
            return "error";
        }
    });
}
function inventoryFormSubmit() {
    $("#InventoryForm").submit(function (e) {
        e.preventDefault(); //prevent default form submit  
        if (!($('#CaratWeight').val() == '' || $('#GoldWeight').val() == '' || $('#GoldContent').val() == '' || $('#Pieces').val() == '' || $('#DiamondPieces').val() == '' )) {           
            var model = {};
            model.Id = $('#Id').val();
            model.CompanyId = $('#Company').val();
            model.JewelTypeId = $('#JewelType').val();
            model.CategoryId = $('#Category').val();
            model.SupplierId = $('#Supplier').val();
            model.CaratWeight = $('#CaratWeight').val();
            model.GoldWeight = $('#GoldWeight').val();
            model.GoldContent = $('#GoldContent').val();
            model.Pieces = $('#Pieces').val();
            model.DiamondPieces = $('#DiamondPieces').val();
            model.DateReceived = $('#DateReceived').val();
            model.Price = $('#Price').val().replace(/[^\-\d\.]/g, '') == '' ? '0.00' : $('#Price').val().replace(/[^\-\d\.]/g, '');
            model.InventoryStatusId = $('#InventoryStatus').val();
            model.Description = $('#Description').val();
            model.Status = $('#Status').val();
            model.ImageSrc = $("#imgCapture")[0].src;

            $.ajax({
                url: '/InventoryMaster/CreateOrUpdate',
                type: "POST",
                data: '{model: ' + JSON.stringify(model) + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result.data == "Updated") {
                        $("#InventoryModalPopup").modal('hide');
                    }
                    if (result.data == "Updated" || result.data == "Created") {
                        getBarcodeImage(result.idValue);
                        $('#ImageModal').modal('show');
                        webcamInitializing();
                        $("#imgCapture").attr("src", "noImageAttached");
                        $('#Id').val('0');
                        $('#GoldWeight').val('');
                        $('#Pieces').val('');
                        $('#DiamondPieces').val('');
                        $('#GoldContent').val('');
                        $('#CaratWeight').val('');
                        $('#Price').val('');
                        $('#InventoryStatus').val('6');
                        $('#redirect_LastId').val(result.idValue);

                        getBarcodeImage(result.idValue);
                        $('#ImageModal').modal('show');

                        $('#InventoryTable').dataTable().fnClearTable();
                        $('#InventoryTable').dataTable().fnDestroy();
                       
                        loadInventoryData();
                        toastr.success('Inventory ' + result.data + ' successfully');
                        $('#CaratWeight').focus();
                    }
                    else {
                        toastr.error("Unable to perform server side operation");
                    }
                },
                error: function (result) {
                    toastr.error(result.data);
                }
            });

        }
        else {
            toastr.error('Enter all the fields');
        }
    });
}
function validateInventoryForm() {
    var validator = $('#InventoryForm').validate({
        rules: {
            CaratWeight: {
                required: true
            },
            GoldWeight: {
                required: true
            },
            GoldContent: {
                required: true
            },
            Pieces: {
                required: true
            },
            DiamondPieces: {
                required: true
            },
            DateReceived: {
                required: true
            },
        },
        messages: {
            CaratWeight: {
                required: "Please enter Gold Weight"
            },
            GoldWeight: {
                required: "Please enter Gold Weight"
            },
            GoldContent: {
                required: "Please enter Gold Content"
            },
            Pieces: {
                required: "Please enter Pieces"
            },
            DiamondPieces: {
                required: "Please enter Diamond Pieces"
            },
            DateReceived: {
                required: "Please enter Date Received"
            },

        },
        errorElement: 'span',
        errorPlacement: function (error, element) {
            error.addClass('invalid-feedback');
            element.closest('.validation-msg').append(error);
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass('is-invalid');
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass('is-invalid');
        }
    });
}
function inventoryDeleteForm() {
    $("#InventoryDeleteForm").submit(function (e) {
        e.preventDefault();
        var id = $("#DeleteInventoryId").val();
        $.ajax({
            url: '/InventoryMaster/DeleteInventory',
            type: "POST",
            data: '{Id: ' + JSON.stringify(id) + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                $('#InventoryTable').dataTable().fnClearTable();
                $('#InventoryTable').dataTable().fnDestroy();
                $("#InventoryDeleteModalPopup").modal('hide');
                loadInventoryData();
                toastr.success('inventory ' + result.data + ' successfully');
            },
            error: function (result) {
                toastr.error(result.data);
            }
        });
    });
}
function deleteInventoryFunction(parm) {
    $("#DeleteInventoryId").val($(parm).parents("tr")[0].children[0].outerText);
    $("#DeleteInventoryName").text($(parm).parents("tr")[0].children[0].outerText);
   
    $("#InventoryDeleteModalPopup").modal('show');
}
function getDate() {
    var todaydate = new Date();
    var day = todaydate.getDate();
    var month = todaydate.getMonth() + 1;
    var year = todaydate.getFullYear();
    var datestring = day + "/" + month + "/" + year;
    document.getElementById("frmDate").value = datestring;
}
function webcamInitializing() {
    Webcam.set({
        width: 238,
        height: 178,
        image_format: 'jpeg',
        jpeg_quality: 100
    });
    Webcam.attach('#webcam');
    $("#btnCapture").click(function () {
        Webcam.snap(function (data_uri) {
            $("#imgCapture")[0].src = data_uri;
            $("#btnUpload").removeAttr("disabled");
        });
    });

    
}
function createInventoryFunction() {
   
    webcamInitializing();
    $('#Id').val(0);
    $('#Company').val('1');
    $('#JewelType').val('2');
    $('#Supplier').val('1003');
    $('#Category').val('1050');
    $('#Description').val($('#Category option:selected').text());
    $('#GoldWeight').val('');
    $('#Pieces').val('');
    $('#DiamondPieces').val('');
    $('#GoldContent').val('');
    $('#CaratWeight').val('');
    $('#Price').val('');
    $('#CaratWeight').removeClass('is-invalid');
    $('#GoldWeight').removeClass('is-invalid');
    $('#GoldContent').removeClass('is-invalid');
    $('#Pieces').removeClass('is-invalid');
    $('#DiamondPieces').removeClass('is-invalid');
    $('#Price').removeClass('is-invalid');

    document.getElementById("Status").value = true;
    $("#imgCapture").attr("src", "noImageAttached");
   
    var date = new Date();
    year = date.getFullYear();
    month = (date.getMonth() + 1).toString().padStart(2, "0");
    var monthAndYear = month + '/' + year;

    document.getElementById("DateReceived").value = monthAndYear;
    $('#CaratWeight').focus();
}
$('#Company').on('change', function () {
    if ($('#Company').val() == "2") {
        $('#JewelType').val("1");
    }
    else {
        $('#JewelType').val("2");
    }
});

$("#Category").change(function () {
    var value = $("#Category").val();
    $('#Category option').each(function () {
        var b = $(this).text();
        if ($(this).val() == value) {
            $('#Description').val(b);
        }
    });
})
function setSlected(element, text) {
    $(element + ' option').each(function () {
        if ($(this).text() == text) {
            $(element).val($(this).val());
        }
        else {
            $(this).removeAttr('selected')
        }
    });
}
function editInventoryFunction(parm) {
    $('#Id').val($(parm).parents("tr")[0].children[0].outerText);
    var idValue = $('#Id').val();
    webcamInitializing();
    getvaluesFromInventoryForEditing(idValue);
}

function getvaluesFromInventoryForEditing(idValue) {
    $.ajax({
        url: '/InventoryMaster/FindInventoryImage',
        type: "POST",
        data: '{Id: ' + JSON.stringify(idValue) + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result.data != '') {
                $("#viewImage").attr("src", result.data)
            }
            else {
                $("#viewImage").attr("src", "noImageAttached");
            }
            setValues(result);
        },
        error: function (result) {
            return "error";
        }
    });
}

function setValues(result) {
    var data = result.values;
    if (data != "error") {
        $('#Id').val(data.Id);
        setSlected("#Company", data.Company)
        setSlected("#JewelType", data.JewelType)
        setSlected("#Category", data.Category)
        setSlected("#Supplier", data.Supplier)
        $("#CaratWeight").val(data.CaratWeight);
        $("#GoldWeight").val(data.GoldWeight);
        $("#GoldContent").val(data.GoldContent);
        $("#Pieces").val(data.Pieces);
        $("#DiamondPieces").val(data.DiamondPieces);
        $("#DateReceived").val(data.DateReceived);
        $("#Price").val(data.Price);
        setSlected("#InventoryStatus", data.InventoryStatus)
        $('#CaratWeight').removeClass('is-invalid');
        $('#GoldWeight').removeClass('is-invalid');
        $('#GoldContent').removeClass('is-invalid');
        $('#Pieces').removeClass('is-invalid');
        $('#DiamondPieces').removeClass('is-invalid');
        $('#Price').removeClass('is-invalid');
        $("#Description").val(data.Description);
        if (data.StatusName == 'Active') {
            document.getElementById("Status").value = true;
        }
        else {
            document.getElementById("Status").value = false;
        }
        $("#imgCapture").attr("src", "noImageAttached");
        getImage(data.Id);
    }
    $("#InventoryModalPopup").modal('show');
}
function getImage(value) {
    $.ajax({
        url: '/InventoryMaster/FindInventoryImage',
        type: "POST",
        data: '{Id: ' + JSON.stringify(value) + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result.data != '') {
                $("#imgCapture").attr("src", result.data)
            }
            else {
                $("#imgCapture").attr("src", "noImageAttached");
            }
        },
        error: function (result) {
            toastr.error("Unable to fetch Image");
        }
    });
}
function clearFilters() {
    $('#filterCategory').val('');
    $('#filterSupplier').val('');
    $('#filterStatus').val('');
    $('#filterSearchId').val('');
    $('#filterInventoryStatus').val('');
    $('#InventoryTable').dataTable().fnClearTable();
    $('#InventoryTable').dataTable().fnDestroy();
    loadInventoryData();
}



function GetBarcodeId(parm) {    
    $('#barCodeId').val($(parm).parents("tr")[0].children[0].outerText);
    var id = $('#barCodeId').val();
    getBarcodeImage(id);
    $('#ImageModal').modal('show');
}
$('#datePicker').datetimepicker({
    format: 'MM/yyyy'
});


$('#printLastLabel').click(function () {
    var barcodeId = $('#redirect_LastId').val();
    if (barcodeId == null || barcodeId == '') {

        toastr.error("Last Label is not found.");
    }
    else {        
        getBarcodeImage(barcodeId);
        $('#ImageModal').modal('show');
    }
    webcamInitializing();
    $('#CaratWeight').focus();
});

function getBarcodeImage(value) {
    $.ajax({
        url: '/InventoryMaster/GenerateBarcode',
        type: "POST",
        data: '{Id: ' + JSON.stringify(value) + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            setBarcodeData(result);

        },
        error: function (result) {
            toastr.error("Unable to fetch Image");
        }
    });
}
function clearBarcodeData() {
    $("#leftText1").text("");
    $("#line2").text("");
    $("#barcodeImage").attr("src", "");
    $("#text3").text("");
    $("#rightText1").text("");
    $("#rightText2").text("");
    $("#rightText3").text("");
    $("#rightText4").text("");
    $("#rightText5").text("");
}
function setBarcodeData(result) {
    clearBarcodeData();
    $("#leftText1").text(result.leftText1);
    $("#line2").text(result.line2);
    $("#barcodeImage").attr("src", result.data);


    if (result.value.CompanyId == 1) {
        $("#rightText1").text(result.rightText);
        $("#rightText2").text(result.value.GoldContent + "kt gold");
        $("#rightText3").text(result.value.GoldWeight + "gm");
        $("#rightText4").text(result.value.Pieces + "pcs")
    }

    else {
        $("#text3").text(result.text3);
        $("#rightText1").text(result.value.GoldContent + "kt " + result.value.GoldWeight + "gm");
        $("#rightText2").text(result.value.CaratWeight + "ct/" + result.value.Pieces + "pcs");
        $("#rightText3").text(result.value.DiamondPieces + "pcs");
        $("#rightText4").text("$" + parseFloat(4 * parseFloat(result.value.Price)).toFixed(2));
    }


}


function PrintBarcode() {
    
    var divToPrint = document.getElementById("barcodeDiv");



    var value = "7";



    if ($('#leftText1')[0].innerHTML.toLowerCase().indexOf('kv') != -1) {
        value = "6"
    }
    var newWin = window.open('', 'Print-Window');
    newWin.document.open();
    newWin.document.write('<html><head>' +
        '<link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700&amp;display=fallback">' +
        '<link href="/admin-lte/skin-blue.min.css" rel="stylesheet">' +



        '<link href="/admin-lte/plugins/datatables-bs4/css/dataTables.bootstrap4.min.css" rel="stylesheet">' +
        '<link href="/admin-lte/plugins/datatables-responsive/css/responsive.bootstrap4.min.css" rel="stylesheet">' +
        '<link href="/admin-lte/dist/css/adminlte.min.css" rel="stylesheet">' +
        '<link href="/Content/site.css" rel="stylesheet">' +
        '<script src="/Scripts/modernizr-2.8.3.js"></script>' +
        '<script src="/Scripts/jquery-3.4.1.js"></script>' +
        '<script src="/Scripts/jquery.validate.js"></script>' +
        '<script src="/Scripts/bootstrap.bundle.min.js"></script>' +
        '<script src="/admin-lte/plugins/datatables-responsive/js/dataTables.responsive.min.js"></script>' +
        '<script src="/admin-lte/plugins/datatables-responsive/js/responsive.bootstrap4.min.js"></script>' +
        '<script src="/admin-lte/plugins/bs-custom-file-input/bs-custom-file-input.min.js"></script>' +
        '<script src="/admin-lte/plugins/moment/moment.min.js"></script>' +
        '<script src="/admin-lte/dist/js/adminlte.min.js"></script>' +
        '<style>@media print{@page {size:landscape}} label {margin:0 !important;font-weight:600 !important}</style>' +
        '</head><body style="font-size:12px;padding-top:21px;line-height:12px">' +
        '<div class="col-9 pl-0" >' + divToPrint.innerHTML + ' </div>' +



        '<script>$(document).ready(function(){$(".col-7")[0].className="col-' + value + '"; window.print();window.close();})</script></body></html>');




    newWin.document.close();
    document.getElementById("barcodeImage").style.height = "16px";
    document.getElementById("barcodeImage").style.width = "140px";






}



$('#Price').on('change', function (parm) {
    parm.currentTarget.value = parm.currentTarget.value.replace(/[^\-\d\.]/g, '') == '' ? '' : '$' + parseFloat(parm.currentTarget.value.replace(/[^\-\d\.]/g, '') == '' ? 0 : parm.currentTarget.value.replace(/[^\-\d\.]/g, '')).toFixed(2);



});
   
