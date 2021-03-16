using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JTS.Core;

namespace JTS.Entity
{
    public class dict_treedata
    {
        public string id { get; set; }
        public string text { get; set; }
        public string iconCls { get; set; }
        public string state { get; set; }
        public dict_attributes attributes { get; set; }
        public List<dict_treedata> children { get; set; }
    }

    public class dict_attributes
    {
        public string dict_code { get; set; }
        public string dict_pcode { get; set; }
        public int dict_sort { get; set; }
        public int dict_enabled { get; set; }
        public string dict_remark { get; set; }
    }

}
