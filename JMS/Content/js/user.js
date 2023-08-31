$(function () {
    loadUserData();
    validateUserForm();
    validateUserChangePasswordForm();
    userFormSubmit();
    userDeleteForm();
    userChangePasswordForm();
   
   
});
function loadUserData() {
    $('#userTable').DataTable({
        "bServerSide": true,
        "sAjaxSource": '/UserMaster/GetUserJsonData',
        "sAjaxDataProp": "aaData",
        "bProcessing": true,
        "dom": "<'row'<'col-sm-4'l><'col-sm-4'><'col-sm-4'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]],
        "aoColumns": [
            { "data": "Id" },
            { "data": "UserId" },
            { "data": "Name" },
            { "data": "RoleName" },
            { "data": "Phone" },
            { "data": "UserName" },
            { "data": "StatusName" },
            {
                "data": null,
                "mRender": function () {
                    return '<a id="tooltip" data-toggle="tooltip" data-placement="top" title="Edit User"><button class="btn btn-primary btn-icon" onclick="editUserFunction(this)" data-toggle="modal" data-target="#UserModalPopup"><i class="fas fa-edit fa-xs"></i></button></a>&nbsp;<a id="tooltip" data-toggle="tooltip" data-placement="top" title="Change Password"><button class="btn btn-primary" onclick="userChangePasswordClick(this)" data-toggle="modal" data-target="#UserChangePasswordModalPopup"><i class="fas fa-key fa-xs"></i></button></a>&nbsp;<a id="tooltip" data-toggle="tooltip" data-placement="top" title="Delete User"><button class="btn btn-danger btn-icon" onclick = "deleteUserFunction(this)" data-toggle="modal" data-target="#UserDeleteModalPopup" > <i class="fas fa-trash fa-xs"></i></button></a>';
                }
            }
        ],
        'columnDefs': [{
            'targets': -1, // column index (start from 0)
            'orderable': false, // set orderable false for selected columns
        }],
        "order": [[2, 'asc']],
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
    }).buttons().container().appendTo('#userTable_wrapper .col-md-6:eq(0)');
}

