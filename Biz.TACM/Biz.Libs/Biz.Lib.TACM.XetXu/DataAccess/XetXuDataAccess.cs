using System;
using System.Collections.Generic;
using System.Linq;
using Biz.Lib.TACM.XetXu.IDataAccess;
using Biz.Lib.TACM.XetXu.Model;
using System.Data.SqlClient;
using Biz.Lib.Helpers;
using Biz.Lib.SettingData.Model;

namespace Biz.Lib.TACM.XetXu.DataAccess
{
    public class XetXuDataAccess : IXetXuDataAccess
    {
        #region XetXu
        public IEnumerable<XetXuModel> DanhSachNgayXetXu(int hoSoVuAnID, int giaiDoan, int xetXuGroup)
        {
            List<XetXuModel> danhSachNgay = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", hoSoVuAnID));
                listParameter.Add(new SqlParameter("@GiaiDoan", giaiDoan));
                listParameter.Add(new SqlParameter("@XetXuGroup", xetXuGroup));

                danhSachNgay = DBUtils.ExecuteSPList<XetXuModel>("SP_XetXu_NoiDung_Get_Danh_Sach_Ngay_Sua_Doi", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSachNgay;
        }

        public List<NhanVienModel> DanhSachThamPhanDuKhuyet(DateTime? ngayTao)
        {
            List<NhanVienModel> results = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@NgayTao", ngayTao));

                results = DBUtils.ExecuteSPList<NhanVienModel>("SP_XetXu_NoiDung_DanhSachThamPhanDuKhuyet", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return results;
        }

        public List<NhanVienModel> DanhSachHoiThamNhanDanDuKhuyet(DateTime? ngayTao)
        {
            List<NhanVienModel> results = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@NgayTao", ngayTao));

                results = DBUtils.ExecuteSPList<NhanVienModel>("SP_XetXu_NoiDung_DanhSachHoiThamNhanDanDuKhuyet", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return results;
        }

        public List<NhanVienModel> DanhSachThuKyDuKhuyet(DateTime? ngayTao)
        {
            List<NhanVienModel> results = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@NgayTao", ngayTao));

                results = DBUtils.ExecuteSPList<NhanVienModel>("SP_XetXu_NoiDung_DanhSachThuKyDuKhuyet", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return results;
        }

        public List<NhanVienModel> DanhSachKiemSatVienDuKhuyet(DateTime? ngayTao)
        {
            List<NhanVienModel> results = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@NgayTao", ngayTao));

                results = DBUtils.ExecuteSPList<NhanVienModel>("SP_XetXu_NoiDung_DanhSachKiemSatVienDuKhuyet", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return results;
        }

        public IEnumerable<NhanVienModel> DanhSachNhanVien(string nhomChucNang, int toaAnId)
        {
            List<NhanVienModel> danhSachNhanVien = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@ChucDanh", nhomChucNang));
                listParameter.Add(new SqlParameter("@ToaAnID", toaAnId));

                danhSachNhanVien = DBUtils.ExecuteSPList<NhanVienModel>("SP_NhanVien_DanhSachNhanVien_Theo_ChucDanh", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSachNhanVien;
        }

        public NhanVienModel ThongTinNhanVien(string maNV)
        {
            NhanVienModel dbModel = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@MaNV", maNV));

                dbModel = DBUtils.ExecuteSP<NhanVienModel>("SP_XetXu_NoiDung_ChiTiet_NhanVien", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (dbModel == null)
            {
                dbModel = new NhanVienModel();
            }

            return dbModel;
        }

        public XetXuModel ThongTinXetXuTheoHoSoVuAnID(int hoSoVuAnID, int giaiDoan, int xetXuGroup)
        {
            XetXuModel dbModel = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", hoSoVuAnID));
                listParameter.Add(new SqlParameter("@GiaiDoan", giaiDoan));
                listParameter.Add(new SqlParameter("@XetXuGroup", xetXuGroup));

                dbModel = DBUtils.ExecuteSP<XetXuModel>("SP_XetXu_NoiDung_ChiTiet_Theo_HoSoVuAnID", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public XetXuModel ThongTinXetXuTheoXetXuID(int id)
        {
            XetXuModel dbModel = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@XetXuID", id));

                dbModel = DBUtils.ExecuteSP<XetXuModel>("SP_XetXu_NoiDung_ChiTiet_Theo_XetXuID", listParameter,
                    AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public ResponseResult ThemXetXu(XetXuModel model)
        {
            ResponseResult result = null;
            try
            {
                string thamPhanDuKhuyet = "";
                if (model.ThamPhanDuKhuyet != null)
                    thamPhanDuKhuyet = String.Join(",", model.ThamPhanDuKhuyet.Where(m => m.MaNV != null).Select(g => g.MaNV));

                string hoiThamNhanDanDuKhuyet = "";
                if (model.HoiThamNhanDanDuKhuyet != null)
                    hoiThamNhanDanDuKhuyet = String.Join(",", model.HoiThamNhanDanDuKhuyet.Where(m => m.MaNV != null).Select(g => g.MaNV));

                string thuKyDuKhuyet = "";
                if (model.ThuKyDuKhuyet != null)
                    thuKyDuKhuyet = String.Join(",", model.ThuKyDuKhuyet.Where(m => m.MaNV != null).Select(g => g.MaNV));

                string kiemSatVienDuKhuyet = "";
                if (model.KiemSatVienDuKhuyet != null)
                    kiemSatVienDuKhuyet = String.Join(",", model.KiemSatVienDuKhuyet.Where(m => m.MaNV != null).Select(g => g.MaNV));

                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", model.HoSoVuAnID));
                listParameter.Add(new SqlParameter("@ThuTuc", model.ThuTuc));
                listParameter.Add(new SqlParameter("@NgayRaQuyetDinh", model.NgayRaQuyetDinh));
                listParameter.Add(new SqlParameter("@ThoiGianMoPhienToa", model.ThoiGianMoPhienToa));
                listParameter.Add(new SqlParameter("@DiaDiemMoPhienToa", model.DiaDiemMoPhienToa));
                listParameter.Add(new SqlParameter("@VuAnDuocXetXu", model.VuAnDuocXetXu));
                listParameter.Add(new SqlParameter("@ThamPhanChuToaPhienToa", model.ThamPhan));
                listParameter.Add(new SqlParameter("@ThamPhan1", model.ThamPhan1));
                listParameter.Add(new SqlParameter("@ThamPhan2", model.ThamPhan2));
                listParameter.Add(new SqlParameter("@ThamPhanKhac", model.ThamPhanKhac));
                listParameter.Add(new SqlParameter("@HoiThamNhanDan", model.HoiThamNhanDan));
                listParameter.Add(new SqlParameter("@HoiThamNhanDan2", model.HoiThamNhanDan2));
                listParameter.Add(new SqlParameter("@HoiThamNhanDan3", model.HoiThamNhanDan3));
                listParameter.Add(new SqlParameter("@ThuKy", model.ThuKy));
                listParameter.Add(new SqlParameter("@KiemSatVien", model.KiemSatVien));
                listParameter.Add(new SqlParameter("@HoiDong", model.HoiDong));
                listParameter.Add(new SqlParameter("@XetXuGroup", model.XetXuGroup));
                listParameter.Add(new SqlParameter("@NguoiTao", model.NguoiTao));
                listParameter.Add(new SqlParameter("@NgayTao", model.NgayTao));
                listParameter.Add(new SqlParameter("@ThamPhanDuKhuyet", thamPhanDuKhuyet));
                listParameter.Add(new SqlParameter("@HoiThamNhanDanDuKhuyet", hoiThamNhanDanDuKhuyet));
                listParameter.Add(new SqlParameter("@ThuKyDuKhuyet", thuKyDuKhuyet));
                listParameter.Add(new SqlParameter("@KiemSatVienDuKhuyet", kiemSatVienDuKhuyet));

                result = DBUtils.ExecuteSP<ResponseResult>("SP_XetXu_NoiDung_Them", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
        #endregion

        #region TrieuTap
        public IEnumerable<TrieuTapModel> DanhSachNgayTrieuTap(int hoSoVuAnID, int giaiDoan)
        {
            List<TrieuTapModel> danhSachNgay = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", hoSoVuAnID));
                listParameter.Add(new SqlParameter("@GiaiDoan", giaiDoan));

                danhSachNgay = DBUtils.ExecuteSPList<TrieuTapModel>("SP_XetXu_TrieuTap_Get_Danh_Sach_Ngay_Sua_Doi", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSachNgay;
        }

        public List<DuongSuModel> DanhSachDuongSu(int hoSoVuAnID)
        {
            List<DuongSuModel> danhSachDuongSu = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", hoSoVuAnID));

                danhSachDuongSu = DBUtils.ExecuteSPList<DuongSuModel>("SP_XetXu_TrieuTap_DanhSachDuongSu", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSachDuongSu;
        }

        public List<DuongSuModel> DanhSachTrieuTapDuongSu(int trieuTapID)
        {
            List<DuongSuModel> danhSachDuongSu = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@TrieuTapID", trieuTapID));

                danhSachDuongSu = DBUtils.ExecuteSPList<DuongSuModel>("SP_XetXu_TrieuTap_DanhSachTrieuTapDuongSu", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSachDuongSu;
        }

        public TrieuTapModel ThongTinTrieuTapTheoHoSoVuAnID(int hoSoVuAnID, int giaiDoan)
        {
            TrieuTapModel dbModel = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", hoSoVuAnID));
                listParameter.Add(new SqlParameter("@GiaiDoan", giaiDoan));

                dbModel = DBUtils.ExecuteSP<TrieuTapModel>("SP_XetXu_TrieuTap_ChiTiet_Theo_HoSoVuAnID", listParameter,
                    AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public TrieuTapModel ThongTinTrieuTapTheoTrieuTapID(int id)
        {
            TrieuTapModel dbModel = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@TrieuTapID", id));

                dbModel = DBUtils.ExecuteSP<TrieuTapModel>("SP_XetXu_TrieuTap_ChiTiet_Theo_TrieuTapID", listParameter,
                    AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public ResponseResult ThemTrieuTap(TrieuTapModel model)
        {
            ResponseResult result = null;
            try
            {
                string trieuTapDuongSuIDs = "";
                if (model.DuongSuID != null)
                {
                    trieuTapDuongSuIDs = String.Join(",", model.DuongSuID);
                }

                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", model.HoSoVuAnID));
                listParameter.Add(new SqlParameter("@ThoiGianMoPhienToa", model.ThoiGianMoPhienToa));
                listParameter.Add(new SqlParameter("@DiaDiemMoPhienToa", model.DiaDiemMoPhienToa));
                listParameter.Add(new SqlParameter("@LanThu", model.LanThu));
                listParameter.Add(new SqlParameter("@NguoiKy", model.NguoiKy));
                listParameter.Add(new SqlParameter("@NguoiTao", model.NguoiTao));
                listParameter.Add(new SqlParameter("@TrieuTapDuongSuIDs", trieuTapDuongSuIDs));

                result = DBUtils.ExecuteSP<ResponseResult>("SP_XetXu_TrieuTap_Them", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
        
        #endregion
    }
}
