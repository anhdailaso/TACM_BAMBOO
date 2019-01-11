using Biz.Lib.Helpers;
using Biz.Lib.TACM.ChuanBiXetXu.IDataAccess;
using Biz.Lib.TACM.ChuanBiXetXu.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Lib.TACM.ChuanBiXetXu.DataAccess
{
    public class ChuanBiXetXuDataAccess : IChuanBiXetXuDataAccess
    {
        #region HoaGiai
        public IEnumerable<HoaGiaiModel> DanhSachNgayHoaGiai(int hoSoVuAnID, int giaiDoan)
        {
            List<HoaGiaiModel> danhSachNgay = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", hoSoVuAnID));
                listParameter.Add(new SqlParameter("@GiaiDoan", giaiDoan));

                danhSachNgay = DBUtils.ExecuteSPList<HoaGiaiModel>("SP_ChuanBiXetXu_HoaGiai_Get_Danh_Sach_Ngay_Sua_Doi", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSachNgay;
        }

        public IEnumerable<NhanVienModel> DanhSachNhanVien(string nhomChucNang)
        {
            List<NhanVienModel> danhSachNhanVien = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@NhomChucNang", nhomChucNang));

                danhSachNhanVien = DBUtils.ExecuteSPList<NhanVienModel>("SP_ChuanBiXetXu_HoaGiai_DanhSachNhanVien_Theo_NhomChucNang", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSachNhanVien;
        }

        public IEnumerable<NhanVienModel> DanhSachThuKyTheoThamPhan(string MaNV)
        {
            List<NhanVienModel> danhSachNhanVien = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@MaNV", MaNV));

                danhSachNhanVien = DBUtils.ExecuteSPList<NhanVienModel>("SP_ChuanBiXetXu_HoaGiai_DanhSachThuKy_Theo_ThamPhan", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSachNhanVien;
        }

        public HoaGiaiModel ThongTinHoaGiaiTheoHoSoVuAnID(int hoSoVuAnID, int giaiDoan)
        {
            HoaGiaiModel dbModel = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", hoSoVuAnID));
                listParameter.Add(new SqlParameter("@GiaiDoan", giaiDoan));

                dbModel = DBUtils.ExecuteSP<HoaGiaiModel>("SP_ChuanBiXetXu_HoaGiai_ChiTiet_Theo_HoSoVuAnID", listParameter,
                    AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public HoaGiaiModel ThongTinHoaGiaiTheoHoaGiaiID(int id)
        {
            HoaGiaiModel dbModel = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoaGiaiID", id));

                dbModel = DBUtils.ExecuteSP<HoaGiaiModel>("SP_ChuanBiXetXu_HoaGiai_ChiTiet_Theo_HoaGiaiID", listParameter,
                    AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public ResponseResult ThemHoaGia(HoaGiaiModel model)
        {
            ResponseResult result = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", model.HoSoVuAnID));
                listParameter.Add(new SqlParameter("@NgayMoPhienHop", model.NgayMoPhienHop));
                listParameter.Add(new SqlParameter("@NoiDungPhienHop", model.NoiDungPhienHop));
                listParameter.Add(new SqlParameter("@ThamPhan", model.ThamPhan));
                listParameter.Add(new SqlParameter("@ThuKy", model.ThuKy));
                listParameter.Add(new SqlParameter("@NguoiTao", model.NguoiTao));

                result = DBUtils.ExecuteSP<ResponseResult>("SP_ChuanBiXetXu_HoaGiai_Them", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
        #endregion

        #region QuyetDinh
        public IEnumerable<QuyetDinhModel> DanhSachQuyetDinh(int hoSoVuAnID, int giaiDoan)
        {
            List<QuyetDinhModel> danhSachQuyetDinh = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", hoSoVuAnID));
                listParameter.Add(new SqlParameter("@GiaiDoan", giaiDoan));

                danhSachQuyetDinh = DBUtils.ExecuteSPList<QuyetDinhModel>("SP_ChuanBiXetXu_QuyetDinh_DanhSach_Theo_HoSoVuAnID", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSachQuyetDinh;
        }

        //public IEnumerable<DLLoaiQuyetDinhModel> DLLoaiQuyetDinh(int giaiDoan)
        //{
        //    List<DLLoaiQuyetDinhModel> danhSachLoaiQuyetDinh = null;
        //    try
        //    {
        //        List<SqlParameter> listParameter = new List<SqlParameter>();
        //        listParameter.Add(new SqlParameter("@GiaiDoan", giaiDoan));

        //        danhSachLoaiQuyetDinh = DBUtils.ExecuteSPList<DLLoaiQuyetDinhModel>("SP_ChuanBiXetXu_QuyetDinh_DanhSachLoaiQuyetDinh", listParameter, AppName.BizSecurity);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return danhSachLoaiQuyetDinh;
        //}

        //public IEnumerable<DLQuyetDinhModel> DLDanhSachQuyetDinh(int loaiQuyetDinhID)
        //{
        //    List<DLQuyetDinhModel> danhSachQuyetDinh = null;
        //    try
        //    {
        //        List<SqlParameter> listParameter = new List<SqlParameter>();
        //        listParameter.Add(new SqlParameter("@LoaiQuyetDinhID", loaiQuyetDinhID));

        //        danhSachQuyetDinh = DBUtils.ExecuteSPList<DLQuyetDinhModel>("SP_ChuanBiXetXu_QuyetDinh_DanhSachQuyetDinh_Theo_LoaiQuyetDinh", listParameter, AppName.BizSecurity);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return danhSachQuyetDinh;
        //}

        public QuyetDinhModel ThongTinQuyetDinhTheoQuyetDinhID(int id)
        {
            QuyetDinhModel dbModel = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@QuyetDinhID", id));

                dbModel = DBUtils.ExecuteSP<QuyetDinhModel>("SP_ChuanBiXetXu_QuyetDinh_ChiTiet_Theo_QuyetDinhID", listParameter,
                    AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public ResponseResult SuaQuyetDinh(QuyetDinhModel model)
        {
            ResponseResult result = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", model.HoSoVuAnID));
                listParameter.Add(new SqlParameter("@QuyetDinhLoai", model.QuyetDinhLoai));
                listParameter.Add(new SqlParameter("@QuyetDinhID", model.QuyetDinhID));
                listParameter.Add(new SqlParameter("@TenQuyetDinh", model.TenQuyetDinh));
                listParameter.Add(new SqlParameter("@NgayRaQuyetDinh", model.NgayRaQuyetDinh));
                listParameter.Add(new SqlParameter("@ThoiHanGiaHan", model.ThoiHanGiaHan));
                listParameter.Add(new SqlParameter("@GhiChu", model.GhiChu));
                listParameter.Add(new SqlParameter("@NguoiTao", model.NguoiTao));

                result = DBUtils.ExecuteSP<ResponseResult>("SP_ChuanBiXetXu_QuyetDinh_Sua", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public ResponseResult ThemQuyetDinh(QuyetDinhModel model)
        {
            ResponseResult result = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", model.HoSoVuAnID));
                listParameter.Add(new SqlParameter("@QuyetDinhLoai", model.QuyetDinhLoai));
                listParameter.Add(new SqlParameter("@TenQuyetDinh", model.TenQuyetDinh));
                listParameter.Add(new SqlParameter("@NgayRaQuyetDinh", model.NgayRaQuyetDinh));
                listParameter.Add(new SqlParameter("@ThoiHanGiaHan", model.ThoiHanGiaHan));
                listParameter.Add(new SqlParameter("@GhiChu", model.GhiChu));
                listParameter.Add(new SqlParameter("@NguoiTao", model.NguoiTao));

                result = DBUtils.ExecuteSP<ResponseResult>("SP_ChuanBiXetXu_QuyetDinh_Them", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public ResponseResult XoaQuyetDinh(int quyetDinhID)
        {
            ResponseResult result = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@QuyetDinhID", quyetDinhID));

                result = DBUtils.ExecuteSP<ResponseResult>("SP_ChuanBiXetXu_QuyetDinh_Xoa", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
        #endregion

        #region QuyetDinh NhomAn HinhSu

        public IEnumerable<QuyetDinhHinhSuModel> DanhSachNgaySuaQuyetDinhHinhSu(int hoSoVuAnID, int giaiDoan, int quyetDinhGroup)
        {
            List<QuyetDinhHinhSuModel> danhSachNgay = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", hoSoVuAnID));
                listParameter.Add(new SqlParameter("@GiaiDoan", giaiDoan));
                listParameter.Add(new SqlParameter("@QuyetDinhGroup", quyetDinhGroup));

                danhSachNgay = DBUtils.ExecuteSPList<QuyetDinhHinhSuModel>("SP_ChuanBiXetXu_QuyetDinh_HinhSu_DanhSachNgaySuaDoi", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSachNgay;
        }

        public QuyetDinhHinhSuModel ThongTinQuyetDinhHinhSuTheoHoSoVuAnID(int hoSoVuAnID, int giaiDoan, int quyetDinhGroup)
        {
            QuyetDinhHinhSuModel dbModel = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", hoSoVuAnID));
                listParameter.Add(new SqlParameter("@GiaiDoan", giaiDoan));
                listParameter.Add(new SqlParameter("@QuyetDinhGroup", quyetDinhGroup));

                dbModel = DBUtils.ExecuteSP<QuyetDinhHinhSuModel>("SP_ChuanBiXetXu_QuyetDinh_HinhSu_ChiTiet_Theo_HoSoVuAnID", listParameter,
                    AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public QuyetDinhHinhSuModel ThongTinQuyetDinhHinhSuTheoQuyetDinhID(int id)
        {
            QuyetDinhHinhSuModel dbModel = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@QuyetDinhID", id));

                dbModel = DBUtils.ExecuteSP<QuyetDinhHinhSuModel>("SP_ChuanBiXetXu_QuyetDinh_ChiTiet_Theo_QuyetDinhID", listParameter,
                    AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public ResponseResult ThemQuyetDinhHinhSu(QuyetDinhHinhSuModel model, int quyetdinhGroup)
        {
            ResponseResult result = null;
            try
            {
                string danhSachLyDo = "";
                if (quyetdinhGroup != 5)
                {
                    if (model.DanhSachLyDo != null)
                        danhSachLyDo = String.Join("&", model.DanhSachLyDo);
                }
                else
                    danhSachLyDo = model.LyDo;
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", model.HoSoVuAnID));
                listParameter.Add(new SqlParameter("@SoQuyetDinh", model.SoQuyetDinh));
                listParameter.Add(new SqlParameter("@LanThu", model.LanThu));
                listParameter.Add(new SqlParameter("@NgayRaQuyetDinh", model.NgayRaQuyetDinh));
                listParameter.Add(new SqlParameter("@NgayRutHoSo", model.NgayRutHoSo));
                listParameter.Add(new SqlParameter("@LyDo", danhSachLyDo));
                listParameter.Add(new SqlParameter("@KetQuaGiaiQuyet", model.KetQuaGiaiQuyet));
                listParameter.Add(new SqlParameter("@DinhKemFile", model.DinhKemFile));
                listParameter.Add(new SqlParameter("@QuyetDinhGroup", model.QuyetDinhGroup));
                listParameter.Add(new SqlParameter("@LoaiDinhChi", model.LoaiDinhChi));
                listParameter.Add(new SqlParameter("@GhiChu", model.GhiChu));
                listParameter.Add(new SqlParameter("@NguoiTao", model.NguoiTao));

                result = DBUtils.ExecuteSP<ResponseResult>("SP_ChuanBiXetXu_QuyetDinh_HinhSu_Them", listParameter, AppName.BizSecurity);
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
