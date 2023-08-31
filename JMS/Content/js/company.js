$(function () {
    loadCompanyData();
    validateCompanyForm();
    companyFormSubmit();
    companyDeleteForm();
    
});
function companyDeleteForm() {
    $("#companyDeleteform").submit(function (e) {
        e.preventDefault();
        var id = document.getElementsByName("Id")[1].value;
        $.ajax({
            url: '/CompanyMaster/DeleteCompany',
            type: "POST",
            data: '{Id: ' + JSON.stringify(id) + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                $('#CompanyTable').dataTable().fnClearTable();
                $('#CompanyTable').dataTable().fnDestroy();
                $("#companyDeleteModalPopup").modal('hide');
                loadCompanyData();
                toastr.success('Company ' + result.data + ' successfully');
            },
            error: function (result) {
                toastr.error(result.data);
            }
        });
    });
}
function companyFormSubmit() {
    $("#Companyform").submit(function (e) {
        if (!($('#CompanyName').hasClass('is-invalid'))) {
            var model = {};
            model.Id = $('#Id').val();
            model.CompanyName = $('#CompanyName').val();
            model.Status = $('#Status').val();
            e.preventDefault(); //prevent default form submit
            $.ajax({
                url: '/CompanyMaster/CreateOrUpdate',
                type: "POST",
                data: '{model: ' + JSON.stringify(model) + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    $('#CompanyTable').dataTable().fnClearTable();
                    $('#CompanyTable').dataTable().fnDestroy();
                 
                    createCompanyFunction();
                    loadCompanyData();
                    toastr.success('Company ' + result.data + ' successfully');
                },
                error: function (result) {
                    toastr.error(result.data);
                }
            });
        }
    });
}
function validateCompanyForm() {
    $('#Companyform').validate({
        rules: {
            CompanyName: {
                required: true
            }
        },
        messages: {
            CompanyName: {
                required: "Please enter Company Name"
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
function loadCompanyData() {
    $('#CompanyTable').DataTable({
        "bServerSide": true,
        "sAjaxSource": '/CompanyMaster/GetCompanyJsonData',
        "sAjaxDataProp": "aaData",
        "bProcessing": true,
        "dom": "<'row'<'col-sm-4'l><'col-sm-4'><'col-sm-4'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]],
        "aoColumns": [
            { "data": "Id" },
            { "data": "CompanyName" },
            { "data": "StatusName" },
            {
                "data": null,
                "mRender": function () {
                    return '<a id="tooltip" data-toggle="tooltip" data-placement="top" title="Edit Company"><button class="btn btn-primary btn-icon" onclick="editCompanyFunction(this)" data-toggle="modal" data-target="#companyModalPopup"><i class="fas fa-edit fa-xs"></i></button></a>&nbsp;<a id="tooltip" data-toggle="tooltip" data-placement="top" title="Delete Company"><button class="btn btn-danger btn-icon" onclick = "deleteCompanyFunction(this)" data-toggle="modal" data-target="#companyDeleteModalPopup" > <i class="fas fa-trash fa-xs"></i></button ></a>';
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
    }).buttons().container().appendTo('#CompanyTable_wrapper .col-md-6:eq(0)');
}
function createCompanyFunction() {
    $('#Companyform')[0].reset();
    clearCompanyValidation();
    $('#Id').val('0');
    $('#Status').val('true');
}
function editCompanyFunction(parm) {
    clearCompanyValidation();
    document.getElementsByName("CompanyName")[0].value = $(parm).parents("tr")[0].children[1].outerText;
    $('#Id').val($(parm).parents("tr")[0].children[0].outerText);
    if ($(parm).parents("tr")[0].children[2].outerText == 'Active') {
        $('#Status').val('true');
    }
    else {
        $('#Status').val('false');
    }
}
function clearCompanyValidation() {
    $('#CompanyName').removeClass('is-invalid');
}
function deleteCompanyFunction(parm) {
    $("#deleteCompanyName").text($(parm).parents("tr")[0].children[1].outerText);
    document.getElementsByName("Id")[1].value = $(parm).parents("tr")[0].children[0].outerText
}