using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace bsod.Common.Extensions
{
    public static class xml_Extensions
    {

        /// <summary>
        /// Converts Any Object to XML
        /// </summary>
        /// <param name="obj">Object To Convert</param>
        /// <returns>Returns XML Representation of the Object</returns>
        public static XmlDocument ConvertToXml(this object obj)
        {
            return ConvertToXml(obj, null);
        }
        /// <summary>
        /// Converts Any Object to XML
        /// </summary>
        /// <param name="obj">Object To Convert</param>
        /// <param name="RootName">Root Name to give XML</param>
        /// <returns>Returns XML Representation of the Object</returns>
        public static XmlDocument ConvertToXml(this object obj, string RootName)
        {
            XmlDocument xDoc = new XmlDocument();
            XmlElement xRoot = null;

            if (String.IsNullOrWhiteSpace(RootName)) { xRoot = xDoc.CreateElement("xRoot"); }
            else { xRoot = xDoc.CreateElement(RootName); }

            Type type = obj.GetType();
            if (type.IsArray || type.IsGenericType)
            {
                if (obj.HasMethod("ToArray"))
                {
                    obj = ((dynamic)obj).ToArray();
                }
                else if (type.IsArray)
                {
                    obj = (Array)obj;
                }
                else
                {
                    throw new InvalidCastException(string.Format("Unable to cast object type to array: {0}", type.ToString()));
                }
                //XmlElement e = null;
                for (int m = 0; m < ((Array)obj).GetLength(0); m++)
                {
                    object mo = ((Array)obj).GetValue(m);
                    Type moType = mo.GetType();
                    XmlElement n = xDoc.CreateElement(moType.Name + (m + 1).ToString());
                    mo.getProperty(xDoc, n);
                    xRoot.AppendChild(n);
                }
                //xRoot.AppendChild(e);
            }
            else
            {
                xRoot = obj.getProperty(xDoc, xRoot);

            }
            xDoc.AppendChild(xRoot);

            return xDoc;
        }

        private static XmlElement getProperty(this object o, XmlDocument xDoc, XmlElement xRoot)
        {
            Type modelType = o.GetType();
            if (modelType == typeof(string) || modelType == typeof(int) ||
                    modelType == typeof(bool) || modelType == typeof(float) ||
                    modelType == typeof(decimal))
            {
                xRoot.InnerText = o.ToString();
            }
            else if (modelType.IsArray || modelType.IsGenericType)
            {
                if (o.HasMethod("ToArray"))
                {
                    o = ((dynamic)o).ToArray();
                }
                else if (modelType.IsArray)
                {
                    o = (Array)o;
                }
                else if (modelType.GetGenericTypeDefinition() == typeof(Collection<>))
                {
                    o = ((dynamic)o).ToGenericArray();
                }
                else
                {
                    throw new InvalidCastException(string.Format("Unable to cast object type to array: {0}", modelType.ToString()));
                }
                XmlElement e = null;
                for (int m = 0; m < ((Array)o).GetLength(0); m++)
                {
                    object mo = ((Array)o).GetValue(m);
                    Type moType = mo.GetType();
                    if (e == null) { e = xDoc.CreateElement(moType.Name); }
                    XmlElement n = xDoc.CreateElement("t" + (m + 1).ToString());
                    mo.getProperty(xDoc, n);
                    e.AppendChild(n);
                }
                if (e != null) { xRoot.AppendChild(e); }
                else
                {
                    //No Data in Object
                    //Console.WriteLine("Fail");
                }
            }
            else
            {
                PropertyInfo[] pis = modelType.GetProperties();
                for (int i = 0; i < pis.Count(); i++)
                {
                    if (pis[i].CanRead)
                    {
                        XmlElement el = xDoc.CreateElement(pis[i].Name);
                        if (pis[i].PropertyType.IsArray)
                        {
                            Array arr = (Array)pis[i].GetValue(o, null);
                            for (int x = 0; x < arr.Length; x++)
                            {
                                x.getProperty(xDoc, el);
                            }
                        }
                        else
                        {

                            object t = pis[i].GetValue(o, null);
                            if (t != null)
                            {
                                Type tt = t.GetType();
                                if (tt == typeof(string) || tt == typeof(int) || tt == typeof(double) ||
                                            tt == typeof(bool) || tt == typeof(float) ||
                                            tt == typeof(decimal) || tt == typeof(DateTime) || tt.IsEnum)
                                {
                                    el.InnerText = t.ToString();
                                }
                                else if (t.HasProperty("InnerXml"))
                                {
                                    el.InnerXml = ((dynamic)t).InnerXml;
                                }
                                else
                                {
                                    t.getProperty(xDoc, el);
                                }
                            }
                        }
                        xRoot.AppendChild(el);
                    }
                }
            }
            return xRoot;
        }

        /// <summary>
        /// Converts XML Document To Given Type
        /// </summary>
        /// <typeparam name="T">Type to Convert XML To</typeparam>
        /// <param name="xDoc">XML Document To Convert</param>
        /// <returns>Object as Given Type</returns>
        public static T ConvertToObject<T>(this XmlDocument xDoc)
        {
            return xDoc.ConvertToObject<T>(false).FirstOrDefault();
        }

        /// <summary>
        /// Converts XML Document To Given Type
        /// </summary>
        /// <typeparam name="T">Type to Convert XML To</typeparam>
        /// <param name="xDoc">XML Document To Convert</param>
        /// <param name="isArray">If Children of Root Is Array of the Given Type</param>
        /// <returns>Array of Object as Given Type</returns>
        public static IEnumerable<T> ConvertToObject<T>(this XmlDocument xDoc, bool isArray)
        {
            Type ObjectType = typeof(T);
            List<T> os = new List<T>();

            XmlElement xRoot = xDoc.DocumentElement;
            if (isArray)
            {
                XmlNodeList arrRoot = xRoot.ChildNodes;
                for (int p = 0; p < arrRoot.Count; p++)
                {
                    XmlElement arr = (XmlElement)arrRoot[p];
                    //XmlElement el = (XmlElement)arr[t];
                    os.AddRange(arr.ConvertToObject<T>(true));
                }
            }
            else
            {
                os.AddRange(((XmlElement)xRoot).ConvertToObject<T>(true));
            }
            return (IEnumerable<T>)os;
        }

        /// <summary>
        /// Converts XML Document To Given Type
        /// </summary>
        /// <typeparam name="T">Type to Convert XML To</typeparam>
        /// <param name="xDoc">XML Document To Convert</param>
        /// <param name="obj">Object to convert to</param>
        public static T ConvertToObject<T>(this XmlDocument xDoc, T obj)
        {
            XmlElement xRoot = xDoc.DocumentElement;
            xRoot.ConvertToObject(obj);
            return obj;
        }

        /// <summary>
        /// Converts XML Element To Given Type
        /// </summary>
        /// <typeparam name="T">Type to Convert XML To</typeparam>
        /// <param name="xEl">XML Element To Convert</param>
        /// <returns>Object as Given Type</returns>
        public static T ConvertToObject<T>(this XmlElement xEl)
        {
            return xEl.ConvertToObject<T>(true).FirstOrDefault();
        }

        /// <summary>
        /// Converts XML Element To Given Type
        /// </summary>
        /// <typeparam name="T">Type to Convert XML To</typeparam>
        /// <param name="xEl">XML Element To Convert</param>
        /// <param name="isArray">True If Array</param>
        /// <returns>Array of Object as Given Type</returns>
        public static IEnumerable<T> ConvertToObject<T>(this XmlElement xEl, bool isArray)
        {
            Type ObjectType = typeof(T);
            object obj = Activator.CreateInstance(ObjectType);
            object convertObj = xEl.ConvertToObject(ObjectType);
            dynamic ret = obj.CastToType(convertObj);
            List<T> list = new List<T>
            {
                (T)ret
            };
            return list;
        }

        /// <summary>
        /// Converts XML Element To Given Type
        /// </summary>
        /// <typeparam name="T">Type to Convert XML To</typeparam>
        /// <param name="xEl">XML Element To Convert</param>
        /// <param name="obj">Object to Send the Data to</param>
        public static T ConvertToObject<T>(this XmlElement xEl, T obj)
        {
            Type ObjectType = typeof(T);
            object convertObj = xEl.ConvertToObject(ObjectType);
            obj = obj.CastToType(convertObj);
            return obj;
        }

        private static IEnumerable<object> ConvertToObject(this XmlElement xEl, Type ObjectType)
        {
            Type arrType = ObjectType.MakeArrayType();
            ////List<dynamic> os = new List<dynamic>();
            object o = Activator.CreateInstance(ObjectType);
            object[] oArr = (object[])Activator.CreateInstance(arrType, 0);

            XmlNodeList xProperties = xEl.ChildNodes;
            //PropertyInfo[] properties = ObjectType.GetProperties();

            for (int p = 0; p < xProperties.Count; p++)
            {
                XmlElement xProp = (XmlElement)xProperties[p];
                string propName = xProp.Name;

                //if (children.Count > 0)
                //{
                //     xProp.ConvertToObject(
                //}
                //else
                //{
                object propVal = xProp.InnerText;
                if (o.HasProperty(propName))
                {


                    PropertyInfo prop = ObjectType.GetProperty(propName);
                    Type propType = prop.PropertyType;
                    XmlNodeList children = xProp.ChildNodes;
                    if (propType.IsArray || propType.HasMethod("ToArray"))
                    {
                        dynamic arr = Activator.CreateInstance(propType);
                        Type arrItmType = arr.GetBaseType();
                        XmlNodeList arrRoot = xProp.ChildNodes;
                        for (int a = 0; a < arrRoot.Count; a++)
                        {
                            XmlNodeList itms = arrRoot[a].ChildNodes;
                            for (int s = 0; s < itms.Count; s++)
                            {
                                XmlElement itm = (XmlElement)itms[s];
                                dynamic ret = itm.ConvertToObject(arrItmType);
                                ret = arr.CastToType(ret);
                                arr.AddRange(ret);
                            }
                        }
                        propVal = arr;
                        if (propType.IsArray) { propVal = arr.ToArray(); }
                    }
                    else if (children.Count > 0 && !(children.Count == 1 && children[0].NodeType != XmlNodeType.Element))
                    {
                        //XmlElement itm = (XmlElement)itms[s];
                        propVal = xProp.ConvertToObject(propType).FirstOrDefault();
                        //dynamic propObj = prop.
                        //ret = CastToType(arr, ret);
                        //arr.AddRange(ret);
                    }
                    if (String.IsNullOrWhiteSpace(propVal.ToString()))
                    {
                        propVal = null;
                    }
                    propVal = propType.ConvertValue(propVal);
                    prop.SetValue(o, propVal, null);
                }
                //}
            }
            oArr.Add(o);

            return oArr; //os.AsEnumerable();
        }


        /// <summary>
        /// Checks for specific ChildNode by name, not case sensitive and returns true if found.
        /// </summary>
        /// <param name="el">Element to Check</param>
        /// <param name="name">Name of Element to find</param>
        /// <returns>true if child node is found, false if not found.</returns>
        public static bool HasChildNode(this XmlElement el, string name)
        {
            try
            {
                for (int i = 0; i < el.ChildNodes.Count; i++)
                {
                    XmlElement child = (XmlElement)el.ChildNodes[i];
                    if (child.Name.ToLower() == name.ToLower())
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// Gets Root of Xml Document
        /// </summary>
        /// <param name="xDoc">Gets the Root of the Xml Document</param>
        /// <returns>the root of the xml document</returns>
        public static XmlElement GetRoot(this XmlDocument xDoc)
        {
            return GetRoot(xDoc, null);
        }

        /// <summary>
        /// Gets the Root of the XmlDocument by name case insensitive, if no name is given, then returns first root of document. If Name is given will search all children for the name and return the first it finds.
        /// </summary>
        /// <param name="XDoc">XmlDocument to get root from</param>
        /// <param name="RootName">Name of Root to find</param>
        /// <returns>Root of Xml Document or Xml Element with the name given</returns>
        public static XmlElement GetRoot(this XmlDocument XDoc, string RootName)
        {
            if (XDoc == null || XDoc.DocumentElement == null || XDoc.ChildNodes.Count <= 0)
            {
                throw new Exception("Failed to Load XML Document or Document Not in Correct Format");
            }
            XmlElement root = XDoc.DocumentElement;
            if (String.IsNullOrWhiteSpace(RootName)) { return root; }
            if (RootName == root.Name) { return root; }
            return GetRoot(root, RootName);
        }

        /// <summary>
        /// Finds a specific node of the XmlElement by name. Name must be populated or will throw a null argument exception. Search is Case sensitive and will search all children.
        /// </summary>
        /// <param name="root">XmlElement to search</param>
        /// <param name="RootName">Name to search for case insensitive</param>
        /// <returns>XmlElemnt with the name given</returns>
        public static XmlElement GetRoot(this XmlElement root, string RootName)
        {
            if (String.IsNullOrWhiteSpace(RootName)) { throw new ArgumentNullException("RootName", "Cannot find root of an Element without the name of the root."); }
            if (root.Name.ToLower() == RootName.ToLower()) { return root; }
            for (int i = 0; i < root.ChildNodes.Count; i++)
            {
                XmlNode node = root.ChildNodes[i];
                if (node.GetType() == typeof(XmlElement) || node.GetType().BaseType == typeof(XmlElement))
                {
                    XmlElement enode = (XmlElement)node;

                    if (enode.Name.ToLower() == RootName.ToLower()) { return enode; }
                    if (enode.HasChildNodes)
                    {
                        XmlElement foundNode = GetRoot(enode, RootName);
                        if (foundNode != null && foundNode.Name.ToLower() == RootName.ToLower())
                        {
                            return foundNode;
                        }
                    }
                }
            }
            return null;
        }
    }
}
