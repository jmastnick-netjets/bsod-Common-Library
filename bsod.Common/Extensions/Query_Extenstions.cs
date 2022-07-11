using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace bsod.Common.Extensions
{
    public static class Query_Extenstions
    {

        public static async Task<IEnumerable<T>> QueryAllAsync<T>(this IEnumerable<T> model, IEnumerable<QueryFilterItem> filters)
        {
            return await Task.Run(() => QueryAll(model, filters));
        }


        public static IEnumerable<T> QueryAll<T>(this IEnumerable<T> model, IEnumerable<QueryFilterItem> filters)
        {
            if (!(filters.Count() > 0)) return model;
            List<T> retList = new List<T>();
            //string cField = null;
            for (int i = 0; i < filters.Count(); i++)
            {
                QueryFilterItem filter = filters.ElementAt(i);
                if (filter.Field == null)
                {
                    continue;
                }

                //retList.AddRange(Query(model, filter));
                if (filter.Field == QueryFilterItem.AllOption)
                {
                    model = model.QueryAll(filter.Value);
                }
                else
                {
                    model = Query(model, filter);
                }
                if (model == null) return null;
            }
            //return retList.Distinct();
            return model.Distinct();
        }

        public static IEnumerable<T> Query<T>(this IEnumerable<T> model, QueryFilterItem filter)
        {
            if (filter.Field != null)
            {
                Type type = typeof(T);
                Enum.TryParse(filter.Condition, out QueryFilterCondition con);
                PropertyInfo prop = type.GetProperty(filter.Field);
                return Query(model, prop, filter.Value, con);
            }
            return null;
        }

        public static IEnumerable<T> Query<T>(this IEnumerable<T> model, PropertyInfo prop, string value, QueryFilterCondition condition)
        {
            StringComparison ignore = StringComparison.CurrentCultureIgnoreCase;

            switch (condition)
            {
                case QueryFilterCondition.Eq:
                    model = (from r in model
                             where (prop.GetValue(r) ?? "").ToString().Equals(value ?? "", ignore) == true
                             select r);
                    break;
                case QueryFilterCondition.Neq:
                    model = (from r in model
                             where (prop.GetValue(r) ?? "").ToString().Equals(value ?? "", ignore) == false
                             select r);
                    break;
                case QueryFilterCondition.Gt:
                    try
                    {
                        if (prop.PropertyType == typeof(Int32) || prop.PropertyType == typeof(Nullable<Int32>))
                        {
                            if (Int32.TryParse(value, out int oVal))
                            {
                                model = (from r in model
                                         where (int?)prop.GetValue(r) > oVal
                                         select r);
                            }
                        }
                        else if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(Nullable<System.DateTime>))
                        {
                            if (DateTime.TryParse(value, out DateTime oVal))
                            {
                                model = (from r in model
                                         where (DateTime?)prop.GetValue(r) > oVal
                                         select r);
                            }
                        }
                        else
                        {
                            model = (from r in model
                                     where prop.GetValue(r).ToString().ConvertToASCIISum() > (value ?? String.Empty).ConvertToASCIISum()
                                     select r);
                        }
                    }
                    catch (Exception)
                    {
                        try
                        {
                            model = (from r in model
                                     where (prop.GetValue(r) ?? String.Empty).ToString().ConvertToASCIISum() > (value ?? String.Empty).ConvertToASCIISum()
                                     select r);
                        }
                        catch (Exception)
                        { }
                    }
                    break;
                case QueryFilterCondition.Ge:
                    try
                    {
                        if (prop.PropertyType == typeof(Int32) || prop.PropertyType == typeof(Nullable<Int32>))
                        {
                            if (Int32.TryParse(value, out int oVal))
                            {
                                model = (from r in model
                                         where (int?)prop.GetValue(r) >= oVal
                                         select r);
                            }
                            else if (prop.PropertyType == typeof(Nullable<Int32>) && value == null)
                            {
                                model = (from r in model
                                         where (int?)prop.GetValue(r) == null
                                         select r);
                            }
                        }
                        else if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(Nullable<System.DateTime>))
                        {
                            if (DateTime.TryParse(value, out DateTime oVal))
                            {
                                model = (from r in model
                                         where (DateTime?)prop.GetValue(r) >= oVal
                                         select r);
                            }
                            else if (prop.PropertyType == typeof(Nullable<System.DateTime>) && value == null)
                            {
                                model = (from r in model
                                         where (DateTime?)prop.GetValue(r) == null
                                         select r);
                            }
                        }
                        else
                        {
                            model = (from r in model
                                     where (prop.GetValue(r) ?? String.Empty).ToString().ConvertToASCIISum() >= (value ?? String.Empty).ConvertToASCIISum()
                                     select r);
                        }
                    }
                    catch (Exception)
                    {
                        try
                        {
                            model = (from r in model
                                     where (prop.GetValue(r) ?? String.Empty).ToString().ConvertToASCIISum() >= (value ?? String.Empty).ConvertToASCIISum()
                                     select r);
                        }
                        catch (Exception)
                        { }
                    }
                    break;
                case QueryFilterCondition.Lt:
                    try
                    {
                        if (prop.PropertyType == typeof(Int32) || prop.PropertyType == typeof(Nullable<Int32>))
                        {
                            if (Int32.TryParse(value, out int oVal))
                            {
                                model = (from r in model
                                         where (int?)prop.GetValue(r) < oVal
                                         select r);
                            }
                        }
                        else if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(Nullable<System.DateTime>))
                        {
                            if (DateTime.TryParse(value, out DateTime oVal))
                            {
                                model = (from r in model
                                         where (DateTime?)prop.GetValue(r) < oVal
                                         select r);
                            }
                        }
                        else
                        {
                            model = (from r in model
                                     where (prop.GetValue(r) ?? String.Empty).ToString().ConvertToASCIISum() < (value ?? String.Empty).ConvertToASCIISum()
                                     select r);
                        }
                    }
                    catch (Exception)
                    {
                        try
                        {
                            model = (from r in model
                                     where (prop.GetValue(r) ?? String.Empty).ToString().ConvertToASCIISum() < (value ?? String.Empty).ConvertToASCIISum()
                                     select r);
                        }
                        catch (Exception)
                        { }
                    }
                    break;
                case QueryFilterCondition.Le:
                    try
                    {
                        if (prop.PropertyType == typeof(Int32) || prop.PropertyType == typeof(Nullable<Int32>))
                        {
                            if (Int32.TryParse(value, out int oVal))
                            {
                                model = (from r in model
                                         where (int?)prop.GetValue(r) <= oVal
                                         select r);
                            }
                            else if (prop.PropertyType == typeof(Nullable<Int32>) && value == null)
                            {
                                model = (from r in model
                                         where (int?)prop.GetValue(r) == null
                                         select r);
                            }
                        }
                        else if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(Nullable<System.DateTime>))
                        {
                            if (DateTime.TryParse(value, out DateTime oVal))
                            {
                                model = (from r in model
                                         where (DateTime?)prop.GetValue(r) <= oVal
                                         select r);
                            }
                            else if (prop.PropertyType == typeof(Nullable<System.DateTime>) && value == null)
                            {
                                model = (from r in model
                                         where (DateTime?)prop.GetValue(r) == null
                                         select r);
                            }
                        }
                        else
                        {
                            model = (from r in model
                                     where (prop.GetValue(r) ?? String.Empty).ToString().ConvertToASCIISum() <= (value ?? String.Empty).ConvertToASCIISum()
                                     select r);
                        }
                    }
                    catch (Exception)
                    {
                        try
                        {
                            model = (from r in model
                                     where (prop.GetValue(r) ?? String.Empty).ToString().ConvertToASCIISum() <= (value ?? String.Empty).ConvertToASCIISum()
                                     select r);
                        }
                        catch (Exception)
                        { }
                    }
                    break;
                case QueryFilterCondition.Like:
                    model = (from r in model
                             where (prop.GetValue(r) ?? String.Empty).ToString().IndexOf(value ?? String.Empty, ignore) >= 0
                             select r);
                    break;
                case QueryFilterCondition.NotLike:
                    model = (from r in model
                             where (prop.GetValue(r) ?? String.Empty).ToString().IndexOf(value ?? String.Empty, ignore) < 0
                             select r);
                    break;
                default:
                    break;
            }
            return model;
        }
    }
}
