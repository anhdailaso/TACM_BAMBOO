using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Biz.Lib.Authentication;
using Biz.Lib.Helpers;
using Biz.Lib.SettingData.Model;
using Biz.Lib.TACM.NhanDon.Model;
using Biz.TACM.Enums;
using Biz.TACM.IServices;
using Biz.TACM.Services;
using Biz.TACM.Infrastructure.Attributes;
using Biz.TACM.Models.ViewModel.NhanDon;
using Biz.TACM.Models.ViewModel.Shared;
using PagedList;
using Biz.Lib.TACM.Resources.Resources;
using HoSoVuAnModel = Biz.Lib.TACM.NhanDon.Model.HoSoVuAnModel;

namespace Biz.TACM.Controllers
{
    [WorkFlow(WorkFlow.NhanDon)]
    public class NhanDonController : BizController
    {
        private ISettingDataService _settingDataService;
        private ISettingDataService SettingDataService => _settingDataService ?? (_settingDataService = new SettingDataService());
        private INhanDonService _nhandonService;
        private INhanDonService NhanDonService
        { get { return _nhandonService ?? (_nhandonService = new NhanDonService()); } }

        protected readonly IHoSoService HosoService;
        protected readonly ITrangThaiService TrangThaiService;
        protected readonly IThuLyService _thulyService;

        public NhanDonController(IHoSoService hoSoService, ITrangThaiService trangThaiService, IThuLyService thuLyService)
        {
            this.HosoService = hoSoService;
            this.TrangThaiService = trangThaiService;
            this._thulyService = thuLyService;
        }
        
