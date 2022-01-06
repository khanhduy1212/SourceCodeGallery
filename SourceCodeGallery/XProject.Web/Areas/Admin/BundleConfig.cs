using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using NS.Mvc.Helpers;

namespace XProject.Web.Areas.Admin
{
    internal class BundleConfig
    {
        internal static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/admin/display/css/min").Include(
                  "~/Areas/Admin/Display/css/bootstrap.min.css"
                , "~/Areas/Admin/Display/css/bootstrap-multiselect.min.css"
                , "~/Areas/Admin/Display/font-awesome/4.5.0/css/font-awesome.min.css"
                , "~/Areas/Admin/Display/css/fonts.googleapis.com.css"
                , "~/Areas/Admin/Display/css/dropzone.min.css"
                , "~/Areas/Admin/Display/lib/magnific-popup/magnific-popup.css"
                , "~/Areas/Admin/Display/lib/select2-4.0.1/css/select2.css"
                , "~/Areas/Admin/Display/lib/flag-icon-css/flag-icon.css"
                , "~/Areas/Admin/Display/css/bootstrap-colorpicker.min.css"
                , "~/Areas/Admin/Display/css/colorbox.min.css"
                , "~/Areas/Admin/Display/css/ace.min.css"
                , "~/Areas/Admin/Display/css/ace-skins.min.css"
                , "~/Areas/Admin/Display/css/ace-rtl.min.css"
                , "~/Areas/Admin/Display/css/style.css"

