using System;
using Biz.Lib.TACM.QuanLyNhanVien.Model;
using Biz.TACM.Infrastructure.Attributes;
using Biz.TACM.IServices;
using Biz.TACM.Services;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Biz.Lib.Helpers;
using Biz.TACM.Models.ViewModel.Shared;
using Newtonsoft.Json;
using System.Web.UI.WebControls;
using System.IO;
using Biz.Lib.TACM.Resources.Resources;
using Biz.TACM.Models.ViewModel.QuanLyNhanVien;
using Biz.Lib.Authentication;
using Biz.TACM.Models.ViewModel.TaiKhoanNhanVien;
using static Biz.TACM.Models.ViewModel.QuanLyNhanVien.NhanVienViewModel;

namespace Biz.TACM.Controllers
{
    public class QuanLyNhanVienController : BizController
    {

        private ISettingDataService _settingDataService;
        private ISettingDataService SettingDataService => _settingDataService ?? (_settingDataService = new SettingDataService());
        private readonly IQuanLyNhanVienService _quanLyNhanVienService;
        public QuanLyNhanVienController(IQuanLyNhanVienService quanLyNhanVienService)
        {
            _quanLyNhanVienService = quanLyNhanVienService;
        }

        private ITaiKhoanNhanVienService _taiKhoanNhanVienService;
        private ITaiKhoanNhanVienService TaiKhoanNhanVienService => _taiKhoanNhanVienService ?? (_taiKhoanNhanVienService = new TaiKhoanNhanVienService());

        public ActionResult Index()
        {
            int id = GetAnSessionInfo().ToaAnId;
            ViewBag.listToaAn = SettingDataService.SelectListDanhSachToaAnValueIsToaAnIDTheoNV(id, AccountUtils.CurrentUsername());
            ViewBag.ToaAnID = id;
            var viewmodel = _quanLyNhanVienService.DanhsachNhanVienTheoToaAn(id);
            return View(viewmodel);
        }
        public ActionResult Danhsachnhanvientheotoaan(int id)
        {
            var viewmodel = _quanLyNhanVienService.DanhsachNhanVienTheoToaAn(id);
            return PartialView("~/Views/QuanLyNhanVien/_NhanvienItem.cshtml", viewmodel);
        }

