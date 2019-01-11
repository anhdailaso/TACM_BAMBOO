using Biz.Lib.Authentication;
using Biz.Lib.Helpers;
using Biz.Lib.TACM.XetXu.DataAccess;
using Biz.Lib.TACM.XetXu.IDataAccess;
using Biz.Lib.TACM.XetXu.Model;
using Biz.TACM.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Biz.Lib.SettingData.Model;
using Biz.Lib.TACM.Resources.Resources;

namespace Biz.TACM.Services
{
    public class XetXuService : IXetXuService
    {
        private IXetXuDataAccess _xetXuDA;
        private IXetXuDataAccess XetXuDA => _xetXuDA ?? (_xetXuDA = new XetXuDataAccess());

        private ISettingDataService _settingDataService;
        private ISettingDataService SettingDataService => _settingDataService ?? (_settingDataService = new SettingDataService());

        #region NoiDung
        //public SelectList SelectListThuTuc(string selectedValue)
        //{
        //    var list = XMLUtils.BindData("ThuTuc");
        //    return new SelectList(list, "Value", "Text", selectedValue);
        //}

        //public SelectList SelectListVuAnDuocXetXu(string selectedValue)
        //{
        //    var list = XMLUtils.BindData("VuAn");
        //    return new SelectList(list, "Value", "Text", selectedValue);
        //}

