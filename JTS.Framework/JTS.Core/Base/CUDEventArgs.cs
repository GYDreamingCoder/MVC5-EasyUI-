/*************************************************************************
 * 文件名称 ：CUDEventArgs.cs                          
 * 描述说明 ：插入/更新 /删除 (CUD)事件参数
 * 
 **************************************************************************/
namespace JTS.Core
{
    using FluentData;

    /// <summary>
    /// 插入事件参数
    /// </summary>
    public class InsertEventArgs
    {
        public IDbContext db { get; set; }
        public ParamInsertData data { get; set; }
        public int executeValue { get; set; }
    }

    /// <summary>
    /// 更新事件参数
    /// </summary>
    public class UpdateEventArgs
    {
        public IDbContext db { get; set; }
        public ParamUpdateData data { get; set; }
        public int executeValue { get; set; }
    }

    /// <summary>
    /// 删除事件参数
    /// </summary>
    public class DeleteEventArgs
    {
        public IDbContext db { get; set; }
        public ParamDeleteData data { get; set; }
        public int executeValue { get; set; }
    }
}
