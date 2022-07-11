using bsod.Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;

namespace bsod.Common.DataAnnotations
{
    public static class DataAnnotations_Extensions
    {

        public static T GetMetaDataAttribute<T>(this PropertyInfo property) where T : class
        {
            return GetMetaDataAttribute<T>(property.DeclaringType, property);
        }
        public static T GetMetaDataAttribute<T>(this Type model, PropertyInfo property) where T : class
        {
            T retVal = null;
            if (property != null)
            {
                var foundAtributes= property.GetCustomAttributes();
                foreach (var att in foundAtributes)
                {
                    if (att.IsType<T>(out retVal))
                        break;
                }
                if (retVal == null)
                {
                    var metaDataType = model.GetCustomAttributes(typeof(MetadataTypeAttribute), true).OfType<MetadataTypeAttribute>().FirstOrDefault();
                    if (metaDataType != null)
                    {
                        var metaData = ModelMetadataProviders.Current.GetMetadataForType(null, metaDataType.MetadataClassType);
                        retVal = GetMetaDataAttribute<T>(metaData.ModelType, metaData.ModelType.GetProperty(property.Name));
                    }
                }
            }
            return retVal;
        }
        /// <summary>
        /// Checkes to see if the Key Attribute is attached to the property.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static bool IsKey(this PropertyInfo property)
        {
            object metaData = property.GetMetaDataAttribute<IsKeyAttribute>();
            if(metaData== null)
            {
                metaData = property.GetMetaDataAttribute<KeyAttribute>();
            }
            return metaData != null;
        }
        /// <summary>
        /// Returnes true if the hide attribute is attached to the property.
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static bool IsHidden(this PropertyInfo prop)
        {
            try
            {
                var hidden = prop.GetCustomAttribute<HideAttribute>();
                return hidden != null;

            }
            catch (Exception)
            { }
            return false;
        }

        public static int GetOrderNumb(this PropertyInfo prop)
        {
            var attri = prop.GetCustomAttribute<DisplayAttribute>();
            if (attri != null && attri.GetOrder() != null)
            {
                return attri.Order;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// Returns the display name of the property given, if no display name given returns property name.
        /// </summary>
        /// <param name="model">Type the property derived from.</param>
        /// <param name="property">Property to get the display name from.</param>
        /// <param name="DisplayNameOnly">If false will return the property name if display name not found. If true and 
        /// display name not found returns null.</param>
        /// <returns></returns>
        public static string GetDisplayName(this Type model, PropertyInfo property, bool DisplayNameOnly = false)
        {
            string retVal = null;
            if (property != null)
            {
                DisplayAttribute attr = GetMetaDataAttribute<DisplayAttribute>(model, property);
                if (attr != null)
                {
                    retVal = attr.Name;
                }
                else
                {
                    DisplayNameAttribute dattr = GetMetaDataAttribute<DisplayNameAttribute>(model, property);
                    if (dattr != null)
                    {
                        retVal = dattr.DisplayName;
                    }
                }
            }
            return retVal ?? (DisplayNameOnly ? null : property.Name);
        }

        public static string GetDisplayName<TModel>(Expression<Func<TModel, object>> expression)
        {

            Type type = typeof(TModel);

            string propertyName = null;
            string[] properties = null;
            IEnumerable<string> propertyList;
            //unless it's a root property the expression NodeType will always be Convert
            switch (expression.Body.NodeType)
            {
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    var ue = expression.Body as UnaryExpression;
                    propertyList = (ue?.Operand).ToString().Split(".".ToCharArray()).Skip(1); //don't use the root property
                    break;
                default:
                    propertyList = expression.Body.ToString().Split(".".ToCharArray()).Skip(1);
                    break;
            }

            //the propert name is what we're after
            propertyName = propertyList.Last();
            //list of properties - the last property name
            properties = propertyList.Take(propertyList.Count() - 1).ToArray(); //grab all the parent properties

            Expression expr = null;
            foreach (string property in properties)
            {
                PropertyInfo propertyInfo = type.GetProperty(property);
                expr = Expression.Property(expr, type.GetProperty(property));
                type = propertyInfo.PropertyType;
            }

            DisplayAttribute attr;
            attr = (DisplayAttribute)type.GetProperty(propertyName).GetCustomAttributes(typeof(DisplayAttribute), true).SingleOrDefault();

            // Look for [MetadataType] attribute in type hierarchy
            // http://stackoverflow.com/questions/1910532/attribute-isdefined-doesnt-see-attributes-applied-with-metadatatype-class
            if (attr == null)
            {
                MetadataTypeAttribute metadataType = (MetadataTypeAttribute)type.GetCustomAttributes(typeof(MetadataTypeAttribute), true).FirstOrDefault();
                if (metadataType != null)
                {
                    var property = metadataType.MetadataClassType.GetProperty(propertyName);
                    if (property != null)
                    {
                        attr = (DisplayAttribute)property.GetCustomAttributes(typeof(DisplayNameAttribute), true).SingleOrDefault();
                    }
                }
            }
            return (attr != null) ? attr.Name : String.Empty;

        }

        public static string GetDisplayNames(this Type model, bool DisplayNamesOnly, string delimiter = ",")
        {
            Type type = model;
            PropertyInfo[] properties = type.GetProperties();
            string c = ""; StringBuilder retStr = new StringBuilder();
            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo prop = properties[i];
                string displayName = type.GetDisplayName(prop, DisplayNamesOnly);
                if (displayName == null && DisplayNamesOnly) continue;
                retStr.Append($"{c}{displayName ?? prop.Name}");
                c = delimiter;
            }
            return retStr.ToString();
        }

        public static IEnumerable<string> GetEnumDisplayNames(this Type enumeration, bool DisplayNameOnly)
        {
            if (!enumeration.IsEnum) throw new NotSupportedException("This method only support Enums.");
            var enums = Enum.GetValues(enumeration);
            List<string> displayNames = new List<string>();
            for (int i = 0; i < enums.Length; i++)
            {
                var e = enums.GetValue(i);
                string name = e.GetEnumDisplayName(DisplayNameOnly);
                if (name == null && DisplayNameOnly) continue;
                displayNames.Add(name);
            }
            return displayNames;
        }

        public static string GetEnumDisplayName(this object enumeration, bool DisplayNameOnly)
        {
            Type t = enumeration.GetType();
            if (!t.IsEnum) throw new NotSupportedException("This method only support Enums.");
            string name = Enum.GetName(t, enumeration);
            FieldInfo info = t.GetField(name);
            DisplayAttribute attr = (DisplayAttribute)info.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();
            return (attr != null) ? attr.Name : (DisplayNameOnly) ? null : name;

        }
    }
}
