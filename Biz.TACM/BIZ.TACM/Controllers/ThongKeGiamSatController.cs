using Biz.Lib.TACM.Resources.Resources;
using Biz.Lib.TACM.ThongKeGiamSat.Models;
using Biz.TACM.IServices;
using Biz.TACM.Models.ViewModel.Shared;
using Biz.TACM.Models.ViewModel.ThongKeGiamSat;
using Biz.TACM.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Biz.TACM.Enums;
using Biz.Lib.Helpers;
using Microsoft.Ajax.Utilities;

namespace Biz.TACM.Controllers
{
    public class ThongKeGiamSatController : BizController
    {
        private ISettingDataService _settingDataService;
        private ISettingDataService SettingDataService => _settingDataService ?? (_settingDataService = new SettingDataService());

        private IThongKeGiamSatService _thongKeGiamSatService;
        private IThongKeGiamSatService ThongKeGiamSatService => _thongKeGiamSatService ?? (_thongKeGiamSatService = new ThongKeGiamSatService());

        #region Giam Sat
        public ActionResult GiamSatIndex()
        {
            var viewModel = new GiamSatLocDuLieuViewModel();
            viewModel.TuNgay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToShortDateString();
            viewModel.DenNgay = DateTime.Now.ToShortDateString();

            //var listToaAn = SettingDataService.DanhSachToaAn();
            //var listItem = listToaAn.Select(
            //    x => new SelectListItem
            //    {
            //        Text = x.TenToaAn,
            //        Value = x.ToaAnID.ToString()
            //    });

            ViewBag.ddlToaAn = SettingDataService.SelectListDanhSachToaAnValueIsToaAnID(null);      //new SelectList(listItem, "Value", "Text");
            ViewBag.ddlNhomAn = SettingDataService.DanhSachNhomAnChoDDL("DINHNGHIACHUNG_NHOMANTOANAN", null);
            ViewBag.ddlGiaiDoan = SettingDataService.SettingDataItemHaveValueText("DINHNGHIACHUNG_GIAIDOANHOSO", null);
            return View("GiamSat/GiamSatIndex", viewModel);
        }

