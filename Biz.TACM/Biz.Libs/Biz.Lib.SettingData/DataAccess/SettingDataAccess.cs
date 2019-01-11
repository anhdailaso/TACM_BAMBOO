using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Biz.Lib.Helpers;
using System.Runtime.Caching;
using Biz.Lib.SettingData.Model;

namespace Biz.Lib.SettingData.DataAccess
{
    public class SettingDataAccess : ISettingDataAccess
    {
        public List<Model.SettingItemModel> GetDataItem(string groupName)
        {
            List<Model.SettingItemModel> listItem = null;
            try
            {
                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@GroupName", groupName.ToString()));

                listItem = DBUtils.ExecuteSPList<Model.SettingItemModel>("SP_Sec_DataItem_Setting", paramList, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return listItem;
        }

        public List<Model.SettingItemModel> GetDataItemCached(string groupName)
        {
            return GetDataItemObjectCached(groupName);
        }

        private List<Model.SettingItemModel> GetDataItemObjectCached(string groupName)
        {
            ObjectCache cache = MemoryCache.Default;

            List<Model.SettingItemModel> listItem = cache.Get(groupName.ToString()) as List<Model.SettingItemModel>;

            if (listItem == null)
            {
                listItem = GetDataItem(groupName);

                CacheItemPolicy policy = new CacheItemPolicy();
                policy.Priority = CacheItemPriority.NotRemovable;

                cache.Add(groupName.ToString(), listItem, policy);
            }

            return listItem;
        }

        public List<ToaAnModel> DanhSachToaAn()
        {
            List<ToaAnModel> danhSachToaAn = null;
            try
            {
                danhSachToaAn = DBUtils.ExecuteSPList<ToaAnModel>("SP_ToaAn_DanhSachToaAn", null, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSachToaAn;
        }

        public List<ToaAnModel> DanhSachToaAnTheoNV(string maNV)
        {
            List<ToaAnModel> danhSachToaAn = null;
            var parameters = new List<SqlParameter> { new SqlParameter("@MaNV", maNV) };
            try
            {
                danhSachToaAn = DBUtils.ExecuteSPList<ToaAnModel>("SP_ToaAn_DanhSachToaAn_Theo_NV", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSachToaAn;
        }

        public ToaAnModel GetToaAnTheoMaNhanVien(string maNV)
        {
            ToaAnModel model = null;
            try
            {
                var parameters = new List<SqlParameter> { new SqlParameter("@MaNV", maNV) };
                model = DBUtils.ExecuteSP<ToaAnModel>("SP_ToaAn_ChiTiet_Theo_MaNV", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }

        public ToaAnModel GetToaAnTheoToaAnID(int toaAnID)
        {
            ToaAnModel model = null;
            try
            {
                var parameters = new List<SqlParameter> { new SqlParameter("@ToaAnID", toaAnID) };
                model = DBUtils.ExecuteSP<ToaAnModel>("SP_ToaAn_ChiTiet_Theo_ToaAnID", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }

        public List<NhomAnModel> DanhSachNhomAnTheoToaAn(int toaAnId)
        {
            List<NhomAnModel> danhSachNhomAn = null;
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ToaAnID", toaAnId));
                danhSachNhomAn = DBUtils.ExecuteSPList<NhomAnModel>("SP_NhomAn_DanhSachNhomAn_Theo_ToaAnID", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSachNhomAn;
        }

        //public IEnumerable<NhanVienModel> DanhSachNhanVienTheoChucDanh(string chucDanh)
        public List<NhanVienModel> DanhSachNhanVienTheoChucDanh(string chucDanh, int? toaAnId)
        {
            List<NhanVienModel> danhSachNhanVien = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@ChucDanh", chucDanh));
                listParameter.Add(new SqlParameter("@ToaAnID", toaAnId));

                danhSachNhanVien = DBUtils.ExecuteSPList<NhanVienModel>("SP_NhanVien_DanhSachNhanVien_Theo_ChucDanh", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSachNhanVien;
        }

        //public IEnumerable<NhanVienModel> DanhSachNhanVienTheoTenChucNangNghiepVu(string tenChucNang)
        public List<NhanVienModel> DanhSachNhanVienTheoTenChucNangNghiepVu(string tenChucNang, int toaAnId)
        {
            List<NhanVienModel> danhSachNhanVien = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@TenChucNang", tenChucNang));
                listParameter.Add(new SqlParameter("@ToaAnID", toaAnId));

                danhSachNhanVien = DBUtils.ExecuteSPList<NhanVienModel>("SP_NhanVien_DanhSachNhanVien_Theo_ChucNangNghiepVu", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSachNhanVien;
        }       

        public NhanVienModel ChiTietNhanVienTheoMaNhanVien(string maNV)
        {
            NhanVienModel model;
            try
            {
                var parameters = new List<SqlParameter> { new SqlParameter("@MaNV", maNV) };
                model = DBUtils.ExecuteSP<NhanVienModel>("SP_NhanVien_ChiTiet_Theo_MaNV", parameters, AppName.BizSecurity);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }

        public NhanVienModel GetSessionInfoNhanVienTheoMaNhanVien(string maNV)
        {
            NhanVienModel model;
            try
            {
                var parameters = new List<SqlParameter> { new SqlParameter("@MaNV", maNV) };
                model = DBUtils.ExecuteSP<NhanVienModel>("SP_NhanVien_ChiTiet_SessionInfo_Theo_MaNV", parameters, AppName.BizSecurity);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }

        public List<HoSoVuAnModel> DanhSachHoSoVuAnIsInRole(string maNV)
        {
            List<HoSoVuAnModel> model;
            try
            {
                var parameters = new List<SqlParameter> { new SqlParameter("@MaNV", maNV) };
                model = DBUtils.ExecuteSPList<HoSoVuAnModel>("SP_NhanVien_DanhSach_HoSoVuAn_IsInRole", parameters, AppName.BizSecurity);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }

        public string GetLoaiQuanHe(int hoSoVuAnId)
        {
            string qh = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", hoSoVuAnId));
                qh = DBUtils.ExecuteSP<LoaiQuanHeModel>("SP_HoSoVuAn_LoaiQuanHe", listParameter, AppName.BizSecurity).LoaiQuanHe;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return qh;
        }

        public ToaAnModel GetToaAnTheoTenToaAn(string tenToaAn)
        {
            ToaAnModel model = null;
            try
            {
                var parameters = new List<SqlParameter> { new SqlParameter("@TenToaAn", tenToaAn) };
                model = DBUtils.ExecuteSP<ToaAnModel>("SP_ToaAn_ChiTiet_Theo_TenToaAn", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }

        public List<NhanVienModel> DanhSachThamPhanDuKhuyetTheoHoSoVuAnId(int hoSoVuAnId)
        {
            List<NhanVienModel> results = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter> {new SqlParameter("@HoSoVuAnID", hoSoVuAnId)};

                results = DBUtils.ExecuteSPList<NhanVienModel>("SP_DanhSachDuKhuyet_ThamPhanDuKhuyet_Theo_HoSoVuAnID", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return results;
        }

        public List<NhanVienModel> DanhSachHoiThamNhanDanDuKhuyetTheoHoSoVuAnId(int hoSoVuAnId)
        {
            List<NhanVienModel> results = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter> { new SqlParameter("@HoSoVuAnID", hoSoVuAnId) };

                results = DBUtils.ExecuteSPList<NhanVienModel>("SP_DanhSachDuKhuyet_HoiThamNhanDanDuKhuyet_Theo_HoSoVuAnID", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return results;
        }

        public List<NhanVienModel> DanhSachThuKyDuKhuyetTheoHoSoVuAnId(int hoSoVuAnId)
        {
            List<NhanVienModel> results = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter> { new SqlParameter("@HoSoVuAnID", hoSoVuAnId) };

                results = DBUtils.ExecuteSPList<NhanVienModel>("SP_DanhSachDuKhuyet_ThuKyDuKhuyet_Theo_HoSoVuAnID", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return results;
        }

        public List<NhanVienModel> DanhSachKiemSatVienDuKhuyetTheoHoSoVuAnId(int hoSoVuAnId)
        {
            List<NhanVienModel> results = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter> { new SqlParameter("@HoSoVuAnID", hoSoVuAnId) };

                results = DBUtils.ExecuteSPList<NhanVienModel>("SP_DanhSachDuKhuyet_KiemSatVienDuKhuyet_Theo_HoSoVuAnID", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return results;
        }

    }
}