        public ActionResult Index(HoSoVuAnModel model, string sortOrder, int? page, string keyword, string nhomAn = null, string maNv = null)
        {
            var anSession = GetAnSessionInfo();
            if (nhomAn == null)
                nhomAn = anSession.MaNhomAn;

            UpdateAnSessionInfo(idToaAn: anSession.ToaAnId, maToaAn: anSession.MaToaAn, maNhomAn: nhomAn, idGiaiDoan: anSession.GiaiDoanId, idCongDoan: CongDoan.NhanDon.GetHashCode(), idVuAn: model.HoSoVuAnID);
            if (anSession.MaNhomAn.Equals(Setting.MANHOMAN_APDUNG_BPXLHC))
            {
                if (anSession.ToaAnId == ToaAn.TinhCaMau.GetHashCode())
                {
                    UpdateAnSessionInfo(idToaAn: anSession.ToaAnId, maToaAn: anSession.MaToaAn, maNhomAn: nhomAn, idGiaiDoan: GiaiDoan.PhucTham.GetHashCode(), idCongDoan: CongDoan.NhanDon.GetHashCode(), idVuAn: model.HoSoVuAnID);
                }
            }

            anSession = GetAnSessionInfo();

            model.ToaAnID = anSession.ToaAnId;
            model.NhomAn = anSession.MaNhomAn;
            model.GiaiDoan = anSession.GiaiDoanId;

            ViewBag.GiaiDoan = anSession.GiaiDoanId;
            ViewBag.SoLuongHoSoChoPhucTham = NhanDonService.GetSoLuongHoSoChoPhucTham(anSession.MaNhomAn);

            IEnumerable<HoSoVuAnModel> viewModel;
            if (maNv == null)
            {
                viewModel = NhanDonService.DanhSachHoSoVuAn(model, null); //get all hsva
                ViewBag.IsKeywordEmpty = false;
                ViewBag.TongCongDoan = NhanDonService.CongDoanHoSo(model, null);
            }
            else if (keyword == null)
            {
                viewModel = NhanDonService.DanhSachHoSoVuAn(model, maNv); //get danh sach hsva theo manv
                ViewBag.IsKeywordEmpty = false;
                ViewBag.TongCongDoan = NhanDonService.CongDoanHoSo(model, maNv);
            }
            else
            {
                if (keyword == "")
                    ViewBag.IsKeywordEmpty = true;
                else
                    ViewBag.IsKeywordEmpty = false;
                viewModel = NhanDonService.DanhSachHoSoVuAnSearchByKeyword(keyword);
            }
            ViewBag.ddlTrangThai = NhanDonService.SelectListCongDoanHoSo(Int32.Parse(Request.QueryString["CongDoanHoSo"] ?? "1"), anSession.GiaiDoanId);
            ViewBag.ddlTrangThaiAll = NhanDonService.SelectListCongDoanHoSo(7, anSession.GiaiDoanId);
            ViewBag.ddlHinhThucGoiDon = SettingDataService.SettingDataItem("DINHNGHIACHUNG_HINHTHUCGOIDON", null);
            ViewBag.ddlLoaiDon = SettingDataService.SettingDataItem("DINHNGHIACHUNG_LOAIDON", null);
            ViewBag.ddlLoaiQuanHe = SettingDataService.SettingDataItem("DS_SOTHAM_LOAIQUANHE", null);
            ViewBag.ddlYeuToNuocNgoai = SettingDataService.SettingDataItem("DINHNGHIACHUNG_YEUTONUOCNGOAI", null);
            ViewBag.ddlNguoiKyXacNhanDaNhanDon = SettingDataService.DanhSachNhanVienTheoTenChucNangNghiepVu(Setting.CHUCNANG_KYXACNHAN_NHANDON, anSession.ToaAnId, null);
            ViewBag.ddlDuongSu = NhanDonService.SelectListDuongSu(anSession.ToaAnId,0);

            ViewBag.CurrentSort = sortOrder;
            ViewBag.SoHoSoSortParm = sortOrder == "SoHoSo" ? "SoHoSo_desc" : "SoHoSo";
            ViewBag.MaHoSoSortParm = sortOrder == "MaHoSo" ? "MaHoSo_desc" : "MaHoSo";
            ViewBag.NgayTaoSortParm = sortOrder == "NgayTao" ? "NgayTao_desc" : "NgayTao";
            ViewBag.TrangThaiSortParm = sortOrder == "TrangThai" ? "TrangThai_desc" : "TrangThai";
            ViewBag.ThamPhanSortParm = sortOrder == "ThamPhan" ? "ThamPhan_desc" : "ThamPhan";
            ViewBag.CongDoanHoSoSortParm = sortOrder == "CongDoanHoSo" ? "CongDoanHoSo_desc" : "CongDoanHoSo";

            switch (sortOrder)
            {
                case "SoHoSo":
                    viewModel = viewModel.OrderBy(i => i.SoHoSo);
                    break;
                case "SoHoSo_desc":
                    viewModel = viewModel.OrderByDescending(i => i.SoHoSo);
                    break;
                case "MaHoSo":
                    viewModel = viewModel.OrderBy(i => i.MaHoSo);
                    break;
                case "MaHoSo_desc":
                    viewModel = viewModel.OrderByDescending(i => i.MaHoSo);
                    break;
                case "NgayTao":
                    viewModel = viewModel.OrderBy(i => i.NgayTao);
                    break;
                case "NgayTao_desc":
                    viewModel = viewModel.OrderByDescending(i => i.NgayTao);
                    break;
                case "TrangThai":
                    viewModel = viewModel.OrderBy(i => i.TrangThai);
                    break;
                case "TrangThai_desc":
                    viewModel = viewModel.OrderByDescending(i => i.TrangThai);
                    break;
                case "ThamPhan":
                    viewModel = viewModel.OrderBy(i => i.NhanVienThamPhan.HoTenVaMaNV);
                    break;
                case "ThamPhan_desc":
                    viewModel = viewModel.OrderByDescending(i => i.NhanVienThamPhan.HoTenVaMaNV);
                    break;
                case "CongDoanHoSo":
                    viewModel = viewModel.OrderBy(i => i.CongDoanHoSo);
                    break;
                case "CongDoanHoSo_desc":
                    viewModel = viewModel.OrderByDescending(i => i.CongDoanHoSo);
                    break;
            }


            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(viewModel.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult CapNhatGiaiDoanHoSo()
        {
            var giaiDoan = GetAnSessionInfo().GiaiDoanId;
            if (giaiDoan == 0 || giaiDoan == GiaiDoan.SoTham.GetHashCode())
            {
                giaiDoan = GiaiDoan.PhucTham.GetHashCode();
            }
            else
            {
                giaiDoan = GiaiDoan.SoTham.GetHashCode();
            }
            UpdateAnSessionInfo(idToaAn: GetAnSessionInfo().ToaAnId, maToaAn: GetAnSessionInfo().MaToaAn, maNhomAn: GetAnSessionInfo().MaNhomAn, idGiaiDoan: giaiDoan, idVuAn: GetAnSessionInfo().VuAnId);

            return Json(new { status = "Success" }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult ChiTietHoSo(int id)
        {
            var anSession = GetAnSessionInfo();
            if (anSession.MaNhomAn == Setting.MANHOMAN_APDUNG_BPXLHC)
            {
                return RedirectToAction("ChiTietHoSoApDungBPXLHC", new { id = id });
            }
            if (anSession.MaNhomAn == Setting.MANHOMAN_HINHSU)
            {
                return RedirectToAction("ChiTietHoSoHinhSu", new { id = id });
            }
            if (anSession.GiaiDoanId == GiaiDoan.PhucTham.GetHashCode())
            {
                return RedirectToAction("Index", "NhanHoSo", new { id = id });
            }
            var chiTietHoSo = NhanDonService.ChiTietHoSoVuAnTheoId(id);
            if (anSession.GiaiDoanId == 0)
                anSession.GiaiDoanId = chiTietHoSo.GiaiDoan;
            UpdateAnSessionInfo(idToaAn: chiTietHoSo.ToaAnID, maToaAn: anSession.MaToaAn, maNhomAn: chiTietHoSo.NhomAn, idGiaiDoan: anSession.GiaiDoanId, idCongDoan: CongDoan.NhanDon.GetHashCode(), idVuAn: id);
            
            var viewModel = NhanDonService.ChiTietHoSoVuAnTheoGiaiDoan(id, GiaiDoan.SoTham.GetHashCode());

            if (viewModel != null)
            {
                ViewBag.listNgayTao = NhanDonService.DanhSachNgayHoSoVuAn(viewModel.HoSoVuAnID, anSession.GiaiDoanId, 0);
                ViewBag.ddlTrangThai = NhanDonService.SelectListCongDoanHoSo(viewModel.CongDoanHoSo, anSession.GiaiDoanId);
            }
            ViewBag.ActiveCongDoan = chiTietHoSo.CongDoanHoSo;

            var giaiDoanHoSo = chiTietHoSo.GiaiDoan;
            ViewBag.RoleGiaiDoan = giaiDoanHoSo == anSession.GiaiDoanId ? 1 : -1;

            return View(viewModel);
        }

        public ActionResult ChiTietHoSoTheoLog(int id)
        {
            var anSession = GetAnSessionInfo();
            if (anSession.MaNhomAn == Setting.MANHOMAN_APDUNG_BPXLHC)
            {
                return RedirectToAction("ChiTietHoSoApDungBPXLHCTheoLog", new { id = id });
            }
            if (anSession.MaNhomAn == Setting.MANHOMAN_HINHSU)
            {
                return RedirectToAction("ChiTietHoSoHinhSuTheoLog", new { id = id });
            }
            var viewModel = NhanDonService.ChiTietHoSoVuAnTheoLog(id);

            if (viewModel != null)
            {
                ViewBag.listNgayTao = NhanDonService.DanhSachNgayHoSoVuAn(viewModel.HoSoVuAnID, anSession.GiaiDoanId, 0);
            }
            //ViewBag.ddlTrangThai = NhanDonService.SelectListCongDoanHoSo(viewModel.CongDoanHoSo, giaiDoan);

            return PartialView("ChiTietHoSoLog", viewModel);
        }

        //public ActionResult NhanHoSoMoi()
        //{
        //    return PartialView("_NhanHoSoMoi");
        //}

        #region HoSoVuAn
        [CustomAuthorize]
        public ActionResult ThemHoSoVuAn(int? id)
        {
            var anSession = GetAnSessionInfo();
            if (anSession.MaNhomAn == Setting.MANHOMAN_APDUNG_BPXLHC)
            {
                return RedirectToAction("ThemHoSoVuAnApDungBPXLHC", new { id = id });
            }
            if (anSession.MaNhomAn == Setting.MANHOMAN_HINHSU)
            {
                return RedirectToAction("ThemHoSoVuAnHinhSu", new { id = id });
            }

            int hoSoVuAnID = id ?? 0;

            var viewModel = NhanDonService.ChiTietHoSoVuAn(hoSoVuAnID);

            if (viewModel == null)
            {
                viewModel = new HoSoVuAnModel();
                viewModel.HoSoVuAnID = 0;
                viewModel.NgayLamDon = DateTime.Now;
                viewModel.NgayNopDonTaiToaAn = DateTime.Now;
            }
            else
            {
                viewModel.HinhThucGoiDonKhac = viewModel.HinhThucGoiDon;
                /*if (NhanDonService.SelectListHinhThucGuiDon(null).Where(m => m.Value==viewModel.HinhThucGoiDon).Count() == 0)
                {
                    viewModel.HinhThucGoiDon = "Khác";
                }*/

            }

            if (anSession.MaNhomAn == Setting.MANHOMAN_HANHCHINH)
            {
                ViewBag.ddlHinhThucGoiDon = SettingDataService.SettingDataItem("HC_SOTHAM_HINHTHUCGOIDON", null);
                viewModel.LoaiQuanHe = Setting.LOAIQUANHE_TRANHCHAP;

                var listLoaiQuanHe = SettingDataService.SettingDataItem("DS_SOTHAM_LOAIQUANHE", null).ToList().Where(x => x.Value == "Tranh chấp");
                ViewBag.ddlLoaiQuanHe = new SelectList(listLoaiQuanHe, "Value", "Text");
            }
            else
            {
                ViewBag.ddlHinhThucGoiDon = SettingDataService.SettingDataItem("DINHNGHIACHUNG_HINHTHUCGOIDON", null);
                ViewBag.ddlLoaiQuanHe = SettingDataService.SettingDataItem("DS_SOTHAM_LOAIQUANHE", null);
            }
            InitQuanHePhapLuatTheoNhomAn(anSession.MaNhomAn, viewModel.LoaiQuanHe);
            ViewBag.ddlLoaiDon = SettingDataService.SettingDataItem("DINHNGHIACHUNG_LOAIDON", null);
            viewModel.CanBoNhanDon = SettingDataService.ChiTietNhanVienTheoMaNhanVien(AccountUtils.CurrentUsername()).HoTenVaMaNV;
            ViewBag.ddlYeuToNuocNgoai = SettingDataService.SettingDataItem("DINHNGHIACHUNG_YEUTONUOCNGOAI", null);
            ViewBag.ddlNguoiKyXacNhanDaNhanDon = SettingDataService.DanhSachNhanVienTheoTenChucNangNghiepVu(Setting.CHUCNANG_KYXACNHAN_NHANDON, anSession.ToaAnId, null);
            ViewBag.IsToaAnTinh = anSession.ToaAnId == ToaAn.TinhCaMau.GetHashCode();

            return PartialView("~/Views/NhanDon/_NhanHoSoMoi.cshtml", viewModel);
        }
        
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult ThemHoSoVuAn(HoSoVuAnModel viewModel)
        {
            var anSession = GetAnSessionInfo();

            if (anSession.MaNhomAn == Setting.MANHOMAN_HANHCHINH)
            {
                if (String.IsNullOrEmpty(viewModel.QuanHePhapLuat))
                {
                    ModelState.Remove("QuanHePhapLuat");
                    ModelState.AddModelError("QuanHePhapLuat", string.Format(ValidationMessages.VALIDATION_KIEMTRARONG, ViewText.LABEL_KHIEUKIEN));
                }
                viewModel.LoaiQuanHe = Setting.LOAIQUANHE_TRANHCHAP;
            }
            else if (anSession.MaNhomAn == Setting.MANHOMAN_APDUNG_BPXLHC)
            {
                viewModel.LoaiQuanHe = Setting.LOAIQUANHE_YEUCAU;
            }

            InitModelStateHoSoVuAnTheoToaAn(anSession.ToaAnId);

            
            //if (viewModel.HinhThucGoiDon == "-1")
            //{
            //    if (String.IsNullOrEmpty(viewModel.HinhThucGoiDonKhac))
            //    {
            //        ModelState.AddModelError("HinhThucGoiDon",ValidationMessages.VALIDATION_GUIDONKHAC_KHONGDUOCTRONG);
            //    }
            //    else
            //    {
            //        viewModel.HinhThucGoiDon = viewModel.HinhThucGoiDonKhac;
            //    }
            //}

            if (viewModel.HoSoVuAnID > 0)
            {
                ModelState.Remove("NoiDungDon");
            }

            if (viewModel.HoSoVuAnID == 0 && viewModel.NgayLamDon > DateTime.Now.Date)
            {
                ModelState.AddModelError("NgayLamDon", ValidationMessages.VALIDATION_GIOIHAN_NGAYLAMDON);
            }
            else if (viewModel.NgayLamDon > viewModel.NgayNopDonTaiToaAn)
            {
                ModelState.AddModelError("NgayLamDon", ValidationMessages.VALIDATION_GIOIHAN_NGAYLAMDON_NHOHON);
            }

            if (ModelState.IsValid)
            {
                viewModel.CanBoNhanDon = SettingDataService.GetNhanVienSessionInfo().MaNV;
                ResponseResult result = null;
                if (viewModel.HoSoVuAnID > 0)
                {
                    result = NhanDonService.SuaHoSoVuAn(viewModel);
                }
                else
                {
                    viewModel.ToaAnID = anSession.ToaAnId;
                    viewModel.NhomAn = anSession.MaNhomAn;
                    result = NhanDonService.ThemHoSoVuAn(viewModel);
                }


                if (result != null && result.ResponseCode == 1)
                {
                    //reload danh sach hosovuan is in role
                    Sessions.AddObject<List<HoSoVuAnModel>>("AnRoleObject", null);

                    Success(result.ResponseMessage);
                    return Json(new { status = "success" });
                }
            }

            InitQuanHePhapLuatTheoNhomAn(anSession.MaNhomAn, viewModel.LoaiQuanHe);

            if (anSession.MaNhomAn == Setting.MANHOMAN_HANHCHINH)
            {
                ViewBag.ddlHinhThucGoiDon = SettingDataService.SettingDataItem("HC_SOTHAM_HINHTHUCGOIDON", null);
                viewModel.LoaiQuanHe = Setting.LOAIQUANHE_TRANHCHAP;
                var listLoaiQuanHe = SettingDataService.SettingDataItem("DS_SOTHAM_LOAIQUANHE", null).ToList().Where(x => x.Value == "Tranh chấp");
                ViewBag.ddlLoaiQuanHe = new SelectList(listLoaiQuanHe, "Value", "Text");
            }
            else
            {
                ViewBag.ddlHinhThucGoiDon = SettingDataService.SettingDataItem("DINHNGHIACHUNG_HINHTHUCGOIDON", null);
                ViewBag.ddlLoaiQuanHe = SettingDataService.SettingDataItem("DS_SOTHAM_LOAIQUANHE", null);
            }

            ViewBag.ddlLoaiDon = SettingDataService.SettingDataItem("DINHNGHIACHUNG_LOAIDON", null);
            viewModel.CanBoNhanDon = SettingDataService.ChiTietNhanVienTheoMaNhanVien(AccountUtils.CurrentUsername()).HoTenVaMaNV;
            ViewBag.ddlYeuToNuocNgoai = SettingDataService.SettingDataItem("DINHNGHIACHUNG_YEUTONUOCNGOAI", null);
            ViewBag.ddlNguoiKyXacNhanDaNhanDon = SettingDataService.DanhSachNhanVienTheoTenChucNangNghiepVu(Setting.CHUCNANG_KYXACNHAN_NHANDON, anSession.ToaAnId, null);
            ViewBag.IsToaAnTinh = anSession.ToaAnId == ToaAn.TinhCaMau.GetHashCode();

            return PartialView("~/Views/NhanDon/_NhanHoSoMoi.cshtml", viewModel);
        }

        private void InitModelStateHoSoVuAnTheoToaAn(int toaAnId)
        {
            if (toaAnId != ToaAn.TinhCaMau.GetHashCode())
            {
                ModelState.Remove("YeuToNuocNgoai");
            }
        }

        public void InitQuanHePhapLuatTheoNhomAn(string nhomAn, string loaiQuanHe)
        {

            if (nhomAn == Setting.MANHOMAN_DANSU)
            {
                if (loaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                {
                    ViewBag.listQuanHePhapLuat = SettingDataService.SettingDataItem("DS_SOTHAM_QHPL_YEUCAU", null);
                }
                else if (loaiQuanHe == Setting.LOAIQUANHE_TRANHCHAP)
                {
                    ViewBag.listQuanHePhapLuat = SettingDataService.SettingDataItem("DS_SOTHAM_QHPL_TRANHCHAP", null);
                }
                else
                {
                    ViewBag.listQuanHePhapLuat = new SelectList(Enumerable.Empty<SelectListItem>());
                }
                ViewBag.listQuanHePhapLuatTranhChap = SettingDataService.SettingDataItem("DS_SOTHAM_QHPL_TRANHCHAP", null);
                ViewBag.listQuanHePhapLuatYeuCau = SettingDataService.SettingDataItem("DS_SOTHAM_QHPL_YEUCAU", null);
            }
            else if (nhomAn == Setting.MANHOMAN_HONNHAN)
            {
                if (loaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                {
                    ViewBag.listQuanHePhapLuat = SettingDataService.SettingDataItem("HN_SOTHAM_QHPL_YEUCAU", null);
                }
                else if (loaiQuanHe == Setting.LOAIQUANHE_TRANHCHAP)
                {
                    ViewBag.listQuanHePhapLuat = SettingDataService.SettingDataItem("HN_SOTHAM_QHPL_TRANHCHAP", null);
                }
                else
                {
                    ViewBag.listQuanHePhapLuat = new SelectList(Enumerable.Empty<SelectListItem>());
                }
                ViewBag.listQuanHePhapLuatTranhChap = SettingDataService.SettingDataItem("HN_SOTHAM_QHPL_TRANHCHAP", null);
                ViewBag.listQuanHePhapLuatYeuCau = SettingDataService.SettingDataItem("HN_SOTHAM_QHPL_YEUCAU", null);
            }
            else if (nhomAn == Setting.MANHOMAN_KINHTE)
            {
                if (loaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                {
                    ViewBag.listQuanHePhapLuat = SettingDataService.SettingDataItem("KT_SOTHAM_QHPL_YEUCAU", null);
                }
                else if (loaiQuanHe == Setting.LOAIQUANHE_TRANHCHAP)
                {
                    ViewBag.listQuanHePhapLuat = SettingDataService.SettingDataItem("KT_SOTHAM_QHPL_TRANHCHAP", null);
                }
                else
                {
                    ViewBag.listQuanHePhapLuat = new SelectList(Enumerable.Empty<SelectListItem>());
                }
                ViewBag.listQuanHePhapLuatTranhChap = SettingDataService.SettingDataItem("KT_SOTHAM_QHPL_TRANHCHAP", null);
                ViewBag.listQuanHePhapLuatYeuCau = SettingDataService.SettingDataItem("KT_SOTHAM_QHPL_YEUCAU", null);
            }
            else if (nhomAn == Setting.MANHOMAN_LAODONG)
            {
                if (loaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                {
                    ViewBag.listQuanHePhapLuat = SettingDataService.SettingDataItem("LD_SOTHAM_QHPL_YEUCAU", null);
                }
                else if (loaiQuanHe == Setting.LOAIQUANHE_TRANHCHAP)
                {
                    ViewBag.listQuanHePhapLuat = SettingDataService.SettingDataItem("LD_SOTHAM_QHPL_TRANHCHAP", null);
                }
                else
                {
                    ViewBag.listQuanHePhapLuat = new SelectList(Enumerable.Empty<SelectListItem>());
                }
                ViewBag.listQuanHePhapLuatTranhChap = SettingDataService.SettingDataItem("LD_SOTHAM_QHPL_TRANHCHAP", null);
                ViewBag.listQuanHePhapLuatYeuCau = SettingDataService.SettingDataItem("LD_SOTHAM_QHPL_YEUCAU", null);
            }
            else if (nhomAn == Setting.MANHOMAN_HANHCHINH)
            {
                ViewBag.listQuanHePhapLuat = SettingDataService.SettingDataItem("HC_SOTHAM_KHIEUKIEN", null);
            }
            else
            {
                ViewBag.listQuanHePhapLuatTranhChap = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.listQuanHePhapLuatYeuCau = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.listQuanHePhapLuat = new SelectList(Enumerable.Empty<SelectListItem>());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChuyenCongDoan(int HoSoVuAnID, int CongDoanHoSo)
        {
            var anSession = GetAnSessionInfo();
            UpdateAnSessionInfo(idToaAn: anSession.ToaAnId, maToaAn: anSession.MaToaAn, maNhomAn: anSession.MaNhomAn, idGiaiDoan: anSession.GiaiDoanId, idCongDoan: CongDoanHoSo, idVuAn: HoSoVuAnID);
            var viewModel = NhanDonService.ChiTietHoSoVuAn(HoSoVuAnID);
            viewModel.CongDoanHoSo = CongDoanHoSo;

            NhanDonService.ChuyenCongDoanHoSo(viewModel);

            return Json(new { status = "success" });
        }

        [HttpPost]
        public ActionResult ChuyenCongDoanNext(int HoSoVuAnID)
        {
            var anSession = GetAnSessionInfo();
            var viewModel = NhanDonService.ChiTietHoSoVuAn(HoSoVuAnID);
            int congDoanNext = viewModel.CongDoanHoSo;
            string urlCongDoanNext = "";

            //update cong doan
            if (viewModel.CongDoanHoSo == CongDoan.SauXetXu.GetHashCode())
            {
                congDoanNext = CongDoan.LuuKho.GetHashCode();
            }
            else if (viewModel.CongDoanHoSo != CongDoan.LuuKho.GetHashCode())
            {
                congDoanNext = viewModel.CongDoanHoSo + 1;
            }
            if (congDoanNext < 6)
            {
                viewModel.CongDoanHoSo = congDoanNext;
                NhanDonService.ChuyenCongDoanHoSo(viewModel);
                UpdateAnSessionInfo(idToaAn: anSession.ToaAnId, maToaAn: anSession.MaToaAn, maNhomAn: anSession.MaNhomAn, idGiaiDoan: anSession.GiaiDoanId, idCongDoan: congDoanNext, idVuAn: HoSoVuAnID);
            }

            //chuyen trang
            if (congDoanNext == CongDoan.ThuLy.GetHashCode())
            {
                urlCongDoanNext = "/ThuLy/Index/" + HoSoVuAnID;
            }
            else if (congDoanNext == CongDoan.ChuanBiXetXu.GetHashCode())
            {
                urlCongDoanNext = "/ChuanBiXetXu/Index/" + HoSoVuAnID;
            }
            else if (congDoanNext == CongDoan.KetQuaXetXu.GetHashCode())
            {
                urlCongDoanNext = "/KetQuaXetXu/Index/" + HoSoVuAnID;
            }
            else if (congDoanNext == CongDoan.SauXetXu.GetHashCode())
            {
                urlCongDoanNext = "/SauXetXu/Index/" + HoSoVuAnID;
            }
            else if (congDoanNext == CongDoan.LuuKho.GetHashCode())
            {
                urlCongDoanNext = "/SauXetXu/Index/" + HoSoVuAnID;
            }

            return Json(new { status = "success", urlCongDoan = urlCongDoanNext });
        }

        [HttpPost]
        public ActionResult ChuyenCongDoanPrev(int HoSoVuAnID)
        {
            var anSession = GetAnSessionInfo();
            var viewModel = NhanDonService.ChiTietHoSoVuAn(HoSoVuAnID);
            int congDoanPrev;
            string urlCongDoanPrev = "";

            //update cong doan
            
            if (viewModel.CongDoanHoSo == CongDoan.LuuKho.GetHashCode())
            {
                congDoanPrev = CongDoan.SauXetXu.GetHashCode();
            }
            else
            {
                congDoanPrev = viewModel.CongDoanHoSo - 1;
            }
            if (congDoanPrev > 0)
            {
                viewModel.CongDoanHoSo = congDoanPrev;
                NhanDonService.ChuyenCongDoanHoSo(viewModel);
                UpdateAnSessionInfo(idToaAn: anSession.ToaAnId, maToaAn: anSession.MaToaAn, maNhomAn: anSession.MaNhomAn, idGiaiDoan: anSession.GiaiDoanId, idCongDoan: congDoanPrev, idVuAn: HoSoVuAnID);
            }

            //chuyen trang
            if (congDoanPrev == CongDoan.NhanDon.GetHashCode())
            {
                urlCongDoanPrev = "/NhanDon/ChiTietHoSo/" + HoSoVuAnID;
            }
            else if (congDoanPrev == CongDoan.ThuLy.GetHashCode())
            {
                urlCongDoanPrev = "/ThuLy/Index/" + HoSoVuAnID;
            }
            else if (congDoanPrev == CongDoan.ChuanBiXetXu.GetHashCode())
            {
                urlCongDoanPrev = "/ChuanBiXetXu/Index/" + HoSoVuAnID;
            }
            else if (congDoanPrev == CongDoan.KetQuaXetXu.GetHashCode())
            {
                urlCongDoanPrev = "/KetQuaXetXu/Index/" + HoSoVuAnID;
            }
            else if (congDoanPrev == CongDoan.SauXetXu.GetHashCode())
            {
                urlCongDoanPrev = "/SauXetXu/Index/" + HoSoVuAnID;
            }

            return Json(new { status = "success", urlCongDoan = urlCongDoanPrev });
        }

        #endregion

        #region DuongSu
        public ActionResult DanhSachDuongSuCaNhan(int hoSoVuAnId)
        {
            var viewModel = NhanDonService.GetDanhSachDuongSuCaNhan(hoSoVuAnId);
            ViewBag.hoSoVuAnId = hoSoVuAnId;

            return Json(new { data = viewModel }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DanhSachDuongSuToChuc(int hoSoVuAnId)
        {
            var viewModel = NhanDonService.GetDanhSachDuongSuToChuc(hoSoVuAnId);
            ViewBag.hoSoVuAnId = hoSoVuAnId;
            return Json(new { data = viewModel }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChiTietDuongSu(int duongSuId)
        {
            var anSession = GetAnSessionInfo();
            var viewModel = NhanDonService.ChiTietDuongSuTheoId(duongSuId);

            if (anSession.MaNhomAn.Equals(Setting.MANHOMAN_HINHSU))
            {
                ViewBag.listDacDiemNhanThanBiCao = NhanDonService.DanhSachDacDiemSelected(SettingDataService.SettingDataItem("HS_DACDIEM_NHANTHAN_BICAO", ""), viewModel.ListDacDiemNhanThanBiCao);
                ViewBag.listDacDiemNhanThanBiHai = SettingDataService.SettingDataItem("HS_DACDIEM_NHANTHAN_BIHAI", "");

                return PartialView("DuongSu/HS/_ThongTinDuongSuNhomAnHS", viewModel);
            }
            else
            {
                return PartialView("DuongSu/_ThongTinDuongSu", viewModel);
            }
        }

        public ActionResult ChiTietDuongSuLaToChuc(int duongSuId)
        {
            var viewModel = NhanDonService.ChiTietDuongSuTheoId(duongSuId);
            return PartialView("DuongSu/_ThongTinDuongSuLaToChuc", viewModel);
        }

        [CustomAuthorize]
        public ActionResult TaoDuongSu(int hoSoVuAnId)
        {
            var anSession = GetAnSessionInfo();
            var hoSoVuAnModel = NhanDonService.ChiTietHoSoVuAnTheoId(hoSoVuAnId);
            var maNhomAn = hoSoVuAnModel.NhomAn;
            var viewModel = new DuongSuViewModel();
            viewModel.ListDacDiemNhanThanBiCao = new List<string>();
            //viewModel.ListDacDiemNhanThanBiHai = new List<string>();
            var loaiQuanHe = hoSoVuAnModel.LoaiQuanHe;

            if (maNhomAn.Equals(Setting.MANHOMAN_HANHCHINH))
            {
                ViewBag.listTuCachThamGiaToTung = SettingDataService.SettingDataItem("HC_TUCACHTHAMGIATOTUNG", "");
            }
            else if (maNhomAn.Equals(Setting.MANHOMAN_APDUNG_BPXLHC))
            {
                ViewBag.listTuCachThamGiaToTung = SettingDataService.SettingDataItem("AD_TUCACHTHAMGIATOTUNG", "");
                ViewBag.listGioiTinh = SettingDataService.SettingDataItem("DINHNGHIACHUNG_GIOITINH", "");
                ViewBag.listQuocTich = SettingDataService.SettingDataItem("DINHNGHIACHUNG_QUOCTICH", "Việt Nam");
                ViewBag.listDuongSuLa = SettingDataService.SettingDataItem("DS_SOTHAM_DuongSuLa", "");
                viewModel.HoSoVuAnID = hoSoVuAnId;
                //viewModel.NgayCapCMND = DateTime.Now.ToString();
                return PartialView("DuongSu/AD/_TaoDuongSuNhomAnAD", viewModel);
            }
            else if (maNhomAn.Equals(Setting.MANHOMAN_HINHSU))
            {
                var listTuCachThamGiaToTungHS = SettingDataService.SettingDataItem("HS_TUCACHTHAMGIATOTUNG", "");

                if (anSession.CongDoanId <= 2)
                {
                    listTuCachThamGiaToTungHS = new SelectList(listTuCachThamGiaToTungHS
                                                                .Where(x => x.Value != Setting.HS_TUCACHTOTUNG_BICAO)
                                                                .ToList(),
                                                                "Text",
                                                                "Text");
                }
                else
                {
                    listTuCachThamGiaToTungHS = new SelectList(listTuCachThamGiaToTungHS
                                                                .Where(x => x.Value != Setting.HS_TUCACHTOTUNG_BICAN)
                                                                .ToList(),
                                                                "Text",
                                                                "Text");
                }
                ViewBag.listTuCachThamGiaToTung = listTuCachThamGiaToTungHS;
                ViewBag.listGioiTinh = SettingDataService.SettingDataItem("DINHNGHIACHUNG_GIOITINH", "");
                ViewBag.listQuocTich = SettingDataService.SettingDataItem("DINHNGHIACHUNG_QUOCTICH", "Việt Nam");
                ViewBag.listDuongSuLa = SettingDataService.SettingDataItem("DS_SOTHAM_DUONGSULA", "");
                ViewBag.listDacDiemNhanThanBiCao = SettingDataService.SettingDataItem("HS_DACDIEM_NHANTHAN_BICAO", "");
                ViewBag.listTinhTrangGiamGiu = SettingDataService.SettingDataItem("HS_TINHTRANGGIAMGIU", "");
                var toidanhtruyto = NhanDonService.GetDanhSachToiDanhTruyTo(hoSoVuAnId);
                foreach(var item in toidanhtruyto)
                {
                    item.DieuLuatApDung = item.Dieu + " " + item.DieuLuatApDung;
                }
                ViewBag.listToiDanhTruyTo = new SelectList(toidanhtruyto, "DieuLuatApDung", "ToiDanh");
                ViewBag.listDieuLuatTruyTo = new SelectList(toidanhtruyto.OrderBy(x => x.Dieu), "DieuLuatApDung", "DieuLuatApDung");
                ViewBag.listDacDiemNhanThanBiHai = SettingDataService.SettingDataItem("HS_DACDIEM_NHANTHAN_BIHAI", "");
                ViewBag.listNguoiBaoChuaLa = SettingDataService.SettingDataItem("HS_NGUOIBAOCHUALA", "");
                ViewBag.listTinhThanhPho = SettingDataService.SettingDataItemXML("city_province", "");
                ViewBag.listNguoiDaiDienCua = NhanDonService.GetDanhSachDuongSuChoNguoiDaiDien(hoSoVuAnId);
                ViewBag.listNguoiBaoVeCua = NhanDonService.GetDanhSachDuongSuChoNguoiBaoVe(hoSoVuAnId);

                viewModel.HoSoVuAnID = hoSoVuAnId;
                viewModel.NgayCapCMND = DateTime.Now.ToString();
                return PartialView("DuongSu/HS/_TaoDuongSuNhomAnHS", viewModel);
            }
            else
            {
                if (loaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                {
                    ViewBag.listTuCachThamGiaToTung = SettingDataService.SettingDataItem("DS_SOTHAM_TUCACHTHAMGIATOTUNG_YEUCAU", "");
                }
                else
                {
                    ViewBag.listTuCachThamGiaToTung = SettingDataService.SettingDataItem("DS_SOTHAM_TUCACHTHAMGIATOTUNG_TRANHCHAP", "");
                }
            }

            ViewBag.listGioiTinh = SettingDataService.SettingDataItem("DINHNGHIACHUNG_GIOITINH", "");
            ViewBag.listQuocTich = SettingDataService.SettingDataItem("DINHNGHIACHUNG_QUOCTICH", "Việt Nam");
            ViewBag.listDuongSuLa = SettingDataService.SettingDataItem("DS_SOTHAM_DuongSuLa", "");
            viewModel.HoSoVuAnID = hoSoVuAnId;
            return PartialView("DuongSu/_TaoDuongSu", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TaoDuongSu(DuongSuViewModel viewModel)
        {
            try
            {
                ResponseResult result = NhanDonService.TaoDuongSu(viewModel);
                if (result != null && result.ResponseCode == 1)
                {
                    return Json(JsonResponseViewModel.CreateSuccess());
                }
                if (result != null && result.ResponseCode == -1)
                {
                    return Json(JsonResponseViewModel.CreateFail(result.ResponseMessage));
                }
                return Json(JsonResponseViewModel.CreateFail("Tạo đương sự không thành công"));
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }

        [CustomAuthorize]
        public ActionResult SuaDuongSu(int duongSuId)
        {
            var anSession = GetAnSessionInfo();
            var viewModel = NhanDonService.ChiTietDuongSuTheoId(duongSuId);
            int hoSoVuAnId = viewModel.HoSoVuAnID;

            var hoSoVuAnModel = NhanDonService.ChiTietHoSoVuAnTheoId(hoSoVuAnId);
            var maNhomAn = hoSoVuAnModel.NhomAn;
            var loaiQuanHe = hoSoVuAnModel.LoaiQuanHe;

            if (maNhomAn == Setting.MANHOMAN_HANHCHINH)
            {
                ViewBag.listTuCachThamGiaToTung = SettingDataService.SettingDataItem("HC_TUCACHTHAMGIATOTUNG", "");
            }
            else if (maNhomAn.Equals(Setting.MANHOMAN_APDUNG_BPXLHC))
            {
                ViewBag.listTuCachThamGiaToTung = SettingDataService.SettingDataItem("AD_TUCACHTHAMGIATOTUNG", "");
                ViewBag.listGioiTinh = SettingDataService.SettingDataItem("DINHNGHIACHUNG_GIOITINH", "");
                ViewBag.listQuocTich = SettingDataService.SettingDataItem("DINHNGHIACHUNG_QUOCTICH", "Việt Nam");
                ViewBag.listDuongSuLa = SettingDataService.SettingDataItem("DS_SOTHAM_DuongSuLa", "");
                return PartialView("DuongSu/AD/_SuaDuongSuNhomAnAD", viewModel);
            }
            else if (maNhomAn.Equals(Setting.MANHOMAN_HINHSU))
            {
                var listTuCachThamGiaToTungHS = SettingDataService.SettingDataItem("HS_TUCACHTHAMGIATOTUNG", "");

                if (anSession.CongDoanId <= 2)
                {
                    listTuCachThamGiaToTungHS = new SelectList(listTuCachThamGiaToTungHS
                                                                .Where(x => x.Value != Setting.HS_TUCACHTOTUNG_BICAO)
                                                                .ToList(),
                                                                "Text",
                                                                "Text");
                }
                else
                {
                    listTuCachThamGiaToTungHS = new SelectList(listTuCachThamGiaToTungHS
                                                                .Where(x => x.Value != Setting.HS_TUCACHTOTUNG_BICAN)
                                                                .ToList(),
                                                                "Text",
                                                                "Text");
                }
                ViewBag.listTuCachThamGiaToTung = listTuCachThamGiaToTungHS;
                ViewBag.listGioiTinh = SettingDataService.SettingDataItem("DINHNGHIACHUNG_GIOITINH", "");
                ViewBag.listQuocTich = SettingDataService.SettingDataItem("DINHNGHIACHUNG_QUOCTICH", "Việt Nam");
                ViewBag.listDuongSuLa = SettingDataService.SettingDataItem("DS_SOTHAM_DUONGSULA", "");
                ViewBag.listDacDiemNhanThanBiCao = NhanDonService.DanhSachDacDiemSelected(SettingDataService.SettingDataItem("HS_DACDIEM_NHANTHAN_BICAO", ""), viewModel.ListDacDiemNhanThanBiCao);
                ViewBag.listTinhTrangGiamGiu = SettingDataService.SettingDataItem("HS_TINHTRANGGIAMGIU", "");
                var toidanhtruyto = NhanDonService.GetDanhSachToiDanhTruyTo(GetAnSessionInfo().VuAnId);
                foreach (var item in toidanhtruyto)
                {
                    item.DieuLuatApDung = item.Dieu + " " + item.DieuLuatApDung;
                }
                ViewBag.listToiDanhTruyTo = new SelectList(toidanhtruyto, "DieuLuatApDung", "ToiDanh");
                ViewBag.listDieuLuatTruyTo = new SelectList(toidanhtruyto.OrderBy(x => x.Dieu), "DieuLuatApDung", "DieuLuatApDung");
                ViewBag.listDacDiemNhanThanBiHai = SettingDataService.SettingDataItem("HS_DACDIEM_NHANTHAN_BIHAI", ""); //NhanDonService.DanhSachDacDiemSelected(SettingDataService.SettingDataItem("HS_DACDIEM_NHANTHAN_BIHAI", ""), viewModel.ListDacDiemNhanThanBiHai);
                ViewBag.listNguoiBaoChuaLa = SettingDataService.SettingDataItem("HS_NGUOIBAOCHUALA", "");
                ViewBag.listTinhThanhPho = SettingDataService.SettingDataItemXML("city_province", "");
                ViewBag.listNguoiDaiDienCua = NhanDonService.GetDanhSachDuongSuChoNguoiDaiDien(hoSoVuAnId);
                ViewBag.listNguoiBaoVeCua = NhanDonService.GetDanhSachDuongSuChoNguoiBaoVe(hoSoVuAnId);

                viewModel.NgayCapCMND = DateTime.Now.ToString();
                return PartialView("DuongSu/HS/_SuaDuongSuNhomAnHS", viewModel);
            }
            else
            {
                if (loaiQuanHe == Setting.LOAIQUANHE_YEUCAU)
                {
                    ViewBag.listTuCachThamGiaToTung = SettingDataService.SettingDataItem("DS_SOTHAM_TUCACHTHAMGIATOTUNG_YEUCAU", "");
                }
                else
                {
                    ViewBag.listTuCachThamGiaToTung = SettingDataService.SettingDataItem("DS_SOTHAM_TUCACHTHAMGIATOTUNG_TRANHCHAP", "");
                }
            }

            ViewBag.listGioiTinh = SettingDataService.SettingDataItem("DINHNGHIACHUNG_GIOITINH", "");
            ViewBag.listQuocTich = SettingDataService.SettingDataItem("DINHNGHIACHUNG_QUOCTICH", "Việt Nam");
            ViewBag.listDuongSuLa = SettingDataService.SettingDataItem("DS_SOTHAM_DuongSuLa", "");

            return PartialView("DuongSu/_SuaDuongSu", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaDuongSu(DuongSuViewModel viewModel)
        {
            try
            {
                ResponseResult result = NhanDonService.SuaDuongSu(viewModel);
                if (result != null && result.ResponseCode == 1)
                {
                    return Json(JsonResponseViewModel.CreateSuccess());
                }
                if (result != null && result.ResponseCode == -1)
                {
                    return Json(JsonResponseViewModel.CreateFail(result.ResponseMessage));
                }
                return Json(JsonResponseViewModel.CreateFail("Sửa thông tin đương sự không thành công"));
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }

        [CustomAuthorize]
        public ActionResult GetXoaDuongSu(int duongSuId)
        {
            return PartialView("DuongSu/_XoaDuongSu", duongSuId);
        }

        [HttpPost]
        public ActionResult XoaDuongSu(int duongSuId)
        {
            try
            {
                ResponseResult result = NhanDonService.XoaDuongSu(duongSuId);
                if (result != null && result.ResponseCode == 1)
                {
                    return Json(JsonResponseViewModel.CreateSuccess());
                }
                return Json(JsonResponseViewModel.CreateFail(NotifyMessage.THONGBAO_KHONGXOA_DUONGSU));
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }

        #endregion

        #region NoiDungDon

        public ActionResult NoiDungDon(int hoSoVuAnId)
        {
            var listNgayTao = NhanDonService.GetDanhSachNgaySuaDoiNoiDungDon(hoSoVuAnId, GetAnSessionInfo().GiaiDoanId, 0);
            if (listNgayTao.Any())
            {
                var noiDungDonId = Protect.ToInt32(listNgayTao.First().Value);
                var viewModel = NhanDonService.ChiTietNoiDungDonTheoId(noiDungDonId);
                ViewBag.listNgayTao = listNgayTao;

                return PartialView("NoiDungDon/Detail", viewModel);
            }
            ViewBag.hoSoVuAnId = hoSoVuAnId;
            return PartialView("NoiDungDon/Detail");
        }

        public ActionResult GetNoiDungDonTheoId(int noiDungDonId)
        {
            var viewModel = NhanDonService.ChiTietNoiDungDonTheoId(noiDungDonId);
            if (viewModel != null)
            {
                ViewBag.listNgayTao = NhanDonService.GetDanhSachNgaySuaDoiNoiDungDon(viewModel.HoSoVuAnID, GetAnSessionInfo().GiaiDoanId, noiDungDonId);
                return PartialView("NoiDungDon/Detail", viewModel);
            }
            return PartialView("NoiDungDon/Detail");
        }

        [CustomAuthorize]
        public ActionResult EditNoiDungDon(int hoSoVuAnId, int noiDungDonId)
        {
            var viewModel = new NoiDungDonViewModel();
            if (noiDungDonId != 0)
            {
                viewModel = NhanDonService.ChiTietNoiDungDonTheoId(noiDungDonId);
                return PartialView("NoiDungDon/Edit", viewModel);
            }
            viewModel.HoSoVuAnID = hoSoVuAnId;
            return PartialView("NoiDungDon/Edit", viewModel);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult EditNoiDungDon(NoiDungDonViewModel viewModel)
        {
            try
            {
                ResponseResult result = NhanDonService.SuaDoiNoiDungDon(viewModel);
                if (result != null && result.ResponseCode == 1)
                {
                    return Json(JsonResponseViewModel.CreateSuccess());
                }
                return Json(JsonResponseViewModel.CreateFail(NotifyMessage.THONGBAO_KHONGSUA_THONGTIN_THAMPHAN));
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }
        #endregion

        #region ChuyenDon
        public ActionResult ChuyenDon(int hoSoVuAnId)
        {
            var listNgayTao = NhanDonService.GetDanhSachNgaySuaDoiChuyenDon(hoSoVuAnId, GetAnSessionInfo().GiaiDoanId, GetAnSessionInfo().CongDoanId, 0);
            if (listNgayTao.Any())
            {
                var chuyenDonId = Protect.ToInt32(listNgayTao.First().Value);
                var viewModel = NhanDonService.ChiTietChuyenDonTheoId(chuyenDonId);
                ViewBag.listNgayTao = listNgayTao;

                return PartialView("ChuyenDon/Detail", viewModel);
            }
            ViewBag.hoSoVuAnId = hoSoVuAnId;
            return PartialView("ChuyenDon/Detail");
        }

        public ActionResult GetChuyenDonTheoId(int chuyenDonId)
        {
            var viewModel = NhanDonService.ChiTietChuyenDonTheoId(chuyenDonId);
            if (viewModel != null)
            {
                ViewBag.listNgayTao = NhanDonService.GetDanhSachNgaySuaDoiChuyenDon(viewModel.HoSoVuAnID, GetAnSessionInfo().GiaiDoanId, GetAnSessionInfo().CongDoanId, chuyenDonId);
                return PartialView("ChuyenDon/Detail", viewModel);
            }
            return PartialView("ChuyenDon/Detail");
        }

        [CustomAuthorize]
        public ActionResult EditChuyenDon(int hoSoVuAnId, int chuyenDonId)
        {
            var viewModel = NhanDonService.ChiTietChuyenDonTheoId(chuyenDonId);
            var toaAnActive = GetAnSessionInfo().ToaAnId;
            var listToaAn = SettingDataService.SelectListDanhSachToaAnValueIsToaAnID(null).Where(x => x.Value != toaAnActive.ToString()).ToList();
            string vungChuyenDon;

            if (viewModel == null)
            {
                viewModel = new ChuyenDonViewModel
                {
                    CongDoanHoSo = GetAnSessionInfo().CongDoanId,
                    GiaiDoan = GetAnSessionInfo().GiaiDoanId,
                    HoSoVuAnID = hoSoVuAnId
                };
                vungChuyenDon = Setting.VUNG_CHUYENHOSO_TRONGTINH;
            }
            else
            {
                vungChuyenDon = Setting.VUNG_CHUYENHOSO_NGOAITINH;
                foreach (var item in listToaAn)
                {
                    if (item.Text == viewModel.ToaAnChuyenDen)
                    {
                        vungChuyenDon = Setting.VUNG_CHUYENHOSO_TRONGTINH;
                        break;
                    }
                }
            }

            ViewBag.listVungChuyenDon = NhanDonService.SelectListVungChuyenDon(vungChuyenDon);
            ViewBag.listToaAnHuyen = new SelectList(listToaAn, "Text", "Text");

            return PartialView("ChuyenDon/Edit", viewModel);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult EditChuyenDon(ChuyenDonViewModel viewModel)
        {
            try
            {
                var toaAnActive = SettingDataService.GetToaAnTheoToaAnID(GetAnSessionInfo().ToaAnId);
                viewModel.ToaAnChuyenDi = toaAnActive.TenToaAn;
                viewModel.TrangThaiCongDoan = Setting.TRANGTHAICONGDOAN_CHUYENDON;

                ResponseResult result = NhanDonService.SuaDoiChuyenDon(viewModel);
                if (result != null && result.ResponseCode == 1)
                {
                    var toaAnChuyen = SettingDataService.GetToaAnTheoTenToaAn(viewModel.ToaAnChuyenDi);
                    var toaAnNhan = SettingDataService.GetToaAnTheoTenToaAn(viewModel.ToaAnChuyenDen);
                    var hoSo = NhanDonService.ChiTietHoSoVuAn(viewModel.HoSoVuAnID);
                    if (toaAnNhan != null)
                    {
                        try
                        {
                            GuiEmailChuyenDon(hoSo, viewModel, toaAnChuyen, toaAnNhan);
                        }
                        catch (Exception ex)
                        {
                            return Json(JsonResponseViewModel.CreateFail(string.Format(NotifyMessage.THONGBAO_KHONGTHANHCONG, ViewText.LABEL_GUI_EMAIL)));
                        }                 
                    }
                    //reload danh sach hosovuan is in role
                    Sessions.AddObject<List<HoSoVuAnModel>>("AnRoleObject", null);

                    return Json(JsonResponseViewModel.CreateSuccess(string.Format(NotifyMessage.CAPNHAT_THANHCONG, ViewText.LABEL_CHUYENDON)));
                }
                return Json(JsonResponseViewModel.CreateFail(string.Format(NotifyMessage.CAPNHAT_KHONGTHANHCONG, ViewText.LABEL_CHUYENDON)));
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }

        public void GuiEmailChuyenDon(HoSoVuAnModel hoSoModel, ChuyenDonViewModel chuyenDonModel, ToaAnModel toaAnChuyen, ToaAnModel toaAnNhan)
        {
            string mailBody = EmailTemplate.CHUYENDON_EMAIL_BODY;
            mailBody = mailBody.Replace("{_ToaAnNhan_}", toaAnNhan.TenToaAn);
            mailBody = mailBody.Replace("{_ToaAnChuyen_}", toaAnChuyen.TenToaAn);
            mailBody = mailBody.Replace("{_MaHoSo_}", hoSoModel.MaHoSo);
            mailBody = mailBody.Replace("{_NgayChuyen_}", chuyenDonModel.NgayChuyenDon);
            mailBody = mailBody.Replace("{_NguoiChuyen_}", string.Format(AccountUtils.CurrentFullName() + " (" + chuyenDonModel.NguoiTao + ")"));
            mailBody = mailBody.Replace("{_LyDoChuyen_}", chuyenDonModel.LyDoChuyenDon);
            mailBody = mailBody.Replace("{_EmailToaAnChuyen_}", toaAnChuyen.Email);
            Commons.SendMail(EmailTemplate.CHUYENDON_EMAIL_SUBJECT, toaAnNhan.Email, mailBody, cc: toaAnChuyen.Email);
        }
        #endregion

        #region ThamPhan
        public ActionResult ThamPhan(int hoSoVuAnId)
        {

            var listNgayTao = NhanDonService.GetDanhSachNgaySuaDoiThamPhan(hoSoVuAnId, GetAnSessionInfo().GiaiDoanId, 0);
            if (listNgayTao.Any())
            {
                var thamPhanId = Protect.ToInt32(listNgayTao.First().Value);
                var viewModel = NhanDonService.ChiTietThamPhanTheoId(thamPhanId);
                ViewBag.listNgayTao = listNgayTao;

                return PartialView("ThamPhan/Detail", viewModel);
            }
            ViewBag.hoSoVuAnId = hoSoVuAnId;

            return PartialView("ThamPhan/Detail");
        }

        public ActionResult GetThamPhanTheoId(int thamphanId)
        {
            var viewModel = NhanDonService.ChiTietThamPhanTheoId(thamphanId);
            if (viewModel != null)
            {
                ViewBag.listNgayTao = NhanDonService.GetDanhSachNgaySuaDoiThamPhan(viewModel.HoSoVuAnID, GetAnSessionInfo().GiaiDoanId, thamphanId);
                return PartialView("ThamPhan/Detail", viewModel);
            }
            return PartialView("ThamPhan/Detail");
        }

        [CustomAuthorize]
        public ActionResult EditThamPhan(int hoSoVuAnId, int thamPhanId)
        {
            var anSession = GetAnSessionInfo();
            var viewModel = new ThamPhanViewModel();
            if (thamPhanId == 0)
            {
                viewModel.HoSoVuAnID = hoSoVuAnId;
            }
            else
            {
                viewModel = NhanDonService.ChiTietThamPhanTheoId(thamPhanId);
            }

            ViewBag.listThamPhan = SettingDataService.DanhSachNhanVienTheoChucDanh("Thẩm Phán", anSession.ToaAnId, viewModel.ThamPhan);
            ViewBag.listNguoiPhanCong = SettingDataService.DanhSachNhanVienTheoTenChucNangNghiepVu("Phân Công Thẩm Phán", anSession.ToaAnId, viewModel.TenNguoiPhanCong);

            return PartialView("ThamPhan/Edit", viewModel);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult EditThamPhan(ThamPhanViewModel viewModel)
        {
            try
            {
                ResponseResult result = NhanDonService.SuaDoiThamPhan(viewModel);
                if (result != null && result.ResponseCode == 1)
                {
                    return Json(JsonResponseViewModel.CreateSuccess());
                }
                return Json(JsonResponseViewModel.CreateFail(NotifyMessage.THONGBAO_KHONGTHESUA_THONGTIN_HOSO));
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }
        #endregion

        #region SuaDoiBoSungDon
        public ActionResult GetSuaDoiBoSungDonTheoHoSoVuAnID(int id)
        {
            var viewModel = NhanDonService.ChiTietSuaDoiBoSungDonTheoHoSoVuAnID(id);

            if (viewModel != null)
            {
                ViewBag.listNgayTao = NhanDonService.DanhSachNgaySuaDoiBoSungDon(viewModel.HoSoVuAnID, GetAnSessionInfo().GiaiDoanId, 0);
            }

            return PartialView("~/Views/NhanDon/SuaDoiBoSungDon/Detail.cshtml", viewModel);
        }

        public ActionResult GetSuaDoiBoSungDonTheoSuaDoiBoSungDonID(int id)
        {
            var viewModel = NhanDonService.ChiTietSuaDoiBoSungDonTheoSuaDoiBoSungDonID(id);

            if (viewModel == null)
            {
                viewModel = new SuaDoiBoSungDonModel();
                viewModel.HoSoVuAnID = id;
            }

            viewModel.SuaDoiBoSungDonID = id;

            ViewBag.listNgayTao = NhanDonService.DanhSachNgaySuaDoiBoSungDon(viewModel.HoSoVuAnID, GetAnSessionInfo().GiaiDoanId, viewModel.SuaDoiBoSungDonID);

            return PartialView("~/Views/NhanDon/SuaDoiBoSungDon/Detail.cshtml", viewModel);
        }

        [CustomAuthorize]
        public ActionResult EditSuaDoiBoSungDon(int id)
        {
            var viewModel = NhanDonService.ChiTietSuaDoiBoSungDonTheoHoSoVuAnID(id);

            if (viewModel == null)
            {
                viewModel = new SuaDoiBoSungDonModel();
                viewModel.HoSoVuAnID = id;
                viewModel.NgayYeuCau = DateTime.Now;
                viewModel.ThoiHanSuaDoi = DateTime.Now;
            }

            return PartialView("~/Views/NhanDon/SuaDoiBoSungDon/Edit.cshtml", viewModel);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult EditSuaDoiBoSungDon(SuaDoiBoSungDonModel viewModel)
        {
            if (viewModel.NgayYeuCau > DateTime.Now)
            {
                ModelState.AddModelError("NgayYeuCau", ValidationMessages.VALIDATION_GIOIHAN_NGAYYEUCAY_SO_NGAYHIENTAI);
            }

            if (viewModel.ThoiHanSuaDoi < viewModel.NgayYeuCau)
            {
                ModelState.AddModelError("ThoiHanSuaDoi", ValidationMessages.VALIDATION_GIOIHAN_THOIHANSUADOI_SO_NGAYYEUCAU);
            }

            if (ModelState.IsValid)
            {
                ResponseResult result = NhanDonService.ThemSuaDoiBoSungDon(viewModel);

                if (result != null && result.ResponseCode == 1)
                {
                    Success(result.ResponseMessage);
                    return RedirectToAction("GetSuaDoiBoSungDonTheoHoSoVuAnID", new { id = viewModel.HoSoVuAnID });
                }
            }

            return PartialView("~/Views/NhanDon/SuaDoiBoSungDon/Edit.cshtml", viewModel);
        }
        #endregion

        #region Tra lai don
        public ActionResult GetTraLaiDonTheoHoSoVuAnID(int id)
        {
            var viewModel = NhanDonService.ChiTietTraLaiDonTheoHoSoVuAnID(id, GetAnSessionInfo().CongDoanId);

            if (viewModel != null)
            {
                ViewBag.listNgayTao = NhanDonService.DanhSachNgayTraLaiDon(viewModel.HoSoVuAnID, GetAnSessionInfo().GiaiDoanId, GetAnSessionInfo().CongDoanId, 0);
            }

            return PartialView("~/Views/NhanDon/TraLaiDon/Detail.cshtml", viewModel);
        }

        public ActionResult GetTraLaiDonTheoTraLaiDonID(int id)
        {
            var viewModel = NhanDonService.ChiTietTraLaiDonTheoTraLaiDonID(id);

            if (viewModel == null)
            {
                viewModel = new TraLaiDonModel();
                viewModel.HoSoVuAnID = id;
            }

            ViewBag.listNgayTao = NhanDonService.DanhSachNgayTraLaiDon(viewModel.HoSoVuAnID, GetAnSessionInfo().GiaiDoanId, GetAnSessionInfo().CongDoanId, viewModel.TraLaiDonID);
            return PartialView("~/Views/NhanDon/TraLaiDon/Detail.cshtml", viewModel);
        }

        [CustomAuthorize]
        public ActionResult EditTraLaiDon(int id)
        {
            var anSession = GetAnSessionInfo();
            var viewModel = NhanDonService.ChiTietTraLaiDonTheoHoSoVuAnID(id, GetAnSessionInfo().CongDoanId);
            if (viewModel == null)
            {
                viewModel = new TraLaiDonModel();
                viewModel.HoSoVuAnID = id;
                viewModel.NgayTraDon = DateTime.Now;
            }

            if (anSession.MaNhomAn == Setting.MANHOMAN_HANHCHINH)
            {
                ViewBag.ddlLyDo = SettingDataService.SettingDataItem("HC_SOTHAM_LYDOTRADON", null);
            }
            else
            {
                ViewBag.ddlLyDo = SettingDataService.SettingDataItem("DS_SOTHAM_LYDOTRADON", null);
            }
            return PartialView("~/Views/NhanDon/TraLaiDon/Edit.cshtml", viewModel);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult EditTraLaiDon(TraLaiDonModel viewModel)
        {
            var anSession = GetAnSessionInfo();
            viewModel.GiaiDoan = anSession.GiaiDoanId;
            viewModel.CongDoanHoSo = anSession.CongDoanId;
            if (viewModel.NgayTraDon > DateTime.Now)
            {
                ModelState.AddModelError("NgayTraDon", ValidationMessages.VALIDATION_GIOIHAN_NGAYTRADON_SO_NGAYHIENTAI);
            }
            if (ModelState.IsValid)
            {
                ResponseResult result = NhanDonService.ThemTraLaiDon(viewModel);

                if (result != null && result.ResponseCode == 1)
                {
                    Success(result.ResponseMessage);
                    return RedirectToAction("GetTraLaiDonTheoHoSoVuAnID", new { id = viewModel.HoSoVuAnID });
                }
            }

            if (anSession.MaNhomAn == Setting.MANHOMAN_HANHCHINH)
            {
                ViewBag.ddlLyDo = SettingDataService.SettingDataItem("HC_SOTHAM_LYDOTRADON", null);
            }
            else
            {
                ViewBag.ddlLyDo = SettingDataService.SettingDataItem("DS_SOTHAM_LYDOTRADON", null);
            }

            return PartialView("~/Views/NhanDon/TraLaiDon/Edit.cshtml", viewModel);
        }
        #endregion

        #region KhieuNaiTraDon
        public ActionResult GetKhieuNaiTraDonTheoHoSoVuAnID(int id)
        {
            var viewModel = NhanDonService.ChiTietKhieuNaiTraDonTheoHoSoVuAnID(id);

            if (viewModel != null)
            {
                ViewBag.listNgayTao = NhanDonService.DanhSachNgayKhieuNaiTraDon(viewModel.HoSoVuAnID, GetAnSessionInfo().GiaiDoanId, 0);
            }

            return PartialView("~/Views/NhanDon/KhieuNaiTraDon/_ChiTietKhieuNaiTraDon.cshtml", viewModel);
        }

        public ActionResult GetKhieuNaiTraDonTheoKhieuNaiTraDonID(int id)
        {
            var viewModel = NhanDonService.ChiTietKhieuNaiTraDonTheoKhieuNaiTraDonID(id);

            if (viewModel == null)
            {
                viewModel = new KhieuNaiTraDonModel();
                viewModel.HoSoVuAnID = id;
            }

            ViewBag.listNgayTao = NhanDonService.DanhSachNgayKhieuNaiTraDon(viewModel.HoSoVuAnID, GetAnSessionInfo().GiaiDoanId, viewModel.KhieuNaiViecTraDonID);

            return PartialView("~/Views/NhanDon/KhieuNaiTraDon/_ChiTietKhieuNaiTraDon.cshtml", viewModel);
        }

        [CustomAuthorize]
        public ActionResult EditKhieuNaiTraDon(int id)
        {
            var viewModel = NhanDonService.ChiTietKhieuNaiTraDonTheoHoSoVuAnID(id);

            if (viewModel == null)
            {
                viewModel = new KhieuNaiTraDonModel();
                viewModel.HoSoVuAnID = id;
                viewModel.NgayKhieuNai = DateTime.Now;
                viewModel.LanThu = 0;
            }

            ViewBag.ddlNhom = SettingDataService.SettingDataItem("DS_SOTHAM_NHOMKHIEUNAI", null);
            ViewBag.ddlLanThu = NhanDonService.SelectListLanThu(viewModel.LanThu);
            ViewBag.ddlNguoiKhieuNai = NhanDonService.SelectListNguoiKhieuNai(id, 0);


            return PartialView("~/Views/NhanDon/KhieuNaiTraDon/_EditKhieuNaiTraDon.cshtml", viewModel);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult EditKhieuNaiTraDon(KhieuNaiTraDonModel viewModel)
        {
            if (ModelState.IsValid)
            {
                ResponseResult result = NhanDonService.ThemKhieuNaiTraDon(viewModel);

                if (result != null && result.ResponseCode == 1)
                {
                    //Success(result.ResponseMessage);
                    return RedirectToAction("GetKhieuNaiTraDonTheoHoSoVuAnID", new { id = viewModel.HoSoVuAnID });
                }
            }

            var modelKhieuNaiTraDon = NhanDonService.ChiTietKhieuNaiTraDonTheoHoSoVuAnID(viewModel.HoSoVuAnID);
            if (modelKhieuNaiTraDon == null)
            {
                modelKhieuNaiTraDon = new KhieuNaiTraDonModel();
                modelKhieuNaiTraDon.HoSoVuAnID = viewModel.HoSoVuAnID;
                modelKhieuNaiTraDon.NgayKhieuNai = DateTime.Now;
                modelKhieuNaiTraDon.LanThu = 0;
            }
            ViewBag.ddlNhom = SettingDataService.SettingDataItem("DS_SOTHAM_NHOMKHIEUNAI", null);
            ViewBag.ddlLanThu = NhanDonService.SelectListLanThu(modelKhieuNaiTraDon.LanThu);
            ViewBag.ddlNguoiKhieuNai = NhanDonService.SelectListNguoiKhieuNai(viewModel.HoSoVuAnID, 0);

            return PartialView("~/Views/NhanDon/KhieuNaiTraDon/_EditKhieuNaiTraDon.cshtml", viewModel);
        }
        #endregion

        #region KienNghiTraDon

        public ActionResult GetKienNghiTraDonTheoHoSoVuAnID(int id)
        {
            var viewModel = NhanDonService.ChiTietKienNghiTraDonTheoHoSoVuAnID(id);

            if (viewModel != null)
            {
                ViewBag.listNgayTaoKienNghi = NhanDonService.DanhSachNgayKienNghiTraDon(viewModel.HoSoVuAnID, GetAnSessionInfo().GiaiDoanId, 0);
            }

            return PartialView("~/Views/NhanDon/KhieuNaiTraDon/_ChiTietKienNghiTraDon.cshtml", viewModel);
        }

        public ActionResult GetKienNghiTraDonTheoKienNghiTraDonID(int id)
        {
            var viewModel = NhanDonService.ChiTietKienNghiTraDonTheoKienNghiTraDonID(id);

            if (viewModel == null)
            {
                viewModel = new KienNghiTraDonModel();
                viewModel.HoSoVuAnID = id;
            }

            ViewBag.listNgayTaoKienNghi = NhanDonService.DanhSachNgayKienNghiTraDon(viewModel.HoSoVuAnID, GetAnSessionInfo().GiaiDoanId, viewModel.KienNghiViecTraDonID);

            return PartialView("~/Views/NhanDon/KhieuNaiTraDon/_ChiTietKienNghiTraDon.cshtml", viewModel);
        }

        [CustomAuthorize]
        public ActionResult EditKienNghiTraDon(int id)
        {
            var viewModel = NhanDonService.ChiTietKienNghiTraDonTheoHoSoVuAnID(id);

            if (viewModel == null)
            {
                viewModel = new KienNghiTraDonModel();
                viewModel.HoSoVuAnID = id;
                viewModel.NgayKienNghi = DateTime.Now;
                viewModel.LanThu = 0;
            }

            ViewBag.ddlLanThu = NhanDonService.SelectListLanThu(viewModel.LanThu);
            ViewBag.ddlVKSKienNghi = SettingDataService.SettingDataItem("DS_SOTHAM_VIENKIEMSATKIENNGHI", null);

            return PartialView("~/Views/NhanDon/KhieuNaiTraDon/_EditKienNghiTraDon.cshtml", viewModel);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult EditKienNghiTraDon(KienNghiTraDonModel viewModel)
        {
            if (ModelState.IsValid)
            {
                ResponseResult result = NhanDonService.ThemKienNghiTraDon(viewModel);

                if (result != null && result.ResponseCode == 1)
                {
                    //Success(result.ResponseMessage);
                    return RedirectToAction("GetKienNghiTraDonTheoHoSoVuAnID", new { id = viewModel.HoSoVuAnID });
                }
            }

            var modelKienNghiTraDon = NhanDonService.ChiTietKienNghiTraDonTheoHoSoVuAnID(viewModel.HoSoVuAnID);
            if (modelKienNghiTraDon == null)
            {
                modelKienNghiTraDon = new KienNghiTraDonModel();
                modelKienNghiTraDon.HoSoVuAnID = viewModel.HoSoVuAnID;
                modelKienNghiTraDon.NgayKienNghi = DateTime.Now;
                modelKienNghiTraDon.LanThu = 0;
            }
            ViewBag.ddlLanThu = NhanDonService.SelectListLanThu(modelKienNghiTraDon.LanThu);
            ViewBag.ddlVKSKienNghi = SettingDataService.SettingDataItem("DS_SOTHAM_VIENKIEMSATKIENNGHI", null);

            return PartialView("~/Views/NhanDon/KhieuNaiTraDon/_EditKienNghiTraDon.cshtml", viewModel);
        }

        #endregion

        #region Ajax

        [Route("updatestate")]
        [HttpPost]
        public JsonResult UpdateState(int hosoId, int trangThai)
        {
            this.HosoService.UpdateState(hosoId, trangThai);

            return Json(new { success = true });
        }

        [Route("getallstate")]
        public JsonResult GetAvailableStates()
        {
            return Json(this.TrangThaiService.GetAvailableHosoStates(), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Searching on Header by Keyword
        //progressing in Index

        //autocomplete
        public ActionResult AutoComplete(string keyword)
        {
            var viewModel = NhanDonService.DanhSachHoSoVuAnSearchByKeyword(keyword);
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region NhanHoSo PhucTham
        public ActionResult NhanHoSoPhucTham()
        {
            return View("NhanHoSoPhucTham/NhanHoSoPhucTham");
        }

        public ActionResult DanhSachHoSoChoPhucTham()
        {
            var anSession = GetAnSessionInfo();
            var viewModel = NhanDonService.GetDanhSachHoSoChoPhucTham(anSession.MaNhomAn);
            return Json(new { data = viewModel }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetChiTietHoSoChoNhanPhucTham(int chuyenDonId)
        {
            var viewModel = NhanDonService.ChiTietChuyenDonTheoId(chuyenDonId);
            return PartialView("NhanHoSoPhucTham/_ThongTinChiTietHoSoChoNhanPhucTham", viewModel);
        }

        [CustomAuthorize]
        public ActionResult XacNhanHoSoPhucTham(int hoSoVuAnId)
        {
            var viewModel = new NhanHoSoModel();
            viewModel.HoSoVuAnID = hoSoVuAnId;
            viewModel.NgayNhanHoSo = DateTime.Now;
            var nv = SettingDataService.GetNhanVienSessionInfo();
            //display current username
            viewModel.NguoiNhanHoSo = nv.HoVaTen+" ("+nv.MaNVMoi+")";

            return View("NhanHoSoPhucTham/_XacNhanHoSoPhucTham", viewModel);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult XacNhanHoSoPhucTham(NhanHoSoModel viewModel)
        {
            try
            {
                var hoSoVuAnModel = new HoSoVuAnModel()
                {
                    HoSoVuAnID = viewModel.HoSoVuAnID,
                    ToaAnID = ToaAn.TinhCaMau.GetHashCode(),
                    GiaiDoan = GiaiDoan.PhucTham.GetHashCode(),
                    CanBoNhanDon = SettingDataService.GetNhanVienSessionInfo().MaNV,
                    TrangThaiCongDoan = ViewText.LABEL_NHANHOSO
                };
                viewModel.GiaiDoan = GiaiDoan.PhucTham.GetHashCode();

                ResponseResult resultNhanHoSo = NhanDonService.SuaChiTietNhanHoSo(viewModel);

                if (resultNhanHoSo.ResponseCode == 1)
                {
                    ResponseResult resultHoSoVuAn = NhanDonService.XacNhanNhanHoSoPhucTham(hoSoVuAnModel);
                    if (resultHoSoVuAn.ResponseCode == 1)
                    {
                        var chuyenDonModel = NhanDonService.ChiTietChuyenDonTheoHoSoVuAnID(viewModel.HoSoVuAnID, GiaiDoan.SoTham.GetHashCode(), CongDoan.SauXetXu.GetHashCode());
                        var toaAnChuyen = SettingDataService.GetToaAnTheoTenToaAn(chuyenDonModel.ToaAnChuyenDen);
                        var toaAnNhan = SettingDataService.GetToaAnTheoTenToaAn(chuyenDonModel.ToaAnChuyenDi);
                        var hoSo = NhanDonService.ChiTietHoSoVuAn(viewModel.HoSoVuAnID);
                        if (toaAnNhan != null)
                        {
                            GuiEmailNhanHoSo(hoSo, viewModel, toaAnChuyen, toaAnNhan);
                        }

                        //reload danh sach hosovuan is in role
                        Sessions.AddObject<List<HoSoVuAnModel>>("AnRoleObject", null);
                        return Json(JsonResponseViewModel.CreateSuccess(string.Format(NotifyMessage.THONGBAO_THANHCONG, ViewText.LABEL_NHANHOSO)));
                    }
                }
                return Json(JsonResponseViewModel.CreateFail(string.Format(NotifyMessage.CAPNHAT_KHONGTHANHCONG, ViewText.LABEL_NHANHOSO.ToLower())));
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }

        public void GuiEmailNhanHoSo(HoSoVuAnModel hoSoModel, NhanHoSoModel nhanHoSoModel, ToaAnModel toaAnChuyen, ToaAnModel toaAnNhan)
        {
            try
            {
                string mailBody = EmailTemplate.NHANHOSOPHUCTHAM_EMAIL_BODY;
                mailBody = mailBody.Replace("{_ToaAnNhan_}", toaAnNhan.TenToaAn);
                mailBody = mailBody.Replace("{_ToaAnChuyen_}", toaAnChuyen.TenToaAn);
                mailBody = mailBody.Replace("{_MaHoSo_}", hoSoModel.MaHoSo);
                mailBody = mailBody.Replace("{_NguoiNhanHoSo_}", nhanHoSoModel.NguoiNhanHoSo);
                mailBody = mailBody.Replace("{_EmailToaAnChuyen_}", toaAnChuyen.Email);
                Commons.SendMail(EmailTemplate.NHANHOSOPHUCTHAM_EMAIL_SUBJECT, toaAnNhan.Email, mailBody, cc: toaAnChuyen.Email);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("GuiEmailChuyenHoSo with error [{0}]", ex.ToString()), AppName.BizSecurity);
                throw;
            }
        }
        #endregion

        #region HoSoVuAn ApDung_BPXLHC
        [CustomAuthorize]
        public ActionResult ThemHoSoVuAnApDungBPXLHC(int? id)
        {
            var anSession = GetAnSessionInfo();
            int hoSoVuAnID = id ?? 0;

            var viewModel = NhanDonService.ChiTietHoSoVuAnApDungBPXLHC(hoSoVuAnID);

            ViewBag.SoThuLy = viewModel != null ? viewModel.SoThuLy : _thulyService.SoThuLyMaxChoADBPXLHC(anSession.ToaAnId, anSession.MaNhomAn, anSession.GiaiDoanId).ToString();

            if (viewModel == null)
            {
                viewModel = new HoSoVuAnApDungModel
                {
                    HoSoVuAnID = 0,
                    NgayNopDonTaiToaAn = DateTime.Now
                };

            }

            ViewBag.ddlTenCoQuanDeNghi = SettingDataService.SettingDataItem("AD_SOTHAM_TENCOQUANDENGHI", null);
            ViewBag.ddlHoSoDeNghi = SettingDataService.SettingDataItem("AD_SOTHAM_HOSODENGHI", null);
            ViewBag.ddlBienPhapXLHC = SettingDataService.SettingDataItem("AD_SOTHAM_BIENPHAPXULYHANHCHINH", null);
            ViewBag.ddlThoiHanApDung = SettingDataService.SettingDataItem("AD_SOTHAM_THOIHANAPDUNG", null);
            ViewBag.ddlHinhThucGoiDon = SettingDataService.SettingDataItem("AD_SOTHAM_HINHTHUCNHANHOSO", null);
            viewModel.CanBoNhanDon = SettingDataService.ChiTietNhanVienTheoMaNhanVien(AccountUtils.CurrentUsername()).HoTenVaMaNV;

            return PartialView("~/Views/NhanDon/_NhanHoSoMoiApDungBPXLHC.cshtml", viewModel);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult ThemHoSoVuAnApDungBPXLHC(HoSoVuAnApDungModel viewModel)
        {
            var anSession = GetAnSessionInfo();

            viewModel.CanBoNhanDon = SettingDataService.GetNhanVienSessionInfo().MaNV;

            if (ModelState.IsValid)
            {
                ResponseResult result = null;
                if (viewModel.HoSoVuAnID > 0)
                {
                    result = NhanDonService.SuaHoSoVuAnApDungBPXLHC(viewModel);
                }
                else
                {
                    viewModel.ToaAnID = anSession.ToaAnId;
                    result = NhanDonService.ThemHoSoVuAnApDungBPXLHC(viewModel);
                }


                if (result != null && result.ResponseCode == 1)
                {
                    //reload danh sach hosovuan is in role
                    Sessions.AddObject<List<HoSoVuAnModel>>("AnRoleObject", null);

                    Success(result.ResponseMessage);
                    return Json(new { status = "success" });
                }
            }

            ViewBag.ddlTenCoQuanDeNghi = SettingDataService.SettingDataItem("AD_SOTHAM_TENCOQUANDENGHI", null);
            ViewBag.ddlHoSoDeNghi = SettingDataService.SettingDataItem("AD_SOTHAM_HOSODENGHI", null);
            ViewBag.ddlBienPhapXLHC = SettingDataService.SettingDataItem("AD_SOTHAM_BIENPHAPXULYHANHCHINH", null);
            ViewBag.ddlThoiHanApDung = SettingDataService.SettingDataItem("AD_SOTHAM_THOIHANAPDUNG", null);
            ViewBag.ddlHinhThucGoiDon = SettingDataService.SettingDataItem("AD_SOTHAM_HINHTHUCNHANHOSO", null);
            viewModel.CanBoNhanDon = SettingDataService.ChiTietNhanVienTheoMaNhanVien(AccountUtils.CurrentUsername()).HoTenVaMaNV;

            return PartialView("~/Views/NhanDon/_NhanHoSoMoiApDungBPXLHC.cshtml", viewModel);
        }

        [CustomAuthorize]
        public ActionResult ChiTietHoSoApDungBPXLHC(int id)
        {
            var giaiDoan = GetAnSessionInfo().GiaiDoanId;
            if (giaiDoan == GiaiDoan.PhucTham.GetHashCode())
            {
                return RedirectToAction("Index", "NhanHoSo", new { id = id });
            }

            UpdateAnSessionInfo(idToaAn: GetAnSessionInfo().ToaAnId, maToaAn: GetAnSessionInfo().MaToaAn, maNhomAn: GetAnSessionInfo().MaNhomAn, idGiaiDoan: giaiDoan, idCongDoan: CongDoan.NhanDon.GetHashCode(), idVuAn: id);
            var chiTietHoSo = NhanDonService.ChiTietHoSoVuAnApDungBPXLHCTheoId(id);
            var viewModel = NhanDonService.ChiTietHoSoVuAnApDungBPXLHCTheoGiaiDoan(id, GiaiDoan.SoTham.GetHashCode());

            if (viewModel != null)
            {
                ViewBag.listNgayTao = NhanDonService.DanhSachNgayHoSoVuAn(viewModel.HoSoVuAnID, giaiDoan, 0);
                ViewBag.ddlTrangThai = NhanDonService.SelectListCongDoanHoSo(viewModel.CongDoanHoSo, giaiDoan);
            }
            ViewBag.ActiveCongDoan = chiTietHoSo.CongDoanHoSo;

            var giaiDoanHoSo = chiTietHoSo.GiaiDoan;
            ViewBag.RoleGiaiDoan = giaiDoanHoSo == giaiDoan ? 1 : -1;

            return View("~/Views/NhanDon/ChiTietHoSoApDungBPXLHC.cshtml", viewModel);
        }

        public ActionResult ChiTietHoSoApDungBPXLHCTheoLog(int id)
        {
            var giaiDoan = GetAnSessionInfo().GiaiDoanId;
            var viewModel = NhanDonService.ChiTietHoSoVuAnApDungBPXLHCTheoLog(id);

            if (viewModel != null)
            {
                ViewBag.listNgayTao = NhanDonService.DanhSachNgayHoSoVuAn(viewModel.HoSoVuAnID, giaiDoan, 0);
            }

            return PartialView("~/Views/NhanDon/ChiTietHoSoLogApDungBPXLHC.cshtml", viewModel);
        }
        #endregion

        #region HoSoVuAn HinhSu
        [CustomAuthorize]
        public ActionResult ChiTietHoSoHinhSu(int id)
        {
            var giaiDoan = GetAnSessionInfo().GiaiDoanId;
            if (giaiDoan == GiaiDoan.PhucTham.GetHashCode())
            {
                return RedirectToAction("Index", "NhanHoSo", new { id = id });
            }

            UpdateAnSessionInfo(idToaAn: GetAnSessionInfo().ToaAnId, maToaAn: GetAnSessionInfo().MaToaAn, maNhomAn: GetAnSessionInfo().MaNhomAn, idGiaiDoan: giaiDoan, idCongDoan: CongDoan.NhanDon.GetHashCode(), idVuAn: id);
            var chiTietHoSo = NhanDonService.ChiTietHoSoVuAnHinhSuTheoId(id);
            var viewModel = NhanDonService.ChiTietHoSoVuAnHinhSuTheoGiaiDoan(id, GiaiDoan.SoTham.GetHashCode());

            if (viewModel != null)
            {
                ViewBag.listNgayTao = NhanDonService.DanhSachNgayHoSoVuAn(viewModel.HoSoVuAnID, giaiDoan, 0);
                ViewBag.ddlTrangThai = NhanDonService.SelectListCongDoanHoSo(viewModel.CongDoanHoSo, giaiDoan);
            }
            ViewBag.ActiveCongDoan = chiTietHoSo.CongDoanHoSo;

            var giaiDoanHoSo = chiTietHoSo.GiaiDoan;
            ViewBag.RoleGiaiDoan = giaiDoanHoSo == giaiDoan ? 1 : -1;

            return View("~/Views/NhanDon/ChiTietHoSoHinhSu.cshtml", viewModel);
        }

        public ActionResult ChiTietHoSoHinhSuTheoLog(int id)
        {
            var giaiDoan = GetAnSessionInfo().GiaiDoanId;
            var viewModel = NhanDonService.ChiTietHoSoVuAnHinhSuTheoLog(id);

            if (viewModel != null)
            {
                ViewBag.listNgayTao = NhanDonService.DanhSachNgayHoSoVuAn(viewModel.HoSoVuAnID, giaiDoan, 0);
            }

            return PartialView("~/Views/NhanDon/ChiTietHoSoLogHinhSu.cshtml", viewModel);
        }

        [CustomAuthorize]
        public ActionResult ThemHoSoVuAnHinhSu(int? id)
        {
            int hoSoVuAnID = id ?? 0;

            var viewModel = NhanDonService.ChiTietHoSoVuAnHinhSu(hoSoVuAnID);

            if (viewModel == null)
            {
                viewModel = new HoSoVuAnHinhSuModel()
                {
                    HoSoVuAnID = 0,
                    NgayTruyTo = DateTime.Now
                };
            }
            //else
            //{
            //    if (SettingDataService.SettingDataItem("HS_SOTHAM_VIENKIEMSATTRUYTO", null).Where(m => m.Value == viewModel.VienKiemSatTruyTo).Count() == 0)
            //    {
            //        viewModel.VienKiemSatTruyToKhac = viewModel.VienKiemSatTruyTo;
            //        viewModel.VienKiemSatTruyTo = "";
            //    }
            //}

            ViewBag.ddlTruyToBang = SettingDataService.SettingDataItem("HS_SOTHAM_TRUYTOBANG", null);
            ViewBag.ddlVKSTruyTo = SettingDataService.SettingDataItem("HS_SOTHAM_VIENKIEMSATTRUYTO", null);

            return PartialView("~/Views/NhanDon/_NhanHoSoMoiHinhSu.cshtml", viewModel);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult ThemHoSoVuAnHinhSu(HoSoVuAnHinhSuModel viewModel)
        {
            var anSession = GetAnSessionInfo();

            viewModel.CanBoNhanDon = SettingDataService.GetNhanVienSessionInfo().MaNV;
            if (viewModel.HoSoVuAnID == 0 && viewModel.NgayTruyTo > DateTime.Now.Date)
            {
                ModelState.AddModelError("NgayTruyTo", "Ngày tháng năm không được lớn hơn ngày hiện tại.");
            }

            if (ModelState.IsValid)
            {
                ResponseResult result = null;
                if (viewModel.HoSoVuAnID > 0)
                {
                    result = NhanDonService.SuaHoSoVuAnHinhSu(viewModel);
                }
                else
                {
                    viewModel.ToaAnID = anSession.ToaAnId;
                    result = NhanDonService.ThemHoSoVuAnHinhSu(viewModel);
                }


                if (result != null && result.ResponseCode == 1)
                {
                    //reload danh sach hosovuan is in role
                    Sessions.AddObject<List<HoSoVuAnModel>>("AnRoleObject", null);

                    Success(result.ResponseMessage);
                    return Json(new { status = "success" });
                }
            }

            ViewBag.ddlTruyToBang = SettingDataService.SettingDataItem("HS_SOTHAM_TRUYTOBANG", null);
            ViewBag.ddlVKSTruyTo = SettingDataService.SettingDataItem("HS_SOTHAM_VIENKIEMSATTRUYTO", null);
            viewModel.CanBoNhanDon = SettingDataService.ChiTietNhanVienTheoMaNhanVien(AccountUtils.CurrentUsername()).HoTenVaMaNV;

            return PartialView("~/Views/NhanDon/_NhanHoSoMoiHinhSu.cshtml", viewModel);
        }
        #endregion

        #region ToiDanhTruyTo
        public ActionResult DanhSachToiDanhTruyTo(int hoSoVuAnId)
        {
            var viewModel = NhanDonService.GetDanhSachToiDanhTruyTo(hoSoVuAnId);

            return Json(new { data = viewModel }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChiTietToiDanhTruyTo(int toiDanhTruyToId)
        {
            var viewModel = NhanDonService.ChiTietToiDanhTruyToTheoId(toiDanhTruyToId);
            var khoandiem = NhanDonService.DanhSachToiDanhTruyToKhoanDiemTheoId(toiDanhTruyToId);
            if (khoandiem == null || !khoandiem.Any())
                viewModel.KhoanDiem = new List<ToiDanhTruyTo_KhoanDiemModel>();
            else
                viewModel.KhoanDiem = khoandiem;
            return PartialView("ToiDanhTruyTo/_ThongTinToiDanhTruyTo", viewModel);
        }

        
        [CustomAuthorize]
        public ActionResult ThemToiDanhTruyTo(int hoSoVuAnId)
        {
            var viewModel = new ToiDanhTruyToModel {HoSoVuAnID = hoSoVuAnId};
            var temp1 = SettingDataService.ListSettingDataItem("HS_SOTHAM_TOIDANH").Where(x => x.ItemText.Contains("Điều") || x.ItemText=="Khác");
            foreach(var item in temp1)
            {
                if (item.ItemText != "Khác")
                {
                    string str = item.ItemText.Substring(0, item.ItemText.IndexOf("."));
                    string str2 = item.ItemText.Substring(item.ItemText.IndexOf(".") + 2);
                    item.ItemText = str;
                    item.ItemRemark = str2;
                }
                else
                    item.ItemRemark = "";
            }
            ViewBag.ddlDieu = new SelectList(temp1, "ItemText", "ItemText");
            ViewBag.ddlKhoan = new SelectList(SettingDataService.ListSettingDataItem("HS_TOIDANHTRUYTO_KHOAN").OrderBy(x => int.Parse(x.ItemValue)), "ItemText", "ItemText");
            ViewBag.ToiDanhHiddenList = new SelectList(temp1, "ItemText", "ItemRemark");
            return PartialView("ToiDanhTruyTo/_ThemToiDanhTruyTo", viewModel);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult ThemToiDanhTruyTo(ToiDanhTruyToModel viewModel)
        {
            try
            {
                ResponseResult result = NhanDonService.ThemToiDanhTruyTo(viewModel);
                if (result != null && result.ResponseCode == 1)
                {
                    foreach(var item in viewModel.KhoanDiem)
                    {
                        if(item.check==1)
                        {
                            item.ToiDanhTruyToID = int.Parse(result.ResponseMessage);
                            var result2 = NhanDonService.ThemToiDanhTruyTo_KhoanDiem(item);
                            if (result2 == null || result2.ResponseCode == 0)
                                return Json(JsonResponseViewModel.CreateFail("Có lỗi xảy ra. Thêm tội danh truy tố không thành công."));
                        }
                    }
                    return Json(JsonResponseViewModel.CreateSuccess("Thêm tội danh truy tố thành công."));
                }
                return Json(JsonResponseViewModel.CreateFail("Có lỗi xảy ra. Thêm tội danh truy tố không thành công."));
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }
       
        [CustomAuthorize]
        public ActionResult SuaToiDanhTruyTo(int toiDanhTruyToId)
        {
            var viewModel = NhanDonService.ChiTietToiDanhTruyToTheoId(toiDanhTruyToId) ?? new ToiDanhTruyToModel();
            viewModel.KhoanDiem = NhanDonService.DanhSachToiDanhTruyToKhoanDiemTheoId(toiDanhTruyToId);
            var khoandiem = NhanDonService.DanhSachToiDanhTruyToKhoanDiemTheoId(toiDanhTruyToId);
            if (khoandiem == null || !khoandiem.Any())
                viewModel.KhoanDiem = new List<ToiDanhTruyTo_KhoanDiemModel>();
            else
                viewModel.KhoanDiem = khoandiem;
            var temp1 = SettingDataService.ListSettingDataItem("HS_SOTHAM_TOIDANH").Where(x => x.ItemText.Contains("Điều") || x.ItemText == "Khác");
            foreach (var item in temp1)
            {
                if (item.ItemText != "Khác")
                {
                    string str = item.ItemText.Substring(0, item.ItemText.IndexOf("."));
                    string str2 = item.ItemText.Substring(item.ItemText.IndexOf(".") + 2);
                    item.ItemText = str;
                    item.ItemRemark = str2;
                }
                else
                    item.ItemRemark = "";
            }
            ViewBag.ddlDieu = new SelectList(temp1, "ItemText", "ItemText");
            ViewBag.ddlistKhoan = new SelectList(SettingDataService.ListSettingDataItem("HS_TOIDANHTRUYTO_KHOAN").OrderBy(x=> int.Parse(x.ItemValue)), "ItemText", "ItemText");
            ViewBag.ToiDanhList = new SelectList(temp1, "ItemText", "ItemRemark");
            return PartialView("ToiDanhTruyTo/_SuaToiDanhTruyTo", viewModel);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult SuaToiDanhTruyTo(ToiDanhTruyToModel viewModel)
        {
            try
            {
                ResponseResult result = NhanDonService.SuaToiDanhTruyTo(viewModel);
                if (result != null && result.ResponseCode == 1)
                {
                    foreach (var item in viewModel.KhoanDiem)
                    {
                        if (item.check!=1)
                        {
                            result = NhanDonService.XoaToiDanhTruyTo_KhoanDiem(item.KhoanDiemID);
                            if (result == null || result.ResponseCode == 0)
                                return Json(JsonResponseViewModel.CreateFail("Có lỗi xảy ra. Sửa tội danh truy tố không thành công."));
                        }
                        else if(item.KhoanDiemID==0 && item.check==1)
                        {
                            item.ToiDanhTruyToID = viewModel.ToiDanhTruyToID;
                            result = NhanDonService.ThemToiDanhTruyTo_KhoanDiem(item);
                            if (result == null || result.ResponseCode == 0)
                                return Json(JsonResponseViewModel.CreateFail("Có lỗi xảy ra. Sửa tội danh truy tố không thành công."));
                        }
                        else if(item.check==1)
                        {
                            item.ToiDanhTruyToID = viewModel.ToiDanhTruyToID;
                            result = NhanDonService.SuaToiDanhTruyTo_KhoanDiem(item);
                            if (result == null || result.ResponseCode == 0)
                                return Json(JsonResponseViewModel.CreateFail("Có lỗi xảy ra. Sửa tội danh truy tố không thành công."));
                        }

                    }
                    return Json(JsonResponseViewModel.CreateSuccess("Sửa tội danh truy tố thành công."));
                }
                return Json(JsonResponseViewModel.CreateFail("Có lỗi xảy ra. Sửa tội danh truy tố không thành công."));
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }

        public ActionResult KhoanDiemItem(int i)
        {
            ViewBag.count = i;
            ViewBag.ddlKhoan = new SelectList(SettingDataService.ListSettingDataItem("HS_TOIDANHTRUYTO_KHOAN").OrderBy(x => int.Parse(x.ItemValue)), "ItemText", "ItemText");
            return PartialView("ToiDanhTruyTo/_KhoanDiemItem");
        }

        [CustomAuthorize]
        public ActionResult GetXoaToiDanhTruyTo(int toiDanhTruyToId)
        {
            return PartialView("ToiDanhTruyTo/_XoaToiDanhTruyTo", toiDanhTruyToId);
        }

        [HttpPost]
        public ActionResult XoaToiDanhTruyTo(int toiDanhTruyToId)
        {
            try
            {
                ResponseResult result = NhanDonService.XoaToiDanhTruyTo(toiDanhTruyToId);
                if (result != null && result.ResponseCode == 1)
                {
                    return Json(JsonResponseViewModel.CreateSuccess("Xóa tội danh truy tố thành công."));
                }
                return Json(JsonResponseViewModel.CreateFail("Có lỗi xảy ra. Xóa tội danh truy tố không thành công."));
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }
        #endregion
    }
}