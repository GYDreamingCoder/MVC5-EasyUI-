using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JTS.Core;

namespace JTS.Entity
{
    public class org_treedata
    {
        public string id { get; set; }
        public string text { get; set; }
        public string iconCls { get; set; }
        public string state { get; set; }
        public org_attributes attributes { get; set; }
        public List<org_treedata> children { get; set; }
    }

    public class org_attributes
    {
        /// <summary>
        /// 公司需要用。对应：CompanyType  公司分类.0=未定义 1=总部(集团) 2=公司 3=分公司
        /// </summary>
        public int org_type { get; set; }
        public string org_code { get; set; }
        public string org_pcode { get; set; }
        /// <summary>
        /// 部门用于 对应的CompanyCode字段，公司可以不用
        /// </summary>
        public string org_fcode { get; set; }
        public int org_sort { get; set; }
        public int org_enabled { get; set; }
        public string org_remark { get; set; }
    }

}
