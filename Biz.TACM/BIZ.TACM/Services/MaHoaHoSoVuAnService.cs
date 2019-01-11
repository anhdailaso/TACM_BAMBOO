using Biz.Lib.Helpers;
using Biz.TACM.IServices;
using Biz.TACM.Models.ViewModel.NhanDon;
using Spire.Doc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Xceed.Words.NET;

namespace Biz.TACM.Services
{
    public class MaHoaHoSoVuAnService : IMaHoaHoSoVuAnService
    {
        private ISettingDataService _settingDataService;
        private ISettingDataService SettingDataService => _settingDataService ?? (_settingDataService = new SettingDataService());

        public string[] TestEditingFileWord(string fileName, string folderPath, List<DuongSuViewModel> danhSachDuongSu)
        {
            string filePath = folderPath + fileName;
            // We will need a file name for our output file (change to suit your machine):
            fileName = string.Concat(
                        Path.GetFileNameWithoutExtension(fileName),
                        "_",
                        "Encrypted",
                        Path.GetExtension(fileName)
                    );
            //string pdfFileName = string.Concat(Path.GetFileNameWithoutExtension(fileName), ".pdf");

            // Let's save the file with a meaningful name, including the 
            // applicant name and the letter date:
            string outputFileName = folderPath + fileName;
            //string pdfOutputFileName = folderPath + pdfFileName;

            string fileExtension = Path.GetExtension(fileName);
            if (fileExtension.Equals(".doc"))
            {
                filePath = converter(filePath, ".doc", ".docx");
            }
            using (DocX document = DocX.Load(filePath))
            {
                string[] pronouns = { "Anh ", "anh ", "Chị ", "chị ", "Ông ", "ông ", "Bà ", "bà " };
                // Perform the replace:
                foreach (var duongsu in danhSachDuongSu)
                {
                    document.ReplaceText(duongsu.HoVaTen, EncryptName(duongsu.HoVaTen));
                    document.ReplaceText(duongsu.NoiDKHKTT, EncryptAddress(duongsu.NoiDKHKTT));
                    foreach (var pronoun in pronouns)
                    {
                        document.ReplaceText(pronoun + GetFirstName(duongsu.HoVaTen), pronoun + EncryptName(GetFirstName(duongsu.HoVaTen)));
                    }
                }

                // Save as New filename:
                document.SaveAs(outputFileName);

                //Save all changes made to document.
                //document.Save();
            }
            // Open in word:
            //Process.Start("WINWORD.EXE", "\"" + outputFileName + "\"");

            string pdfOutputFileName = converter(outputFileName, fileExtension, ".pdf");

            return new string[] { outputFileName, pdfOutputFileName };
        }

        public string converter(string filePath, string fromExtension, string toExtension)
        {
            Document document = new Document();
            document.LoadFromFile(filePath);
            string newfilePath = filePath.Replace(fromExtension, toExtension);
            switch (toExtension)
            {
                case ".doc":
                    document.SaveToFile(newfilePath, FileFormat.Doc); break;
                case ".docx":
                    document.SaveToFile(newfilePath, FileFormat.Docx2010); break;
                case ".pdf":
                    document.SaveToFile(newfilePath, FileFormat.PDF); break;
            }
            File.Delete(filePath);
            return newfilePath;
        }

        /*public string ConvertFile(string filePath, string fromExtension, string toExtension)
        {
            Application word = new Application();

            var sourceFile = new FileInfo(filePath);
            var document = word.Documents.Open(sourceFile.FullName);

            string newFileName = sourceFile.FullName.Replace(fromExtension, toExtension);

            WdSaveFormat saveFormat = WdSaveFormat.wdFormatXMLDocument;
            switch (toExtension)
            {
                case ".doc":
                case ".docx":
                    saveFormat = WdSaveFormat.wdFormatXMLDocument;
                    break;
                case ".pdf":
                    saveFormat = WdSaveFormat.wdFormatPDF;
                    break;
            }

            document.SaveAs2(newFileName, FileFormat: saveFormat,
                             CompatibilityMode: WdCompatibilityMode.wdWord2013);

            word.ActiveDocument.Close();
            word.Quit();

            //File.Delete(filePath);

            return filePath.Replace(fromExtension, toExtension);
        }*/

        public SelectList DanhSachTuCachThamGiaToTung()
        {
            var yeuCau = SettingDataService.SettingDataItem("DS_SOTHAM_TUCACHTHAMGIATOTUNG_YEUCAU", "");
            var tranhChap = SettingDataService.SettingDataItem("DS_SOTHAM_TUCACHTHAMGIATOTUNG_TRANHCHAP", "");

            var selectList = yeuCau.Union(tranhChap);

            return new SelectList(selectList, "Text", "Text", "");
        }

        public string EncryptName(string name)
        {
            return name.Remove(name.LastIndexOf(" ") + 2);
        }

        public string EncryptAddress(string address)
        {
            //Phạm Thanh Hồng Số 62B, đường Trương Phùng Xuân, khóm 4, phường 8, thành phố Cà Mau, tỉnh Cà Mau.
            //Lê Văn Cang Ấp Vồ Dơi, xã Trần Hợi, huyện Trần Văn Thời, tỉnh Cà Mau.
            //Lê Ngọc Diễu Ấp Vồ Dơi, xã Trần Hợi, huyện Trần Văn Thời, tỉnh Cà Mau.
            //Lưu Chí Lam Ấp Rạch Lăng, xã Lợi An, huyện Trần Văn Thời, tỉnh Cà Mau.
            string encryptAddress = "";
            string[] tokens = address.Split(',');
            for (int i = 0; i < tokens.Count(); i++)
            {
                string token = tokens[i].Trim();
                if (token.Equals(tokens.Last()))
                {
                    encryptAddress += token;
                }
                else if (token.IndexOf("thành phố") != -1)
                {
                    encryptAddress += token.Remove(token.IndexOf(" ", 6) + 2) + ", ";
                }
                else if (!int.TryParse(token[token.IndexOf(" ") + 1].ToString(), out int x))
                {
                    encryptAddress += token.Remove(token.IndexOf(" ") + 2) + ", ";
                }
                else
                {
                    encryptAddress += token + ", ";
                }
            }
            return encryptAddress;
        }

        public string GetFirstName(string fullName)
        {
            string firstName = fullName.Substring(fullName.LastIndexOf(" ") + 1);
            return firstName;
        }
    }
}