using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mail;
using System.Web;
using XProject.Domain.Abstract;
using XProject.Domain.Entities;
using XProject.Web.Models;
using ImageResizer;
using System.Text;
namespace XProject.Web.Infrastructure.Helpers
{
    public class ImageHelper
    {
        public const int MaxWidthImageUpload = 1024;
        public const int MaxHeightImageUpload = 1024;
        public const int MaxWidthThumbnailUpload = 400;
        public const int MaxHeightThumbnailUpload = 400;
        public const int MaxWidthUserAvatar = 400;
        public const int MaxHeightUserAvatar = 400;
        public static void CreateImageHighQuality(string sOriginalPath, string sPhysicalPath, string sOrgFileName, string sTargetFileName, int maxHeight, int maxWidth)
        {
            try
            {
                var oImg = Image.FromFile(sOriginalPath + @"\" + sOrgFileName);
                int imgWidth = GetScaleFactorWidth(oImg, maxHeight, maxWidth);
                int imgHeight = GetScaleFactorHeight(oImg, maxHeight, maxWidth);

                Image oThumbNail = new Bitmap(imgWidth, imgHeight);
                Graphics oGraphic = Graphics.FromImage(oThumbNail);
                oGraphic.DrawImage(oImg, 0, 0);
                oGraphic.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                oGraphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                oGraphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                var oRectangle = new Rectangle(0, 0, imgWidth, imgHeight);
                oGraphic.DrawImage(oImg, oRectangle);
                var targetPath = sPhysicalPath + @"\" + sTargetFileName;
                oThumbNail.Save(targetPath);
                oImg.Dispose();
            }
            catch (Exception ex)
            {
            }
        }
        public static void CreateImage60PercentQuality(string imageType, string sOriginalPath, string sPhysicalPath, string sOrgFileName, string sTargetFileName, int maxHeight, int maxWidth)
        {
            try
            {
                var oImg = Image.FromFile(sOriginalPath + @"\" + sOrgFileName);
                int imgWidth = GetScaleFactorWidth(oImg, maxHeight, maxWidth);
                int imgHeight = GetScaleFactorHeight(oImg, maxHeight, maxWidth);

                Image oThumbNail = new Bitmap(imgWidth, imgHeight);
                Graphics oGraphic = Graphics.FromImage(oThumbNail);
                oGraphic.DrawImage(oImg, 0, 0);
                oGraphic.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                oGraphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                oGraphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                var oRectangle = new Rectangle(0, 0, imgWidth, imgHeight);
                oGraphic.DrawImage(oImg, oRectangle);
                var targetPath = sPhysicalPath + @"\" + sTargetFileName;
                //SaveJpeg(targetPath, oThumbNail, 60, imageType);
                //oThumbNail.Save(sPhysicalPath + @"\" + sTargetFileName);
                oImg.Dispose();
            }
            catch (Exception ex)
            {
            }
        }
        //public static void SaveJpeg(string path, Image img, int quality, string mimeType)
        //{
        //    if (quality < 0 || quality > 100)
        //        throw new ArgumentOutOfRangeException("quality must be between 0 and 100.");

        //    // Encoder parameter for image quality 
        //    EncoderParameter qualityParam = new EncoderParameter(Encoder.Quality, quality);
        //    // JPEG image codec 
        //    ImageCodecInfo jpegCodec = GetEncoderInfo(mimeType);
        //    EncoderParameters encoderParams = new EncoderParameters(1);
        //    encoderParams.Param[0] = qualityParam;
        //    img.Save(path, jpegCodec, encoderParams);
        //}

        /// <summary> 
        /// Returns the image codec with the given mime type 
        /// </summary> 
        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats 
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec 
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];

            return null;
        }
        public static Image GetImage(string path)
        {
            try
            {
                return Image.FromFile(path);
            }
            catch (Exception ex)
            {
            }
            return null;
        }



