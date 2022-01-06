using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace XProject.Web.Infrastructure.Helpers
{
    public class PDFHelper
    {
        public static byte[] GeneratePdfFileFromUrl(string path,  HttpServerUtilityBase server,string name)
        {
            string url = path;

            //output PDF file Path
            string filename = string.Format(name + "-{0}.pdf", DateTime.Now.ToString("ddMMyy"));

            string filepath = "\"" + server.MapPath("~/data/pdf/") + filename;
            string directory = Path.GetDirectoryName( server.MapPath("~/data/pdf"));
            if (directory != null) Directory.CreateDirectory(directory);
            //variable to store pdf file content


            string args = " -q -n --disable-smart-shrinking -L 0 -R 0 --outline-depth 0 --dpi 300 --page-size A4 --viewport-size 1190x1684";


         

            var process = new Process
            {
                StartInfo =
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    FileName = server.MapPath("~/wkhtmltopdf/") + "wkhtmltopdf.exe",
                    Arguments = args + " " + url + " " + filepath,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true
                }
            };
            process.Start();

            //wait until the conversion is done
            process.WaitForExit();

            // read the exit code, close process
            process.Close();
            string fileReadPath = server.MapPath("~/data/pdf/") + filename;
            //initialize the filestream with filepath
            var fs = new FileStream(fileReadPath, FileMode.Open, FileAccess.Read);
            var fileContent = new byte[(int)fs.Length];

            //read the content
            fs.Read(fileContent, 0, (int)fs.Length);

            //close the stream
            fs.Close();
            File.Delete(fileReadPath);
            return fileContent;
        }

        public static byte[] GeneratePdfFile(string htmlCode, HttpServerUtilityBase server, string name)
        {
            string filename = string.Format(name + "-{0}.pdf", DateTime.Now.ToString("ddMMyy"));
            string wkhtmlToPdfExePath = server.MapPath("~/wkhtmltopdf/") + "wkhtmltopdf.exe";
            string filepath = server.MapPath("~/data/pdf/") + filename;
            string directory = Path.GetDirectoryName(filepath);
            if (directory != null) Directory.CreateDirectory(directory);
            Process process;
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = wkhtmlToPdfExePath;
            processStartInfo.WorkingDirectory = server.MapPath("~/data/pdf/");

            // run the conversion utility
            processStartInfo.UseShellExecute = false;
            processStartInfo.CreateNoWindow = true;
            processStartInfo.RedirectStandardInput = true;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardError = true;

            // note: that we tell wkhtmltopdf to be quiet and not run scripts
            string args = "-q -n ";
            args += "--disable-smart-shrinking ";
            args += "";
            args += "--outline-depth 0 ";
            args += "--page-size A4 ";
            args += " - -";

            processStartInfo.Arguments = args;

            process = Process.Start(processStartInfo);

            using (StreamWriter stramWriter = process.StandardInput)
            {
                stramWriter.AutoFlush = true;
                stramWriter.Write(htmlCode);
            }

            //read output
            byte[] buffer = new byte[32768];
            byte[] file;
            using (var memoryStream = new MemoryStream())
            {
                while (true)
                {
                    int read = process.StandardOutput.BaseStream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                        break;
                    memoryStream.Write(buffer, 0, read);
                }
                file = memoryStream.ToArray();
            }

            process.StandardOutput.Close();
            // wait or exit
            process.WaitForExit(60000);

            // read the exit code, close process
            int returnCode = process.ExitCode;
            process.Close();

            process.Dispose();
            if (returnCode == 0 || returnCode == 1)
            {
                return file;
            }
            else
            {
                throw new Exception(string.Format("Could not create PDF, returnCode:{0}", returnCode));
            }

        }

        public static void ExportToFile(HttpResponseBase response, byte[] fileContent, string fileName, string fileExt)
        {
            if (fileContent != null)
            {
                string attachment = string.Format("attachment; filename={0}-{1}.{2}", fileName, DateTime.Now.ToString("ddMMyy"), fileExt);
                response.Clear();
                response.AddHeader("content-disposition", attachment);
                response.ContentType = "application/" + fileExt;
                response.BinaryWrite(fileContent);
                response.End();
            }
        }

        public static string SaveFile(HttpServerUtilityBase server, string filename, byte[] fileContent)
        {
            try
            {
                System.IO.FileStream file = System.IO.File.Create(server.MapPath("~/data/pdf/") + filename);
                file.Write(fileContent, 0, fileContent.Length);
                file.Close();
                return "~/data/pdf/"+ filename;
            }
            catch {
                return "";
            }
            
        }
    }
}