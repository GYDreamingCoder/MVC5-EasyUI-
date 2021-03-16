using System;

namespace JTS.Core
{
    /// <summary>
    /// 摘要：保存数据后返回的结果对象。
    /// <para>用来封装执行数据的返回结果，主要是ResultID和ResultMsg信息，不包括其他结果表信息。</para>
    /// <para>说明：数据库返回结果的字段必须是ResultID和ResultMsg，命名对不上的话将报错。</para>
    /// </summary>
    [Serializable]
    public class SaveResult : CommandResult
    {
        public SaveResult() { }

        private object _CustomObject = null;

        /// <summary>
        /// 获取或设置自定义对象。
        /// </summary>
        public object CustomObject
        {
            get { return this._CustomObject; }
            set { this._CustomObject = value; }
        }

        /// <summary>
        /// 创建默认的结果对象。
        /// </summary>
        /// <returns></returns>
        public static SaveResult CreateDefault()
        {
            SaveResult result = new SaveResult() { ResultID = 0, ResultMsg = "保存成功！" };
            return result;
        }

        /// <summary>
        /// 当保存失败时记录系统异常的内容
        /// </summary>
        /// <param name="ex">Exception</param>
        public void SetException(Exception ex)
        {
            ResultID = -1;
            ResultMsg += "\n\n" + ex.Message;
        }

    }
}
