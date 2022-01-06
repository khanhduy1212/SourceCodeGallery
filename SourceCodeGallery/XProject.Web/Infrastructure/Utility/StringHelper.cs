using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Helpers;

namespace XProject.Web.Infrastructure.Utility
{
    public class StringHelper
    {
        public static string SubString(string str, int index, int len)
        {
            int l = str.Length;
            int maxlen = index + len > l ? l - index : len;
            return str.Substring(index, maxlen);
        }

        public static string Ensure(object obj)
        {
            if (obj == null)
                return string.Empty;

            return System.Convert.ToString(obj);
        }

        public static decimal DecNormalize(decimal value)
        {
            return value/1.000000000000000000000000000000000m;
        }

        public static string NumNormalize(string s)
        {
            if (string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s)) return "";

            const string f = @"[^0-9.]?";
            var result = System.Text.RegularExpressions.Regex.Replace(s, f, "");
            result = result.TrimStart('0', '.').TrimEnd('.');

            var pre = 0;
            if (result.Count(j => j == '.') > 1)
            {
                var i = result.IndexOf('.');
                result = System.Text.RegularExpressions.Regex.Replace(result, @"[^0-9]?", "");
                result = result.Insert(i, ".").TrimEnd('0');
                pre = result.Length - 1 - i;
            }

            result = double.Parse(result).ToString("N" + pre);
            return result;
        }

        public static string EncodeDecode(object _data, bool encode)
        {
            var data = _data.ToString();
            if (encode)
                try
                {
                    byte[] encDataByte = Encoding.UTF8.GetBytes(data);
                    var encodedData = Convert.ToBase64String(encDataByte);
                    return encodedData;
                }
                catch
                {
                    return "";
                }
            try
            {
                var encoder = new UTF8Encoding();
                var utf8Decode = encoder.GetDecoder();

                var todecodeByte = Convert.FromBase64String(data);
                var charCount = utf8Decode.GetCharCount(todecodeByte, 0, todecodeByte.Length);
                var decodedChar = new char[charCount];
                utf8Decode.GetChars(todecodeByte, 0, todecodeByte.Length, decodedChar, 0);
                var result = new String(decodedChar);
                return result;
            }
            catch
            {
                return "";
            }
        }

        public static T Encrypt<T>(object _toEncrypt, bool isEncrypt)
        {
            var toEncrypt = _toEncrypt.ToString();
            const string securitykey = "3ecur1tyKey";
            if (isEncrypt)
            {
                var toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);

                var hashmd5 = new MD5CryptoServiceProvider();
                var keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(securitykey));
                hashmd5.Clear();

                var tdes = new TripleDESCryptoServiceProvider
                               {
                                   Key = keyArray,
                                   Mode = CipherMode.ECB,
                                   Padding = PaddingMode.PKCS7
                               };

                var cTransform = tdes.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                tdes.Clear();

                var value = Convert.ToBase64String(resultArray, 0, resultArray.Length);
                return (T) Convert.ChangeType(value, typeof (T));
            }
            else
            {
                var toEncryptArray = Convert.FromBase64String(toEncrypt);

                var hashmd5 = new MD5CryptoServiceProvider();
                var keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(securitykey));
                hashmd5.Clear();

                var tdes = new TripleDESCryptoServiceProvider
                               {
                                   Key = keyArray,
                                   Mode = CipherMode.ECB,
                                   Padding = PaddingMode.PKCS7
                               };

                var cTransform = tdes.CreateDecryptor();
                var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                tdes.Clear();

                var value = Encoding.UTF8.GetString(resultArray);
                return (T) Convert.ChangeType(value, typeof (T));
            }
        }

        public static string GetExcelConnection(string strFilePath)
        {
            string strConn;
            if (strFilePath.Substring(strFilePath.LastIndexOf('.')).ToLower() == ".xlsx")
                strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strFilePath +
                          ";Extended Properties=\"Excel 12.0;IMEX=1\"";
            else
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strFilePath +
                          ";Extended Properties=\"Excel 8.0;IMEX=1\"";
            return strConn;
        }
        
        public class NameValue
        {
            public string Name { get; set; }
            public object Value { get; set; }
        }

        public static bool Equals(string input1, string input2)
        {
            return input1.Trim().ToUpper() == input2.Trim().ToUpper();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="folder">ex: tourday</param>
        /// <param name="imageName"></param>
        /// <param name="saveThumb"></param>
        /// <param name="maxWidth"></param>
        /// <param name="maxHeight"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string UploadImage(HttpPostedFileBase file, string folder, ref string imageName, ref int filesize, bool saveThumb = false, int maxWidth=1600, int maxHeight=1000, string id = "")
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    //string fileExt = Path.GetExtension(file.FileName);
                    string fileExt = file.ContentType.Split('/')[1];
                    string fileName = (!string.IsNullOrEmpty(id) ? id : Guid.NewGuid().ToString());
                    string physicalFilePath = HttpContext.Current.Server.MapPath("~/data/" + folder + "/");
                    string filePath = physicalFilePath + imageName + "_" + fileName + "." + fileExt;

                    string directory = Path.GetDirectoryName(filePath);
                    if (directory != null) Directory.CreateDirectory(directory);

                    try
                    {
                        WebImage img = new WebImage(file.InputStream);
                        var width = img.Width > maxWidth ? maxWidth : img.Width;
                        var height = img.Height > maxHeight ? maxHeight : img.Height;

                        if (width != img.Width || height != img.Height)
                            img.Resize(width, height, true, true);

                        img.Save(filePath);
                        if (saveThumb)
                        {
                            var imageNameThumb = imageName + "_" + fileName + "_thumb" +"."+ fileExt;
                            if (img.Width > 350)
                            {
                                img.Resize(350, 250, true, true);
                            }
                            img.Save(physicalFilePath + imageNameThumb);
                        }

                        filesize = (int) (new FileInfo(filePath).Length);
                    }
                    catch (Exception ex1)
                    {
                        return "";
                    }

                    return imageName + "_" + fileName + "." + fileExt;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return null;
        }
        public static string UploadFile(HttpPostedFileBase file, string folder, ref string name, ref int filesize, string id = "")
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    string fileExt = Path.GetExtension(file.FileName);
                    string fileName = (!string.IsNullOrEmpty(id) ? id : Guid.NewGuid().ToString());
                    string physicalFilePath = HttpContext.Current.Server.MapPath("~/data/" + folder + "/");
                    string filePath = physicalFilePath + name + "_" + fileName +"."+ fileExt;

                    string directory = Path.GetDirectoryName(filePath);
                    if (directory != null) Directory.CreateDirectory(directory);

                    name = name + "_" + fileName +"."+ fileExt;
                    try
                    {
                        file.SaveAs(filePath);
                    }
                    catch (Exception ex1)
                    {
                        return "";
                    }
                    filesize = (int)(new FileInfo(filePath).Length );
                    return (name);
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return null;
        }
        
    }
}