        [HttpGet]
        public ActionResult ThemNhanVien(int toaAnId)
        {
            NhanVienViewModel viewmodel = new NhanVienViewModel();
            var listSDTC = _quanLyNhanVienService.DanhSachChucDanh(toaAnId);
            ViewBag.listChucDanh = new SelectList(listSDTC.Where(x=>x.Loai !=2), "SoDoToChucID", "ChucDanh");
            ViewBag.listChucVu = new SelectList(listSDTC.Where(x => x.Loai == 2), "SoDoToChucID", "ChucDanh");
            ViewBag.listGioiTinh = new SelectList(XMLUtils.BindData("gender"), "value", "text");
            ViewBag.listhoatdong = new SelectList(XMLUtils.BindData("trangthaihoatdong"), "Value", "Text", 2);

            viewmodel.ChucNangNhanVien = TaiKhoanNhanVienService.DanhSachChucNangTheoMaNhanVien(null);

            return PartialView("~/Views/QuanLyNhanVien/_ThemNhanVien.cshtml", viewmodel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemNhanVien(NhanVienViewModel viewModel)
        {
            try
            {
                ModelState.Remove("MaNVMoi");
                var model = _quanLyNhanVienService.GetManVTheoEmail(viewModel.Email);
                if (model != null)
                {
                    ModelState.AddModelError("Email", NotifyMessage.THONGBAO_EMAIL_DATONTAI);
                }
                var model2 = SettingDataService.ChiTietNhanVienTheoMaNhanVien(viewModel.MaNV);
                if (model2 != null)
                {
                    ModelState.AddModelError("MaNV", string.Format(NotifyMessage.MANVDATONTAI, viewModel.MaNV));
                }
                if (ModelState.IsValid)
                {
                    ResponseResult result = _quanLyNhanVienService.ThemNhanVien(viewModel);
                    if (result != null && result.ResponseCode == 1)
                    {
                        if (viewModel.ChucNangNhanVien.ChucNangChinh != null || viewModel.ChucNangNhanVien.ListChucNang.Where(x => x.IsHaveChucNang == true).FirstOrDefault() != null)
                        {
                            foreach (var chucNang in viewModel.ChucNangNhanVien.ListChucNang)
                            {
                                chucNang.NhanVienID = result.NhanVienIDMoi;
                            }
                            ResponseResult resultChucNang = _quanLyNhanVienService.CapNhatChucNangNhanVien(viewModel.ChucNangNhanVien);
                            if (resultChucNang != null && resultChucNang.ResponseCode == 1)
                            {
                                return Json(JsonResponseViewModel.CreateSuccess());
                            }
                            return Json(JsonResponseViewModel.CreateFail(NotifyMessage.THONGBAO_KHONGCAPNHAT_CHUCNANG));
                        }
                        else
                        {
                            return Json(JsonResponseViewModel.CreateSuccess());
                        }

                    }
                    else if (result != null && result.ResponseCode == -1)
                    {
                        return Json(JsonResponseViewModel.CreateFail(result.ResponseMessage));
                    }
                    return Json(JsonResponseViewModel.CreateFail(NotifyMessage.THONGBAO_THEMNHANVIEN_KHONGTHANHCONG));
                }
                var listSDTC = _quanLyNhanVienService.DanhSachChucDanh(viewModel.ToaAnID);
                ViewBag.listChucDanh = new SelectList(listSDTC.Where(x => x.Loai != 2), "SoDoToChucID", "ChucDanh");
                ViewBag.listChucVu = new SelectList(listSDTC.Where(x => x.Loai == 2), "SoDoToChucID", "ChucDanh");
                ViewBag.listGioiTinh = new SelectList(XMLUtils.BindData("gender"), "value", "text");
                ViewBag.listhoatdong = new SelectList(XMLUtils.BindData("trangthaihoatdong"), "Value", "Text", 2);

                return PartialView("~/Views/QuanLyNhanVien/_ThemNhanVien.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }
        public ActionResult ChiTietNhanVien(int id)
        {
            ViewBag.dschucnang = _quanLyNhanVienService.ChitietNhanVienChucNang(id);
            var viewModel = _quanLyNhanVienService.ChitietNhanVien(id);
            var listSDTC = _quanLyNhanVienService.DanhSachChucDanh(viewModel.ToaAnID);
            ViewBag.listChucDanh = new SelectList(listSDTC.Where(x => x.Loai != 2), "SoDoToChucID", "ChucDanh");
            ViewBag.listChucVu = new SelectList(listSDTC.Where(x => x.Loai == 2), "SoDoToChucID", "ChucDanh");
            return PartialView("~/Views/QuanLyNhanVien/_ChiTietNhanVien.cshtml", viewModel);
        }

        public ActionResult CapNhatNhanVien(int id, int toaanid)
        {
            var viewModel = _quanLyNhanVienService.ChitietNhanVien(id);
            //ViewBag.listChucDanh = new SelectList(_quanLyNhanVienService.DanhSachChucDanh(), "SoDoToChucID", "ChucDanh");
            var listSDTC = _quanLyNhanVienService.DanhSachChucDanh(toaanid);
            ViewBag.listChucDanh = new SelectList(listSDTC.Where(x => x.Loai != 2), "SoDoToChucID", "ChucDanh");
            ViewBag.listChucVu = new SelectList(listSDTC.Where(x => x.Loai == 2), "SoDoToChucID", "ChucDanh");
            ViewBag.listGioiTinh = new SelectList(XMLUtils.BindData("gender"), "value", "text");
            ViewBag.listhoatdong = new SelectList(XMLUtils.BindData("trangthaihoatdong"), "value", "text");
            return PartialView("~/Views/QuanLyNhanVien/_CapNhatNhanVien.cshtml", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CapNhatNhanVien(NhanVienViewModel viewModel)
        {
            try
            {
                ResponseResult result = _quanLyNhanVienService.SuaNhanVien(viewModel);
                if (result != null && result.ResponseCode == 1)
                {
                    var NvSession = SettingDataService.GetNhanVienSessionInfo();
                    if (viewModel.NhanvienID == NvSession.NhanvienID && result.ResponseMessage!=viewModel.TenNV)
                    {
                        SettingDataService.UpdateNhanVienSessionInfo(NvSession.NhanvienID, viewModel.MaNV, viewModel.MaNVMoi, viewModel.HoNV, viewModel.TenLotNV, viewModel.TenNV, viewModel.HoVaTen, NvSession.DuongDanHinhDaiDien);
                    }
                    return Json(JsonResponseViewModel.CreateSuccess());
                }
                if (result != null && result.ResponseCode == -1)
                {
                    return Json(JsonResponseViewModel.CreateFail(result.ResponseMessage));
                }
                return Json(JsonResponseViewModel.CreateFail(NotifyMessage.THONGBAO_KHONGSUA_NHANVIEN));
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }

        public ActionResult XoaNhanVien(int id)
        {
            return PartialView("~/Views/QuanLyNhanVien/_XoaNhanVien.cshtml", id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult XoaNhanVienConfirmed(int nhanVienID)
        {
            try
            {
                ResponseResult result = _quanLyNhanVienService.XoaNhanVien(nhanVienID);
                if (result != null && result.ResponseCode == 1)
                {
                    return Json(JsonResponseViewModel.CreateSuccess());
                }
                return Json((JsonResponseViewModel.CreateFail(NotifyMessage.THONGBAO_KHONGXOA_NHANVIEN)));
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }

        //public ActionResult Capnhathinhdaidien(int id)
        //{
        //    return PartialView("~/Views/QuanLyNhanVien/_Capnhathinhdaidien.cshtml", id);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CapNhatHinhDaiDien(string maNV, HttpPostedFileBase file)
        {
            try
            {
                ResponseResult result = TaiKhoanNhanVienService.CapNhatHinhDaiDien(maNV, file);
                if (result != null && result.ResponseCode == 1)
                {
                    var NvSession = SettingDataService.GetNhanVienSessionInfo();
                    if (maNV== NvSession.MaNVMoi)
                    {
                        SettingDataService.UpdateNhanVienSessionInfo(NvSession.NhanvienID, NvSession.MaNV, maNV, NvSession.HoNV, NvSession.TenLotNV, NvSession.TenNV, NvSession.HoVaTen, result.ResponseMessage);
                    }
                    return Json(JsonResponseViewModel.CreateSuccess());
                }
                return Json(JsonResponseViewModel.CreateFail(NotifyMessage.THONGBAO_KHONGCAPNHAT_HINHDAIDIEN));
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }

        public ActionResult ImportNhanVien()
        {
            return PartialView("~/Views/QuanLyNhanVien/_ImportNhanVien.cshtml");
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult ImportNhanVien(ImportNhanVienViewModel file_imp, int toaAnID)
        {
            try
            {
                var c = Request.Files[0];
                var result = _quanLyNhanVienService.ValidateExcel(c,toaAnID);
                if (!string.IsNullOrEmpty(result))
                {
                    return Json(JsonResponseViewModel.CreateFail(result));
                }

                var responseCode = _quanLyNhanVienService.ImportNhanVien(c, toaAnID);
                if (responseCode == ResponseCode.Error.GetHashCode())
                {
                    return Json(JsonResponseViewModel.CreateFail(NotifyMessage.THEMNHANVIEN_TU_FILE_KHONGTHANHCONG));
                }

                Success(NotifyMessage.THEMNHANVIEN_TU_FILE_THANHCONG);
                return Json(JsonResponseViewModel.CreateSuccess());

            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }

        public ActionResult ThuKyTheoThamPhan(string manv,int idta)
        {
            var nvtktp = _quanLyNhanVienService.DanhsachThuKyThamPhan(manv);
            var model = new NhanVienViewModel();
            model.MaNV = manv;
            model.ToaAnID = idta;
            var nvtk = _quanLyNhanVienService.DanhSachNhanVienTheoChucDanh(idta,"Thư Ký");
            var nvttv= _quanLyNhanVienService.DanhSachNhanVienTheoChucDanh(idta, "Thẩm tra viên");
            var nv = new List<NhanVienModel>(nvtk.Count() + nvttv.Count());
            nv.AddRange(nvtk);
            nv.AddRange(nvttv);
            var thuky = new List<NhanVienThuKy>();
            foreach (var item in nv)
            {
                NhanVienThuKy temp = new NhanVienThuKy();
                temp.NhaanVienID = item.NhanvienID;
                temp.HoVaTen = item.HoVaTen;
                temp.HoTenVaMaNV = item.HoTenVaMaNV;
                temp.MaNV = item.MaNV;
                temp.ChucDanh = item.ChucDanh;
                var x = nvtktp.Where(xx => xx.MaNVThuKy == item.MaNV).FirstOrDefault();
                if (x != null)
                    temp.isckeck = true;
                else temp.isckeck = false;
                thuky.Add(temp);
            }
            ViewBag.sltk = nvtk.Count();
            model.ListNhanVienThuKy = thuky;
            return PartialView("~/Views/QuanLyNhanVien/_CapNhatThuKyChoThamPhan.cshtml", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThuKyTheoThamPhan(NhanVienViewModel danhsachthuky)
        {
            try
            {
                ResponseResult result = _quanLyNhanVienService.CapNhatThuKyThamPhan(danhsachthuky);
                if (result != null && result.ResponseCode == 1)
                {
                    return Json(JsonResponseViewModel.CreateSuccess(NotifyMessage.THONGBAO_CAPNHAT_THUKY));
                }
                return Json((JsonResponseViewModel.CreateFail(NotifyMessage.THONGBAO_KHONGCAPNHAT_THUKY)));
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }
        public ActionResult TruyCapTheoToaAn(int id)
        {
            var nv = _quanLyNhanVienService.ChitietNhanVien(id);
            var model = new NhanVienToaAnViewModel();
            model.NhanvienID = nv.NhanvienID;
            model.ToaAnID = nv.ToaAnID;
            var nvta = _quanLyNhanVienService.DanhSachNhanVienToaAn(id);
            var toaAn = new List<ToaAnNhanVien>();
            foreach(var ta in SettingDataService.DanhSachToaAn())
            {
                ToaAnNhanVien temp = new ToaAnNhanVien();
                temp.ToaAnID = ta.ToaAnID;
                temp.TenToaAn = ta.TenToaAn;
                var x = nvta.Where(xx => xx.ToaAnID == ta.ToaAnID).FirstOrDefault();
                if (x != null)
                    temp.isckeck = true;
                else temp.isckeck = false;
                toaAn.Add(temp);
            }
            model.ListToaAn = toaAn;
            return PartialView("~/Views/QuanLyNhanVien/_NhanVienToaAn.cshtml",model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TruyCapTheoToaAn(NhanVienToaAnViewModel model)
        {
            ResponseResult result = new ResponseResult();
            try
            {
                var nvta = _quanLyNhanVienService.DanhSachNhanVienToaAn(model.NhanvienID);
                var nv = _quanLyNhanVienService.ChitietNhanVien(model.NhanvienID);
                if (nv.ToaAnID != model.ToaAnID)
                {
                    result = _quanLyNhanVienService.SuaToaAnTrucThuoc(model.NhanvienID,model.ToaAnID);
                    if (result == null && result.ResponseCode != 1)
                    {
                        return Json(JsonResponseViewModel.CreateFail("Không sửa được " + ViewText.TABLE_TOAANTRUCTHUOC));
                    }
                    result = new ResponseResult();
                }
                for (int i = 0; i < model.ListToaAn.Count(); i++)
                {
                    var x = nvta.Where(xx => xx.ToaAnID == model.ListToaAn[i].ToaAnID).FirstOrDefault();
                    if (model.ListToaAn[i].isckeck)
                    {
                        if (x == null)
                            result = _quanLyNhanVienService.ThemNhanVienToaAn(model.NhanvienID, model.ListToaAn[i].ToaAnID);
                        else
                        {
                            result.ResponseCode = 1;
                            result.ResponseMessage = "OK";
                        }
                        if (result == null && result.ResponseCode != 1)
                        {
                            return Json(JsonResponseViewModel.CreateFail("Không sửa được " + ViewText.TABLE_TOAANTRUCTHUOC));
                        }
                        result = new ResponseResult();
                    }
                    else if (model.ListToaAn[i].isckeck != true)
                    {
                        if (x != null)
                        {
                            result = _quanLyNhanVienService.XoaNhanVienToaAn(x.NhanVienToaAnID, x.NhanVienID, x.ToaAnID);
                        }
                        else
                        {
                            result.ResponseCode = 1;
                            result.ResponseMessage = "OK";
                        }
                        if (result == null && result.ResponseCode != 1)
                        {
                            return Json(JsonResponseViewModel.CreateFail("Không sửa được " + ViewText.TITLE_TRUYCAPTHEOTOAAN));
                        }
                    }
                }
                return Json(JsonResponseViewModel.CreateSuccess());
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }

        #region Searching Nhan Vien by Keyword
        public ActionResult DanhSachNhanVienTheoKeyword(string keyword, int? toaAnId, string chucDanh)
        {
            var viewmodel = _quanLyNhanVienService.DanhSachNhanVienSearchByKeyword(keyword, toaAnId);
            return PartialView("~/Views/QuanLyNhanVien/_NhanvienItem.cshtml", viewmodel);
        }
        #endregion

        #region So Do To Chuc Quan Ly Nhan Vien
        public ActionResult SoDoToChucQuanLyNhanVien(int? toaAnId)
        {
            var viewmodel = _quanLyNhanVienService.SoDoToChucQuanLyNhanVien(toaAnId);
            ViewBag.loai = 1;
            return PartialView("_SoDoToChucTreeViewQLNV", viewmodel);
        }
        public ActionResult SoDoToChucQuanLyNhanVienChucVu(int? toaAnId)
        {
            var viewmodel = _quanLyNhanVienService.SoDoToChucQuanLyNhanVienChucVu(toaAnId);
            ViewBag.loai = 2;
            return PartialView("_SoDoToChucTreeViewQLNV", viewmodel);
        }
        public ActionResult DanhSachNhanVienTheoChucDanh(int? toaAnId, string chucDanh, int? loai)
        {
            //loai = 2 là chức vụ
            if(loai == 2)
            {
                var dsNhanVien = _quanLyNhanVienService.DanhSachNhanVienTheoChucVu(toaAnId, chucDanh);
                return PartialView("_NhanvienItem", dsNhanVien);
            }
            var danhSachNhanVien = _quanLyNhanVienService.DanhSachNhanVienTheoChucDanh(toaAnId, chucDanh);
            return PartialView("_NhanvienItem", danhSachNhanVien);
        }
        #endregion

        #region Cap Nhat Chuc Nang Nhan Vien
        public ActionResult CapNhatChucNang(string maNV)
        {
            var viewModel = TaiKhoanNhanVienService.DanhSachChucNangTheoMaNhanVien(maNV);
            return PartialView("~/Views/QuanLyNhanVien/_CapNhatChucNang.cshtml", viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CapNhatChucNang(ChucNangViewModel danhSachChucNang)
        {
            try
            {
                if(danhSachChucNang.ChucNangChinh==null || danhSachChucNang.ChucNangChinh=="")
                {
                    ModelState.AddModelError("ChucNangChinh", string.Format(ValidationMessages.VALIDATION_KIEMTRARONG, ViewText.LABEL_CHUCNANGCHINH));
                }
                if (ModelState.IsValid)
                {
                    ResponseResult result = _quanLyNhanVienService.CapNhatChucNangNhanVien(danhSachChucNang);
                    if (result != null && result.ResponseCode == 1)
                    {
                        return Json(JsonResponseViewModel.CreateSuccess());
                    }
                    return Json((JsonResponseViewModel.CreateFail(NotifyMessage.THONGBAO_KHONGCAPNHAT_CHUCNANG)));
                }
                return PartialView("~/Views/QuanLyNhanVien/_CapNhatChucNang.cshtml", danhSachChucNang);
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }
        #endregion
    }
}