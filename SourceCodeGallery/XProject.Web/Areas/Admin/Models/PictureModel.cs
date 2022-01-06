using System;
using System.Collections.Generic;
using System.ComponentModel;
using XProject.Domain.Entities;


namespace XProject.Web.Models
{
    public class PictureModel
    {

        public List<XPicture> listPicture { get; set; }
      
        public XPicture XPicture { get; set; }
      
        public int idProducts { get; set; }
        public string idProductsTmp { get; set; }
        public int TypeUpload { get; set; }
        public int PersonalId { get; set; }
        public string cfile { get; set; }
        public string nameImg { get; set; }
        public string PathImage { get; set; }
        public byte isactive { get; set; }
        public int idpicture { get; set; }
        public int index { get; set; }
        public int typeId { get; set; }
        public int sizeImage { get; set; }
        public string NameTypeUpload { get; set; }
        public string GetFilePathPhysical(string size)
        {
            // check if we have converted files
            //if (IsConverted)
            return DirectoryPhysical + FileName(size);
            //else
            //    return tblPicture.originalFilepath;
        }
        public enum PictureSize : int
        {
            Original = 853, // The summ of ascii values of the word "original"
            Large = 1,
            Medium = 2,
            Small = 3,
            Tiny = 4,
            tata = 6,
            company = 7,


            size100x100=1,
            size128x90 = 2,
            size132x93 = 3,
            size500x350 = 4,
            size500x300 = 5,
            size140x84 = 6,
            size503x303 = 7,
            size75x50 = 8,



        }
        public string DirectoryPhysical
        {
            get { return "~/fileUpload"; }
        }
        public string FileName(string wh)
        {
            // check if we have converted files
            //if (IsConverted)
            //{
            int size = 0;
            if (wh.Contains("x"))
            {
                size = int.Parse(wh.Split('x')[0] + wh.Split('x')[1]);
            }
            else
            {
                size = int.Parse(wh);
            }
             
            switch (AngelType)
            {
                case RotationAngle.Rotated0:
                    return string.Format(XPicture.ConvertedFilename, (int)size);
                    break;

                case RotationAngle.Rotated90:
                    if (!string.IsNullOrWhiteSpace(XPicture.ConvertedFilename90))
                        return string.Format(XPicture.ConvertedFilename90, (int)size);
                    break;

                case RotationAngle.Rotated180:
                    if (!string.IsNullOrWhiteSpace(XPicture.ConvertedFilename180))
                        return string.Format(XPicture.ConvertedFilename180, (int)size);
                    break;

                case RotationAngle.Rotated270:
                    if (!string.IsNullOrWhiteSpace(XPicture.ConvertedFilename270))
                        return string.Format(XPicture.ConvertedFilename270, (int)size);
                    break;
            }

            return "";

        }
        public RotationAngle AngelType
        {
            get { return (RotationAngle)AngelTypeId; }
            set { AngelTypeId = (int)value; }
        }
        public enum RotationAngle : int
        {
            Rotated0 = 0,
            Rotated90 = 1,
            Rotated180 = 2,
            Rotated270 = 3,
        }
        public int AngelTypeId { get; set; }
        public string GetConvertedFileName()
        {
            // check if we have converted files
            //if (IsConverted)
            //{
            switch (AngelType)
            {
                case RotationAngle.Rotated0:
                    return XPicture.ConvertedFilename;
                    break;

                case RotationAngle.Rotated90:
                    return XPicture.ConvertedFilename90;
                    break;

                case RotationAngle.Rotated180:
                    return XPicture.ConvertedFilename180;
                    break;

                case RotationAngle.Rotated270:
                    return XPicture.ConvertedFilename270;
                    break;
            }

            return null;
            //}
            //else
            //    return null;
        }
        public string SetFileName(string filenamePattern)
        {
            // check if we have converted files

            switch (AngelType)
            {
                case RotationAngle.Rotated0:
                    XPicture.ConvertedFilename = filenamePattern;
                    break;

                case RotationAngle.Rotated90:
                    XPicture.ConvertedFilename90 = filenamePattern;
                    break;

                case RotationAngle.Rotated180:
                    XPicture.ConvertedFilename180 = filenamePattern;
                    break;

                case RotationAngle.Rotated270:
                    XPicture.ConvertedFilename270 = filenamePattern;
                    break;
            }

            return "";
        }
        public string CreateFilename()
        {
            string encoded = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            encoded = encoded.Replace("/", "_").Replace("+", "-");
            return encoded.Substring(0, 22);
        }
        public string GetFilePath(string size)
        {
            // check if we have converted files

            return FileName(size);

        }
        public WatermarkType WaterMarkLarge { get; set; }
        public enum WatermarkType
        {
            [Description("none")]
            None = 0,
            [Description("image")]
            Image,
            [Description("text")]
            Text
        }
    }
}