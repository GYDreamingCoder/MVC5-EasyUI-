//------------------------------------------------------------------------------
// <自动生成>
//   此代码由代码生成器生成 autocode 2015-11-05 22:37:40
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
using Newtonsoft.Json.Linq;

namespace JTS.Service
{
    /// <summary>
    /// 【公司】服务实例
    /// </summary>
    public class Base_CompanyService : BaseService<Base_Company>
    {

        /// <summary>
        /// 【公司】服务实例【单例模式】
        /// </summary>
        private static readonly Base_CompanyService _Instance = new Base_CompanyService();

        /// <summary>
        /// 获取【公司】服务实例【单例模式】
        /// </summary>
        public static Base_CompanyService Instance
        {
            get
            {
                return _Instance;
            }
        }


        #region  查询公司和部门的数据，最终构造成tree数据，便于树状显示
        /// <summary>
        /// 查询公司和部门的数据，最终构造成tree数据，便于树状显示
        /// </summary>
        public List<org_treedata> GetTreeData_Comany()
        {
            var listComany = Base_CompanyService.Instance.GetList_Fish("and 1=1 order by Sort ASC", true);
            var listDepartment = Base_DepartmentService.Instance.GetList_Fish("and 1=1 order by Sort ASC", true);
            List<org_treedata> listTreeData = new List<org_treedata>();

            var rootParentComany = listComany.Find(p => p.ParentCode == "0");//查询根节点。 其实就一个根节点，集团
            org_treedata rootdata = new org_treedata()
            {
                id = rootParentComany.CompanyCode,
                text = rootParentComany.CompanyName,
                iconCls = "icon-standard-house",
                state = "open",
                attributes = new org_attributes() { org_type = 1, org_code = rootParentComany.CompanyCode, org_pcode = rootParentComany.ParentCode, org_sort = rootParentComany.Sort, org_enabled = rootParentComany.Enabled, org_remark = rootParentComany.Remark }
            };

            //根公司的下级公司
            List<org_treedata> listTreeData_ChildrenComany = new List<org_treedata>();
            //根节点下一级节点 
            var listComany_Children = listComany.FindAll(p => p.ParentCode == rootParentComany.CompanyCode);//一级节点所有公司
            if (listComany_Children.Count > 0)
            {
                listTreeData_ChildrenComany = GetTreeData_ChildrenComany(listComany, listComany_Children, listDepartment);
            }
            //根公司的下级部门
            List<org_treedata> listTreeData_ChildrenDepartment = new List<org_treedata>();
            var listDepartment_Current = listDepartment.FindAll(p => p.CompanyCode == rootParentComany.CompanyCode && p.ParentCode == "0");//获取当前公司下面的所有一级部门
            if (listDepartment_Current.Count > 0)
            {
                listTreeData_ChildrenDepartment = GetTreeData_Department(listDepartment, listDepartment_Current);
            }
            listTreeData_ChildrenComany.AddRange(listTreeData_ChildrenDepartment);
            rootdata.children = listTreeData_ChildrenComany;
            listTreeData.Add(rootdata);
            return listTreeData;
        }
        private List<org_treedata> GetTreeData_ChildrenComany(List<Base_Company> listComany, List<Base_Company> listComany_Children, List<Base_Department> listDepartment)
        {
            List<org_treedata> listResult = new List<org_treedata>();
            foreach (var item in listComany_Children)
            {
                org_treedata tempdata = new org_treedata()
                {
                    id = item.CompanyCode,
                    text = item.CompanyName,
                    iconCls = "icon-standard-image",
                    state = "open",
                    children = new List<org_treedata>(),
                    attributes = new org_attributes() { org_type = item.CompanyType, org_code = item.CompanyCode, org_pcode = item.ParentCode, org_sort = item.Sort, org_enabled = item.Enabled, org_remark = item.Remark }
                };
                List<org_treedata> listTreeData_Children = new List<org_treedata>(); //存储所有下级节点数据
                //递归下级公司
                var listComany_Children_Temp = listComany.FindAll(p => p.ParentCode == item.CompanyCode);//递归获取子节点
                if (listComany_Children_Temp.Count > 0)
                {
                    listTreeData_Children = GetTreeData_ChildrenComany(listComany, listComany_Children_Temp, listDepartment);
                }
                //下级部门
                var listDepartment_Current = listDepartment.FindAll(p => p.CompanyCode == item.CompanyCode && p.ParentCode == "0");//获取当前公司下面的所有一级部门
                if (listDepartment_Current.Count > 0)
                {
                    listTreeData_Children.AddRange(GetTreeData_Department(listDepartment, listDepartment_Current));
                }
                tempdata.children = listTreeData_Children;
                listResult.Add(tempdata);
            }
            return listResult;
        }
        private List<org_treedata> GetTreeData_Department(List<Base_Department> listDepartment, List<Base_Department> listDepartment_Current)
        {
            List<org_treedata> listResult = new List<org_treedata>();
            foreach (var item in listDepartment_Current)
            {
                org_treedata tempdata = new org_treedata()
                {
                    id = item.DepartmentCode,
                    text = item.DepartmentName,
                    iconCls = "icon-standard-note",
                    state = "open",
                    children = new List<org_treedata>(),
                    attributes = new org_attributes() { org_type = 9, org_code = item.DepartmentCode, org_pcode = item.ParentCode, org_fcode = item.CompanyCode, org_sort = item.Sort, org_enabled = item.Enabled, org_remark = item.Remark }
                };
                var listDepartment_Children_Temp = listDepartment.FindAll(p => p.ParentCode == item.DepartmentCode);//递归获取子节点
                if (listDepartment_Children_Temp.Count > 0)
                {
                    tempdata.children = GetTreeData_ChildrenDepartment(listDepartment, listDepartment_Children_Temp);
                }
                listResult.Add(tempdata);
            }
            return listResult;
        }
        private List<org_treedata> GetTreeData_ChildrenDepartment(List<Base_Department> listDepartment, List<Base_Department> listDepartment_Children)
        {
            List<org_treedata> listTreeData_Children = new List<org_treedata>();
            foreach (var item in listDepartment_Children)
            {
                org_treedata tempdata = new org_treedata()
                {
                    id = item.DepartmentCode,
                    text = item.DepartmentName,
                    iconCls = "icon-standard-note",
                    state = "open",
                    children = new List<org_treedata>(),
                    attributes = new org_attributes() { org_type = 9, org_code = item.DepartmentCode, org_pcode = item.ParentCode, org_fcode = item.CompanyCode, org_sort = item.Sort, org_enabled = item.Enabled, org_remark = item.Remark }
                };
                var listDepartment_Children_Temp = listDepartment.FindAll(p => p.ParentCode == item.DepartmentCode);//递归获取子节点
                if (listDepartment_Children_Temp.Count > 0)
                {
                    tempdata.children = GetTreeData_ChildrenDepartment(listDepartment, listDepartment_Children_Temp);
                }
                listTreeData_Children.Add(tempdata);
            }
            return listTreeData_Children;
        }
        #endregion