        static int GetScaleFactorWidth(System.Drawing.Image currentImage, int maxHeight, int maxWidth)
        {
            int imgWidth = currentImage.Width;
            int imgHeight = currentImage.Height;
            double scaleFactor = 0.0;
            if ((imgWidth > maxWidth) || (imgHeight > maxHeight))
            {
                if ((maxHeight / imgHeight) > (maxWidth / imgWidth))
                {
                    scaleFactor = maxHeight / Convert.ToDouble(imgHeight);
                }
                else
                {
                    scaleFactor = maxWidth / Convert.ToDouble(imgWidth);
                }
            }
            if (imgWidth > maxWidth)
            {
                scaleFactor = maxWidth / Convert.ToDouble(imgWidth);
                imgWidth = System.Convert.ToInt32(imgWidth * scaleFactor);
                imgHeight = System.Convert.ToInt32(imgHeight * scaleFactor);
            }
            if (imgHeight > maxHeight)
            {
                imgWidth = System.Convert.ToInt32(imgWidth / scaleFactor);
                imgHeight = System.Convert.ToInt32(imgHeight / scaleFactor);
                scaleFactor = maxHeight / Convert.ToDouble(imgHeight);
                imgWidth = System.Convert.ToInt32(imgWidth * scaleFactor);
                imgHeight = System.Convert.ToInt32(imgHeight * scaleFactor);
            }
            return imgWidth;
        }

        static int GetScaleFactorHeight(System.Drawing.Image currentImage, int maxHeight, int maxWidth)
        {
            int imgWidth = currentImage.Width;
            int imgHeight = currentImage.Height;
            double scaleFactor = 0.0;
            if ((imgWidth > maxWidth) || (imgHeight > maxHeight))
            {
                if ((maxHeight / imgHeight) > (maxWidth / imgWidth))
                {
                    scaleFactor = maxHeight / Convert.ToDouble(imgHeight);
                }
                else
                {
                    scaleFactor = maxWidth / Convert.ToDouble(imgWidth);
                }
            }
            if (imgWidth > maxWidth)
            {
                scaleFactor = maxWidth / Convert.ToDouble(imgWidth);
                imgWidth = System.Convert.ToInt32(imgWidth * scaleFactor);
                imgHeight = System.Convert.ToInt32(imgHeight * scaleFactor);
            }
            if (imgHeight > maxHeight)
            {
                imgWidth = System.Convert.ToInt32(imgWidth / scaleFactor);
                imgHeight = System.Convert.ToInt32(imgHeight / scaleFactor);
                scaleFactor = maxHeight / Convert.ToDouble(imgHeight);
                imgWidth = System.Convert.ToInt32(imgWidth * scaleFactor);
                imgHeight = System.Convert.ToInt32(imgHeight * scaleFactor);
            }
            return imgHeight;
        }


        public static bool SendEmail(string from, string to, string subject, string body)
        {
            //var emailUsername = Settings.GetSetting("EmailConfig", "Username", typeof(string));
            //var emailPassword = Settings.GetSetting("EmailConfig", "Password", typeof(string));
            try
            {
                var section = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
                var message = new MailMessage("donotreply@example.com", to, subject, body) { IsBodyHtml = true };
                message.From = new MailAddress("donotreply@example.com", "", Encoding.UTF8);// Settings.GetSetting("Report", "CompanyName", typeof(string))
                message.ReplyToList.Add(from);
                message.Bcc.Add("danhbavieclam.vn@gmail.com");
                var client = new SmtpClient();
                client.Port = section.Network.Port;//Settings.GetSetting("EmailConfig", "Port", typeof(int));
                client.Host = section.Network.Host;//Settings.GetSetting("EmailConfig", "Host", typeof(string));
                // client.EnableSsl = section.Network.EnableSsl;//true;
                //setup up the host, increase the timeout to 60 minutes
                client.Timeout = (60 * 60 * 1000);
                client.DeliveryMethod = section.DeliveryMethod;//SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(section.Network.UserName, section.Network.Password);

                client.Send(message);

            }
            catch (Exception e)
            {
                //   Tools.Logger(e.ToString(),"sendmailerr","SendMail");
                return false;
            }
            return true;
        }

