$(function () {
    loadSupplierData();
    validateSupplierForm();
    supplierFormSubmit();
    supplierDeleteForm();
   
});
function loadSupplierData() {
    $('#supplierTable').DataTable({
        "bServerSide": true,
        "sAjaxSource": '/SupplierMaster/GetSupplierJsonData',
        "sAjaxDataProp": "aaData",
        "bProcessing": true,
        "dom": "<'row'<'col-sm-4'l><'col-sm-4'><'col-sm-4'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]],
        "aoColumns": [
            { "data": "Id" },
            { "data": "SupplierName" },
            { "data": "SupplierCode" },
            { "data": "Address" },
            { "data": "Phone" },
            { "data": "StatusName" },
            {
                "data": null,
                "mRender": function () {
                    return '<a  data-toggle="tooltip" data-placement="top" title="Edit Supplier"><button class="btn btn-primary btn-icon" onclick="editSupplierFunction(this)" data-toggle="modal" data-target="#supplierModalPopup"><i class="fas fa-edit fa-xs"></i></button></a>&nbsp;<a id="tooltip" data-toggle="tooltip" data-placement="top" title="Delete Supplier"><button class="btn btn-danger btn-icon" onclick = "deleteSupplierFunction(this)" data-toggle="modal" data-target="#supplierDeleteModalPopup" > <i class="fas fa-trash fa-xs"></i></button></a>';
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
    }).buttons().container().appendTo('#supplierTable_wrapper .col-md-6:eq(0)');
}
function validateSupplierForm() {
    $('#supplierForm').validate({
        rules: {
            SupplierName: {
                required: true
            },
            SupplierCode: {
                required: true
            },
            Address: {
                required: true
            },
            Phone: {
                required: true
            }
        },
        messages: {
            SupplierName: {
                required: "Please enter Supplier Name"
            },
            SupplierCode: {
                required: "Please enter Supplier Code"
            },
            Address: {
                required: "Please enter Address"
            },
            Phone: {
                required: "Please enter Phone"
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
function supplierFormSubmit() {

    $("#supplierForm").submit(function (e) {
     
        if (!($("#Supplier").hasClass('is-invalid') || $("#Address").hasClass('is-invalid') || $("#Phone").hasClass('is-invalid') || $("#SupplierCode").hasClass('is-invalid')))
        {
            var model = {};
            model.Id = $('#Id').val();
            model.SupplierName = $('#SupplierName').val();
            model.SupplierCode = $('#SupplierCode').val();
            model.Address = $('#Address').val();
            model.Phone = $('#Phone').val();
            model.Status = $('#Status').val();
            var data = $('#supplierForm').serialize();
            e.preventDefault(); //prevent default form submit
            $.ajax({
                url: '/SupplierMaster/CreateOrUpdate',
                type: "POST",
                data: '{model:' + JSON.stringify(model) + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    $('#supplierTable').dataTable().fnClearTable();
                    $('#supplierTable').dataTable().fnDestroy();
                    /*$("#supplierModalPopup").modal('hide');*/
                    createSupplierFunction();
                    loadSupplierData();
                    toastr.success('Supplier ' + result.data + ' successfully');
                },
                error: function (result) {
                    toastr.error(result.data);
                }

            });
        }
    });
}

function supplierDeleteForm() {
    $("#SupplierDeleteForm").submit(function (e) {
        e.preventDefault();
        var id = $("#deleteSupplierId").val();
        $.ajax({
            url: '/SupplierMaster/DeleteSupplier',
            type: "POST",
            data: '{Id: ' + JSON.stringify(id) + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                $('#supplierTable').dataTable().fnClearTable();
                $('#supplierTable').dataTable().fnDestroy();
                $("#supplierDeleteModalPopup").modal('hide');
                loadSupplierData();
                toastr.success('Supplier ' + result.data + ' successfully');
            },
            error: function (result) {
                toastr.error(result.data);
            }
        });
    });
}
function createSupplierFunction() {
    $('#supplierForm')[0].reset();
    clearSupplierValidation();
    $('#Id').val('0');
    $("#Status").val('true');
}

function editSupplierFunction(parm) {
    clearSupplierValidation();
    $('#Id').val($(parm).parents("tr")[0].children[0].outerText);
    document.getElementsByName("SupplierName")[0].value = $(parm).parents("tr")[0].children[1].outerText;
    document.getElementsByName("SupplierCode")[0].value = $(parm).parents("tr")[0].children[2].outerText;
    document.getElementsByName("Address")[0].value = $(parm).parents("tr")[0].children[3].outerText;
    document.getElementsByName("Phone")[0].value = $(parm).parents("tr")[0].children[4].outerText;
    document.getElementsByName("Status")[0].value = $(parm).parents("tr")[0].children[5].outerText;
    if ($(parm).parents("tr")[0].children[5].outerText == 'Active') {
        document.getElementById("Status").value = true;
    }
    else {
        document.getElementById("Status").value = false;
    }
}
function deleteSupplierFunction(parm) {
    $("#deleteSupplierId").val($(parm).parents("tr")[0].children[0].outerText);
    $("#deleteSupplierName").text($(parm).parents("tr")[0].children[1].outerText);
}
function clearSupplierValidation() {
    $('#SupplierName').removeClass('is-invalid');
    $('#SupplierCode').removeClass('is-invalid');
    $('#Address').removeClass('is-invalid');
    $('#Phone').removeClass('is-invalid');

}