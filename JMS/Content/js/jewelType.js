$(function () {
    loadjewelTypeData();
    validatejewelTypeForm();
    jewelTypeFormSubmit();
    jewelTypeDeleteForm();
  
});

function loadjewelTypeData() {
    $('#jewelTypeTable').DataTable({
        "bServerSide": true,
        "sAjaxSource": '/JewelTypeMaster/GetJewelTypeJsonData',
        "sAjaxDataProp": "aaData",
        "bProcessing": true,
        "dom": "<'row'<'col-sm-4'l><'col-sm-4'><'col-sm-4'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]],
        "aoColumns": [
            { "data": "Id" },
            { "data": "JewelTypeName" },
            { "data": "StatusName" },
            {
                "data": null,
                "mRender": function () {
                    return '<a id="tooltip" data-toggle="tooltip" data-placement="top" title="Edit JewelType"><button class="btn btn-primary btn-icon" onclick="editjewelTypeFunction(this)" data-toggle="modal" data-target="#jewelTypeModalPopup"><i class="fas fa-edit fa-xs"></i></button></a>&nbsp;<a id="tooltip" data-toggle="tooltip" data-placement="top" title="Delete JewelType"><button class="btn btn-danger btn-icon" onclick = "deleteJewelTypeFunction(this)" data-toggle="modal" data-target="#jewelTypeDeleteModalPopup" > <i class="fas fa-trash fa-xs"></i></button></a>';
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
    }).buttons().container().appendTo('#jewelTypeTable_wrapper .col-md-6:eq(0)');
}
function validatejewelTypeForm() {
    $('#jewelTypeform').validate({
        rules: {
            JewelTypeName: {
                required: true
            }
        },
        messages: {
            JewelTypeName: {
                required: "Please enter JewelType Name"
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
function jewelTypeFormSubmit() {
    $("#jewelTypeform").submit(function (e) {
       
        if (!($("#JewelTypeName").hasClass('is-invalid')))
        {
            var model = {};
            model.Id = $('#Id').val();
            model.JewelTypeName = $('#JewelTypeName').val();
            model.Status = $('#Status').val();
            var data = $('#jewelTypeform').serialize();
            e.preventDefault(); //prevent default form submit
            $.ajax({
                url: '/JewelTypeMaster/CreateOrUpdate',
                type: "POST",
                data: '{model:' + JSON.stringify(model) + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    $('#jewelTypeTable').dataTable().fnClearTable();
                    $('#jewelTypeTable').dataTable().fnDestroy();
                  
                    createJewelTypeFunction();
                    loadjewelTypeData();
                    toastr.success('JewelType ' + result.data + ' successfully');
                },
                error: function (result) {
                    toastr.error(result.data);
                }
            });
        }

    });
}
function jewelTypeDeleteForm() {
    $("#jewelTypeDeleteform").submit(function (e) {
        e.preventDefault();
        var id = $("#deletejewelTypeId").val();
        $.ajax({
            url: '/JewelTypeMaster/DeleteJewelType',
            type: "POST",
            data: '{Id: ' + JSON.stringify(id) + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                $('#jewelTypeTable').dataTable().fnClearTable();
                $('#jewelTypeTable').dataTable().fnDestroy();
                $("#jewelTypeDeleteModalPopup").modal('hide');
                loadjewelTypeData();
                toastr.success('JewelType ' + result.data + ' successfully');
            },
            error: function (result) {
                toastr.error(result.data);
            }
        });
    });
}
function createJewelTypeFunction() {
    $('#jewelTypeform')[0].reset();
    clearJewelTypeValidation();
    $('#Id').val('0');
    $("#Status").val('true');
}
function editjewelTypeFunction(parm) {
    clearJewelTypeValidation();
    var id = $('#Id').val($(parm).parents("tr")[0].children[0].outerText);
    document.getElementsByName("JewelTypeName")[0].value = $(parm).parents("tr")[0].children[1].outerText;
    document.getElementsByName("Status")[0].value = $(parm).parents("tr")[0].children[2].outerText;
    if ($(parm).parents("tr")[0].children[2].outerText == 'Active') {
        document.getElementById("Status").value = true;
    }
    else {
        document.getElementById("Status").value = false;
    }
}
function deleteJewelTypeFunction(parm) {
    $("#deletejewelTypeId").val($(parm).parents("tr")[0].children[0].outerText);
    $("#deleteJewelTypeName").text($(parm).parents("tr")[0].children[1].outerText);
}

function clearJewelTypeValidation() {
    $('#JewelTypeName').removeClass('is-invalid');
}