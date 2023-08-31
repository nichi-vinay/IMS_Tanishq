$(function () {
 
    validateAdHocForm();
    adHocSubmitForm();
    loadInventoryData();
    $("#StatusSearch")[0].children[2].removeAttribute("selected");
    $('#CategorySearch').val('');
    setSlected('#EmployeeId', $('#UserName').val());
    setSlected('#Company', 'Kirti Jewelers');
    $('#Status').val('5');
    $('#Tax').val('INC');
    var invoiceId = $('#redirect').val();
    if (invoiceId != "" && invoiceId != undefined) {
        InvoiceEdit(invoiceId)
    }
    $("price").attr("autocomplete", "off");
});
function InvoiceEdit(invoiceId) {
  
    $('#btnPayment').addClass("d-none");
    $('#btnEditPayment').removeClass("d-none");
    $.ajax({
        url: '/Invoice/GetInvoiceDetails',
        type: "POST",
        data: '{invoiceId: ' + invoiceId + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            
            addInvoiceItems(result.items);
            setInvoiceDetails(result.invoiceDetails)
          
        },
        error: function (result) {
            toastr.error("Unable to fetch invoice items")
        }
    })
}
function setInvoiceDetails(invoice) {
    $('#DLNumber').val(invoice.DLNumber);
    $('#ExpDate').val(invoice.ExpDate);
    $('#DOB').val(invoice.DOB);
    $('#CustomerName').val(invoice.CustomerName);
    $('#CustomerPhone').val(invoice.CustomerPhone);
    $('#Address').val(invoice.Address);
    $('#Email').val(invoice.Email);
    $('#State').val(invoice.State);
    $('#City').val(invoice.City);
    $('#Zip').val(invoice.Zip);
    $('#CompanyId').val(invoice.CompanyId);
    $('#EmployeeId').val(invoice.UserId);
    $('#Tax').val(invoice.TaxType);
    $('#Status').val(invoice.InventoryStatusId);
    $('#Check').val(invoice.Check == '' ? '' : '$' + invoice.Check);
    $('#CreditCard').val(invoice.CreditCard == '' ? '' : '$' + invoice.CreditCard);
    $('#Cash').val(invoice.Cash == '' ? '' : '$' + invoice.Cash);
}
function addInvoiceItems(items) {
    items.forEach(function (item) {
        $('#primaryDiv').append(loaderDiv());
        $.ajax({
            url: '/InventoryMaster/FindInventoryImage',
            type: "POST",
            data: '{Id: ' + item.InventoryId + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                $('#primaryDiv')[0].removeChild($('#primaryDiv')[0].lastChild);

                if (result.values.Id == null) {
                    toastr.error("No Item found");
                }
                else {
                    var body = $("#tableDestroy").find('tbody');
                    var allRows = body.find('tr').length;
                    if (body.find('tr').length >= 10) {
                        toastr.error("Max items reached, Proceed for payment")
                    }
                    else {
                        setValues(result.values);
                    }
                    clearFilter();
                    $('#Barcode').val('');
                    body.find('tr')[allRows].children[7].children[0].value = '$' + item.Price;
                    var total = 0.00;
                    var rows = body.find('tr');
                    $.each(rows, function (index, number) {
                        total = (parseFloat(total) + parseFloat(number.children[7].firstElementChild.value == '' ? 0 : number.children[7].firstElementChild.value.replace(/[^\-\d\.]/g, ''))).toFixed(2);
                    });
                    $("#totalValueText").text("$" + total);
                }
            },
            error: function (result) {
                $('#primaryDiv')[0].removeChild($('#primaryDiv')[0].lastChild);
                toastr.error("Unable to load items")
            }
        });
        clearFilter();
    })
}
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
function loadDataToCard(lst) {
    var divelements = "";
    lst.forEach(function (value) {
        if (value.InventoryStatusId == 6) {
            var body = $("#tableDestroy").find('tbody');
            var allRows = body.find('tr');
            var arrayOfId = new Array();
            var button = '';
            var selectedMessage = '';
            $.each(allRows, function (index, row) {
                arrayOfId.push(row.children[0].textContent)
            })
            if (arrayOfId.length != 0 && arrayOfId.includes((value.Id).toString())) {
                button = '<button class="btn btn-primary btn-icon" onclick="selectInventoryFunction(this,' + value.Id + ')" disabled="disabled"><i class="fa fa-check-square fa-xs"></i></button>';
                selectedMessage = '<p class="text-sm text-danger">Item already exists in the list</p>';
            }
            else {
                button = '<button class="btn btn-primary btn-icon" onclick="selectInventoryFunction(this,' + value.Id + ')"><i class="fa fa-check-square fa-xs"></i></button>';
            }
            var inActive = value.StatusName == "In-active" ? true : value.StatusName == "In-Active" ? true : false;
            var textColor = '';
            if (inActive) {
                button = '<button class="btn btn-primary btn-icon" onclick="selectInventoryFunction(this,' + value.Id + ')" disabled="disabled"><i class="fa fa-check-square fa-xs"></i></button>';
                textColor = 'style="color:red !important"';
            }
            divelements += '<div class="col-12 col-sm-6 col-md-4 d-flex align-items-stretch flex-column">' +
                '<div class="card bg-light d-flex flex-fill">' +
                '<div class="card-header text-muted border-bottom-0"></div>' +
                '<div class="card-body pt-0">' +
                '<div class="row">' + '<div class="col-12 text-center">' + '<img src="' + value.ImageSrc + '" class="img-fluid rounded" style="width: 100%;height:205px;border-width: 2px !important;">' + '</div>' +
                '<div class="col-12" >' + '<h3 class="lead" style="text-align: center;"><b>' + value.Category + '</b></h3>' +
                '<div class="row">' +
                '<div class="col-6">' +
                '<div class="row"><p class="text-muted text-sm col-7"><b>JewelType </b></p> <p class="text-muted text-sm col-5">:' + value.JewelType + ' </p></div>' +
                '<div class="row"><p class="text-muted text-sm col-7"><b>CaratWeight </b></p> <p class="text-muted text-sm col-5">: ' + value.CaratWeight + ' </p></div>' +
                '<div class="row"><p class="text-muted text-sm col-7"><b>GoldWeight </b></p> <p class="text-muted text-sm col-5">: ' + value.GoldWeight + ' </p></div>' +
                '<div class="row"><p class="text-muted text-sm col-7"><b>Pieces </b></p> <p class="text-muted text-sm col-5">: ' + value.Pieces + ' </p></div>' +

                '</div>' +
                '<div class="col-6">' +
                '<div class="row"><p class="text-muted text-sm col-7"><b>Price </b></p> <p class="text-muted text-sm col-5">: ' + value.Price + ' </p></div>' +
                '<div class="row"><p class="text-muted text-sm col-7"><b>DateReceived </b></p> <p class="text-muted text-sm col-5">: ' + value.DateReceived + ' </p></div>' +
                '<div class="row"><p class="text-muted text-sm col-7"><b>Status </b></p> <p class="text-muted text-sm col-5"' + textColor + ' >: ' + value.StatusName + ' </p></div>' +
                '</div>' +
                '<div class="col-12 row"><p class="text-muted text-sm col-4"><b>Description </b></p> <p class="text-muted text-sm col-8" style="padding:0px">: ' + value.Description + ' </p></div>' +
                '<div class="col-12 row"><p class="text-muted text-sm col-4"><b>Company </b></p> <p class="text-muted text-sm col-8" style="padding:0px">: ' + value.Company + ' </p></div>' +
                '<div class="col-12 row"><p class="text-muted text-sm col-4"><b>Supplier </b></p> <p class="text-muted text-sm col-8" style="padding:0px">: ' + value.Supplier + ' </p></div>' +
                '</div>' +
                '</div>' + '</div>' + '</div>' +
                '<div class="card-footer"> <div class="row"><div class="col-9"><p class="text-muted text-sm"><b>Item ID : </b>' + value.Id + '(' + value.InventoryStatus + ')</p>' + selectedMessage + '</div>' +
                '<div class="col-3 text-right"><a id="tooltip" data-toggle="tooltip" data-placement="top" title="Select Inventory">' + button + '</a ></div></div>' + '</div>' + '</div>' + '</div>'

        }
        $("#addContents").html(divelements)
    })

}
function ajaxCall(model) {

    $("#addContents").html("");
    $('#startingDiv').append(loaderDiv());
    $.ajax({
        url: '/InventoryMaster/GetInventoryInstockJsonData',
        type: "POST",
        data: '{param: ' + JSON.stringify(model) + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadDataToCard(result.aaData);
            setPagination(result.iTotalRecords, result.iDisplayStart);
            $('#startingDiv')[0].removeChild($('#startingDiv')[0].lastChild);
        },
        error: function (result) {
            toastr.error("");
            $('#startingDiv')[0].removeChild($('#startingDiv')[0].lastChild);
        }
    });
}
function loadInventoryData() {
    var model = {};
    model.sEcho = "1";
    model.iDisplayLength = 12;
    model.iDisplayStart = 0;
    model.iColumns = 5;
    model.iSortCol_0 = 0;
    model.sSortDir_0 = "dec";
    model.iSortingCols = 1;
    model.sColumns = ",,,,";
    ajaxCall(model);
}
function setPagination(totalRecords, iDisplayStart) {

    var value = parseInt(Number(totalRecords) / 12);
    if (Number(totalRecords) % 12 != 0) {
        value += 1;
    }
    for (var i = 1; i < 8; i++) {
        $("ul.pagination")[0].children[i].classList.remove("d-none");
    }
    if (Number(totalRecords) == 0) {
        for (var i = 1; i < 8; i++) {
            $("ul.pagination")[0].children[i].classList.add("d-none");
        }
        toastr.warning('No items found');
    }
    else if (value > 6) {
        $("ul.pagination")[0].lastElementChild.previousElementSibling.innerHTML = "<a class=\"page-link\" href='#'>" + value + "</a>";
    }
    else if (value == 6) {
        $("ul.pagination")[0].children[6].classList.add("d-none");
        $("ul.pagination")[0].lastElementChild.previousElementSibling.innerHTML = "<a class=\"page-link\" href='#'>" + value + "</a>";
    }
    else if (value == 0) {
        $("ul.pagination")[0].lastElementChild.classList.add('disabled');
        for (var i = 0; i < 7; i++) {
            $("ul.pagination")[0].lastElementChild.previousElementSibling.classList.add("d-none");
        }
    }
    else {
        $("ul.pagination")[0].children[6].classList.add("d-none");
        for (var i = 1; i < $("ul.pagination")[0].children.length; i++) {
            if ($("ul.pagination")[0].children[i].innerText > value) {
                $("ul.pagination")[0].children[i].classList.add("d-none");
            }
        }
    }
    var a = loaderDiv();
    if ((Number(iDisplayStart) / 12) + 1 == value) {
        $("ul.pagination")[0].lastElementChild.classList.add('disabled');
    }
    else {
        $("ul.pagination")[0].lastElementChild.classList.remove('disabled');
    }
    if ((Number(iDisplayStart) / 12) + 1 == 1) {
        $("ul.pagination")[0].firstElementChild.classList.add('disabled');
    }
    else {
        $("ul.pagination")[0].firstElementChild.classList.remove('disabled');
    }
    var list = $("ul.pagination")[0].children;
    Array.from(list).forEach(function (value) {
        if (value.innerText == ((Number(iDisplayStart) / 12) + 1).toString()) {
            value.classList.add('active');
        }
        else {
            value.classList.remove('active');
        }
    })
}
$('#previousButton').click(function () {
    if (!$('#previousButton')[0].classList.contains('disabled')) {
        var list = $("ul.pagination")[0].children;
        var number = '';
        var changePagination = false;
        Array.from(list).forEach(function (value) {
            if (value.classList.contains('active')) {

                if (value.previousElementSibling.innerText == "...") {
                    changePagination = true;
                }
                number = value.innerText;
            }
        })
        if (changePagination) {

            list[2].classList.add("disabled");
            list[2].innerHTML = "<a class='page-link' href='#'>...</a>";
            list[3].innerHTML = "<a class='page-link' href='#'>" + (Number(number) - 3) + "</a>";
            list[4].innerHTML = "<a class='page-link' href=\"#\">" + (Number(number) - 2) + "</a>";
            list[5].innerHTML = "<a class='page-link' href=\"#\">" + (Number(number) - 1) + "</a>";

        }
        if (Number(number) <= 6) {
            list[2].classList.remove("disabled");
            list[2].innerHTML = "<a class='page-link' href='#'>" + 2 + "</a>";
            list[3].innerHTML = "<a class='page-link' href='#'>" + 3 + "</a>";
            list[4].innerHTML = "<a class='page-link' href=\"#\">" + 4 + "</a>";
            list[5].innerHTML = "<a class='page-link' href=\"#\">" + 5 + "</a>";
        }
        paginationOperation((Number(number) - 1).toString());
    }
})
$('#nextButton').click(function () {
    if (!$('#nextButton')[0].classList.contains('disabled')) {

        var list = $("ul.pagination")[0].children;
        var number = '';
        
        var changePagination = false;
        Array.from(list).forEach(function (value) {
            if (value.classList.contains('active')) {
                if (value.nextElementSibling.innerText == "..." && value.innerText != "1") {
                    changePagination = true;
                }
                if (value.nextElementSibling.innerText == "..." && value.innerText == "1") {
                    list[2].classList.remove("disabled");
                    list[2].innerHTML = "<a class='page-link' href='#'>" + 2 + "</a>";
                    list[3].innerHTML = "<a class='page-link' href='#'>" + 3 + "</a>";
                    list[4].innerHTML = "<a class='page-link' href=\"#\">" + 4 + "</a>";
                    list[5].innerHTML = "<a class='page-link' href=\"#\">" + 5 + "</a>";
                }
                number = value.innerText;
            }
        })
        if (changePagination) {
            list[2].classList.add("disabled");
            list[2].innerHTML = "<a class='page-link' href='#'>...</a>";
            list[3].innerHTML = "<a class='page-link' href='#'>" + (Number(number) - 1) + "</a>";
            list[4].innerHTML = "<a class='page-link' href=\"#\">" + Number(number) + "</a>";
            list[5].innerHTML = "<a class='page-link' href=\"#\">" + (Number(number) + 1) + "</a>";
        }


        paginationOperation((Number(number) + 1).toString());
        if ((Number(number) + 1) == Number(list[7].innerText)) {
            list[8].classList.add('disabled')
        }
        else {
            list[8].classList.remove('disabled')
        }
    }

})
$('.page-item').on('click', function (e) {
    var pageNumber = e.target.innerHTML;

    if (!isNaN(pageNumber)) {
        paginationOperation(pageNumber);
    }
});
function paginationOperation(pageNumber) {
    if (!isNaN(pageNumber)) {
        var model = {};
        model.sSearch = $("#idSearch").val();
        model.categorySearch = $("#CategorySearch").val();
        model.suplierSearch = $("#SupplierSearch").val();
        model.statusSearch = $("#StatusSearch").val();
        model.sEcho = "1";
        model.iDisplayLength = 12;
        model.iDisplayStart = (parseInt(pageNumber, 10) - 1) * 12;
        model.iColumns = 5;
        model.iSortCol_0 = 0;
        model.sSortDir_0 = "dec";
        model.iSortingCols = 1;
        model.sColumns = ",,,,";
        ajaxCall(model);
    }
}
$('#ImageModal').on('hidden.bs.modal', function (e) {
    $('#InventoryModal').modal('show');
})
$('#invoicePrintForm').keydown(function (e) {
    if (e.keyCode == 13) {
        e.preventDefault();
        return false;
    }
})
$('#invoicePrintForm').submit(function (e) {
    e.preventDefault();
})
$('#paymentConfirm').click(function (e) {
    e.preventDefault();
    if (!e.detail || e.detail == 1) {
        if (($('#CustomerName').val() == '')) {
            $('#validateCustomerName').removeClass('d-none');
            $('#CustomerName').css('border-color', 'red');
        }
        if ($("#CompanyId").val() == '') {
            $('#validateCompany').removeClass('d-none');
            $('#CompanyId').css('border-color', 'red');
        }
        if ($("#EmployeeId").val() == '') {
            $('#validateEmployee').removeClass('d-none');
            $('#EmployeeId').css('border-color', 'red');
        }
        else if ($('#Check').val() != '' || $('#CreditCard').val() != '' || $('#Cash').val() != '') {
            var body = $("#tableDestroy").find('tbody');
            var allRows = body.find('tr');
            var arrayOfId = new Array();
            var arrayOfPrice = new Array();
            var arrayOfGoldWeight = new Array();
            var arrayOfCaratWeight = new Array();
            var arrayOfGoldContent = new Array();
            var arrayOfPieces = new Array();
            var arrayOfDescription = new Array();
            var arrayOfCompanyId = new Array();
            var arrayOfCategoryId = new Array();
            var arrayOfJewelTypeId = new Array();
            var arrayOfSupplier = new Array();
            var arrayOfStatus = new Array();
            var arrayOfDiamondPieces = new Array();
            var arrayOfInventoryStatus = new Array();
            var arrayOfDateReceived = new Array();            
            $.each(allRows, function (index, row) {
                arrayOfId.push(row.children[0].textContent);
                arrayOfGoldWeight.push(row.children[1].textContent);
                arrayOfCaratWeight.push(row.children[2].textContent);
                arrayOfGoldContent.push(row.children[3].textContent);
                arrayOfPieces.push(row.children[4].textContent);
                arrayOfDescription.push(row.children[5].textContent);
                arrayOfPrice.push(row.children[7].firstElementChild.value.replace(/[^\-\d\.]/g, ''));
                if (row.children[0].textContent == "") {
                    arrayOfCompanyId.push(row.children[9].textContent);
                    arrayOfCategoryId.push(row.children[10].textContent);
                    arrayOfJewelTypeId.push(row.children[11].textContent);
                    arrayOfSupplier.push(row.children[12].textContent);
                    arrayOfStatus.push(row.children[13].textContent);
                    arrayOfDiamondPieces.push(row.children[14].textContent);
                    arrayOfInventoryStatus.push(row.children[15].textContent);
                    arrayOfDateReceived.push(row.children[16].textContent);
                }
                else {
                    arrayOfCompanyId.push("");
                    arrayOfCategoryId.push("");
                    arrayOfJewelTypeId.push("");
                    arrayOfSupplier.push("");
                    arrayOfStatus.push("");
                    arrayOfDiamondPieces.push("");
                    arrayOfInventoryStatus.push("");
                    arrayOfDateReceived.push("");
                }
            })
            printBill(arrayOfId, arrayOfCompanyId, arrayOfCategoryId, arrayOfJewelTypeId, arrayOfGoldWeight, arrayOfCaratWeight, arrayOfGoldContent, arrayOfPieces, arrayOfDescription, arrayOfPrice, arrayOfSupplier, arrayOfStatus, arrayOfDiamondPieces, arrayOfInventoryStatus, arrayOfDateReceived);
        }
        else {
            toastr.error('Please enter valid price');
        }
    }
})
$('#CustomerName').on('keyup', function () {

    if ($('#CustomerName').val() != '') {
        $('#validateCustomerName').addClass('d-none');
        $('#CustomerName').css('border-color', '');
    }
    else {
        $('#validateCustomerName').removeClass('d-none');
        $('#CustomerName').css('border-color', 'red');
    }
});

