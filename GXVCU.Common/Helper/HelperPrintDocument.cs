using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Text;

namespace GXVCU.Common.Helper
{
    public class HelperPrintDocument
    {
        public HelperPrintDocument()
        {
            _printDocument = new PrintDocument();
            _printDocument.PrintPage += HelperPrintDocument_PrintPage;
        }

        /// <summary>
        /// 实例化对象
        /// </summary>
        /// <param name="printName">打印机名称</param>
        public HelperPrintDocument(string printName)
        {
            _printDocument = new PrintDocument();
            _printDocument.PrinterSettings.PrinterName = printName;
            _printDocument.PrintPage += HelperPrintDocument_PrintPage;
        }

        /// <summary>
        /// 打印数据对象
        /// </summary>
        private Bitmap _bitmap { get; set; }
        /// <summary>
        /// 打印机对象
        /// </summary>
        private PrintDocument _printDocument { get; set; }

        /// <summary>
        /// 打印事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HelperPrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                //打印文档
                Graphics g = e.Graphics;//获得绘图对象
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //设置高质量,低速度呈现平滑程度
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.Half;
                g.DrawImage(_bitmap, new Rectangle(1, 1, 900, 250), new Rectangle(1, 1, 1900, 500), GraphicsUnit.Pixel);
            }
            catch (Exception ex)
            {
                HelperLog.Error("打印失败！", ex);
            }
        }

        /// <summary>
        /// 获取安装的所有打印机名称
        /// </summary>
        /// <returns></returns>
        public string[] GetPrintNames()
        {
            try
            {
                var prints = PrinterSettings.InstalledPrinters;
                string[] strs = new string[prints.Count];
                PrinterSettings.InstalledPrinters.CopyTo(strs, 0);
            }
            catch (Exception ex)
            {
                HelperLog.Error("获取安装所有打印机失败！", ex);
            }
            return new string[] { };
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="entityBitmap"></param>
        public void Print(EntityBitmapLHGQ entityBitmap)
        {
            _bitmap = HelperBitmap.GetBitmap(entityBitmap);
            _printDocument.Print();
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="entityBitmap"></param>
        public void Print(Bitmap bitmap)
        {
            _bitmap = bitmap;
            _printDocument.Print();
        }


    }
}