        public SelectList DanhSachNgayXetXu(int hoSoVuAnID, int giaiDoan, int xetXuGroup, int selected)
        {
            try
            {
                var dbModel = XetXuDA.DanhSachNgayXetXu(hoSoVuAnID, giaiDoan, xetXuGroup);

                var listNgayTao = dbModel.Select(
                    x => new SelectListItem
                    {
                        Text = String.Format("{0:dd/MM/yyyy HH:mm:ss}", x.NgayTao),
                        Value = x.XetXuID.ToString()
                    }
                );

                return new SelectList(listNgayTao, "Value", "Text", selected.ToString());
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("DanhSachNgayXetXu with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public SelectList SelectListNhanVien(string nhomChucNang, int toaAnId, int selected)
        {
            try
            {
                var dbModel = XetXuDA.DanhSachNhanVien(nhomChucNang, toaAnId);

                var danhSachNhanVien = dbModel.Select(
                    x => new SelectListItem
                    {
                        Text = x.HoTenVaMaNV,
                        Value = x.NhanVienID.ToString()
                    }
                );

                return new SelectList(danhSachNhanVien, "Text", "Text", selected.ToString());
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("SelectListNhanVien with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public List<NhanVienModel> DanhSachNhanVien(string nhomChucNang, int toaAnId, List<NhanVienModel> selected)
        {
            try
            {
                var listNhanVien = XetXuDA.DanhSachNhanVien(nhomChucNang, toaAnId).ToList();
                if (nhomChucNang.ToLower() == "thư ký")
                    listNhanVien.AddRange(XetXuDA.DanhSachNhanVien("Thẩm tra viên", toaAnId).ToList());
                List<string> manv = new List<string>();

                if (selected != null)
                {
                    foreach (NhanVienModel nv in selected)
                    {
                        manv.Add(nv.MaNV);
                    }
                }

                var list = new List<NhanVienModel>();

                foreach (var item in listNhanVien)
                {
                    bool check = selected != null && manv.Contains(item.MaNV);
                    item.Checked = check;
                    list.Add(item);
                }


                return list;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("DanhSachNhanVien with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public XetXuModel ChiTietXetXuTheoHoSoVuAnID(int hoSoVuAnID, int giaiDoan, int xetXuGroup)
        {
            try
            {
                var dbModel = XetXuDA.ThongTinXetXuTheoHoSoVuAnID(hoSoVuAnID, giaiDoan, xetXuGroup);

                if (dbModel != null)
                {
                    dbModel = AddDanhSachDuKhuyetToModel(dbModel);
                }                

                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ChiTietXetXuTheoHoSoVuAnID with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return new XetXuModel();
            }
        }

        public XetXuModel ChiTietXetXuTheoXetXuID(int khieuNaiTraDonID)
        {
            try
            {
                var dbModel = XetXuDA.ThongTinXetXuTheoXetXuID(khieuNaiTraDonID);

                if (dbModel != null)
                {
                    dbModel = AddDanhSachDuKhuyetToModel(dbModel);
                }

                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ChiTietXetXuTheoXetXuID with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return new XetXuModel();
            }
        }

        public ResponseResult ThemXetXu(XetXuModel viewModel)
        {
            ResponseResult result = null;
            viewModel.NguoiTao = SettingDataService.GetNhanVienSessionInfo().MaNV;
            viewModel.NgayTao = DateTime.Now;
            result = XetXuDA.ThemXetXu(viewModel);
            return result;
        }

        private XetXuModel AddDanhSachDuKhuyetToModel(XetXuModel dbModel)
        {
            var listThamPhanDuKhuyet = XetXuDA.DanhSachThamPhanDuKhuyet(dbModel.NgayTao);
            List<NhanVienModel> namesThamPhan = new List<NhanVienModel>();
            foreach (var item in listThamPhanDuKhuyet)
            {
                namesThamPhan.Add(item);
            }

            var listHoiThamNhanDanDuKhuyet = XetXuDA.DanhSachHoiThamNhanDanDuKhuyet(dbModel.NgayTao);
            List<NhanVienModel> namesHoiThamNhanDan = new List<NhanVienModel>();
            foreach (var item in listHoiThamNhanDanDuKhuyet)
            {
                namesHoiThamNhanDan.Add(item);
            }

            var listThuKyDuKhuyet = XetXuDA.DanhSachThuKyDuKhuyet(dbModel.NgayTao);
            List<NhanVienModel> namesThuKy = new List<NhanVienModel>();
            foreach (var item in listThuKyDuKhuyet)
            {
                namesThuKy.Add(item);
            }

            var listKiemSatVienDuKhuyet = XetXuDA.DanhSachKiemSatVienDuKhuyet(dbModel.NgayTao);
            List<NhanVienModel> namesKiemSatVien = new List<NhanVienModel>();
            foreach (var item in listKiemSatVienDuKhuyet)
            {
                namesKiemSatVien.Add(item);
            }

            dbModel.ThamPhanDuKhuyet = namesThamPhan;
            dbModel.HoiThamNhanDanDuKhuyet = namesHoiThamNhanDan;
            dbModel.ThuKyDuKhuyet = namesThuKy;
            dbModel.KiemSatVienDuKhuyet = namesKiemSatVien;

            //nhan vien model
            dbModel.NhanVienThamPhan = XetXuDA.ThongTinNhanVien(dbModel.ThamPhan);
            dbModel.NhanVienThamPhan1 = XetXuDA.ThongTinNhanVien(dbModel.ThamPhan1);
            dbModel.NhanVienThamPhan2 = XetXuDA.ThongTinNhanVien(dbModel.ThamPhan2);
            dbModel.NhanVienThamPhanKhac = XetXuDA.ThongTinNhanVien(dbModel.ThamPhanKhac);
            dbModel.NhanVienHoiThamNhanDan = XetXuDA.ThongTinNhanVien(dbModel.HoiThamNhanDan);
            dbModel.NhanVienHoiThamNhanDan2 = XetXuDA.ThongTinNhanVien(dbModel.HoiThamNhanDan2);
            dbModel.NhanVienHoiThamNhanDan3 = XetXuDA.ThongTinNhanVien(dbModel.HoiThamNhanDan3);
            dbModel.NhanVienThuKy = XetXuDA.ThongTinNhanVien(dbModel.ThuKy);       
            dbModel.NhanVienKiemSatVien = XetXuDA.ThongTinNhanVien(dbModel.KiemSatVien);

            return dbModel;
        }

        public XetXuModel AddDanhSachDuKhuyetTheoHoSoVuAnIdToModel(XetXuModel dbModel)
        {
            var listThamPhanDuKhuyet = SettingDataService.DanhSachThamPhanDuKhuyetTheoHoSoVuAnId(dbModel.HoSoVuAnID);
            List<NhanVienModel> namesThamPhan = new List<NhanVienModel>();
            foreach (var item in listThamPhanDuKhuyet)
            {
                namesThamPhan.Add(item);
            }

            var listHoiThamNhanDanDuKhuyet = SettingDataService.DanhSachHoiThamNhanDanDuKhuyetTheoHoSoVuAnId(dbModel.HoSoVuAnID);
            List<NhanVienModel> namesHoiThamNhanDan = new List<NhanVienModel>();
            foreach (var item in listHoiThamNhanDanDuKhuyet)
            {
                namesHoiThamNhanDan.Add(item);
            }

            var listThuKyDuKhuyet = SettingDataService.DanhSachThuKyDuKhuyetTheoHoSoVuAnId(dbModel.HoSoVuAnID);
            List<NhanVienModel> namesThuKy = new List<NhanVienModel>();
            foreach (var item in listThuKyDuKhuyet)
            {
                namesThuKy.Add(item);
            }

            var listKiemSatVienDuKhuyet = SettingDataService.DanhSachKiemSatVienDuKhuyetTheoHoSoVuAnId(dbModel.HoSoVuAnID);
            List<NhanVienModel> namesKiemSatVien = new List<NhanVienModel>();
            foreach (var item in listKiemSatVienDuKhuyet)
            {
                namesKiemSatVien.Add(item);
            }

            dbModel.ThamPhanDuKhuyet = namesThamPhan;
            dbModel.HoiThamNhanDanDuKhuyet = namesHoiThamNhanDan;
            dbModel.ThuKyDuKhuyet = namesThuKy;
            dbModel.KiemSatVienDuKhuyet = namesKiemSatVien;

            return dbModel;
        }
        #endregion

        #region TrieuTap
        public SelectList DanhSachNgayTrieuTap(int hoSoVuAnID, int giaiDoan, int selected)
        {
            try
            {
                var dbModel = XetXuDA.DanhSachNgayTrieuTap(hoSoVuAnID, giaiDoan);

                var listNgayTao = dbModel.Select(
                    x => new SelectListItem
                    {
                        Text = String.Format("{0:dd/MM/yyyy HH:mm:ss}", x.NgayTao),
                        Value = x.TrieuTapID.ToString()
                    }
                );

                return new SelectList(listNgayTao, "Value", "Text", selected.ToString());
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("DanhSachNgayTrieuTap with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public List<DuongSuModel> DanhSachDuongSu(int hoSoVuAnID, List<string> selected)
        {
            try
            {
                var listDuongSu = XetXuDA.DanhSachDuongSu(hoSoVuAnID);

                var list = new List<DuongSuModel>();

                foreach (var item in listDuongSu)
                {
                    bool check = selected != null && selected.Contains(item.DuongSuID.ToString());
                    item.Checked = check;
                    list.Add(item);
                }


                return list;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("DanhSachDuongSu with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public List<DuongSuModel> DanhSachTrieuTapDuongSu(int trieuTapID)
        {
            try
            {
                var dbModel = XetXuDA.DanhSachTrieuTapDuongSu(trieuTapID);

                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("DanhSachTrieuTapDuongSu with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return null;
            }
        }

        public TrieuTapModel ChiTietTrieuTapTheoHoSoVuAnID(int hoSoVuAnID, int giaiDoan)
        {
            try
            {
                var dbModel = XetXuDA.ThongTinTrieuTapTheoHoSoVuAnID(hoSoVuAnID, giaiDoan);

                if (dbModel != null)
                {
                    var listTrieuTapDuongSu = DanhSachTrieuTapDuongSu(dbModel.TrieuTapID);

                    List<string> ids = new List<string>();

                    foreach (var item in listTrieuTapDuongSu)
                    {
                        ids.Add(item.DuongSuID.ToString());
                    }

                    dbModel.DuongSuID = ids;

                    dbModel.NhanVienNguoiKy = XetXuDA.ThongTinNhanVien(dbModel.NguoiKy);
                    dbModel.NguoiTao = XetXuDA.ThongTinNhanVien(dbModel.NguoiTao).HoTenVaMaNV;
                }

                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ChiTietTrieuTapTheoHoSoVuAnID with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return new TrieuTapModel();
            }
        }

        public TrieuTapModel ChiTietTrieuTapTheoTrieuTapID(int trieuTapID)
        {
            try
            {
                var dbModel = XetXuDA.ThongTinTrieuTapTheoTrieuTapID(trieuTapID);

                if (dbModel != null)
                {
                    var listTrieuTapDuongSu = DanhSachTrieuTapDuongSu(dbModel.TrieuTapID);

                    List<string> ids = new List<string>();

                    foreach (var item in listTrieuTapDuongSu)
                    {
                        ids.Add(item.DuongSuID.ToString());
                    }

                    dbModel.DuongSuID = ids;

                    dbModel.NhanVienNguoiKy = XetXuDA.ThongTinNhanVien(dbModel.NguoiKy);
                    dbModel.NguoiTao = XetXuDA.ThongTinNhanVien(dbModel.NguoiTao).HoTenVaMaNV;
                }

                return dbModel;
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ChiTietTrieuTapTheoTrieuTapID with error [{0}]", ex.ToString()), AppName.BizSecurity);
                return new TrieuTapModel();
            }
        }

        public ResponseResult ThemTrieuTap(TrieuTapModel viewModel)
        {
            ResponseResult result = null;
            try
            {
                viewModel.NguoiTao = SettingDataService.GetNhanVienSessionInfo().MaNV;
                result = XetXuDA.ThemTrieuTap(viewModel);
            }
            catch (Exception ex)
            {
                WriteLog.Error(string.Format("ThemTrieuTap with error [{0}]", ex.ToString()), AppName.BizSecurity); //AppName.TACMThuLy
                result = null;
            }
            return result;
        }
        public SelectList SelectListLanThu(int lanThu)
        {
            var list = new List<SelectListItem>();

            for (int i = 1; i <= (lanThu + 1); i++)
            {
                list.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }

            return new SelectList(list, "Value", "Text", list);
        }
        #endregion
    }
}