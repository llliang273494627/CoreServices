using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GXVCU.Common;
using GXVCU.Common.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GXVCU.Api.Controllers
{
    /// <summary>
    /// 日志信息
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LogInfoController : ControllerBase
    {
        [HttpPost("GetErrorAllFiles")]
        public MessageModel<string[]> GetErrorAllFiles()
        {
            var res = new MessageModel<string[]>();
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Log");
            if (Directory.Exists(filePath))
            {
                res.Response = Directory.GetFiles(filePath);
            }
            return res;
        }

        [HttpPost("ReadFileTextAll")]
        public MessageModel<Exception> ReadFileTextAll(string path)
        {
            return FileHelper.ReadFileTextAll(path);
        }
    }
}