                  , "~/Areas/Admin/Scripts/bootstrap-fileinput/css/fileinput.min.css"
                ));
            bundles.Add(new JsBundle("~/admin/ace-extra").Include(
                  "~/Areas/Admin/Display/js/ace-extra.min.js"
                ));
            bundles.Add(new JsBundle("~/admin/display/jquery").Include(
                  "~/Areas/Admin/Display/js/jquery-2.1.4.min.js"
                ));
            bundles.Add(new JsBundle("~/admin/display/js/min").Include(
                "~/Areas/Admin/Display/js/bootstrap.min.js"
                , "~/Areas/Admin/Display/js/bootstrap-multiselect.min.js"

                , "~/Areas/Admin/Display/js/wizard.min.js"
                , "~/Areas/Admin/Display/js/jquery.dataTables.min.js"
                , "~/Areas/Admin/Display/js/jquery.dataTables.bootstrap.min.js"
                , "~/Areas/Admin/Display/js/dataTables.buttons.min.js"
                , "~/Areas/Admin/Display/js/buttons.flash.min.js"
                , "~/Areas/Admin/Display/js/buttons.html5.min.js"
                , "~/Areas/Admin/Display/js/buttons.print.min.js"
                , "~/Areas/Admin/Display/js/buttons.colVis.min.js"
                , "~/Areas/Admin/Display/js/dataTables.select.min.js"
                , "~/Areas/Admin/Display/js/bootbox.js"
                , "~/Areas/Admin/Display/js/jquery-ui.custom.min.js"
                , "~/Areas/Admin/Display/js/jquery.ui.touch-punch.min.js"
                , "~/Areas/Admin/Display/js/chosen.jquery.min.js"
                , "~/Areas/Admin/Display/js/spinbox.min.js"
                , "~/Areas/Admin/Display/js/bootstrap-datepicker.min.js"
                , "~/Areas/Admin/Display/js/moment.min.js"
                , "~/Areas/Admin/Display/js/daterangepicker.min.js"
                , "~/Areas/Admin/Display/js/bootstrap-colorpicker.min.js"
                , "~/Areas/Admin/Display/js/jquery.knob.min.js"
                , "~/Areas/Admin/Display/js/autosize.min.js"
                , "~/Areas/Admin/Display/js/jquery.inputlimiter.min.js"
                , "~/Areas/Admin/Display/js/jquery.maskedinput.min.js"
                , "~/Areas/Admin/Display/js/bootstrap-tag.min.js"
                , "~/Areas/Admin/Display/js/jquery.nestable.min.js"
                , "~/Areas/Admin/Display/lib/select2-4.0.1/js/select2.full.js"
                , "~/Areas/Admin/Display/lib/magnific-popup/jquery.magnific-popup.js"


                , "~/Areas/Admin/Display/js/Highcharts-6.0.1/highcharts.js"
                , "~/Areas/Admin/Display/js/Highcharts-6.0.1/modules/drilldown.js"
                //, "~/Areas/Admin/Display/js/highchart/highcharts-more.js"

                //, "~/Areas/Admin/Display/js/highchart/grouped-categories.js"
                //, "~/Areas/Admin/Display/js/highchart/modules/exporting.js" // datatable
                //, "~/Areas/Admin/Display/js/highchart/modules/no-data-to-display.js" // datatable

                , "~/Areas/Admin/Display/js/jQuery.tmpl.min.js"
                , "~/Areas/Admin/Display/js/jquery.validate.js"
                , "~/Areas/Admin/Display/js/jquery.validate.unobtrusive.js"
                , "~/Areas/Admin/Display/js/jquery.unobtrusive-ajax.js"
                , "~/Areas/Admin/Display/js/cldrjs-0.4.4/dist/cldr.js"
                , "~/Areas/Admin/Display/js/cldrjs-0.4.4/dist/cldr/event.js"
                , "~/Areas/Admin/Display/js/cldrjs-0.4.4/dist/cldr/supplemental.js"
                , "~/Areas/Admin/Display/js/globalize-1.1.1/dist/globalize.js"
                , "~/Areas/Admin/Display/js/globalize-1.1.1/dist/globalize/number.js"
                , "~/Areas/Admin/Display/js/globalize-1.1.1/dist/globalize/date.js"

                , "~/Areas/Admin/Display/js/bootstrap-wysiwyg.min.js"
                , "~/Areas/Admin/Display/js/jquery.colorbox.min.js"
                , "~/Areas/Admin/Display/js/ace-elements.min.js"
                , "~/Areas/Admin/Display/js/ace.min.js"
                , "~/Areas/Admin/Display/js/tree.min.js"
                //, "~/Areas/Admin/Display/js/main.js"

                , "~/Content/ckfinder/ckfinder.js"
                , "~/Content/ckeditor/ckeditor.js"
                   , "~/Areas/Admin/Display/js/bootstrap3-typeahead.min.js"
                   , "~/Areas/Admin/Display/js/autosize.js"
                     , "~/Areas/Admin/Scripts/bootstrap-fileinput/js/plugins/piexif.min.js"
                       , "~/Areas/Admin/Scripts/bootstrap-fileinput/js/plugins/sortable.min.js"
                         , "~/Areas/Admin/Scripts/bootstrap-fileinput/js/plugins/purify.min.js"
                           , "~/Areas/Admin/Scripts/bootstrap-fileinput/js/plugins/popper.min.js"
                            , "~/Areas/Admin/Scripts/bootstrap-fileinput/js/fileinput.js"
                             , "~/Areas/Admin/Scripts/bootstrap-fileinput/js/locales/LANG.js"

                ));

            bundles.Add(new StyleBundle("~/client/css/min").Include(
                  "~/Content/css/bootstrap.min.css"
                , "~/Content/css/font-awesome.min.css"
                , "~/Content/css/magnific-popup.css"
                , "~/Content/css/shortcodes/shortcodes.css"
                , "~/Content/css/owl.carousel.css"
                , "~/Content/css/owl.theme.css"
                , "~/Content/css/style.css"
                , "~/Content/css/blog.css"
                , "~/Content/css/style-responsive.css"
                , "~/Content/css/default-theme.css"
                ));
            bundles.Add(new JsBundle("~/client/js/min").Include(
                  "~/Content/js/jquery-1.10.2.min.js"
                  , "~/Content/js/bootstrap.min.js"
                  , "~/Content/js/menuzord.js"
                  , "~/Content/js/jquery.flexslider-min.js"
                  , "~/Content/js/owl.carousel.min.js"
                  , "~/Content/js/jquery.isotope.js"
                  , "~/Content/js/imagesloaded.js"
                  , "~/Content/js/jquery.magnific-popup.min.js"
                  , "~/Content/js/jquery.countTo.js"
                  , "~/Content/js/visible.js"
                  , "~/Content/js/smooth.js"
                  , "~/Content/js/wow.min.js"
                  , "~/Content/js/imagesloaded.js"
                  , "~/Content/js/jquery.nav.js"
                  , "~/Content/js/scripts.js"
                ));
        }
    }
}