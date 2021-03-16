/*************************************************************************
 * 文件名称 ：ServiceBaseUpdate.cs                          
 * 描述说明 ：定义数据服务基类中的更新处理
 * 
 **************************************************************************/

using System;
using System.Reflection;
using JTS.Utils;
namespace JTS.Core
{
    public partial class BaseService<T> where T : BaseEntity, new()
    {
        protected virtual bool OnBeforeUpdate(UpdateEventArgs arg)
        {
            return true;
        }

        protected virtual void OnAfterUpdate(UpdateEventArgs arg)
        {

        }
        /// <summary>
        /// 获取Update参数类
        /// </summary>
        /// <param name="data">实体对象</param>
        /// <returns>ParamUpdate</returns>
        public ParamUpdate GetParamUpdate(T data)
        {
            var paramUpdate = ParamUpdate.Instance().Update(typeof(T).Name);
            PropertyInfo[] pis = typeof(T).GetProperties();
            var tableAttribute = AttributeHelper.GetCustomAttribute<TableAttribute>(typeof(T));
            foreach (PropertyInfo p in pis)
            {
                bool existsC = GetCommonFieldName().ContainsKey(p.Name);//排开所有公共字段
                if (p.Name == "tempid" || p.Name == tableAttribute.PrimaryField || p.Name == tableAttribute.IdentityField || existsC == true) continue;
                if (tableAttribute.IgnoreUpdateFieldsList != null && tableAttribute.IgnoreUpdateFieldsList.Contains(p.Name) == true) continue;
                paramUpdate.Column(p.Name, data.GetValue(p.Name));
            }
            paramUpdate.AndWhere(tableAttribute.PrimaryField, data.GetValue(tableAttribute.PrimaryField));
            return paramUpdate;
        }

        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="data">实体对象</param>
        /// <returns>int</returns>
        public int Update(T data)
        {
            return this.Update(this.GetParamUpdate(data));
        }


        /// <summary>
        ///  更新记录
        /// </summary>
        /// <param name="param">ParamUpdate</param>
        /// <returns>int</returns>
        public int Update(ParamUpdate param)
        {
            var result = 0;
            db.UseTransaction(true);
            var rtnBefore = this.OnBeforeUpdate(new UpdateEventArgs() { db = db, data = param.GetData() });
            if (!rtnBefore) return result;
            string sql = BuilderParse(param).Data.Command.Data.Context.Data.FluentDataProvider.GetSqlForUpdateBuilder(BuilderParse(param).Data);
            LogHelper.WriteDb("(ParamUpdate)执行更新记录sql语句：" + sql);
            result = BuilderParse(param).Execute();
            this.CommandResult.Set(true, APP.Msg_Update_Success);
            this.OnAfterUpdate(new UpdateEventArgs() { db = db, data = param.GetData(), executeValue = result });
            db.Commit();
            return result;
        }
    }
}
