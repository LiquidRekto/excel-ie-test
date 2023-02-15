using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE_ExcelImportExportTest.Controllers
{
    [Route("api")]
    [ApiController]
    public class JsonController : ControllerBase
    {
        private IWebHostEnvironment hostingEnv;

        public JsonController(IWebHostEnvironment env)

        {
            

            this.hostingEnv = env;

        }

        [HttpPost]
        [Route("json/showimport")]
        public JsonResult POST_JsonShow(JsonResult x)
        {
            return x;
        }
    }
}
