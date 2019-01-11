using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReportManagement;
using Biz.TACM.Models.ViewModel.MauIn;
using Biz.TACM.Models.ViewModel.Shared;
using Biz.TACM.IServices;
using Biz.TACM.Services;
using System.IO;
using System.Net;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Biz.Lib.Helpers;
using Biz.Lib.TACM.Resources.Resources;
using Biz.TACM.Enums;
using DocumentFormat.OpenXml.Packaging;
using Biz.Lib.TACM.MauIn.Model;
using DocumentFormat.OpenXml;

namespace Biz.TACM.Controllers
{
    public class MauInController : BizController
    {
        private IMauInService _mauInService;
        private IMauInService MauInService => _mauInService ?? (_mauInService = new MauInService());

        private ISettingDataService _settingDataService;
        private ISettingDataService SettingDataService => _settingDataService ?? (_settingDataService = new SettingDataService());

        private IMaHoaHoSoVuAnService _maHoaHoSoVuAnService;
        private IMaHoaHoSoVuAnService MaHoaHoSoVuAnService => _maHoaHoSoVuAnService ?? (_maHoaHoSoVuAnService = new MaHoaHoSoVuAnService());

        private IKetQuaXetXuService _ketQuaXetXuService;
        private IKetQuaXetXuService KetQuaXetXuService { get { return _ketQuaXetXuService ?? (_ketQuaXetXuService = new KetQuaXetXuService()); } }

        // GET: MauIn
        public ActionResult DownloadFile(string filePath)
        {
            //Show pdf file to browser
            WebClient webClient = new WebClient();
            Byte[] fileBuffer = webClient.DownloadData(filePath);
            if (fileBuffer != null)
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-length", fileBuffer.Length.ToString());
                Response.BinaryWrite(fileBuffer);
            }
            return new EmptyResult();
        }

        public ActionResult CreatePDFFile(object mauInObject, string filePath, string viewName, string maHoSo, int hoSoVuAnId, int? soMauIn = null)
        {
            if (mauInObject != null)
            {
                HtmlViewRenderer htmlViewRender = new HtmlViewRenderer();

                //Create pdf file
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                using (iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 85.05f, 42.525f, 56.7f, 56.7f)) //1cm = 28.35pt
                using (PdfWriter writer = PdfWriter.GetInstance(doc, fs))
                {
                    doc.Open();
                    writer.Open();
                    var htmlText = htmlViewRender.RenderViewToString(this, viewName, mauInObject);

                    using (var msHtml = new MemoryStream(Encoding.UTF8.GetBytes(htmlText)))
                    {
                        XMLWorkerFontProvider fontImp = new XMLWorkerFontProvider(XMLWorkerFontProvider.DONTLOOKFORFONTS);
                        fontImp.Register(Server.MapPath("~/Content/fonts/mauin/TIMES.TTF"), "Times New Roman");
                        fontImp.Register(Server.MapPath("~/Content/fonts/mauin/TIMESBD.TTF"), "Times New Roman Bold");
                        fontImp.Register(Server.MapPath("~/Content/fonts/mauin/TIMESBI.TTF"), "Times New Roman Bold Italic");
                        fontImp.Register(Server.MapPath("~/Content/fonts/mauin/TIMESI.TTF"), "Times New Roman Italic");

                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msHtml, null, Encoding.UTF8, fontImp);
                    }
                    doc.Close();
                    writer.Close();
                }

