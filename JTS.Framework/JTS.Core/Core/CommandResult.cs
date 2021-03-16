using System;

namespace JTS.Core
{
    /// <summary>
    /// 结果对象接口。
    /// <remarks>应用于许多方法中的返回值。</remarks>
    /// </summary>
    public interface IResult
    {
        int ResultID { get; set; }
        string ResultMsg { get; set; }
        bool Succeed { get; }
    }

    /// <summary>
    /// 摘要：命令结果对象。
    /// <para>用来封装执行数据的返回结果，主要是ResultID和ResultMsg信息，不包括其他结果表信息。</para>
    /// <para>说明：数据库返回结果的字段必须是ResultID和ResultMsg，命名对不上的话将报错。</para>
    /// </summary>
    [Serializable]
    public class CommandResult : IResult
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public CommandResult()
        {
            Set(-1, "失败");
        }

        /// <summary>
        /// 获取成功的CommandResult实例对象[成功]
        /// </summary>
        public static CommandResult Instance_Succeed
        {
            get
            {
                return new CommandResult() { ResultID = 0, ResultMsg = "成功" };
            }
        }

        /// <summary>
        /// 获取失败的CommandResult实例对象
        /// </summary>
        public static CommandResult Instance_Error
        {
            get
            {
                return new CommandResult() { ResultID = -1, ResultMsg = "失败" };
            }
        }
        /// <summary>
        /// 设置CommandResult对象
        /// </summary>
        /// <param name="resultID">resultID</param>
        /// <param name="resultMsg">resultMsg</param>
        /// <returns>CommandResult</returns>
        public CommandResult Set(int resultID, string resultMsg)
        {
            this.ResultID = resultID;
            this.ResultMsg = resultMsg;
            return this;
        }
        /// <summary>
        /// 设置CommandResult对象
        /// </summary>
        /// <param name="success">是否成功</param>
        /// <param name="resultMsg">resultMsg消息</param>
        /// <returns>CommandResult</returns>
        public CommandResult Set(bool success, string resultMsg)
        {
            this.ResultID = success == true ? 0 : -1;
            this.ResultMsg = resultMsg;
            return this;
        }
        private int _ResultID = -1;
        /// <summary>
        /// 获取或设置存储过程返回的ResultID。
        /// </summary>
        public int ResultID
        {
            get { return this._ResultID; }
            set { this._ResultID = value; }
        }

        private string _ResultMsg = string.Empty;

        /// <summary>
        /// 获取或设置执行数据返回的ResultMsg。
        /// </summary>
        public string ResultMsg
        {
            get { return this._ResultMsg; }
            set { this._ResultMsg = value; }
        }

        /// <summary>
        /// 获取执行数据是否执行成功。成功则返回true；否则返回false，
        /// </summary>
        public bool Succeed
        {
            get
            {
                return this._ResultID == 0;
            }
        }

        /// <summary>
        /// 获取或设置结果数据
        /// </summary>
        public object ResultData
        {
            get;
            set;
        }

        /// <summary>
        /// 获取是否成功 true或false  同Succeed
        /// </summary>
        public bool s
        {
            get
            {
                return this.Succeed;
            }
        }
        /// <summary>
        /// 获取消息，同ResultMsg
        /// </summary>
        public string emsg
        {
            get
            {
                return this.ResultMsg;
            }
        }

    }
}
