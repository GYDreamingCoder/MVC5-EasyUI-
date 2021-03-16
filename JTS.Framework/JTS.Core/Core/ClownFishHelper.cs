/*************************************************************************
 * 文件名称 ：ClownFishHelper.cs                          
 * 描述说明 ：ClownFish数据库访问组件初始化类
 * 
 **************************************************************************/

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;
using ClownFish;

namespace JTS.Core
{
    /// <summary>
    /// ClownFish数据库访问组件初始化类
    /// </summary>
    public static class ClownFishHelper
    {
        /// <summary>
        /// 初始化ClownFish数据库访问组件[支持多个数据库连接]
        /// </summary>
        public static void Init(string conn)
        {
            try
            {
                /*初始化ClownFish数据库连接。注册SQLSERVER数据库连接字符串*/

                // 设置配置参数：当成功执行数据库操作后，如果有输出参数，则自动获取返回值并赋值到实体对象的对应数据成员中。
                DbContextDefaultSetting.AutoRetrieveOutputValues = true;
                // 注册编译失败事件，用于检查在编译实体加载器时有没有失败。
                BuildManager.OnBuildException += new BuildExceptionHandler(BuildManager_OnBuildException);

                // 启动自动编译数据实体加载器的工作模式。
                // 编译的触发条件：请求实体加载器超过2000次，或者，等待编译的类型数量超过100次
                BuildManager.StartAutoCompile(() => BuildManager.RequestCount > 2000 || BuildManager.WaitTypesCount > 100);

                // 注册sqlserver数据库连接字符串。
                //     注册数据库的连接信息。
                //     如果程序要访问二种不同类型的数据库，如：SQLSERVER和MySql，那么至少需要调用本方法二次。
                //     每种类型的数据库如果有多个“数据库的连接”，可以在构造方法中指定。这里的连接字符串只是做为默认的连接字符串
                //conn = conn == null ? "conn_clownfish" : conn;
                ConnectionStringSettings SqlConnectionStringSettings = ConfigurationManager.ConnectionStrings[conn];
                string providerName = SqlConnectionStringSettings.ProviderName; 
                if (providerName == "SqlServer")
                {
                    providerName = "System.Data.SqlClient";
                }
                DbContext.RegisterDbConnectionInfo("sqlserver", providerName, "@", SqlConnectionStringSettings.ConnectionString);

                //启用数据监控。
                /*
                 为了能保证监视能正常工作，还需要确保网站bin目录下存在ClownFishProfilerLib.dll文件， 此文件订阅了前面所说的事件，并通过Remoting给ClownFishSQLProfiler.exe发送应用程序访问数据库的所有操作细节。 当ClownFishProfilerLib.dll不存在时，ClownFishSQLProfiler.exe不会收到任何通知，但并不影响网站正常运行。
                 */
                //Profiler.ApplicationName = SqlConnectionStringSettings.ConnectionString;
                Profiler.TryStartClownFishProfiler();
            }
            catch (Exception ex)
            {
                //  ex = new Exception("InitializationClownFishDbContext初始化发生异常。消息：" + ex.Message);
                throw ex;
            }
        }
        /// <summary>
        /// 注册编译失败事件处理。
        /// </summary>
        /// <param name="ex"></param>
        static void BuildManager_OnBuildException(Exception ex)
        {
            CompileException ce = ex as CompileException;
            if (ce != null)
            {
                //throw new Exception("编译数据实体加载器代码时引发的异常。详细信息：" + ce.GetDetailMessages());
                SafeLogException(ce.GetDetailMessages());
            }
            else
            {
                // 未知的异常类型
                //throw ex;
                // 未知的异常类型
                SafeLogException(ex.ToString());
            }
        }
        public static void SafeLogException(string message)
        {
            try
            {
                string logfilePath = Path.Combine(HttpRuntime.AppDomainAppPath, @"finshlog\ErrorLog.txt");

                File.AppendAllText(logfilePath, "=========================================\r\n" + message, System.Text.Encoding.UTF8);
            }
            catch { }
        }
    }
}
