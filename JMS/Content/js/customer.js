$(function () {
    
    loadCustomerData();
    validateCustomerForm();
    customerFormSubmit();
    customerDeleteForm();
  
});
document.getElementById('CustomerPhone').addEventListener('input', function (e) {
    var x = e.target.value.replace(/\D/g, '').match(/(\d{0,3})(\d{0,3})(\d{0,4})/);
    e.target.value = !x[2] ? x[1] : '(' + x[1] + ') ' + x[2] + (x[3] ? '-' + x[3] : '');
});
function loadCustomerData() {
    
    $('#customerTable').DataTable({
        "bServerSide": true,
        "sAjaxSource": '/CustomerMaster/GetCustomerJsonData',
        "sAjaxDataProp": "aaData",
        "bProcessing": true,
        "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]],
        "dom": "<'row'<'col-sm-4'l><'col-sm-4'><'col-sm-4'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        "aoColumns": [
            { "data": "Id" },
            { "data": "CustomerName" },
            { "data": "CustomerPhone" },
            { "data": "Address" },
            { "data": "DLNumber" },
            { "data": "ExpDate" },
            { "data": "DOB" },
            { "data": "Email" },
            { "data": "City" },
            { "data": "State" },
            { "data": "Zip" },
            { "data": "StatusName" },
            {
                "data": null,
                "mRender": function () {
                    return '<a id="tooltip" data-toggle="tooltip" data-placement="top" title="Edit Customer"><button class="btn btn-primary btn-icon" onclick="editCustomerFunction(this)" data-toggle="modal" data-target="#CustomerModalPopup"><i class="fas fa-edit fa-xs"></i></button></a>&nbsp;<a id="tooltip" data-toggle="tooltip" data-placement="top" title="Delete Customer"><button class="btn btn-danger btn-icon" onclick = "deleteCustomerFunction(this)" data-toggle="modal" data-target="#CustomerDeleteModalPopup" > <i class="fas fa-trash fa-xs"></i></button></a>';
                }
            }
        ],
        'columnDefs': [{
            'targets': -1, // column index (start from 0)
            'orderable': false, // set orderable false for selected columns
        }],
      
        "order": [[1, 'asc']],
        "sScrollXInner": "150%",
        "scrollX": true,
       
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
    }).buttons().container().appendTo('#customerTable_wrapper .col-md-6:eq(0)');
}

function validateCustomerForm() {
    $('#customerForm').validate({
        rules: {
            CustomerName: {
                required: true
            },
           
        },
        messages: {
            CustomerName: {
                required: "Please enter  CustomerName"
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
function customerFormSubmit() {
    $("#customerForm").submit(function (e) {
        var a =  $("#CustomerPhone").hasClass('is-invalid')
        if (!($("#CustomerName").hasClass('is-invalid') || $("#CustomerPhone").hasClass('is-invalid') || $("#Address").hasClass('is-invalid') || $("#DLNumber").hasClass('is-invalid') || $("#ExpDate").hasClass('is-invalid') || $("#DOB").hasClass('is-invalid') || $("#Email").hasClass('is-invalid')))
        {
            var model = {};
            model.Id = $('#Id').val();
            model.CustomerName = $('#CustomerName').val();
            model.CustomerPhone = $('#CustomerPhone').val();
            model.Address = $('#Address').val();
            model.DLNumber = $('#DLNumber').val();
            model.ExpDate = $('#ExpDate').val();
            model.DOB = $('#DOB').val();
            model.Email = $('#Email').val();
            model.City = $('#City').val();
            model.State = $('#State').val();
            model.Zip = $('#Zip').val();
            model.Status = $('#Status').val();
            e.preventDefault(); //prevent default form submit
            $.ajax({
                url: '/CustomerMaster/CreateOrUpdate',
                type: "POST",
                data: '{model:' + JSON.stringify(model) + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    $('#customerTable').dataTable().fnClearTable();
                    $('#customerTable').dataTable().fnDestroy();
                    
                    createCustomerFunction();
                    loadCustomerData();
                    toastr.success('Customer ' + result.data + ' successfully');
                },
                error: function (result) {
                    toastr.error(result.data);
                }
            });
        }

    });
}
function customerDeleteForm() {
    $("#customerDeleteForm").submit(function (e) {
        e.preventDefault();
        var id = document.getElementsByName("Id")[1].value;
        $.ajax({
            url: '/CustomerMaster/DeleteCustomer',
            type: "POST",
            data: '{Id: ' + JSON.stringify(id) + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                $('#customerTable').dataTable().fnClearTable();
                $('#customerTable').dataTable().fnDestroy();
                $("#CustomerDeleteModalPopup").modal('hide');
                loadCustomerData();
                toastr.success('Customer ' + result.data + ' successfully');
            },
            error: function (result) {
                toastr.error(result.data);
            }
        });
    });
}
function createCustomerFunction() {
    $('#customerForm')[0].reset();
    clearCustomerValidation();
    $('#Id').val('0');
    $("#Status").val('true');
}
function editCustomerFunction(parm) {

    clearCustomerValidation();
    $('#Id').val($(parm).parents("tr")[0].children[0].outerText);
    document.getElementsByName("CustomerName")[0].value = $(parm).parents("tr")[0].children[1].outerText;
    document.getElementsByName("CustomerPhone")[0].value = $(parm).parents("tr")[0].children[2].outerText;
    document.getElementsByName("Address")[0].value = $(parm).parents("tr")[0].children[3].outerText;
    document.getElementsByName("DLNumber")[0].value = $(parm).parents("tr")[0].children[4].outerText;
    document.getElementsByName("ExpDate")[0].value = $(parm).parents("tr")[0].children[5].outerText;
    document.getElementsByName("DOB")[0].value = $(parm).parents("tr")[0].children[6].outerText;
    document.getElementsByName("Email")[0].value = $(parm).parents("tr")[0].children[7].outerText;
    document.getElementsByName("City")[0].value = $(parm).parents("tr")[0].children[8].outerText;
    document.getElementsByName("State")[0].value = $(parm).parents("tr")[0].children[9].outerText;
    document.getElementsByName("Zip")[0].value = $(parm).parents("tr")[0].children[10].outerText;
    document.getElementsByName("Status")[0].value = $(parm).parents("tr")[0].children[11].outerText;
    if ($(parm).parents("tr")[0].children[11].outerText == 'Active') {
        document.getElementById("Status").value = true;
    }
    else {
        document.getElementById("Status").value = false;
    }
}
function deleteCustomerFunction(parm) {
    $("#deletecustomerId").val($(parm).parents("tr")[0].children[0].outerText);
    $("#deleteCustomerName").text($(parm).parents("tr")[0].children[1].outerText);
}
function clearCustomerValidation() {
    $('#CustomerName').removeClass('is-invalid');
    $('#CustomerPhone').removeClass('is-invalid');
    $('#Address').removeClass('is-invalid');
    $('#DLNumber').removeClass('is-invalid');
    $('#ExpDate').removeClass('is-invalid');
    $('#DOB').removeClass('is-invalid');
    $('#Email').removeClass('is-invalid');
    $('#City').removeClass('is-invalid');
    $('#State').removeClass('is-invalid');
    $('#Zip').removeClass('is-invalid');
}