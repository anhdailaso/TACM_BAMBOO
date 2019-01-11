using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Biz.Lib.Authentication;
using Biz.TACM.IServices;
using Biz.TACM.Models.ViewModel.SauXetXu.KhangCao;
using Biz.TACM.Models.ViewModel.SauXetXu.KhangCao.Request;
using Biz.TACM.Models.ViewModel.SauXetXu.KhangCaoQuaHan;
using Biz.TACM.Models.ViewModel.SauXetXu.KhangCaoQuaHan.Request;
using Biz.TACM.Models.ViewModel.SauXetXu.KhangNghi;
using Biz.TACM.Models.ViewModel.SauXetXu.PhatHanhBanAn;
using Biz.TACM.Models.ViewModel.SauXetXu.SuaChuaBanAn;
using Biz.TACM.Models.ViewModel.SauXetXu.SuaChuaBanAn.Request;
using Biz.TACM.Models.ViewModel.Shared;
using Biz.TACM.Models.ViewModel.SauXetXu.KhangNghi.Request;
using Biz.TACM.Models.ViewModel.SauXetXu.LuuKho;
using Biz.TACM.Models.ViewModel.SauXetXu.LuuKho.Request;
using Biz.TACM.Models.ViewModel.SauXetXu.TamUngAnPhi;
using Biz.TACM.Models.ViewModel.SauXetXu.ChuyenHoSo;
using Biz.Lib.TACM.SauXetXu.Model.LuuKho.DataRequest;
using System.Web;
using System.IO;
using Biz.Lib.Helpers;
using Biz.Lib.SettingData.Model;
using Biz.Lib.TACM.NhanDon.Model;
using Biz.Lib.TACM.Resources.Resources;
using Biz.TACM.Enums;
using HoSoVuAnModel = Biz.Lib.TACM.NhanDon.Model.HoSoVuAnModel;
using ResponseResult = Biz.Lib.TACM.SauXetXu.Model.TamUngAnPhi.ResponseResult;
using Biz.Lib.TACM.KetQuaXetXu.Model;

namespace Biz.TACM.Controllers
{
    public class SauXetXuController : BizController
    {
        private readonly ISettingDataService _settingDataService;
        private readonly INhanDonService _nhanDonService;
        private readonly ISauXetXuService _sauXetXuService;
        private readonly IKetQuaXetXuService _ketQuaXetXuService;

        public SauXetXuController(ISauXetXuService sauXetXuService, ISettingDataService settingDataService, INhanDonService nhanDonService, IKetQuaXetXuService ketQuaXetXuService)
        {
            _sauXetXuService = sauXetXuService;
            _settingDataService = settingDataService;
            _nhanDonService = nhanDonService;
            _ketQuaXetXuService = ketQuaXetXuService;
        }

        // GET: SauXetXu
        [CustomAuthorize]
        public ActionResult Index(int id)
        {
            var anSession = GetAnSessionInfo();
            var chiTietHoSo = _nhanDonService.ChiTietHoSoVuAnTheoId(id);

            if (anSession.GiaiDoanId == 0)
                anSession.GiaiDoanId = chiTietHoSo.GiaiDoan;
            UpdateAnSessionInfo(idToaAn: chiTietHoSo.ToaAnID, maToaAn: anSession.MaToaAn, maNhomAn: chiTietHoSo.NhomAn, idGiaiDoan: anSession.GiaiDoanId, idCongDoan: CongDoan.SauXetXu.GetHashCode(), idVuAn: id);

            var viewModel = _nhanDonService.ChiTietHoSoVuAn(id);
            if (viewModel == null)
            {
                viewModel = new HoSoVuAnModel
                {
                    HoSoVuAnID = id,
                    CongDoanHoSo = GetAnSessionInfo().CongDoanId,
                    GiaiDoan = GetAnSessionInfo().GiaiDoanId
                };
            }
            ViewBag.ActiveCongDoan = viewModel.CongDoanHoSo;
            ViewBag.ddlTrangThai = _nhanDonService.SelectListCongDoanHoSo(viewModel.CongDoanHoSo, anSession.GiaiDoanId);

            var giaiDoanHoSo = _nhanDonService.ChiTietHoSoVuAn(id).GiaiDoan;
            ViewBag.RoleGiaiDoan = giaiDoanHoSo == anSession.GiaiDoanId ? 1 : -1;

            //reload danh sach hosovuan is in role
            Sessions.AddObject<List<HoSoVuAnModel>>("AnRoleObject", null);

            return View(viewModel);
        }

        #region PhatHanhBanAn
        [HttpGet]
        public ActionResult PhatHanhBanAn(int hoSoVuAnId)
        {
            var danhSachNgayPhatHanhBanAn = _sauXetXuService.DanhSachNgayPhatHanhBanAn(hoSoVuAnId, GetAnSessionInfo().GiaiDoanId).ToList();
            BanAnModel modelBanAn = _ketQuaXetXuService.ChiTietBanAnTheoHoSoVuAnID(hoSoVuAnId, GetAnSessionInfo().GiaiDoanId);

            if (modelBanAn == null)
            {
                modelBanAn = new BanAnModel();
                modelBanAn.NgayTuyenAn = DateTime.Now;
            }
            else
            {
                if (modelBanAn.NgayTuyenAn == null)
                    modelBanAn.NgayTuyenAn = DateTime.Now;
            }

            if (danhSachNgayPhatHanhBanAn.Any())
            {
                var phatHanhBanAnId = danhSachNgayPhatHanhBanAn.First().PhatHanhBanAnId;
                var thongTinPhatHanhBanAn = _sauXetXuService.ThongTinPhatHanhBanAnTheoId(phatHanhBanAnId);

                var selectListItem = danhSachNgayPhatHanhBanAn.Select(s => new SelectListItem
                {
                    Selected = s.PhatHanhBanAnId == phatHanhBanAnId,
                    Text = s.NgayPhatHanh,
                    Value = s.PhatHanhBanAnId.ToString()
                });

                var viewModel = new PhatHanhBanAnViewModel
                {
                    DanhSachNgayPhatHanhBanAn = new SelectList(selectListItem, "Value", "Text"),
                    ThongTinPhatHanhBanAn = new ThongTinPhatHanhBanAnViewModel
                    {
                        PhatHanhBanAnId = thongTinPhatHanhBanAn.PhatHanhBanAnId,
                        NgayPhatHanh = thongTinPhatHanhBanAn.NgayPhatHanh,
                        HieuLuc = thongTinPhatHanhBanAn.HieuLuc,
                        NgayTao = thongTinPhatHanhBanAn.NgayTao,
                        NguoiTao = thongTinPhatHanhBanAn.NguoiTao,
                        NgayTuyenAn = modelBanAn.NgayTuyenAn.ToString("dd/MM/yyyy")
                    }
                };

                return PartialView("PhatHanhBanAn/_PhatHanhBanAn", viewModel);
            }

            ViewBag.NgayTuyenAn = modelBanAn.NgayTuyenAn.ToString("dd/MM/yyyy");
            return PartialView("PhatHanhBanAn/_PhatHanhBanAn");
        }

        [CustomAuthorize]
        [HttpGet]
        public ActionResult PhatHanhBanAnModal(int phatHanhBanAnId)
        {
            if (phatHanhBanAnId != 0)
            {
                var thongTinPhatHanhBanAn = _sauXetXuService.ThongTinPhatHanhBanAnTheoId(phatHanhBanAnId);
                ViewBag.ngayphathanh = thongTinPhatHanhBanAn.NgayPhatHanh;
                ViewBag.HieuLuc = thongTinPhatHanhBanAn.HieuLuc;
            }
            else
            {
                ViewBag.ngayphathanh = null;
                ViewBag.HieuLuc = null;
            }
            return PartialView("PhatHanhBanAn/_PhatHanhBanAnModal");
        }

        [HttpGet]
        public ActionResult ThongTinPhatHanhBanAnTheoId(int phatHanhBanAnId, int hoSoVuAnId)
        {
            var thongTinPhatHanhBanAn = _sauXetXuService.ThongTinPhatHanhBanAnTheoId(phatHanhBanAnId);
            var modelBanAn = _ketQuaXetXuService.ChiTietBanAnTheoHoSoVuAnID(hoSoVuAnId, GetAnSessionInfo().GiaiDoanId);

            var viewModel = new ThongTinPhatHanhBanAnViewModel
            {
                PhatHanhBanAnId = thongTinPhatHanhBanAn.PhatHanhBanAnId,
                NgayPhatHanh = thongTinPhatHanhBanAn.NgayPhatHanh,
                HieuLuc = thongTinPhatHanhBanAn.HieuLuc,
                NgayTao = thongTinPhatHanhBanAn.NgayTao,
                NguoiTao = thongTinPhatHanhBanAn.NguoiTao,
                NgayTuyenAn = modelBanAn.NgayTuyenAn.ToString("dd/MM/yyyy")
            };

            return PartialView("PhatHanhBanAn/_ThongTinPhatHanhBanAn", viewModel);
        }


