using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bsod.Common.Extensions
{
    public static class Exception_Extensions
    {

        /// <summary>
        /// Gets All The Data from an Error Message
        /// </summary>
        /// <param name="ex">Exception to gather the data from</param>
        /// <returns>All the Data from the Exception</returns>
        public static string Get_ErrorMessages(this Exception ex)
        {
            if (ex == null)
                return "{No Exception}";
            return ex.Get_ErrorMessages(false);
        }
        private static string _tabOver = "     ";
        /// <summary>
        /// Gets All The Data from an Error Message
        /// </summary>
        /// <param name="ex">Exception to gather the data from</param>
        /// <param name="isInner">Is the Exception given an inner exception</param>
        /// <returns>All the Data from the Exception</returns>
        public static string Get_ErrorMessages(this Exception ex, bool isInner)
        {
            string brk = "";//"*-------------------------------------------------------------*";
            string nLne = "\r\n";
            StringBuilder str = new StringBuilder();
            if (isInner)
            {
                if (ex == null)
                    return "";
                str.AppendFormat("{0}{1}INNER EXCEPTION: ", nLne, _tabOver);
                _tabOver = _tabOver + _tabOver;
            }
            else
            {
                if (ex == null)
                    return "{No Exception}";
                _tabOver = "     ";
                str.Append(brk);
                str.AppendFormat("{0}EXCEPTION OCCURRED:", nLne);
            }
            string exType = ex.GetType().ToString();
            if (!String.IsNullOrWhiteSpace(exType))
                str.AppendFormat("{0}{1}TYPE: {2}", nLne, _tabOver, exType);
            if (!String.IsNullOrWhiteSpace(ex.Source))
                str.AppendFormat("{0}{1}SOURCE: {2}", nLne, _tabOver, ex.Source);
            if (ex.TargetSite != null)
                str.AppendFormat("{0}{1}THROWN IN: {2}", nLne, _tabOver, ex.TargetSite);
            //str.AppendFormat("{0}{1}Occurred at: {2}", nLne, tabOver, DateTime.Now.ToString());
            //if (ex.GetType() == typeof(C_Exception) || ex.GetType().BaseType == typeof(C_Exception) ||
            //    (ex.GetType().BaseType != null && ex.GetType().BaseType.BaseType == typeof(C_Exception)))
            //{
            //    C_Exception cEx = ex as C_Exception;
            //    str.AppendFormat("{0}{1}CREATED AT: {2}", nLne, _tabOver, cEx.Created);
            //    str.AppendFormat("{0}{1}ERROR CODE: {2}", nLne, _tabOver, cEx.ErrorCode);
            //}
            if (!String.IsNullOrWhiteSpace(ex.Message))
                str.AppendFormat("{0}{1}MESSAGE: {2}", nLne, _tabOver, ex.Message.Replace("\r\n", String.Format("{0}{1}{1}", nLne, _tabOver).Replace("\t", "     ")));
            if (!String.IsNullOrWhiteSpace(ex.StackTrace))
                str.AppendFormat("{0}{1}STACK TRACE: {0}{1}{2}", nLne, _tabOver, ex.StackTrace.Replace("\r\n", String.Format("{0}{1}", nLne, _tabOver)));
            if (ex.GetType() == typeof(SqlException))
            {
                SqlException sqlEx = (SqlException)ex;
                str.AppendFormat("{0}{1}SQL Error has Occurred from server: {2} Provider: {3}", nLne, _tabOver, sqlEx.Server, sqlEx.Source);
                for (int i = 0; i < sqlEx.Errors.Count; i++)
                {
                    str.AppendFormat("{0}Index #{1}{0}Message: {2}{0}LineNumber: {3}{0}Source: {4}{0}Procedure: {5}",
                        String.Format("{0}{1}", nLne, _tabOver), i, sqlEx.Errors[i].Message, sqlEx.Errors[i].LineNumber, sqlEx.Errors[i].Source, sqlEx.Errors[i].Procedure);
                }
            }
            if (ex.GetType() == typeof(OdbcException))
            {
                OdbcException odbcEx = (OdbcException)ex;
                str.AppendFormat("{0}{1}ODBC Error has Occurred from Provider: {2}", nLne, _tabOver, odbcEx.Source);
                for (int i = 0; i < odbcEx.Errors.Count; i++)
                {
                    str.AppendFormat("{0}Index #{1}{0}Message: {2}{0}NativeError: {3}{0}Source: {4}{0}SQL: {5}",
                        String.Format("{0}{1}", nLne, _tabOver), i, odbcEx.Errors[i].Message, odbcEx.Errors[i].NativeError.ToString(), odbcEx.Errors[i].Source, odbcEx.Errors[i].SQLState);
                }
            }

            if (ex.InnerException != null) { str.Append(ex.InnerException.Get_ErrorMessages(true)); }
            else { str.AppendFormat("{0}{1}", nLne, brk); }
            return str.ToString();
        }
    }
}
