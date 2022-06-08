using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bsod.Common.Extensions
{
    public static class DataTable_Extensions
    {
        /// <summary>
        /// Converts DataTable to a Dynamic List for Populating WebGrids
        /// </summary>
        /// <param name="dt">DataTable to convert</param>
        /// <returns>Dynamic view of DataTable with Column Names and values as list</returns>
        public static dynamic SerializeToDynamic(this DataTable dt)
        {
            if (dt != null)
            {
                var result = new List<dynamic>();
                for (int r = 0; r < dt.Rows.Count; r++)
                {
                    var obj = (IDictionary<string, object>)new System.Dynamic.ExpandoObject();
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        obj.Add(dt.Columns[i].ColumnName, dt.Rows[r][dt.Columns[i].ColumnName]);
                    }
                    result.Add(obj);
                }
                return result;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns an Array of Column Names
        /// </summary>
        /// <param name="ColumnsToGet">Columns to Convert</param>
        /// <returns>String Array of the Column Names</returns>
        public static IEnumerable<string> GetColumnNames(this DataColumnCollection ColumnsToGet)
        {
            try
            {
                List<string> colsVals = new List<string>();
                if (ColumnsToGet != null && ColumnsToGet.Count > 0)
                {
                    int colsCnt = ColumnsToGet.Count;
                    for (int c = 0; c < colsCnt; c++)
                    {
                        DataColumn col = ColumnsToGet[c];
                        colsVals.Add(col.ColumnName);
                    }
                }
                return colsVals.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Checks DataTable for column that equals the name given. Case insensitive.
        /// </summary>
        /// <param name="dt">DataTable to check</param>
        /// <param name="ColumnName">Column name to check collection for</param>
        /// <returns>true if collection contains column name given</returns>
        public static bool HasColumn(this DataTable dt, string ColumnName)
        {
            try
            {
                if (dt == null) { throw new ArgumentNullException(); }
                return dt.Columns.HasColumn(ColumnName);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Checks DataColumn Collection for column that equals the name given. Case insensitive.
        /// </summary>
        /// <param name="ColumnsToCheck">Column Collection</param>
        /// <param name="ColumnName">Column name to check collection for</param>
        /// <returns>true if collection contains column name given</returns>
        public static bool HasColumn(this DataColumnCollection ColumnsToCheck, string ColumnName)
        {
            try
            {
                if (ColumnsToCheck == null) { throw new ArgumentNullException(); }
                for (int i = 0; i < ColumnsToCheck.Count; i++)
                {
                    DataColumn col = ColumnsToCheck[i];
                    if (col.ColumnName.ToLower() == ColumnName.ToLower())
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Writes DataTable to CSV file.
        /// </summary>
        /// <param name="dataTable">DataTable to write.</param>
        /// <param name="filePath">FilePath to write the DataTable to.</param>
        public static void WriteToCsvFile(this DataTable dataTable, string filePath)
        {
            StringBuilder fileContent = new StringBuilder();
            foreach (var col in dataTable.Columns)
            {
                fileContent.Append(col.ToString() + ",");
            }
            fileContent.Replace(",", System.Environment.NewLine, fileContent.Length - 1, 1);
            foreach (DataRow dr in dataTable.Rows)
            {
                foreach (var column in dr.ItemArray)
                {
                    fileContent.Append("\"" + column.ToString() + "\",");
                }
                fileContent.Replace(",", System.Environment.NewLine, fileContent.Length - 1, 1);
            }
            System.IO.File.WriteAllText(filePath, fileContent.ToString());
        }
        /// <summary>
        /// Marks all rows as new rows to be inserted. Allowing SQLClass.UpdateDataTable to insert all records. 
        /// If this is set and the record exists in the SQL table that you are attempting to send this data to, 
        /// the SQLClass.UpdateDataTable will fail, because it will be attempting to insert a record that exists.
        /// 
        /// This function is mainly used for inserting records from one table to another, when you pull the data from 
        /// another SQL table to a DataTable if you don't change the DataRowState, SQLClass.UpdateDataTable will not 
        /// attempt to insert the records into the target table because the DataRowState is set to Unchanged. This will 
        /// mark all DataRowStates to Add or Insert forcing the SQLClass.UpdateDataTable to insert every record.
        /// </summary>
        /// <param name="dataTable">Data table to mark.</param>
        /// <param name="ignoreStates">States to ignore, for example if you need a row deleted from the table that exists set this to DataRowState.Deleted. 
        /// or if you need rows that were modified to be updated instead of inserted (IE they still exist in the SQL table) set this to DataRowState.Modified</param>
        public static void MarkInsertAll(this DataTable dataTable, params DataRowState[] ignoreStates)
        {
            if (dataTable == null || dataTable.Rows == null || dataTable.Rows.Count <= 0)
                throw new Exception("Cannot mark Insert when DataTable or Rows are not populated.");

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow rw = dataTable.Rows[i];
                bool skip = false;
                for (int s = 0; s < ignoreStates.Length; s++)
                {
                    DataRowState iState = ignoreStates[s];
                    if (rw.RowState == iState)
                    {
                        skip = true;
                        break;
                    }
                }
                if (skip) continue;
                rw.AcceptChanges();
                rw.SetAdded();
            }
        }
        /// <summary>
        /// Marks all rows as new rows to be updated. Forcing SQLClass.UpdateDataTable to update all records. 
        /// If this is set and the record does not exist in the SQL table that you are attempting to send this data to, 
        /// the SQLClass.UpdateDataTable will fail, because it will be attempting to update a record that does not exist.
        /// 
        /// This function is mainly used for updating all records from one table to another, when you pull the data from 
        /// another SQL table to a DataTable if you don't change the DataRowState, SQLClass.UpdateDataTable will not 
        /// attempt to update the records in the target table because the DataRowState is set to Unchanged. This will 
        /// mark all DataRowStates to Modified or Update forcing the SQLClass.UpdateDataTable to update every record.
        /// </summary>
        /// <param name="dataTable">Data table to mark.</param>
        /// <param name="ignoreStates">States to ignore, for example if you need a row deleted from the table that exists set this to DataRowState.Deleted. 
        /// or if you need rows that were added to be inserted instead of updated (IE they still exist in the SQL table) set this to DataRowState.Modified</param>
        public static void MarkUpdateAll(this DataTable dataTable, params DataRowState[] ignoreStates)
        {
            if (dataTable == null || dataTable.Rows == null || dataTable.Rows.Count <= 0)
                throw new Exception("Cannot mark Insert when DataTable or Rows are not populated.");

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow rw = dataTable.Rows[i];
                bool skip = false;
                for (int s = 0; s < ignoreStates.Length; s++)
                {
                    DataRowState iState = ignoreStates[s];
                    if (rw.RowState == iState)
                    {
                        skip = true;
                        break;
                    }
                }
                if (skip) continue;
                rw.AcceptChanges();
                rw.SetAdded();
            }
        }
        /// <summary>
        /// Gets the count of rows that have a changed state (IE Added, Deleted, or Modified). If you 
        /// want a specific state or states you can specify StatesToCount parameter. For example if you want just 
        /// newly added rows: DataTable.GetChangesCount(DataRowState.Added); or if you want newly added rows and 
        /// modified rows: DataTable.GetChangesCount(DataRowState.Added, DataRowState.Modifed);
        /// </summary>
        /// <param name="dataTable">Data table to get count from.</param>
        /// <param name="StatesToCount">DataRowState types to get count from.</param>
        public static int GetChangesCount(this DataTable dataTable, params DataRowState[] StatesToCount)
        {
            if (dataTable == null || dataTable.Rows == null || dataTable.Rows.Count <= 0)
                throw new Exception("Cannot mark Insert when DataTable or Rows are not populated.");
            int retCnt = 0;
            if (StatesToCount == null || StatesToCount.Length == 0)
                StatesToCount = new DataRowState[] { DataRowState.Added, DataRowState.Deleted, DataRowState.Modified };
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow rw = dataTable.Rows[i];
                bool mark = true;
                for (int s = 0; s < StatesToCount.Length; s++)
                {
                    DataRowState iState = StatesToCount[s];
                    if (rw.RowState == iState)
                    {
                        mark = true;
                        break;
                    }
                    mark = false;
                }
                if (mark)
                    retCnt++;
            }
            return retCnt;
        }
    }
}