        public static string UploadFile(HttpPostedFile httpPostedFile)
        {

            var path = string.Empty; var path1 = string.Empty;
            var NewPath = string.Empty;
            var fortmatName = "";
            var fileNameFull = string.Empty;

            if (httpPostedFile != null && httpPostedFile.ContentLength > 0)
            {
                var fileName = Path.GetFileName(httpPostedFile.FileName);


                string newFileNmae = Path.GetFileNameWithoutExtension(fileName);
                fortmatName = Path.GetExtension(fileName);
                NewPath = newFileNmae.Replace(newFileNmae, (DateTime.Now.Day.ToString() + DateTime.Now.Month + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second));
                fileNameFull = DateTime.Now.Day + "" + DateTime.Now.Month + "_" + fileName.UrlFrendly() + NewPath + fortmatName;
                path = System.Web.HttpContext.Current.Server.MapPath("~/FileDocument/") + DateTime.Now.Day + DateTime.Now.Month + "/";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                path1 = Path.Combine(path, fileNameFull);
                httpPostedFile.SaveAs(path1);

            }

            return fileNameFull;
        }
        public static string UploadImage(HttpPostedFileBase httpPostedFile)
        {

            var path = string.Empty; var path1 = string.Empty;
            var NewPath = string.Empty;
            var fortmatName = "";
            var fileNameFull = string.Empty;
            string fullPath = "";
            if (httpPostedFile != null && httpPostedFile.ContentLength > 0)
            {
                var fileName = Path.GetFileName(httpPostedFile.FileName);

                var fo = DateTime.Now.Day + "" + DateTime.Now.Month;
                string newFileNmae = Path.GetFileNameWithoutExtension(fileName);
                fortmatName = Path.GetExtension(fileName);
                NewPath = newFileNmae.Replace(newFileNmae, (DateTime.Now.Day.ToString() + DateTime.Now.Month + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second));
                fileNameFull = fo + "_" + fileName.UrlFrendly() + NewPath + fortmatName;

                path = System.Web.HttpContext.Current.Server.MapPath("~/fileUpload/") + fo + "/";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                path1 = Path.Combine(path, fileNameFull);
                httpPostedFile.SaveAs(path1);

            }

            return fileNameFull;
        }


