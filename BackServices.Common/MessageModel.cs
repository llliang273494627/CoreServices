using System;
using System.Collections.Generic;
using System.Text;

namespace BackServices.Common
{
    public class MessageModel<T>
    {
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool Success { get; set; } = false;
        public string Msg { get; set; }
        public T Response { get; set; }
    }
}