$('#CompanyId').on('change', function () {
    if ($('#CompanyId').val() != '') {
        $('#validateCompany').addClass('d-none');
        $('#CompanyId').css('border-color', '');
    }
    else {
        $('#validateCompany').removeClass('d-none');
        $('#CompanyId').css('border-color', 'red');
    }
});
$('#EmployeeId').on('change ', function () {
    if ($('#EmployeeId').val() != '') {
        $('#validateEmployee').addClass('d-none');
        $('#EmployeeId').css('border-color', '');
    }
    else {
        $('#validateEmployee').removeClass('d-none');
        $('#EmployeeId').css('border-color', 'red');
    }
});
$('#Status').on('change', function () {
    if ($('#Status').val() == '1') {
        $('#Cash').val('0')
    }
    else {
        $('#Cash').val('')
    }
});
$('#searchCustomer').click(function () {
    $('#CustomerName').val('');
    $('#CustomerPhone').val('');
    $('#Address').val('');
    $('#DLNumber').val('');
    $('#ExpDate').val('');
    $('#DOB').val('');
    $('#Email').val('');
    searchSingleCustomer();
});

$('#ScanId')[0].addEventListener('keydown', e => {

   
    if ((e.ctrlKey && e.which == 74) || (e.ctrlKey && e.which == 77) || (e.ctrlKey && e.which == 13) || (e.which == 13)) {
        $('#ScanId').val($('#ScanId').val() + '\n');
        e.preventDefault();
    }

});