function validateUserForm() {
    $('#userForm').validate({
        rules: {
            UserId: {
                required: true
            },
            Name: {
                required: true
            },
            RoleId: {
                required: true
            },
            Password: {
                required: true
            },
            Phone: {
                required: true
            },
            UserName: {
                required: true
            },
        },
        messages: {
            UserId: {
                required: "Please enter  User ID"
            },
            Name: {
                required: "Please enter Name"
            },
            RoleId: {
                required: "Please enter Role ID"
            },
            Phone: {
                required: "Please enter Phone"
            },
            UserName: {
                required: "Please enter User Name"
            },
            Password: {
                required:"Please enter Password "
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
function validateUserChangePasswordForm() {
    $('#changePasswordforUserForm').validate({
        rules: {
            changePassword: {
                required: true,
                minlength: 8
            }
        },
        messages: {
            changePassword: {
                required: "Please enter Password",
                minlength:"Minimum 8 characters Needed"
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
function userFormSubmit() {
    $("#userForm").submit(function (e) {
        if (!($("#UserId").hasClass('is-invalid') || $("#Name").hasClass('is-invalid') || $("#Phone").hasClass('is-invalid') || $("#UserName").hasClass('is-invalid') || $("#Password").hasClass('is-invalid'))) {


            var model = {};
            model.Id = $('#Id').val();
            model.UserId = $('#UserId').val();
            model.Name = $('#Name').val();
            model.RoleId = $('#RoleId').val();
            model.Phone = $('#Phone').val();
            model.UserName = $('#UserName').val();
            model.Password = $('#Password').val();
            model.Status = $('#Status').val();
            e.preventDefault(); //prevent default form submit
            $.ajax({
                url: '/UserMaster/CreateOrUpdate',
                type: "POST",
                data: '{model:' + JSON.stringify(model) + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    $('#userTable').dataTable().fnClearTable();
                    $('#userTable').dataTable().fnDestroy();
                    /*$("#UserModalPopup").modal('hide');*/
                    createUserFunction();
                    loadUserData();
                    toastr.success('User ' + result.data + ' successfully');
                },
                error: function (result) {
                    toastr.error(result.data);
                }
            });
        }

    });
}
function userDeleteForm() {
    $("#deleteUserForm").submit(function (e) {
        e.preventDefault();
        var id = $("#deleteUserId").val();
        $.ajax({
            url: '/UserMaster/DeleteUser',
            type: "POST",
            data: '{Id: ' + JSON.stringify(id) + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                $('#userTable').dataTable().fnClearTable();
                $('#userTable').dataTable().fnDestroy();
                $("#UserDeleteModalPopup").modal('hide');
                loadUserData();
                toastr.success('User ' + result.data + ' successfully');
            },
            error: function (result) {
                toastr.error(result.data);
            }
        });
    });
}
function userChangePasswordForm() {
    $("#changePasswordforUserForm").submit(function (e) {
        e.preventDefault();
        if (!($("#changePassword").hasClass('is-invalid'))) {
            var model = {};
            model.Id = $('#changePasswordId').val();
        model.Password = $('#changePassword').val();
            $.ajax({
                url: '/UserMaster/UserChangePassword',
                type: "POST",
                data: '{model:' + JSON.stringify(model) + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    $('#userTable').dataTable().fnClearTable();
                    $('#userTable').dataTable().fnDestroy();
                    $("#UserChangePasswordModalPopup").modal('hide');
                    loadUserData();
                    toastr.success('User ' + result.data + ' successfully');
                },
                error: function (result) {
                    toastr.error(result.data);
                }
            });
        }
        });

}
function userChangePasswordClick(parm) {
  
    $("#changePasswordId").val($(parm).parents("tr")[0].children[0].outerText);
    $("#changePaswordUserName").val($(parm).parents("tr")[0].children[2].outerText);
    $("#changePassword").val('');
    $('#changePassword').removeClass('is-invalid');
}

function createUserFunction() {
    $('#PasswordFormGroup').show();
    $('#userForm')[0].reset();
    clearUserValidation();
    $('#Id').val('0');
    $("#Status").val('true');
}
function editUserFunction(parm) {
    clearUserValidation();
    $('#PasswordFormGroup').hide();
    $('#Id').val($(parm).parents("tr")[0].children[0].outerText);

    document.getElementsByName("UserId")[0].value = $(parm).parents("tr")[0].children[1].outerText;
    document.getElementsByName("Name")[0].value = $(parm).parents("tr")[0].children[2].outerText;
    document.getElementsByName("Phone")[0].value = $(parm).parents("tr")[0].children[4].outerText;
    document.getElementsByName("UserName")[0].value = $(parm).parents("tr")[0].children[5].outerText;
    document.getElementsByName("Status")[0].value = $(parm).parents("tr")[0].children[6].outerText;
    var roleName = document.getElementsByName("RoleId")[0].value = $(parm).parents("tr")[0].children[3].outerText;
    setSelected('#RoleId', roleName);
    if ($(parm).parents("tr")[0].children[6].outerText == 'Active') {
        document.getElementById("Status").value = true;
    }
    else {
        document.getElementById("Status").value = false;
    }
}
function deleteUserFunction(parm) {
    $("#deleteUserId").val($(parm).parents("tr")[0].children[0].outerText);
    $("#deleteUserName").text($(parm).parents("tr")[0].children[1].outerText);
    $("#deleteName").text($(parm).parents("tr")[0].children[2].outerText);
}
function setSelected(element, text) {
    $(element + ' option').each(function () {
        var b = $(this).text();
        if ($(this).text() == text) {
            $(element).val($(this).val());
        }
        else {
            $(this).removeAttr('selected')
        }
    });
}

function clearUserValidation() {
    $('#UserId').removeClass('is-invalid');
    $('#Name').removeClass('is-invalid');
    $('#RoleId').removeClass('is-invalid');
    $('#Phone').removeClass('is-invalid');
    $('#UserName').removeClass('is-invalid');
    $('#Password').removeClass('is-invalid');
}