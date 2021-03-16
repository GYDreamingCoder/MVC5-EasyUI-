/*************************************************************************
 * 文件名称 ：ParamSP.cs                          
 * 描述说明 ：存储过程SP参数构建类
 *
**************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using FluentData;

namespace JTS.Core
{
    /// <summary>
    /// 存储过程SP参数构建类
    /// </summary>
    public class ParamSP
    {
        protected ParamSPData data;

        public ParamSP Name(string name)
        {
            data.Name = name;
            return this;
        }

        public ParamSP Parameter(string name, object value)
        {
            data.Parameter.Add(name, value);
            return this;
        }

        public ParamSP ParameterOut(string name, DataTypes type)
        {
            data.ParameterOut.Add(name, type);
            return this;
        }

        public ParamSP()
        {
            data = new ParamSPData();
        }

        public static ParamSP Instance()
        {
            return new ParamSP();
        }

        public ParamSPData GetData()
        {
            return data;
        }
    }


    /// <summary>
    ///存储过程 SP参数数据
    /// </summary>
    public class ParamSPData
    {
        public string Name { get; set; }
        public Dictionary<string, object> Parameter { get; set; }
        public Dictionary<string, DataTypes> ParameterOut { get; set; }

        public ParamSPData()
        {
            Name = string.Empty;
            Parameter = new Dictionary<string, object>();
            ParameterOut = new Dictionary<string, DataTypes>();
        }
    }

}
