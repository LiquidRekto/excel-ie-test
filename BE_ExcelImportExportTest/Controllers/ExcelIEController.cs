
using System.IO;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using FastExcel;

namespace BE_ExcelImportExportTest.Controllers
{
    [ApiController]
    [Route("api")]
    public class ExcelIEController : ControllerBase
    {
        private IWebHostEnvironment hostingEnv;

        public ExcelIEController(IWebHostEnvironment env)

        {

            this.hostingEnv = env;

        }

        [HttpPost]
        [Route("excel/upload")]
        public string POST_uploadExcel()
        {
            string result = string.Empty;

            try

            {

                long size = 0;

                var file = Request.Form.Files;

                Console.WriteLine(file.ToString());

                var filename = ContentDispositionHeaderValue.Parse(file[0].ContentDisposition).FileName.Trim('"');

                string FilePath = hostingEnv.WebRootPath + $@"storage\{filename}";

                size += file[0].Length;
                string output = "File: \"" + file[0].Name + "\" contains the following sheets:\n";

                using (FileStream fs = System.IO.File.Create(FilePath))

                {

                    file[0].CopyTo(fs);

                    FastExcel.FastExcel fe = new FastExcel.FastExcel(fs);
                        


                        foreach (Worksheet sheet in fe.Worksheets)
                        {
                            output += sheet.Name + "\n";
                        }

                    fs.Flush();
                    

                }
                System.IO.File.Delete(FilePath);

                result = output;

            }

            catch (Exception ex)

            {

                result = ex.Message;

            }

            return result;
        }

        [HttpGet]
        [Route("data/hello")]
        public string GET_hello()
        {
            return "{string:Hello world!}";
        }
        [HttpGet]
        [Route("data/fromexcel/{path}/sheets")]
        public string GET_sheetsFromExcel(string path)
        {
            FileInfo file = new FileInfo(path);
            FastExcel.FastExcel fe;
            try
            { 
                // Using to prevent file already accessed error
                using (fe = new FastExcel.FastExcel(file))
                {
                    string output = "File: \"" + path + "\" contains the following sheets:\n";


                    foreach (Worksheet sheet in fe.Worksheets)
                    {
                        output += sheet.Name + "\n";
                    }
                    return output;
                }
            }
            catch (Exception ex)
            {
                return "ERROR: " + ex.Message;
            }
          
           
        }

        [HttpGet]
        [Route("data/fromexcel/{path}/atsheet/{sheetname}")]
        public string GET_dataFromExcelSheet(string path, string sheetname)
        {
            FileInfo file = new FileInfo(path);
            FastExcel.FastExcel fe;
            Worksheet worksheet;

            try
            {
                using (fe = new FastExcel.FastExcel(file))
                {

                    try
                    {
                        worksheet = fe.Read(sheetname);
                    }
                    catch (Exception ex)
                    {
                        return "ERROR: " + ex.Message;
                    }
                    var rows = worksheet.Rows.ToArray();
                    string output = "";
                    foreach (Row row in rows)
                    {
                        foreach (Cell cell in row.Cells)
                        {
                            output += cell.Value + " ";
                        }
                        output += "\n";
                    }
                    return output;
                }
            } 
            catch (Exception ex)
            { 
                return "ERROR: " + ex.Message;
            }
            
        }
    }
}
