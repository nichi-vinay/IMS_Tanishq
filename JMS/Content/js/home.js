$(function () {
    loadInventoryAuditData();
    loadCategoryWiseSalesData();
    loadSalesByYearData();
});

function loadSalesByYearData() {
    $.ajax({
        type: "GET",
        url: "/Home/GetSalesByYearJsonData",
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            successFunc2(result.aaData);
        },
    });
}

function loadCategoryWiseSalesData() {
    $.ajax({
        type: "GET",
        url: "/Home/GetCategoryWiseSalesJsonData",
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            successFunc1(result.aaData);
        },
    });
}

function loadInventoryAuditData() {
    $.ajax({
        type: "GET",
        url: "/Home/GetInventoryAuditJsonData",
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            successFunc(result.aaData);
        },
    });
}

function successFunc2(jsondata) {

    var itemsInYearValue = [];
    var itemsInInvoiceCount = [];

    $.each(jsondata, function (i, data) {
        itemsInYearValue.push(data.YearValue);
        itemsInInvoiceCount.push(data.InvoiceCount);
    });

    var areaChartData = {
        labels: itemsInYearValue,
        datasets: [
            {
                label: 'Invoice Count',
                backgroundColor: 'green',
                borderColor: 'green',
                pointRadius: false,
                pointColor: 'green',
                pointStrokeColor: 'green',
                pointHighlightFill: '#fff',
                pointHighlightStroke: 'green',
                data: itemsInInvoiceCount
            }
        ]
    };

    var barChartCanvas = $('#SalesByYearChart').get(0).getContext('2d');
    var barChartData = $.extend(true, {}, areaChartData);
    var temp0 = areaChartData.datasets[0];
    var temp1 = areaChartData.datasets[1];
    barChartData.datasets[0] = temp0;
    

    var barChartOptions = {
        responsive: false,
        maintainAspectRatio: false,
        datasetFill: false,
        scales: {
            yAxes: [{
                display: true,
                ticks: {
                    min: 0,
                    max: this.max
                }
            }]
        }
    };

    var barChart = new Chart(barChartCanvas, {
        type: 'bar',
        data: barChartData,
        options: barChartOptions
    });
}

function successFunc1(jsondata) {
    var itemsInLables = [];
    var itemsInValues = [];

    $.each(jsondata, function (i, data) {
        itemsInLables.push(data.JewelTypeName);
        itemsInValues.push(data.CountValue);
    });

    var donutData = {
        labels: itemsInLables,
        datasets: [
            {
                data: itemsInValues,
                backgroundColor: ['#f56954', '#00a65a', '#f39c12', '#00c0ef', '#3c8dbc', '#d2d6de'],
            }
        ]
    };

    var pieChartCanvas = $('#pieChart').get(0).getContext('2d')
    var pieData = donutData;
    var pieOptions = {
        maintainAspectRatio: false,
        responsive: true,
    }
    var pieChart = new Chart(pieChartCanvas, {
        type: 'pie',
        data: pieData,
        options: pieOptions
    })


}

function successFunc(jsondata) {

    var itemsInAuditDate = [];
    var itemsInShelves = [];
    var itemsInInventory = [];
    var itemsInVarianceItems = [];

    $.each(jsondata, function (i, data) {
        itemsInAuditDate.push(data.AuditDate);
        itemsInShelves.push(data.ItemsInShelves);
        itemsInInventory.push(data.ItemsInInventory);
        itemsInVarianceItems.push(data.VarianceItems);
    });

    var areaChartData = {
        labels: itemsInAuditDate,
        datasets: [
            {
                label: 'Items in Shelves',
                backgroundColor: 'blue',
                borderColor: 'blue',
                pointRadius: false,
                pointColor: 'blue',
                pointStrokeColor: 'blue',
                pointHighlightFill: '#fff',
                pointHighlightStroke: 'blue',
                data: itemsInShelves
            },
            {
                label: 'Items in Inventory',
                backgroundColor: 'green',
                borderColor: 'green',
                pointRadius: false,
                pointColor: 'green',
                pointStrokeColor: 'green',
                pointHighlightFill: '#fff',
                pointHighlightStroke: 'green',
                data: itemsInInventory
            },
            {
                label: 'Variance Items',
                backgroundColor: 'red',
                borderColor: 'red',
                pointRadius: false,
                pointColor: 'red',
                pointStrokeColor: 'red',
                pointHighlightFill: '#fff',
                pointHighlightStroke: 'red',
                data: itemsInVarianceItems
            },
        ]
    };

    var barChartCanvas = $('#barChart').get(0).getContext('2d');
    var barChartData = $.extend(true, {}, areaChartData);
    var temp0 = areaChartData.datasets[0];
    var temp1 = areaChartData.datasets[1];
    barChartData.datasets[0] = temp1;
    barChartData.datasets[1] = temp0;

    var barChartOptions = {
        responsive: false,
        maintainAspectRatio: false,
        datasetFill: false,
        scales: {
            yAxes: [{
                display: true,
                ticks: {
                    min: 0,
                    max: this.max
                }
            }]
        }
    };

    var barChart = new Chart(barChartCanvas, {
        type: 'bar',
        data: barChartData,
        options: barChartOptions
    });
}