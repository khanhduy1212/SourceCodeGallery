﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using XProject.Web.Infrastructure.Helpers;

namespace XProject.Web.Infrastructure.Utility
{
    public class PictureUpload
    {
        public static string Upload(int id, string foldername, HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    if (!Directory.Exists(GetfolderPath(foldername)))
                    {
                        Directory.CreateDirectory(GetfolderPath(foldername));
                    }
                    string fileName = id + Path.GetExtension(file.FileName);
                    string thumbName = id + "_400x400" + Path.GetExtension(file.FileName);
                    string filePath = HttpContext.Current.Server.MapPath(GetImagePath(foldername, fileName));

                    string directory = Path.GetDirectoryName(filePath);
                    if (directory != null) Directory.CreateDirectory(directory);
                    file.SaveAs(filePath);
                    ImageHelper.CreateImageHighQuality(GetfolderPath(foldername), GetfolderPath(foldername), fileName, thumbName, ImageHelper.MaxHeightUserAvatar, ImageHelper.MaxWidthUserAvatar);

                    return fileName;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return null;
        }

        public static bool Delete(int id, string foldername, string fileName)
        {
            try
            {
                string path = HttpContext.Current.Server.MapPath(GetImagePath(foldername, fileName));
                File.Delete(path);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string GetfolderPath(string foldername)
        {
            return HttpContext.Current.Server.MapPath(foldername);
        }
        public static string GetImagePath(string foldername, string fileName)
        {
            return foldername + fileName;
        }
    }
}