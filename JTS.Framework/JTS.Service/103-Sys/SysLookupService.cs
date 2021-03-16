using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTS.Core;
using JTS.Entity;
using JTS.Utils;

namespace JTS.Service
{
    public class SysLookupService : BaseService<SysLookup>
    {
        /// <summary>
        /// SysLookupService实例
        /// </summary>
        /// <returns>SysLookupService</returns>
        public static SysLookupService Instance()
        {
            return new SysLookupService();
        }
        /// <summary>
        /// 获取数据字典分类。
        /// </summary>
        /// <param name="lookupID">操作数据需要的参数。</param>
        /// <returns>返回List数据列表。</returns>
        public DataTable GetLookupID(string lookupID)
        {
            string sql = "select LookupID,LookupName,LookupType from SysLookup  group by LookupType,LookupID,LookupName";
            return this.GetDataTable_Fish(sql);
        }

        /// <summary>
        /// 删除记录。
        /// </summary>
        /// <param name="ID">ID</param>
        /// <returns>返回bool</returns>
        public bool Delete(int ID)
        {
            string sql = "delete from dbo.SysLookup where ID=" + ID;
            bool exists = this.Exists(" LookupType=0 and ID=" + ID);
            if (exists)//如果删除的记录是系统数据字典，则返回false
            {
                return false;
            }
            int result = this.ExecuteNonQuery(sql);
            return result > 0;
        }
    }
}
