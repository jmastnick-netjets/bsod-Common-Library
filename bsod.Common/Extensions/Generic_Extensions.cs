using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace bsod.Common.Extensions
{
    public static class Generic_Extensions
    {
        /// <summary>
        /// Removes Given Index From Array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">Array to remove Index From</param>
        /// <param name="index">Index to remove From Array</param>
        /// <returns>Returns Array With Index Removed From It</returns>
        public static T[] Remove<T>(this T[] array, int index)
        {
            Array.Copy(array, index + 1, array, index, array.Length - index - 1);
            Array.Resize(ref array, array.Length - 1);
            return array;
        }


        /// <summary>
        /// Casts given object to Given Type
        /// </summary>
        /// <param name="type">Type to Convert Object to</param>
        /// <param name="val">Object to Convert</param>
        /// <returns>Returns Object of Given Type</returns>
        public static dynamic ConvertValue(this Type type, dynamic val)
        {
            dynamic retValue = null; if (val == null) { return retValue; }
            if (type == typeof(double) || type == typeof(double?))
            {
                if (Double.TryParse(val.ToString(), out double dob))
                {
                    retValue = dob;
                }
            }
            else if (type == typeof(int))
            {
                if (Int32.TryParse(val.ToString(), out int integer))
                {
                    retValue = integer;
                }
            }
            else if (type == typeof(float))
            {
                if (Single.TryParse(val.ToString(), out float floa))
                {
                    retValue = floa;
                }
            }
            else if (type == typeof(bool))
            {
                if (Boolean.TryParse(val.ToString(), out bool boolean))
                {
                    retValue = boolean;
                }
            }
            else if (type == typeof(decimal))
            {
                if (Decimal.TryParse(val.ToString(), out decimal boolean))
                {
                    retValue = boolean;
                }
            }
            else if (type == typeof(string))
            {
                retValue = val.ToString();
            }
            else if (type.BaseType == typeof(Enum))
            {

                Enum e = Enum.Parse(type, val.ToString());
                retValue = e;
            }
            else
            {
                throw new Exception(String.Format("Unable to convert to type {0}", type.ToString()));
            }
            return retValue;
        }

        /// <summary>
        /// Checks to see if object has method by name given, case sensitive
        /// </summary>
        /// <param name="obj">Object to check</param>
        /// <param name="MethodName">Method name to check in object</param>
        /// <returns>true if object contains method name, false if it doesn't</returns>
        public static bool HasMethod(this object obj, string MethodName)
        {
            Type t = obj.GetType();
            return t.HasMethod(MethodName);

        }
        /// <summary>
        /// Checks to see if object has method by name given, case sensitive
        /// </summary>
        /// <param name="obj">Object to check</param>
        /// <param name="MethodName">Method name to check in object</param>
        /// <returns>true if object contains method name, false if it doesn't</returns>
        public static bool HasMethod<T>(this T obj, string MethodName)
        {
            Type t = obj.GetType();
            return t.HasMethod(MethodName);

        }

        /// <summary>
        /// Checks to see if class has method by name given, case sensitive
        /// </summary>
        /// <param name="t">class to check</param>
        /// <param name="MethodName">Method name to check in object</param>
        /// <returns>true if class contains method name, false if it doesn't</returns>
        public static bool HasMethod(this Type t, string MethodName)
        {
            return t.GetMethod(MethodName) != null;
        }

        /// <summary>
        /// Checks to see if object has property by name given, case sensitive
        /// </summary>
        /// <param name="obj">Object to check</param>
        /// <param name="PropertyName">Property name to check in object</param>
        /// <param name="CanSet">Has check see if the property can be set</param>
        /// <returns>true if object contains property name, false if it doesn't or false if it cannot be set</returns>
        public static bool HasProperty(this object obj, string PropertyName, bool CanSet = false)
        {
            Type t = obj.GetType();
            PropertyInfo pi = t.GetProperty(PropertyName);
            if (pi != null && !CanSet && pi.CanRead) { return true; }
            else if (pi != null && CanSet && pi.CanWrite) { return true; }
            else { return false; }
        }

        /// <summary>
        /// Checks to see if object has property by name given, case sensitive
        /// </summary>
        /// <param name="obj">Object to check</param>
        /// <param name="PropertyName">Property name to check in object</param>
        /// <param name="CanSet">Has check see if the property can be set</param>
        /// <returns>true if object contains property name, false if it doesn't or false if it cannot be set</returns>
        public static bool HasProperty<T>(this T obj, string PropertyName, bool CanSet = false)
        {
            Type t = obj.GetType();
            PropertyInfo pi = t.GetProperty(PropertyName);
            if (pi != null && !CanSet && pi.CanRead) { return true; }
            else if (pi != null && CanSet && pi.CanWrite) { return true; }
            else { return false; }
        }
        /// <summary>
        /// Checks to see if object has property by name given, case sensitive
        /// </summary>
        /// <param name="t">Object to check</param>
        /// <param name="PropertyName">Property name to check in object</param>
        /// <param name="CanSet">Has check see if the property can be set</param>
        /// <returns>true if object contains property name, false if it doesn't or false if it cannot be set</returns>
        public static bool HasProperty(this Type t, string PropertyName, bool CanSet = false)
        {
            PropertyInfo pi = t.GetProperty(PropertyName);
            if (pi != null && !CanSet && pi.CanRead) { return true; }
            else if (pi != null && CanSet && pi.CanWrite) { return true; }
            else { return false; }
        }

        /// <summary>
        /// Gets Name of Class
        /// </summary>
        /// <typeparam name="T">Any Type</typeparam>
        /// <param name="item">Object to check</param>
        /// <returns>string value representing the class name</returns>
        public static string GetName<T>(this T item) where T : class
        {
            if (item == null)
                return string.Empty;

            return typeof(T).GetProperties()[0].Name;
        }


        private static void Add<T>(ref T[] arr, T value) where T : class
        {
            if (arr == null) { arr = new T[0]; }
            Array.Resize(ref arr, arr.Length + 1);
            arr[arr.Length - 1] = value;
        }


        private static void Add<T>(ref IEnumerable<T> arr, T value)
        {
            if (arr == null) { throw new ArgumentNullException("arr", "Cannot add to null array."); }
            List<T> list = arr.ToList<T>();
            list.Add(value);
            arr = (IEnumerable<T>)list;
        }

        /// <summary>
        /// Returns a Collection as an Array
        /// </summary>
        /// <typeparam name="T">Type to cast to</typeparam>
        /// <param name="arr">Collection to Convert</param>
        /// <returns>An Array of the Collection</returns>
        public static T[] ToGenericArray<T>(Collection<T> arr)
        {
            try
            {
                if (arr != null)
                {
                    return arr.ToArray();
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Adds Object To Array
        /// </summary>
        /// <typeparam name="T">Type Of Array</typeparam>
        /// <param name="arr">Array To Add To</param>
        /// <param name="value">Object to Add To Array</param>
        public static IEnumerable<T> Add<T>(this IEnumerable<T> arr, T value)
        {
            if (arr == null) { throw new ArgumentNullException("arr", "Cannot add to null array."); }
            List<T> list = arr.ToList<T>();
            list.Add(value);
            arr = (IEnumerable<T>)list;
            return arr;
        }

        /// <summary>
        /// Adds Array of Objects To Array
        /// </summary>
        /// <typeparam name="T">Type Of Array</typeparam>
        /// <param name="arr">Array To Add To</param>
        /// <param name="value">Array of Objects to Add To Array</param>
        public static void AddRange<T>(this IEnumerable<T> arr, IEnumerable<T> value)
        {
            if (value == null) { return; }
            for (int i = 0; i < value.Count(); i++)
            {
                Add(ref arr, value.ElementAt(i));
            }
        }

        /// <summary>
        /// This Method does some hacking to convert the Given Object to The Type of the Object of Reference. 
        /// This Method is good if you don't have the known type to convert to, but you have an object of that type. 
        /// IE: dynamic NewObject = ObjectOfReference.CastToType(GivenObject)
        /// </summary>
        /// <typeparam name="T">Type to Convert To</typeparam>
        /// <param name="hackToInferNeededType">Object Of Reference</param>
        /// <param name="givenObject">Given Object To Convert</param>
        /// <returns>New object representing the Given Object but converted to the given type of the Object of reference</returns>
        public static T CastToType<T>(this T hackToInferNeededType, object givenObject) //where T : class
        {
            Type t = typeof(T);
            Type ot = givenObject.GetType();
            if (t.HasMethod("ToArray"))
            {
                return ConvertEnum(hackToInferNeededType, givenObject);
            }
            else if (t.IsArray)
            {
                return ConvertArr(hackToInferNeededType, givenObject);
            }
            else if (ot.HasMethod("ToArray"))
            {
                dynamic test = new List<T>() { hackToInferNeededType };
                return ConvertEnum(test, givenObject);
            }
            else if (ot.IsArray)
            {
                dynamic test = new T[] { hackToInferNeededType };
                return ConvertArr(test, givenObject)[0];
            }
            else
            {
                return (T)givenObject;
            }
        }
        /// <summary>
        /// Returns true if object is Type given. 
        /// </summary>
        /// <typeparam name="T">Type to check.</typeparam>
        /// <param name="obj">Object to check if Type.</param>
        public static bool IsType<T>(this object obj)
        {
            return IsType<T>(obj, out T outObj);
        }
        /// <summary>
        /// Returns true if object is Type given. Also will return an object of the Type.
        /// </summary>
        /// <typeparam name="T">Type to check.</typeparam>
        /// <param name="obj">Object to check if Type.</param>
        /// <param name="TypeObject">Returns an Object of the Type</param>
        public static bool IsType<T>(this object obj, out T TypeObject)
        {
            TypeObject = default(T);
            Type t = typeof(T);
            if (IsType(obj, t))
            {
                TypeObject = (T)obj;
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsType(this object obj, Type t)
        {
            bool ret = false;
            if (obj == null)
                return false;
            Type objType = obj.GetType();
            if (objType == t)
            {
                ret = true;
            }
            else
            {
                foreach (Type pi in objType.GetInterfaces())
                {
                    if (objType == pi)
                        ret = true;
                }
            }
            if (ret == false && objType.BaseType != null && t != objType.BaseType)
                ret = IsType(obj, objType.BaseType);
                
            return ret;
        }
        /// <summary>
        /// This Method does some hacking to convert the Given Object to The Type of the Object of Reference. 
        /// This Method is good if you don't have the known type to convert to, but you have an object of that type. 
        /// IE: dynamic NewObject = ObjectOfReference.CastToType(GivenObject)
        /// </summary>
        /// <typeparam name="T">Type to Convert To</typeparam>
        /// <param name="hackToInferNeededType">Object Of Reference</param>
        /// <param name="givenObject">Given Object To Convert</param>
        /// <returns>New object representing the Given Object but converted to the given type of the object of reference</returns>
        public static T[] CastToType<T>(this T[] hackToInferNeededType, object givenObject) where T : class
        {
            List<T> list = new List<T>();
            IEnumerable<object> objs = (IEnumerable<object>)givenObject;
            for (int i = 0; i < objs.Count(); i++)
            {
                dynamic obj = objs.ElementAt(i);
                list.Add((T)obj);
            }
            return list.ToArray();
        }

        private static T ConvertArr<T>(this T hackToInferNeededType, object givenObject) //where T : class
        {
            Type t = typeof(T);
            //Type bt = t.BaseType;
            T list;
            if (t.IsArray)
            {
                list = (T)Activator.CreateInstance(t, 0);
            }
            else
            {
                list = (T)Activator.CreateInstance(t);
            }
            dynamic objs = givenObject;
            for (int i = 0; i < objs.Length; i++)
            {
                dynamic obj = objs[i];
                Add(ref list, obj);
            }
            return list;
        }

        private static T ConvertEnum<T>(this T hackToInferNeededType, dynamic givenObject)
        {
            Type t = typeof(T);
            Type bt = GetBaseType(t);
            dynamic list = Activator.CreateInstance(t);
            IEnumerable<object> objs = (IEnumerable<object>)givenObject;
            for (int i = 0; i < objs.Count(); i++)
            {
                dynamic obj = objs.ElementAt(i);
                list.Add(obj);
            }
            return list;
        }

        /// <summary>
        /// Returns Object Representing The Object at the Index in the Array Object
        /// </summary>
        /// <typeparam name="T">Type of Array Object</typeparam>
        /// <param name="arr">Array Object</param>
        /// <param name="index">Index of Object to return</param>
        /// <returns>Object at the Given Index of the Array Object</returns>
        public static T GetIndex<T>(this T[] arr, int index)
        {
            if (arr == null) { arr = new T[1]; }
            if (arr.Count() < index + 1) { Array.Resize(ref arr, index + 1); }
            return arr[index];
        }

        /// <summary>
        /// Returns Object Representing The Object at the Index in the Array Object
        /// </summary>
        /// <typeparam name="T">Type of Array Object</typeparam>
        /// <param name="arr">Array Object</param>
        /// <param name="index">Index of Object to return</param>
        /// <returns>Object at the Given Index of the Array Object</returns>
        public static T GetIndex<T>(this IEnumerable<T> arr, int index)
        {
            dynamic tArr = new T[1];
            if (arr == null) { arr = tArr; }
            if (arr.Count() < index + 1) { AddRange(arr, tArr); }
            return arr.ElementAt(index);
        }

        /// <summary>
        /// Gets the Base Type of the Array
        /// </summary>
        /// <typeparam name="T">Type of Array</typeparam>
        /// <param name="arr">Array to return Type of</param>
        /// <returns>Type of the Base Type of the Array</returns>
        public static Type GetBaseType<T>(this T[] arr) where T : class
        {
            return typeof(T);
        }
        /// <summary>
        /// Gets the Base Type of the Array
        /// </summary>
        /// <typeparam name="T">Type of Array</typeparam>
        /// <param name="arr">Array to return Type of</param>
        /// <returns>Type of the Base Type of the Array</returns>
        public static Type GetBaseType<T>(this IEnumerable<T> arr) where T : class
        {
            return typeof(T);
        }
        /// <summary>
        /// Gets the Base Type of the Type
        /// </summary>
        /// <param name="type">type to return Type of</param>
        /// <returns>Type of the Base Type of the Type</returns>
        public static Type GetBaseType(this Type type)
        {
            return type.BaseType;
        }

        /// <summary>
        /// Returns true if object given is a nullable type.
        /// </summary>
        /// <typeparam name="T">Type of object given.</typeparam>
        /// <param name="obj">Object to check if nullable.</param>
        /// <returns></returns>
        public static bool IsNullable<T>(this T obj)
        {
            if (obj == null) return true;
            Type type = typeof(T);
            if (!type.IsValueType) return true;
            if (Nullable.GetUnderlyingType(type) != null) return true;
            return false;
        }
    }
}