        /// <summary>
        /// 新增组织机构，可能是公司或者部门
        /// </summary>
        /// <param name="obj">JObject参数</param>
        /// <param name="addby">操作人</param>
        /// <returns>CommandResult</returns>
        public CommandResult AddOrg(JObject obj, string addby)
        {
            this.CommandResult.Set(true, "保存成功");
            var Select_OP = obj.Value<int>("Select_OP");//1 2 是分部  3 4是部门
            var OrgCode = obj.Value<string>("OrgCode");
            var ParentOrgCode = obj.Value<string>("ParentOrgCode");
            var OrgCompanyCode = obj.Value<string>("OrgCompanyCode");
            var OrgName = obj.Value<string>("OrgName");
            var OrgType = obj.Value<int>("OrgType");
            var OrgSort = obj.Value<int>("OrgSort");
            var OrgEnabled = obj.Value<int>("OrgEnabled");
            var OrgRemark = obj.Value<string>("OrgRemark");
            string sql1 = "INSERT INTO [dbo].[Base_Company] ([CompanyCode],[ParentCode],[CompanyName],[ShortName],[CompanyType],[Sort],[Enabled],[Remark],[AddBy]) ";
            string sql2 = "INSERT INTO [dbo].[Base_Department] ([DepartmentCode] ,[ParentCode] ,[DepartmentName] ,[ShortName] ,[CompanyCode] ,[Sort] ,[Enabled] ,[Remark] ,[AddBy]) ";
            string sql = "";
            if (Select_OP == 1 || Select_OP == 2)
            {
                sql += sql1 + string.Format("VALUES('{0}','{1}','{2}','{3}',{4},{5},{6},'{7}','{8}')", OrgCode, ParentOrgCode, OrgName, OrgName, OrgType, OrgSort, OrgEnabled, OrgRemark, addby);
            }
            else if (Select_OP == 3 || Select_OP == 4)
            {
                sql += sql2 + string.Format("VALUES('{0}','{1}','{2}','{3}','{4}',{5},{6},'{7}','{8}')", OrgCode, ParentOrgCode, OrgName, OrgName, OrgCompanyCode, OrgSort, OrgEnabled, OrgRemark, addby);
            }
            else
            {
                this.CommandResult.Set(false, "保存模式有误，Select_OP不是1/2/3/4任何一种");
            }
            int intResult = this.ExecuteNonQuery_Fish(sql);
            if (intResult == 0)
            {
                this.CommandResult.Set(false, "保存失败");
            }
            return this.CommandResult;
        }

