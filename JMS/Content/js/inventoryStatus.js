$(function () {
    loadInventoryStatusData();
    validateInventoryStatusForm();
    InventoryStatusFormSubmit();
    InventoryStatusDeleteForm();
  
});
function InventoryStatusDeleteForm() {
    $("#inventoryStatusDeleteform").submit(function (e) {
        e.preventDefault();
        var id = $('#DeleteStatusId').val();
        $.ajax({
            url: '/InventoryStatusMaster/DeleteInventoryStatus',
            type: "POST",
            data: '{Id: ' + JSON.stringify(id) + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                $('#InventoryStatusTable').dataTable().fnClearTable();
                $('#InventoryStatusTable').dataTable().fnDestroy();
                $("#inventoryStatusDeleteModalPopup").modal('hide');
                loadInventoryStatusData();
                toastr.success('Inventory status ' + result.data + ' successfully');
            },
            error: function (result) {
                toastr.error(result.data);
            }
        });
    });
}
function loadInventoryStatusData() {
    $('#InventoryStatusTable').DataTable({
        "bServerSide": true,
        "sAjaxSource": '/InventoryStatusMaster/GetInventoryStatusJsonData',
        "sAjaxDataProp": "aaData",
        "bProcessing": true,
        "dom": "<'row'<'col-sm-4'l><'col-sm-4'><'col-sm-4'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]],
        "aoColumns": [
            { "data": "Id" },
            { "data": "InventoryStatusName" },
            { "data": "StatusName" },
            {
                "data": null,
                "mRender": function () {
                    return '<a id="tooltip" data-toggle="tooltip" data-placement="top" title="Edit InventoryStatus"><button class="btn btn-primary btn-icon" onclick="editInventoryStatusFunction(this)" data-toggle="modal" data-target="#inventoryStatusModalPopup"><i class="fas fa-edit fa-xs"></i></button></a>&nbsp;<a id="tooltip" data-toggle="tooltip" data-placement="top" title="Delete InventoryStatus"><button class="btn btn-danger btn-icon" onclick = "deleteInventoryStatusFunction(this)" data-toggle="modal" data-target="#inventoryStatusDeleteModalPopup" > <i class="fas fa-trash fa-xs"></i></button></a>';
                }
            }
        ],
        'columnDefs': [{
            'targets': -1, // column index (start from 0)
            'orderable': false, // set orderable false for selected columns
        }],
        "order": [[1, 'asc']],
        "language": {
            "emptyTable": "No data found...",
            "processing":
                '<i class="fa fa-spinner fa-spin fa-3x fa-fw" style="color:#2a2b2b;"></i><span class="sr-only">Loading...</span> '
        }, 
        "pageLength": 10,
        "responsive": true, "lengthChange": true, "autoWidth": true,
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            if (aData.StatusName == "In-Active") {
                $('td', nRow).css('color', 'Red');
            }
        }
    }).buttons().container().appendTo('#InventoryStatusTable_wrapper .col-md-6:eq(0)');
}
function validateInventoryStatusForm() {
    $('#inventoryStatusform').validate({
        rules: {
            InventoryStatusName: {
                required: true
            }
        },
        messages: {
            InventoryStatusName: {
                required: "Please enter Inventory Status Name"
            }
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
function InventoryStatusFormSubmit() {
    $("#inventoryStatusform").submit(function (e) {
        e.preventDefault(); //prevent default form submit
        if (!($('#InventoryStatusName').hasClass('is-invalid'))) {
            var model = {};
            model.Id = $('#Id').val();
            model.InventoryStatusName = $('#InventoryStatusName').val();
            model.Status = $('#Status').val();
            $.ajax({
                url: '/InventoryStatusMaster/CreateOrUpdate',
                type: "POST",
                data: '{model: ' + JSON.stringify(model) + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    $('#InventoryStatusTable').dataTable().fnClearTable();
                    $('#InventoryStatusTable').dataTable().fnDestroy();
                   
                    createInventoryStatusFunction();
                    loadInventoryStatusData();
                    toastr.success('Inventory status ' + result.data + ' successfully');
                },
                error: function (result) {
                    toastr.error(result.data);
                }
            });
        }
    });
}
function createInventoryStatusFunction() {
    $('#inventoryStatusform')[0].reset();
    clearInventoryStatusValidation();
    $('#Id').val('0');
    $('#Status').val('true');
}
function editInventoryStatusFunction(parm) {
    clearInventoryStatusValidation();
    $('#Id').val($(parm).parents("tr")[0].children[0].outerText);
    $('#InventoryStatusName').val($(parm).parents("tr")[0].children[1].outerText);
    if ($(parm).parents("tr")[0].children[2].outerText == 'Active') {
        $('#Status').val('true');
    }
    else {
        $('#Status').val('false');
    }
}
function clearInventoryStatusValidation() {
    $('#InventoryStatusName').removeClass('is-invalid');
}
function deleteInventoryStatusFunction(parm) {
    $("#deleteInventoryStatusName").text($(parm).parents("tr")[0].children[1].outerText);
    $("#DeleteStatusId").val($(parm).parents("tr")[0].children[0].outerText);
}