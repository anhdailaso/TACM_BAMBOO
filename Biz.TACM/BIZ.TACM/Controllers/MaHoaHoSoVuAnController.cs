using Biz.TACM.IServices;
using Biz.TACM.Models.ViewModel.NhanDon;
using Biz.TACM.Models.ViewModel.Shared;
using Biz.TACM.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Biz.Lib.TACM.Resources.Resources;
using System.Web.Mvc;

namespace Biz.TACM.Controllers
{
    public class MaHoaHoSoVuAnController : BizController
    {
        private IMaHoaHoSoVuAnService _maHoaHoSoVuAnService;
        private IMaHoaHoSoVuAnService MaHoaHoSoVuAnService => _maHoaHoSoVuAnService ?? (_maHoaHoSoVuAnService = new MaHoaHoSoVuAnService());

        public static List<DuongSuViewModel> DanhSachDuongSu = new List<DuongSuViewModel>();
        public static int STT = 0;
        private static float fileSize = 5f; //Waiting for value from Setting

        // GET: MaHoaHoSoVuAn
        public ActionResult Index()
        {
            ViewBag.listTuCachThamGiaToTung = MaHoaHoSoVuAnService.DanhSachTuCachThamGiaToTung();
            return View();
        }

        [HttpPost]
        public ActionResult Encrypted(string fileName, string folderPath)
        {
            string[] outputFiles = MaHoaHoSoVuAnService.TestEditingFileWord(fileName, folderPath, DanhSachDuongSu);

            return Json(new JsonResponseViewModel(true, new string[] { }, new { outputFileName = outputFiles[0], pdfOutputFileName = outputFiles[1] }));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDuongSu(DuongSuViewModel duongSu)
        {
            STT++;
            duongSu.STT = STT;
            DanhSachDuongSu.Add(duongSu);

            return RedirectToAction("Index");
        }

        public ActionResult BangDanhSachDuongSu()
        {
            return Json(new { data = DanhSachDuongSu }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UploadFile()
        {
            string[] allowFileType = { ".doc", ".docx" };

            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase file = files[0];
                    string fname;
                    string folderPath;

                    // Checking for Internet Explorer  
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;
                    }

                    if (!allowFileType.Contains(Path.GetExtension(fname).ToLower()))
                    {
                        return Json(new { isSuccess = false, msg =Setting.FILEHOTRO}); //Waiting for value from Setting
                    }

                    if (file.ContentLength / 1024f / 1024f > fileSize)
                    {
                        return Json(new { isSuccess = false, msg = Setting.DUNGLUONGTOIDA }); //Waiting for value from Setting
                    }

                    fname = string.Concat(
                        Path.GetFileNameWithoutExtension(fname),
                        "_",
                        DateTime.Now.ToString("ddMMyyyyHHmmssfff"),
                        Path.GetExtension(fname)
                    );

                    folderPath = Server.MapPath("~/FileManagement/HoSoVuAn/");

                    // Get the complete folder path and store the file inside it.  
                    file.SaveAs(Path.Combine(folderPath, fname));

                    // Returns message that successfully uploaded  
                    return Json(new JsonResponseViewModel(true, new string[] { }, new { fileName = fname, folderPath }));
                }
                catch (Exception ex)
                {
                    return Json(new { isSuccess = false, msg = Setting.THONGBAOLOI + ex.Message });
                }
            }
            else
            {
                return Json(new { isSuccess = false, msg = Setting.THONGBAO_KHONGCOTEP_TAILEN }); //Waiting for value from Setting
            }
        }

        public ActionResult DownloadFile(string filePath)
        {
            FileInfo file = new FileInfo(filePath);
            if (file.Exists)
            {
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.AddHeader("Content-Disposition", "attachment; filename=\"" + file.Name + "\"");
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.ContentType = "Application/msword";
                Response.Flush();
                Response.TransmitFile(file.FullName);
                Response.End();
            }

            ViewBag.listTuCachThamGiaToTung = MaHoaHoSoVuAnService.DanhSachTuCachThamGiaToTung();
            return new EmptyResult();
        }
    }
}