                //Luu lich su in
                var lichSuInViewModel = new LichSuInViewModel
                {
                    HoSoVuAnID = hoSoVuAnId,
                    SoMauIn = soMauIn,
                    DuongDanFileIn = filePath,
                    NhomAn=GetAnSessionInfo().MaNhomAn
                };
                MauInService.LuuLichSuIn(lichSuInViewModel);
                return Json(new { success = true, filePath }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { success = false, filePath }, JsonRequestBehavior.AllowGet);
        }
                
        public ActionResult CreateDocFile(object mauInObject, string viewName)
        {
            HtmlViewRenderer htmlViewRender = new HtmlViewRenderer();
            var htmlText = htmlViewRender.RenderViewToString(this, viewName, mauInObject);

            Response.Clear();
            Response.Charset = "";
            Response.ContentType = "application/msword";
            string strFileName = "GenerateDocument" + ".doc";
            Response.AddHeader("Content-Disposition", "inline;filename=" + strFileName);
            StringBuilder strHTMLContent = new StringBuilder();
            strHTMLContent.Append(htmlText);
            Response.Write(strHTMLContent);
            Response.End();
            Response.Flush();

            return new EmptyResult();
        }

        public ActionResult MauInSo24(int hoSoVuAnId)
        {
            string folder = Server.MapPath(string.Format(Setting.MAU_IN, DateTime.Now.Year, DateTime.Now.Month));
            if (!Directory.Exists((folder)))
            {
                Directory.CreateDirectory(folder);
            }
            string filePath = Path.Combine(folder, string.Format(@"GiayXacNhanDaNhanDonYeuCauKhoiKien_{0}.pdf", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));

            var mauInObject = MauInService.ChiTietMauInSo24(hoSoVuAnId);
            string nhomAn = GetAnSessionInfo().MaNhomAn;
            if (nhomAn == "HC")
                return CreatePDFFile(mauInObject, filePath, "HC/_MauInSo24", mauInObject.MaHoSo, hoSoVuAnId, 24);
            return CreatePDFFile(mauInObject, filePath, "_MauInSo24", mauInObject.MaHoSo, hoSoVuAnId, 24);
            //return CreateDocFile(mauInObject, "_MauInSo24");
        }

        public ActionResult MauInSo29(int hoSoVuAnId)
        {
            string folder = Server.MapPath(string.Format(Setting.MAU_IN, DateTime.Now.Year, DateTime.Now.Month));
            if (!Directory.Exists((folder)))
            {
                Directory.CreateDirectory(folder);
            }
            string filePath = Path.Combine(folder, string.Format(@"ThongBaoNopTienTamUngLePhiAnPhi_{0}.pdf", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));

            var mauInObject = MauInService.DuLieuMauInSo29(hoSoVuAnId);
            //return RedirectToAction("CreatePDFFile", new{mauInObject, filePath, viewName = "_MauInSo29", maHoSo = mauInObject.MaHoSo, hoSoVuAnId, soMauIn = 29}) ;
            string nhomAn = GetAnSessionInfo().MaNhomAn;
            if (nhomAn == "HC")
                return CreatePDFFile(mauInObject, filePath, "HC/_MauInSo29", mauInObject.MaHoSo, hoSoVuAnId, 29);
            return CreatePDFFile(mauInObject, filePath, "_MauInSo29", mauInObject.MaHoSo, hoSoVuAnId, 29);
        }

        public ActionResult MauInSo30(int hoSoVuAnId)
        {
            string folder = Server.MapPath(string.Format(Setting.MAU_IN, DateTime.Now.Year, DateTime.Now.Month));
            if (!Directory.Exists((folder)))
            {
                Directory.CreateDirectory(folder);  
            }
            string filePath = Path.Combine(folder, string.Format(@"ThongBaoVeViecThuLyViecYeuCauVuAn_{0}.pdf", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));

            var mauInObject = MauInService.ChiTietMauInSo30(hoSoVuAnId);
            string nhomAn = GetAnSessionInfo().MaNhomAn;
            if (nhomAn == "AD")
            {
                filePath = Path.Combine(folder, string.Format(@"ThongBaoVeViecThuLyViecYeuCau_{0}.pdf", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                return CreatePDFFile(mauInObject, filePath, "AD/_MauInSo30", mauInObject.MaHoSo, hoSoVuAnId, 30);
            }
            if (nhomAn == "HC")
                return CreatePDFFile(mauInObject, filePath, "HC/_MauInSo30", mauInObject.MaHoSo, hoSoVuAnId, 30);
            if (nhomAn == "KT")
                return CreatePDFFile(mauInObject, filePath, "KT/_MauInSo30", mauInObject.MaHoSo, hoSoVuAnId, 30);
            if (nhomAn == "LD")
                return CreatePDFFile(mauInObject, filePath, "LD/_MauInSo30", mauInObject.MaHoSo, hoSoVuAnId, 30);
            if (nhomAn == "HN")
                return CreatePDFFile(mauInObject, filePath, "HN/_MauInSo30", mauInObject.MaHoSo, hoSoVuAnId, 30);
            //default la dan su
            return CreatePDFFile(mauInObject, filePath, "_MauInSo30", mauInObject.MaHoSo, hoSoVuAnId, 30);
            //return PartialView("../MauIn/_MauInSo30", mauInObject);
        }

        public ActionResult MauInSo47(int hoSoVuAnId)
        {
            string folder = Server.MapPath(string.Format(Setting.MAU_IN, DateTime.Now.Year, DateTime.Now.Month));
            if (!Directory.Exists((folder)))
            {
                Directory.CreateDirectory(folder);
            }
            string filePath = Path.Combine(folder, string.Format(@"QuyetDinhMoPhienHopDuaVuAnRaXetXu_{0}.pdf", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));

            var mauInObject = MauInService.DuLieuMauInSo47(hoSoVuAnId);
            string nhomAn = GetAnSessionInfo().MaNhomAn;
            if (nhomAn == "HS")
            {
                filePath = Path.Combine(folder, string.Format(@"QuyetDinhDuaVuAnRaXetXu_{0}.pdf", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                return CreatePDFFile(mauInObject, filePath, "HS/_MauInSo47", mauInObject.MaHoSo, hoSoVuAnId, 47);
            } 
            if (nhomAn == "HS")
            {
                filePath = Path.Combine(folder, string.Format(@"QuyetDinhDuaVuAnRaXetXu_{0}.pdf", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                return CreatePDFFile(mauInObject, filePath, "HS/_MauInSo47", mauInObject.MaHoSo, hoSoVuAnId, 47);
            }  
            if (nhomAn == "AD")
            {
                filePath = Path.Combine(folder, string.Format(@"QuyetDinhMoPhienHop_{0}.pdf", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                return CreatePDFFile(mauInObject, filePath, "AD/_MauInSo47", mauInObject.MaHoSo, hoSoVuAnId, 47);
            }  
            if (nhomAn == "HC")
            {
                filePath = Path.Combine(folder, string.Format(@"QuyetDinhDuaVuAnRaXetXu_{0}.pdf", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                return CreatePDFFile(mauInObject, filePath, "HC/_MauInSo47", mauInObject.MaHoSo, hoSoVuAnId, 47);
            }   
            if (nhomAn == "KT")
                return CreatePDFFile(mauInObject, filePath, "KT/_MauInSo47", mauInObject.MaHoSo, hoSoVuAnId, 47);
            if (nhomAn == "LD")
                return CreatePDFFile(mauInObject, filePath, "LD/_MauInSo47", mauInObject.MaHoSo, hoSoVuAnId, 47);
            if (nhomAn == "HN")
                return CreatePDFFile(mauInObject, filePath, "HN/_MauInSo47", mauInObject.MaHoSo, hoSoVuAnId, 47);
            //default la dan su
            return CreatePDFFile(mauInObject, filePath, "_MauInSo47", mauInObject.MaHoSo, hoSoVuAnId, 47);
        }

        public ActionResult MauInSo47PT(int hoSoVuAnId)
        {
            string folder = Server.MapPath(string.Format(Setting.MAU_IN, DateTime.Now.Year, DateTime.Now.Month));
            if (!Directory.Exists((folder)))
            {
                Directory.CreateDirectory(folder);
            }
            string filePath = Path.Combine(folder, string.Format(@"QuyetDinhMoPhienHopDuaVuAnRaXetXuPhucTham_{0}.pdf", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));

            var mauInObject = MauInService.DuLieuMauInSo47PT(hoSoVuAnId);
            string nhomAn = GetAnSessionInfo().MaNhomAn;
            if (nhomAn == "HS")
            {
                filePath = Path.Combine(folder, string.Format(@"QuyetDinhDuaVuAnRaXetXuPhucTham_{0}.pdf", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                return CreatePDFFile(mauInObject, filePath, "HS/_MauInQDXXPhucTham", mauInObject.MaHoSo, hoSoVuAnId, null);
            }
            if (nhomAn == "AD")
            {
                filePath = Path.Combine(folder, string.Format(@"QuyetDinhMoPhienHopPhucTham_{0}.pdf", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                return CreatePDFFile(mauInObject, filePath, "AD/_MauInQDXXPhucTham", mauInObject.MaHoSo, hoSoVuAnId, null);
            }
            if (nhomAn == "HC")
                return CreatePDFFile(mauInObject, filePath, "HC/_MauInQDXXPhucTham", mauInObject.MaHoSo, hoSoVuAnId, null);
            if (nhomAn == "KT")
                return CreatePDFFile(mauInObject, filePath, "KT/_MauInQDXXPhucTham", mauInObject.MaHoSo, hoSoVuAnId, null);
            if (nhomAn == "LD")
                return CreatePDFFile(mauInObject, filePath, "LD/_MauInQDXXPhucTham", mauInObject.MaHoSo, hoSoVuAnId, null);
            if (nhomAn == "HN")
                return CreatePDFFile(mauInObject, filePath, "HN/_MauInQDXXPhucTham", mauInObject.MaHoSo, hoSoVuAnId, null);
            //default la dan su
            return CreatePDFFile(mauInObject, filePath, "_MauInQDXXPhucTham", mauInObject.MaHoSo, hoSoVuAnId, null);
        }

        public ActionResult MauInSo61(int hoSoVuAnId)
        {
            string folder = Server.MapPath(string.Format(Setting.MAU_IN, DateTime.Now.Year, DateTime.Now.Month));
            if (!Directory.Exists((folder)))
            {
                Directory.CreateDirectory(folder);
            }
            string filePath = Path.Combine(folder, string.Format(@"ThongBaoNopTienTamUngLePhiAnPhiPhucTham_{0}.pdf", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));

            var mauInObject = MauInService.DuLieuMauInSo61(hoSoVuAnId);
            string nhomAn = GetAnSessionInfo().MaNhomAn;
            if (nhomAn == "HC")
                return CreatePDFFile(mauInObject, filePath, "HC/_MauInSo61", mauInObject.MaHoSo, hoSoVuAnId, 61);
            return CreatePDFFile(mauInObject, filePath, "_MauInSo61", mauInObject.MaHoSo, hoSoVuAnId, 61);
        }

        public ActionResult MauInSo65(int hoSoVuAnId)
        {
            string folder = Server.MapPath(string.Format(Setting.MAU_IN, DateTime.Now.Year, DateTime.Now.Month));
            if (!Directory.Exists((folder)))
            {
                Directory.CreateDirectory(folder);
            }
            string filePath = Path.Combine(folder, string.Format(@"ThongBaoVeViecThuLyDeXetXuPhucTham_{0}.pdf", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));

            var mauInObject = MauInService.DuLieuMauInSo65(hoSoVuAnId);
            string nhomAn = GetAnSessionInfo().MaNhomAn;
            if (nhomAn == "HC")
                return CreatePDFFile(mauInObject, filePath, "HC/_MauInSo65", mauInObject.MaHoSo, hoSoVuAnId, 65);
            if (nhomAn == "KT")
                return CreatePDFFile(mauInObject, filePath, "KT/_MauInSo65", mauInObject.MaHoSo, hoSoVuAnId, 65);
            if (nhomAn == "LD")
                return CreatePDFFile(mauInObject, filePath, "LD/_MauInSo65", mauInObject.MaHoSo, hoSoVuAnId, 65);
            if (nhomAn == "HN")
                return CreatePDFFile(mauInObject, filePath, "HN/_MauInSo65", mauInObject.MaHoSo, hoSoVuAnId, 65);
            //default la dan su
            return CreatePDFFile(mauInObject, filePath, "_MauInSo65", mauInObject.MaHoSo, hoSoVuAnId, 65);
        }

        public ActionResult MauInQuyetDinhPhanCongThamPhanThuLy(int hoSoVuAnId)
        {
            string folder = Server.MapPath(string.Format(Setting.MAU_IN, DateTime.Now.Year, DateTime.Now.Month));
            if (!Directory.Exists((folder)))
            {
                Directory.CreateDirectory(folder);
            }
            string nhomAn = GetAnSessionInfo().MaNhomAn;
            int giaiDoan = GetAnSessionInfo().GiaiDoanId;
            var mauInObject = MauInService.DuLieuMauInQuyetDinhPCTP(hoSoVuAnId, giaiDoan);
            if (nhomAn=="HS")
            {
                string filePath = Path.Combine(folder, string.Format(@"QuyetDinhPhanCongThamPhanHS_{0}.pdf", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                return CreatePDFFile(mauInObject, filePath, "HS/_MauInQuyetDinhPCTP_HS", mauInObject.MaHoSo, hoSoVuAnId, null);
            }
            if (giaiDoan == 2)
            {
                string filePath = Path.Combine(folder, string.Format(@"QuyetDinhPhanCongThamPhanThuLyPhucTham_{0}.pdf", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));

                if (nhomAn == "AD")
                    return CreatePDFFile(mauInObject, filePath, "AD/_MauInQuyetDinhPCTPThuLyPhucTham", mauInObject.MaHoSo, hoSoVuAnId, null);
                if (nhomAn == "HC")
                    return CreatePDFFile(mauInObject, filePath, "HC/_MauInQuyetDinhPCTPThuLyPhucTham", mauInObject.MaHoSo, hoSoVuAnId, null);
                if (nhomAn == "KT")
                    return CreatePDFFile(mauInObject, filePath, "KT/_MauInQuyetDinhPCTPThuLyPhucTham", mauInObject.MaHoSo, hoSoVuAnId, null);
                if (nhomAn == "HN")
                    return CreatePDFFile(mauInObject, filePath, "HN/_MauInQuyetDinhPCTPThuLyPhucTham", mauInObject.MaHoSo, hoSoVuAnId, null);
                if (nhomAn == "LD")
                    return CreatePDFFile(mauInObject, filePath, "LD/_MauInQuyetDinhPCTPThuLyPhucTham", mauInObject.MaHoSo, hoSoVuAnId, null);
                //default la dan su
                return CreatePDFFile(mauInObject, filePath, "_MauInQuyetDinhPCTPThuLyPhucTham", mauInObject.MaHoSo, hoSoVuAnId, null);
            }
            else //default la so tham
            {
                string filePath = Path.Combine(folder, string.Format(@"QuyetDinhPhanCongThamPhanThuLySoTham_{0}.pdf", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                if (nhomAn == "AD")
                    return CreatePDFFile(mauInObject, filePath, "AD/_MauInQuyetDinhPCTPThuLySoTham", mauInObject.MaHoSo, hoSoVuAnId, null);
                if (nhomAn == "HC")
                    return CreatePDFFile(mauInObject, filePath, "HC/_MauInQuyetDinhPCTPThuLySoTham", mauInObject.MaHoSo, hoSoVuAnId, null);
                if (nhomAn == "KT")
                    return CreatePDFFile(mauInObject, filePath, "KT/_MauInQuyetDinhPCTPThuLySoTham", mauInObject.MaHoSo, hoSoVuAnId, null);
                if (nhomAn == "HN")
                    return CreatePDFFile(mauInObject, filePath, "HN/_MauInQuyetDinhPCTPThuLySoTham", mauInObject.MaHoSo, hoSoVuAnId, null);
                if (nhomAn == "LD")
                    return CreatePDFFile(mauInObject, filePath, "LD/_MauInQuyetDinhPCTPThuLySoTham", mauInObject.MaHoSo, hoSoVuAnId, null);
                //default la dan su
                return CreatePDFFile(mauInObject, filePath, "_MauInQuyetDinhPCTPThuLySoTham", mauInObject.MaHoSo, hoSoVuAnId, null);
            }
        }

        public ActionResult MauInQuyetDinhPhanCongThamPhanGiaiQuyetDon(int hoSoVuAnId)
        {
            string folder = Server.MapPath(string.Format(Setting.MAU_IN, DateTime.Now.Year, DateTime.Now.Month));
            if (!Directory.Exists((folder)))
            {
                Directory.CreateDirectory(folder);
            }
            string filePath = Path.Combine(folder, string.Format(@"QuyetDinhPhanCongThamPhanGiaiQuyetDon_{0}.pdf", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));

            var mauInObject = MauInService.DuLieuMauInQuyetDinhPCTP(hoSoVuAnId, GetAnSessionInfo().GiaiDoanId);
            //default la dan su
            return CreatePDFFile(mauInObject, filePath, "_MauInQuyetDinhPCTPGiaiQuyetDon", mauInObject.MaHoSo, hoSoVuAnId, null);
        }

        public ActionResult MauInQuyetDinhPhanCongHoiTham(int hoSoVuAnId)
        {
            string folder = Server.MapPath(string.Format(Setting.MAU_IN, DateTime.Now.Year, DateTime.Now.Month));
            if (!Directory.Exists((folder)))
            {
                Directory.CreateDirectory(folder);
            }
            string filePath = Path.Combine(folder, string.Format(@"QuyetDinhPhanCongHoiThamHS_{0}.pdf", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));

            var mauInObject = MauInService.DuLieuMauInQuyetDinhPCTP(hoSoVuAnId, GetAnSessionInfo().GiaiDoanId);
            //default la dan su
            return CreatePDFFile(mauInObject, filePath, "HS/_MauInQuyetDinhPCHT_HS", mauInObject.MaHoSo, hoSoVuAnId, null);
        }

        public ActionResult MauInQuyetDinhPhanCongThuKy(int hoSoVuAnId)
        {
            string folder = Server.MapPath(string.Format(Setting.MAU_IN, DateTime.Now.Year, DateTime.Now.Month));
            if (!Directory.Exists((folder)))
            {
                Directory.CreateDirectory(folder);
            }
            string filePath = Path.Combine(folder, string.Format(@"QuyetDinhPhanCongThuKyHS_{0}.pdf", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));

            var mauInObject = MauInService.DuLieuMauInQuyetDinhPCTP(hoSoVuAnId, GetAnSessionInfo().GiaiDoanId);
            //default la dan su
            return CreatePDFFile(mauInObject, filePath, "HS/_MauInQuyetDinhPCTK_HS", mauInObject.MaHoSo, hoSoVuAnId, null);
        }

        public ActionResult MauInQuyetDinhGiaHanCBXX(int hoSoVuAnId)
        {
            string folder = Server.MapPath(string.Format(Setting.MAU_IN, DateTime.Now.Year, DateTime.Now.Month));
            if (!Directory.Exists((folder)))
            {
                Directory.CreateDirectory(folder);
            }
            string filePath = Path.Combine(folder, string.Format(@"QuyetDinhGiaHanChuanBiXetXu_{0}.pdf", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));

            var mauInObject = MauInService.ChiTietMauInGiaHanCBXX(hoSoVuAnId, GetAnSessionInfo().GiaiDoanId);
            mauInObject.Giaidoan = GetAnSessionInfo().GiaiDoanId;
            return CreatePDFFile(mauInObject, filePath, "HS/_MauInQuyetDinhGiaHanCBXX", mauInObject.MaHoSo, hoSoVuAnId, null);
        }

        public ActionResult MauInQuyetDinhTamHoanPhienToa(int hoSoVuAnId, string loai)
        {
            string folder = Server.MapPath(string.Format(Setting.MAU_IN, DateTime.Now.Year, DateTime.Now.Month));
            if (!Directory.Exists((folder)))
            {
                Directory.CreateDirectory(folder);
            }
            string filePath = Path.Combine(folder, string.Format(@"QuyetDinhTamHoanPhienToa_ChanhAn_{0}.pdf", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));

            var mauInObject = MauInService.ChiTietMauInTamHoan(hoSoVuAnId, GetAnSessionInfo().GiaiDoanId);
            mauInObject.GiaiDoan = GetAnSessionInfo().GiaiDoanId;
            if(loai=="hoidong")
            {
                filePath = Path.Combine(folder, string.Format(@"QuyetDinhTamHoanPhienToa_HoiDongXetXu_{0}.pdf", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                return CreatePDFFile(mauInObject, filePath, "HS/_MauInQuyetDinhTamHoanHD", mauInObject.MaHoSo, hoSoVuAnId, null);
            }
                
            else
                return CreatePDFFile(mauInObject, filePath, "HS/_MauInQuyetDinhTamHoanCA", mauInObject.MaHoSo, hoSoVuAnId, null);
        }

        public ActionResult MauInQuyetDinhDinhChi(int hoSoVuAnId, string loai)
        {
            string folder = Server.MapPath(string.Format(Setting.MAU_IN, DateTime.Now.Year, DateTime.Now.Month));
            if (!Directory.Exists((folder)))
            {
                Directory.CreateDirectory(folder);
            }
            string filePath = Path.Combine(folder, string.Format(@"QuyetDinh_DinhChi_{0}.pdf", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));

            var mauInObject = MauInService.ChiTietMauInQuyetDinhDinhChi(hoSoVuAnId, loai);
            mauInObject.Loai = loai;
            if (loai == "tamdinhchi")
            {
                filePath = Path.Combine(folder, string.Format(@"QuyetDinh_TamDinhChi_{0}.pdf", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
            }
                return CreatePDFFile(mauInObject, filePath, "AD/_MauInQuyetDinhDinhChi", mauInObject.MaHoSo, hoSoVuAnId, null);
        }

        public ActionResult MauInLenhTrichXuat(int hoSoVuAnId)
        {
            string folder = Server.MapPath(string.Format(Setting.MAU_IN, DateTime.Now.Year, DateTime.Now.Month));
            if (!Directory.Exists((folder)))
            {
                Directory.CreateDirectory(folder);
            }
            string filePath = Path.Combine(folder, string.Format(@"LenhTrichXuat_{0}.pdf", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));

            var mauInObject = MauInService.ChiTietMauInLenhTrichXuat(hoSoVuAnId, GetAnSessionInfo().GiaiDoanId);
            mauInObject.GiaiDoan = GetAnSessionInfo().GiaiDoanId;
            return CreatePDFFile(mauInObject, filePath, "HS/_MauInLenhTrichXuat", mauInObject.MaHoSo, hoSoVuAnId, null);
        }

        public ActionResult MauInBienBanGiaoNhan(int hoSoVuAnId)
        {
            string folder = Server.MapPath(string.Format(Setting.MAU_IN, DateTime.Now.Year, DateTime.Now.Month));
            if (!Directory.Exists((folder)))
            {
                Directory.CreateDirectory(folder);
            }
            string filePath = Path.Combine(folder, string.Format(@"BienBanGiaoNhan_{0}.pdf", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));

            var mauInObject = MauInService.ChiTietMauInBienBanGiaoNhan(hoSoVuAnId);
            return CreatePDFFile(mauInObject, filePath, "HS/_MauInBienBanGiaoNhan", mauInObject.MaHoSo, hoSoVuAnId, null);
        }

        public ActionResult MauInGiayTrieuTap(int hoSoVuAnId, string loai)
        {
            string folder = Server.MapPath(string.Format(Setting.MAU_IN, DateTime.Now.Year, DateTime.Now.Month));
            if (!Directory.Exists((folder)))
            {
                Directory.CreateDirectory(folder);
            }
            string filePath = Path.Combine(folder, string.Format(@"GiayTrieuTap_{0}.pdf", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));

            MauInGiayTrieuTapViewModel mauInObject = new MauInGiayTrieuTapViewModel();
            mauInObject = MauInService.DuLieuMauInGiayTrieuTap(hoSoVuAnId);
            mauInObject.GiaiDoan = GetAnSessionInfo().GiaiDoanId;
            if(loai=="bicao")
            {
                filePath = Path.Combine(folder, string.Format(@"GiayTrieuTapBiCao_{0}.pdf", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                return CreatePDFFile(mauInObject, filePath, "HS/_MauInGiayTrieuTapBiCao", mauInObject.MaHoSo, hoSoVuAnId, null);
            }
            string nhomAn = GetAnSessionInfo().MaNhomAn;
            if (nhomAn == "AD")
                return CreatePDFFile(mauInObject, filePath, "AD/_MauInGiayTrieuTap", mauInObject.MaHoSo, hoSoVuAnId, null);
            if (nhomAn == "KT")
                return CreatePDFFile(mauInObject, filePath, "KT/_MauInGiayTrieuTap", mauInObject.MaHoSo, hoSoVuAnId, null);
            if (nhomAn == "LD")
                return CreatePDFFile(mauInObject, filePath, "LD/_MauInGiayTrieuTap", mauInObject.MaHoSo, hoSoVuAnId, null);
            if (nhomAn == "HN")
                return CreatePDFFile(mauInObject, filePath, "HN/_MauInGiayTrieuTap", mauInObject.MaHoSo, hoSoVuAnId, null);
            if (nhomAn == "HC")
                return CreatePDFFile(mauInObject, filePath, "HC/_MauInGiayTrieuTap", mauInObject.MaHoSo, hoSoVuAnId, null);
            if (nhomAn == "HS")
                return CreatePDFFile(mauInObject, filePath, "HS/_MauInGiayTrieuTap", mauInObject.MaHoSo, hoSoVuAnId, null);
            //default la dan su
            return CreatePDFFile(mauInObject, filePath, "_MauInGiayTrieuTap", mauInObject.MaHoSo, hoSoVuAnId, null);
        }

        public ActionResult MauInGXNDaNhanKhangCao(int hoSoVuAnId)
        {
            string folder = Server.MapPath(string.Format(Setting.MAU_IN, DateTime.Now.Year, DateTime.Now.Month));
            if (!Directory.Exists((folder)))
            {
                Directory.CreateDirectory(folder);
            }
            string filePath = Path.Combine(folder, string.Format(@"GiayXacNhanDaNhanDonKhangCao_{0}.pdf", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));

            MauInGXNKhangCaoViewModel mauInObject = new MauInGXNKhangCaoViewModel();
            mauInObject = MauInService.ChiTietGXNKhangCao(hoSoVuAnId);
            mauInObject.NhomAn = GetAnSessionInfo().MaNhomAn;
            mauInObject.GiaiDoan = GetAnSessionInfo().GiaiDoanId;
            return CreatePDFFile(mauInObject, filePath, "_MauInGXNKhangCao", mauInObject.MaHoSo, hoSoVuAnId, null);
        }

        public ActionResult MauInThongBaoKhangCao(int hoSoVuAnId)
        {
            string folder = Server.MapPath(string.Format(Setting.MAU_IN, DateTime.Now.Year, DateTime.Now.Month));
            if (!Directory.Exists((folder)))
            {
                Directory.CreateDirectory(folder);
            }
            string filePath = Path.Combine(folder, string.Format(@"QuyetDinhThongBaoKhangCao_{0}.pdf", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));

            MauInGXNKhangCaoViewModel mauInObject = new MauInGXNKhangCaoViewModel();
            mauInObject = MauInService.ChiTietGXNKhangCao(hoSoVuAnId);
            mauInObject.NhomAn = GetAnSessionInfo().MaNhomAn;
            mauInObject.GiaiDoan = GetAnSessionInfo().GiaiDoanId;
            return CreatePDFFile(mauInObject, filePath, "HS/_MauInThongBaoKhangCao", mauInObject.MaHoSo, hoSoVuAnId, null);
        }

        public ActionResult MauInQuyetDinhTamGiam(int hoSoVuAnId, int mauInSo)
        {
            string folder = Server.MapPath(string.Format(Setting.MAU_IN, DateTime.Now.Year, DateTime.Now.Month));
            if (!Directory.Exists((folder)))
            {
                Directory.CreateDirectory(folder);
            }
            string filePath = Path.Combine(folder, string.Format(@"QuyetDinhTamGiam_so "+mauInSo+"_{0}.pdf", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));

            MauInQDTamGiamViewModel mauInObject = new MauInQDTamGiamViewModel();
            mauInObject = MauInService.ChiTietMauInTamGiam_4_5_9(hoSoVuAnId, GetAnSessionInfo().GiaiDoanId, GetAnSessionInfo().CongDoanId, mauInSo);
            mauInObject.NhomAn = GetAnSessionInfo().MaNhomAn;
            mauInObject.GiaiDoan = GetAnSessionInfo().GiaiDoanId;
            mauInObject.CongDoan = GetAnSessionInfo().CongDoanId;
            mauInObject.MauSo = mauInSo;
            return CreatePDFFile(mauInObject, filePath, "HS/_MauInQuyetDinhTamGiam", mauInObject.MaHoSo, hoSoVuAnId, null);
        }

        public ActionResult MauInBienBanKhangCao(int hoSoVuAnId)
        {
            string folder = Server.MapPath(string.Format(Setting.MAU_IN, DateTime.Now.Year, DateTime.Now.Month));
            if (!Directory.Exists((folder)))
            {
                Directory.CreateDirectory(folder);
            }
            string filePath = Path.Combine(folder, string.Format(@"BienBanKhangCao_{0}.pdf", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));

            MauInGXNKhangCaoViewModel mauInObject = new MauInGXNKhangCaoViewModel();
            mauInObject = MauInService.ChiTietGXNKhangCao(hoSoVuAnId);
            
            return CreatePDFFile(mauInObject, filePath, "HS/_MauInBienBanKhangCao", mauInObject.MaHoSo, hoSoVuAnId, null);
        }

        public ActionResult MauInBiaHoSoNhanDon(int hoSoVuAnId)
        {
            string folder = Server.MapPath(string.Format(Setting.MAU_IN, DateTime.Now.Year, DateTime.Now.Month));
            if (!Directory.Exists((folder)))
            {
                Directory.CreateDirectory(folder);
            }
            string filePath = Path.Combine(folder, string.Format(@"BiaHoSoNhanDon_{0}.pdf", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));

            MauInBiaHoSoNhanDonViewModel mauInObject = new MauInBiaHoSoNhanDonViewModel();
            mauInObject = MauInService.ChiTietMauInBiaHoSoNhanDon(hoSoVuAnId);
            string nhomAn = GetAnSessionInfo().MaNhomAn;
            if (nhomAn == "HC")
                return CreatePDFFile(mauInObject, filePath, "HC/_MauInBiaHoSoNhanDon", mauInObject.MaHoSo, hoSoVuAnId, null);
            return CreatePDFFile(mauInObject, filePath, "_MauInBiaHoSoNhanDon", mauInObject.MaHoSo, hoSoVuAnId, null);
            
        }

        public ActionResult MauInBiaHoSo(int hoSoVuAnId)
        {
            string folder = Server.MapPath(string.Format(Setting.MAU_IN, DateTime.Now.Year, DateTime.Now.Month));
            if (!Directory.Exists((folder)))
            {
                Directory.CreateDirectory(folder);
            }
            string filePath = Path.Combine(folder, string.Format(@"BiaHoSoThuLySoTham_{0}.pdf", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));

            MauInBiaHoSoViewModel mauInObject = new MauInBiaHoSoViewModel();
            mauInObject = MauInService.DuLieuMauInBiaHoSo(hoSoVuAnId);
            mauInObject.GiaiDoan = GetAnSessionInfo().GiaiDoanId;
            string nhomAn = GetAnSessionInfo().MaNhomAn;
            if (nhomAn == "AD")
                return CreatePDFFile(mauInObject, filePath, "AD/_MauInBiaHoSo", mauInObject.MaHoSo, hoSoVuAnId, null);
            if (mauInObject.GiaiDoan == 2)
            {
                filePath = Path.Combine(folder, string.Format(@"BiaHoSoThuLyPhucTham_{0}.pdf", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                if (nhomAn == "KT")
                    return CreatePDFFile(mauInObject, filePath, "KT/_MauInBiaHoSoPhucTham", mauInObject.MaHoSo, hoSoVuAnId, null);
                if (nhomAn == "LD")
                    return CreatePDFFile(mauInObject, filePath, "LD/_MauInBiaHoSoPhucTham", mauInObject.MaHoSo, hoSoVuAnId, null);
                if (nhomAn == "HN")
                    return CreatePDFFile(mauInObject, filePath, "HN/_MauInBiaHoSoPhucTham", mauInObject.MaHoSo, hoSoVuAnId, null);
                if (nhomAn == "HC")
                    return CreatePDFFile(mauInObject, filePath, "HC/_MauInBiaHoSoPhucTham", mauInObject.MaHoSo, hoSoVuAnId, null);
                if (nhomAn == "HS")
                    return CreatePDFFile(mauInObject, filePath, "HS/_MauInBiaHoSoPhucTham", mauInObject.MaHoSo, hoSoVuAnId, null);
                return CreatePDFFile(mauInObject, filePath, "_MauInBiaHoSoPhucTham", mauInObject.MaHoSo, hoSoVuAnId, null);
            }
            else
            {
                if (nhomAn == "HS")
                    return CreatePDFFile(mauInObject, filePath, "HS/_MauInBiaHoSoSoTham", mauInObject.MaHoSo, hoSoVuAnId, null);
                if (nhomAn == "KT")
                    return CreatePDFFile(mauInObject, filePath, "KT/_MauInBiaHoSoSoTham", mauInObject.MaHoSo, hoSoVuAnId, null);
                if (nhomAn == "LD")
                    return CreatePDFFile(mauInObject, filePath, "LD/_MauInBiaHoSoSoTham", mauInObject.MaHoSo, hoSoVuAnId, null);
                if (nhomAn == "HN")
                    return CreatePDFFile(mauInObject, filePath, "HN/_MauInBiaHoSoSoTham", mauInObject.MaHoSo, hoSoVuAnId, null);
                if (nhomAn == "HC")
                    return CreatePDFFile(mauInObject, filePath, "HC/_MauInBiaHoSoSoTham", mauInObject.MaHoSo, hoSoVuAnId, null);
                return CreatePDFFile(mauInObject, filePath, "_MauInBiaHoSoSoTham", mauInObject.MaHoSo, hoSoVuAnId, null);
            }
        }

        public ActionResult DanhSachMauInTheoGiaiDoanVaNhomAn(int? giaiDoan, string nhomAn)
        {
            var danhSachMauIn = MauInService.DanhSachMauInTheoGiaiDoanVaNhomAn(giaiDoan, nhomAn);
            return PartialView("_MauInItem", danhSachMauIn);
        }

        public ActionResult DanhSachMauInSearchByKeyword(string keyword)
        {
            var danhSachMauIn = MauInService.DanhSachMauInSearchByKeyword(keyword);
            return PartialView("_MauInItem", danhSachMauIn);
        }

        public ActionResult DanhSachMauIn(string nhomAn)
        {
            var ToaAn = GetAnSessionInfo().ToaAnId;
            var listNhomAnModel = MauInService.DanhSachNhomAnTheoToaAn(ToaAn);
            var listNhomAn = listNhomAnModel.Select(
            x => new SelectListItem
            {
                Text = x.TenNhomAn,
                Value = x.MaNhomAn
            });
            ViewBag.nhomAn = new SelectList(listNhomAn, "Value", "Text", GetAnSessionInfo().MaNhomAn);
            nhomAn = (nhomAn == null ? "" : GetAnSessionInfo().MaNhomAn);
            var listMauIn = MauInService.DanhSachMauInTheoGiaiDoanVaNhomAn(GetAnSessionInfo().GiaiDoanId, nhomAn);
            var giaiDoan = (XMLUtils.BindData("GiaiDoan"));
            giaiDoan.RemoveAt(giaiDoan.Count-1);
            var listItem = giaiDoan.Select(
                    x => new SelectListItem
                    {
                        Text = x.text,
                        Value = x.value
                    });
            ViewBag.giaiDoanddl = new SelectList(listItem, "Value", "Text", GetAnSessionInfo().GiaiDoanId);
            return PartialView("~/Views/Shared/_MauInPartial.cshtml",listMauIn);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult getDanhSachMauIn(string maNhanVien)
        {
            try
            {
                return Json(JsonResponseViewModel.CreateFail(NotifyMessage.THONGBAO_KHONGCO_MAUIN));
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }

        public ActionResult MauIn24FileDoc(int hoSoVuAnId)
        {
            string folder = Server.MapPath(string.Format(Setting.MAU_IN_DOC, DateTime.Now.Year, DateTime.Now.Month));
            if (!Directory.Exists((folder)))
            {
                Directory.CreateDirectory(folder);
            }
            string folderTemplate = Server.MapPath(Setting.TEMPLATES_MAUIN_FOLDER);
            if (!Directory.Exists((folderTemplate)))
            {
                Directory.CreateDirectory(folderTemplate);
            }
            var nhoman = GetAnSessionInfo().MaNhomAn;
            if (nhoman == Setting.MANHOMAN_HANHCHINH)
            {              
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HC_SOTHAM_GIAYXACNHANDANHANDONKHOIKIEN);
                var mauInObject = MauInService.ChiTietMauInSo24(hoSoVuAnId);
                string filename = Setting.TEMPLATE_MAUIN_HC_SOTHAM_GIAYXACNHANDANHANDONKHOIKIEN.Remove(Setting.TEMPLATE_MAUIN_HC_SOTHAM_GIAYXACNHANDANHANDONKHOIKIEN.IndexOf("."));
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauInHC02Doc(mauInObject, filePath, templatePath);
                if(outputFiles!=null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else if (nhoman == Setting.MANHOMAN_DANSU || nhoman==Setting.MANHOMAN_HONNHAN)
            {
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_DS_SOTHAM_GIAYXACNHANDANHANDONKHOIKIEN);

                var mauInObject = MauInService.ChiTietMauInSo24(hoSoVuAnId);
                string filename = Setting.TEMPLATE_MAUIN_DS_SOTHAM_GIAYXACNHANDANHANDONKHOIKIEN.Remove(Setting.TEMPLATE_MAUIN_DS_SOTHAM_GIAYXACNHANDANHANDONKHOIKIEN.IndexOf("."));
                if(mauInObject.LoaiQuanHe==Setting.LOAIQUANHE_YEUCAU)
                {
                    templatePath= Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_DS_SOTHAM_GIAYXACNHANDANHANDONYEUCAU);
                    filename = Setting.TEMPLATE_MAUIN_DS_SOTHAM_GIAYXACNHANDANHANDONYEUCAU.Remove(Setting.TEMPLATE_MAUIN_DS_SOTHAM_GIAYXACNHANDANHANDONYEUCAU.IndexOf("."));
                }
                if (nhoman == Setting.MANHOMAN_HONNHAN)
                {
                    if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                    {
                        templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HN_SOTHAM_GIAYXACNHANDANHANDONYEUCAU);
                    }
                    filename = filename.Replace(Setting.MANHOMAN_DANSU, Setting.MANHOMAN_HONNHAN);
                }                    
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauIn02GiayXacNhanDaNhanDonDoc(mauInObject, filePath, templatePath);
                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            if (nhoman == Setting.MANHOMAN_KINHTE)
            {
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_KD_SOTHAM_GIAYXACNHANDANHANDONKHOIKIEN);

                var mauInObject = MauInService.ChiTietMauInSo24(hoSoVuAnId);
                string filename = Setting.TEMPLATE_MAUIN_KD_SOTHAM_GIAYXACNHANDANHANDONKHOIKIEN.Remove(Setting.TEMPLATE_MAUIN_KD_SOTHAM_GIAYXACNHANDANHANDONKHOIKIEN.IndexOf("."));
                if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_KD_SOTHAM_GIAYXACNHANDANHANDONYEUCAU);
                    filename = Setting.TEMPLATE_MAUIN_KD_SOTHAM_GIAYXACNHANDANHANDONYEUCAU.Remove(Setting.TEMPLATE_MAUIN_KD_SOTHAM_GIAYXACNHANDANHANDONYEUCAU.IndexOf("."));
                }
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauIn02GiayXacNhanDaNhanDonDoc(mauInObject, filePath, templatePath);
                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            if (nhoman == Setting.MANHOMAN_LAODONG)
            {
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_LD_SOTHAM_GIAYXACNHANDANHANDONKHOIKIEN);

                var mauInObject = MauInService.ChiTietMauInSo24(hoSoVuAnId);
                string filename = Setting.TEMPLATE_MAUIN_LD_SOTHAM_GIAYXACNHANDANHANDONKHOIKIEN.Remove(Setting.TEMPLATE_MAUIN_LD_SOTHAM_GIAYXACNHANDANHANDONKHOIKIEN.IndexOf("."));
                if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_KD_SOTHAM_GIAYXACNHANDANHANDONYEUCAU);
                    filename = Setting.TEMPLATE_MAUIN_KD_SOTHAM_GIAYXACNHANDANHANDONYEUCAU.Remove(Setting.TEMPLATE_MAUIN_KD_SOTHAM_GIAYXACNHANDANHANDONYEUCAU.IndexOf("."));
                    filename = filename.Replace("KD", "LD");
                }
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauIn02GiayXacNhanDaNhanDonDoc(mauInObject, filePath, templatePath);
                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MauInQuyetDinhPCTPDoc(int hoSoVuAnId)
        {
            string folder = Server.MapPath(string.Format(Setting.MAU_IN_DOC, DateTime.Now.Year, DateTime.Now.Month));
            if (!Directory.Exists((folder)))
            {
                Directory.CreateDirectory(folder);
            }
            string folderTemplate = Server.MapPath(Setting.TEMPLATES_MAUIN_FOLDER);
            if (!Directory.Exists((folderTemplate)))
            {
                Directory.CreateDirectory(folderTemplate);
            }
            string pathChuKy = Server.MapPath(Setting.CHUKY_CHANH_AN_TINH);
            int giaidoan = GetAnSessionInfo().GiaiDoanId;
            var nhoman = GetAnSessionInfo().MaNhomAn;
            if (nhoman==Setting.MANHOMAN_HANHCHINH)
            { 
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HC_QUYETDINHPHANCONGTHAMPHANHOITHAM);
                var mauInObject = MauInService.DuLieuMauInQuyetDinhPCTP(hoSoVuAnId, giaidoan);
                string filename = Setting.TEMPLATE_MAUIN_HC_QUYETDINHPHANCONGTHAMPHANHOITHAM.Remove(Setting.TEMPLATE_MAUIN_HC_QUYETDINHPHANCONGTHAMPHANHOITHAM.IndexOf("."));
                if(giaidoan==2)
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HC_QUYETDINHPHANCONGTHAMPHAN);
                    filename = Setting.TEMPLATE_MAUIN_HC_QUYETDINHPHANCONGTHAMPHAN.Remove(Setting.TEMPLATE_MAUIN_HC_QUYETDINHPHANCONGTHAMPHAN.IndexOf("."));
                }
                
                string filePath = Path.Combine(folder, string.Format(filename+@"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauInHCQuyetDinhPCTPHTDoc(mauInObject, filePath, templatePath);

                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else if (nhoman == Setting.MANHOMAN_HINHSU)
            {                
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HS_QUYETDINHPHANCONGTHAMPHAN);
                var mauInObject = MauInService.DuLieuMauInQuyetDinhPCTP(hoSoVuAnId, GetAnSessionInfo().GiaiDoanId);
                string filename = Setting.TEMPLATE_MAUIN_HS_QUYETDINHPHANCONGTHAMPHAN.Remove(Setting.TEMPLATE_MAUIN_HS_QUYETDINHPHANCONGTHAMPHAN.IndexOf("."));               
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauInHSQuyetDinhPCTPDoc(mauInObject, filePath, templatePath);

                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else if (nhoman == Setting.MANHOMAN_DANSU)
            {
                var mauInObject = MauInService.DuLieuMauInQuyetDinhPCTP(hoSoVuAnId, giaidoan);
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_DS_QUYETDINHPHANCONGTHAMPHANGIAIQUYETVUANST);                
                string filename = Setting.TEMPLATE_MAUIN_DS_QUYETDINHPHANCONGTHAMPHANGIAIQUYETVUANST.Remove(Setting.TEMPLATE_MAUIN_DS_QUYETDINHPHANCONGTHAMPHANGIAIQUYETVUANST.IndexOf("."));
                if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_DS_QUYETDINHPHANCONGTHAMPHANGIAIQUYETVUVIECST);
                    filename = Setting.TEMPLATE_MAUIN_DS_QUYETDINHPHANCONGTHAMPHANGIAIQUYETVUVIECST.Remove(Setting.TEMPLATE_MAUIN_DS_QUYETDINHPHANCONGTHAMPHANGIAIQUYETVUVIECST.IndexOf("."));
                }
                if (giaidoan==2)
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_DS_QDPHANCONGTHAMPHANGIAIQUYETVUANPT);
                    filename = Setting.TEMPLATE_MAUIN_DS_QDPHANCONGTHAMPHANGIAIQUYETVUANPT.Remove(Setting.TEMPLATE_MAUIN_DS_QDPHANCONGTHAMPHANGIAIQUYETVUANPT.IndexOf("."));
                    if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                    {
                        templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_DS_QDPHANCONGTHAMPHANGIAIQUYETVUVIECPT);
                        filename = Setting.TEMPLATE_MAUIN_DS_QDPHANCONGTHAMPHANGIAIQUYETVUVIECPT.Remove(Setting.TEMPLATE_MAUIN_DS_QDPHANCONGTHAMPHANGIAIQUYETVUVIECPT.IndexOf("."));
                    }
                }
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauIn0613QuyetDinhPCTPDoc(mauInObject, filePath, templatePath, pathChuKy);

                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else if (nhoman == Setting.MANHOMAN_HONNHAN)
            {
                var mauInObject = MauInService.DuLieuMauInQuyetDinhPCTP(hoSoVuAnId, giaidoan);
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HN_QUYETDINHPHANCONGTHAMPHANGIAIQUYETVUANST);
                string filename = Setting.TEMPLATE_MAUIN_HN_QUYETDINHPHANCONGTHAMPHANGIAIQUYETVUANST.Remove(Setting.TEMPLATE_MAUIN_HN_QUYETDINHPHANCONGTHAMPHANGIAIQUYETVUANST.IndexOf("."));
                if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HN_QUYETDINHPHANCONGTHAMPHANGIAIQUYETVUVIECST);
                    filename = Setting.TEMPLATE_MAUIN_HN_QUYETDINHPHANCONGTHAMPHANGIAIQUYETVUVIECST.Remove(Setting.TEMPLATE_MAUIN_HN_QUYETDINHPHANCONGTHAMPHANGIAIQUYETVUVIECST.IndexOf("."));
                }
                if (giaidoan == 2)
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HN_QDPHANCONGTHAMPHANGIAIQUYETVUANPT);
                    filename = Setting.TEMPLATE_MAUIN_HN_QDPHANCONGTHAMPHANGIAIQUYETVUANPT.Remove(Setting.TEMPLATE_MAUIN_HN_QDPHANCONGTHAMPHANGIAIQUYETVUANPT.IndexOf("."));
                    if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                    {
                        templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HN_QDPHANCONGTHAMPHANGIAIQUYETVUVIECPT);
                        filename = Setting.TEMPLATE_MAUIN_HN_QDPHANCONGTHAMPHANGIAIQUYETVUVIECPT.Remove(Setting.TEMPLATE_MAUIN_HN_QDPHANCONGTHAMPHANGIAIQUYETVUVIECPT.IndexOf("."));
                    }
                }
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauIn0613QuyetDinhPCTPDoc(mauInObject, filePath, templatePath, pathChuKy);

                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else if (nhoman == Setting.MANHOMAN_APDUNG_BPXLHC)
            {
                var mauInObject = MauInService.DuLieuMauInQuyetDinhPCTP(hoSoVuAnId, giaidoan);
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_AD_QDPHANCONGTHAMPHAN_ST);
                string filename = Setting.TEMPLATE_MAUIN_AD_QDPHANCONGTHAMPHAN_ST.Remove(Setting.TEMPLATE_MAUIN_AD_QDPHANCONGTHAMPHAN_ST.IndexOf("."));
                if (giaidoan == 2)
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_AD_QDPHANCONGTHAMPHAN_PT);
                    filename = Setting.TEMPLATE_MAUIN_AD_QDPHANCONGTHAMPHAN_PT.Remove(Setting.TEMPLATE_MAUIN_AD_QDPHANCONGTHAMPHAN_PT.IndexOf("."));
                }
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauInADQuyetDinhPCTPDoc(mauInObject, filePath, templatePath);

                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else if (nhoman == Setting.MANHOMAN_KINHTE)
            {
                var mauInObject = MauInService.DuLieuMauInQuyetDinhPCTP(hoSoVuAnId, giaidoan);
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_KD_QUYETDINHPHANCONGTHAMPHANGIAIQUYETVUANST);
                string filename = Setting.TEMPLATE_MAUIN_KD_QUYETDINHPHANCONGTHAMPHANGIAIQUYETVUANST.Remove(Setting.TEMPLATE_MAUIN_KD_QUYETDINHPHANCONGTHAMPHANGIAIQUYETVUANST.IndexOf("."));
                if (giaidoan == 2)
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_KD_QDPHANCONGTHAMPHANGIAIQUYETVUANPT);
                    filename = Setting.TEMPLATE_MAUIN_KD_QDPHANCONGTHAMPHANGIAIQUYETVUANPT.Remove(Setting.TEMPLATE_MAUIN_HN_QDPHANCONGTHAMPHANGIAIQUYETVUANPT.IndexOf("."));
                }
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauIn0613QuyetDinhPCTPDoc(mauInObject, filePath, templatePath, pathChuKy);

                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else if (nhoman == Setting.MANHOMAN_LAODONG)
            {
                var mauInObject = MauInService.DuLieuMauInQuyetDinhPCTP(hoSoVuAnId, giaidoan);
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_LD_QUYETDINHPHANCONGTHAMPHANGIAIQUYETVUANST);
                string filename = Setting.TEMPLATE_MAUIN_LD_QUYETDINHPHANCONGTHAMPHANGIAIQUYETVUANST.Remove(Setting.TEMPLATE_MAUIN_LD_QUYETDINHPHANCONGTHAMPHANGIAIQUYETVUANST.IndexOf("."));
                if (giaidoan == 2)
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_KD_QDPHANCONGTHAMPHANGIAIQUYETVUANPT);
                    filename = Setting.TEMPLATE_MAUIN_KD_QDPHANCONGTHAMPHANGIAIQUYETVUANPT.Remove(Setting.TEMPLATE_MAUIN_HN_QDPHANCONGTHAMPHANGIAIQUYETVUANPT.IndexOf("."));
                }
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauIn0613QuyetDinhPCTPDoc(mauInObject, filePath, templatePath, pathChuKy);

                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
    }

        public ActionResult MauInQuyetDinhPCTPGiaiQuyetDonDoc(int hoSoVuAnId)
        {
            string folder = Server.MapPath(string.Format(Setting.MAU_IN_DOC, DateTime.Now.Year, DateTime.Now.Month));
            if (!Directory.Exists((folder)))
            {
                Directory.CreateDirectory(folder);
            }
            string folderTemplate = Server.MapPath(Setting.TEMPLATES_MAUIN_FOLDER);
            if (!Directory.Exists((folderTemplate)))
            {
                Directory.CreateDirectory(folderTemplate);
            }
            var nhoman = GetAnSessionInfo().MaNhomAn;
            string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_DS_SOTHAM_QUYETDINHPHANCONGTHAMPHANGIAIQUYETDON);
            var mauInObject = MauInService.DuLieuMauInQuyetDinhPCTP(hoSoVuAnId, GetAnSessionInfo().GiaiDoanId);
            string filename = Setting.TEMPLATE_MAUIN_DS_SOTHAM_QUYETDINHPHANCONGTHAMPHANGIAIQUYETDON.Remove(Setting.TEMPLATE_MAUIN_DS_SOTHAM_QUYETDINHPHANCONGTHAMPHANGIAIQUYETDON.IndexOf("."));
            if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
            {
                templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_DS_SOTHAM_QUYETDINHPHANCONGTHAMPHANGIAIQUYETDON_YC);
                filename = Setting.TEMPLATE_MAUIN_DS_SOTHAM_QUYETDINHPHANCONGTHAMPHANGIAIQUYETDON_YC.Remove(Setting.TEMPLATE_MAUIN_DS_SOTHAM_QUYETDINHPHANCONGTHAMPHANGIAIQUYETDON_YC.IndexOf("."));
            }
            if (nhoman == Setting.MANHOMAN_HONNHAN)
            {
                if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HN_SOTHAM_QUYETDINHPHANCONGTHAMPHANGIAIQUYETDON_YC);
                }
                filename = filename.Replace(Setting.MANHOMAN_DANSU, Setting.MANHOMAN_HONNHAN);
            }                
            string filePath = Path.Combine(folder, string.Format(filename+@"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
            var outputFiles = MauInService.MauIn03QuyetDinhPCTPGiaiQuyetDonDoc(mauInObject, filePath, templatePath);

            return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MauIn29Doc(int hoSoVuAnId)
        {
            string folder = Server.MapPath(string.Format(Setting.MAU_IN_DOC, DateTime.Now.Year, DateTime.Now.Month));
            if (!Directory.Exists((folder)))
            {
                Directory.CreateDirectory(folder);
            }
            string folderTemplate = Server.MapPath(Setting.TEMPLATES_MAUIN_FOLDER);
            if (!Directory.Exists((folderTemplate)))
            {
                Directory.CreateDirectory(folderTemplate);
            }
            var nhoman = GetAnSessionInfo().MaNhomAn;
            if (nhoman==Setting.MANHOMAN_HANHCHINH)
            {                
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HC_SOTHAM_THONGBAOTAMUNGANPHI);
                var mauInObject = MauInService.DuLieuMauInSo29(hoSoVuAnId);
                string filename = Setting.TEMPLATE_MAUIN_HC_SOTHAM_THONGBAOTAMUNGANPHI;
                filename = filename.Remove(filename.IndexOf("."));
                string filePath = Path.Combine(folder, string.Format(filename+@"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauInHC04Doc(mauInObject, filePath, templatePath);

                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else if (nhoman == Setting.MANHOMAN_DANSU || nhoman==Setting.MANHOMAN_HONNHAN)
            {
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_DS_SOTHAM_THONGBAOTAMUNGANPHI);
                var mauInObject = MauInService.DuLieuMauInSo29(hoSoVuAnId);
                string filename = Setting.TEMPLATE_MAUIN_DS_SOTHAM_THONGBAOTAMUNGANPHI;
                if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_DS_SOTHAM_THONGBAOTAMUNGLEPHI);
                    filename = Setting.TEMPLATE_MAUIN_DS_SOTHAM_THONGBAOTAMUNGLEPHI;
                }
                filename = filename.Remove(filename.IndexOf("."));
                if (nhoman == Setting.MANHOMAN_HONNHAN)
                {
                    if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                    {
                        templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HN_SOTHAM_THONGBAOTAMUNGLEPHI);
                    }
                    filename = filename.Replace(Setting.MANHOMAN_DANSU, Setting.MANHOMAN_HONNHAN);
                }
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauIn04ThongBaoNopTienTamUngSTDoc(mauInObject, filePath, templatePath);

                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else if (nhoman == Setting.MANHOMAN_KINHTE)
            {
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_KD_SOTHAM_THONGBAOTAMUNGANPHI);
                var mauInObject = MauInService.DuLieuMauInSo29(hoSoVuAnId);
                string filename = Setting.TEMPLATE_MAUIN_KD_SOTHAM_THONGBAOTAMUNGANPHI;
                if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_KD_SOTHAM_THONGBAOTAMUNGLEPHI);
                    filename = Setting.TEMPLATE_MAUIN_KD_SOTHAM_THONGBAOTAMUNGLEPHI;
                }
                filename = filename.Remove(filename.IndexOf("."));
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauIn04ThongBaoNopTienTamUngSTDoc(mauInObject, filePath, templatePath);

                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else if (nhoman == Setting.MANHOMAN_LAODONG)
            {
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_LD_SOTHAM_THONGBAOTAMUNGANPHI);
                var mauInObject = MauInService.DuLieuMauInSo29(hoSoVuAnId);
                string filename = Setting.TEMPLATE_MAUIN_LD_SOTHAM_THONGBAOTAMUNGANPHI;
                if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_LD_SOTHAM_THONGBAOTAMUNGLEPHI);
                    filename = Setting.TEMPLATE_MAUIN_LD_SOTHAM_THONGBAOTAMUNGLEPHI;
                }
                filename = filename.Remove(filename.IndexOf("."));
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauIn04ThongBaoNopTienTamUngSTDoc(mauInObject, filePath, templatePath);

                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MauIn30Doc(int hoSoVuAnId)
        {
            string folder = Server.MapPath(string.Format(Setting.MAU_IN_DOC, DateTime.Now.Year, DateTime.Now.Month));
            if (!Directory.Exists((folder)))
            {
                Directory.CreateDirectory(folder);
            }
            string folderTemplate = Server.MapPath(Setting.TEMPLATES_MAUIN_FOLDER);
            if (!Directory.Exists((folderTemplate)))
            {
                Directory.CreateDirectory(folderTemplate);
            }
            var nhoman = GetAnSessionInfo().MaNhomAn;
            if (nhoman==Setting.MANHOMAN_HANHCHINH)
            {                
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HC_SOTHAM_THONGBAOTHULYVUAN);
                var mauInObject = MauInService.ChiTietMauInSo30(hoSoVuAnId);
                string filename = Setting.TEMPLATE_MAUIN_HC_SOTHAM_THONGBAOTHULYVUAN.Remove(Setting.TEMPLATE_MAUIN_HC_SOTHAM_THONGBAOTHULYVUAN.IndexOf("."));
                string filePath = Path.Combine(folder, string.Format(filename+@"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauInHC06Doc(mauInObject, filePath, templatePath);

                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            if (nhoman == Setting.MANHOMAN_DANSU)
            {
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_DS_SOTHAM_THONGBAOTHULYVUAN);
                var mauInObject = MauInService.ChiTietMauInSo30(hoSoVuAnId);
                string filename = Setting.TEMPLATE_MAUIN_DS_SOTHAM_THONGBAOTHULYVUAN.Remove(Setting.TEMPLATE_MAUIN_DS_SOTHAM_THONGBAOTHULYVUAN.IndexOf("."));
                if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_DS_SOTHAM_THONGBAOTHULYVUVIEC);
                    filename = Setting.TEMPLATE_MAUIN_DS_SOTHAM_THONGBAOTHULYVUVIEC.Remove(Setting.TEMPLATE_MAUIN_DS_SOTHAM_THONGBAOTHULYVUVIEC.IndexOf("."));
                }
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauIn05ThongBaoThuLySTDoc(mauInObject, filePath, templatePath);

                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            if (nhoman == Setting.MANHOMAN_HONNHAN)
            {
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HN_SOTHAM_THONGBAOTHULYVUAN);
                var mauInObject = MauInService.ChiTietMauInSo30(hoSoVuAnId);
                string filename = Setting.TEMPLATE_MAUIN_HN_SOTHAM_THONGBAOTHULYVUAN.Remove(Setting.TEMPLATE_MAUIN_HN_SOTHAM_THONGBAOTHULYVUAN.IndexOf("."));
                if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HN_SOTHAM_THONGBAOTHULYVUVIEC);
                    filename = Setting.TEMPLATE_MAUIN_HN_SOTHAM_THONGBAOTHULYVUVIEC.Remove(Setting.TEMPLATE_MAUIN_HN_SOTHAM_THONGBAOTHULYVUVIEC.IndexOf("."));
                }
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauIn05ThongBaoThuLySTDoc(mauInObject, filePath, templatePath);

                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            if (nhoman == Setting.MANHOMAN_KINHTE)
            {
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_KD_SOTHAM_THONGBAOTHULYVUAN);
                var mauInObject = MauInService.ChiTietMauInSo30(hoSoVuAnId);
                string filename = Setting.TEMPLATE_MAUIN_KD_SOTHAM_THONGBAOTHULYVUAN.Remove(Setting.TEMPLATE_MAUIN_KD_SOTHAM_THONGBAOTHULYVUAN.IndexOf("."));
                if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_KD_SOTHAM_THONGBAOTHULYVUVIEC);
                    filename = Setting.TEMPLATE_MAUIN_KD_SOTHAM_THONGBAOTHULYVUVIEC.Remove(Setting.TEMPLATE_MAUIN_KD_SOTHAM_THONGBAOTHULYVUVIEC.IndexOf("."));
                }
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauIn05ThongBaoThuLySTDoc(mauInObject, filePath, templatePath);

                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            if (nhoman == Setting.MANHOMAN_LAODONG)
            {
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_LD_SOTHAM_THONGBAOTHULYVUAN);
                var mauInObject = MauInService.ChiTietMauInSo30(hoSoVuAnId);
                string filename = Setting.TEMPLATE_MAUIN_LD_SOTHAM_THONGBAOTHULYVUAN.Remove(Setting.TEMPLATE_MAUIN_LD_SOTHAM_THONGBAOTHULYVUAN.IndexOf("."));
                if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_LD_SOTHAM_THONGBAOTHULYVUVIEC);
                    filename = Setting.TEMPLATE_MAUIN_LD_SOTHAM_THONGBAOTHULYVUVIEC.Remove(Setting.TEMPLATE_MAUIN_LD_SOTHAM_THONGBAOTHULYVUVIEC.IndexOf("."));
                }
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauIn05ThongBaoThuLySTDoc(mauInObject, filePath, templatePath);

                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            if (nhoman == Setting.MANHOMAN_APDUNG_BPXLHC)
            {
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_AD_SOTHAM_THONGBAOTHULY_ST);
                var mauInObject = MauInService.ChiTietMauInSo30(hoSoVuAnId);
                string filename = Setting.TEMPLATE_MAUIN_AD_SOTHAM_THONGBAOTHULY_ST.Remove(Setting.TEMPLATE_MAUIN_AD_SOTHAM_THONGBAOTHULY_ST.IndexOf("."));
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauInAD01Doc(mauInObject, filePath, templatePath);

                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MauIn47Doc(int hoSoVuAnId)
        {
            string folder = Server.MapPath(string.Format(Setting.MAU_IN_DOC, DateTime.Now.Year, DateTime.Now.Month));
            if (!Directory.Exists((folder)))
            {
                Directory.CreateDirectory(folder);
            }
            string folderTemplate = Server.MapPath(Setting.TEMPLATES_MAUIN_FOLDER);
            if (!Directory.Exists((folderTemplate)))
            {
                Directory.CreateDirectory(folderTemplate);
            }
            var nhoman = GetAnSessionInfo().MaNhomAn;
            if(nhoman==Setting.MANHOMAN_HANHCHINH)
            {
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HC_SOTHAM_QDDUAVUANRAXETXU);
                var mauInObject = MauInService.DuLieuMauInSo47(hoSoVuAnId);
                string filename = Setting.TEMPLATE_MAUIN_HC_SOTHAM_THONGBAOTHULYVUAN.Remove(Setting.TEMPLATE_MAUIN_HC_SOTHAM_THONGBAOTHULYVUAN.IndexOf("."));
                if (mauInObject.ThuTuc == Setting.THUTUC_RUTGON)
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HC_SOTHAM_QDDUAVUANRAXETXURUTGON);
                    filename = Setting.TEMPLATE_MAUIN_HC_SOTHAM_QDDUAVUANRAXETXURUTGON.Remove(Setting.TEMPLATE_MAUIN_HC_SOTHAM_QDDUAVUANRAXETXURUTGON.IndexOf("."));
                }
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauInHCQuyetDinhDuaVuAnXetXuSoThamDoc(mauInObject, filePath, templatePath);

                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else if (nhoman == Setting.MANHOMAN_DANSU)
            {
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_DS_QUYETDINHDUAVUANRAXETXUST);
                var mauInObject = MauInService.DuLieuMauInSo47(hoSoVuAnId);
                string filename = Setting.TEMPLATE_MAUIN_DS_QUYETDINHDUAVUANRAXETXUST.Remove(Setting.TEMPLATE_MAUIN_DS_QUYETDINHDUAVUANRAXETXUST.IndexOf("."));
                if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_DS_QUYETDINHMOPHIENHOPST);
                    filename = Setting.TEMPLATE_MAUIN_DS_QUYETDINHMOPHIENHOPST.Remove(Setting.TEMPLATE_MAUIN_DS_QUYETDINHMOPHIENHOPST.IndexOf("."));
                }
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauIn08QuyetDinhDuaVuAnXetXuSoThamDoc(mauInObject, filePath, templatePath);

                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else if (nhoman == Setting.MANHOMAN_HONNHAN)
            {
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HN_QUYETDINHDUAVUANRAXETXUST);
                var mauInObject = MauInService.DuLieuMauInSo47(hoSoVuAnId);
                string filename = Setting.TEMPLATE_MAUIN_HN_QUYETDINHDUAVUANRAXETXUST.Remove(Setting.TEMPLATE_MAUIN_HN_QUYETDINHDUAVUANRAXETXUST.IndexOf("."));
                if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HN_QUYETDINHMOPHIENHOPST);
                    filename = Setting.TEMPLATE_MAUIN_HN_QUYETDINHMOPHIENHOPST.Remove(Setting.TEMPLATE_MAUIN_HN_QUYETDINHMOPHIENHOPST.IndexOf("."));
                }
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauIn08QuyetDinhDuaVuAnXetXuSoThamDoc(mauInObject, filePath, templatePath);

                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else if (nhoman == Setting.MANHOMAN_KINHTE)
            {
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_KD_QUYETDINHDUAVUANRAXETXUST);
                var mauInObject = MauInService.DuLieuMauInSo47(hoSoVuAnId);
                string filename = Setting.TEMPLATE_MAUIN_KD_QUYETDINHDUAVUANRAXETXUST.Remove(Setting.TEMPLATE_MAUIN_KD_QUYETDINHDUAVUANRAXETXUST.IndexOf("."));
                if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_KD_QUYETDINHMOPHIENHOPST);
                    filename = Setting.TEMPLATE_MAUIN_KD_QUYETDINHMOPHIENHOPST.Remove(Setting.TEMPLATE_MAUIN_KD_QUYETDINHMOPHIENHOPST.IndexOf("."));
                }
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauIn08QuyetDinhDuaVuAnXetXuSoThamDoc(mauInObject, filePath, templatePath);

                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else if (nhoman == Setting.MANHOMAN_LAODONG)
            {
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_LD_QUYETDINHDUAVUANRAXETXUST);
                var mauInObject = MauInService.DuLieuMauInSo47(hoSoVuAnId);
                string filename = Setting.TEMPLATE_MAUIN_LD_QUYETDINHDUAVUANRAXETXUST.Remove(Setting.TEMPLATE_MAUIN_LD_QUYETDINHDUAVUANRAXETXUST.IndexOf("."));
                if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_LD_QUYETDINHMOPHIENHOPST);
                    filename = Setting.TEMPLATE_MAUIN_LD_QUYETDINHMOPHIENHOPST.Remove(Setting.TEMPLATE_MAUIN_LD_QUYETDINHMOPHIENHOPST.IndexOf("."));
                }
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauIn08QuyetDinhDuaVuAnXetXuSoThamDoc(mauInObject, filePath, templatePath);

                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else if (nhoman == Setting.MANHOMAN_APDUNG_BPXLHC)
            {
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_AD_QUYETDINHMOPHIENHOP_ST);
                var mauInObject = MauInService.DuLieuMauInSo47(hoSoVuAnId);
                string filename = Setting.TEMPLATE_MAUIN_AD_QUYETDINHMOPHIENHOP_ST.Remove(Setting.TEMPLATE_MAUIN_AD_QUYETDINHMOPHIENHOP_ST.IndexOf("."));
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauInADQuyetDinhMoPhienHopSoThamDoc(mauInObject, filePath, templatePath);

                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else if(nhoman==Setting.MANHOMAN_HINHSU)
            {
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HS_QUYETDINHDUAVUANRAXETXUST);
                var mauInObject = MauInService.DuLieuMauInSo47(hoSoVuAnId);
                string filename = Setting.TEMPLATE_MAUIN_HS_QUYETDINHDUAVUANRAXETXUST.Remove(Setting.TEMPLATE_MAUIN_HS_QUYETDINHDUAVUANRAXETXUST.IndexOf("."));
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauInHSQuyetDinhDuaVuAnXetXuSoThamDoc(mauInObject, filePath, templatePath);

                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult MauInGTTDoc(int hoSoVuAnId)
        {
            string folder = Server.MapPath(string.Format(Setting.MAU_IN_DOC, DateTime.Now.Year, DateTime.Now.Month));
            if (!Directory.Exists((folder)))
            {
                Directory.CreateDirectory(folder);
            }
            string folderTemplate = Server.MapPath(Setting.TEMPLATES_MAUIN_FOLDER);
            if (!Directory.Exists((folderTemplate)))
            {
                Directory.CreateDirectory(folderTemplate);
            }
            var nhoman = GetAnSessionInfo().MaNhomAn;
            if(nhoman==Setting.MANHOMAN_HANHCHINH)
            {
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HC_GIAYTRIEUTAP);
                var mauInObject = MauInService.DuLieuMauInGiayTrieuTap(hoSoVuAnId);
                string filename = Setting.TEMPLATE_MAUIN_HC_GIAYTRIEUTAP.Remove(Setting.TEMPLATE_MAUIN_HC_GIAYTRIEUTAP.IndexOf("."));

                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauInHCGiayTrieuTapDoc(mauInObject, filePath, templatePath);

                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            if (nhoman == Setting.MANHOMAN_HINHSU)
            {
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HS_GIAYTRIEUTAPNGUOITHAMGIATOTUNG);
                var mauInObject = MauInService.DuLieuMauInGiayTrieuTap(hoSoVuAnId);
                string filename = Setting.TEMPLATE_MAUIN_HS_GIAYTRIEUTAPNGUOITHAMGIATOTUNG.Remove(Setting.TEMPLATE_MAUIN_HS_GIAYTRIEUTAPNGUOITHAMGIATOTUNG.IndexOf("."));

                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauInHSGiayTrieuTapNguoiThamGiaTTDoc(mauInObject, filePath, templatePath);

                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            if (nhoman == Setting.MANHOMAN_APDUNG_BPXLHC)
            {
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_AD_GIAYTRIEUTAP);
                var mauInObject = MauInService.DuLieuMauInGiayTrieuTap(hoSoVuAnId);
                string filename = Setting.TEMPLATE_MAUIN_AD_GIAYTRIEUTAP.Remove(Setting.TEMPLATE_MAUIN_AD_GIAYTRIEUTAP.IndexOf("."));

                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauInADGiayTrieuTapDoc(mauInObject, filePath, templatePath);

                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            if (nhoman == Setting.MANHOMAN_DANSU || nhoman == Setting.MANHOMAN_HONNHAN)
            {
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_DS_GIAYTRIEUTAP_TC);
                var mauInObject = MauInService.DuLieuMauInGiayTrieuTap(hoSoVuAnId);
                string filename = "";                
                if (GetAnSessionInfo().GiaiDoanId!=2)
                {
                    filename = Setting.TEMPLATE_MAUIN_DS_GIAYTRIEUTAP_TC.Remove(Setting.TEMPLATE_MAUIN_DS_GIAYTRIEUTAP_TC.IndexOf("."));
                    if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                    {
                        templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_DS_GIAYTRIEUTAP_YC);
                        filename = Setting.TEMPLATE_MAUIN_DS_GIAYTRIEUTAP_YC.Remove(Setting.TEMPLATE_MAUIN_DS_GIAYTRIEUTAP_YC.IndexOf("."));
                    }
                }
                else
                {
                    filename = Setting.TEMPLATE_MAUIN_DS_GIAYTRIEUTAP_PT_TC.Remove(Setting.TEMPLATE_MAUIN_DS_GIAYTRIEUTAP_PT_TC.IndexOf("."));
                    if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                    {
                        templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_DS_GIAYTRIEUTAP_YC_PT);
                        filename = Setting.TEMPLATE_MAUIN_DS_GIAYTRIEUTAP_YC_PT.Remove(Setting.TEMPLATE_MAUIN_DS_GIAYTRIEUTAP_YC_PT.IndexOf("."));
                    }
                }
                if (nhoman == Setting.MANHOMAN_HONNHAN)
                {
                    if (GetAnSessionInfo().GiaiDoanId != 2)
                    {
                        templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HN_GIAYTRIEUTAP_YC);
                    }
                    filename = filename.Replace(Setting.MANHOMAN_DANSU, Setting.MANHOMAN_HONNHAN);
                }
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauIn09GiayTrieuTapDoc(mauInObject, filePath, templatePath);

                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            if (nhoman == Setting.MANHOMAN_KINHTE)
            {
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_KD_GIAYTRIEUTAP_TC);
                var mauInObject = MauInService.DuLieuMauInGiayTrieuTap(hoSoVuAnId);
                string filename = Setting.TEMPLATE_MAUIN_KD_GIAYTRIEUTAP_TC.Remove(Setting.TEMPLATE_MAUIN_KD_GIAYTRIEUTAP_TC.IndexOf("."));
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauIn09GiayTrieuTapDoc(mauInObject, filePath, templatePath);

                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            if (nhoman == Setting.MANHOMAN_LAODONG)
            {
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_LD_GIAYTRIEUTAP_TC);
                var mauInObject = MauInService.DuLieuMauInGiayTrieuTap(hoSoVuAnId);
                string filename = Setting.TEMPLATE_MAUIN_LD_GIAYTRIEUTAP_TC.Remove(Setting.TEMPLATE_MAUIN_LD_GIAYTRIEUTAP_TC.IndexOf("."));
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauIn09GiayTrieuTapDoc(mauInObject, filePath, templatePath);

                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MauInGTTBiCaoDoc(int hoSoVuAnId)
        {
            string folder = Server.MapPath(string.Format(Setting.MAU_IN_DOC, DateTime.Now.Year, DateTime.Now.Month));
            if (!Directory.Exists((folder)))
            {
                Directory.CreateDirectory(folder);
            }
            string folderTemplate = Server.MapPath(Setting.TEMPLATES_MAUIN_FOLDER);
            if (!Directory.Exists((folderTemplate)))
            {
                Directory.CreateDirectory(folderTemplate);
            }
            string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HS_GIAYTRIEUTAPBICAO);
            var mauInObject = MauInService.DuLieuMauInGiayTrieuTap(hoSoVuAnId);
            string filename = Setting.TEMPLATE_MAUIN_HS_GIAYTRIEUTAPBICAO.Remove(Setting.TEMPLATE_MAUIN_HS_GIAYTRIEUTAPBICAO.IndexOf("."));

            string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
            var outputFiles = MauInService.MauInHSGiayTrieuTapBiCaoDoc(mauInObject, filePath, templatePath);

            if (outputFiles != null)
                return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MauInGXNKCDoc(int hoSoVuAnId)
        {
            string folder = Server.MapPath(string.Format(Setting.MAU_IN_DOC, DateTime.Now.Year, DateTime.Now.Month));
            if (!Directory.Exists((folder)))
            {
                Directory.CreateDirectory(folder);
            }
            string folderTemplate = Server.MapPath(Setting.TEMPLATES_MAUIN_FOLDER);
            if (!Directory.Exists((folderTemplate)))
            {
                Directory.CreateDirectory(folderTemplate);
            }
            string templatePath = "", filename = "";
            var nhoman = GetAnSessionInfo().MaNhomAn;
            var mauInObject = MauInService.ChiTietGXNKhangCao(hoSoVuAnId);
            if (nhoman==Setting.MANHOMAN_HANHCHINH)
            {
                templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HC_SOTHAM_XACNHANDANHANDONKHANGCAO);
                filename = Setting.TEMPLATE_MAUIN_HC_SOTHAM_XACNHANDANHANDONKHANGCAO.Remove(Setting.TEMPLATE_MAUIN_HC_SOTHAM_XACNHANDANHANDONKHANGCAO.IndexOf("."));
            }
            else if (nhoman == Setting.MANHOMAN_DANSU)
            {
                templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_DS_SOTHAM_XACNHANDANHANDONKHANGCAO);
                filename = Setting.TEMPLATE_MAUIN_DS_SOTHAM_XACNHANDANHANDONKHANGCAO.Remove(Setting.TEMPLATE_MAUIN_DS_SOTHAM_XACNHANDANHANDONKHANGCAO.IndexOf("."));
                if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_DS_SOTHAM_XACNHANDANHANDONKHANGCAO_YC);
                    filename = Setting.TEMPLATE_MAUIN_DS_SOTHAM_XACNHANDANHANDONKHANGCAO_YC.Remove(Setting.TEMPLATE_MAUIN_DS_SOTHAM_XACNHANDANHANDONKHANGCAO_YC.IndexOf("."));
                }
            }
            else if (nhoman == Setting.MANHOMAN_HONNHAN)
            {
                templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HN_SOTHAM_XACNHANDANHANDONKHANGCAO);
                filename = Setting.TEMPLATE_MAUIN_HN_SOTHAM_XACNHANDANHANDONKHANGCAO.Remove(Setting.TEMPLATE_MAUIN_HN_SOTHAM_XACNHANDANHANDONKHANGCAO.IndexOf("."));
                if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HN_SOTHAM_XACNHANDANHANDONKHANGCAO_YC);
                    filename = Setting.TEMPLATE_MAUIN_HN_SOTHAM_XACNHANDANHANDONKHANGCAO_YC.Remove(Setting.TEMPLATE_MAUIN_HN_SOTHAM_XACNHANDANHANDONKHANGCAO_YC.IndexOf("."));
                }
            }
            else if (nhoman == Setting.MANHOMAN_KINHTE)
            {
                templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_KD_SOTHAM_XACNHANDANHANDONKHANGCAO);
                filename = Setting.TEMPLATE_MAUIN_KD_SOTHAM_XACNHANDANHANDONKHANGCAO.Remove(Setting.TEMPLATE_MAUIN_KD_SOTHAM_XACNHANDANHANDONKHANGCAO.IndexOf("."));
            }
            else if (nhoman == Setting.MANHOMAN_LAODONG)
            {
                templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_LD_SOTHAM_XACNHANDANHANDONKHANGCAO);
                filename = Setting.TEMPLATE_MAUIN_LD_SOTHAM_XACNHANDANHANDONKHANGCAO.Remove(Setting.TEMPLATE_MAUIN_LD_SOTHAM_XACNHANDANHANDONKHANGCAO.IndexOf("."));
            }
            string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
            var outputFiles = MauInService.MauInGiayXacNhanKhangCaoDoc(mauInObject, filePath, templatePath);

            if (outputFiles != null)
                return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MauIn61Doc(int hoSoVuAnId)
        {
            string folder = Server.MapPath(string.Format(Setting.MAU_IN_DOC, DateTime.Now.Year, DateTime.Now.Month));
            if (!Directory.Exists((folder)))
            {
                Directory.CreateDirectory(folder);
            }
            string folderTemplate = Server.MapPath(Setting.TEMPLATES_MAUIN_FOLDER);
            if (!Directory.Exists((folderTemplate)))
            {
                Directory.CreateDirectory(folderTemplate);
            }
            var nhoman = GetAnSessionInfo().MaNhomAn;
            if(nhoman==Setting.MANHOMAN_HANHCHINH)
            {
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HC_SOTHAM_THONGBAOTAMUNGANPHIPT);
                var mauInObject = MauInService.DuLieuMauInSo61(hoSoVuAnId);
                string filename = Setting.TEMPLATE_MAUIN_HC_SOTHAM_THONGBAOTAMUNGANPHIPT.Remove(Setting.TEMPLATE_MAUIN_HC_SOTHAM_THONGBAOTAMUNGANPHIPT.IndexOf("."));
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauInHC31Doc(mauInObject, filePath, templatePath);

                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else if (nhoman == Setting.MANHOMAN_DANSU || nhoman == Setting.MANHOMAN_HONNHAN || nhoman==Setting.MANHOMAN_KINHTE || nhoman==Setting.MANHOMAN_LAODONG)
            {
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_DS_SOTHAM_THONGBAOTAMUNGANPHIPT);
                var mauInObject = MauInService.DuLieuMauInSo61(hoSoVuAnId);
                string filename = Setting.TEMPLATE_MAUIN_DS_SOTHAM_THONGBAOTAMUNGANPHIPT.Remove(Setting.TEMPLATE_MAUIN_DS_SOTHAM_THONGBAOTAMUNGANPHIPT.IndexOf("."));                
                if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_DS_SOTHAM_THONGBAOTAMUNGLEPHIPT);
                    filename = Setting.TEMPLATE_MAUIN_DS_SOTHAM_THONGBAOTAMUNGLEPHIPT.Remove(Setting.TEMPLATE_MAUIN_DS_SOTHAM_THONGBAOTAMUNGLEPHIPT.IndexOf("."));
                }
                if (nhoman == Setting.MANHOMAN_HONNHAN)
                    filename = filename.Replace(Setting.MANHOMAN_DANSU, Setting.MANHOMAN_HONNHAN);
                if (nhoman == Setting.MANHOMAN_KINHTE)
                {
                    if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                        filename = "Mau so 61-KDTM Thong bao nop tien tam ung le phi phuc tham";
                    else
                        filename = "Mau so 61-KDTM Thong bao nop tien tam ung an phi phuc tham";
                }
                if (nhoman == Setting.MANHOMAN_LAODONG)
                {
                    if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                        filename = "Mau so 61-LD Thong bao nop tien tam ung le phi phuc tham";
                    else
                        filename = "Mau so 61-LD Thong bao nop tien tam ung an phi phuc tham";
                }
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauIn11ThongBaoNopTienTamUngPTDoc(mauInObject, filePath, templatePath);

                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MauIn65Doc(int hoSoVuAnId)
        {
            string folder = Server.MapPath(string.Format(Setting.MAU_IN_DOC, DateTime.Now.Year, DateTime.Now.Month));
            if (!Directory.Exists((folder)))
            {
                Directory.CreateDirectory(folder);
            }
            string folderTemplate = Server.MapPath(Setting.TEMPLATES_MAUIN_FOLDER);
            if (!Directory.Exists((folderTemplate)))
            {
                Directory.CreateDirectory(folderTemplate);
            }
            var nhoman = GetAnSessionInfo().MaNhomAn;
            if (nhoman == Setting.MANHOMAN_HANHCHINH)
            {
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HC_PHUCTHAM_THONGBAOTHULYVUAN);
                var mauInObject = MauInService.DuLieuMauInSo65(hoSoVuAnId);
                string filename = Setting.TEMPLATE_MAUIN_HC_PHUCTHAM_THONGBAOTHULYVUAN.Remove(Setting.TEMPLATE_MAUIN_HC_PHUCTHAM_THONGBAOTHULYVUAN.IndexOf("."));
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauInHC35Doc(mauInObject, filePath, templatePath);

                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else if (nhoman == Setting.MANHOMAN_DANSU)
            {
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_DS_PHUCTHAM_THONGBAOTHULYVUAN);
                var mauInObject = MauInService.DuLieuMauInSo65(hoSoVuAnId);
                string filename = Setting.TEMPLATE_MAUIN_DS_PHUCTHAM_THONGBAOTHULYVUAN.Remove(Setting.TEMPLATE_MAUIN_DS_PHUCTHAM_THONGBAOTHULYVUAN.IndexOf("."));
                if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_DS_PHUCTHAM_THONGBAOTHULYVUVIEC);
                    filename = Setting.TEMPLATE_MAUIN_DS_PHUCTHAM_THONGBAOTHULYVUVIEC.Remove(Setting.TEMPLATE_MAUIN_DS_PHUCTHAM_THONGBAOTHULYVUVIEC.IndexOf("."));
                }
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauIn12ThongBaoThuLyPTDoc(mauInObject, filePath, templatePath);

                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else if (nhoman == Setting.MANHOMAN_HONNHAN)
            {
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HN_PHUCTHAM_THONGBAOTHULYVUAN);
                var mauInObject = MauInService.DuLieuMauInSo65(hoSoVuAnId);
                string filename = Setting.TEMPLATE_MAUIN_HN_PHUCTHAM_THONGBAOTHULYVUAN.Remove(Setting.TEMPLATE_MAUIN_HN_PHUCTHAM_THONGBAOTHULYVUAN.IndexOf("."));
                if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HN_PHUCTHAM_THONGBAOTHULYVUVIEC);
                    filename = Setting.TEMPLATE_MAUIN_HN_PHUCTHAM_THONGBAOTHULYVUVIEC.Remove(Setting.TEMPLATE_MAUIN_HN_PHUCTHAM_THONGBAOTHULYVUVIEC.IndexOf("."));
                }
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauIn12ThongBaoThuLyPTDoc(mauInObject, filePath, templatePath);

                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else if (nhoman == Setting.MANHOMAN_KINHTE)
            {
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_KD_PHUCTHAM_THONGBAOTHULYVUAN);
                var mauInObject = MauInService.DuLieuMauInSo65(hoSoVuAnId);
                string filename = Setting.TEMPLATE_MAUIN_KD_PHUCTHAM_THONGBAOTHULYVUAN.Remove(Setting.TEMPLATE_MAUIN_KD_PHUCTHAM_THONGBAOTHULYVUAN.IndexOf("."));
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauIn12ThongBaoThuLyPTDoc(mauInObject, filePath, templatePath);

                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else if (nhoman == Setting.MANHOMAN_LAODONG)
            {
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_LD_PHUCTHAM_THONGBAOTHULYVUAN);
                var mauInObject = MauInService.DuLieuMauInSo65(hoSoVuAnId);
                string filename = Setting.TEMPLATE_MAUIN_LD_PHUCTHAM_THONGBAOTHULYVUAN.Remove(Setting.TEMPLATE_MAUIN_LD_PHUCTHAM_THONGBAOTHULYVUAN.IndexOf("."));
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauIn12ThongBaoThuLyPTDoc(mauInObject, filePath, templatePath);

                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MauIn47PTDoc(int hoSoVuAnId)
        {
            string folder = Server.MapPath(string.Format(Setting.MAU_IN_DOC, DateTime.Now.Year, DateTime.Now.Month));
            if (!Directory.Exists((folder)))
            {
                Directory.CreateDirectory(folder);
            }
            string folderTemplate = Server.MapPath(Setting.TEMPLATES_MAUIN_FOLDER);
            if (!Directory.Exists((folderTemplate)))
            {
                Directory.CreateDirectory(folderTemplate);
            }
            var nhoman = GetAnSessionInfo().MaNhomAn;
            if(nhoman==Setting.MANHOMAN_HANHCHINH)
            {
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HC_PHUCTHAM_QDDUAVUANRAXETXU);
                var mauInObject = MauInService.DuLieuMauInSo47PT(hoSoVuAnId);
                string filename = Setting.TEMPLATE_MAUIN_HC_PHUCTHAM_QDDUAVUANRAXETXU.Remove(Setting.TEMPLATE_MAUIN_HC_PHUCTHAM_QDDUAVUANRAXETXU.IndexOf("."));
                if (mauInObject.ThuTuc == Setting.THUTUC_RUTGON)
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HC_PHUCTHAM_QDDUAVUANRAXETXURUTGON);
                    filename = Setting.TEMPLATE_MAUIN_HC_PHUCTHAM_QDDUAVUANRAXETXURUTGON.Remove(Setting.TEMPLATE_MAUIN_HC_PHUCTHAM_QDDUAVUANRAXETXURUTGON.IndexOf("."));
                }
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauInHCQuyetDinhDuaVuAnXetXuPhucThamDoc(mauInObject, filePath, templatePath);

                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else if(nhoman==Setting.MANHOMAN_HINHSU)
            {
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HS_QUYETDINHDUAVUANRAXETXUPT);
                var mauInObject = MauInService.DuLieuMauInSo47PT(hoSoVuAnId);
                string filename = Setting.TEMPLATE_MAUIN_HS_QUYETDINHDUAVUANRAXETXUPT.Remove(Setting.TEMPLATE_MAUIN_HS_QUYETDINHDUAVUANRAXETXUPT.IndexOf("."));                
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauInHSQuyetDinhDuaVuAnXetXuPhucThamDoc(mauInObject, filePath, templatePath);
                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else if (nhoman == Setting.MANHOMAN_DANSU)
            {
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_DS_PHUCTHAM_QDDUAVUANRAXETXU);                
                string filename = Setting.TEMPLATE_MAUIN_DS_PHUCTHAM_QDDUAVUANRAXETXU.Remove(Setting.TEMPLATE_MAUIN_DS_PHUCTHAM_QDDUAVUANRAXETXU.IndexOf("."));
                if(KetQuaXetXuService.ChiTietQuyetDinhTheoHoSoVuAnID(hoSoVuAnId, GiaiDoan.SoTham.GetHashCode())!=null)
                {
                    templatePath= Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_DS_PHUCTHAM_QDMOPHIENHOP_TC);
                    filename = Setting.TEMPLATE_MAUIN_DS_PHUCTHAM_QDMOPHIENHOP_TC.Remove(Setting.TEMPLATE_MAUIN_DS_PHUCTHAM_QDMOPHIENHOP_TC.IndexOf("."));
                }
                var mauInObject = MauInService.DuLieuMauInSo47PT(hoSoVuAnId);
                if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_DS_PHUCTHAM_QDMOPHIENHOP);
                    filename = Setting.TEMPLATE_MAUIN_DS_PHUCTHAM_QDMOPHIENHOP.Remove(Setting.TEMPLATE_MAUIN_DS_PHUCTHAM_QDMOPHIENHOP.IndexOf("."));
                }
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauIn14QuyetDinhDuaVuAnXetXuPhucThamDoc(mauInObject, filePath, templatePath);
                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else if (nhoman == Setting.MANHOMAN_HONNHAN)
            {
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HN_PHUCTHAM_QDDUAVUANRAXETXU);
                var mauInObject = MauInService.DuLieuMauInSo47PT(hoSoVuAnId);
                string filename = Setting.TEMPLATE_MAUIN_HN_PHUCTHAM_QDDUAVUANRAXETXU.Remove(Setting.TEMPLATE_MAUIN_HN_PHUCTHAM_QDDUAVUANRAXETXU.IndexOf("."));
                if (KetQuaXetXuService.ChiTietQuyetDinhTheoHoSoVuAnID(hoSoVuAnId, GiaiDoan.SoTham.GetHashCode()) != null)
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HN_PHUCTHAM_QDMOPHIENHOP_TC);
                    filename = Setting.TEMPLATE_MAUIN_HN_PHUCTHAM_QDMOPHIENHOP_TC.Remove(Setting.TEMPLATE_MAUIN_HN_PHUCTHAM_QDMOPHIENHOP_TC.IndexOf("."));
                }
                if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HN_PHUCTHAM_QDMOPHIENHOP);
                    filename = Setting.TEMPLATE_MAUIN_HN_PHUCTHAM_QDMOPHIENHOP.Remove(Setting.TEMPLATE_MAUIN_HN_PHUCTHAM_QDMOPHIENHOP.IndexOf("."));
                }
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauIn14QuyetDinhDuaVuAnXetXuPhucThamDoc(mauInObject, filePath, templatePath);
                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else if (nhoman == Setting.MANHOMAN_APDUNG_BPXLHC)
            {
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_AD_QUYETDINHMOPHIENHOP_PT);
                var mauInObject = MauInService.DuLieuMauInSo47PT(hoSoVuAnId);
                string filename = Setting.TEMPLATE_MAUIN_AD_QUYETDINHMOPHIENHOP_PT.Remove(Setting.TEMPLATE_MAUIN_AD_QUYETDINHMOPHIENHOP_PT.IndexOf("."));                
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauInADQuyetDinhMoPhienHopPhucThamDoc(mauInObject, filePath, templatePath);
                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else if (nhoman == Setting.MANHOMAN_KINHTE)
            {
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_KD_PHUCTHAM_QDDUAVUANRAXETXU);
                var mauInObject = MauInService.DuLieuMauInSo47PT(hoSoVuAnId);
                string filename = Setting.TEMPLATE_MAUIN_KD_PHUCTHAM_QDDUAVUANRAXETXU.Remove(Setting.TEMPLATE_MAUIN_KD_PHUCTHAM_QDDUAVUANRAXETXU.IndexOf("."));
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauIn14QuyetDinhDuaVuAnXetXuPhucThamDoc(mauInObject, filePath, templatePath);
                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else if (nhoman == Setting.MANHOMAN_LAODONG)
            {
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_LD_PHUCTHAM_QDDUAVUANRAXETXU);
                var mauInObject = MauInService.DuLieuMauInSo47PT(hoSoVuAnId);
                string filename = Setting.TEMPLATE_MAUIN_LD_PHUCTHAM_QDDUAVUANRAXETXU.Remove(Setting.TEMPLATE_MAUIN_LD_PHUCTHAM_QDDUAVUANRAXETXU.IndexOf("."));
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauIn14QuyetDinhDuaVuAnXetXuPhucThamDoc(mauInObject, filePath, templatePath);
                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MauInBiaHoSoNhanDonDoc(int hoSoVuAnId)
        {
            string folder = Server.MapPath(string.Format(Setting.MAU_IN_DOC, DateTime.Now.Year, DateTime.Now.Month));
            if (!Directory.Exists((folder)))
            {
                Directory.CreateDirectory(folder);
            }
            string folderTemplate = Server.MapPath(Setting.TEMPLATES_MAUIN_FOLDER);
            if (!Directory.Exists((folderTemplate)))
            {
                Directory.CreateDirectory(folderTemplate);
            }
            var nhoman = GetAnSessionInfo().MaNhomAn;
            if(nhoman==Setting.MANHOMAN_HANHCHINH)
            {
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HC_SOTHAM_BIAHOSONHANDON);
                var mauInObject = MauInService.ChiTietMauInBiaHoSoNhanDon(hoSoVuAnId);
                string filename = Setting.TEMPLATE_MAUIN_HC_SOTHAM_BIAHOSONHANDON.Remove(Setting.TEMPLATE_MAUIN_HC_SOTHAM_BIAHOSONHANDON.IndexOf("."));
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauInHCBiaHoSoNhanDonDoc(mauInObject, filePath, templatePath);

                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else if (nhoman == Setting.MANHOMAN_DANSU)
            {
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_DS_SOTHAM_BIAHOSONHANDON);
                var mauInObject = MauInService.ChiTietMauInBiaHoSoNhanDon(hoSoVuAnId);
                string filename = Setting.TEMPLATE_MAUIN_DS_SOTHAM_BIAHOSONHANDON.Remove(Setting.TEMPLATE_MAUIN_DS_SOTHAM_BIAHOSONHANDON.IndexOf("."));
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_DS_SOTHAM_BIAHOSONHANDON_YC);
                    filename = Setting.TEMPLATE_MAUIN_DS_SOTHAM_BIAHOSONHANDON_YC.Remove(Setting.TEMPLATE_MAUIN_DS_SOTHAM_BIAHOSONHANDON_YC.IndexOf("."));
                }
                var outputFiles = MauInService.MauIn01BiaHoSoNhanDonDoc(mauInObject, filePath, templatePath);

                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else if (nhoman == Setting.MANHOMAN_HONNHAN)
            {
                string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HN_SOTHAM_BIAHOSONHANDON);
                var mauInObject = MauInService.ChiTietMauInBiaHoSoNhanDon(hoSoVuAnId);
                string filename = Setting.TEMPLATE_MAUIN_HN_SOTHAM_BIAHOSONHANDON.Remove(Setting.TEMPLATE_MAUIN_HN_SOTHAM_BIAHOSONHANDON.IndexOf("."));
                if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HN_SOTHAM_BIAHOSONHANDON_YC);
                    filename = Setting.TEMPLATE_MAUIN_HN_SOTHAM_BIAHOSONHANDON_YC.Remove(Setting.TEMPLATE_MAUIN_HN_SOTHAM_BIAHOSONHANDON_YC.IndexOf("."));
                }
                string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                var outputFiles = MauInService.MauIn01BiaHoSoNhanDonDoc(mauInObject, filePath, templatePath);

                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MauInBiaHoSoDoc(int hoSoVuAnId)
        {
            string folder = Server.MapPath(string.Format(Setting.MAU_IN_DOC, DateTime.Now.Year, DateTime.Now.Month));
            if (!Directory.Exists((folder)))
            {
                Directory.CreateDirectory(folder);
            }
            string folderTemplate = Server.MapPath(Setting.TEMPLATES_MAUIN_FOLDER);
            if (!Directory.Exists((folderTemplate)))
            {
                Directory.CreateDirectory(folderTemplate);
            }
            var nhoman = GetAnSessionInfo().MaNhomAn;
            if(nhoman==Setting.MANHOMAN_HANHCHINH)
            {
                string filePath = "", templatePath = "";
                if (GetAnSessionInfo().GiaiDoanId != 2)
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HC_SOTHAM_BIAHOSO);
                    string filename = Setting.TEMPLATE_MAUIN_HC_SOTHAM_BIAHOSO.Remove(Setting.TEMPLATE_MAUIN_HC_SOTHAM_BIAHOSO.IndexOf("."));
                    filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                }
                else
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HC_PHUCTHAM_BIAHOSO);
                    string filename = Setting.TEMPLATE_MAUIN_HC_PHUCTHAM_BIAHOSO.Remove(Setting.TEMPLATE_MAUIN_HC_PHUCTHAM_BIAHOSO.IndexOf("."));
                    filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                }
                var mauInObject = MauInService.DuLieuMauInBiaHoSo(hoSoVuAnId);
                var outputFiles = MauInService.MauInHCBiaHoSoDoc(mauInObject, filePath, templatePath);
                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else if (nhoman == Setting.MANHOMAN_HINHSU)
            {
                string filePath = "", templatePath = "";
                string[] outputFiles;
                var mauInObject = MauInService.DuLieuMauInBiaHoSo(hoSoVuAnId);
                if (GetAnSessionInfo().GiaiDoanId != 2)
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HS_BIAHOSOSOTHAM);
                    string filename = Setting.TEMPLATE_MAUIN_HS_BIAHOSOSOTHAM.Remove(Setting.TEMPLATE_MAUIN_HS_BIAHOSOSOTHAM.IndexOf("."));
                    filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                    outputFiles = MauInService.MauInHSBiaHoSoSTDoc(mauInObject, filePath, templatePath);
                }
                else
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HS_BIAHOSOPHUCTHAM);
                    string filename = Setting.TEMPLATE_MAUIN_HS_BIAHOSOPHUCTHAM.Remove(Setting.TEMPLATE_MAUIN_HS_BIAHOSOPHUCTHAM.IndexOf("."));
                    filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                    outputFiles = MauInService.MauInHSBiaHoSoPTDoc(mauInObject, filePath, templatePath);
                }               
                
                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else if (nhoman == Setting.MANHOMAN_DANSU)
            {
                string filePath = "", templatePath = "";
                string[] outputFiles;
                var mauInObject = MauInService.DuLieuMauInBiaHoSo(hoSoVuAnId);
                if (GetAnSessionInfo().GiaiDoanId != 2)
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_DS_BIAHOSOSOTHAM_VUAN);
                    string filename = Setting.TEMPLATE_MAUIN_DS_BIAHOSOSOTHAM_VUAN.Remove(Setting.TEMPLATE_MAUIN_DS_BIAHOSOSOTHAM_VUAN.IndexOf("."));
                    if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                    {
                        templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_DS_BIAHOSOSOTHAM_VUVIEC);
                        filename = Setting.TEMPLATE_MAUIN_DS_BIAHOSOSOTHAM_VUVIEC.Remove(Setting.TEMPLATE_MAUIN_DS_BIAHOSOSOTHAM_VUVIEC.IndexOf("."));
                    }
                    filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));

                }
                else
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_DS_BIAHOSOPHUCTHAM_VUAN);
                    string filename = Setting.TEMPLATE_MAUIN_DS_BIAHOSOPHUCTHAM_VUAN.Remove(Setting.TEMPLATE_MAUIN_DS_BIAHOSOPHUCTHAM_VUAN.IndexOf("."));
                    if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                    {
                        templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_DS_BIAHOSOPHUCTHAM_VUVIEC);
                        filename = Setting.TEMPLATE_MAUIN_DS_BIAHOSOPHUCTHAM_VUVIEC.Remove(Setting.TEMPLATE_MAUIN_DS_BIAHOSOPHUCTHAM_VUVIEC.IndexOf("."));
                    }
                    filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                }
                outputFiles = MauInService.MauIn0716BiaHoSoDoc(mauInObject, filePath, templatePath);
                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else if (nhoman == Setting.MANHOMAN_HONNHAN)
            {
                string filePath = "", templatePath = "";
                string[] outputFiles;
                var mauInObject = MauInService.DuLieuMauInBiaHoSo(hoSoVuAnId);
                if (GetAnSessionInfo().GiaiDoanId != 2)
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HN_BIAHOSOSOTHAM_VUAN);
                    string filename = Setting.TEMPLATE_MAUIN_HN_BIAHOSOSOTHAM_VUAN.Remove(Setting.TEMPLATE_MAUIN_HN_BIAHOSOSOTHAM_VUAN.IndexOf("."));
                    if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                    {
                        templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HN_BIAHOSOSOTHAM_VUVIEC);
                        filename = Setting.TEMPLATE_MAUIN_HN_BIAHOSOSOTHAM_VUVIEC.Remove(Setting.TEMPLATE_MAUIN_HN_BIAHOSOSOTHAM_VUVIEC.IndexOf("."));
                    }
                    filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));

                }
                else
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HN_BIAHOSOPHUCTHAM_VUAN);
                    string filename = Setting.TEMPLATE_MAUIN_HN_BIAHOSOPHUCTHAM_VUAN.Remove(Setting.TEMPLATE_MAUIN_HN_BIAHOSOPHUCTHAM_VUAN.IndexOf("."));
                    if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                    {
                        templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HN_BIAHOSOPHUCTHAM_VUVIEC);
                        filename = Setting.TEMPLATE_MAUIN_HN_BIAHOSOPHUCTHAM_VUVIEC.Remove(Setting.TEMPLATE_MAUIN_HN_BIAHOSOPHUCTHAM_VUVIEC.IndexOf("."));
                    }
                    filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                }
                outputFiles = MauInService.MauIn0716BiaHoSoDoc(mauInObject, filePath, templatePath);
                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else if (nhoman == Setting.MANHOMAN_APDUNG_BPXLHC)
            {
                string filePath = "", templatePath = "";
                string[] outputFiles;
                var mauInObject = MauInService.DuLieuMauInBiaHoSo(hoSoVuAnId);
                if (GetAnSessionInfo().GiaiDoanId != 2)
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_AD_BIAHOSOSOTHAM);
                    string filename = Setting.TEMPLATE_MAUIN_AD_BIAHOSOSOTHAM.Remove(Setting.TEMPLATE_MAUIN_AD_BIAHOSOSOTHAM.IndexOf("."));
                    filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                    outputFiles = MauInService.MauInADBiaHoSoDoc(mauInObject, filePath, templatePath);
                }
                else
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_AD_BIAHOSOPHUCTHAM);
                    string filename = Setting.TEMPLATE_MAUIN_AD_BIAHOSOPHUCTHAM.Remove(Setting.TEMPLATE_MAUIN_AD_BIAHOSOPHUCTHAM.IndexOf("."));
                    filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                    outputFiles = MauInService.MauInADBiaHoSoDoc(mauInObject, filePath, templatePath);
                }

                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else if (nhoman == Setting.MANHOMAN_KINHTE)
            {
                string filePath = "", templatePath = "";
                string[] outputFiles;
                var mauInObject = MauInService.DuLieuMauInBiaHoSo(hoSoVuAnId);
                if (GetAnSessionInfo().GiaiDoanId != 2)
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_KD_BIAHOSOSOTHAM_VUAN);
                    string filename = Setting.TEMPLATE_MAUIN_KD_BIAHOSOSOTHAM_VUAN.Remove(Setting.TEMPLATE_MAUIN_KD_BIAHOSOSOTHAM_VUAN.IndexOf("."));
                    if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                    {
                        templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_KD_BIAHOSOSOTHAM_VUVIEC);
                        filename = Setting.TEMPLATE_MAUIN_KD_BIAHOSOSOTHAM_VUVIEC.Remove(Setting.TEMPLATE_MAUIN_KD_BIAHOSOSOTHAM_VUVIEC.IndexOf("."));
                    }
                    filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                    outputFiles = MauInService.MauInKDBiaHoSoDoc(mauInObject, filePath, templatePath);
                }
                else
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_KD_BIAHOSOPHUCTHAM_VUAN);
                    string filename = Setting.TEMPLATE_MAUIN_KD_BIAHOSOPHUCTHAM_VUAN.Remove(Setting.TEMPLATE_MAUIN_KD_BIAHOSOPHUCTHAM_VUAN.IndexOf("."));
                    if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                    {
                        templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_KD_BIAHOSOPHUCTHAM_VUVIEC);
                        filename = Setting.TEMPLATE_MAUIN_KD_BIAHOSOPHUCTHAM_VUVIEC.Remove(Setting.TEMPLATE_MAUIN_KD_BIAHOSOPHUCTHAM_VUVIEC.IndexOf("."));
                    }
                    filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                    outputFiles = MauInService.MauInKDBiaHoSoDoc(mauInObject, filePath, templatePath);
                }

                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else if (nhoman == Setting.MANHOMAN_LAODONG)
            {
                string filePath = "", templatePath = "";
                string[] outputFiles;
                var mauInObject = MauInService.DuLieuMauInBiaHoSo(hoSoVuAnId);
                if (GetAnSessionInfo().GiaiDoanId != 2)
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_LD_BIAHOSOSOTHAM_VUAN);
                    string filename = Setting.TEMPLATE_MAUIN_LD_BIAHOSOSOTHAM_VUAN.Remove(Setting.TEMPLATE_MAUIN_LD_BIAHOSOSOTHAM_VUAN.IndexOf("."));
                    if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                    {
                        templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_LD_BIAHOSOSOTHAM_VUVIEC);
                        filename = Setting.TEMPLATE_MAUIN_LD_BIAHOSOSOTHAM_VUVIEC.Remove(Setting.TEMPLATE_MAUIN_LD_BIAHOSOSOTHAM_VUVIEC.IndexOf("."));
                    }
                    filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                    outputFiles = MauInService.MauInKDBiaHoSoDoc(mauInObject, filePath, templatePath);
                }
                else
                {
                    templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_LD_BIAHOSOPHUCTHAM_VUAN);
                    string filename = Setting.TEMPLATE_MAUIN_LD_BIAHOSOPHUCTHAM_VUAN.Remove(Setting.TEMPLATE_MAUIN_LD_BIAHOSOPHUCTHAM_VUAN.IndexOf("."));
                    if (mauInObject.LoaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                    {
                        templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_LD_BIAHOSOPHUCTHAM_VUVIEC);
                        filename = Setting.TEMPLATE_MAUIN_LD_BIAHOSOPHUCTHAM_VUVIEC.Remove(Setting.TEMPLATE_MAUIN_LD_BIAHOSOPHUCTHAM_VUVIEC.IndexOf("."));
                    }
                    filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
                    outputFiles = MauInService.MauInKDBiaHoSoDoc(mauInObject, filePath, templatePath);
                }

                if (outputFiles != null)
                    return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MauInQDPhanCongHT(int hoSoVuAnId)
        {
            string folder = Server.MapPath(string.Format(Setting.MAU_IN_DOC, DateTime.Now.Year, DateTime.Now.Month));
            if (!Directory.Exists((folder)))
            {
                Directory.CreateDirectory(folder);
            }
            string folderTemplate = Server.MapPath(Setting.TEMPLATES_MAUIN_FOLDER);
            if (!Directory.Exists((folderTemplate)))
            {
                Directory.CreateDirectory(folderTemplate);
            }
            string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HS_QUYETDINHPHANCONGHOITHAM);
            string filename = Setting.TEMPLATE_MAUIN_HS_QUYETDINHPHANCONGHOITHAM.Remove(Setting.TEMPLATE_MAUIN_HS_QUYETDINHPHANCONGHOITHAM.IndexOf("."));
            string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
            var mauInObject = MauInService.DuLieuMauInQuyetDinhPCTP(hoSoVuAnId,1);
            var outputFiles = MauInService.MauInHSQuyetDinhPCHTTKDoc(mauInObject, filePath, templatePath);
            if (outputFiles != null)
                return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MauInQDPhanCongTK(int hoSoVuAnId)
        {
            string folder = Server.MapPath(string.Format(Setting.MAU_IN_DOC, DateTime.Now.Year, DateTime.Now.Month));
            if (!Directory.Exists((folder)))
            {
                Directory.CreateDirectory(folder);
            }
            string folderTemplate = Server.MapPath(Setting.TEMPLATES_MAUIN_FOLDER);
            if (!Directory.Exists((folderTemplate)))
            {
                Directory.CreateDirectory(folderTemplate);
            }
            string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_HS_QUYETDINHPHANCONGTHUKY);
            string filename = Setting.TEMPLATE_MAUIN_HS_QUYETDINHPHANCONGTHUKY.Remove(Setting.TEMPLATE_MAUIN_HS_QUYETDINHPHANCONGTHUKY.IndexOf("."));
            string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
            var mauInObject = MauInService.DuLieuMauInQuyetDinhPCTP(hoSoVuAnId, 1);
            var outputFiles = MauInService.MauInHSQuyetDinhPCHTTKDoc(mauInObject, filePath, templatePath);
            if (outputFiles != null)
                return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MauInQDDinhChiDoc(int hoSoVuAnId, string loai)
        {
            string folder = Server.MapPath(string.Format(Setting.MAU_IN_DOC, DateTime.Now.Year, DateTime.Now.Month));
            if (!Directory.Exists((folder)))
            {
                Directory.CreateDirectory(folder);
            }
            string folderTemplate = Server.MapPath(Setting.TEMPLATES_MAUIN_FOLDER);
            if (!Directory.Exists((folderTemplate)))
            {
                Directory.CreateDirectory(folderTemplate);
            }

            string templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_AD_QUYETDINHDINHCHI);
            string filename = Setting.TEMPLATE_MAUIN_AD_QUYETDINHDINHCHI.Remove(Setting.TEMPLATE_MAUIN_AD_QUYETDINHDINHCHI.IndexOf("."));
            if(loai== "tamdinhchi")
            {
                templatePath = Path.Combine(folderTemplate, Setting.TEMPLATE_MAUIN_AD_QUYETDINHTAMDINHCHI);
                filename = Setting.TEMPLATE_MAUIN_AD_QUYETDINHTAMDINHCHI.Remove(Setting.TEMPLATE_MAUIN_AD_QUYETDINHTAMDINHCHI.IndexOf("."));
            }
            string filePath = Path.Combine(folder, string.Format(filename + @"-{0}.docx", DateTime.Now.ToString("ddMMyyyyhhmmssfff")));
            var mauInObject = MauInService.ChiTietMauInQuyetDinhDinhChi(hoSoVuAnId, loai);
            var outputFiles = MauInService.MauInADQuyetDinhDinhChiDoc(mauInObject, filePath, templatePath);
            if (outputFiles != null)
                return Json(new { success = true, filePath = outputFiles }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { success = false, filePath = "" }, JsonRequestBehavior.AllowGet);
        }
    }
}