        public static void SaveImg(PictureModel newPicture)
        {

            var img = newPicture.cfile ?? "";
            var file = img.Replace("data:image/png;base64,", "");
            var photoBytes = Convert.FromBase64String(file);
            var urlImg = "";
            string format = "jpg";

            var picture = new PictureModel
            {
                XPicture = new XPicture { ProductsId = newPicture.idProducts, AngleType = 0, Converted = DateTime.Now, ToCheck = true, TypeId = newPicture.typeId, Title = newPicture.nameImg, Position = newPicture.isactive }
            };
            if (newPicture.cfile != null)
            {
                if (string.IsNullOrEmpty(newPicture.nameImg))
                {

                    var settings = new ResizeSettings();
                    settings.Scale = ScaleMode.UpscaleCanvas;
                    settings.Format = format;
                    settings.BackgroundColor = Color.White;
                    //string uploadFolder = picture.DirectoryPhysical;
                    string path = HttpContext.Current.Server.MapPath("~/fileUpload/") + DateTime.Now.Day + DateTime.Now.Month + "/";
                    // check for directory
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    // filename with placeholder for size
                    if (picture.GetConvertedFileName() == null || string.IsNullOrWhiteSpace(picture.GetConvertedFileName()))
                        picture.SetFileName(DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + "_" + ConfigurationManager.AppSettings["NameWebsite"] + picture.CreateFilename() + "_{0}." + format);
                    var ms = new MemoryStream(photoBytes);
                    var image = Image.FromStream(ms);
                    var name = picture.FileName("853");
                    image.Save(path + name);
                    if (picture.GetFilePathPhysical("100x100") != null)
                    {
                        string dest = path + picture.FileName("100x100");
                        settings.Width = 100;
                        settings.Height = 100;
                        if (picture.WaterMarkLarge == PictureModel.WatermarkType.None)
                            ImageBuilder.Current.Build(photoBytes, dest, settings, disposeSource: false, addFileExtension: false);
                        // save biggest version as original
                        if (string.IsNullOrWhiteSpace(picture.XPicture.OriginalFilepath))
                            urlImg = picture.XPicture.ConvertedFilename;
                    }
                    var setting = DependencyHelper.GetService<IGeneralRepository<Setting>>();
                    var listByName = setting.GetIQueryableItems().Where(x => x.Name == newPicture.NameTypeUpload).ToList();
                    foreach (var item in listByName)
                    {

                        var listSize = item.Value.Split(';');
                        foreach (var itemsize in listSize)
                        {
                            if (itemsize != null)
                            {
                                var wh = itemsize.Split('x');

                                if (picture.GetFilePathPhysical(itemsize) != null)
                                {
                                    string dest = path + picture.FileName(itemsize);
                                    settings.Width = int.Parse(wh[0]);
                                    settings.Height = int.Parse(wh[1]);
                                    if (picture.WaterMarkLarge == PictureModel.WatermarkType.None)
                                        ImageBuilder.Current.Build(photoBytes, dest, settings, disposeSource: false, addFileExtension: false);
                                    // save biggest version as original
                                    if (string.IsNullOrWhiteSpace(picture.XPicture.OriginalFilepath))
                                        urlImg = picture.GetFilePath(itemsize);
                                }
                            }
                        }
                    }

                }
            }

        }
        public static PictureModel SaveImgToString(PictureModel newPicture)
        {

            var img = newPicture.cfile ?? "";
            var file = img.Replace("data:image/png;base64,", "");
            var photoBytes = Convert.FromBase64String(file);
            var urlImg = "";
            string format = "jpg";

            var picture = new PictureModel
            {
                XPicture = new XPicture { ProductsId = newPicture.idProducts, AngleType = 0, Converted = DateTime.Now, ToCheck = true, TypeId = newPicture.typeId, Title = newPicture.nameImg, Position = newPicture.isactive }
            };
            if (newPicture.cfile != null)
            {
                if (string.IsNullOrEmpty(newPicture.nameImg))
                {

                    var settings = new ResizeSettings();
                    settings.Scale = ScaleMode.UpscaleCanvas;
                    settings.Format = format;
                    settings.BackgroundColor = Color.White;
                    //string uploadFolder = picture.DirectoryPhysical;
                    string path = HttpContext.Current.Server.MapPath("~/fileUpload/") + DateTime.Now.Day + DateTime.Now.Month + "/";
                    // check for directory
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    // filename with placeholder for size
                    if (picture.GetConvertedFileName() == null || string.IsNullOrWhiteSpace(picture.GetConvertedFileName()))
                        picture.SetFileName(DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + "_" + ConfigurationManager.AppSettings["NameWebsite"] + picture.CreateFilename() + "_{0}." + format);
                    var ms = new MemoryStream(photoBytes);
                    var image = Image.FromStream(ms);
                    var name = picture.FileName("853");
                    image.Save(path + name);
                    if (picture.GetFilePathPhysical("50x50") != null)
                    {
                        string dest = path + picture.FileName("50x50");
                        settings.Width = 50;
                        settings.Height = 50;
                        if (picture.WaterMarkLarge == PictureModel.WatermarkType.None)
                            ImageBuilder.Current.Build(photoBytes, dest, settings, disposeSource: false, addFileExtension: false);
                        // save biggest version as original
                        if (string.IsNullOrWhiteSpace(picture.XPicture.OriginalFilepath))
                            urlImg = picture.XPicture.ConvertedFilename;
                    }
                    if (picture.GetFilePathPhysical("100x100") != null)
                    {
                        string dest = path + picture.FileName("100x100");
                        settings.Width = 100;
                        settings.Height = 100;
                        if (picture.WaterMarkLarge == PictureModel.WatermarkType.None)
                            ImageBuilder.Current.Build(photoBytes, dest, settings, disposeSource: false, addFileExtension: false);
                        // save biggest version as original
                        if (string.IsNullOrWhiteSpace(picture.XPicture.OriginalFilepath))
                            urlImg = picture.XPicture.ConvertedFilename;
                    }
                    var setting = DependencyHelper.GetService<IGeneralRepository<Setting>>();
                    var listByName = setting.GetIQueryableItems().Where(x => x.Name == newPicture.NameTypeUpload).ToList();
                    foreach (var item in listByName)
                    {

                        var listSize = item.Value.Split(';');
                        foreach (var itemsize in listSize)
                        {
                            if (itemsize != null)
                            {
                                var wh = itemsize.Split('x');

                                if (picture.GetFilePathPhysical(itemsize) != null)
                                {
                                    string dest = path + picture.FileName(itemsize);
                                    settings.Width = int.Parse(wh[0]);
                                    settings.Height = int.Parse(wh[1]);




                                    if (picture.WaterMarkLarge == PictureModel.WatermarkType.None)
                                        ImageBuilder.Current.Build(photoBytes, dest, settings, disposeSource: false, addFileExtension: false);
                                    // save biggest version as original
                                    if (string.IsNullOrWhiteSpace(picture.XPicture.OriginalFilepath))
                                        urlImg = picture.GetFilePath(itemsize);
                                }
                            }
                        }
                    }

                }
            }
            return picture;

        }
        public static Image LoadImage(string bb)
        {
            //data:image/gif;base64,
            //this image is a single pixel (black)
            byte[] bytes = Convert.FromBase64String(bb);

            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
            }

            return image;
        }
      
