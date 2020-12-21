using BackServices.Common.SettingEntity;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using ThoughtWorks.QRCode.Codec;
using ZXing;
using ZXing.Common;

namespace BackServices.Common.Helper
{
    public class HelperCode
    {
        /// <summary>
        /// 生成条码
        /// </summary>
        /// <param name="valueCode">条码值</param>
        /// <param name="width">条码宽</param>
        /// <param name="herght">条码高</param>
        /// <param name="format">条码格式</param>
        /// <returns></returns>
        public static Bitmap CreatCode(string valueCode, int width, int herght,int format)
        {
            try
            {
                if (string.IsNullOrEmpty(valueCode))
                    valueCode = " ";
                EncodingOptions options = new EncodingOptions()
                {
                    Width = width,
                    Height = herght,
                };
                BarcodeWriter writer = new BarcodeWriter();
                //使用ITF 格式，不能被现在常用的支付宝、微信扫出来
                //如果想生成可识别的可以使用 CODE_128 格式
                //writer.Format = BarcodeFormat.ITF;
                writer.Format = (BarcodeFormat)format;
                writer.Options = options;
                Bitmap map = writer.Write(valueCode.Replace("@@", "+"));
                return map;
            }
            catch (Exception ex)
            {
                HelperLog.Error("生成条码失败！", ex);
                return null;
            }
        }

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="valueCode">条码值</param>
        /// <param name="mode">编码模式</param>
        /// <param name="scale">比例</param>
        /// <param name="version">版本</param>
        /// <param name="correct">更正</param>
        /// <returns></returns>
        public static Bitmap CreatQRCode(string valueCode, int mode,int scale,int version,int correct)
        {
            try
            {
                QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                qrCodeEncoder.QRCodeEncodeMode = (QRCodeEncoder.ENCODE_MODE)mode;
                qrCodeEncoder.QRCodeScale = scale;
                qrCodeEncoder.QRCodeVersion = version;
                qrCodeEncoder.QRCodeErrorCorrect = (QRCodeEncoder.ERROR_CORRECTION)correct;
                Bitmap image = qrCodeEncoder.Encode(valueCode);
                return image;
            }
            catch (Exception ex)
            {
                HelperLog.Error("生成二维码码失败！", ex);
                return null;
            }
        }

        /// <summary>
        /// 默认条码设置
        /// </summary>
        /// <param name="valueCode"></param>
        /// <returns></returns>
        public static Bitmap DefaultCode(string valueCode)
        {
            //使用ITF 格式，不能被现在常用的支付宝、微信扫出来
            //如果想生成可识别的可以使用 CODE_128 格式
            return CreatCode(valueCode, 560, 60,16);
        }

        /// <summary>
        /// 默认二维码设置
        /// </summary>
        /// <param name="valueCode">条码值</param>
        /// <returns></returns>
        public static Bitmap DefaultQRCode(string valueCode)
        {
            return CreatQRCode(valueCode, 2, 4, 8, 1);
        }
    }
}