        public ActionResult DanhSachHoSoVuAnGiamSat(string listHoSoVuAnID)
        {
            var viewModel = ThongKeGiamSatService.GetDanhSachHoSoVuAnGiamSat(listHoSoVuAnID);

            return Json(new { data = viewModel }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult HoSoVuAnGiamSatTable()
        {
            return PartialView("GiamSat/_HoSoVuAnGiamSatTable");
        }


        public ActionResult LocDuLieuGiamSat(GiamSatLocDuLieuViewModel viewModel)
        {
            var result = ThongKeGiamSatService.LocDuLieuGiamSat(viewModel);

            var model = new ChartViewModel
            {
                Data = string.Join(", ", result.ListData.Select(x => x.Amount).ToArray()),
                Label = "'" + string.Join("    ', '", result.ListData.Select(x => x.GroupName).ToArray()) + "    '",
                Title = "",
                PointLabels = "'" + string.Join("', '", result.ListData.Select(x => x.Amount).ToArray()) + "'"
            };

            ViewBag.ListHoSoVuAnID = result.ListHoSoVuAnID;
            return PartialView("GiamSat/_BieuDoGiamSat", model);
        }
        #endregion

        #region Thong Ke
        public ActionResult BaoCaoThongKe()
        {
            var viewModel = new LocDuLieuThongKeModel();
            var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            viewModel.TuNgay = firstDayOfMonth.ToString("dd/MM/yyyy");
            viewModel.DenNgay = DateTime.Now.ToString("dd/MM/yyyy");

            ViewBag.LoaiThongKe = ThongKe.TongHop.GetHashCode();
            ViewBag.ddlToaAn = SettingDataService.SelectListDanhSachToaAnValueIsToaAnID(null);
            ViewBag.ddlNhomAn = SettingDataService.SettingDataItemHaveValueText("DINHNGHIACHUNG_NHOMANTOANAN", "");
            ViewBag.ddlGiaiDoan = SettingDataService.SettingDataItemHaveValueText("DINHNGHIACHUNG_GIAIDOANHOSO", "");
            ViewBag.ddlLoaiQuanHe = new SelectList(SettingDataService.ListSettingDataItem("DS_SOTHAM_LOAIQUANHE"), "ItemText", "ItemText");
            ViewBag.KhieuKienddl = new SelectList(SettingDataService.ListSettingDataItem("HC_SOTHAM_KHIEUKIEN"), "ItemText", "ItemText"); 
            ViewBag.DSTC = new SelectList(SettingDataService.ListSettingDataItem("DS_SOTHAM_QHPL_TRANHCHAP"), "ItemText", "ItemText"); 
            ViewBag.DSYC = new SelectList(SettingDataService.ListSettingDataItem("DS_SOTHAM_QHPL_YEUCAU"), "ItemText", "ItemText");
            ViewBag.HNTC = new SelectList(SettingDataService.ListSettingDataItem("HN_SOTHAM_QHPL_TRANHCHAP"), "ItemText", "ItemText"); 
            ViewBag.HNYC = new SelectList(SettingDataService.ListSettingDataItem("HN_SOTHAM_QHPL_YEUCAU"), "ItemText", "ItemText");
            ViewBag.KTTC = new SelectList(SettingDataService.ListSettingDataItem("KT_SOTHAM_QHPL_TRANHCHAP"), "ItemText", "ItemText");
            ViewBag.KTYC = new SelectList(SettingDataService.ListSettingDataItem("KT_SOTHAM_QHPL_YEUCAU"), "ItemText", "ItemText");
            ViewBag.LDTC = new SelectList(SettingDataService.ListSettingDataItem("LD_SOTHAM_QHPL_TRANHCHAP"), "ItemText", "ItemText");
            ViewBag.LDYC = new SelectList(SettingDataService.ListSettingDataItem("LD_SOTHAM_QHPL_YEUCAU"), "ItemText", "ItemText");
            ViewBag.KhieuKienddl = new SelectList(SettingDataService.ListSettingDataItem("HC_SOTHAM_KHIEUKIEN"), "ItemText", "ItemText");
            ViewBag.ddlLoaiDeNghi = new SelectList(SettingDataService.ListSettingDataItem("AD_SOTHAM_BIENPHAPXULYHANHCHINH"), "ItemText", "ItemText");
            var temp1 = SettingDataService.ListSettingDataItem("HS_SOTHAM_TOIDANH").Where(x => x.ItemText.Contains("Điều"));
            foreach (var item in temp1)
            {
                string str = item.ItemText.Substring(0, item.ItemText.IndexOf("."));
                string str2 = item.ItemText.Substring(item.ItemText.IndexOf(".") + 2);
                item.ItemText = str;
                item.ItemRemark = str2;
            }            
            ViewBag.ddlToiDanh = new SelectList(temp1, "ItemRemark", "ItemRemark");
            return View("ThongKe/BaoCaoThongKe", viewModel);
        }

        public ActionResult ChiTietBaoCaoThongKe(int loaiThongKe)
        {
            ViewBag.LoaiThongKe = loaiThongKe;
            if (loaiThongKe == ThongKe.TongHop.GetHashCode())
            {
                return PartialView("ThongKe/_ThongKeTongHop");
            }
            if(loaiThongKe == ThongKe.NhomAn.GetHashCode())
            {
                return PartialView("ThongKe/_ThongKeNhomAn");
            }
            if (loaiThongKe == ThongKe.ThamPhan.GetHashCode())
            {
                return PartialView("ThongKe/_ThongKeThamPhan");
            }
            if (loaiThongKe == ThongKe.ThuKy.GetHashCode())
            {
                return PartialView("ThongKe/_ThongKeThuKy");
            }
            return PartialView("ThongKe/_ThongKeTongHop");
        }

        public ActionResult ChiTietBaoCaoThongKeTable(int loaiThongKe, string tuNgay, string denNgay, int? toaAnID = null, string nhomAn = null, int? giaiDoan = null, string loaiQuanHe = null, string loaiDeNghi = null, string quanHePhapLuat = null, string toiDanh = null)
        {
            var viewModel = ThongKeGiamSatService.ThongKeTongHop(loaiThongKe, tuNgay, denNgay, toaAnID, nhomAn, giaiDoan, loaiQuanHe, loaiDeNghi, quanHePhapLuat, toiDanh);
            if (loaiThongKe == ThongKe.ThamPhan.GetHashCode())
                viewModel = viewModel.OrderBy(x => x.TenThamPhan.TrimStart());
            if (loaiThongKe == ThongKe.ThuKy.GetHashCode())
                viewModel = viewModel.OrderBy(x => x.TenThuKy.TrimStart());
            return Json(new { data = viewModel }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DanhSachHoSoBaoCaoThongKe(string danhSachId)
        {
            var viewModel = ThongKeGiamSatService.DanhSachHoSoBaoCaoThongKe(danhSachId);    
            return Json(new { data = viewModel }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DanhSachHoSoModal(string danhSachId, string tuNgay, string denNgay, int? toaAnID)
        {
            ViewBag.TuNgay = tuNgay;
            ViewBag.DenNgay = denNgay;
            ViewBag.ToaAnID = toaAnID;
            ViewBag.DanhSachID = danhSachId;
            return PartialView("ThongKe/_DanhSachHoSo");
        }

        [HttpPost]
        public ActionResult XuatExcelDanhSachHoSoThongKe(string danhSachId)
        {
            var fileName = string.Format(Setting.THONGKE_DANHSACHHOSO_FILENAME, DateTime.Now.ToString("ddMMyyyyhhmmssfff"));            
            var content = ThongKeGiamSatService.ExportDanhSachHoSoThongKe(danhSachId);

            return File(content, "application/vnd.ms-excel", fileName);           
        }

        public ActionResult XuatExcelChiTienXetXu(string tuNgay, string denNgay, int? toaAnID = null)
        {
            var fileName = string.Format(Setting.THONGKE_CHITIENXETXU_FILENAME, DateTime.Now.ToString("ddMMyyyyhhmmssfff"));
            var content = ThongKeGiamSatService.ExportChiTienXetXu(tuNgay, denNgay, toaAnID);

            return File(content, "application/vnd.ms-excel", fileName);
        }

        public ActionResult ThongKePhanCongThamPhan()
        {
            //var thoiGian = SettingDataService.SettingDataItem("THONGKE_PCTP_TUNGAY_DENNGAY", "");
            var viewModel = new LocDuLieuThongKeModel
            {
                TuNgay = DateTime.MinValue.ToString("dd/MM/yyyy"),//thoiGian.First().Text,
                DenNgay = DateTime.Now.ToString("dd/MM/yyyy"),//thoiGian.Last().Text,
                ToaAnID = GetAnSessionInfo().ToaAnId
            };
            ViewBag.ddlToaAn = SettingDataService.SelectListDanhSachToaAnValueIsToaAnID(null);
            return View("ThongKe/PhanCongThamPhan/ThongKePhanCongThamPhan", viewModel);
        }

        public ActionResult DanhSachHoSoChuaPhanCongThamPhanTable(string tuNgay, string denNgay, int toaAnId)
        {
            var viewModel = ThongKeGiamSatService.DanhSachHoSoChuaPhanCongThamPhan(tuNgay, denNgay, toaAnId);
            return Json(new { data = viewModel }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChiTietThongKeTrucTuyenPCTPTable(string tuNgay, string denNgay, int toaAnId)
        {
            var viewModel = ThongKeGiamSatService.ThongKeTrucTuyenPhanCongThamPhan(tuNgay, denNgay, toaAnId);
            if (toaAnId == ToaAn.TinhCaMau.GetHashCode())
            {
                return PartialView("ThongKe/PhanCongThamPhan/_DuLieuThamPhanOnline", viewModel);
            }
            return PartialView("ThongKe/PhanCongThamPhan/_DuLieuThamPhanOnlineSoTham", viewModel);
            // return Json(new { data = viewModel }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ThongKeLuuKho

        public ActionResult ThongKeLuuKho()
        {
            var viewModel = new LocDuLieuThongKeModel();
            var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            viewModel.TuNgay = firstDayOfMonth.ToString("dd/MM/yyyy");
            viewModel.DenNgay = DateTime.Now.ToString("dd/MM/yyyy");

            ViewBag.ddlToaAn = SettingDataService.SelectListDanhSachToaAnValueIsToaAnID(null);
            ViewBag.ddlNhomAn = SettingDataService.SettingDataItemHaveValueText("DINHNGHIACHUNG_NHOMANTOANAN", "");
            ViewBag.ddlGiaiDoan = SettingDataService.SettingDataItemHaveValueText("DINHNGHIACHUNG_GIAIDOANHOSO", "");

            return View("ThongKe/ThongKeLuuKho/ThongKeLuuKho", viewModel);
        }

        public ActionResult BieuDoThongKeLuuKho(int loaiThongKe, string tuNgay, string denNgay, int group, int? toaAnId = null, string nhomAn = null, int? giaiDoan = null)
        { 
            string listHoSoVuAnId = "";
            var result = ThongKeGiamSatService.DuLieuBieuDoThongKeLuuKho(ref listHoSoVuAnId, tuNgay, denNgay, group, toaAnId, nhomAn, giaiDoan);

            if (loaiThongKe == 2)
                return RedirectToAction("DanhSachHoSoThongKeLuuKho", new { listHoSoVuAnId });

            var model = new ChartViewModel
            {
                Data = string.Join(", ", result.ListData.Select(x => x.SoLuongHoSo).ToArray()),
                Label = "'" + string.Join("    ', '", result.ListData.Select(x => x.TenNhom).ToArray()) + "    '",
                Title = "",
                PointLabels = "'" + string.Join("', '", result.ListData.Select(x => x.SoLuongHoSo).ToArray()) + "'"
            };

            ViewBag.ListHoSoVuAnID = result.ListHoSoVuAnID;
            return PartialView("ThongKe/ThongKeLuuKho/_BieuDoThongKeLuuKho", model);
        }

        public ActionResult DanhSachHoSoThongKeLuuKhoTable(string listHoSoVuAnId)
        {
            var viewModel = ThongKeGiamSatService.DanhSachHoSoThongKeLuuKho(listHoSoVuAnId);
            return Json(new { data = viewModel }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DanhSachHoSoThongKeLuuKho(string listHoSoVuAnId)
        {
            ViewBag.ListHoSoVuAnID = listHoSoVuAnId;
            return PartialView("ThongKe/ThongKeLuuKho/_DanhSachHoSo");
        }

        public ActionResult XuatExcelDanhSachHoSoThongKeLuuKho(string danhSachId)
        {
            var fileName = string.Format(Setting.THONGKELUUKHO_DANHSACHHOSO_FILENAME, DateTime.Now.ToString("ddMMyyyyhhmmssfff"));
            var content = ThongKeGiamSatService.ExportThongKeLuuKho(danhSachId);

            return File(content, "application/vnd.ms-excel", fileName);
        }

        #endregion

        #region ThongKeAnHuySua

        public ActionResult ThongKeAnHuySua()
        {
            var viewModel = new LocDuLieuThongKeModel();
            var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            viewModel.TuNgay = firstDayOfMonth.ToString("dd/MM/yyyy");
            viewModel.DenNgay = DateTime.Now.ToString("dd/MM/yyyy");

            ViewBag.ddlToaAn = SettingDataService.SelectListDanhSachToaAnValueIsToaAnID(null);
            ViewBag.ddlThamPhan = SettingDataService.DanhSachNhanVienTheoChucDanh(Setting.CHUCDANH_THAMPHAN, null, "");

            return View("ThongKe/ThongKeAnHuySua/ThongKeAnHuySua", viewModel);
        }

        public ActionResult BieuDoThongKeAnHuySua(int loaiThongKe, string tuNgay, string denNgay, int? toaAnId = null, string thamPhan = null)
        {
            string listHoSoHuyId = "";
            string listHoSoSuaId = "";
            var viewModel = ThongKeGiamSatService.DuLieuBieuDoThongKeAnHuySua(ref listHoSoHuyId, ref listHoSoSuaId, tuNgay, denNgay, toaAnId, thamPhan);

            if (loaiThongKe == 2)
                return RedirectToAction("DanhSachHoSoThongKeAnHuySua", new { listHoSoHuyId, listHoSoSuaId });
            
            ViewBag.ListHoSoHuyID = listHoSoHuyId;
            ViewBag.ListHoSoSuaID = listHoSoSuaId;
            return PartialView("ThongKe/ThongKeAnHuySua/_BieuDo");
        }

        public ActionResult BieuDoThongKeAnHuySuaTable(string tuNgay, string denNgay, int? toaAnId = null, string thamPhan = null)
        {
            string listHoSoHuyId = "";
            string listHoSoSuaId = "";
            var viewModel = ThongKeGiamSatService.DuLieuBieuDoThongKeAnHuySua(ref listHoSoHuyId, ref listHoSoSuaId, tuNgay, denNgay, toaAnId, thamPhan);
            return Json(new { data = viewModel.ListData }, JsonRequestBehavior.AllowGet);
        }
     
        public ActionResult DanhSachHoSoThongKeAnHuySua(string listHoSoHuyId, string listHoSoSuaId)
        {
            ViewBag.ListHoSoHuyID = listHoSoHuyId;
            ViewBag.ListHoSoSuaID = listHoSoSuaId;
            return PartialView("ThongKe/ThongKeAnHuySua/_DanhSach");
        }

        public ActionResult DanhSachHoSoThongKeAnHuySuaTable(string listHoSoHuyId, string listHoSoSuaId)
        {
            var viewModel = ThongKeGiamSatService.DanhSachHoSoThongKeAnHuySua(listHoSoHuyId, listHoSoSuaId);
            return Json(new { data = viewModel }, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult XemThongKeAnHuySua(int loaiThongKe, string listHoSoHuyId, string listHoSoSuaId)
        //{
        //    if (loaiThongKe == 2)
        //        return RedirectToAction("DanhSachHoSoThongKeAnHuySua", new { listHoSoHuyId, listHoSoSuaId });
        //    return RedirectToAction("BieuDoThongKeAnHuySua", new { listHoSoHuyId, listHoSoSuaId });
        //}

        public JsonResult DanhSachThamPhanTheoToaAn(int toaAnId)
        {
            var listThamPhan = SettingDataService.DanhSachNhanVienTheoChucDanh(Setting.CHUCDANH_THAMPHAN, toaAnId, "");
            return Json(listThamPhan, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}