var initial = true;
var currentPage = 0;
$(function () {
   
    $("#cashPayment").val('0');
    validateaddPaymentForm();
    validateEditPaymentForm();
    invoiceUpdateFormSubmit();
    deleteInvoiceForm();
    getbackInvoiceInvoiceForm();
    addNewPaymentFormSubmit();
    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
    var yyyy = today.getFullYear();
    $('#fromDate').val(mm + '-' + dd + '-' + yyyy);
    $('#toDate').val(mm + '-' + dd + '-' + yyyy);
    $('#filterStatus').val('true');
   
    redirectLoad();
    

});

$('#filterSearchId').on('keydown', function () {
    if (event.keyCode === 13) {
        $('#invoiceList').dataTable().fnClearTable();
        $('#invoiceList').dataTable().fnDestroy();
        loadInvoices();
    }
});
$('#CustomerPhone').on('keydown', function () {
    if (event.keyCode === 13) {
        $('#invoiceList').dataTable().fnClearTable();
        $('#invoiceList').dataTable().fnDestroy();
        loadInvoices();
    }
});
$('#filterStatus').on('change', function () {
    $('#invoiceList').dataTable().fnClearTable();
    $('#invoiceList').dataTable().fnDestroy();
    loadInvoices();
});

function redirectLoad() {

    if (($('#redirect_fromDate').val() != '' ) && ($('#redirect_toDate').val() != '' )) {
        $('#fromDate').val($('#redirect_fromDate').val());
        $('#toDate').val($('#redirect_toDate').val());
        $('#CustomerPhone').val($('#redirect_phoneNumber').val());
        $('#filterStatus').val($('#redirect_statusValue').val());
        currentPage = parseInt($('#redirect_currentPage').val());
        $('#filterSearchId').val($('#redirect_filterInvoiceId').val());
        $('#filterInvoiceListByDate').click();
    }
}
$('#invoiceList').on('page.dt', function () {
    currentPage = $('#invoiceList').DataTable().page.info().page;
});
function getTaxdetails() {
    var from = $('#fromDate').val();
    var to = $('#toDate').val();
    var filter = $('#invoiceList_filter')[0].firstElementChild.firstElementChild.value;
    $("#allTaxDetails").append(loaderDiv());
    $.ajax({
        url: '/InvoiceList/GetTaxDetails',
        type: "POST",
        data: '{from: ' + JSON.stringify(from) + ',to:' + JSON.stringify(to) + ',phNo:' + JSON.stringify($('#CustomerPhone').val()) + ',filter:' + JSON.stringify(filter) + ',status:' + JSON.stringify($('#filterStatus').val()) + ',invoiceId:' + JSON.stringify($('#filterSearchId').val())+'}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#allTaxDetails')[0].removeChild($('#allTaxDetails')[0].lastChild);
            if (result.data == 'success') {
                $('#Taxable').val('$' + parseFloat(result.taxable).toFixed(2))
                $('#Non-Taxable').val('$' + parseFloat(result.nontaxable).toFixed(2))
                $('#Total').val('$' + parseFloat(result.total).toFixed(2))
            }
            else {
                toastr.error('Something went wrong');
            }
        },
        error: function (result) {
            $('#allTaxDetails')[0].removeChild($('#allTaxDetails')[0].lastChild);
            toastr.error(result.data);
        }
    });
}
function loaderDiv() {
    return '<div class="overlay">' +
        '<i class="fas fa-2x fa-sync-alt fa-spin"></i>' +
        '</div>';
}
function changeInputValue(parm) {
    parm.value = parm.value == '' ? '' : ('$' + parm.value.replace(/[^\d\.]/g, ''));
}
function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode == 46) {
        return true;
    }
    else if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    return true;
}
function format(d) {
    var items = d.invoiceItems;
    var rows = '';
    items.forEach((itemGot) => {
        var othStones = itemGot.OtherStones == null ? 0 : itemGot.OtherStones;
        rows += '<tr>' +
            '<td>' + itemGot.Id + '</td>' +
            '<td>' + itemGot.Description + '</td>' +
            '<td>' + itemGot.CaratWeight + '</td>' +
            '<td>' + itemGot.GoldWeight + '</td>' +
            '<td>' + itemGot.GoldContent + '</td>' +
            '<td>' + itemGot.Pieces + '</td>' +
            '<td>' + othStones + '</td>' +
            '<td>' + itemGot.Price + '</td>' +
            '</tr>'
    })
    // `d` is the original data object for the row
    return '<table class="table table-bordered table-condensed" style="margin-top:5px;">' +
        '<thead style="font-weight: bold;"> <tr style="background-color: darkgrey">' +
        '<td>Id</td>' +
        '<td>Description</td>' +
        '<td>CaratWeight</td>' +
        '<td>GoldWeight</td>' +
        '<td>GoldContent</td>' +
        '<td>Pieces</td>' +
        '<td>OtherStones</td>' +
        '<td>Price</td>' +
        '</tr></thead>' +
        '<tbody>' + rows +
        '</tbody>' +
        '</table>';
}
$('#reservationdate').datetimepicker({
    format: 'MM-DD-yyyy'
});
$('#reservationdate2').datetimepicker({
    format: 'MM-DD-yyyy'
});
$('#invoiceList').on('search.dt', function () {
    getTaxdetails();
})
function loadInvoices() {
    var pageNumber = 0;
    if (!initial) {
        pageNumber = currentPage * 100
    }
    initial = false;
    $("#nestedTableDiv").removeClass("d-none")
    var table = $('#invoiceList').DataTable({
        "bServerSide": true,
        "sAjaxSource": '/InvoiceList/GetInvoiceListJsonData',
        "sAjaxDataProp": "aaData",
        "bProcessing": true,
        "dom": "<'row'<'col-sm-4'l><'col-sm-4'><'col-sm-4'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        "fnServerData": function (sSource, aoData, fnCallback) {
            aoData.push({ "name": "FromDate", "value": $('#fromDate').val() });
            aoData.push({ "name": "ToDate", "value": $('#toDate').val() });
            aoData.push({ "name": "PhoneNumber", "value": $('#CustomerPhone').val() });
            aoData.push({ "name": "statusSearch", "value": $('#filterStatus').val() });
            aoData.push({ "name": "statusSearchId", "value": $('#filterSearchId').val() });
            $.ajax({
                type: "POST",
                data: aoData,
                url: sSource,
                success: fnCallback
            })
        },
        "aoColumns": [
            {
                "className": 'details-control',
                "orderable": false,
                "data": null,
                "defaultContent": ''
            },
            { "data": "InvoiceDate", "orderable": false },
            { "data": "InvoiceId" },
            { "data": "Customer" },
            { "data": "Total" },
            { "data": "Employee", "orderable": false },
            { "data": "Status", "orderable": false },
            { "data": "Balance" },
            { "data": "ValidStatus", "visible": false },
            {
                "data": null,
                 "orderable": false,
                "mRender": function () {
                    return '<button class="btn btn-primary" onclick="editInvoiceRedirect(this)" title="Edit"><i class="fas fa-edit"></i></button><button class="btn btn-primary d-none" onclick="editInvoiceListFunction(this)"data-toggle="modal" data-target="#invoiceListModalPopup"  title="View/Update"><i class="fas fa-eye"></i></button> <button class="btn btn-primary" onclick="rePrintInvoiceForm(this)" data-toggle="modal" data-target="#rePrintModalPopup" title="Re-print!"><i class="fas fa-print"></i></button> <button class="btn btn-primary"  onclick="invoicePaymentFunction(this)"data-toggle="modal" data-target="#invoicePaymentModalPopup" title="Payments" ><i class="fas fa-money-check-alt"></i></button>&nbsp;<button id="deleteButton" class="btn btn-danger btn-icon" onclick = "deleteInvoiceFunction(this)" data-toggle="modal" data-target="#invoiceDeleteModalPopup" title="Delete Invoice"> <i class="fas fa-trash fa-xs"></i></button><button class="btn btn-primary" onclick = "getbackDeletedInvoice(this)" data-toggle="modal" data-target="#getbackInvoiceModalPopup" title = "Activate Invoice" > <i class="fas fa-trash-restore-alt"></i></button > ';
                }
            }
        ],
        'columnDefs': [
             { "width": "60px", "targets": 0 },
            { "width": "80px", "targets": 1 },
            { "width": "70px", "targets": 2 },
            { "width": "150px", "targets": 3 },
            { "width": "80px", "targets": 4 },
            { "width": "100px", "targets": 5 },
            { "width": "80px", "targets": 6 },
            { "width": "90px", "targets": 7 },
            { "width": "40px", "targets": 8 },
            { "width": "150px", "targets": 9 }
        ],
        "order": [[2, 'desc']],
        "language": {
            "emptyTable": "No data found...",
            "processing":
                '<i class="fa fa-spinner fa-spin fa-3x fa-fw" style="color:#2a2b2b;"></i><span class="sr-only">Loading...</span> '
        },
        "pageLength": 100,
        "displayStart": pageNumber,
        "responsive": true, "lengthChange": true, "autoWidth": true,
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            if (aData.Status == "Sold") {
                 nRow.cells[8].children[3].className += " d-none";
            }
            if ($('#disableUserId').val() != '1') {
                nRow.cells[8].children[4].className += " d-none";
                nRow.cells[8].children[5].className += " d-none";
            }
            if (aData.invoiceItems == null) {
                nRow.cells[0].classList.remove("details-control");
            }
            if (aData.ValidStatus == false) {
                $('td', nRow).css('color', 'Red');
            }
            if (aData.ValidStatus == false) {
                nRow.cells[8].children[0].className += " d-none";
                nRow.cells[8].children[1].className += " d-none";
                nRow.cells[8].children[2].className += " d-none";
                nRow.cells[8].children[4].className += " d-none";
                nRow.cells[8].children[3].className += " d-none";
            }
            if (aData.ValidStatus == true) {
                nRow.cells[8].children[5].className += " d-none";

            }
        },
        "initComplete": function (oSettings, json) {
            getTaxdetails();
        }
    });

    // Add event listener for opening and closing details
    $('#invoiceList tbody').on('click', 'td.details-control', function () {
        var tr = $(this).closest('tr');
        var row = table.row(tr);
        var a = row.data();
        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
            tr.removeClass('shown');
        }
        else {
            // Open this row
            row.child(format(row.data())).show();
            tr.addClass('shown');
        }
    });
}

