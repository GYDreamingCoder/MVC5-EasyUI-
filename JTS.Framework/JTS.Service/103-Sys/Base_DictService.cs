//------------------------------------------------------------------------------
// <自动生成>
//   此代码由代码生成器生成 autocode 2015-11-05 22:37:42
// </自动生成>
//------------------------------------------------------------------------------
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
    /// <summary>
    /// 【数据字典分类】服务
    /// </summary>
    public class Base_DictService : BaseService<Base_Dict>
    {

        /// <summary>
        /// 【数据字典分类】服务实例【单例模式】
        /// </summary>
        private static readonly Base_DictService _Instance = new Base_DictService();

        /// <summary>
        /// 获取【数据字典分类】服务实例【单例模式】
        /// </summary>
        public static Base_DictService Instance
        {
            get
            {
                return _Instance;
            }
        }

        public List<dict_treedata> GetTreeData_Dict()
        {
            List<Base_Dict> listDict = Base_DictService.Instance.GetList_Fish("and 1=1 order by Sort ASC", true);
            List<dict_treedata> listTreeData = new List<dict_treedata>();

            //创建一个默认根节点
            dict_treedata rootdata = new dict_treedata()
            {
                id = "all",
                text = "所有数据字典分类",
                iconCls = "icon-standard-house",
                state = "open",
                children = new List<dict_treedata>(),
                attributes = new dict_attributes() { dict_code = "0", dict_pcode="0", dict_sort = 0, dict_enabled = 1, dict_remark = "所有数据字典分类" }
            };

            //List<dict_treedata> listTreeData_Children = new List<dict_treedata>();
            List<Base_Dict> listChildrenDict = listDict.FindAll(p => p.ParentDictCode == "0");//查询所有根节点。
            if (listChildrenDict.Count > 0)
            {
                rootdata.children = GetTreeData_Children(listDict, listChildrenDict);
            }

            listTreeData.Add(rootdata);
            return listTreeData;
        }

        private List<dict_treedata> GetTreeData_Children(List<Base_Dict> listDict, List<Base_Dict> listDict_Children)
        {
            List<dict_treedata> listResult = new List<dict_treedata>();
            foreach (var item in listDict_Children)
            {
                dict_treedata tempdata = new dict_treedata()
                {
                    id = item.DictCode,
                    text = item.DictName,
                    iconCls = "icon-standard-image",
                    state = "open",
                    children = new List<dict_treedata>(),
                    attributes = new dict_attributes() { dict_code = item.DictCode, dict_pcode = item.ParentDictCode, dict_sort = item.Sort, dict_enabled = item.Enabled, dict_remark = item.Remark }
                };
                List<dict_treedata> listTreeData_Children = new List<dict_treedata>(); //存储所有下级节点数据
                //递归下级公司
                var listDict_Children_Temp = listDict.FindAll(p => p.ParentDictCode == item.DictCode);//递归获取子节点
                if (listDict_Children_Temp.Count > 0)
                {
                    listTreeData_Children = GetTreeData_Children(listDict, listDict_Children_Temp);
                    tempdata.children = listTreeData_Children;
                }
                listResult.Add(tempdata);
            }
            return listResult;
        }


    }
}
