using System.Web.Optimization;

namespace JMS
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                    "~/Scripts/bootstrap.bundle.min.js"));

            bundles.Add(new ScriptBundle("~/adminlte/js").Include(
              "~/admin-lte/plugins/datatables/jquery.dataTables.min.js",
              "~/admin-lte/plugins/datatables-bs4/js/dataTables.bootstrap4.min.js",
              "~/admin-lte/plugins/datatables-responsive/js/dataTables.responsive.min.js",
              "~/admin-lte/plugins/datatables-responsive/js/responsive.bootstrap4.min.js",
              "~/admin-lte/plugins/datatables-buttons/js/dataTables.buttons.min.js",
              "~/admin-lte/plugins/datatables-buttons/js/buttons.bootstrap4.min.js",
              "~/admin-lte/plugins/jszip/jszip.min.js",
              "~/admin-lte/plugins/pdfmake/pdfmake.min.js",
              "~/admin-lte/plugins/pdfmake/vfs_fonts.js",
              "~/admin-lte/plugins/datatables-buttons/js/buttons.html5.min.js",
              "~/admin-lte/plugins/datatables-buttons/js/buttons.print.min.js",
              "~/admin-lte/plugins/datatables-buttons/js/buttons.colVis.min.js",
              "~/admin-lte/plugins/bs-custom-file-input/bs-custom-file-input.min.js",
              "~/admin-lte/plugins/chart.js/Chart.min.js",
              "~/admin-lte/plugins/moment/moment.min.js",
              "~/admin-lte/plugins/daterangepicker/daterangepicker.js",
              "~/admin-lte/plugins/tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4.min.js",
              "~/admin-lte/plugins/toastr/toastr.min.js",
              "~/admin-lte/dist/js/adminlte.min.js"));

            bundles.Add(new Bundle("~/Content/login").Include(
               "~/Content/js/login.js"));

            bundles.Add(new Bundle("~/Content/category").Include(
               "~/Content/js/category.js"));

            bundles.Add(new Bundle("~/Content/inventory").Include(
                "~/Content/js/jquery.webcam.js",
                "~/Content/js/jspdf.js",
                "~/Content/js/jQuery.print.js",
                "~/Content/js/html2canvas.js",
                "~/Content/js/canvas2image.js",
               "~/Content/js/inventory.js"));

            bundles.Add(new Bundle("~/Content/role").Include(
                "~/Content/js/role.js"));

            bundles.Add(new Bundle("~/Content/jewelType").Include(
                "~/Content/js/jewelType.js"));

            bundles.Add(new Bundle("~/Content/inventoryStatus").Include(
                "~/Content/js/inventoryStatus.js"));

            bundles.Add(new Bundle("~/Content/supplier").Include(
                "~/Content/js/supplier.js"));

            bundles.Add(new Bundle("~/Content/customer").Include(
               "~/Content/js/customer.js"));

            bundles.Add(new Bundle("~/Content/user").Include(
              "~/Content/js/user.js"));

            bundles.Add(new Bundle("~/Content/company").Include(
                "~/Content/js/company.js"));

            bundles.Add(new Bundle("~/Content/invoiceList").Include(
              "~/Content/js/invoiceList.js"));


            bundles.Add(new Bundle("~/Content/invoice").Include(
                "~/Content/js/jquery.webcam.js",
              "~/Content/js/invoice.js"));

            bundles.Add(new Bundle("~/Content/home").Include(
             "~/Content/js/home.js"));

            bundles.Add(new Bundle("~/Content/customerReport").Include(
             "~/Content/js/customerReport.js"));

            bundles.Add(new Bundle("~/Content/auditreport").Include(
             "~/Content/js/auditreport.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/admin-lte/skin-blue.min.css",
                      "~/Content/bootstrap.css",
                      "~/admin-lte/plugins/fontawesome-free/css/all.min.css",
                      "~/admin-lte/plugins/datatables-bs4/css/dataTables.bootstrap4.min.css",
                      "~/admin-lte/plugins/datatables-responsive/css/responsive.bootstrap4.min.css",
                      "~/admin-lte/plugins/datatables-buttons/css/buttons.bootstrap4.min.css",
                      "~/admin-lte/plugins/daterangepicker/daterangepicker.css",
                      "~/admin-lte/plugins/tempusdominus-bootstrap-4/css/tempusdominus-bootstrap-4.min.css",
                      "~/admin-lte/plugins/toastr/toastr.min.css",
                      "~/admin-lte/dist/css/adminlte.min.css",
                      "~/Content/site.css"));
        }
    }
}
