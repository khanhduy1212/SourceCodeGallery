using System.Web.Optimization;
using NS;
using NS.Mvc.Helpers;

namespace XProject.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new StyleBundle("~/content/css/min").Include(
                "~/Content/css/bootstrap.min.css"
                , "~/Content/css/font-awesome.min.css"
                , "~/Content/css/magnific-popup.css"
                , "~/Content/lib/toastr-2.1.3/toastr.min.css"
                , "~/Content/css/bootstrap-colorpicker.min.css"
                , "~/Content/css/shortcodes/shortcodes.css"
                , "~/Content/css/owl.carousel.css"
                , "~/Content/css/owl.theme.css"
                , "~/Content/css/style.css"
                , "~/Content/css/style-responsive.css"
                , "~/Content/css/default-theme.css"
                ));
            //bundles.Add(new CssBundle("~/content/css/print").Include(
            //        "~/Display/css/print.css"
            //    ));

            bundles.Add(new JsBundle("~/content/js/min").Include(
                  "~/Content/js/jquery-2.1.4.js"
                , "~/Content/js/bootstrap.min.js"
                , "~/Content/js/menuzord.js"
                , "~/Content/js/jquery.flexslider-min.js"
                , "~/Content/js/owl.carousel.min.js"
                , "~/Content/js/jquery.isotope.js"
                
                , "~/Content/js/jquery.magnific-popup.min.js"
                , "~/Content/lib/toastr-2.1.3/toastr.min.js"
                , "~/Content/js/jquery.countTo.js"
                , "~/Content/js/visible.js"
                , "~/Content/js/smooth.js"
                , "~/Content/js/wow.min.js"
                , "~/Content/js/imagesloaded.js"
                , "~/Content/js/jquery.nav.js"
                , "~/Content/js/bootstrap-colorpicker.min.js"
                , "~/Content/js/jquery.maskedinput.min.js"
                //, "~/Content/js/bootstrap-datepicker.min.js"

                , "~/Areas/Admin/Display/js/bootbox.js"
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

                , "~/Content/js/scripts.js"
                ));


            bundles.Add(new CssBundle("~/css/ie8").Include("~/Content/css/ie.css"));

            bundles.Add(new JsBundle("~/js/ie9").Include(
                "~/Content/js/ie/html5.js"
                , "~/Content/js/ie/respond.min.js"
                , "~/Content/lib/flot/excanvas.min.js"));

            bundles.Add(new JsBundle("~/js/eislideshow").Include(
                "~/Content/js/jquery.eislideshow.js"
                , "~/Content/js/jquery.easing.1.3.js"));

            bundles.Add(new StyleBundle("~/Template/css/cssWeb").Include(
"~/Template/css/normalize.css",
"~/Template/css/bootstrap.css",
"~/Template/css/style.css",
"~/Template/css/fontawesome-all.css",
"~/Template/css/font-awesome.css",
"~/Template/css/reponsive.css",
"~/Template/css/slick.css",
"~/Template/css/slider.css",
"~/Template/css/slick-theme.css",
"~/Template/css/select2-bootstrap.css",
//"~/Template/css/bootstrap-datepicker.css",
//"~/Template/css/bootstrap-datetimepicker.min.css",
"~/Template/css/jasny-bootstrap.css",
"~/Template/css/CusWeb.css",
"~/Content/bootstrap/tagsinput/bootstrap-tagsinput.css"
));

            bundles.Add(new JsBundle("~/Template/js/jsWeb").Include(
"~/Template/js/jquery.js",
"~/Template/js/bootstrap.js",
"~/Template/js/slick.js",
"~/Template/js/jquery.sticky.js",
"~/Template/js/select2.full.js",
//"~/Template/js/bootstrap-datepicker.js",
//"~/Template/js/bootstrap-datetimepicker.min.js",
"~/Areas/Admin/Display/js/jquery.dataTables.min.js"
         , "~/Areas/Admin/Display/js/jquery.dataTables.bootstrap.min.js"
         , "~/Areas/Admin/Display/js/dataTables.buttons.min.js",
"~/Template/js/jasny-bootstrap.js",
//"~/Template/js/CustomWeb.js",
//"~/Template/js/FunctionWeb.js",
"~/Content/bootstrap/tagsinput/bootstrap-tagsinput.js",
 "~/Areas/Admin/Display/js/bootstrap3-typeahead.min.js"
));

bundles.Add(new StyleBundle("~/Template/css/cssUploadImg").Include(
"~/Template/UploadImg/main.css",
"~/Template/UploadImg/dropzone.css",
"~/Template/UploadImg/cropper.min.css",
"~/Template/UploadImg/cussupload.css"
));
            bundles.Add(new JsBundle("~/Template/js/jsUploadImg").Include(
"~/Template/UploadImg/cusjsVG.js",
"~/Template/UploadImg/dropzoneVG.js",
"~/Template/UploadImg/cropperVG.js"

));

                         bundles.Add(new StyleBundle("~/Template/css/cssUploadImgProduct").Include(
"~/Template/UploadBDS/main.css",
"~/Template/UploadBDS/dropzone.css",
"~/Template/UploadBDS/cropper.min.css",
"~/Template/UploadBDS/cussupload.css"
));
            bundles.Add(new JsBundle("~/Template/js/jsUploadImgProduct").Include(
"~/Template/UploadBDS/cusjsNew.js",
"~/Template/UploadBDS/dropzoneNew.js",
"~/Template/UploadBDS/cropperNew.js"

));



//            bundles.Add(new StyleBundle("~/Content/css/cropper").Include(
//            "~/Content/cropper/cropper.css"
 
//            ));
//            bundles.Add(new JsBundle("~/Content/js/cropper").Include(
//"~/Content/cropper/cropper.js"


//));



            bundles.Add(new StyleBundle("~/Content/css/tagsinput").Include(
                "~/Content/bootstrap/tagsinput/bootstrap-tagsinput.css"

            ));
            bundles.Add(new JsBundle("~/Content/js/tagsinput").Include(
                "~/Content/bootstrap/tagsinput/bootstrap-tagsinput.js"


            ));


            bundles.Add(new JsBundle("~/Template/js/jsCustomWeb").Include(
"~/Template/js/CustomWeb.js",
"~/Template/js/FunctionWeb.js"
));
            //BundleTable.EnableOptimizations = true;
        }





    }
}
