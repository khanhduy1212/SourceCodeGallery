using System;
using System.IO;


namespace XProject.Domain.Helpers
{
    public class ImageHelper
    {
        public static void RemoveImage(string filePath)
        {
            try
            {
                var extension = Path.GetExtension(filePath);
                if (extension != null)
                {
                    string fileThumbPath = filePath.Replace(extension, "_thumb" + extension);
                    string fileScalePath = filePath.Replace(extension, "_x1024" + extension);
                    if (File.Exists(filePath))
                        File.Delete(filePath);
                    if (File.Exists(fileScalePath))
                        File.Delete(fileScalePath);
                    if (File.Exists(fileThumbPath))
                        File.Delete(fileThumbPath);

                }
                else if (File.Exists(filePath))
                    File.Delete(filePath);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