function fetchDLData() {
   
}
function searchSingleCustomer() {
    $('#CustomerModal .modal-dialog').append(loaderDiv());
    $.ajax({
        url: '/Invoice/GetCustomerByDlOrPhone',
        type: "POST",
        data: '{value: ' + JSON.stringify($('#ScanId').val()) + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result.data == "null") {
                $('#CustomerModal .modal-dialog')[0].removeChild($('#CustomerModal .modal-dialog')[0].lastChild);
                toastr.error("No Customer found");
            }
            else {
                $('#CustomerModal .modal-dialog')[0].removeChild($('#CustomerModal .modal-dialog')[0].lastChild);
                $('#CustomerName').val(result.data.CustomerName);
                $('#CustomerPhone').val(result.data.CustomerPhone == '(   )    -' ? '' : result.data.CustomerPhone == null ? '' : result.data.CustomerPhone);
                $('#Address').val(result.data.Address);
                $('#DLNumber').val(result.data.DLNumber);
                $('#ExpDate').val(result.data.ExpDate);
                $('#DOB').val(result.data.DOB);
                $('#Email').val(result.data.Email);
                $('#State').val(result.data.State);
                $('#City').val(result.data.City);
                $('#Zip').val(result.data.Zip);
                $('#CustomerName').removeClass('is-invalid');
                $('#CustomerPhone').removeClass('is-invalid');
                $('#Address').removeClass('is-invalid');
                $('#DLNumber').removeClass('is-invalid');
                $('#ExpDate').removeClass('is-invalid');
                $('#DOB').removeClass('is-invalid');
                $('#Email').removeClass('is-invalid');
                $('#ScanId').val('');
            }
        },
        error: function (result) {
            toastr.error("something went wrong");
            $('#CustomerModal .modal-dialog')[0].removeChild($('#CustomerModal .modal-dialog')[0].lastChild);
        }
    });
}
$('#clearCustomerId').on('click', function () {
    $('#ScanId').val('');
});
function printBill(arrayId, arrayOfCompanyId, arrayOfCategoryId, arrayOfJewelTypeId, arrayOfGoldWeight, arrayOfCaratWeight, arrayOfGoldContent, arrayOfPieces, arrayOfDescription, arrayPrice, arrayOfSupplier, arrayOfStatus, arrayOfDiamondPieces, arrayOfInventoryStatus, arrayOfDateReceived) {
    var model = {};
    model.CustomerName = $("#CustomerName").val();
    model.CustomerPhone = $("#CustomerPhone").val();
    model.Address = $("#Address").val();
    model.State = $("#State").val();
    model.City = $("#City").val();
    model.Zip = $("#Zip").val();
    model.DOB = $("#DOB").val();
    model.ExpDate = $("#ExpDate").val();
    model.DLNumber = $("#DLNumber").val();
    model.Email = $("#Email").val();
    model.InventoryStatusId = $("#Status").val();
    model.EmployeeId = $("#EmployeeId").val();
    model.Tax = $("#Tax").val();
    model.SubTotal = $("#SubTotal").val().replace(/[^\-\d\.]/g, '');
    model.Cheque = $("#Check").val() == "" ? 0 : $("#Check").val() == "$" ? 0 : $("#Check").val().replace(/[^\-\d\.]/g, '');
    model.CreditCard = $("#CreditCard").val() == "" ? 0 : $("#CreditCard").val() == "$" ? 0 : $("#CreditCard").val().replace(/[^\-\d\.]/g, '');
    model.Cash = $("#Cash").val() == "" ? 0 : $("#Cash").val() == "$" ? 0 : $("#Cash").val().replace(/[^\-\d\.]/g, '');
    model.Balance = $("#Balance").val().substring(1) == "-0.00" ? "0.00" : $("#Balance").val().substring(1);
    model.arrayOfPrice = arrayPrice;
    model.arrayOfIds = arrayId;
    model.arrayOfCompanyId = arrayOfCompanyId;
    model.arrayOfCategoryId = arrayOfCategoryId;
    model.arrayOfJewelTypeId = arrayOfJewelTypeId;
    model.arrayOfGoldWeight = arrayOfGoldWeight;
    model.arrayOfCaratWeight = arrayOfCaratWeight;
    model.arrayOfGoldContent = arrayOfGoldContent;
    model.arrayOfPieces = arrayOfPieces;
    model.arrayOfDescription = arrayOfDescription;
    model.arrayOfSupplier = arrayOfSupplier;
    model.arrayOfStatus = arrayOfStatus;
    model.arrayOfDiamondPieces = arrayOfDiamondPieces;
    model.arrayOfInventoryStatus = arrayOfInventoryStatus;
    model.arrayOfDateReceived = arrayOfDateReceived;    
    model.CompanyId = $("#CompanyId").val();
    $('#CustomerModal .modal-dialog').append(loaderDiv());
    if ($('#redirect').val() == "" || $('#redirect').val() == undefined) {
        model.InvoiceId = 0;
        $.ajax({
            url: '/Invoice/RedirectToInvoicePrint',
            type: "POST",
            data: '{model: ' + JSON.stringify(model) + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.data == 'error') {
                    toastr.error("Something went wrong");
                    $('#CustomerModal .modal-dialog')[0].removeChild($('#CustomerModal .modal-dialog')[0].lastChild);
                }
                else {
                    window.location = result.url;
                }
            },
            error: function (result) {
                toastr.error("Something went wrong");
                $('#CustomerModal .modal-dialog')[0].removeChild($('#CustomerModal .modal-dialog')[0].lastChild);
            }
        });
    }
    else {
        model.InvoiceId = $('#redirect').val();
        $.ajax({
            url: '/Invoice/UpdateInvoice',
            type: "POST",
            data: '{model: ' + JSON.stringify(model) + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.data == 'error') {
                    toastr.error("Something went wrong");
                    $('#CustomerModal .modal-dialog')[0].removeChild($('#CustomerModal .modal-dialog')[0].lastChild);
                }
                else {
                    window.location = result.url;
                }
            },
            error: function (result) {
                toastr.error("Something went wrong");
                $('#CustomerModal .modal-dialog')[0].removeChild($('#CustomerModal .modal-dialog')[0].lastChild);
            }
        });
    }

}
function getCustomerModel() {
    var customerModel = {};
    customerModel.CustomerName = $("#CustomerName").val();
    customerModel.CustomerPhone = $("#CustomerPhone").val();
    customerModel.CustomerAddress = $("#Address").val();
    customerModel.CustomerPhone = $("#CustomerPhone").val();
    customerModel.CustomerDOB = $("#DOB").val();
    customerModel.ExpDate = $("#ExpDate").val();
    customerModel.DLNumber = $("#DLNumber").val();
    return customerModel;
}
function getModel() {
    var model = {}
    model.InventoryStatusId = $("#Status").val();
    model.EmployeeId = $("#EmployeeId").val();
    model.Tax = $("#Tax").val();
    model.SubTotal = $("#SubTotal").val();
    model.Check = $("#Check").val();
    model.CreditCard = $("#CreditCard").val();
    model.Cash = $("#Cash").val();
    model.Balance = $("#Balance").val();
    return model;
}
function loaderDiv() {
    return '<div class="overlay">' +
        '<i class="fas fa-2x fa-sync-alt fa-spin"></i>' +
        '</div>';
}
function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode == 46) {
        return true;
    }
    else if (charCode == 45) {
        return true;
    }
    else if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    return true;
}
function changePaymentValue(parm) {
    parm.value = parm.value.replace(/[^\-\d\.]/g, '') == '' ? '' : '$' + parseFloat(parm.value.replace(/[^\-\d\.]/g, '')).toFixed(2);
}
$("#idSearch")[0].addEventListener("keyup", function (event) {
    if (event.keyCode === 13) {

        var model = {};
        model.sSearch = $("#idSearch").val();
        model.categorySearch = $("#CategorySearch").val();
        model.suplierSearch = $("#SupplierSearch").val();
        model.statusSearch = $("#StatusSearch").val();
        model.sEcho = "1";
        model.iDisplayLength = 12;
        model.iDisplayStart = 0;
        model.iColumns = 5;
        model.iSortCol_0 = 0;
        model.sSortDir_0 = "dec";
        model.iSortingCols = 1;
        model.sColumns = ",,,,";
        ajaxCall(model);
    }
});
$("#Barcode")[0].addEventListener("keydown", function (event) {
    if (event.keyCode === 13) {
        if ($('#Barcode').val().length > 0) {
            var body = $("#tableDestroy").find('tbody');
            var rows = body.find('tr');
            var validId = true;
            $.each(rows, function (index, number) {
                if (number.children[0].textContent == $('#Barcode').val()) {
                    validId = false;
                }
            });
            if (validId) {
                fetchInventoryData(JSON.stringify($('#Barcode').val()));
            }
            else {
                toastr.error("Item already exists in list");
            }
        }
        else {
            toastr.error('Enter the barcode value');
        }
    }
})
function fetchInventoryData(inventoryId) {
    $.ajax({
        url: '/InventoryMaster/FindInventoryImage',
        type: "POST",
        data: '{Id: ' + inventoryId + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result.data == "" && result.values.InventoryStatusId == 6) {
                toastr.error("Item found without image. Please update the image.");
            }
            if (result.values.Id == null) {
                toastr.error("No Item found");
            }
            else if (result.values.InventoryStatusId != 6) {

                toastr.error(" Item found, with status " + result.values.InventoryStatus + ". Please change status to instock.");
            }
            else if (!result.values.Status) {
                toastr.error("Item " + result.values.Id + " has been deleted.</br>Please check the inventory");
            }
            else {
                var body = $("#tableDestroy").find('tbody');
                var allRows = body.find('tr').length;
                if (body.find('tr').length >= 10) {
                    toastr.error("Max items reached, Proceed for payment")
                }
                else {
                    setValues(result.values);
                }
                clearFilter();
                $('#Barcode').val('');
                body.find('tr')[allRows].children[6].children[0].focus();
            }


        },
        error: function (result) {
            return "error";
        }
    });
}
function clearFilter() {
   
    $('#idSearch').val("");
    $('#CategorySearch').val("");
    $('#SupplierSearch').val("");
    $('#StatusSearch').val("");
    commanModel();
}
function commanModel() {
    var model = {}
    model.sSearch = $("#idSearch").val();
    model.categorySearch = $("#CategorySearch").val();
    model.suplierSearch = $("#SupplierSearch").val();
    model.statusSearch = $("#StatusSearch").val();
    model.sEcho = "1";
    model.iDisplayLength = 12;
    model.iDisplayStart = 0;
    model.iColumns = 5;
    model.iSortCol_0 = 0;
    model.sSortDir_0 = "dec";
    model.iSortingCols = 1;
    model.sColumns = ",,,,";
    ajaxCall(model);
}
$('#CategorySearch').change(
    function () {
        commanModel();
    }
);
$('#SupplierSearch').change(
    function () {
        commanModel();
    }
);
$('#StatusSearch').change(
    function () {
        commanModel();
    }
);
function displayImage(img) {

    $('#InventoryModal').modal('hide');
    $('#ImageModal').modal('show');
    $('.myImage').attr('src', '/Content/Images/bangle.jpg');
}
function uploadToModel() {
    if (validPrice()) {
        $()
        $('#CustomerModal').modal('show')
        $('#invoicePrintForm')[0].reset();
        $("#SubTotal").val($("#totalValueText").text());
        $("#Balance").val($("#totalValueText").text());
        $("#CompanyId").val('');
        $("#EmployeeId").val('');
      
        $('#Status').val('5');
        $('#Tax').val('INC');
        $('#validateCustomerName').addClass('d-none');
        $('#CustomerName').css('border-color', '');
        $('#validateCompany').addClass('d-none');
        $('#CompanyId').css('border-color', '');
        $('#validateEmployee').addClass('d-none');
        $('#EmployeeId').css('border-color', '');
    }
    else {
        toastr.error('Enter valid price');
        var body = $("#tableDestroy").find('tbody');
        var rows = body.find('tr');
        $.each(rows, function (index, number) {
            if (number.children[7].firstElementChild.value == '') {
                number.children[7].firstElementChild.focus();
                return false;
            }
        });
    }
}
function uploadEditModel() {
    if ($("#tableDestroy").find('tbody').find('tr').length == 0) {
        toastr.error('Enter item and then proceed for payment');
    }
    else {
        if (validPrice()) {
            $('#CustomerModal').modal('show');
            $("#SubTotal").val($("#totalValueText").text());
          
            var check = $("#Check").val() == "" ? 0 : $("#Check").val() == "$" ? 0 : $("#Check").val().replace(/[^\-\d\.]/g, '');
            var creditCard = $("#CreditCard").val() == "" ? 0 : $("#CreditCard").val() == "$" ? 0 : $("#CreditCard").val().replace(/[^\-\d\.]/g, '');
            var cash = $("#Cash").val() == "" ? 0 : $("#Cash").val() == "$" ? 0 : $("#Cash").val().replace(/[^\-\d\.]/g, '');
            var subtotal = $("#SubTotal").val().replace(/[^\-\d\.]/g, '');
            $("#Balance").val('$' + (parseFloat(subtotal) - parseFloat(check) - parseFloat(creditCard) - parseFloat(cash)).toFixed(2));
        }
        else {
            toastr.error('Enter valid price');
        }
    }
}
function validPrice() {
    var body = $("#tableDestroy").find('tbody');
    var rows = body.find('tr');
    var render = true;
    $.each(rows, function (index, number) {
        if (number.children[7].firstElementChild.value == '') {
            render = false;
        }
    });
    return render;
}
function deleteRow(r) {
    var i = r.parentNode.parentNode.rowIndex;
    document.getElementById("tableDestroy").deleteRow(i);
    var body = $("#tableDestroy").find('tbody');
    var rows = body.find('tr');
    if (rows.length == 0) {
        $("#btnPayment").attr('disabled', 'disabled')
      
    }
    clearFilter();
    var total = 0.00;
    $.each(rows, function (index, number) {
        total = (parseFloat(total) + parseFloat(number.children[7].firstElementChild.value == '' ? 0 : number.children[7].firstElementChild.value.replace(/[^\-\d\.]/g, ''))).toFixed(2);
    });
    $("#totalValueText").text("$" + parseFloat(total).toFixed(2));
}
$("#Category").change(function () {
    var value = $("#Category").val();
    $('#Category option').each(function () {
        var b = $(this).text();
        if ($(this).val() == value) {
            $('#Description').val(b);
        }
    });
    $('#Description').removeClass('is-invalid');
})
function adHocSubmitForm() {
    $('#adHocForm').submit(function (e) {
        e.preventDefault();
        if (!($('#GoldWeight').hasClass('is-invalid') || $('#Caratweight').hasClass('is-invalid') || $('#GoldContent').hasClass('is-invalid') ||
            $('#pieces').hasClass('is-invalid') || $('#Description').hasClass('is-invalid') || $('#Price').hasClass('is-invalid') ||
            $('#DiamondPieces').hasClass('is-invalid'))) {
            var goldWeight = $('#GoldWeight').val();
            var caratWeight = $('#CaratWeight').val();
            var goldContent = $('#GoldContent').val();
            var noOfPieces = $('#pieces').val();
            var description = $('#Description').val();
            var price = $('#Price').val().replace(/[^\-\d\.]/g, '');
            e.preventDefault();
            $("#tableDestroy").append(
                "<tr>"
                + "<td>" + '' + "</td>"
                + "<td>" + goldWeight + "</td>"
                + "<td>" + caratWeight + "</td>"
                + "<td>" + goldContent + "</td>"
                + "<td>" + noOfPieces + "</td>"
                + "<td>" + description + "</td>"
                + "<td><input type='text' id='perGram'  onkeypress='return isNumberKey(event)' onchange='changePerGram(this)' placeholder='Current Price' name='pricePerGram' style='width:100%'/></td>"
                + "<td><input type='text' id='priceClick'  onkeypress='return isNumberKey(event)' onchange='changeTotal(this)'  name='price' value=" + "$" + parseFloat(price).toFixed(2) + " style='width:100%'/></td>"
                + "<td><a onclick='deleteRow(this)' href='#'>Delete</a></td>"
                + "<td class='d-none'>" + $('#Company').val() + "</td>"
                + "<td class='d-none'>" + $('#Category').val() + "</td>"
                + "<td class='d-none'>" + $('#JewelType').val() + "</td>"
                + "<td class='d-none'>" + $('#Supplier').val() + "</td>"
                + "<td class='d-none'>" + $('#ValidStatus').val() + "</td>"
                + "<td class='d-none'>" + $('#DiamondPieces').val() + "</td>"
                + "<td class='d-none'>" + $('#InventoryStatus').val() + "</td>"
                + "<td class='d-none'>" + $('#DateReceived').val() + "</td>"
                + "<td class='d-none'><img id='imgCapture' src='" + $('#imgCapture').attr('src') + "'/></td>"
                + "</tr>"
            )
            $("#adHocModalPopup").modal('hide');
            $('#adHocForm')[0].reset();
            $("#btnPayment").removeAttr('disabled');
       
            var body = $("#tableDestroy").find('tbody');
            var rows = body.find('tr');
            var total = 0.00;
            $.each(rows, function (index, number) {
                total = (parseFloat(total) + parseFloat(number.children[7].firstElementChild.value == '' ? 0 : number.children[7].firstElementChild.value.replace(/[^\-\d\.]/g, ''))).toFixed(2);
            });
            $("#totalValueText").text("$" + total);
        }
    });
}
function validateAdHocForm() {
    $('#adHocForm').validate({
        rules: {
            GoldWeight: {
                required: true
            },
            CaratWeight: {
                required: true
            },
            GoldContent: {
                required: true
            },
            pieces: {
                required: true
            },
            Description: {
                required: true
            },
            Price: {
                required: true
            },
            DiamondPieces: {
                required: true
            }
        },
        messages: {
            GoldWeight: {
                required: "Please enter gold weight"
            },
            CaratWeight: {
                required: "Please enter carat weight"
            },
            GoldContent: {
                required: "Please enter gold content"
            },
            pieces: {
                required: "Please enter no. of pieces"
            },
            Description: {
                required: "Please enter description"
            },
            Price: {
                required: "Please enter price"
            },
            DiamondPieces: {
                required: "Please enter diamond pieces"
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
function clearadHocValidation() {
    $('#GoldWeight').removeClass('is-invalid');
    $('#CaratWeight').removeClass('is-invalid');
    $('#GoldContent').removeClass('is-invalid');
    $('#NoOfPieces').removeClass('is-invalid');
    $('#Description').removeClass('is-invalid');
    $('#Price').removeClass('is-invalid');
}
function adHocAddFunction() {
    $('#Company').val('1');
    $('#JewelType').val('2');
    $('#Category').val('1050');
    $('#Description').val($('#Category option:selected').text());
    $('#GoldWeight').val('');
    $('#NoOfPieces').val('');
    $('#InventoryStatus').val('5');
    $('#DiamondPieces').val('');
    $('#GoldContent').val('');
    $('#CaratWeight').val('');
    $('#Price').val('');   
    clearadHocValidation();

    var date = new Date();
    year = date.getFullYear();
    month = (date.getMonth() + 1).toString().padStart(2, "0");
    var monthAndYear = month + '/' + year;

    document.getElementById("DateReceived").value = monthAndYear;
    var a = $("#DateRecevied").val();
    var b = $("#DateRecevied").val();   
}

$('#Company').on('change', function () {
    if ($('#Company').val() == "2") {
        $('#JewelType').val("1");
    }
    else {
        $('#JewelType').val("2");
    }
});

function selectInventoryFunction(parm, idValue) {
    var body = $("#tableDestroy").find('tbody');
    if (body.find('tr').length >= 10) {
        toastr.error("Max items reached, Proceed for payment")
    }
    else {
        if (!parm.classList.contains("disabled")) {
            findInventoryAjax(idValue);
            parm.classList.add("disabled");
            let p = document.createElement("p");
            p.append("Item already exists in the list")
            p.classList.add("text-sm");
            p.classList.add("text-danger");
            parm.closest(".card-footer").querySelector("p").parentElement.append(p);

        }

    }


}
function findInventoryAjax(idValue) {
    $.ajax({
        url: '/InventoryMaster/FindInventoryImage',
        type: "POST",
        data: '{Id: ' + JSON.stringify(idValue) + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            setValues(result.values);
        },
        error: function (result) {
            return "error";
        }
    });
}
function setValues(values) {
    var body = $("#tableDestroy").find('tbody');
    var allRows = body.find('tr').length;

    if (body.find('tr').length > 10) {
        toastr.error("Max items reached, Proceed for payment")
    }
    else {
        var price = parseFloat(values.Price.replace(/[^\-\d\.]/g, '')).toFixed(2);
      
        var totalPrice = parseFloat(price) == 0 ? '' : ('$' + parseFloat(price).toFixed(0) + '.00');
        $("tbody").append(
            "<tr>"
            + "<td>" + values.Id + "</td>"
            + "<td>" + values.GoldWeight + "</td>"
            + "<td>" + values.CaratWeight + "</td>"
            + "<td> " + values.GoldContent + " </td>"
            + "<td>" + values.Pieces + "</td>"
            + "<td>" + values.Description + "</td>"
            + "<td><input type='text' id='perGram'  onkeypress='return isNumberKey(event)' onchange='changePerGram(this)' placeholder='Current Price'  name='pricePerGram' style='width:100%'/></td>"
            + "<td><input type='text' autocomplete='off' onkeypress='return isNumberKey(event)' onchange='changeTotal(this)' name='price' value='" + totalPrice + "' style='width:100%'/></td>"
            + "<td><a onclick='deleteRow(this)' href='#'>Delete</a></td>"
            + "</tr>"
        )
        changeTotal(document.getElementById('perGram'));
    }
    $("#btnPayment").removeAttr('disabled');


}
$('#InventoryModal').focusout(function () {
    var rows = $("#tableDestroy").find('tbody').find('tr');
    $.each(rows, function (index, number) {
  
        if (number.children[6].firstElementChild.value == '') {
            number.children[6].children[0].focus();
            return false;
        }
    });
});
$('#idSearch').click(function () {
    $('#idSearch').focus();
    $('#idSearch').focus();
    $('#idSearch').focus();
});
function changePrice(parm) {
    parm.value = parm.value.replace(/[^\-\d\.]/g, '') == '' ? '' : '$' + parseFloat(parm.value.replace(/[^\-\d\.]/g, '') == '' ? 0 : parm.value.replace(/[^\-\d\.]/g, '')).toFixed(2);
}
$('#perGram').on('keypress', function (e) {
    if (e.which == 13) {
        changePerGram($('#perGram')[0]);
    }
});
function changePerGram(parm) {
    
    var number = parm.closest('tr');
    if (parm.value.replace(/[^\-\d\.]/g, '') != '') {
        parm.closest('tr').children[7].firstElementChild.value = "$" + (parseFloat(number.children[1].innerText == '' ? 0 :
            parseFloat(parm.value.replace(/[^\-\d\.]/g, '')) * parseFloat(number.children[1].innerText))).toFixed(0) + '.00';
    }
    var total = 0.00;


    changeTotal(parm);
}
function changeTotal(parm) {
    parm.value = parm.value.replace(/[^\-\d\.]/g, '') == '' ? '' :
        '$' + parseFloat(parm.value.replace(/[^\-\d\.]/g, '') == '' ? 0 :
            parm.value.replace(/[^\-\d\.]/g, '')).toFixed(2);
    var body = $("#tableDestroy").find('tbody');
    var rows = body.find('tr');
    var total = 0.00;
    $.each(rows, function (index, number) {
        total = (parseFloat(total) + parseFloat(number.children[7].firstElementChild.value == '' ? 0 : number.children[7].firstElementChild.value.replace(/[^\-\d\.]/g, ''))).toFixed(2);
    });
    $("#totalValueText").text("$" + total);
    $("#Barcode").focus();
}
function changeBalance(parm) {
    parm.value = parm.value == '' ? '' : ('$' + parm.value.replace(/[^\-\d\.]/g, ''));
    var check = $("#Check").val() == "" ? 0 : $("#Check").val() == "$" ? 0 : $("#Check").val().replace(/[^\-\d\.]/g, '');
    var creditCard = $("#CreditCard").val() == "" ? 0 : $("#CreditCard").val() == "$" ? 0 : $("#CreditCard").val().replace(/[^\-\d\.]/g, '');
    var cash = $("#Cash").val() == "" ? 0 : $("#Cash").val() == "$" ? 0 : $("#Cash").val().replace(/[^\-\d\.]/g, '');
    var subtotal = $("#SubTotal").val().replace(/[^\-\d\.]/g, '');
    $("#Balance").val('$' + (parseFloat(subtotal) - parseFloat(check) - parseFloat(creditCard) - parseFloat(cash)).toFixed(2));
}
$('#clearTbody').on('click', function () {
    $("tbody").html('');
    $("#totalValueText").text("$0.00");
    $("#btnPayment").attr('disabled', 'disabled');
    clearFilter();
    $("#editInvoice").html('');
});
document.getElementById('CustomerPhone').addEventListener('input', function (e) {
    var x = e.target.value.replace(/\D/g, '').match(/(\d{0,3})(\d{0,3})(\d{0,4})/);
    e.target.value = !x[2] ? x[1] : '(' + x[1] + ') ' + x[2] + (x[3] ? '-' + x[3] : '');
});

function validatePaymentForm() {
    $('#invoicePrintForm').validate({
        rules: {
            CustomerName: {
                required: true
            },
            Company: {
                required: true
            },
            EmployeeId: {
                required: true
            },
        },
        messages: {
            CustomerName: {
                required: "Please enter Customer Name"
            },
            Company: {
                required: "Please Select Comapany "
            },
            EmployeeId: {
                required: "Please Select Employee "
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
$('#datePicker').datetimepicker({
    format: 'MM/yyyy'
});

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





