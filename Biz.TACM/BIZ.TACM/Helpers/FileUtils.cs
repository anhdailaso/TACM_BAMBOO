//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Web;
//using Biz.Lib.Helpers;

//namespace Biz.TACM.Helpers
//{
//    public class FileUtils
//    {
//        public static string GetFileType(string pFileName)
//        {
//            string fileType = Protect.ToString(pFileName).Trim().ToLower();
//            if (pFileName != string.Empty && pFileName.IndexOf('.') > 0)
//            {
//                string[] arr = fileType.Split('.');
//                if (arr.Length > 0)
//                    fileType = arr[arr.Length - 1];
//            }
//            return fileType;
//        }

//        public static bool CheckFileTypeValid(string pFileType)
//        {
//            pFileType = Protect.ToString(pFileType).Trim().ToLower();
//            if (pFileType == string.Empty)
//                return false;
//            string imageTypes = Protect.ToString(Resources.Settings.FILE_TYPES);
//            if (imageTypes.IndexOf(pFileType) >= 0)
//                return true;
//            return false;
//        }

//        public static bool DeleteFile(string filePath)
//        {
//            try
//            {
//                if (System.IO.File.Exists(filePath))
//                {
//                    System.IO.File.Delete(filePath);
//                }
//            }
//            catch
//            {
//                return false;
//            }

//            return true;
//        }

//        public static StringBuilder BindFileContent(DataTable pData)
//        {
//            StringBuilder sb = new StringBuilder();
//            if (pData != null)
//            {
//                string line = "Date Time";
//                string break_cell = Resources.Settings.BREAKE_CELL_LINE.Trim();

//                //columns
//                foreach (DataColumn col in pData.Columns)
//                {
//                    line += break_cell + col.ColumnName;
//                }
//                sb.AppendLine(line);

//                //rows
//                line = DateTime.Now.ToString();
//                foreach (DataRow row in pData.Rows)
//                {
//                    for (int rindex = 0; rindex < pData.Columns.Count; rindex++)
//                    {
//                        line += break_cell + Protect.ToString(row[rindex]);
//                    }
//                    sb.AppendLine(line);
//                    line = DateTime.Now.ToString();
//                }
//            }
//            return sb;
//        }

//        public static void LogFile(StringBuilder pContent, string pPath, string pFileName)
//        {
//            LogFile(pContent, pPath, pFileName, false, false, false);
//        }

//        public static void LogFile(StringBuilder pContent, string pPath, string pFileName, bool pYearSub)
//        {
//            LogFile(pContent, pPath, pFileName, pYearSub, false, false);
//        }

//        public static void LogFile(StringBuilder pContent, string pPath, string pFileName, bool pYearSub, bool pMonthSub)
//        {
//            LogFile(pContent, pPath, pFileName, pYearSub, pMonthSub, false);
//        }

//        public static void LogFile(StringBuilder pContent, string pPath, string pFileName, bool pYearSub, bool pMonthSub, bool pDaySub)
//        {
//            if (pContent != null && pContent.Length > 0)
//            {
//                CreateFolder(pPath);
//                if (pYearSub)
//                {
//                    pPath += Resources.Settings.BREAK_FOLDER + DateTime.Now.Year;
//                    CreateFolder(pPath);
//                }
//                if (pMonthSub)
//                {
//                    pPath += Resources.Settings.BREAK_FOLDER + DateTime.Now.Month;
//                    CreateFolder(pPath);
//                }
//                if (pDaySub)
//                {
//                    pPath += Resources.Settings.BREAK_FOLDER + DateTime.Now.Day;
//                    CreateFolder(pPath);
//                }
//                pPath += Resources.Settings.BREAK_FOLDER + pFileName;

//                FileStream fs = System.IO.File.Open(pPath, FileMode.CreateNew, FileAccess.Write);
//                StreamWriter sw = new StreamWriter(fs);
//                sw.WriteLine(pContent.ToString());
//                sw.Close();
//                sw = null;
//            }
//        }

//        private static void CreateFolder(string pPath)
//        {
//            if (!Directory.Exists(pPath))
//            {
//                Directory.CreateDirectory(pPath);
//            }
//        }

//        #region Excel utils
//        /// <summary>
//        /// Copies the specified resource to a temporary file and returns its path.
//        /// </summary>
//        public static string CreateFileFromResource(
//            string resourcePath)
//        {
//            var filePath = Path.GetTempFileName();
//            CreateFileFromResource(resourcePath, filePath);
//            return filePath;
//        }

//        /// <summary>
//        /// Copies the specified resource to the specified path.
//        /// </summary>
//        public static void CreateFileFromResource(
//            string resourcePath,
//            string filePath)
//        {
//            var data = new byte[4096];
//            if (System.IO.File.Exists(resourcePath))
//            {
//                using (var inStream = new FileStream(resourcePath, FileMode.Open, FileAccess.Read))
//                using (var outStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
//                {
//                    int bytes;
//                    do
//                    {
//                        bytes = inStream.Read(data, 0, 4096);
//                        outStream.Write(data, 0, bytes);
//                    } while (bytes > 0);
//                }
//            }
//        }
//        #endregion
//    }
//}