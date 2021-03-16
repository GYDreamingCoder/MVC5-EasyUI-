using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClownFish;

namespace JTS.Core
{
    /// <summary>
    /// 基本分页信息类。
    /// <para>继承自ClownFish.PagingInfo</para>
    /// </summary>
    public class PagingInfoExtension : ClownFish.PagingInfo
    {
        public PagingInfoExtension()
        {
        }


        // 摘要:
        //     分页序号，从0开始计数
        //   public int PageIndex { get; set; }
        //
        // 摘要:
        //     分页大小
        //  public int PageSize { get; set; }
        //
        // 摘要:
        //     从相关查询中获取到的符合条件的总记录数
        // public int TotalRecords { get; set; }

        // 摘要:
        //     计算总页数
        //public int CalcPageCount() {
        //    int result;
        //    if (this.PageSize == 0 || this.TotalRecords == 0)
        //    {
        //        result = 0;
        //    }
        //    else
        //    {
        //        result = (int)Math.Ceiling((double)this.TotalRecords / (double)this.PageSize);
        //    }
        //    return result;
        //}


        /// <summary>
        /// 要查询的表名(或者视图名)。
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 要显示的列名，用逗号隔开 。默认值：*。
        /// </summary>
        public string Columns { get; set; }

        /// <summary>
        /// 排序的列名。
        /// </summary>
        public string OrderColumn { get; set; }

        /// <summary>
        /// 排序的方式，升序为asc,降序为 desc 。
        /// </summary>
        public string Order { get; set; }

        /// <summary>
        /// where 条件，如果不带查询条件，请用 1=1 
        /// </summary>
        public string Where { get; set; }

    }
}
