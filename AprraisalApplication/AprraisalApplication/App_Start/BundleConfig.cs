using System.Web;
using System.Web.Optimization;

namespace AprraisalApplication
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            System.Web.Optimization.BundleTable.EnableOptimizations = false;
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                      "~/Content/Template/js/dashmix.core.min.js",
                      "~/Content/Template/js/dashmix.app.min.js",
                      "~/Content/Template/js/plugins/datatables/jquery.dataTables.min.js",
                      "~/Content/Template/js/plugins/datatables/dataTables.bootstrap4.min.js",
                      "~/Content/Template/js/plugins/datatables/buttons/dataTables.buttons.min.js",
                      "~/Content/Template/js/plugins/datatables/buttons/buttons.print.min.js",
                      "~/Content/Template/js/plugins/datatables/buttons/buttons.html5.min.js",
                      "~/Content/Template/js/plugins/datatables/buttons/buttons.flash.min.js",
                      "~/Content/Template/js/plugins/datatables/buttons/buttons.colVis.min.js",
                      "~/Content/Template/js/pages/be_tables_datatables.min.js",
                      "~/Content/Template/js/plugins/bootstrap-datepicker/js/bootstrap-datepicker.min.js",
                      "~/Content/Template/js/plugins/flatpickr/flatpickr.min.js",
                      "~/Content/Template/js/pages/be_comp_dialogs.min.js",
                      "~/Content/Template/js/plugins/es6-promise/es6-promise.auto.min.js",
                      "~/Content/Template/js/plugins/sweetalert2/sweetalert2.min.js",
                      "~/Content/Template/js/plugins/ckeditor/ckeditor.js",
                      "~/Content/Template/js/plugins/chart.js/Chart.bundle.min.js",
                      "~/Content/Template/js/pages/be_comp_charts.min.js",
                      "~/Content/Template/js/plugins/select2/js/select2.full.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/Template/css/dashmix.min.css", new CssRewriteUrlTransform()).Include(
                      "~/Content/site.css",
                      "~/Content/Template/js/plugins/datatables/buttons-bs4/buttons.bootstrap4.min.css",
                      "~/Content/Template/js/plugins/datatables/dataTables.bootstrap4.css",
                      "~/Content/Template/js/plugins/bootstrap-datepicker/css/bootstrap-datepicker3.min.css",
                      "~/Content/Template/js/plugins/flatpickr/flatpickr.min.css",
                      "~/Content/Template/js/plugins/select2/css/select2.min.css",
                      "~/Content/Template/js/plugins/sweetalert2/sweetalert2.min.css")
                        );
        }
    }
}
