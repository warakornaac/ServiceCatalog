using ServiceCatalog.Library;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;

namespace ServiceCatalog.Library
{
    public static class ConvertExtension
    {
        public static T ToGenericFirst<T>(this DataTable datatable) where T : new()
        {
            return (from p in datatable.AsEnumerable() select ConvertToList<T>(p)).FirstOrDefault();
        }

        public static List<T> ToGenericList<T>(this DataTable datatable) where T : new()
        {
            return (from p in datatable.AsEnumerable() select ConvertToList<T>(p)).ToList();
        }

        private static T ConvertToList<T>(DataRow row) where T : new()
        {
            var entity = new T();
            var type = entity.GetType();

            foreach (var propertyInfo in type.GetProperties())
            {
                var attribute = propertyInfo.GetCustomAttributes(true).FirstOrDefault() as ColumnNameAttribute;
                object objValue = null;

                try
                {
                    switch (propertyInfo.PropertyType.Name.ToLower())
                    {
                        case "datetime":
                            if (attribute != null) objValue = Convert.ToDateTime(row[attribute.Key]);
                            break;
                        case "boolean":
                            objValue = attribute != null && Convert.ToBoolean(row[attribute.Key]);
                            break;
                        case "int32":
                            if (attribute != null) objValue = Convert.ToInt32(row[attribute.Key]);
                            break;
                        case "double":
                            if (attribute != null) objValue = Convert.ToDouble(row[attribute.Key]);
                            break;
                        case "decimal":
                            if (attribute != null) objValue = Convert.ToDecimal(row[attribute.Key]);
                            break;
                        case "byte[]":
                            if (attribute != null) objValue = row[attribute.Key];
                            break;
                        default:
                            if (attribute != null) objValue = row[attribute.Key].ToString();
                            break;
                    }
                }
                catch { }

                if (attribute != null && objValue != DBNull.Value)
                    propertyInfo.SetValue(entity, objValue, null);
            }

            return entity;
        }

        public static T ConvertTo<T>(object entity) where T : new()
        {
            var convertProperties = TypeDescriptor.GetProperties(typeof(T)).Cast<PropertyDescriptor>();
            var entityProperties = TypeDescriptor.GetProperties(entity).Cast<PropertyDescriptor>();

            var convert = new T();

            foreach (var entityProperty in entityProperties)
            {
                var property = entityProperty;
                var convertProperty = convertProperties.FirstOrDefault(prop => prop.Name.Replace("_", "").ToLower() == property.Name.ToLower());
                if (convertProperty != null)
                {
                    if (entityProperty.PropertyType == typeof(DateTime))
                        convertProperty.SetValue(convert, Convert.ChangeType(Convert.ToDateTime(entityProperty.GetValue(entity)).ToEpochTime(), convertProperty.PropertyType));
                    else
                        convertProperty.SetValue(convert, Convert.ChangeType(entityProperty.GetValue(entity), convertProperty.PropertyType));
                }
            }

            return convert;
        }

        public static List<T> ConvertToList<T>(object entities) where T : new()
        {
            var convertProperties = TypeDescriptor.GetProperties(typeof(T)).Cast<PropertyDescriptor>();
            var entityProperties = TypeDescriptor.GetProperties(entities).Cast<PropertyDescriptor>();

            var result = new List<T>();

            foreach (var entity in (IEnumerable)entities)
            {
                var convert = new T();
                foreach (var entityProperty in TypeDescriptor.GetProperties(entity).Cast<PropertyDescriptor>())
                {
                    var property = entityProperty;
                    var convertProperty = convertProperties.FirstOrDefault(prop => prop.Name.Replace("_", "").ToLower() == property.Name.ToLower());
                    if (convertProperty != null)
                    {
                        if (entityProperty.PropertyType == typeof(DateTime))
                            convertProperty.SetValue(convert, Convert.ChangeType(Convert.ToDateTime(entityProperty.GetValue(entity)).ToEpochTime(), convertProperty.PropertyType));
                        else
                            convertProperty.SetValue(convert, Convert.ChangeType(entityProperty.GetValue(entity), convertProperty.PropertyType));
                    }
                }

                result.Add(convert);
            }

            return result;
        }

        public static Int32 ToInt32(this string str)
        {
            Int32 result;
            if (!Int32.TryParse(str, out result))
            {
                return 0;
            }
            return result;
        }

        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName && dr[column.ColumnName] != DBNull.Value)
                        try
                        {
                            pro.SetValue(obj, dr[column.ColumnName], null);
                            break;
                        }
                        catch (Exception)
                        { }
                    else
                        continue;
                }
            }
            return obj;
        }
    }
}