document.getElementById('CustomerPhone').addEventListener('input', function (e) {
    var x = e.target.value.replace(/\D/g, '').match(/(\d{0,3})(\d{0,3})(\d{0,4})/);
    e.target.value = !x[2] ? x[1] : '(' + x[1] + ') ' + x[2] + (x[3] ? '-' + x[3] : '');
});//Masking Customer phone number
function editInvoiceRedirect(parm) {
    if ($(parm).parents("tr")[0].children[0].classList.contains("details-control")) {
        $("#editInvoiceId").val($(parm).parents("tr")[0].children[2].outerText);
        $("#Redirect").submit();
    }
    else {
        $('#paymentInvoiceId').val($(parm).parents("tr")[0].children[2].outerText);
        $.ajax({
            url: '/InvoiceList/GetLayawayInvoiceDetails',
            type: "POST",
            data: '{invoiceId: ' + JSON.stringify($(parm).parents("tr")[0].children[2].outerText) + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.data == 'error') {
                    toastr.error("Something went wrong");
                }
                else {
                    $('#editCashPayment').val("$" +result.values.Cash);
                    $('#editCreditCardPayment').val("$" +result.values.CreditCard);
                    $('#editCheckPayment').val("$" + result.values.Check);
                    $('#editBalanceDisplay').html('Balance : $' + parseFloat(result.balance).toFixed(2));
                }
            },
            error: function (result) {
                toastr.error(result.data);
            }
        });
        $('#editPaymentForm')[0].reset();
        $('#editCashPayment').removeClass('is-invalid');
        $('#editCreditCardPayment').removeClass('is-invalid');
        $('#editCheckPayment').removeClass('is-invalid');
        $('#editValidationMsg').hide();
        $("#editLayawayPayment").modal("show");
    }
}
$('#editPaymentForm').submit(function (e) {
    e.preventDefault();
    if (!($("#editCashPayment").hasClass('is-invalid') && $("#editCreditCardPayment").hasClass('is-invalid') && $("#editCheckPayment").hasClass('is-invalid'))) {
        if ($("#editCashPayment").val() == "" && $("#editCreditCardPayment").val() == "" && $("#editCheckPayment").val() == "" ) {
            $('#editValidationMsg').show();
        }
        else {
            $('#editCashPayment').removeClass('is-invalid');
            $('#editCreditCardPayment').removeClass('is-invalid');
            $('#editCheckPayment').removeClass('is-invalid');
            var invoiceId = $('#paymentInvoiceId').val();
            var model = {};
            model.Cash = $("#editCashPayment").val() == "" ? 0.00 : $("#editCashPayment").val() == "$" ? 0.00 : parseFloat($("#editCashPayment").val().replace(/[^\d\.]/g, '')).toFixed(2);
            model.CreditCard = $("#editCreditCardPayment").val() == "" ? 0.00 : $("#editCreditCardPayment").val() == "$" ? 0.00 : parseFloat($("#editCreditCardPayment").val().replace(/[^\d\.]/g, '')).toFixed(2);
            model.Check = $("#editCheckPayment").val() == "" ? 0.00 : $("#editCheckPayment").val() == "$" ? 0.00 : parseFloat($("#editCheckPayment").val().replace(/[^\d\.]/g, '')).toFixed(2);
            model.Total = parseFloat(parseFloat(model.Cash) + parseFloat(model.CreditCard) + parseFloat(model.Check)).toFixed(2);
            model.InvoiceId = invoiceId;
            $.ajax({
                url: '/InvoiceList/UpdateLayawayPayment',
                type: "POST",
                data: '{model: ' + JSON.stringify(model) + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result.data == 'error') {
                        toastr.error("Something went wrong");
                    }
                    else {
                        window.location = result.url;
                    }
                },
                error: function (result) {
                    toastr.error(result.data);
                }
            });
        }

    }
    else {
        $('#editValidationMsg').show();
    }

});
function invoiceUpdateFormSubmit() {
    $("#invoiceListUpdateForm").submit(function (e) {
        var model = {};
        model.CustomerPhone = $('#CustomerPhone').val();
        model.InvoiceId = $('#hiddenInvoiceId').val();
        model.Customer = $('#Customer').val();
        model.Address = $('#Address').val();
        model.DLNumber = $('#DLNumber').val();
        model.Company = $('#Company').val();
        model.DOB = $('#DOB').val();
        model.ExpiryDate = $('#ExpDate').val();
        model.Check = $('#Check').val();
        model.CreditCard = $('CreditCard').val();
        model.Cash = $('#Cash').val();
        model.Status = $('#Status').val();
        model.Balance = $('#Balance').val();
        model.Tax = $('#Tax').val();
        e.preventDefault(); //prevent default form submit
        $.ajax({
            url: '/InvoiceList/ViewOrUpdate',
            type: "POST",
            data: '{model: ' + JSON.stringify(model) + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                $('#invoiceList').dataTable().fnClearTable();
                $('#invoiceList').dataTable().fnDestroy();
                $("#invoiceListModalPopup").modal('hide');
                loadInvoices();
                toastr.success('Invoice ' + result.data + ' successfully');
            },
            error: function (result) {
                toastr.error(result.data);
            }
        });
    });
}
function rePrintInvoiceForm(parm) {
    var id = ($(parm).parents("tr")[0].children[2].outerText)
    $('#rePrintInvoiceId').val(id);
    var fromDate = $("#fromDate").val();
    var toDate = $("#toDate").val();
    var phoneNumber = $("#CustomerPhone").val();
    var statusValue = $("#filterStatus").val();
    var filterInvoiceId = $("#filterSearchId").val();
    $.ajax({
        url: '/InvoiceList/ReprintInvoice',
        type: "POST",
        data: '{invoiceId: ' + JSON.stringify(id) + ',fromDate:' + JSON.stringify(fromDate) + ',toDate:' + JSON.stringify(toDate) + ',phoneNumber:' + JSON.stringify(phoneNumber) + ',currentPage:' + JSON.stringify(currentPage) + ',statusValue:' + JSON.stringify(statusValue) + ',filterInvoiceId:' + JSON.stringify(filterInvoiceId) + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result.data == 'error') {
                toastr.error("Something went wrong");
            }
            else {
                 window.location = result.url;
              
            }
        },
        error: function (result) {
            toastr.error(result.data);
        }
    });
}
$('#clearDates').click(function () {
    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
    var yyyy = today.getFullYear();
    $('#fromDate').val(mm + '-' + dd + '-' + yyyy);
    $('#toDate').val(mm + '-' + dd + '-' + yyyy);
    $('#CustomerPhone').val('');
    $('#Taxable').val('');
    $('#Non-Taxable').val('');
    $('#Total').val('');
    $('#filterStatus').val('true');
    $('#filterStatus').val('true');
    $('#filterSearchId').val('');
    
    if (initial) {

    }
    else {
        $("#nestedTableDiv").addClass("d-none");
        $('#invoiceList').DataTable().clear();
        $('#invoiceList').DataTable().destroy();        
        initial = true;
    }
});
function editInvoiceListFunction(parm) {
    $('#Balance').val($(parm).parents("tr")[0].children[7].outerText);
    $('#subTotal').val($(parm).parents("tr")[0].children[4].outerText);
    $('#hiddenInvoiceId').val($(parm).parents("tr")[0].children[2].outerText);
    var invoiceId = $('#hiddenInvoiceId').val();
    $.ajax({
        url: '/InvoiceList/GetCustomerDetailsandInvoicePaymentsByInvoiceId',
        type: "POST",
        data: '{invoiceId: ' + JSON.stringify(invoiceId) + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#Customer').val(result.data.CustomerName);
            $('#CustomerPhone').val(result.data.CustomerPhone);
            $('#Address').val(result.data.Address);
            $('#DLNumber').val(result.data.DLNumber);
            $('#DOB').val(result.data.DOB);
            $('#ExpDate').val(result.data.ExpiryDate);
            $('#Check').val(result.data.Check);
            $('#CreditCard').val(result.data.CreditCard);
            $('#Cash').val(result.data.Cash);
            $('#Status').val(result.data.Status);
            setSlected('#Tax', result.data.Tax);
            $('#Company').val(result.data.Company);
        },
        error: function (result) {
            toastr.error(result.data);
        }
    });
}
function setSlected(element, text) {
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
function invoicePaymentFunction(parm) {
  
    $('#paymentInvoiceId').val($(parm).parents("tr")[0].children[2].outerText);
    if (parseFloat($(parm).parents("tr")[0].children[7].outerText) == 0) {
        $('#addPaymentbtn').addClass('d-none');
    }
    else {
        $('#addPaymentbtn').removeClass('d-none');
    }
    $('#balanceDisplay').html('Balance : ' + $(parm).parents("tr")[0].children[7].outerText);
    var invoiceId = $('#paymentInvoiceId').val();
    $.ajax({
        url: '/InvoiceList/GetPaymentDetailsByInvoiceId',
        type: "POST",
        data: '{invoiceId: ' + JSON.stringify(invoiceId) + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#paymentTable tbody').html("");
            result.data.forEach((value) => {
             
                $("#paymentTable tbody").append(
                    "<tr>"
                    + "<td>" + value.Id + "</td>"
                    + "<td>$" + parseFloat(value.PaymentAmount).toFixed(2) + "</td>"
                    + "<td>$" + parseFloat(value.Cash).toFixed(2) + "</td>"
                    + "<td>$" + parseFloat(value.CreditCard).toFixed(2) + "</td>"
                    + "<td>$" + parseFloat(value.Cheque).toFixed(2) + "</td>"
                    + "<td>" + value.DisplayDate + "</td>"
                    + "<td><button onclick='printFunction(this," + value.NewInvoiceId + ")' type='button'  class='btn btn-primary'>Print</button></td>"
                    + "</tr>"
                )
            });

        },
        error: function (result) {
            toastr.error(result.data);
        }
    });
}
function validateaddPaymentForm() {
    $('#addNewPaymentForm').validate({
        rules: {
            cashPayment: {
                required: true
            },
            creditCardPayment: {
                required: true
            },
            checkPayment: {
                required: true
            } ,
          
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
function validateEditPaymentForm() {
    $('#editPaymentForm').validate({
        rules: {
            editCashPayment: {
                required: true
            },
            editCreditCardPayment: {
                required: true
            },
            editCheckPayment: {
                required: true
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
function changeFormat(parm) {
  
    parm.value = parm.value.replace(/[^\d\.]/g, '') == '' ? '' : '$' + parseFloat(parm.value.replace(/[^\d\.]/g, '')).toFixed(2);
   
}
document.querySelector('#cashPayment').addEventListener('input', function () {
    $('#creditCardPayment').removeClass('is-invalid');
    $('#checkPayment').removeClass('is-invalid');
    $('#validationMsg').hide();
});
document.querySelector('#creditCardPayment').addEventListener('input', function () {
    $('#cashPayment').removeClass('is-invalid');
    $('#checkPayment').removeClass('is-invalid');
    $('#validationMsg').hide();
});
document.querySelector('#checkPayment').addEventListener('input', function () {
    $('#creditCardPayment').removeClass('is-invalid');
    $('#cashPayment').removeClass('is-invalid');
    $('#validationMsg').hide();
});
document.querySelector('#editCashPayment').addEventListener('input', function () {
    $('#editCreditCardPayment').removeClass('is-invalid');
    $('#editCheckPayment').removeClass('is-invalid');
    $('#editValidationMsg').hide();
});
document.querySelector('#editCreditCardPayment').addEventListener('input', function () {
    $('#editCashPayment').removeClass('is-invalid');
    $('#editCheckPayment').removeClass('is-invalid');
    $('#editValidationMsg').hide();
});
document.querySelector('#editCheckPayment').addEventListener('input', function () {
    $('#editCreditCardPayment').removeClass('is-invalid');
    $('#editCashPayment').removeClass('is-invalid');
    $('#editValidationMsg').hide();
});                                    
function addNewPaymentFormSubmit() {
    $('#addNewPaymentForm').submit(function (e) {
        e.preventDefault();
        if (!($("#cashPayment").hasClass('is-invalid') && $("#creditCardPayment").hasClass('is-invalid') && $("#checkPayment").hasClass('is-invalid'))) {
            clearValidation();
            var invoiceId = $('#paymentInvoiceId').val();
            var model = {};
            model.Cash = $("#cashPayment").val() == "" ? 0.00 : $("#cashPayment").val() == "$" ? 0.00 : parseFloat($("#cashPayment").val().replace(/[^\d\.]/g, '')).toFixed(2);
            model.CreditCard = $("#creditCardPayment").val() == "" ? 0.00 : $("#creditCardPayment").val() == "$" ? 0.00 : parseFloat($("#creditCardPayment").val().replace(/[^\d\.]/g, '')).toFixed(2);
            model.Check = $("#checkPayment").val() == "" ? 0.00 : $("#checkPayment").val() == "$" ? 0.00 : parseFloat($("#checkPayment").val().replace(/[^\d\.]/g, '')).toFixed(2);
            model.Total = parseFloat(parseFloat(model.Cash) + parseFloat(model.CreditCard) + parseFloat(model.Check)).toFixed(2);
            model.InvoiceId = invoiceId;            
            $.ajax({
                url: '/InvoiceList/AddNewInvoice',
                type: "POST",
                data: '{model: ' + JSON.stringify(model) + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result.data == 'error') {
                        toastr.error("Something went wrong");
                    }
                    else {
                        window.location = result.url;
                    }
                },
                error: function (result) {
                    toastr.error(result.data);
                }
            });
        }
        else {
            $('#validationMsg').show();
        }
     
    });
};

function printFunction(parm, invoiceId) {
    var id = $(parm).parents("tr")[0].children[0].outerText;
    $.ajax({
        url: '/InvoiceList/PrintInvoiceFunction',
        type: "POST",
        data: '{invoiceId: ' + JSON.stringify(invoiceId) + ',paymentId:' + JSON.stringify(id) + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result.data == 'error') {
                toastr.error("Something went wrong");
            }
            else {
                window.location = result.url;
            }
        },
        error: function (result) {
            toastr.error(result.data);
        }
    });
}
function deleteInvoiceFunction(parm) {
    $('#deleteInvoiceName').text($(parm).parents("tr")[0].children[2].outerText);
    $('#deleteInvoiceId').val($(parm).parents("tr")[0].children[2].outerText);
}

function addPaymentFunction() {
    $('#validationMsg').hide();
    $('#addNewPaymentForm')[0].reset();
    clearValidation();
    
};

$("#filterInvoiceListByDate").click(function () {
    if (initial) {
        loadInvoices();
    }
    else {
        $('#invoiceList').DataTable().ajax.reload(function () {
            //getTaxdetails();
        });
    }   
    //getTaxdetails();
});
function deleteInvoiceForm() {
    $("#invoiceListDeleteForm").submit(function (e) {
        e.preventDefault();
        var id = $("#deleteInvoiceId").val();
        $.ajax({
            url: '/InvoiceList/DeleteInvoice',
            type: "POST",
            data: '{invoiceId: ' + JSON.stringify(id) + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                $('#invoiceList').dataTable().fnClearTable();
                $('#invoiceList').dataTable().fnDestroy();
                $("#invoiceDeleteModalPopup").modal('hide');
                loadInvoices();
                toastr.success('Invoice ' + result.data + ' successfully');
            },
            error: function (result) {
                toastr.error(result.data);
            }
        });
    });
}
function getbackInvoiceInvoiceForm() {
    $("#getbackDeletedInvoice").submit(function (e) {
        e.preventDefault();
        var id = $("#getbackInvoiceId").val();
        $.ajax({
            url: '/InvoiceList/GetBackDeletedInvoice',
            type: "POST",
            data: '{invoiceId: ' + JSON.stringify(id) + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                $('#invoiceList').dataTable().fnClearTable();
                $('#invoiceList').dataTable().fnDestroy();
                $("#getbackInvoiceModalPopup").modal('hide');
                loadInvoices();
                toastr.success('Invoice ' + result.data + ' successfully');
            },
            error: function (result) {
                toastr.error(result.data);
            }
        });
    });
}
function clearValidation() {
    $('#cashPayment').removeClass('is-invalid');
    $('#creditCardPayment').removeClass('is-invalid');
    $('#checkPayment').removeClass('is-invalid');
}
function getbackDeletedInvoice(parm) {
    $('#getBackInvoiceName').text($(parm).parents("tr")[0].children[2].outerText);
    $('#getbackInvoiceId').val($(parm).parents("tr")[0].children[2].outerText);
}