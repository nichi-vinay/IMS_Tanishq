$(function () {
    loadRoleData();
    validateRoleForm();
    roleFormSubmit();
    roleDeleteForm();
    
});
function loadRoleData() {
    $('#roleTable').DataTable({
        "bServerSide": true,
        "sAjaxSource": '/RoleMaster/GetRoleJsonData',
        "sAjaxDataProp": "aaData",
        "bProcessing": true,
        "dom": "<'row'<'col-sm-4'l><'col-sm-4'><'col-sm-4'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]],
        "aoColumns": [
            { "data": "Id" },
            { "data": "RoleName" },
            { "data": "StatusName" },
            {
                "data": null,
                "mRender": function () {
                    return '<a id="tooltip" data-toggle="tooltip" data-placement="top" title="Edit Role"><button class="btn btn-primary btn-icon" onclick="editRoleFunction(this)" data-toggle="modal" data-target="#roleModalPopup"><i class="fas fa-edit fa-xs"></i></button></a>&nbsp;<a id="tooltip" data-toggle="tooltip" data-placement="top" title="Delete Role"><button class="btn btn-danger btn-icon" onclick = "deleteRoleFunction(this)" data-toggle="modal" data-target="#roleDeleteModalPopup" > <i class="fas fa-trash fa-xs"></i></button></a>';
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
    }).buttons().container().appendTo('#roleTable_wrapper .col-md-6:eq(0)');
}

function validateRoleForm() {
    $('#roleForm').validate({
        rules: {
            RoleName: {
                required: true
            }
        },
        messages: {
            RoleName: {
                required: "Please enter Role Name"
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
function roleFormSubmit() {
    $("#roleForm").submit(function (e) {
        if (!($("#RoleName").hasClass('is-invalid')))
        {
            var model = {};
            model.Id = $('#Id').val();
            model.RoleName = $('#RoleName').val();
            model.Status = $('#Status').val();
            var data = $('#roleform').serialize();
            e.preventDefault(); //prevent default form submit
            $.ajax({
                url: '/RoleMaster/CreateOrUpdate',
                type: "POST",
                data: '{model:' + JSON.stringify(model) + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    $('#roleTable').dataTable().fnClearTable();
                    $('#roleTable').dataTable().fnDestroy();
                    /*$("#roleModalPopup").modal('hide');*/
                    createRoleFunction();
                    loadRoleData();
                    toastr.success('Role ' + result.data + ' successfully');
                },
                error: function (result) {
                    toastr.error(result.data);
                }
            });
        }
 
    });
}
function roleDeleteForm() {
    $("#roleDeleteForm").submit(function (e) {
        e.preventDefault();
        var id = $("#deleteRoleId").val();
        $.ajax({
            url: '/RoleMaster/DeleteRole',
            type: "POST",
            data: '{Id: ' + JSON.stringify(id) + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                $('#roleTable').dataTable().fnClearTable();
                $('#roleTable').dataTable().fnDestroy();
                $("#roleDeleteModalPopup").modal('hide');
                loadRoleData();
                toastr.success('Role ' + result.data + ' successfully');
            },
            error: function (result) {
                toastr.error(result.data);
            }
        });
    });
}
function createRoleFunction() {
    
    $('#roleForm')[0].reset();
    clearRoleValidation();
    document.getElementById("Status").value = true;
    $('#Id').val('0');
}


function editRoleFunction(parm) {
    clearRoleValidation();
    $('#Id').val($(parm).parents("tr")[0].children[0].outerText);
    document.getElementsByName("RoleName")[0].value = $(parm).parents("tr")[0].children[1].outerText;
    document.getElementsByName("Status")[0].value = $(parm).parents("tr")[0].children[2].outerText;
    if ($(parm).parents("tr")[0].children[2].outerText == 'Active') {
        document.getElementById("Status").value = true;
    }
    else {
        document.getElementById("Status").value = false;
    }
}

function deleteRoleFunction(parm) {
    $("#deleteRoleId").val($(parm).parents("tr")[0].children[0].outerText);
    $("#deleteRoleName").text($(parm).parents("tr")[0].children[1].outerText);
}

function clearRoleValidation() {
    $('#RoleName').removeClass('is-invalid');
}