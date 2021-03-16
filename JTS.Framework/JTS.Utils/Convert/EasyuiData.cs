using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JTS.Utils
{

    /// <summary>
    /// easyui datagrid数据列对象
    /// </summary>
    public class JColumn
    {
        //public int rowspan { get; set; }
        //public int colspan { get; set; }
        //public bool checkbox { get; set; }
        public string field { get; set; }
        public string title { get; set; }
        public int width { get; set; }
        public string align { get; set; }

        public string ConvertToJson()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            //if (rowspan != null)
            //{
            //    sb.AppendFormat("\"rowspan\":{0},", rowspan);
            //}
            //if (colspan != null)
            //{
            //    sb.AppendFormat("\"colspan\":{0},", colspan);
            //}
            //if (checkbox != null)
            //{
            //    sb.AppendFormat("\"checkbox\":{0},", checkbox);
            //}
            sb.AppendFormat("\"field\":\"{0}\",", field);
            sb.AppendFormat("\"width\":{0},", width);
            sb.AppendFormat("\"align\":\"{0}\",", align);
            sb.AppendFormat("\"title\":\"{0}\",", title);
            sb.Remove(sb.Length - 1, 1);
            sb.Append("}");
            String str = sb.ToString().ToLower();
            return str;
        }
    }

    /// <summary>
    /// easyui datagrid 对象
    /// </summary>
    public class JDataGrid
    {
        public int total { get; set; }
        public List<Dictionary<string, object>> rows { get; set; }
        public List<JColumn> columns { get; set; }

        public static List<Dictionary<string, object>> ConvertRows(IList list)
        {
            List<Dictionary<string, object>> rowsList = new List<Dictionary<string, object>>();
            if (list != null)
            {
                foreach (object obj in list)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    Type t = obj.GetType();
                    foreach (PropertyInfo pi in t.GetProperties())
                    {
                        string key = pi.Name;
                        object value = pi.GetValue(obj, null);
                        dic.Add(key, value);
                    }
                    rowsList.Add(dic);
                }
            }
            return rowsList;
        }
        public static List<Dictionary<string, object>> ConvertRows(DataTable dt)
        {
            List<Dictionary<string, object>> rowsList = new List<Dictionary<string, object>>();
            if (dt.Rows.Count > 0)
            {
                for (int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    for (int colIndex = 0; colIndex < dt.Columns.Count; colIndex++)
                    {
                        string key = dt.Columns[colIndex].ColumnName;//列名
                        object value = dt.Rows[rowIndex][colIndex];
                        if (value != System.DBNull.Value && value.ToString() == "0")
                        {
                            value = System.DBNull.Value;
                        }
                        // value == value.ToString() == "0" ? "" : value;
                        dic.Add(key, value);
                    }
                    rowsList.Add(dic);
                }
            }
            return rowsList;
        }
        public string ConvertToJson()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{{\"total\":{0},\"rows\":[", total);
            //添加数据      
            if (rows != null && rows.Count > 0)
            {
                for (int i = 0; i < rows.Count; i++)
                {
                    sb.Append("{");
                    foreach (string key in rows[i].Keys)
                    {
                        if (rows[i][key] is ValueType)
                        {
                            sb.AppendFormat("\"{0}\":{1},", key, rows[i][key]);
                        }
                        else
                        {
                            sb.AppendFormat("\"{0}\":\"{1}\",", key, rows[i][key]);
                        }
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append("}");
                    if (i != rows.Count - 1)
                    {
                        sb.Append(",");
                    }
                }
            }
            sb.Append("],");
            sb.Append("\"columns\":[[");
            //添加列      
            if (columns != null && columns.Count > 0)
            {
                for (int i = 0; i < columns.Count; i++)
                {
                    sb.Append(columns[i].ConvertToJson());
                    if (i != columns.Count - 1)
                    {
                        sb.Append(",");
                    }
                }
            }
            sb.Append("]]}");
            string str = sb.ToString();
            return str;
        }
    }
}
