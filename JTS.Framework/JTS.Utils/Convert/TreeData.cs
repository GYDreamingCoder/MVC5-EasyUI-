using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;

namespace JTS.Utils
{
    public partial class ConvertUtil
    {
        /// <summary>
        /// 将List数据转换成TreeData
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">list数据源</param>
        /// <param name="ID">id列名</param>
        /// <param name="PID">父id列名</param>
        /// <returns></returns>
        public static List<dynamic> ListToTreeData<T>(List<T> source, string ID, string PID) where T : class, new()
        {
            Action<List<dynamic>, T, dynamic> AddItem = (parent, item, Recursive) =>
            {
                var childrens = new List<dynamic>();
                var thisitem = GenericUtil.GetDictionaryValues(item);

                source.FindAll(o => GenericUtil.GetValue(item, ID).Equals(GenericUtil.GetValue(o, PID)))
                      .ForEach(subitem => { Recursive(childrens, subitem, Recursive); });

                thisitem.Add("children", childrens);
                parent.Add(thisitem);
            };

            var target = new List<dynamic>();
            source.FindAll(m => { return !source.Exists(n => GenericUtil.GetValue(n, ID).Equals(GenericUtil.GetValue(m, PID))); })
                  .ForEach(item => AddItem(target, item, AddItem));

            return target;
        }

        public static List<T> TreeDataToList<T>(List<dynamic> source)
        {
            Action<List<dynamic>, List<T>, dynamic> AddItem = (mysource, mytarget, Recursive) =>
            {
                foreach (var oldrow in mysource)
                {
                    var newrow = GenericUtil.CreateNew<T>();
                    var dictionary = (IDictionary<string, object>)GenericUtil.GetDictionaryValues(oldrow);

                    var childern = dictionary["childern"] as List<dynamic>;
                    if (childern.Count > 0) Recursive(mysource, mytarget, Recursive);

                    foreach (var property in dictionary)
                        if (property.Key != "children")
                            GenericUtil.SetValue(newrow, property.Key, property.Value);

                    mytarget.Add(newrow);
                }
            };

            var target = new List<T>();
            AddItem(source, target, AddItem);

            return target;
        }
    }
}