        [HttpGet]
        public ActionResult DanhSachDuongSuNhanPhatHanBanAnTheoPhatHanhBanAn(int phatHanhBanAnId)
        {
            var viewModel = _sauXetXuService.DanhSachDuongSuNhanPhatHanhBanAn(phatHanhBanAnId);
            return Json(new { data = viewModel }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DanhSachDuongSuTheoHoSo(int hoSoVuAnId, int? phatHanhBanAnId)
        {
            var viewModel = _sauXetXuService.DanhSachDuongSuTheoHoSo(hoSoVuAnId, phatHanhBanAnId);
            return Json(new { data = viewModel }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult PhatHanhBanAn(int hoSoVuAnId, DateTime ngayPhatHanh, DateTime? hieuLuc, IEnumerable<int> duongSuNhanPhatHanhBanAnIds, IEnumerable<string> duongSuNgayPhatHanh)
        {
            try
            {
                var result = _sauXetXuService.PhatHanhBanAn(hoSoVuAnId, ngayPhatHanh, hieuLuc, duongSuNhanPhatHanhBanAnIds, duongSuNgayPhatHanh);

                if (result)
                {
                    return Json(JsonResponseViewModel.CreateSuccess());
                }

                return Json(JsonResponseViewModel.CreateFail(NotifyMessage.THONGBAO_KHONGTHE_PHATHANH_BANAN));
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }

        }
        #endregion

        #region SuaChuaBanAn
        [HttpGet]
        public ActionResult SuaChuaBanAn(int hoSoVuAnId)
        {
            var danhSachNgaySuaChuaBanAn = _sauXetXuService.DanhSachNgaySuaChuaBanAn(hoSoVuAnId, GetAnSessionInfo().GiaiDoanId).ToList();
            if (danhSachNgaySuaChuaBanAn.Any())
            {
                var suaChuaBanAnId = danhSachNgaySuaChuaBanAn.First().SuaChuaBanAnId;
                var thongTinSuaChuaBanAn = _sauXetXuService.ThongTinSuaChuaBanAnTheoId(suaChuaBanAnId);

                var selectListItem = danhSachNgaySuaChuaBanAn.Select(s => new SelectListItem
                {
                    Selected = s.SuaChuaBanAnId == suaChuaBanAnId,
                    Text = s.NgaySuaChua,
                    Value = s.SuaChuaBanAnId.ToString()
                });

                var viewModel = new SuaChuaBanAnViewModel
                {
                    DanhSachNgaySuaChuaBanAn = new SelectList(selectListItem, "Value", "Text"),
                    ThongTinSuaChuaBanAn = new ThongTinSuaChuaBanAnViewModel
                    {
                        SuaChuaBanAnId = thongTinSuaChuaBanAn.SuaChuaBanAnId,
                        NgaySuaChua = thongTinSuaChuaBanAn.NgaySuaChua,
                        LyDo = thongTinSuaChuaBanAn.LyDo,
                        NguoiKy = thongTinSuaChuaBanAn.NguoiKy,
                        NgayTao = thongTinSuaChuaBanAn.NgayTao,
                        NguoiTao = thongTinSuaChuaBanAn.NguoiTao,
                        NoiDung = thongTinSuaChuaBanAn.NoiDung,
                        DinhKemFile = thongTinSuaChuaBanAn.DinhKemFile
                    }
                };

                return PartialView("SuaChuaBanAn/_SuaChuaBanAn", viewModel);
            }

            return PartialView("SuaChuaBanAn/_SuaChuaBanAn");
        }

        [CustomAuthorize]
        [HttpGet]
        public ActionResult SuaChuaBanAnModal(int id)
        {
            var anSession = GetAnSessionInfo();
            //ViewBag.danhSachNguoiKy = _settingDataService.SettingDataItem("DS_SOTHAM_KYSUACHUABOSUNGBANAN", null);
            ViewBag.danhSachNguoiKy = _settingDataService.DanhSachNhanVienTheoTenChucNangNghiepVu(ViewText.TITLE_CHUCNANG_NGHIEPVU, anSession.ToaAnId, "");

            if (id == 0)
            {
                var viewModel = new ThongTinSuaChuaBanAnViewModel
                {
                    NgaySuaChua = null,
                    LyDo = null,
                    NguoiKy = null,
                    NoiDung = null,
                    DinhKemFile = null
                };

                return PartialView("SuaChuaBanAn/_SuaChuaBanAnModal", viewModel);
            }
            else
            {
                var thongTinSuaChuaBanAn = _sauXetXuService.ThongTinSuaChuaBanAnTheoId(id);
                var viewModel = new ThongTinSuaChuaBanAnViewModel
                {
                    SuaChuaBanAnId = thongTinSuaChuaBanAn.SuaChuaBanAnId,
                    NgaySuaChua = thongTinSuaChuaBanAn.NgaySuaChua,
                    LyDo = thongTinSuaChuaBanAn.LyDo,
                    NguoiKy = thongTinSuaChuaBanAn.NguoiKy,
                    NgayTao = thongTinSuaChuaBanAn.NgayTao,
                    NguoiTao = thongTinSuaChuaBanAn.NguoiTao,
                    NoiDung = thongTinSuaChuaBanAn.NoiDung,
                    DinhKemFile = thongTinSuaChuaBanAn.DinhKemFile
                };

                return PartialView("SuaChuaBanAn/_SuaChuaBanAnModal", viewModel);
            }
        }

        [HttpPost]
        public ActionResult SuaChuaBanAn(SuaChuaBanAnRequest request)
        {
            try
            {
                var result = _sauXetXuService.SuaChuaBanAn(request.HoSoVuAnId, request.NgaySuaChua, request.LyDo, request.NguoiKy, request.NoiDungSuaChua, request.DinhKemFile);
                if (result)
                {
                    return Json(JsonResponseViewModel.CreateSuccess());
                }

                return Json(JsonResponseViewModel.CreateFail(NotifyMessage.THONGBAO_KHONGTHE_SUACHUABANAN));
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }

        }
        [HttpGet]
        public ActionResult ThongTinSuaBanAnTheoId(int suaChuaBanAnId)
        {
            var thongTinSuaChuaBanAn = _sauXetXuService.ThongTinSuaChuaBanAnTheoId(suaChuaBanAnId);

            var viewModel = new ThongTinSuaChuaBanAnViewModel
            {
                SuaChuaBanAnId = thongTinSuaChuaBanAn.SuaChuaBanAnId,
                NgaySuaChua = thongTinSuaChuaBanAn.NgaySuaChua,
                LyDo = thongTinSuaChuaBanAn.LyDo,
                NguoiKy = thongTinSuaChuaBanAn.NguoiKy,
                NgayTao = thongTinSuaChuaBanAn.NgayTao,
                NguoiTao = thongTinSuaChuaBanAn.NguoiTao,
                NoiDung = thongTinSuaChuaBanAn.NoiDung,
                DinhKemFile = thongTinSuaChuaBanAn.DinhKemFile
            };

            return PartialView("SuaChuaBanAn/_ThongTinSuaChuaBanAn", viewModel);
        }

        //
        [HttpPost]
        public ActionResult UploadFiles()
        {
            string[] allowFileType = { ".pdf", ".doc", ".docx" };

            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase file = files[0];
                    string fname;

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
                        return Json(new { status = "fail", msg = Setting.FILEHOTRO });
                    }

                    if (file.ContentLength / 1024f / 1024f > 5f)
                    {
                        return Json(new { status = "fail", msg = Setting.DUNGLUONGTOIDA });
                    }

                    fname = string.Concat(
                        Path.GetFileNameWithoutExtension(fname),
                        "_",
                        DateTime.Now.ToString("yyyyMMddHHmmss"),
                        Path.GetExtension(fname)
                    );

                    // Get the complete folder path and store the file inside it.  
                    file.SaveAs(Path.Combine(Server.MapPath("~/Uploads/"), fname));

                    // Returns message that successfully uploaded  
                    return Json(new { status = "success", fileName = fname });
                }
                catch (Exception ex)
                {
                    return Json(new { status = "fail", msg = Setting.THONGBAOLOI + ex.Message });
                }
            }
            else
            {
                return Json(new { status = "fail", msg = Setting.THONGBAO_KHONGFILE });
            }
        }
        #endregion

        #region KhangNghi
        [HttpGet]
        public ActionResult KhangNghi(int hoSoVuAnId)
        {
            var danhSachNgayKhangNghi = _sauXetXuService.DanhSachNgayKhangNghi(hoSoVuAnId, GetAnSessionInfo().GiaiDoanId).ToList();
            if (danhSachNgayKhangNghi.Any())
            {
                var khangNghiId = danhSachNgayKhangNghi.First().KhangNghiId;
                var thongTinKhangNghi = _sauXetXuService.ThongTinKhangNghiTheoId(khangNghiId);

                var selectListItem = danhSachNgayKhangNghi.Select(s => new SelectListItem
                {
                    Selected = s.KhangNghiId == khangNghiId,
                    Text = s.NgayToaAnNhanVanBan,
                    Value = s.KhangNghiId.ToString()
                });

                var viewModel = new KhangNghiViewModel
                {
                    DanhSachNgayKhangNghi = new SelectList(selectListItem, "Value", "Text"),
                    ThongTinKhangNghi = new ThongTinKhangNghiViewModel
                    {
                        HoSoVuAnId = thongTinKhangNghi.HoSoVuAnID,
                        KhangNghiId = thongTinKhangNghi.KhangNghiId,
                        NgayToaAnNhanVanBan = thongTinKhangNghi.NgayToaAnNhanVanBan,
                        VienKiemSatKhangNghi = thongTinKhangNghi.VienKiemSatKhangNghi,
                        VanBanKhangNghi = thongTinKhangNghi.VanBanKhangNghi,
                        HinhThuc = thongTinKhangNghi.HinhThuc,
                        NoiDungKhangNghi = thongTinKhangNghi.NoiDungKhangNghi,
                        NgayTao = thongTinKhangNghi.NgayTao,
                        NguoiTao = thongTinKhangNghi.NguoiTao,
                        DanhSachNguoiBiKhangNghi = thongTinKhangNghi.DanhSachNguoiBiKhangNghi
                    }
                };

                return PartialView("KhangNghi/_KhangNghi", viewModel);
            }
            ViewBag.HoSoVuAnID = hoSoVuAnId;
            return PartialView("KhangNghi/_KhangNghi");
        }

        [CustomAuthorize]
        [HttpGet]
        public ActionResult KhangNghiModal(int id, int hoSoVuAnId)
        {
            var anSession = GetAnSessionInfo();
            ViewBag.danhSachVienKiemSatKhangNghi = DanhSachVienKiemSatTheoToaAn(anSession.ToaAnId);
            ViewBag.danhSachHinhThuc = _settingDataService.SettingDataItem("DS_SOTHAM_HINHTHUCNHANDONKHANGNGHI", "");
            if (id != 0)
            {
                ViewBag.danhSachVienKiemSatKhangNghi = DanhSachVienKiemSatTheoToaAn(anSession.ToaAnId);
                ViewBag.danhSachHinhThuc = _settingDataService.SettingDataItem("DS_SOTHAM_HINHTHUCNHANDONKHANGNGHI", "");
                var thongTinKhangNghi = _sauXetXuService.ThongTinKhangNghiTheoId(id);
                var viewModel = new ThongTinKhangNghiViewModel
                {
                    KhangNghiId = thongTinKhangNghi.KhangNghiId,
                    NgayToaAnNhanVanBan = thongTinKhangNghi.NgayToaAnNhanVanBan,
                    VienKiemSatKhangNghi = thongTinKhangNghi.VienKiemSatKhangNghi,
                    VanBanKhangNghi = thongTinKhangNghi.VanBanKhangNghi,
                    HinhThuc = thongTinKhangNghi.HinhThuc,
                    NoiDungKhangNghi = thongTinKhangNghi.NoiDungKhangNghi,
                    NgayTao = thongTinKhangNghi.NgayTao,
                    NguoiTao = thongTinKhangNghi.NguoiTao,
                    DanhSachNguoiBiKhangNghi = thongTinKhangNghi.DanhSachNguoiBiKhangNghi
                };
                ViewBag.listDuongSuBiKhangNghi = _sauXetXuService.DanhSachBiCaoSelected(thongTinKhangNghi.HoSoVuAnID, viewModel.DanhSachNguoiBiKhangNghi);
                if (anSession.MaNhomAn == Setting.MANHOMAN_HINHSU)
                {
                    return PartialView("KhangNghi/HinhSu/_KhangNghiHinhSuModal", viewModel);
                }
                return PartialView("KhangNghi/_KhangNghiModal", viewModel);
            }
            else
            {
                var viewModel = new ThongTinKhangNghiViewModel
                {
                    NgayToaAnNhanVanBan = null,
                    VienKiemSatKhangNghi = null,
                    VanBanKhangNghi = null,
                    HinhThuc = null,
                    NoiDungKhangNghi = null,
                };
                ViewBag.listDuongSuBiKhangNghi = _sauXetXuService.DanhSachBiCaoSelected(hoSoVuAnId, viewModel.DanhSachNguoiBiKhangNghi);

                if (anSession.MaNhomAn == Setting.MANHOMAN_HINHSU)
                {
                    return PartialView("KhangNghi/HinhSu/_KhangNghiHinhSuModal", viewModel);
                }
                return PartialView("KhangNghi/_KhangNghiModal", viewModel);
            }
        }
        [HttpPost]
        public ActionResult KhangNghi(KhangNghiRequest request)
        {
            try
            {
                var result = _sauXetXuService.KhangNghi(request.HoSoVuAnId, request.NgayToaAnNhanVanBan, request.VienKiemSatKhangNghi, request.VanBanKhangNghi, request.HinhThuc, request.NoiDungKhangNghi, request.DanhSachNguoiBiKhangNghi);
                if (result)
                {
                    return Json(JsonResponseViewModel.CreateSuccess());
                }

                return Json(JsonResponseViewModel.CreateFail(NotifyMessage.THONGBAO_KHONGTHE_KHANGNGHI));
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }

        [HttpGet]
        public ActionResult ThongTinKhangNghiTheoId(int khangNghiId)
        {
            var thongTinKhangNghi = _sauXetXuService.ThongTinKhangNghiTheoId(khangNghiId);

            var viewModel = new ThongTinKhangNghiViewModel
            {
                HoSoVuAnId = thongTinKhangNghi.HoSoVuAnID,
                KhangNghiId = thongTinKhangNghi.KhangNghiId,
                NgayToaAnNhanVanBan = thongTinKhangNghi.NgayToaAnNhanVanBan,
                VienKiemSatKhangNghi = thongTinKhangNghi.VienKiemSatKhangNghi,
                VanBanKhangNghi = thongTinKhangNghi.VanBanKhangNghi,
                HinhThuc = thongTinKhangNghi.HinhThuc,
                NoiDungKhangNghi = thongTinKhangNghi.NoiDungKhangNghi,
                DanhSachNguoiBiKhangNghi = thongTinKhangNghi.DanhSachNguoiBiKhangNghi,
                NgayTao = thongTinKhangNghi.NgayTao,
                NguoiTao = thongTinKhangNghi.NguoiTao
            };

            return PartialView("KhangNghi/_ThongTinKhangNghi", viewModel);
        }

        private SelectList DanhSachVienKiemSatTheoToaAn(int toanAnId)
        {
            var listVienKiemSat = _settingDataService.SettingDataItemHaveValueText("DS_SOTHAM_VIENKIEMSATKHANGNGHI", "");

            var selectListItem = listVienKiemSat
                .Where(s => s.Value == toanAnId.ToString() || s.Value == ToaAn.TinhCaMau.GetHashCode().ToString())
                .Select(s => new SelectListItem
                {
                    Text = s.Text,
                    Value = s.Text
                });

            return new SelectList(selectListItem, "Value", "Text");
        }
        #endregion

        #region Khang Cao / Khang Nghi Qua Han
        [HttpGet]
        public ActionResult KhangCaoQuaHan(int hoSoVuAnId)
        {
            var danhSachNgayKhangCaoQuaHan = _sauXetXuService.DanhSachNgayKhangCaoQuaHan(hoSoVuAnId, GetAnSessionInfo().GiaiDoanId).ToList();
            if (danhSachNgayKhangCaoQuaHan.Any())
            {
                var khangCaoQuaHanId = danhSachNgayKhangCaoQuaHan.First().KhangCaoQuaHanId;
                var thongTinKhangCaoQuaHan = _sauXetXuService.ThongTinKhangCaoQuaHanTheoId(khangCaoQuaHanId);

                var selectListItem = danhSachNgayKhangCaoQuaHan.Select(s => new SelectListItem
                {
                    Selected = s.KhangCaoQuaHanId == khangCaoQuaHanId,
                    Text = s.NgayTao,
                    Value = s.KhangCaoQuaHanId.ToString()
                });

                var viewModel = new KhangCaoQuaHanViewModel
                {
                    DanhSachNgayKhangCaoQuaHan = new SelectList(selectListItem, "Value", "Text"),
                    ThongTinKhangCaoQuaHan = new ThongTinKhangCaoQuaHanViewModel
                    {
                        KhangCaoQuaHanId = thongTinKhangCaoQuaHan.KhangCaoQuaHanId,
                        KhangCaoHayKhangNghi = thongTinKhangCaoQuaHan.KhangCaoHayKhangNghi,
                        LyDo = thongTinKhangCaoQuaHan.LyDo,
                        KetQua = thongTinKhangCaoQuaHan.KetQua
                    }
                };
                return PartialView("KhangCaoQuaHan/_KhangCaoQuaHan", viewModel);
            }

            return PartialView("KhangCaoQuaHan/_KhangCaoQuaHan");
        }

        [CustomAuthorize]
        [HttpGet]
        public ActionResult KhangCaoQuaHanModal(int id)
        {
            var viewModel = new ThongTinKhangCaoQuaHanViewModel();
            if (id != 0)
            {
                var thongTinKhangCaoQuaHan = _sauXetXuService.ThongTinKhangCaoQuaHanTheoId(id);
                viewModel = new ThongTinKhangCaoQuaHanViewModel
                {
                    KhangCaoQuaHanId = thongTinKhangCaoQuaHan.KhangCaoQuaHanId,
                    KhangCaoHayKhangNghi = thongTinKhangCaoQuaHan.KhangCaoHayKhangNghi,
                    LyDo = thongTinKhangCaoQuaHan.LyDo,
                    KetQua = thongTinKhangCaoQuaHan.KetQua
                };
            }
            else
            {
                viewModel = new ThongTinKhangCaoQuaHanViewModel
                {
                    KhangCaoHayKhangNghi = ViewText.TABLE_KHANGCAO, //default
                    LyDo = null,
                    KetQua = null
                };
            }

            ViewBag.KhangCaoHayKhangNghi = _sauXetXuService.KhangCaoHayKhangNghi(viewModel.KhangCaoHayKhangNghi);

            return PartialView("KhangCaoQuaHan/_KhangCaoQuaHanModal", viewModel);
        }

        [HttpPost]
        public ActionResult KhangCaoQuaHan(KhangCaoQuaHanRequest request)
        {
            try
            {
                var result = _sauXetXuService.KhangCaoQuaHan(request.HoSoVuAnId, request.KhangCaoHayKhangNghi, request.LyDo, request.KetQua);
                if (result)
                {
                    return Json(JsonResponseViewModel.CreateSuccess());
                }

                return Json(JsonResponseViewModel.CreateFail("Không thể tạo kháng cáo quá hạn."));
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }

        [HttpGet]
        public ActionResult ThongTinKhangCaoQuaHanTheoId(int khangCaoQuaHanId)
        {
            var thongTinKhangCaoQuaHan = _sauXetXuService.ThongTinKhangCaoQuaHanTheoId(khangCaoQuaHanId);

            var viewModel = new ThongTinKhangCaoQuaHanViewModel
            {
                KhangCaoQuaHanId = thongTinKhangCaoQuaHan.KhangCaoQuaHanId,
                KhangCaoHayKhangNghi = thongTinKhangCaoQuaHan.KhangCaoHayKhangNghi,
                LyDo = thongTinKhangCaoQuaHan.LyDo,
                KetQua = thongTinKhangCaoQuaHan.KetQua
            };

            return PartialView("KhangCaoQuaHan/_ThongTinKhangCaoQuaHan", viewModel);
        }
        #endregion

        #region LuuKho
        [HttpGet]
        public ActionResult LuuKho(int hoSoVuAnId)
        {
            var danhSachNgayLuuKho = _sauXetXuService.DanhSachNgayLuuKho(hoSoVuAnId, GetAnSessionInfo().GiaiDoanId).ToList();
            if (danhSachNgayLuuKho.Any())
            {
                var luuKhoId = danhSachNgayLuuKho.First().LuuKhoId;
                var thongTinLuuKho = _sauXetXuService.ThongTinLuuKhoTheoId(luuKhoId);

                var selectListItem = danhSachNgayLuuKho.Select(s => new SelectListItem
                {
                    Selected = s.LuuKhoId == luuKhoId,
                    Text = s.NgayTao,
                    Value = s.LuuKhoId.ToString()
                });

                var viewModel = new LuuKhoViewModel
                {
                    DanhSachNgayLuuKho = new SelectList(selectListItem, "Value", "Text"),
                    ThongTinLuuKho = new ThongTinLuuKhoViewModel
                    {
                        LuuKhoId = thongTinLuuKho.LuuKhoId,
                        NgayLuu = thongTinLuuKho.NgayLuu,
                        NguoiGiao = thongTinLuuKho.NguoiGiao,
                        NguoiNhanLuu = thongTinLuuKho.NguoiNhanLuu,
                        GhiChuTinhTrangHoSo = thongTinLuuKho.GhiChuTinhTrangHoSo,
                        GhiChu = thongTinLuuKho.GhiChu,
                        NguoiTao = thongTinLuuKho.NguoiTao,
                        NgayTao = thongTinLuuKho.NgayTao
                    }
                };

                return PartialView("LuuKho/_LuuKho", viewModel);
            }

            return PartialView("LuuKho/_LuuKho");
        }

        [CustomAuthorize]
        [HttpGet]
        public ActionResult LuuKhoModal(int id)
        {
            var anSession = GetAnSessionInfo();

            if (id == 0)
            {
                var viewModel = new ThongTinLuuKhoViewModel
                {
                    NgayLuu = null,
                    NguoiGiao = null,
                    NguoiNhanLuu = null,
                    GhiChuTinhTrangHoSo = null,
                    GhiChu = null

                };
                ViewBag.danhSachNguoiGiao = _settingDataService.DanhSachNhanVienTheoTenChucNangNghiepVu("Giao Lưu Kho", anSession.ToaAnId, "");
                ViewBag.danhSachNguoiNhanLuu = _settingDataService.DanhSachNhanVienTheoTenChucNangNghiepVu("Nhận Lưu Kho", anSession.ToaAnId, "");
                return PartialView("LuuKho/_LuuKhoModal", viewModel);
            }
            else
            {
                var thongTinLuuKho = _sauXetXuService.ThongTinLuuKhoTheoId(id);

                var viewModel = new ThongTinLuuKhoViewModel
                {
                    LuuKhoId = thongTinLuuKho.LuuKhoId,
                    NgayLuu = thongTinLuuKho.NgayLuu,
                    NguoiGiao = thongTinLuuKho.NguoiGiao,
                    NguoiNhanLuu = thongTinLuuKho.NguoiNhanLuu,
                    GhiChuTinhTrangHoSo = thongTinLuuKho.GhiChuTinhTrangHoSo,
                    GhiChu = thongTinLuuKho.GhiChu,
                    NguoiTao = thongTinLuuKho.NguoiTao,
                    NgayTao = thongTinLuuKho.NgayTao
                };
                ViewBag.danhSachNguoiGiao = _settingDataService.DanhSachNhanVienTheoTenChucNangNghiepVu("Giao Lưu Kho", anSession.ToaAnId, "");
                ViewBag.danhSachNguoiNhanLuu = _settingDataService.DanhSachNhanVienTheoTenChucNangNghiepVu("Nhận Lưu Kho", anSession.ToaAnId, "");

                return PartialView("LuuKho/_LuuKhoModal", viewModel);
            }
        }

        [HttpPost]
        public ActionResult LuuKho(LuuKhoRequest request)
        {
            try
            {
                var model = new LuuKhoDataRequest();
                model.HoSoVuAnId = request.HoSoVuAnId;
                model.GhiChu = request.GhiChu;
                model.GhiChuTinhTrangHoSo = request.GhiChuTinhTrangHoSo;
                model.CongDoanHoSo = (int)Enums.CongDoan.SauXetXu;
                model.GiaiDoan = GetAnSessionInfo().GiaiDoanId;
                model.NgayLuu = request.NgayLuu;
                model.NgayTao = DateTime.Now;
                model.NguoiGiao = request.NguoiGiao;
                model.NguoiNhanLuu = request.NguoiNhanLuu;
                model.NguoiTao = _settingDataService.GetNhanVienSessionInfo().MaNV;
                var result = _sauXetXuService.LuuKho(model);
                if (result)
                {
                    return Json(JsonResponseViewModel.CreateSuccess());
                }

                return Json(JsonResponseViewModel.CreateFail(NotifyMessage.THONGBAO_KHONGTHE_LUUKHO));
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }

        [HttpGet]
        public ActionResult ThongTinLuuKhoTheoId(int luuKhoId)
        {
            var thongTinLuuKho = _sauXetXuService.ThongTinLuuKhoTheoId(luuKhoId);

            var viewModel = new ThongTinLuuKhoViewModel
            {
                LuuKhoId = thongTinLuuKho.LuuKhoId,
                NgayLuu = thongTinLuuKho.NgayLuu,
                NguoiGiao = thongTinLuuKho.NguoiGiao,
                NguoiNhanLuu = thongTinLuuKho.NguoiNhanLuu,
                GhiChuTinhTrangHoSo = thongTinLuuKho.GhiChuTinhTrangHoSo,
                GhiChu = thongTinLuuKho.GhiChu,
                NguoiTao = thongTinLuuKho.NguoiTao,
                NgayTao = thongTinLuuKho.NgayTao
            };

            return PartialView("LuuKho/_ThongTinLuuKho", viewModel);
        }
        #endregion

        #region KhangCao

        public ActionResult KhangCao(int hoSoVuAnId)
        {
            var viewModel = _sauXetXuService.DanhSachKhangCao(hoSoVuAnId, GetAnSessionInfo().GiaiDoanId);
            return Json(new { data = viewModel }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ThongTinKhangCaoModal(int khangCaoId)
        {
            var anSession = GetAnSessionInfo();
            var model = _sauXetXuService.ThongTinKhangCaoTheoId(khangCaoId);
            var viewModel = new ThongTinKhangCaoViewModel
            {
                KhangCaoId = model.KhangCaoId,
                NguoiKhangCao = model.NguoiKhangCao,
                TuCachThamGiaToTung = model.TuCachThamGiaToTung,
                NgayLamDon = model.NgayLamDon,
                NgayNopDon = model.NgayNopDon,
                HinhThucNopDon = model.HinhThucNopDon,
                TinhTrangKhangCao = model.TinhTrangKhangCao,
                NoiDungKhangCao = model.NoiDungKhangCao,
                TaiLieuChungTuKemTheo = model.TaiLieuChungTuKemTheo,
                NguoiNhanKhieuNai = model.NguoiNhanKhieuNai,
                NguoiKhieuNai = model.NguoiKhieuNai,
                NguoiBiKhangCao = model.NguoiBiKhangCao,
                DanhSachNguoiBiKhangCao = model.DanhSachNguoiBiKhangCao,
                LyDo = model.LyDo,
                NguoiTao = model.NguoiTao,
                NgayTao = model.NgayTao,
                GhiChu = model.GhiChu
            };
            ViewBag.NhomAn = anSession.MaNhomAn;

            if (anSession.MaNhomAn == Setting.MANHOMAN_HINHSU)
            {
                return PartialView("KhangCao/HinhSu/_ThongTinKhangCaoHinhSuModal", viewModel);
            }
            return PartialView("KhangCao/_ThongTinKhangCaoModal", viewModel);
        }

        [CustomAuthorize]
        public ActionResult TaoKhangCaoModal(int hoSoVuAnId)
        {
            var anSession = GetAnSessionInfo();
            var viewModel = new KhangCaoModalViewModel
            {
                DanhSachHinhThucNop = DanhSachHinhThucNop(),
                DanhSachTinhTrangKhangCao = DanhSachTinhTrangKhangCao(),
                DanhSachDuongSu = DanhSachDuongSuHinhSuTheoHoSoVuAn(hoSoVuAnId),
                DanhSachDuongSuBiKhangCao = DanhSachBiCao(hoSoVuAnId)
                //DanhSachNguoiKhieuNai = DanhSachNguoiKhieuNai(),
            };
            ViewBag.NhomAn = anSession.MaNhomAn;

            if (anSession.MaNhomAn == Setting.MANHOMAN_HINHSU)
            {
                return PartialView("KhangCao/HinhSu/_TaoKhangCaoHinhSuModal", viewModel);
            }
            return PartialView("KhangCao/_TaoKhangCaoModal", viewModel);
        }

        [HttpPost]
        public ActionResult TaoKhangCao(TaoKhangCaoRequest request)
        {
            try
            {
                var result = _sauXetXuService.KhangCao(request.HoSoVuAnId, request.NguoiKhangCao, request.NgayLamDon, request.NgayNopDon, request.HinhThucNop, request.TinhTrangKhangCao, request.NoiDungKhangCao, request.TaiLieuChungTuKemTheo, request.NguoiKhieuNai, request.LyDo, request.GhiChu, request.DanhSachNguoiBiKhangCao);
                if (result)
                {
                    return Json(JsonResponseViewModel.CreateSuccess());
                }

                return Json(JsonResponseViewModel.CreateFail(NotifyMessage.THONGBAO_KHONGTHE_KHANGCAO));
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }

        [CustomAuthorize]
        public ActionResult SuaKhangCaoModal(int khangCaoId)
        {
            var anSession = GetAnSessionInfo();
            var khangCao = _sauXetXuService.ThongTinKhangCaoTheoId(khangCaoId);
            var viewModel = new SuaKhangCaoModalViewModel
            {

                KhangCaoId = khangCao.KhangCaoId,
                DuongSuId = khangCao.DuongSuId,
                NgayLamDon = khangCao.NgayLamDon,
                NgayNopDon = khangCao.NgayNopDon,
                HinhThucNop = khangCao.HinhThucNopDon,
                TinhTrangKhangCao = khangCao.TinhTrangKhangCao,
                NoiDungKhangCao = khangCao.NoiDungKhangCao,
                NguoiNhanKhieuNai = khangCao.NguoiNhanKhieuNai,
                NguoiKhieuNai = khangCao.NguoiKhieuNai,
                LyDo = khangCao.LyDo,
                GhiChu = khangCao.GhiChu,
                TaiLieuChungTuKemTheo = khangCao.TaiLieuChungTuKemTheo,
                //DanhSachNguoiKhieuNai = DanhSachNguoiKhieuNai(),
                DanhSachHinhThucNop = DanhSachHinhThucNop(),
                DanhSachTinhTrangKhangCao = DanhSachTinhTrangKhangCao(),
                DanhSachDuongSu = DanhSachDuongSuHinhSuTheoHoSoVuAn(khangCao.HoSoVuAnId),

                NguoiBiKhangCao = khangCao.NguoiBiKhangCao,
                DanhSachNguoiBiKhangCao = khangCao.DanhSachNguoiBiKhangCao,
            };
            ViewBag.listDuongSuBiKhangCao = _sauXetXuService.DanhSachBiCaoSelected(khangCao.HoSoVuAnId, viewModel.DanhSachNguoiBiKhangCao);
            ViewBag.NhomAn = anSession.MaNhomAn;

            if (anSession.MaNhomAn == Setting.MANHOMAN_HINHSU)
            {
                return PartialView("KhangCao/HinhSu/_SuaKhangCaoHinhSuModal", viewModel);
            }
            return PartialView("KhangCao/_SuaKhangCaoModal", viewModel);
        }

        [HttpPost]
        public ActionResult SuaKhangCao(SuaKhangCaoRequest request)
        {
            try
            {
                var result = _sauXetXuService.SuaKhangCao(request.KhangCaoId, request.NguoiKhangCao, request.NgayLamDon, request.NgayNopDon, request.HinhThucNop, request.TinhTrangKhangCao, request.NoiDungKhangCao, request.TaiLieuChungTuKemTheo, request.NguoiKhieuNai, request.LyDo, request.GhiChu, request.DanhSachNguoiBiKhangCao);
                if (result)
                {
                    return Json(JsonResponseViewModel.CreateSuccess());
                }

                return Json(JsonResponseViewModel.CreateFail(NotifyMessage.THONGBAO_KHONGTHESUA_KHANGCAO));
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }

        [CustomAuthorize]
        public ActionResult XoaKhangCaoModal(int khangCaoId)
        {
            return PartialView("KhangCao/_XoaKhangCaoModal", khangCaoId);
        }

        [HttpPost]
        public ActionResult XoaKhangCao(int khangCaoId)
        {
            try
            {
                var result = _sauXetXuService.XoaKhangCao(khangCaoId);
                if (result)
                {
                    return Json(JsonResponseViewModel.CreateSuccess());
                }

                return Json(JsonResponseViewModel.CreateFail(NotifyMessage.THONGBAO_KHONGTHEXOA_KHANGCAO));
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }

        private SelectList DanhSachHinhThucNop()
        {
            return _settingDataService.SettingDataItem("DINHNGHIACHUNG_HINHTHUCGOIDON", "");
        }

        private SelectList DanhSachTinhTrangKhangCao()
        {
            return _settingDataService.SettingDataItem("DS_SOTHAM_TINHTRANGKHANGCAO", "");
        }

        private SelectList DanhSachDuongSuTheoHoSoVuAn(int hoSoVuAnId)
        {
            var danhSachDuongSuTheoHoSo = _sauXetXuService.DanhSachDuongSuTheoHoSo(hoSoVuAnId);

            var selectListItem = danhSachDuongSuTheoHoSo.Select(s => new SelectListItem
            {
                Text = s.HoVaTen,
                Value = s.DuongSuId.ToString()
            });

            return new SelectList(selectListItem, "Value", "Text");
        }

        private SelectList DanhSachDuongSuHinhSuTheoHoSoVuAn(int hoSoVuAnId)
        {
            var danhSachDuongSuTheoHoSo = _sauXetXuService.DanhSachDuongSuKhacBiCaoTheoHoSo(hoSoVuAnId).ToList();

            var selectListItem = danhSachDuongSuTheoHoSo.Select(s => new SelectListItem
            {
                Text = s.HoVaTen,
                Value = s.DuongSuId.ToString()
            });

            return new SelectList(selectListItem, "Value", "Text");
        }

        private SelectList DanhSachBiCao(int hoSoVuAnId)
        {
            var danhSachDuongSuBiKhangCao = _sauXetXuService.DanhSachBiCao(hoSoVuAnId);

            var selectListItem = danhSachDuongSuBiKhangCao.Select(s => new SelectListItem
            {
                Text = s.HoVaTen,
                Value = s.HoVaTen
            });

            return new SelectList(selectListItem, "Value", "Text");
        }

        //private SelectList DanhSachNguoiKhieuNai()
        //{
        //    return _settingDataService.SettingDataItem("AD_SOTHAM_NGUOIKHIEUNAI", "");
        //}
        #endregion

        #region TamUngAnPhi

        public ActionResult TamUngAnPhi(int hoSoVuAnId)
        {
            var giaiDoan = GetAnSessionInfo().GiaiDoanId;
            var listNgayTao = _sauXetXuService.DanhSachNgaySuaDoiTamUngAnPhi(hoSoVuAnId, giaiDoan, CongDoan.SauXetXu.GetHashCode(), 0);
            if (listNgayTao.Any())
            {
                var anPhiId = Convert.ToInt32(listNgayTao.First().Value);
                var viewModel = _sauXetXuService.ChiTietTamUngAnPhiTheoId(hoSoVuAnId, anPhiId, giaiDoan, CongDoan.SauXetXu.GetHashCode());
                ViewBag.listNgayTao = listNgayTao;

                return PartialView("TamUngAnPhi/_ThongTinTamUngAnPhi", viewModel);
            }
            ViewBag.hoSoVuAnId = hoSoVuAnId;
            return PartialView("TamUngAnPhi/_ThongTinTamUngAnPhi");
        }

        public ActionResult ChiTietTamUngAnPhiTheoId(int hoSoVuAnId, int anPhiId)
        {
            var giaiDoan = GetAnSessionInfo().GiaiDoanId;
            var viewModel = _sauXetXuService.ChiTietTamUngAnPhiTheoId(hoSoVuAnId, anPhiId, giaiDoan, CongDoan.SauXetXu.GetHashCode());
            if (viewModel != null)
            {
                ViewBag.listNgayTao = _sauXetXuService.DanhSachNgaySuaDoiTamUngAnPhi(hoSoVuAnId, giaiDoan, CongDoan.SauXetXu.GetHashCode(), viewModel.AnPhiID);
                return PartialView("TamUngAnPhi/_ThongTinTamUngAnPhi", viewModel);
            }
            return PartialView("TamUngAnPhi/_ThongTinTamUngAnPhi");
        }

        [CustomAuthorize]
        public ActionResult TamUngAnPhiModal(int hoSoVuAnId, int anPhiId)
        {
            var anSession = GetAnSessionInfo();
            var viewModel = _sauXetXuService.ChiTietTamUngAnPhiTheoId(hoSoVuAnId, anPhiId, GetAnSessionInfo().GiaiDoanId, CongDoan.SauXetXu.GetHashCode());
            if (viewModel == null)
            {
                var yeuCauDuNopModel = new YeuCauDuNopAnPhiViewModel()
                {
                    TongAnPhi = Setting.DS_TAMUNGANPHI_TONGANPHI,
                    PhanTramDuNop = Setting.DS_TAMUNGANPHI_PHANTRAMDUNOP,
                    GiaTriDuNop = Setting.DS_TAMUNGANPHI_GIATRIDUNOP
                };
                var ketQuaNopModel = new KetQuaNopAnPhiViewModel();
                viewModel = new TamUngAnPhiViewModel
                {
                    HoSoVuAnID = hoSoVuAnId,
                    YeuCauDuNopViewModel = yeuCauDuNopModel,
                    KetQuaNopViewModel = ketQuaNopModel
                };
            }
            if (GetAnSessionInfo().MaNhomAn == Setting.MANHOMAN_KINHTE)
            {
                viewModel.YeuCauDuNopViewModel.TongAnPhi = Setting.KT_TAMUNGANPHI_TONGANPHI;
                viewModel.YeuCauDuNopViewModel.PhanTramDuNop = Setting.KT_TAMUNGANPHI_PHANTRAMDUNOP;
                viewModel.YeuCauDuNopViewModel.GiaTriDuNop = Setting.KT_TAMUNGANPHI_GIATRIDUNOP;
            }
            else
            {
                viewModel.YeuCauDuNopViewModel.TongAnPhi = Setting.DS_TAMUNGANPHI_TONGANPHI;
                viewModel.YeuCauDuNopViewModel.PhanTramDuNop = Setting.DS_TAMUNGANPHI_PHANTRAMDUNOP;
                viewModel.YeuCauDuNopViewModel.GiaTriDuNop = Setting.DS_TAMUNGANPHI_GIATRIDUNOP;
            }

            viewModel.KetQuaNopViewModel.NhanVienNguoiNhanBienLai =
                _settingDataService.ChiTietNhanVienTheoMaNhanVien(AccountUtils.CurrentUsername());

            ViewBag.listNopAnPhi = _settingDataService.SettingDataItem("DS_SOTHAM_THULY_ANPHILEPHI", "");
            ViewBag.listCoQuanThiHanhAnThu = DanhSachCoQuanThiHanhAnThuTheoToaAn(anSession.ToaAnId);
            ViewBag.listDiaChiCoQuanThiHanhAnThu = _settingDataService.DanhSachDiaChiCoQuanThiHanhAn("DINHNGHIACHUNG_COQUANTHIHANHANTHU", "");
            ViewBag.listNguoiNhanBienLai = _settingDataService.DanhSachNhanVienTheoTenChucNangNghiepVu("Nhận Biên Lai", anSession.ToaAnId, null);

            return PartialView("TamUngAnPhi/_TamUngAnPhiModal", viewModel);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult SuaYeuCauDuNopTamUngAnPhi(TamUngAnPhiViewModel viewModel)
        {
            try
            {
                ResponseResult result = null;
                //sua yeu cau nop an phi, le phi
                if (viewModel.KetQuaNopViewModel == null)
                {
                    result = _sauXetXuService.SuaYeuCauDuNopTamUngAnPhi(viewModel);
                    if (result != null && result.ResponseCode == 1)
                    {
                        return Json(JsonResponseViewModel.CreateSuccess());
                    }
                    return Json(JsonResponseViewModel.CreateFail(NotifyMessage.THONGBAO_KHONGTHESUA_NOPTAMUNGANPHI_LEPHI));
                }

                //sua ket qua nop an phi, le phi
                viewModel.KetQuaNopViewModel.NguoiNhanBienLai = _settingDataService.GetNhanVienSessionInfo().MaNV;
                result = _sauXetXuService.SuaKetQuaDuNopTamUngAnPhi(viewModel);
                if (result != null && result.ResponseCode == 1)
                {
                    return Json(JsonResponseViewModel.CreateSuccess());
                }
                return Json(JsonResponseViewModel.CreateFail(NotifyMessage.THONGBAO_KHONGTHESUA_KETQUA_NOPTAMUNGANPHI_LEPHI));
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult SuaMienDuNopTamUngAnPhi(TamUngAnPhiViewModel viewModel)
        {
            try
            {
                ResponseResult result = _sauXetXuService.SuaMienDuNopTamUngAnPhi(viewModel);
                if (result != null && result.ResponseCode == 1)
                {
                    return Json(JsonResponseViewModel.CreateSuccess());
                }
                return Json(JsonResponseViewModel.CreateFail(NotifyMessage.THONGBAO_KHONGTHESUA_YEUCAUDUNOP_TAMUNGANPHI_LEPHI));
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }

        private SelectList DanhSachCoQuanThiHanhAnThuTheoToaAn(int toanAnId)
        {
            var listCoQuan = _settingDataService.SettingDataItemHaveValueText("DINHNGHIACHUNG_COQUANTHIHANHANTHU", "");

            var selectListItem = listCoQuan
                .Where(s => s.Value == toanAnId.ToString())
                .Select(s => new SelectListItem
                {
                    Text = s.Text,
                    Value = s.Text
                });

            return new SelectList(selectListItem, "Value", "Text");
        }
        #endregion

        #region ChuyenHoSo

        public ActionResult ChuyenHoSo(int hoSoVuAnId)
        {
            var listNgayTao = _nhanDonService.GetDanhSachNgaySuaDoiChuyenDon(hoSoVuAnId, GetAnSessionInfo().GiaiDoanId, GetAnSessionInfo().CongDoanId, 0);
            if (listNgayTao.Any())
            {
                var chuyenHoSoId = Protect.ToInt32(listNgayTao.First().Value);
                var viewModel = _sauXetXuService.ChiTietChuyenHoSoTheoId(chuyenHoSoId);
                ViewBag.listNgayTao = listNgayTao;

                return PartialView("ChuyenHoSo/_ChuyenHoSo", viewModel);
            }
            ViewBag.hoSoVuAnId = hoSoVuAnId;
            return PartialView("ChuyenHoSo/_ChuyenHoSo");
        }

        public ActionResult GetChuyenHoSoTheoId(int chuyenHoSoId)
        {
            var model = _sauXetXuService.ChiTietChuyenHoSoTheoId(chuyenHoSoId);

            if (model != null)
            {
                ViewBag.listNgayTao = _nhanDonService.GetDanhSachNgaySuaDoiChuyenDon(model.HoSoVuAnID, GetAnSessionInfo().GiaiDoanId, GetAnSessionInfo().CongDoanId, 0);
                return PartialView("ChuyenHoSo/_ChiTietChuyenHoSo", model);
            }
            return PartialView("ChuyenHoSo/_ChiTietChuyenHoSo");
        }

        [CustomAuthorize]
        public ActionResult EditChuyenHoSo(int hoSoVuAnId, int chuyenHoSoId)
        {
            var anSession = GetAnSessionInfo();
            var loaiQuanHe = _settingDataService.GetLoaiQuanHe(hoSoVuAnId);
            QuyetDinhModel kqxxQuyetDinh = new QuyetDinhModel();
            BanAnModel kqxxBanAn = new BanAnModel();
            if (anSession.MaNhomAn != Setting.MANHOMAN_APDUNG_BPXLHC)
            {
                kqxxQuyetDinh = _ketQuaXetXuService.ChiTietQuyetDinhTheoHoSoVuAnID(hoSoVuAnId, anSession.GiaiDoanId);
                kqxxBanAn = _ketQuaXetXuService.ChiTietBanAnTheoHoSoVuAnID(hoSoVuAnId, anSession.GiaiDoanId);
                int trangThaiDinhKemFile;

                if (kqxxBanAn == null & kqxxQuyetDinh == null)
                {
                    string warning = NotifyMessage.WARNING_DINHKEMFILE_QUYETDINHVABANAN;
                    return PartialView("ChuyenHoSo/_MessageThongBao", warning);
                }
                else if (kqxxQuyetDinh != null)
                {
                    trangThaiDinhKemFile = _sauXetXuService.KiemTraTinhTrangDinhKemFileQuyetDinh(hoSoVuAnId, anSession.GiaiDoanId);
                    if (trangThaiDinhKemFile == 0)
                    {
                        string warning = NotifyMessage.WARNING_DINHKEMFILE_QUYETDINH;
                        return PartialView("ChuyenHoSo/_MessageThongBao", warning);
                    }
                }
                else if ((loaiQuanHe == Setting.LOAIQUANHE_TRANHCHAP && kqxxBanAn != null) || (GetAnSessionInfo().MaNhomAn==Setting.MANHOMAN_HINHSU && kqxxBanAn != null))
                {
                    trangThaiDinhKemFile = _sauXetXuService.KiemTraTinhTrangDinhKemFileBanAn(hoSoVuAnId, anSession.GiaiDoanId);
                    if (trangThaiDinhKemFile == 0)
                    {
                        string warning = NotifyMessage.WARNING_DINHKEMFILE_BANAN;
                        return PartialView("ChuyenHoSo/_MessageThongBao", warning);
                    }
                }
            }

            var viewModel = _sauXetXuService.ChiTietChuyenHoSoTheoId(chuyenHoSoId);
            var toaAnActive = anSession.ToaAnId;
            var listToaAn = _settingDataService.SelectListDanhSachToaAnValueIsToaAnID(null);
            IEnumerable<SelectListItem> ddlToaAn;
            string vungChuyenDon;

            if (viewModel == null)
            {
                viewModel = new ChuyenHoSoViewModel
                {
                    CongDoanHoSo = anSession.CongDoanId,
                    GiaiDoan = anSession.GiaiDoanId,
                    HoSoVuAnID = hoSoVuAnId,
                    ToaAnChuyenDen = ViewText.TOAAN_CHUYENPHUCTHAM
                };
                vungChuyenDon = Setting.VUNG_CHUYENHOSO_NGOAITINH;
                if ((toaAnActive == ToaAn.TinhCaMau.GetHashCode() && anSession.GiaiDoanId == GiaiDoan.PhucTham.GetHashCode()) || toaAnActive != ToaAn.TinhCaMau.GetHashCode())
                {
                    vungChuyenDon = Setting.VUNG_CHUYENHOSO_TRONGTINH;
                }
            }
            else
            {
                vungChuyenDon = Setting.VUNG_CHUYENHOSO_NGOAITINH;
                foreach (var item in listToaAn)
                {
                    if (item.Value != ToaAn.TinhCaMau.GetHashCode().ToString() && item.Text == viewModel.ToaAnChuyenDen)
                    {
                        vungChuyenDon = Setting.VUNG_CHUYENHOSO_TRONGTINH;
                        break;
                    }
                }
            }
            if (anSession.GiaiDoanId == GiaiDoan.PhucTham.GetHashCode())
            {
                var tenToaAnSoTham = _nhanDonService.ChiTietChuyenDonTheoHoSoVuAnID(hoSoVuAnId, GiaiDoan.SoTham.GetHashCode(), CongDoan.SauXetXu.GetHashCode()).ToaAnChuyenDi;
                var toaAnSoTham = _settingDataService.GetToaAnTheoTenToaAn(tenToaAnSoTham);

                if (anSession.MaNhomAn == Setting.MANHOMAN_HINHSU)
                {
                    var lyDoChuyenHoSoHinhSu = _settingDataService.SettingDataItem("HS_PHUCTHAM_LYDOCHUYENHOSO_SAUXETXU", "");
                    ViewBag.listLyDoChuyen = new SelectList(lyDoChuyenHoSoHinhSu.Where(x => x.Text != "Điều tra lại"), "Text", "Text");
                }
                else
                {
                    ViewBag.listLyDoChuyen = _settingDataService.SettingDataItem("DS_PHUCTHAM_LYDOCHUYENHOSO_SAUXETXU", "");
                }
                ddlToaAn = listToaAn.Where(x => x.Value == toaAnSoTham.ToaAnID.ToString());

                if (kqxxBanAn != null)
                {
                    if (kqxxBanAn.KetQuaXetXu == "Hủy bản án sơ thẩm và chuyển hồ sơ vụ án để điều tra lại")
                    {
                        var listVienKiemSat = _settingDataService.SettingDataItemHaveValueText("HS_SOTHAM_VIENKIEMSATTRUYTO", "");
                        ddlToaAn = listVienKiemSat.Where(x => x.Value == toaAnSoTham.ToaAnID.ToString());
                        var lyDoChuyenHoSoHinhSuDieuTraLai = _settingDataService.SettingDataItem("HS_PHUCTHAM_LYDOCHUYENHOSO_SAUXETXU", "");
                        ViewBag.listLyDoChuyen = new SelectList(lyDoChuyenHoSoHinhSuDieuTraLai.Where(x => x.Text == "Điều tra lại"), "Text", "Text");
                    }
                }
            }
            else
            {
                ddlToaAn = listToaAn.Where(x => x.Value == ToaAn.TinhCaMau.GetHashCode().ToString());
                ViewBag.listLyDoChuyen = _settingDataService.SettingDataItem("DS_SOTHAM_LYDOCHUYENHOSO_SAUXETXU", "");
            }

            ViewBag.listVungChuyenDon = _nhanDonService.SelectListVungChuyenDon(vungChuyenDon);
            ViewBag.listToaAnHuyen = new SelectList(ddlToaAn, "Text", "Text");

            return PartialView("ChuyenHoSo/_SuaDoiChuyenHoSo", viewModel);
        }


        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult EditChuyenHoSo(ChuyenHoSoViewModel viewModel)
        {
            try
            {
                var giaiDoan = GetAnSessionInfo().GiaiDoanId;
                var toaAnActive = _settingDataService.GetToaAnTheoToaAnID(GetAnSessionInfo().ToaAnId);
                viewModel.ToaAnChuyenDi = toaAnActive.TenToaAn;
                viewModel.TrangThaiCongDoan = Setting.TRANGTHAICONGDOAN_CHUYENHOSO;

                var result = _sauXetXuService.SuaDoiChuyenHoSo(viewModel);
                if (result != null && result.ResponseCode == 1)
                {
                    var toaAnChuyen = _settingDataService.GetToaAnTheoTenToaAn(viewModel.ToaAnChuyenDi);
                    var toaAnNhan = _settingDataService.GetToaAnTheoTenToaAn(viewModel.ToaAnChuyenDen);
                    var hoSo = _nhanDonService.ChiTietHoSoVuAn(viewModel.HoSoVuAnID);
                    if (toaAnNhan != null)
                    {
                        if (giaiDoan == GiaiDoan.PhucTham.GetHashCode())
                        {
                            try
                            {
                                GuiEmailChuyenHoSoPhucThamVeSoTham(hoSo, viewModel, toaAnChuyen, toaAnNhan);
                            }
                            catch (Exception ex)
                            {
                                return Json(JsonResponseViewModel.CreateFail(string.Format(NotifyMessage.THONGBAO_KHONGTHANHCONG, ViewText.LABEL_GUI_EMAIL)));
                            }
                        }
                        else
                        {
                            try
                            {
                                GuiEmailChuyenHoSoPhucTham(hoSo, viewModel, toaAnChuyen, toaAnNhan);
                            }
                            catch (Exception ex)
                            {
                                return Json(JsonResponseViewModel.CreateFail(string.Format(NotifyMessage.THONGBAO_KHONGTHANHCONG, ViewText.LABEL_GUI_EMAIL)));
                            }
                        }
                    }

                    return Json(JsonResponseViewModel.CreateSuccess(string.Format(NotifyMessage.THONGBAO_THANHCONG, ViewText.LABEL_CHUYENHOSO)));
                }
                return Json(JsonResponseViewModel.CreateFail(string.Format(NotifyMessage.THONGBAO_KHONGTHANHCONG, ViewText.LABEL_CHUYENHOSO)));
            }
            catch (Exception ex)
            {
                return Json(JsonResponseViewModel.CreateFail(ex));
            }
        }

        public void GuiEmailChuyenHoSoPhucThamVeSoTham(HoSoVuAnModel hoSoModel, ChuyenHoSoViewModel chuyenHoSoModel, ToaAnModel toaAnChuyen, ToaAnModel toaAnNhan)
        {
            var nv = _settingDataService.GetNhanVienSessionInfo();
            string mailBody = EmailTemplate.CHUYENHOSOPHUCTHAM_VE_SOTHAM_EMAIL_BODY;
            mailBody = mailBody.Replace("{_ToaAnNhan_}", toaAnNhan.TenToaAn);
            mailBody = mailBody.Replace("{_ToaAnChuyen_}", toaAnChuyen.TenToaAn);
            mailBody = mailBody.Replace("{_MaHoSo_}", hoSoModel.MaHoSo);
            mailBody = mailBody.Replace("{_NgayChuyen_}", chuyenHoSoModel.NgayChuyenHoSo);
            mailBody = mailBody.Replace("{_NguoiChuyen_}", nv.HoVaTen + " (" + nv.MaNVMoi + ")");
            mailBody = mailBody.Replace("{_TruongHopChuyen_}", chuyenHoSoModel.LyDoChuyenHoSo);
            mailBody = mailBody.Replace("{_GhiChu_}", chuyenHoSoModel.GhiChu);
            mailBody = mailBody.Replace("{_EmailToaAnChuyen_}", toaAnChuyen.Email);
            Commons.SendMail(EmailTemplate.CHUYENHOSOPHUCTHAM_VE_SOTHAM_EMAIL_SUBJECT, toaAnNhan.Email, mailBody, cc: toaAnChuyen.Email);
        }

        public void GuiEmailChuyenHoSoPhucTham(HoSoVuAnModel hoSoModel, ChuyenHoSoViewModel chuyenHoSoModel, ToaAnModel toaAnChuyen, ToaAnModel toaAnNhan)
        {
            var nv = _settingDataService.GetNhanVienSessionInfo();
            string mailBody = EmailTemplate.CHUYENHOSOPHUCTHAM_EMAIL_BODY;
            mailBody = mailBody.Replace("{_ToaAnNhan_}", toaAnNhan.TenToaAn);
            mailBody = mailBody.Replace("{_ToaAnChuyen_}", toaAnChuyen.TenToaAn);
            mailBody = mailBody.Replace("{_MaHoSo_}", hoSoModel.MaHoSo);
            mailBody = mailBody.Replace("{_NgayChuyen_}", chuyenHoSoModel.NgayChuyenHoSo);
            mailBody = mailBody.Replace("{_NguoiChuyen_}", nv.HoVaTen + " (" + nv.MaNVMoi + ")");
            mailBody = mailBody.Replace("{_TruongHopChuyen_}", chuyenHoSoModel.LyDoChuyenHoSo);
            mailBody = mailBody.Replace("{_GhiChu_}", chuyenHoSoModel.GhiChu);
            mailBody = mailBody.Replace("{_EmailToaAnChuyen_}", toaAnChuyen.Email);
            Commons.SendMail(EmailTemplate.CHUYENHOSOPHUCTHAM_EMAIL_SUBJECT, toaAnNhan.Email, mailBody, cc: toaAnChuyen.Email);
        }

        #endregion

        #region Tra lai don
        public ActionResult GetTraLaiDonTheoHoSoVuAnID(int id)
        {
            var viewModel = _nhanDonService.ChiTietTraLaiDonTheoHoSoVuAnID(id, GetAnSessionInfo().CongDoanId);

            if (viewModel != null)
            {
                ViewBag.listNgayTao = _nhanDonService.DanhSachNgayTraLaiDon(viewModel.HoSoVuAnID, GetAnSessionInfo().GiaiDoanId, GetAnSessionInfo().CongDoanId, 0);
            }

            return PartialView("TraLaiDon/Detail", viewModel);
        }

        public ActionResult GetTraLaiDonTheoTraLaiDonID(int id)
        {
            var viewModel = _nhanDonService.ChiTietTraLaiDonTheoTraLaiDonID(id);

            if (viewModel == null)
            {
                viewModel = new TraLaiDonModel();
                viewModel.HoSoVuAnID = id;
            }

            ViewBag.listNgayTao = _nhanDonService.DanhSachNgayTraLaiDon(viewModel.HoSoVuAnID, GetAnSessionInfo().GiaiDoanId, GetAnSessionInfo().CongDoanId, viewModel.TraLaiDonID);
            return PartialView("TraLaiDon/Detail", viewModel);
        }

        [CustomAuthorize]
        public ActionResult EditTraLaiDon(int id)
        {
            var viewModel = _nhanDonService.ChiTietTraLaiDonTheoHoSoVuAnID(id, GetAnSessionInfo().CongDoanId);
            if (viewModel == null)
            {
                viewModel = new TraLaiDonModel();
                viewModel.HoSoVuAnID = id;
                viewModel.NgayTraDon = DateTime.Now;
            }
            ViewBag.ddlLyDo = _settingDataService.SettingDataItem("DS_SOTHAM_LYDOTRADONKHANGCAO", null);
            return PartialView("TraLaiDon/Edit", viewModel);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult EditTraLaiDon(TraLaiDonModel viewModel)
        {
            viewModel.GiaiDoan = GetAnSessionInfo().GiaiDoanId;
            viewModel.CongDoanHoSo = GetAnSessionInfo().CongDoanId;
            if (viewModel.NgayTraDon > DateTime.Now)
            {
                ModelState.AddModelError("NgayTraDon", ValidationMessages.VALIDATION_GIOIHAN_NGAYTRADON_SO_NGAYHIENTAI);
            }
            if (ModelState.IsValid)
            {
                var result = _nhanDonService.ThemTraLaiDon(viewModel);

                if (result != null && result.ResponseCode == 1)
                {
                    Success(result.ResponseMessage);
                    return RedirectToAction("GetTraLaiDonTheoHoSoVuAnID", new { id = viewModel.HoSoVuAnID });
                }
            }
            ViewBag.ddlLyDo = _settingDataService.SettingDataItem("DS_SOTHAM_LYDOTRADONKHANGCAO", null);

            return PartialView("TraLaiDon/Edit", viewModel);
        }
        #endregion
    }
}