        public static PictureModel SaveCroppedImage(Image image, PictureModel newPicture)
        {

            ImageCodecInfo jpgInfo = ImageCodecInfo.GetImageEncoders().FirstOrDefault(codecInfo => codecInfo.MimeType == "image/jpeg");
            Image finalImage = image;
            System.Drawing.Bitmap bitmap = null;

            var img = newPicture.cfile ?? "";
            var file = img.Replace("data:image/png;base64,", "");
            var photoBytes = Convert.FromBase64String(file);
            var urlImg = "";
           

            var originalDirectory = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/fileUpload/"));
            string pathString = System.IO.Path.Combine(originalDirectory.ToString(), DateTime.Now.Day+"" + DateTime.Now.Month + "/");

            if (!Directory.Exists(pathString))
                Directory.CreateDirectory(pathString);

            string format = "jpg";
            var picture = new PictureModel
            {
                XPicture = new XPicture { ProductsId = newPicture.idProducts, AngleType = 0, Converted = DateTime.Now, ToCheck = true, TypeId = newPicture.typeId, Title = newPicture.nameImg, Position = newPicture.isactive }
            };
            if (picture.GetConvertedFileName() == null || string.IsNullOrWhiteSpace(picture.GetConvertedFileName()))
                picture.SetFileName(DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + "_" + ConfigurationManager.AppSettings["NameWebsite"] + picture.CreateFilename() + "_{0}." + format);
            var ms = new MemoryStream(photoBytes);
            var image1 = Image.FromStream(ms);
            var name = picture.FileName("853");
            image1.Save(pathString + name);

            var setting = DependencyHelper.GetService<IGeneralRepository<Setting>>();
            var listByName = setting.GetIQueryableItems().Where(x => x.Name == newPicture.NameTypeUpload).ToList();
            listByName.Insert(0, new Setting {Value = "50x50"});
            foreach (var item in listByName)
            {

                var listSize = item.Value.Split(';');
                foreach (var itemsize in listSize.ToList())
                {
                    if (itemsize != null)
                    {
                        var wh = itemsize.Split('x');

                        if (picture.GetFilePathPhysical(itemsize) != null)
                        {

                            var maxWidth = int.Parse(wh[0]);
                            var maxHeight = int.Parse(wh[1]);
                            try
                            {
                                int left = 0;
                                int top = 0;
                                int srcWidth;
                                int srcHeight;
                                bitmap = new System.Drawing.Bitmap(maxWidth, maxHeight);
                                double croppedHeightToWidth = (double)maxHeight / maxWidth;
                                double croppedWidthToHeight = (double)maxWidth / maxHeight;

                                if (image.Width > image.Height)
                                {
                                    srcWidth = (int)(Math.Round(image.Height * croppedWidthToHeight));
                                    if (srcWidth < image.Width)
                                    {
                                        srcHeight = image.Height;
                                        left = (image.Width - srcWidth) / 2;
                                    }
                                    else
                                    {
                                        srcHeight = (int)Math.Round(image.Height * ((double)image.Width / srcWidth));
                                        srcWidth = image.Width;
                                        top = (image.Height - srcHeight) / 2;
                                    }
                                }
                                else
                                {
                                    srcHeight = (int)(Math.Round(image.Width * croppedHeightToWidth));
                                    if (srcHeight < image.Height)
                                    {
                                        srcWidth = image.Width;
                                        top = (image.Height - srcHeight) / 2;
                                    }
                                    else
                                    {
                                        srcWidth = (int)Math.Round(image.Width * ((double)image.Height / srcHeight));
                                        srcHeight = image.Height;
                                        left = (image.Width - srcWidth) / 2;
                                    }
                                }
                                using (Graphics g = Graphics.FromImage(bitmap))
                                {
                                    g.SmoothingMode = SmoothingMode.HighQuality;
                                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                                    g.CompositingQuality = CompositingQuality.HighQuality;
                                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                    g.DrawImage(image, new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                                    new Rectangle(left, top, srcWidth, srcHeight), GraphicsUnit.Pixel);
                                }
                                finalImage = bitmap;
                            }
                            catch { }
                            try
                            {
                                using (EncoderParameters encParams = new EncoderParameters(1))
                                {
                                    encParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)100);
                                    //quality should be in the range 
                                    //[0..100] .. 100 for max, 0 for min (0 best compression)
                                    var tt = pathString + string.Format(picture.GetConvertedFileName(), wh[0]+ wh[1]);
                                    finalImage.Save(tt, jpgInfo, encParams);
                                  
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                            //if (bitmap != null)
                            //{
                            //    bitmap.Dispose();
                            //}
                            //return "";



                        }
                    }
                }
            }

            return picture;
        }
        public static void DeleteImg(string name, string size)
        {
            try
            {
                var pic1 = name.Split('_')[0];
                foreach (var item in size.Split(';').ToList())
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        string dir = HttpContext.Current.Server.MapPath("~/fileUpload/" + pic1 + "/" + string.Format(name, item));
                        System.IO.File.Delete(dir);
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }


        }
    }
}
