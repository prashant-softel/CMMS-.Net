using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CMMSAPIs.Helper
{
    public static partial class ExtensionMethods
    {
        public static List<T> MapTo<T>(this DataTable table)
              where T : class, new()
        {
            var colErr = "";
            List<Tuple<object, PropertyInfo>> map =
                new List<Tuple<object, PropertyInfo>>();

            foreach (PropertyInfo pi in typeof(T).GetProperties())
            {
                ColumnAttribute col = (ColumnAttribute)
                    Attribute.GetCustomAttribute(pi, typeof(ColumnAttribute));
                if (col == null || col.Name.IsNullOrEmpty())
                {
                    if (table.Columns.Contains(pi.Name))
                    {
                        map.Add(new Tuple<object, PropertyInfo>(
                        table.Columns[pi.Name], pi));
                    }
                }
                else
                {
                    if (!col.Name.Contains(".") && table.Columns.Contains(col.Name))
                    {
                        map.Add(new Tuple<object, PropertyInfo>(
                            table.Columns[col.Name], pi));
                    }
                    else
                    {
                        map.Add(new Tuple<object, PropertyInfo>(
                          col.Name.Split('.').ToList().Last(), pi));
                    }
                }
            }
            try
            {
                List<T> list = new List<T>();
                foreach (DataRow row in table.Rows)
                {
                    if (row == null)
                    {
                        list.Add(null);
                        continue;
                    }
                    T item = new T();
                    foreach (Tuple<object, PropertyInfo> pair in map)
                    {
                        colErr = pair.Value1.ToString();
                        object value;
                        if (pair.Value1.GetType() == typeof(DataColumn))
                        {
                            value = row[(DataColumn)pair.Value1];
                            if (value is DBNull) value = null;
                            pair.Value2.SetValue(item, value, null);
                        }
                        else
                        {
                            pair.Value2.SetValue(item, mapToObject(row, pair.Value2, pair.Value1.ToString()), null); ;

                        }


                    }
                    list.Add(item);
                }
                return list;
            }
            catch (Exception ex)
            {

                throw new Exception($"Error At Col:{colErr}{Environment.NewLine}{ex.Message}", ex);
            }


        }

        public static void ConvertColumnType(this DataTable dt, string columnName, Type newType)
        {
            using (DataColumn dc = new DataColumn(columnName + "_new", newType))
            {
                // Add the new column which has the new type, and move it to the ordinal of the old column
                int ordinal = dt.Columns[columnName].Ordinal;
                dt.Columns.Add(dc);
                dc.SetOrdinal(ordinal);

                // Get and convert the values of the old column, and insert them into the new
                foreach (DataRow dr in dt.Rows)
                    dr[dc.ColumnName] = Convert.ChangeType(dr[columnName], newType);

                // Remove the old column
                dt.Columns.Remove(columnName);

                // Give the new column the old column's name
                dc.ColumnName = columnName;
            }
        }

        public static List<T> GetColumn<T>(this DataTable dt, string column)
        {
            return (from row in dt.AsEnumerable() select row.Field<T>(column)).ToList();
        }

        private static void Each<T>(this IEnumerable<T> els, Action<T, int> a)
        {
            int i = 0;
            foreach (T e in els)
            {
                a(e, i++);
            }
        }

        public static Dictionary<TKey, TValue> Merge<TKey, TValue>(this Dictionary<TKey, TValue> dict, IEnumerable<TKey> keys, IEnumerable<TValue> values)
        {
            var dic = new Dictionary<TKey, TValue>();

            keys.Each((x, i) =>
            {
                dic.Add(x, values.ElementAt(i));
            });

            return dic;
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value) || value.Trim().Length == 0;
        }
        private static object mapToObject(DataRow dataRow, PropertyInfo value2, string key)
        {
            if (dataRow == null) return null;

            DataTable table = dataRow.Table.Clone();
            table.ImportRow(dataRow);
            var dtlFieldNames = table.Columns.Cast<DataColumn>().
             Select(item => new
             {
                 Name = item.ColumnName,
                 Type = item.DataType
             }).ToList();

            List<Tuple<DataColumn, PropertyInfo>> map =
                new List<Tuple<DataColumn, PropertyInfo>>();

            foreach (PropertyInfo pi in value2.PropertyType.GetProperties())
            {
                ColumnAttribute col = (ColumnAttribute)
                    Attribute.GetCustomAttribute(pi, typeof(ColumnAttribute));
                if (col == null || col.Name.IsNullOrEmpty())
                {
                    if (pi.Name.ToLower() == "id")
                    {
                        if (dtlFieldNames.Any(s => s.Name.ToLower() == key.ToLower()))
                        {
                            map.Add(new Tuple<DataColumn, PropertyInfo>(
                            table.Columns[key], pi));
                        }
                    }
                    else if (dtlFieldNames.Any(s => s.Name.ToLower() == pi.Name.ToLower()))
                    {
                        map.Add(new Tuple<DataColumn, PropertyInfo>(
                        table.Columns[pi.Name], pi));
                    }
                }
                else
                {
                    if (dtlFieldNames.Any(s => s.Name.ToLower() == col.Name.ToLower()))
                    {
                        map.Add(new Tuple<DataColumn, PropertyInfo>(
                            table.Columns[col.Name], pi));
                    }
                    else
                    {
                        map.Add(new Tuple<DataColumn, PropertyInfo>(
                           null, pi));
                    }
                }
            }
            try
            {
                var row = table.Rows[0];

                if (value2.PropertyType.IsClass)
                {
                    object instance = Activator.CreateInstance(value2.PropertyType);
                    foreach (Tuple<DataColumn, PropertyInfo> pair in map)
                    {
                        object value;
                        if (pair.Value1 != null)
                        {
                            value = row[pair.Value1];
                            if (value is DBNull) value = null;
                            pair.Value2.SetValue(instance, value, null);
                        }




                    }
                    return instance;

                }

                return null;


            }
            catch (Exception ex)
            {

                throw ex; //new Exception(ex.Message,ex.InnerException);
            }
            finally
            {
                table.Dispose();
            }




        }

        public static int ToInt(this object value)
        {
            try
            {
                if (value.IsNull())
                    value = 0;
                return Convert.ToInt32(value);
            }
            catch (Exception)
            {

                return 0;
            }

        }
        public static bool IsNull(this object obj)
        {
            return (Convert.IsDBNull(obj) || obj == null);
        }
        public sealed class Tuple<T1, T2>
        {
            public Tuple() { }
            public Tuple(T1 value1, T2 value2) { Value1 = value1; Value2 = value2; }
            public T1 Value1 { get; set; }
            public T2 Value2 { get; set; }
        }
     
    }
}
