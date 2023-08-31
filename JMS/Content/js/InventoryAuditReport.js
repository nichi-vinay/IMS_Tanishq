// Array to store existing barcode IDs
var existingIds = [];

// Function to remove leading zeros from a string
function removeLeadingZeros(str) {
    return str.replace(/^0+/, '');
}

// Function to fetch inventory details using AJAX
function fetchInventoryDetails() {
   
    var barcodeId = $('#Barcode').val();

    if (event.keyCode === 13) {

       
        var barcodeId = $('#Barcode').val();

     
        if (existingIds.some(existingId => removeLeadingZeros(existingId) === removeLeadingZeros(barcodeId))) {
          
            alert('Barcode ID already exists in the table.');
            $('#Barcode').val('');
            return;
        }

     
        if (barcodeId.trim() !== '') {
           
            $.ajax({
                url: '/InventoryAuditReport/FetchInventoryDetails',
                type: 'GET',
                data: { Id: barcodeId },
                success: function (data) {
                    if (data && data.Id) {
                        
                        var newRow = '<tr>' +
                            '<td>' + data.Id + '</td>' +
                            '<td>' + (data.GoldWeight || '') + '</td>' +
                            '<td>' + (data.CaratWeight || '') + '</td>' +
                            '<td>' + (data.GoldContent || '') + '</td>' +
                            '<td>' + (data.Pieces || '') + '</td>' +
                            '<td>' + (data.Description || '') + '</td>' +
                            '</tr>';

                       
                        $('#inventoryTableBody').append(newRow);

                     
                        existingIds.push(barcodeId);

                        
                        $('#Barcode').val('');

                        
                        $('#downloadButton').prop('disabled', false);
                    } else {
                    
                        var newRow = '<tr>' +
                            '<td>' + barcodeId + '</td>' +
                            '<td></td>' +
                            '<td></td>' +
                            '<td></td>' +
                            '<td></td>' +
                            '<td><a href="#" class="recreate-link">Recreate</a></td>' +
                            '</tr>';

                
                        $('#inventoryTableBody').append(newRow);

                     
                        existingIds.push(barcodeId);

                        $('#Barcode').val('');

                       
                        $('#downloadButton').prop('disabled', false);
                    }
                },
                error: function () {
              
                    alert('An error occurred while making the request');
                }
            });
        }
    }
}

// Function to check if a barcode ID exists in the table
function isBarcodeIdExists(barcodeId) {
  
    var existingIds = $('#inventoryTableBody td:first-child').map(function () {
        return $(this).text();
    }).get();

  
    return existingIds.includes(barcodeId);
}

// Function to download the table contents as an Excel file
function downloadTableAsExcel() {
    var table = document.getElementById("inventory_audit_table");
    var worksheet = XLSX.utils.table_to_sheet(table); 

    var currentDate = new Date();
    var formattedDate = currentDate.toLocaleString().replace(/[/:]/g, "-");

    var fileName = "Inventory_Audit_" + formattedDate + ".xlsx";

    var workbook = XLSX.utils.book_new(); 
    XLSX.utils.book_append_sheet(workbook, worksheet, "Inventory"); 
    XLSX.writeFile(workbook, fileName); 
}

// Event handler for clicking the "Recreate" link
$(document).on('click', '.recreate-link', function (e) {
    e.preventDefault();

  
    var confirmation = confirm("The record with this ID is not found. Would you like to open the Inventory Page in a new tab to add this record?");
    if (confirmation) {
    
        window.open('/InventoryMaster/Index', '_blank');
    }
});
