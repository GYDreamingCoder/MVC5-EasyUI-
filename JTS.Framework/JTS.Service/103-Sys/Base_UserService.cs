//------------------------------------------------------------------------------
// <自动生成>
//   此代码由代码生成器生成 autocode 2015-11-05 22:37:54
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
    /// 【系统用户】服务类
    /// </summary>
    public class Base_UserService : BaseService<Base_User>
    {
        /// <summary>
        /// 【系统用户】服务实例【单例模式】
        /// </summary>
        private static readonly Base_UserService _Instance = new Base_UserService();

        /// <summary>
        /// 获取【系统用户】服务实例【单例模式】
        /// </summary>
        public static Base_UserService Instance
        {
            get
            {
                return _Instance;
            }
        }

        /// <summary>
        /// 系统登录服务
        /// </summary>
        /// <param name="userCode">登录账号</param>
        /// <param name="password">登录密码，已加密</param>
        /// <param name="ip">登录ip地址</param>
        /// <param name="city">登录所在城市</param>
        /// <returns>返回CommandResult对象</returns>
        public CommandResult Login(string userCode, string password, string ip, string city)
        {
            //执行存储过程登录
            var paramInput = new
            {
                UserCode = userCode,
                Password = password,
                LoginIP = ip,
                LoginCity = city,
            };
            var loginResult = this.SP_Fish("Base_User_Login", paramInput);
            //如果登录失败，返回消息
            //if (loginResult.Succeed == false)
            //{
            //    return loginResult;
            //}
 

            //返回登陆成功
            return loginResult;
        }

        /// <summary>
        /// 获取默认的用户设置
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetDefaultUserSetttins()
        {
            //navigation：accordion/tree/menubutton
            var defaults = new Dictionary<string, object>();
            defaults.Add("theme", "default");
            defaults.Add("navigation", "accordion");
            defaults.Add("gridrows", "20");
            return defaults;
        }

        /// <summary>
        /// 获取当前用户的设置
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetCurrentUserSettings()
        {
            var result = new Dictionary<string, object>();
            var UserId = this.CurrentBaseLoginer.UserId;
            //var config = db.Sql("select ConfigJSON from sys_user where UserCode=@0", UserCode).QuerySingle<string>();
            var userSettings = Base_UserSettingService.Instance.GetList_Fish(" and UserId =" + UserId.ToString(),true);

            foreach (var item in userSettings)
                result.Add(item.SettingCode, item.SettingValue);

            var defaults = GetDefaultUserSetttins();

            foreach (var item in defaults)
                if (!result.ContainsKey(item.Key)) result.Add(item.Key, item.Value);

            return result;
        }

        /// <summary>
        /// 保存用户个性设置数据
        /// </summary>
        /// <param name="settings">JObject settings</param>
        public void SaveCurrentUserSettings(JObject settings)
        {
            var UserId = this.CurrentBaseLoginer.UserId;
            foreach (JProperty item in settings.Children())
            {
                var result = db.Update("Base_UserSetting")
                    .Column("SettingValue", item.Value.ToString())
                    .Where("UserId", UserId)
                    .Where("SettingCode", item.Name)
                    .Execute();

                //var paramU = ParamUpdate.Instance().Update("Base_UserSetting")
                //         .Column("SettingValue", item.Value.ToString())
                //         .AndWhere("UserId", UserId)
                //         .AndWhere("SettingCode", item.Name);
                //var result = this.Update(paramU);

                if (result <= 0)
                {
                    var model = new Base_UserSetting();
                    model.UserId = UserId;
                    model.SettingCode = item.Name;
                    model.SettingValue = item.Value.ToString();
                    //db.Insert<Base_UserSetting>("Base_UserSetting", model).AutoMap(x => x.Id).AutoMap(x => x.tempid).Execute();
                    db.Insert("Base_UserSetting").Column("UserId", model.UserId)
                        .Column("SettingCode", model.SettingCode)
                        .Column("SettingName", model.SettingCode)
                        .Column("SettingValue", model.SettingValue)
                        .ExecuteReturnLastId<int>();
                }
            }
        }

        /// <summary>
        /// 修改自己的密码
        /// </summary>
        /// <param name="newPassword">新密码，未加密</param>
        /// <returns>返回CommandResult</returns>
        public CommandResult ModifySelfPassword(string newPassword,int userid)
        {
            CommandResult commandResult = new CommandResult();
            var paramUpdate = ParamUpdate.Instance().Update("Base_User")
                .Column("Password", Md5Util.MD5(newPassword))
                .AndWhere("UserId", userid);
            int n = this.Update(paramUpdate);
            commandResult.ResultID = n > 0 ? 0 : -1;
            commandResult.ResultMsg = n > 0 ? "密码修改成功" : "密码修改失败";
            LogHelper.Write("修改个人密码。用户：" + this.CurrentBaseLoginer.UserName + "。结果：" + commandResult.ResultMsg);
            return commandResult;
        }

        /// <summary>
        /// 重置某个用户的密码,新密码默认是：123456
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns>返回CommandResult</returns>
        public CommandResult ResetUserPassword(int userId, string userName,string userCode)
        {
            CommandResult cmdResult = CommandResult.Instance_Succeed;
            cmdResult.ResultMsg = "密码重置成功";
            string newPwd = Md5Util.MD5("123456");
            var par = ParamUpdate.Instance().Update("Base_User")
                .Column("Password", newPwd)
                .Column("LastChangePassword", DateTime.Now)
                .AndWhere("UserId", userId);
            var result = Base_UserService.Instance.Update(par);
            if (result == 0) { cmdResult.Set(false, "密码重置失败"); }
            string logmsg = string.Format("用户密码重置。结果：{3}。用户：{0}-({1})，登录账号:{2}", userName, userId, userCode, cmdResult.ResultMsg);
            LogHelper.Write(logmsg);
            return cmdResult;
        }

        /// <summary>
        /// 审核用户
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="op">审核人</param>
        /// <returns>返回CommandResult</returns>
        public CommandResult AuditUser(int userId, string op)
        {
            CommandResult cmdResult = CommandResult.Instance_Succeed;
            cmdResult.ResultMsg = "审核成功"; 
            var par = ParamUpdate.Instance().Update("Base_User")
                .Column("IsAudit", 1)
                .Column("AuditBy", op)
                .Column("AuditTime", DateTime.Now)
                .AndWhere("UserId", userId);
            var result = Base_UserService.Instance.Update(par);
            if (result == 0) { cmdResult.Set(false, "审核失败"); }
            string logmsg = string.Format("用户审核成功。结果：{0}。用户编号：{1}", cmdResult.ResultMsg, userId);
            LogHelper.Write(logmsg);
            return cmdResult;
        }

    }
}
