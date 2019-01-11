using Biz.Lib.Helpers;
using Biz.Lib.TACM.KetQuaXetXu.IDataAccess;
using Biz.Lib.TACM.KetQuaXetXu.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Lib.TACM.KetQuaXetXu.DataAccess
{
    public class KetQuaXetXuDataAccess : IKetQuaXetXuDataAccess
    {
        #region QuyetDinh
        public IEnumerable<LoaiQuyetDinhModel> DanhSachLoaiQuyetDinh()
        {
            List<LoaiQuyetDinhModel> danhSachNgay = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();

                danhSachNgay = DBUtils.ExecuteSPList<LoaiQuyetDinhModel>("SP_KetQuaXetXu_QuyetDinh_DanhSach_LoaiQuyetDinh", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSachNgay;
        }

        public IEnumerable<QuyetDinhModel> DanhSachNgayQuyetDinh(int hoSoVuAnID, int giaiDoan)
        {
            List<QuyetDinhModel> danhSachNgay = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", hoSoVuAnID));
                listParameter.Add(new SqlParameter("@GiaiDoan", giaiDoan));

                danhSachNgay = DBUtils.ExecuteSPList<QuyetDinhModel>("SP_KetQuaXetXu_QuyetDinh_Get_Danh_Sach_Ngay_Sua_Doi", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSachNgay;
        }

        public QuyetDinhModel ThongTinQuyetDinhTheoHoSoVuAnID(int hoSoVuAnID, int giaiDoan)
        {
            QuyetDinhModel dbModel = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", hoSoVuAnID));
                listParameter.Add(new SqlParameter("@GiaiDoan", giaiDoan));

                dbModel = DBUtils.ExecuteSP<QuyetDinhModel>("SP_KetQuaXetXu_QuyetDinh_ChiTiet_Theo_HoSoVuAnID", listParameter,
                    AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public QuyetDinhModel ThongTinQuyetDinhTheoQuyetDinhID(int id)
        {
            QuyetDinhModel dbModel = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@QuyetDinhID", id));

                dbModel = DBUtils.ExecuteSP<QuyetDinhModel>("SP_KetQuaXetXu_QuyetDinh_ChiTiet_Theo_QuyetDinhID", listParameter,
                    AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public QuyetDinhModel ThongTinQuyetDinhTheoSoQuyetDinh(string soQuyetDinh, int hoSoVuAnID, DateTime ngayRaQuyetDinh)
        {
            QuyetDinhModel dbModel = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@SoQuyetDinh", soQuyetDinh));
                listParameter.Add(new SqlParameter("@HoSoVuAnID", hoSoVuAnID));
                listParameter.Add(new SqlParameter("@NgayRaQuyetDinh", ngayRaQuyetDinh));

                dbModel = DBUtils.ExecuteSP<QuyetDinhModel>("SP_KetQuaXetXu_QuyetDinh_ChiTiet_Theo_SoQuyetDinh", listParameter,
                    AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public ResponseResult ThemQuyetDinh(QuyetDinhModel model)
        {
            ResponseResult result = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", model.HoSoVuAnID));
                listParameter.Add(new SqlParameter("@LoaiQuyetDinh", model.LoaiQuyetDinh));
                listParameter.Add(new SqlParameter("@SoQuyetDinh", model.SoQuyetDinh));
                listParameter.Add(new SqlParameter("@NgayRaQuyetDinh", model.NgayRaQuyetDinh));
                listParameter.Add(new SqlParameter("@LoaiQuanHe", model.LoaiQuanHe));
                listParameter.Add(new SqlParameter("@QuanHePhapLuat", model.QuanHePhapLuat));
                listParameter.Add(new SqlParameter("@BienPhapXuLyHanhChinh", model.BienPhapXuLyHanhChinh));
                listParameter.Add(new SqlParameter("@ThoiHanApDung", model.ThoiHanApDung));
                listParameter.Add(new SqlParameter("@LyDo", model.LyDo));
                listParameter.Add(new SqlParameter("@KetQuaGiaiQuyet", model.KetQuaGiaiQuyet));
                //listParameter.Add(new SqlParameter("@HieuLuc", model.HieuLuc));
                listParameter.Add(new SqlParameter("@DinhKemFile", model.DinhKemFile));
                listParameter.Add(new SqlParameter("@NoiDungQuyetDinh", model.NoiDungQuyetDinh));
                listParameter.Add(new SqlParameter("@GhiChu", model.GhiChu));
                listParameter.Add(new SqlParameter("@NguoiTao", model.NguoiTao));
                listParameter.Add(new SqlParameter("@ThamPhan", model.ThamPhan));
                listParameter.Add(new SqlParameter("@ThamPhan1", model.ThamPhan1));
                listParameter.Add(new SqlParameter("@ThamPhan2", model.ThamPhan2));
                listParameter.Add(new SqlParameter("@ThamPhanKhac", model.ThamPhanKhac));
                listParameter.Add(new SqlParameter("@HoiThamNhanDan", model.HoiThamNhanDan));
                listParameter.Add(new SqlParameter("@HoiThamNhanDan2", model.HoiThamNhanDan2));
                listParameter.Add(new SqlParameter("@HoiThamNhanDan3", model.HoiThamNhanDan3));
                listParameter.Add(new SqlParameter("@KiemSatVien", model.KiemSatVien));
                listParameter.Add(new SqlParameter("@ThuKy", model.ThuKy));

                result = DBUtils.ExecuteSP<ResponseResult>("SP_KetQuaXetXu_QuyetDinh_Them", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public int SoQuyetDinhMax(int hoSoVuAnId, DateTime? ngayRaQuyetDinh)
        {
            ResponseResult temp = new ResponseResult();
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", hoSoVuAnId));
                listParameter.Add(new SqlParameter("@NgayRaQuyetDinh", ngayRaQuyetDinh));
                temp = DBUtils.ExecuteSP<ResponseResult>("SP_KetQuaXetXu_QuyetDinh_SoQuyetDinh", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return temp.ResponseCode;
        }
        #endregion

        #region QuyetDinh ADBPXLHC PhucTham

        public QuyetDinhADBPXLHCModel ThongTinQuyetDinhADBPXLHCTheoHoSoVuAnID(int hoSoVuAnID, int giaiDoan)
        {
            QuyetDinhADBPXLHCModel dbModel = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", hoSoVuAnID));
                listParameter.Add(new SqlParameter("@GiaiDoan", giaiDoan));

                dbModel = DBUtils.ExecuteSP<QuyetDinhADBPXLHCModel>("SP_KetQuaXetXu_QuyetDinh_ADBPXLHC_ChiTiet_Theo_HoSoVuAnID", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public QuyetDinhADBPXLHCModel ThongTinQuyetDinhADBPXLHCTheoQuyetDinhID(int quyetDinhID)
        {
            QuyetDinhADBPXLHCModel dbModel = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@QuyetDinhID", quyetDinhID));

                dbModel = DBUtils.ExecuteSP<QuyetDinhADBPXLHCModel>("SP_KetQuaXetXu_QuyetDinh_ADBPXLHC_ChiTiet_Theo_QuyetDinhID", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public ResponseResult ThemQuyetDinhADBPXLHC(QuyetDinhADBPXLHCModel model)
        {
            ResponseResult result = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", model.HoSoVuAnID));
                listParameter.Add(new SqlParameter("@SoQuyetDinh", model.SoQuyetDinh));
                listParameter.Add(new SqlParameter("@NgayRaQuyetDinh", model.NgayRaQuyetDinh));
                listParameter.Add(new SqlParameter("@LyDo", model.LyDo));
                listParameter.Add(new SqlParameter("@KetQuaGiaiQuyet", model.KetQuaGiaiQuyet));
                listParameter.Add(new SqlParameter("@ThamPhan", model.ThamPhan));
                listParameter.Add(new SqlParameter("@ThuKy", model.ThuKy));
                listParameter.Add(new SqlParameter("@KiemSatVien", model.KiemSatVien));
                listParameter.Add(new SqlParameter("@GhiChu", model.GhiChu));
                listParameter.Add(new SqlParameter("@NguoiTao", model.NguoiTao));

                result = DBUtils.ExecuteSP<ResponseResult>("SP_KetQuaXetXu_QuyetDinh_ADBPXLHC_PhucTham_Them", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        #endregion

        #region BanAn
        public IEnumerable<BanAnModel> DanhSachNgayBanAn(int hoSoVuAnID, int giaiDoan)
        {
            List<BanAnModel> danhSachNgay = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", hoSoVuAnID));
                listParameter.Add(new SqlParameter("@GiaiDoan", giaiDoan));

                danhSachNgay = DBUtils.ExecuteSPList<BanAnModel>("SP_KetQuaXetXu_BanAn_Get_Danh_Sach_Ngay_Sua_Doi", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSachNgay;
        }

        public BanAnModel ThongTinBanAnTheoHoSoVuAnID(int hoSoVuAnID, int giaiDoan)
        {
            BanAnModel dbModel = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", hoSoVuAnID));
                listParameter.Add(new SqlParameter("@GiaiDoan", giaiDoan));

                dbModel = DBUtils.ExecuteSP<BanAnModel>("SP_KetQuaXetXu_BanAn_ChiTiet_Theo_HoSoVuAnID", listParameter,
                    AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public BanAnModel ThongTinBanAnTheoBanAnID(int id)
        {
            BanAnModel dbModel = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@KQXXBanAnID", id));

                dbModel = DBUtils.ExecuteSP<BanAnModel>("SP_KetQuaXetXu_BanAn_ChiTiet_Theo_BanAnID", listParameter,
                    AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public BanAnModel ThongTinBanAnTheoSoBanAn(string soBanAn, int hoSoVuAnID, DateTime ngayTuyenAn)
        {
            BanAnModel dbModel = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@SoBanAn", soBanAn));
                listParameter.Add(new SqlParameter("@HoSoVuAnID", hoSoVuAnID));
                listParameter.Add(new SqlParameter("@NgayTuyenAn", ngayTuyenAn));

                dbModel = DBUtils.ExecuteSP<BanAnModel>("SP_KetQuaXetXu_BanAn_ChiTiet_Theo_SoBanAn", listParameter,
                    AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public ResponseResult ThemBanAn(BanAnModel model)
        {
            ResponseResult result = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", model.HoSoVuAnID));
                listParameter.Add(new SqlParameter("@SoBanAn", model.SoBanAn));
                listParameter.Add(new SqlParameter("@NgayThangNamBanAn", model.NgayThangNamBanAn));
                listParameter.Add(new SqlParameter("@NgayMoPhienToa", model.NgayMoPhienToa));
                listParameter.Add(new SqlParameter("@NgayTuyenAn", model.NgayTuyenAn));
                listParameter.Add(new SqlParameter("@HieuLuc", model.HieuLuc));
                //listParameter.Add(new SqlParameter("@LoaiQuanHe", model.LoaiQuanHe));
                listParameter.Add(new SqlParameter("@QuanHePhapLuat", model.QuanHePhapLuat));
                listParameter.Add(new SqlParameter("@KetQuaXetXu", model.KetQuaXetXu));
                //listParameter.Add(new SqlParameter("@XetXu", model.XetXu));
                listParameter.Add(new SqlParameter("@DinhKemFile", model.DinhKemFile));
                listParameter.Add(new SqlParameter("@NoiDungTuyenXu", model.NoiDungTuyenXu));
                listParameter.Add(new SqlParameter("@ThamPhan", model.ThamPhan));
                listParameter.Add(new SqlParameter("@ThamPhan1", model.ThamPhan1));
                listParameter.Add(new SqlParameter("@ThamPhan2", model.ThamPhan2));
                listParameter.Add(new SqlParameter("@ThamPhanKhac", model.ThamPhanKhac));
                listParameter.Add(new SqlParameter("@HoiThamNhanDan", model.HoiThamNhanDan));
                listParameter.Add(new SqlParameter("@HoiThamNhanDan2", model.HoiThamNhanDan2));
                listParameter.Add(new SqlParameter("@HoiThamNhanDan3", model.HoiThamNhanDan3));
                listParameter.Add(new SqlParameter("@KiemSatVien", model.KiemSatVien));
                listParameter.Add(new SqlParameter("@ThuKy", model.ThuKy));
                listParameter.Add(new SqlParameter("@GhiChu", model.GhiChu));
                listParameter.Add(new SqlParameter("@NguoiTao", model.NguoiTao));

                result = DBUtils.ExecuteSP<ResponseResult>("SP_KetQuaXetXu_BanAn_Them", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public int SoBanAnMax(int hoSoVuAnId, DateTime? ngayTuyenAn)
        {
            ResponseResult temp = new ResponseResult();
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", hoSoVuAnId));
                listParameter.Add(new SqlParameter("@NgayThuLy", ngayTuyenAn));
                temp = DBUtils.ExecuteSP<ResponseResult>("SP_KetQuaXetXu_BanAn_SoBanAn", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return temp.ResponseCode;
        }

        public ResponseResult CapNhatHuyBanAnSoTham(int hoSoVuAnID)
        {
            ResponseResult result = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", hoSoVuAnID));

                result = DBUtils.ExecuteSP<ResponseResult>("SP_KetQuaXetXu_BanAn_CapNhatHuyBanAnSoTham", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        #region Toi Danh
        public IEnumerable<ToiDanhModel> DanhSachKetQuaXetXu(int hoSoVuAnId)
        {
            List<ToiDanhModel> listDbModel = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", hoSoVuAnId));
                listDbModel = DBUtils.ExecuteSPList<ToiDanhModel>("SP_KetQuaXetXu_ToiDanh_DanhSach_Theo_HoSoVuAnID", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listDbModel;
        }
        public ToiDanhModel ChiTietToiDanhTheoID(int toiDanhID)
        {
            ToiDanhModel dbModel = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@ToiDanhID", toiDanhID));
                dbModel = DBUtils.ExecuteSP<ToiDanhModel>("SP_KetQuaXetXu_ToiDanh_ChiTiet_Theo_ID", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }
        public ResponseResult TaoToiDanh(ToiDanhModel model)
        {
            ResponseResult result = null;
            decimal.TryParse(model.AnPhi, out decimal anPhi);
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", model.HoSoVuAnID));
                listParameter.Add(new SqlParameter("@BiCao", model.BiCao));
                listParameter.Add(new SqlParameter("@KetQuaXetXu", model.KetQuaXetXu));
                listParameter.Add(new SqlParameter("@DieuLuatApDung", model.DieuLuatApDung));
                listParameter.Add(new SqlParameter("@ToiDanh", model.ToiDanh));
                listParameter.Add(new SqlParameter("@HinhPhat", model.HinhPhat));
                listParameter.Add(new SqlParameter("@AnPhi", anPhi));
                listParameter.Add(new SqlParameter("@BienPhapTuPhap", model.BienPhapTuPhap));
                listParameter.Add(new SqlParameter("@BienPhapKhienTrach", model.BienPhapKhienTrach));
                listParameter.Add(new SqlParameter("@DinhKemFile", model.DinhKemFile));
                listParameter.Add(new SqlParameter("@TrachNhiemDanSu", model.TrachNhiemDanSu));
                listParameter.Add(new SqlParameter("@XuLy", model.XuLy));
                listParameter.Add(new SqlParameter("@GiaiDoan", model.GiaiDoan));
                listParameter.Add(new SqlParameter("@CongDoanHoSo", model.CongDoanHoSo));
                listParameter.Add(new SqlParameter("@GhiChu", model.GhiChu));
                listParameter.Add(new SqlParameter("@NguoiTao", model.NguoiTao));

                result = DBUtils.ExecuteSP<ResponseResult>("SP_KetQuaXetXu_ToiDanh_Tao", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
        public ResponseResult SuaToiDanh(ToiDanhModel model)
        {
            ResponseResult result = null;
            decimal.TryParse(model.AnPhi, out decimal anPhi);
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@ToiDanhID", model.ToiDanhID));
                listParameter.Add(new SqlParameter("@BiCao", model.BiCao));
                listParameter.Add(new SqlParameter("@KetQuaXetXu", model.KetQuaXetXu));
                listParameter.Add(new SqlParameter("@DieuLuatApDung", model.DieuLuatApDung));
                listParameter.Add(new SqlParameter("@ToiDanh", model.ToiDanh));
                listParameter.Add(new SqlParameter("@HinhPhat", model.HinhPhat));
                listParameter.Add(new SqlParameter("@AnPhi", anPhi));
                listParameter.Add(new SqlParameter("@BienPhapTuPhap", model.BienPhapTuPhap));
                listParameter.Add(new SqlParameter("@BienPhapKhienTrach", model.BienPhapKhienTrach));
                listParameter.Add(new SqlParameter("@DinhKemFile", model.DinhKemFile));
                listParameter.Add(new SqlParameter("@TrachNhiemDanSu", model.TrachNhiemDanSu));
                listParameter.Add(new SqlParameter("@XuLy", model.XuLy));
                listParameter.Add(new SqlParameter("@GhiChu", model.GhiChu));
                listParameter.Add(new SqlParameter("@NguoiTao", model.NguoiTao));

                result = DBUtils.ExecuteSP<ResponseResult>("SP_KetQuaXetXu_ToiDanh_Sua", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
        public ResponseResult XoaToiDanh(int toiDanhId, string nguoiTao)
        {
            ResponseResult result = null;
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@ToiDanhID", toiDanhId),
                    new SqlParameter("@NguoiTao", nguoiTao)
                };
                result = DBUtils.ExecuteSP<ResponseResult>("SP_KetQuaXetXu_ToiDanh_Xoa", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        #endregion

        #endregion
    }
}
