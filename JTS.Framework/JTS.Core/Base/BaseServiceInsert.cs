/*************************************************************************
 * 文件名称 ：ServiceBaseInsert.cs                          
 * 描述说明 ：定义数据服务基类中的插入处理
 * 
 **************************************************************************/

using System;
using System.Reflection;
using JTS.Utils;
using Newtonsoft.Json.Linq;
namespace JTS.Core
{
    public partial class BaseService<T> where T : BaseEntity, new()
    {
        protected virtual bool OnBeforeInsert(InsertEventArgs arg)
        {
            return true;
        }

        protected virtual void OnAfterInsert(InsertEventArgs arg)
        {

        }

        /// <summary>
        /// 获取Insert参数类
        /// </summary>
        /// <param name="data">实体对象</param>
        /// <returns>ParamInsert</returns>
        public ParamInsert GetParamInsert(T data)
        {
            var paramInsert = ParamInsert.Instance().Insert(typeof(T).Name);
            var tableAttribute = AttributeHelper.GetCustomAttribute<TableAttribute>(typeof(T));
            foreach (PropertyInfo p in typeof(T).GetProperties())
            {
                bool existsC = GetCommonFieldName().ContainsKey(p.Name); //
                if (p.Name == "tempid" || p.Name == tableAttribute.IdentityField || existsC == true) continue;
                if (tableAttribute.IgnoreInsertFieldsList != null && tableAttribute.IgnoreInsertFieldsList.Contains(p.Name) == true) continue; 
                paramInsert.Column(p.Name, data.GetValue(p.Name));
            }
            return paramInsert;
        }

        /// <summary>
        /// 插入记录
        /// </summary>
        /// <param name="data">实体对象</param>
        /// <returns></returns>
        public int Insert(T data)
        {
            return this.Insert(this.GetParamInsert(data));
        }

        /// <summary>
        /// 插入记录
        /// </summary>
        /// <param name="param">ParamInsert</param>
        /// <returns></returns>
        public int Insert(ParamInsert param)
        {
            var result = 0;
            db.UseTransaction(true);
            var rtnBefore = this.OnBeforeInsert(new InsertEventArgs() { db = db, data = param.GetData() });
            if (!rtnBefore) return result;
            //var Identity = BaseEntity.GetAttributeFields<T, TableAttribute>();
            var Identity = AttributeHelper.GetCustomAttribute<TableAttribute>(typeof(T)).IdentityField;
            result = Identity != "" ? BuilderParse(param).ExecuteReturnLastId<int>() : BuilderParse(param).Execute();
            this.CommandResult.Set(true, APP.Msg_Insert_Success);
            this.OnAfterInsert(new InsertEventArgs() { db = db, data = param.GetData(), executeValue = result });
            db.Commit(); 
            return result;
        }
    }
}
