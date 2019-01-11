using Biz.Lib.Helpers;
using Biz.Lib.TACM.ThongKeGiamSat.IDataAccess;
using Biz.Lib.TACM.ThongKeGiamSat.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Biz.Lib.TACM.ThongKeGiamSat.DataAccess
{
    public class ThongKeGiamSatDataAccess : IThongKeGiamSatDataAccess
    {
        #region Giam Sat Thuc Hien
        public IEnumerable<GiamSatHoSoVuAnModel> GetDanhSachHoSoVuAnGiamSat(string listHoSoVuAnID)
        {
            List<GiamSatHoSoVuAnModel> danhSachHoSoVuAnGiamSat = null;
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@ListHoSoVuAnID", listHoSoVuAnID)
                };
                danhSachHoSoVuAnGiamSat = DBUtils.ExecuteSPList<GiamSatHoSoVuAnModel>("SP_GiamSatThucHien_ThongKeDanhSachHoSoVuAn", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSachHoSoVuAnGiamSat;
        }

        public IEnumerable<GiamSatDuLieuBieuDoModel> LocDuLieuGiamSat(ref string listHoSoVuAnID, string tuNgay, string denNgay, int group, int? toaAnID = null, string nhomAn = null, int? giaiDoan = null)
        {
            List<GiamSatDuLieuBieuDoModel> danhSachHoSoVuAnGiamSat = null;
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@TuNgay", tuNgay),
                    new SqlParameter("@DenNgay", denNgay),
                    new SqlParameter("@ToaAnID", toaAnID),
                    new SqlParameter("@NhomAn", nhomAn),
                    new SqlParameter("@GiaiDoan", giaiDoan),
                    new SqlParameter("@Group", group)
                };

                SqlParameter par = new SqlParameter("@ListHoSoVuAnID", SqlDbType.VarChar, 8000);
                par.Direction = ParameterDirection.Output;
                par.Value = null;
                parameters.Add(par);

                danhSachHoSoVuAnGiamSat = DBUtils.ExecuteSPList<GiamSatDuLieuBieuDoModel>("SP_GiamSatThucHien_ThongKeSoLuongHoSoVuAn_Theo_TungCap", parameters, AppName.BizSecurity);

                listHoSoVuAnID = par.Value.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSachHoSoVuAnGiamSat;
        }
        #endregion

        #region ThongKeTongHop

        public IEnumerable<ThongKeTongHopModel> ThongKeTongHop(int loaiThongKe, string tuNgay, string denNgay, int? toaAnID = null, string nhomAn = null, int? giaiDoan = null, string loaiQuanHe=null, string loaiDeNghi = null, string quanHePhapLuat = null, string toiDanh = null)
        {
            List<ThongKeTongHopModel> list = null;
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@LoaiThongke", loaiThongKe),
                    new SqlParameter("@TuNgay", tuNgay),
                    new SqlParameter("@DenNgay", denNgay),
                    new SqlParameter("@ToaAnID", toaAnID),
                    new SqlParameter("@NhomAn", nhomAn),
                    new SqlParameter("@GiaiDoan", giaiDoan),
                    new SqlParameter("@LoaiQuanHe", loaiQuanHe),
                    new SqlParameter("@LoaiDeNghi", loaiDeNghi),
                    new SqlParameter("@QuanHePhapLuat", quanHePhapLuat),
                    new SqlParameter("@ToiDanh", toiDanh)
                };
                list = DBUtils.ExecuteSPList<ThongKeTongHopModel>("SP_ThongKe_ThongKeTongHop", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        public IEnumerable<HoSoBaoCaoThongKeModel> DanhSachHoSoBaoCaoThongKe(string danhSachId)
        {
            List<HoSoBaoCaoThongKeModel> list = null;
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@DanhSachID", danhSachId)
                };
                list = DBUtils.ExecuteSPList<HoSoBaoCaoThongKeModel>("SP_ThongKe_DanhSachHoSo_TheoListID", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        public DataSet GetDataSetHoSoBaoCaoThongKe(string danhSachId)
        {
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@DanhSachID", danhSachId)
                };

                return DBUtils.ExecDataSetSP("SP_ThongKe_DanhSachHoSo_TheoListID_Export", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetDataSetChiTienXetXu(string tuNgay, string denNgay, int? toaAnId)
        {
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@TuNgay", tuNgay),
                    new SqlParameter("@DenNgay", denNgay),
                    new SqlParameter("@ToaAnID", toaAnId)
                };

                return DBUtils.ExecDataSetSP("SP_ThongKe_ChiTienXetXu_Export", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetDataSetThongKeLuuKho(string danhSachId)
        {
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@ListHoSoVuAnID", danhSachId)
                };

                return DBUtils.ExecDataSetSP("SP_ThongKe_ThongKeLuuKho_Export", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region ThongKePhanCongThamPhan

        public IEnumerable<HoSoPhanCongThamPhanModel> DanhSachHoSoChuaPhanCongThamPhan(string tuNgay, string denNgay, int toaAnId)
        {
            List<HoSoPhanCongThamPhanModel> list = null;
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@TuNgay", tuNgay),
                    new SqlParameter("@DenNgay", denNgay),
                    new SqlParameter("@ToaAnID", toaAnId)
                };
                list = DBUtils.ExecuteSPList<HoSoPhanCongThamPhanModel>("SP_ThongKe_HoSo_Chua_PhanCongThamPhan", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        public List<ThongKeTrucTuyenModel> ThongKeTrucTuyenPhanCongThamPhan(string tuNgay, string denNgay, int toaAnId)
        {
            List<ThongKeTrucTuyenModel> list = null;
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@TuNgay", tuNgay),
                    new SqlParameter("@DenNgay", denNgay),
                    new SqlParameter("@ToaAnID", toaAnId)
                };
                list = DBUtils.ExecuteSPList<ThongKeTrucTuyenModel>("SP_ThongKe_ThongKeTrucTuyenPhanCongThamPhan", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        #endregion

        #region ThongKeLuuKho

        public IEnumerable<DuLieuBieuDoModel> DuLieuBieuDoThongKeLuuKho(ref string listHoSoVuAnId, string tuNgay, string denNgay, int group, int? toaAnId = null, string nhomAn = null, int? giaiDoan = null)
        {
            List<DuLieuBieuDoModel> list = null;
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@TuNgay", tuNgay),
                    new SqlParameter("@DenNgay", denNgay),
                    new SqlParameter("@ToaAnID", toaAnId),
                    new SqlParameter("@NhomAn", nhomAn),
                    new SqlParameter("@GiaiDoan", giaiDoan),
                    new SqlParameter("@Group", group)
                };

                SqlParameter par = new SqlParameter("@ListHoSoVuAnID", SqlDbType.VarChar, 8000);
                par.Direction = ParameterDirection.Output;
                par.Value = null;
                parameters.Add(par);

                list = DBUtils.ExecuteSPList<DuLieuBieuDoModel>("SP_ThongKe_ThongKeLuuKho", parameters, AppName.BizSecurity);
                if (par.Value != null) listHoSoVuAnId = par.Value.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }
        public IEnumerable<HoSoThongKeLuuKhoModel> GetDanhSachHoSoThongKeLuuKho(string listHoSoVuAnId)
        {
            List<HoSoThongKeLuuKhoModel> list = null;
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@ListHoSoVuAnID", listHoSoVuAnId)
                };
                list = DBUtils.ExecuteSPList<HoSoThongKeLuuKhoModel>("SP_ThongKe_ThongKeLuKho_DanhSachHoSo", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        #endregion

        #region ThongKeAnHuySua

        public IEnumerable<DuLieuBieuDoHuySuaModel> DuLieuBieuDoThongKeAnHuySua(ref string listHoSoHuyId, ref string listHoSoSuaId, string tuNgay, string denNgay, int? toaAnId, string thamPhan)
        {
            List<DuLieuBieuDoHuySuaModel> list = null;
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@TuNgay", tuNgay),
                    new SqlParameter("@DenNgay", denNgay),
                    new SqlParameter("@ToaAnID", toaAnId),
                    new SqlParameter("@ThamPhan", thamPhan)
                };

                SqlParameter par1 = new SqlParameter("@ListHoSoHuyID", SqlDbType.VarChar, 8000)
                {
                    Direction = ParameterDirection.Output,
                    Value = null
                };

                SqlParameter par2 = new SqlParameter("@ListHoSoSuaID", SqlDbType.VarChar, 8000)
                {
                    Direction = ParameterDirection.Output,
                    Value = null
                };

                parameters.Add(par1);
                parameters.Add(par2);

                list = DBUtils.ExecuteSPList<DuLieuBieuDoHuySuaModel>("SP_ThongKe_ThongKeAnHuySua", parameters, AppName.BizSecurity);
                if (par1.Value != null) listHoSoHuyId = par1.Value.ToString();
                if (par2.Value != null) listHoSoSuaId = par2.Value.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }
        public IEnumerable<HoSoThongKeHuySuaModel> GetDanhSachHoSoThongKeAnHuySua(string listHoSoHuyId, string listHoSoSuaId)
        {
            List<HoSoThongKeHuySuaModel> list = null;
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@ListHoSoHuyID", listHoSoHuyId),
                    new SqlParameter("@ListHoSoSuaID", listHoSoSuaId)
                };
                list = DBUtils.ExecuteSPList<HoSoThongKeHuySuaModel>("SP_ThongKe_ThongKeAnHuySua_DanhSachHoSo", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }


        #endregion

    }
}
