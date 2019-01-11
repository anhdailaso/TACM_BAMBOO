//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Web;
//using Biz.Lib.Helpers;
//using DataTable = Microsoft.Office.Interop.Word.DataTable;

//namespace Biz.TACM.Helpers
//{
//    public class ExcelUtils
//    {
//        /// <summary>
//        /// Create Excel file with from Excel template with one data object
//        /// </summary>
//        /// <param name="pSourceFile">Excel source template file full path</param>
//        /// <param name="pDescFile">Excel desc file full path</param>
//        /// <param name="pData">Data which will be saved into new Excel file</param>
//        /// <param name="pExcelTableName">The table name inside relate Excel Sheet where data will be fulled fill.</param>
//        /// <returns></returns>
//        public static bool CreateExcelFile(string pSourceFile, string pDescFile, DataTable pData, string pExcelTableName)
//        {
//            if (pData == null || string.IsNullOrEmpty(pSourceFile) || string.IsNullOrEmpty(pDescFile) || string.IsNullOrEmpty(pExcelTableName))
//                return false;

//            try
//            {
//                //create data file
//                FileUtils.CreateFileFromResource(pSourceFile, pDescFile);

//                //fill data
//                using (var package = ExtremeML.Packaging.SpreadsheetDocumentWrapper.Open(pDescFile))
//                {
//                    var table = package.WorkbookPart.GetTablePart(pExcelTableName).Table;
//                    table.Fill(pData);
//                }
//                return true;
//            }
//            catch
//            {
//                return false;
//            }
//        }

//        /// <summary>
//        /// Create Excel file with from Excel template with multible data objects
//        /// </summary>
//        /// <param name="pSourceFile">Excel source template file full path</param>
//        /// <param name="pDescFile">Excel desc file full path</param>
//        /// <param name="pDataTableCollection">Collection of DataTable objects which will be saved into new Excel file</param>
//        /// <param name="pExcelTableNameCollection">Collection of table names inside relate Excel file where data will be fulled fill.</param>
//        /// <returns></returns>
//        public static bool CreateExcelFile(string pSourceFile, string pDescFile, DataSet pDataTableCollection, List<string> pExcelTableNameCollection)
//        {
//            if (pDataTableCollection == null || string.IsNullOrEmpty(pSourceFile) || string.IsNullOrEmpty(pDescFile) || pExcelTableNameCollection == null)
//                return false;

//            if (pDataTableCollection.Tables.Count.Equals(0) || pExcelTableNameCollection.Count.Equals(0))
//                return false;

//            try
//            {
//                //create data file
//                FileUtils.CreateFileFromResource(pSourceFile, pDescFile);
//                for (int i = 0; i < pExcelTableNameCollection.Count; i++)
//                {
//                    string table_name = pExcelTableNameCollection[i];
//                    DataTable data = new DataTable();
//                    data = pDataTableCollection.Tables[i];
//                    if (data != null)
//                    {
//                        using (var package = ExtremeML.Packaging.SpreadsheetDocumentWrapper.Open(pDescFile))
//                        {
//                            var table = package.WorkbookPart.GetTablePart(table_name).Table;
//                            table.Fill(data);
//                        }
//                    }
//                }
//                return true;
//            }
//            catch
//            {
//                return false;
//            }
//        }
//    }
//}