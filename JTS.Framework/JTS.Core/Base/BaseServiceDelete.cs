/*************************************************************************
 * 文件名称 ：ServiceBaseDelete.cs                          
 * 描述说明 ：定义数据服务基类中的删除处理
 * 
 **************************************************************************/

namespace JTS.Core
{
    public partial class BaseService<T> where T : BaseEntity, new()
    {
        protected virtual bool OnBeforeDelete(DeleteEventArgs arg)
        {
            return true;
        }

        protected virtual void OnAfterDelete(DeleteEventArgs arg)
        {

        }

        /// <summary>
        /// 根据条件删除记录 需要带上 and
        /// </summary>
        /// <param name="value">条件，需要带上 and</param>
        /// <returns>int</returns>
        public int DeleteByWhere(string where)
        {
            where = string.IsNullOrEmpty(where) ? " 1=1 " : " 1=1 "+where;
            string sql = string.Format("delete from {0} where {1}", typeof(T).Name, where);
            var result = 0;
            result = this.db.Sql(sql, null).Execute();
            return result;
        }

        /// <summary>
        /// 根据sql语句，删除记录
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>int</returns>
        public int DeleteBySql(string sql)
        {
          var  result = this.db.Sql(sql, null).Execute();
            return result;
        }

        /// <summary>
        /// 根据主键值删除记录
        /// </summary>
        /// <param name="value">主键值</param>
        /// <returns>int</returns>
        public int DeleteByPrimaryField(object value)
        {
            var tableAttribute = AttributeHelper.GetCustomAttribute<TableAttribute>(typeof(T));
            ParamDelete p = ParamDelete.Instance().From(tableAttribute.TableName).AndWhere(tableAttribute.PrimaryField, value);
            return this.Delete(p);
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="param">ParamDelete</param>
        /// <returns>int</returns>
        public int Delete(ParamDelete param)
        {
            var result = 0;

            db.UseTransaction(true);
            var rtnBefore = this.OnBeforeDelete(new DeleteEventArgs() { db = db, data = param.GetData() });
            if (!rtnBefore) return result;
            result = BuilderParse(param).Execute();
            this.CommandResult.Set(true, APP.Msg_Delete_Success);
            this.OnAfterDelete(new DeleteEventArgs() { db = db, data = param.GetData(), executeValue = result });
            db.Commit();

            return result;
        }
    }
}
