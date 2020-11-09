using GXVCU.Common.DB;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GXVCU.Api.Comm
{
    public static class AutoApps
    {
        /// <summary>
        /// 添加种子数据
        /// </summary>
        /// <param name="app"></param>
        /// <param name="dBSeed"></param>
        public static void UseSeedDataMildd(this IApplicationBuilder app, DBSeed dBSeed)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            dBSeed.CreateDataBase();

        }
    }
}
