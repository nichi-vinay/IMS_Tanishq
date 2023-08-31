$(function () {
    loadCategoryData();
    validateCategoryForm();
    categoryFormSubmit();
    categoryDeleteForm();
    
});
function categoryDeleteForm() {
    $("#categoryDeleteform").submit(function (e) {
        e.preventDefault();
        var id = document.getElementsByName("Id")[1].value;
        $.ajax({
            url: '/CategoryMaster/DeleteCategory',
            type: "POST",
            data: '{Id: ' + JSON.stringify(id) + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                $('#CategoryTable').dataTable().fnClearTable();
                $('#CategoryTable').dataTable().fnDestroy();
                $("#categoryDeleteModalPopup").modal('hide');
                loadCategoryData();
                toastr.success('Category ' + result.data + ' successfully');
            },
            error: function (result) {
                toastr.error(result.data);
            }
        });
    });
}
function categoryFormSubmit() {
    $("#categoryform").submit(function (e) {

        if (!($('#CategoryName').hasClass('is-invalid') || $('#CategoryNumber').hasClass('is-invalid'))) {
            var model = {};
            model.Id = $('#Id').val();
            model.CategoryName = $('#CategoryName').val();
            model.CategoryNumber = $('#CategoryNumber').val();
            model.Status = $('#Status').val();
            var data = $('#categoryform').serialize();
            e.preventDefault(); //prevent default form submit
            $.ajax({
                url: '/CategoryMaster/CreateOrUpdate',
                type: "POST",
                data: '{model: ' + JSON.stringify(model) + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    $('#CategoryTable').dataTable().fnClearTable();
                    $('#CategoryTable').dataTable().fnDestroy();
                  
                    createCategoryFunction();
                    loadCategoryData();
                    toastr.success('Category ' + result.data + ' successfully');
                },
                error: function (result) {
                    toastr.error(result.data);
                }
            });
        }
    });
}
function validateCategoryForm() {
    $('#categoryform').validate({
        rules: {
            CategoryName: {
                required: true
            },
            CategoryNumber: {
                required: true
            }
        },
        messages: {
            CategoryName: {
                required: "Please enter Category Name"
            },
            CategoryNumber: {
                required: "Please enter Category Number"
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
function loadCategoryData() {
    $('#CategoryTable').DataTable({
        "bServerSide": true,
        "sAjaxSource": '/CategoryMaster/GetCategoryJsonData',
  
        "sAjaxDataProp": "aaData",
        "bProcessing": true,
        "dom": "<'row'<'col-sm-4'l><'col-sm-4'><'col-sm-4'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]],
        
        "aoColumns": [
            { "data": "Id" },
            { "data": "CategoryName" },
            { "data": "CategoryNumber" },
            { "data": "StatusName" },
            {
                "data": null,
                "mRender": function () {
                    return '<a id="tooltip" data-toggle="tooltip" data-placement="top" title="Edit Category"><button class="btn btn-primary btn-icon" onclick="editCategoryFunction(this)" data-toggle="modal" data-target="#categoryModalPopup"><i class="fas fa-edit fa-xs"></i></button></a>&nbsp;<a id="tooltip" data-toggle="tooltip" data-placement="top" title="Delete Category"><button class="btn btn-danger" onclick="deleteCategoryFunction(this)" data-toggle="modal" data-target="#categoryDeleteModalPopup"><i class="fas fa-trash fa-xs"></i></button></a>';
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
        },
    
          
    }).buttons().container().appendTo('#CategoryTable_wrapper .col-md-6:eq(0)');
}
function createCategoryFunction() {
    
    $('#categoryform')[0].reset();
    clearCategoryValidation();
    document.getElementById("Status").value = true;
    $('#Id').val('0');
}
function editCategoryFunction(parm) {
    clearCategoryValidation();
    $('editCategoryFunction').tooltip();
    $('#CategoryName').val($(parm).parents("tr")[0].children[1].outerText);
    $('#CategoryNumber').val($(parm).parents("tr")[0].children[2].outerText);
    $('#Id').val($(parm).parents("tr")[0].children[0].outerText);
    if ($(parm).parents("tr")[0].children[3].outerText == 'Active') {
        document.getElementById("Status").value = true;
    }
    else {
        document.getElementById("Status").value = false;
    }
}
function clearCategoryValidation() {
    $('#CategoryName').removeClass('is-invalid');
    $('#CategoryNumber').removeClass('is-invalid');
}
function deleteCategoryFunction(parm) {
    $("#deleteCategoryName").text($(parm).parents("tr")[0].children[1].outerText);
    document.getElementsByName("Id")[1].value = $(parm).parents("tr")[0].children[0].outerText;
}
