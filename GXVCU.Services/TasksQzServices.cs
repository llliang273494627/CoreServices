using GXVCU.Model.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GXVCU.Services
{
    public class TasksQzServices
    {
        private readonly ISqlSugarClient _sqlSugarClient;

        public TasksQzServices(ISqlSugarClient sqlSugarClient)
        {
            _sqlSugarClient = sqlSugarClient;
        }

        /// <summary>
        /// 获取所以的任务
        /// </summary>
        /// <param name="obj"></param>
        public async Task<List<TasksQz>> GetTasks()
        {
            return await _sqlSugarClient.Queryable<TasksQz>().ToListAsync();
        }

        public async Task<int> AddTasks(TasksQz tasksQz)
        {
            return await _sqlSugarClient.Insertable(tasksQz).ExecuteCommandAsync();
        }

        public async Task<bool> UpdataJob(TasksQz tasksQz)
        {
            return await _sqlSugarClient.Updateable(tasksQz).ExecuteCommandHasChangeAsync();
        }

        public async Task<TasksQz> SelectJob(int jobId)
        {
            return  await _sqlSugarClient.Queryable<TasksQz>().Where(c => c.Id == jobId).FirstAsync();
        }
    }
}
