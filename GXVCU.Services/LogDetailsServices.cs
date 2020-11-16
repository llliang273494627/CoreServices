using GXVCU.Model.Models;
using SqlSugar;
using System.Threading.Tasks;

namespace GXVCU.Services
{
    public class LogDetailsServices
    {
        private readonly SqlSugarClient _sqlSugarClient;

        public LogDetailsServices(SqlSugarClient sqlSugarClient)
        {
            _sqlSugarClient = sqlSugarClient;
        }

        /// <summary>
        /// 添加日志信息
        /// </summary>
        /// <param name="tasksQz"></param>
        /// <returns></returns>
        public async Task<int> AddLogDetails(LogDetails logDetails)
        {
            return await _sqlSugarClient.Insertable(logDetails).ExecuteCommandAsync();
        }

        /// <summary>
        /// 添加日志信息
        /// </summary>
        /// <param name="tasksQz"></param>
        /// <returns></returns>
        public async Task<int> LogInformation(LogDetails logDetails)
        {
            logDetails.LogLevel = 2;
            return await _sqlSugarClient.Insertable(logDetails).ExecuteCommandAsync();
        }

        /// <summary>
        /// 添加日志信息
        /// </summary>
        /// <param name="tasksQz"></param>
        /// <returns></returns>
        public async Task<int> LogError(LogDetails logDetails)
        {
            logDetails.LogLevel = 4;
            return await _sqlSugarClient.Insertable(logDetails).ExecuteCommandAsync();
        }
    }
}
