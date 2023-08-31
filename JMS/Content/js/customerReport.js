
$(function () {

    $('#customerReportTable').DataTable({
        "bServerSide": true,
        "sAjaxSource": '/CustomerReport/GetCustomerReportJsonData',
        "sAjaxDataProp": "aaData",
        "bProcessing": true,
        "dom": "<'row'<'col-sm-4'l><'col-sm-4'><'col-sm-4'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]],

        "aoColumns": [
            { "data": "CustomerId" },
            { "data": "CustomerName" },
            { "data": "NumberOfInvoices" },
            { "data": "TotalAmount" },
          
        ],
        'columnDefs': [{
            
            'orderable': false, 
        }],
        "order": [[0, 'des']],
        "language": {
            "emptyTable": "No data found...",
            "processing":
                '<i class="fa fa-spinner fa-spin fa-3x fa-fw" style="color:#2a2b2b;"></i><span class="sr-only">Loading...</span> '
        },
        "pageLength": 10,
        "responsive": true, "lengthChange": true, "autoWidth": true,
    }).buttons().container().appendTo('#customerReportTable_wrapper .col-md-6:eq(0)');
});