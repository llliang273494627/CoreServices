using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using GXVCU.Api.Comm;
using GXVCU.Common;
using GXVCU.Common.FromBodys;
using GXVCU.Common.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GXVCU.Api.Controllers.v2
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApbController : ControllerBase
    {
        private readonly ILogger<ApbController> _logger;

        public ApbController(ILogger<ApbController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [CustomRoute(ApiVersions.V2, "apbs")]
        public IEnumerable<string> Get()
        {
            return new string[] { "第二版的 apbs" };
        }

        /// <summary>
        /// 获取本机安装的打印机名称
        /// </summary>
        /// <param name="entityValue"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomRoute(ApiVersions.V2, "GetPrintNames")]
        public MessageModel<string[]> GetPrintNames()
        {
            var data = new MessageModel<string[]>();
            try
            {
                data.Response =new HelperPrintDocument().GetPrintNames();
                data.Success = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                data.Success = false;
                data.Response = null;
                data.Msg = ex.Message;
            }
            return data;
        }

        /// <summary>
        /// 打印测试
        /// </summary>
        /// <param name="entityBitmap"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomRoute(ApiVersions.V2, "Print")]
        public MessageModel<string> Print([FromBody] EntityBitmapLHGQ entityBitmap)
        {
            var data = new MessageModel<string>();
            try
            {
                var print = new HelperPrintDocument();
                print.Print(entityBitmap);
                data.Response = "已经发出打印请求！";
                data.Success = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                data.Success = false;
                data.Response = "打印失败！";
                data.Msg = ex.Message;
            }
            return data;
        }

        /// <summary>
        /// 获取条码Base64Str
        /// </summary>
        /// <param name="entityValue"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomRoute(ApiVersions.V2, "GetCodeBase64Str")]
        public MessageModel<string> GetCodeBase64Str([FromBody] BodyQRCode entityValue)
        {
            var data = new MessageModel<string>();
            try
            {
                MemoryStream memory = new MemoryStream();
                var bmp = HelperCode.DefaultCode(entityValue.Value);
                bmp.Save(memory, ImageFormat.Bmp);
                data.Response = Convert.ToBase64String(memory.ToArray());
                data.Success = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                data.Success = false;
                data.Response = null;
                data.Msg = ex.Message;
            }
            return data;
        }

        /// <summary>
        /// 获取二维码Base64Str
        /// </summary>
        /// <param name="entityValue"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomRoute(ApiVersions.V2, "GetQRCodeBase64Str")]
        public MessageModel<string> GetQRCodeBase64Str([FromBody] BodyQRCode entityValue)
        {
            var data = new MessageModel<string>();
            try
            {
                MemoryStream memory = new MemoryStream();
                var bmp = HelperCode.DefaultQRCode(entityValue.Value);
                bmp.Save(memory, ImageFormat.Bmp);
                data.Response = Convert.ToBase64String(memory.ToArray());
                data.Success = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                data.Success = false;
                data.Response = null;
                data.Msg = ex.Message;
            }
            return data;
        }

    }
}