        /// <summary>
        /// 编辑组织机构，可能是公司或者部门
        /// </summary>
        /// <param name="obj">JObject参数</param>
        /// <param name="addby">操作人</param>
        /// <returns>CommandResult</returns>
        public CommandResult EditOrg(JObject obj, string editby)
        {
            this.CommandResult.Set(true, "保存成功");
            //var Select_OP = obj.Value<int>("Select_OP");//1 2 是分部  3 4是部门
            var OrgCode = obj.Value<string>("OrgCode");
            //var ParentOrgCode = obj.Value<string>("ParentOrgCode");
            var OrgName = obj.Value<string>("OrgName");
            var OrgType = obj.Value<int>("OrgType");
            var OrgSort = obj.Value<int>("OrgSort");
            var OrgEnabled = obj.Value<int>("OrgEnabled");
            var OrgRemark = obj.Value<string>("OrgRemark");

            string sql = string.Format("UPDATE [dbo].[Base_Company] SET [CompanyName] ='{0}',[ShortName] ='{1}',[Sort] ={2},[Enabled] = {3},[Remark] ='{4}',[EditBy] ='{5}',[EditOn] =GETDATE() WHERE [CompanyCode] ='{6}'",
                OrgName, OrgName, OrgSort, OrgEnabled, OrgRemark, editby, OrgCode);
            if (OrgType == 9)
            {
                sql = string.Format("UPDATE [dbo].[Base_Department] SET [DepartmentName] ='{0}',[ShortName] ='{1}',[Sort] ={2},[Enabled] = {3},[Remark] ='{4}',[EditBy] ='{5}',[EditOn] =GETDATE() WHERE [DepartmentCode] ='{6}'",
                    OrgName, OrgName, OrgSort, OrgEnabled, OrgRemark, editby, OrgCode);
            }

            int intResult = this.ExecuteNonQuery_Fish(sql);
            if (intResult == 0)
            {
                this.CommandResult.Set(false, "保存失败");
            }
            return this.CommandResult;

        }

        /// <summary>
        /// 删除一个组织机构（公司或部门）
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public CommandResult DeleteOrg(JObject obj)
        {
            this.CommandResult.Set(true, "删除成功");
            var OrgCode = obj.Value<string>("OrgCode");
            var OrgName = obj.Value<string>("OrgName");
            var OrgType = obj.Value<int>("OrgType");//值为1 的时候 是不能删除的，必须有根节点
            //--删除所有选中的公司和所有下级公司的所有部门数据
            string sql1 = "DELETE FROM Base_Department WHERE CompanyCode IN (select CompanyCode from [dbo].[fn_GetChildrenCompanyCode]('" + OrgCode + "'))";
            //--删除所有选中公司和所有下级公司数据
            string sql2 = "DELETE FROM Base_Company WHERE CompanyCode IN (select CompanyCode from [dbo].[fn_GetChildrenCompanyCode]('" + OrgCode + "'))";
            //删除部门和子部门
            string sql3 = "DELETE FROM Base_Department WHERE DepartmentCode IN (select DepartmentCode from [dbo].[fn_GetChildrenDepartmentCode]('" + OrgCode + "'))";

            bool exists = Base_UserService.Instance.Exists_Fish(" and CompanyCode='" + OrgCode + "' or DepartmentCode='" + OrgCode + "'");
            if (exists)
            {
                this.CommandResult.Set(false, "组织机构已经被用户使用，无法删除");
                return this.CommandResult;
            }

            db.UseTransaction(true);
            if (OrgType == 2)//删除公司
            {
                db.Sql(sql1).Execute();
                db.Sql(sql2).Execute();
            }
            if (OrgType == 9)//删除部门
            {
                db.Sql(sql3).Execute();
            }
            db.Commit();

            return this.CommandResult;

        }








    }
}
