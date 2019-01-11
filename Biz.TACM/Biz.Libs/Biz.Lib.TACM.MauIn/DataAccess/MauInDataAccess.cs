using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Biz.Lib.Helpers;
using Biz.Lib.TACM.MauIn.IDataAccess;
using Biz.Lib.TACM.MauIn.Model;

namespace Biz.Lib.TACM.MauIn.DataAccess
{
    public class MauInDataAccess : IMauInDataAccess
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hoSoVuAnId"></param>
        /// <returns></returns>
        public MauInSo24Model ChiTietMauInSo24(int hoSoVuAnId)
        {
            MauInSo24Model dbModel = null;
            try
            {
                var parameters = new List<SqlParameter> { new SqlParameter("@HoSoVuAnID", hoSoVuAnId) };
                dbModel = DBUtils.ExecuteSP<MauInSo24Model>("SP_MauIn_MauSo24_ChiTietMauIn", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hoSoVuAnId"></param>
        /// <returns></returns>
        public MauInSo30Model ChiTietMauInSo30(int hoSoVuAnId)
        {
            MauInSo30Model dbModel = null;
            try
            {
                var parameters = new List<SqlParameter> { new SqlParameter("@HoSoVuAnID", hoSoVuAnId) };
                dbModel = DBUtils.ExecuteSP<MauInSo30Model>("SP_MauIn_MauSo30_ChiTietMauIn", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hoSoVuAnId"></param>
        /// <returns></returns>
        public IEnumerable<DuongSuModel> DanhSachDuongSuMauInSo24(int hoSoVuAnId)
        {
            List<DuongSuModel> danhSachDuongSu = null;
            try
            {
                var parameters = new List<SqlParameter> { new SqlParameter("@HoSoVuAnID", hoSoVuAnId) };
                danhSachDuongSu = DBUtils.ExecuteSPList<DuongSuModel>("SP_MauIn_MauSo24_DanhSachDuongSu", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSachDuongSu;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hoSoVuAnId"></param>
        /// <returns></returns>
        public IEnumerable<DuongSuModel> DanhSachDuongSuMauInSo30(int hoSoVuAnId)
        {
            List<DuongSuModel> danhSachDuongSu = null;
            try
            {
                var parameters = new List<SqlParameter> { new SqlParameter("@HoSoVuAnID", hoSoVuAnId) };
                danhSachDuongSu = DBUtils.ExecuteSPList<DuongSuModel>("SP_MauIn_MauSo30_DanhSachDuongSu", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSachDuongSu;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbModel"></param>
        /// <returns></returns>
        public ResponseResult LuuLichSuIn(LichSuInModel dbModel)
        {
            ResponseResult result = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", dbModel.HoSoVuAnID));
                listParameter.Add(new SqlParameter("@SoMauIn", dbModel.SoMauIn));
                listParameter.Add(new SqlParameter("@DuongDanFileIn", dbModel.DuongDanFileIn));
                listParameter.Add(new SqlParameter("@NhomNghiepVu", dbModel.NhomNghiepVu));
                listParameter.Add(new SqlParameter("@NhomAn", dbModel.NhomAn));
                listParameter.Add(new SqlParameter("@GiaiDoan", dbModel.GiaiDoan));
                listParameter.Add(new SqlParameter("@CongDoanHoSo", dbModel.CongDoanHoSo));
                listParameter.Add(new SqlParameter("@NguoiTao", dbModel.NguoiTao));
                listParameter.Add(new SqlParameter("@GhiChu", dbModel.GhiChu));

                result = DBUtils.ExecuteSP<ResponseResult>("SP_MauIn_Luu_LichSuIn", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        #region Mau In So 29
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hoSoVuAnId"></param>
        /// <returns>MauInSo29Model</returns>
        public MauInSo29Model DuLieuMauInSo29(int hoSoVuAnId)
        {
            MauInSo29Model mauIn = new MauInSo29Model();
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>
                {
                    new SqlParameter("@HoSoVuAnID", hoSoVuAnId)
                };
                mauIn = DBUtils.ExecuteSP<MauInSo29Model>("SP_MauIn_MauSo29_ChiTietMauIn", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return mauIn;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hoSoVuAnId"></param>
        /// <returns>List<DuongSuModel></returns>
        public IEnumerable<DuongSuModel> DanhSachDuongSuMauInSo29(int hoSoVuAnId)
        {
            List<DuongSuModel> danhSachDuongSu = new List<DuongSuModel>();
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>
                {
                    new SqlParameter("@HoSoVuAnID", hoSoVuAnId)
                };
                danhSachDuongSu = DBUtils.ExecuteSPList<DuongSuModel>("SP_MauIn_MauSo29_DanhSachDuongSu", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSachDuongSu;
        }
        #endregion

        #region Mau In So 47
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hoSoVuAnId"></param>
        /// <returns>MauInSo47Model</returns>
        public MauInSo47Model DuLieuMauInSo47(int hoSoVuAnId)
        {
            MauInSo47Model mauIn = new MauInSo47Model();
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>
                {
                    new SqlParameter("@HoSoVuAnID", hoSoVuAnId)
                };
                mauIn = DBUtils.ExecuteSP<MauInSo47Model>("SP_MauIn_MauSo47_ChiTietMauIn", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return mauIn;
        }
        public MauInSo47PTModel DuLieuMauInSo47PhucTham(int hoSoVuAnId)
        {
            MauInSo47PTModel mauIn = new MauInSo47PTModel();
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>
                {
                    new SqlParameter("@HoSoVuAnID", hoSoVuAnId)
                };
                mauIn = DBUtils.ExecuteSP<MauInSo47PTModel>("SP_MauIn_MauSo47PhucTham_ChiTietMauIn", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return mauIn;
        }

        public IEnumerable<DuongSuModel> NguoiKhangCaoMauInSo47(int hoSoVuAnId)
        {
            List<DuongSuModel> danhSachDuongSu = new List<DuongSuModel>();
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>
                {
                    new SqlParameter("@HoSoVuAnID", hoSoVuAnId)
                };
                danhSachDuongSu = DBUtils.ExecuteSPList<DuongSuModel>("SP_MauIn_MauSo47_NguoiKhangCao", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSachDuongSu;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hoSoVuAnId"></param>
        /// <returns>List<DuongSuMauInSo29Model></returns>
        public IEnumerable<DuongSuModel> DanhSachDuongSuMauInSo47(int hoSoVuAnId)
        {
            List<DuongSuModel> danhSachDuongSu = new List<DuongSuModel>();
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>
                {
                    new SqlParameter("@HoSoVuAnID", hoSoVuAnId)
                };
                danhSachDuongSu = DBUtils.ExecuteSPList<DuongSuModel>("SP_MauIn_MauSo47_DanhSachDuongSu", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSachDuongSu;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hoSoVuAnId"></param>
        /// <returns>List<ThamPhanDuKhuyetMauInSo47Model></returns>
        public IEnumerable<ThamPhanDuKhuyetMauInSo47Model> DanhSachThamPhanDuKhuyetMauInSo47(int hoSoVuAnId)
        {
            List<ThamPhanDuKhuyetMauInSo47Model> danhSachThamPhanDuKhuyet = new List<ThamPhanDuKhuyetMauInSo47Model>();
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>
                {
                    new SqlParameter("@HoSoVuAnID", hoSoVuAnId)
                };
                danhSachThamPhanDuKhuyet = DBUtils.ExecuteSPList<ThamPhanDuKhuyetMauInSo47Model>("SP_MauIn_MauSo47_DanhSachThamPhanDuKhuyet", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSachThamPhanDuKhuyet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hoSoVuAnId"></param>
        /// <returns>List<HoiThamNhanDanDuKhuyetMauInSo47Model></returns>
        public IEnumerable<HoiThamNhanDanDuKhuyetMauInSo47Model> DanhSachHoiThamNhanDanDuKhuyetMauInSo47(int hoSoVuAnId)
        {
            List<HoiThamNhanDanDuKhuyetMauInSo47Model> danhSachHoiThamNhanDanDuKhuyet = new List<HoiThamNhanDanDuKhuyetMauInSo47Model>();
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>
                {
                    new SqlParameter("@HoSoVuAnID", hoSoVuAnId)
                };
                danhSachHoiThamNhanDanDuKhuyet = DBUtils.ExecuteSPList<HoiThamNhanDanDuKhuyetMauInSo47Model>("SP_MauIn_MauSo47_DanhSachHoiThamNhanDanDuKhuyet", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSachHoiThamNhanDanDuKhuyet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hoSoVuAnId"></param>
        /// <returns>List<KiemSatVienDuKhuyetMauInSo47Model></returns>
        public IEnumerable<KiemSatVienDuKhuyetMauInSo47Model> DanhSachKiemSatVienDuKhuyetMauInSo47(int hoSoVuAnId)
        {
            List<KiemSatVienDuKhuyetMauInSo47Model> danhSachKiemSatVienDuKhuyet = new List<KiemSatVienDuKhuyetMauInSo47Model>();
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>
                {
                    new SqlParameter("@HoSoVuAnID", hoSoVuAnId)
                };
                danhSachKiemSatVienDuKhuyet = DBUtils.ExecuteSPList<KiemSatVienDuKhuyetMauInSo47Model>("SP_MauIn_MauSo47_DanhSachKiemSatVienDuKhuyet", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSachKiemSatVienDuKhuyet;
        }
        #endregion

        #region Mau In So 61
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hoSoVuAnId"></param>
        /// <returns>MauInSo61Model</returns>
        public MauInSo61Model DuLieuMauInSo61(int hoSoVuAnId)
        {
            MauInSo61Model mauIn = new MauInSo61Model();
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>
                {
                    new SqlParameter("@HoSoVuAnID", hoSoVuAnId)
                };
                mauIn = DBUtils.ExecuteSP<MauInSo61Model>("SP_MauIn_MauSo61_ChiTietMauIn", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return mauIn;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hoSoVuAnId"></param>
        /// <returns>List<NguoiKhangCaoMauInSo61Model></returns>
        public IEnumerable<NguoiKhangCaoMauInSo61Model> DanhSachNguoiKhangCaoMauInSo61(int hoSoVuAnId)
        {
            List<NguoiKhangCaoMauInSo61Model> danhSachNguoiKhangCao = new List<NguoiKhangCaoMauInSo61Model>();
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>
                {
                    new SqlParameter("@HoSoVuAnID", hoSoVuAnId)
                };
                danhSachNguoiKhangCao = DBUtils.ExecuteSPList<NguoiKhangCaoMauInSo61Model>("SP_MauIn_MauSo61_DanhSachNguoiKhangCao", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSachNguoiKhangCao;
        }
        #endregion

        #region Mau In So 65
        public MauInSo65Model DuLieuMauInSo65(int hoSoVuAnId)
        {
            MauInSo65Model mauIn = new MauInSo65Model();
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>
                {
                    new SqlParameter("@HoSoVuAnID", hoSoVuAnId)
                };
                mauIn = DBUtils.ExecuteSP<MauInSo65Model>("SP_MauIn_MauSo65_ChiTietMauIn", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return mauIn;
        }
        public IEnumerable<NguoiKhangCaoMauInSo65Model> NguoiKhangCao(int hoSoVuAnId)
        {
            List<NguoiKhangCaoMauInSo65Model> danhSachDuongSu=new List<NguoiKhangCaoMauInSo65Model>();
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter> { new SqlParameter("@HoSoVuAnID", hoSoVuAnId) };
                danhSachDuongSu = DBUtils.ExecuteSPList<NguoiKhangCaoMauInSo65Model>("SP_MauIn65_DanhSachNguoiKhangCao", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSachDuongSu;
        }
        #endregion

        #region Mau In Quyet Dinh Phan Cong Tham Phan

        public MauInQuyetDinhPCTPModel ChiTietMauInQuetDinhPCTP(int hoSoVuAnId, int giaiDoan)
        {
            MauInQuyetDinhPCTPModel dbModel = null;
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@HoSoVuAnID", hoSoVuAnId),
                    new SqlParameter("@GiaiDoan", giaiDoan)
                };
                dbModel = DBUtils.ExecuteSP<MauInQuyetDinhPCTPModel>("SP_MauIn_QuyetDinhPhanCongThamPhan_ChiTietMauIn", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        #endregion
        public MauInQuyetDinhDinhChiModel ChiTietMauInQuyetDinhDinhChi(int hoSoVuAnId, string loai)
        {
            MauInQuyetDinhDinhChiModel dbModel = null;
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@HoSoVuAnID", hoSoVuAnId),
                    new SqlParameter("@Loai", loai)
                };
                dbModel = DBUtils.ExecuteSP<MauInQuyetDinhDinhChiModel>("SP_MauIn_QDDinhChi", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }
        public MauInQDGiaHanCBXX ChiTietMauInGiaHanCBXX(int hoSoVuAnId, int giaiDoan)
        {
            MauInQDGiaHanCBXX dbModel = null;
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@HoSoVuAnID", hoSoVuAnId),
                    new SqlParameter("@GiaiDoan", giaiDoan)
                };
                dbModel = DBUtils.ExecuteSP<MauInQDGiaHanCBXX>("SP_MauIn_QDGiaHanCBXX", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }
        public MauInQDTamHoanModel ChiTietMauInTamHoan(int hoSoVuAnId, int giaiDoan)
        {
            MauInQDTamHoanModel dbModel = null;
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@HoSoVuAnID", hoSoVuAnId),
                    new SqlParameter("@GiaiDoan", giaiDoan)
                };
                dbModel = DBUtils.ExecuteSP<MauInQDTamHoanModel>("SP_MauIn_QDTamHoanPhienToa", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }
        public MauInLenhTrichXuatModel ChiTietMauInLenhTrichXuat(int hoSoVuAnId, int giaiDoan)
        {
            MauInLenhTrichXuatModel dbModel = null;
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@HoSoVuAnID", hoSoVuAnId),
                    new SqlParameter("@GiaiDoan", giaiDoan)
                };
                dbModel = DBUtils.ExecuteSP<MauInLenhTrichXuatModel>("SP_MauIn_QDTamHoanPhienToa", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }
        public MauInBBGiaoNhanModel ChiTietMauInBienBanGiaoNhan(int hoSoVuAnId)
        {
            MauInBBGiaoNhanModel dbModel = null;
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@HoSoVuAnID", hoSoVuAnId)
                };
                dbModel = DBUtils.ExecuteSP<MauInBBGiaoNhanModel>("SP_MauIn_BienBanGiaoNhan_ChiTietMauIn", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public MauInTamGiamModel ChiTietMauInTamGiam_4_5_9(int hoSoVuAnId, int giaiDoan)
        {
            MauInTamGiamModel dbModel = null;
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@HoSoVuAnID", hoSoVuAnId),
                    new SqlParameter("@GiaiDoan",giaiDoan)
                };
                dbModel = DBUtils.ExecuteSP<MauInTamGiamModel>("SP_MauIn_QDTamGiam_04_05_09", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public List<DuongSuModel> DanhSachBiCaoHinhPhat(int hoSoVuAnId)
        {
            List<DuongSuModel> dbModel = new List<DuongSuModel>();
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@HoSoVuAnID", hoSoVuAnId)
                };
                dbModel = DBUtils.ExecuteSPList<DuongSuModel>("SP_MauIn_HinhPhat_DanhSachBiCao", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        #region Mau In Giay Trieu Tap
        /// <summary>
        /// Get chi tiet mau in giay trieu tap
        /// </summary>
        /// <param name="hoSoVuAnId"></param>
        /// <returns>MauInGiayTrieuTapModel</returns>
        public MauInGiayTrieuTapModel ChiTietMauInGiayTrieuTap(int hoSoVuAnId)
        {
            MauInGiayTrieuTapModel dbModel = null;
            try
            {
                var parameters = new List<SqlParameter> { new SqlParameter("@HoSoVuAnID", hoSoVuAnId) };
                dbModel = DBUtils.ExecuteSP<MauInGiayTrieuTapModel>("SP_MauIn_GiayTrieuTap_ChiTietMauIn", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }
        /// <summary>
        /// Lay danh sach duong su trong bang Trieu Tap
        /// </summary>
        /// <param name="hoSoVuAnId"></param>
        /// <returns>List<DuongSuMauInSo29Model></returns>
        public IEnumerable<DuongSuModel> DanhSachDuongSuMauInGiayTrieuTap(int hoSoVuAnId)
        {
            List<DuongSuModel> danhSachDuongSu = new List<DuongSuModel>();
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>
                {
                    new SqlParameter("@HoSoVuAnID", hoSoVuAnId)
                };
                danhSachDuongSu = DBUtils.ExecuteSPList<DuongSuModel>("SP_MauIn_GiayTrieuTap_DanhSachDuongSu", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSachDuongSu;
        }
        #endregion

        #region Mau In BiaHoSo
        /// <summary>
        /// Get chi tiet mau in bia ho so
        /// </summary>
        /// <param name="hoSoVuAnId"></param>
        /// <returns>MauInBiaHoSoModel</returns>
        public MauInBiaHoSoModel ChiTietMauInBiaHoSo(int hoSoVuAnId)
        {
            MauInBiaHoSoModel dbModel = null;
            try
            {
                var parameters = new List<SqlParameter> { new SqlParameter("@HoSoVuAnID", hoSoVuAnId) };
                dbModel = DBUtils.ExecuteSP<MauInBiaHoSoModel>("SP_MauIn_BiaHoSo_ChiTietMauIn", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }
        public MauInBiaHoSoNhanDonModel ChiTietMauInBiaHoSoNhanDon(int hoSoVuAnId)
        {
            MauInBiaHoSoNhanDonModel dbModel = null;
            try
            {
                var parameters = new List<SqlParameter> { new SqlParameter("@HoSoVuAnID", hoSoVuAnId) };
                dbModel = DBUtils.ExecuteSP<MauInBiaHoSoNhanDonModel>("SP_MauIn_BiaHoSoNhanDon_ChiTietMauIn", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }
        public List<ThuKyDuKhuyetModel> DanhSachThuKyDuKhuyet(int hoSoVuAnId)
        {
            List<ThuKyDuKhuyetModel> dbModel = new List<ThuKyDuKhuyetModel>();
            try
            {
                var parameters = new List<SqlParameter> { new SqlParameter("@HoSoVuAnID", hoSoVuAnId) };
                dbModel = DBUtils.ExecuteSPList<ThuKyDuKhuyetModel>("SP_MauIn_DanhSachThuKyDuKhuyet", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }
        #endregion

        #region GXN Đã Nhận Đơn Kháng Cáo
        public MauInDaNhanKhangCaoModel ChiTietGXNKhangCao(int hoSoVuAnId)
        {
            MauInDaNhanKhangCaoModel dbModel = null;
            try
            {
                var parameters = new List<SqlParameter> { new SqlParameter("@HoSoVuAnID", hoSoVuAnId) };
                dbModel = DBUtils.ExecuteSP<MauInDaNhanKhangCaoModel>("SP_MauIn_GXNKhangCao_ChiTietMauIn", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }

        public IEnumerable<NguoiKhangCaoGXNModel> DanhSachNguoiKhangCaoGXN(int hoSoVuAnId)
        {
            List<NguoiKhangCaoGXNModel> danhSachDuongSu;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter> { new SqlParameter("@HoSoVuAnID", hoSoVuAnId) };
                danhSachDuongSu = DBUtils.ExecuteSPList<NguoiKhangCaoGXNModel>("SP_MauIn_GXN_DanhSachNguoiKhangCao", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSachDuongSu;
        }
        #endregion

        public IEnumerable<DuongSuModel> DanhSachDuongSuTheoHoSoVuAnID(int hoSoVuAnId)
        {
            List<DuongSuModel> danhSachDuongSu;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>{ new SqlParameter("@HoSoVuAnID", hoSoVuAnId) };
                danhSachDuongSu = DBUtils.ExecuteSPList<DuongSuModel>("SP_MauIn_DanhSachDuongSu_Theo_HoSoVuAnID", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSachDuongSu;
        }

        public IEnumerable<ToiDanhTruyToModel> DanhSachToiDanhTruyTo(int hoSoVuAnId)
        {
            List<ToiDanhTruyToModel> danhSach;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter> { new SqlParameter("@HoSoVuAnID", hoSoVuAnId) };
                danhSach = DBUtils.ExecuteSPList<ToiDanhTruyToModel>("SP_MauIn_DanhSach_ToiDanhTruyTo", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSach;
        }

        public IEnumerable<NhomAnModel> DanhSachNhomAnTheoToaAn(int toaAnId)
        {
            List<NhomAnModel> danhSachNhomAn = null;
            try
            {
                var parameters = new List<SqlParameter> { new SqlParameter("@ToaAnID", toaAnId) };
                danhSachNhomAn = DBUtils.ExecuteSPList<NhomAnModel>("SP_NhomAn_DanhSachNhomAn_Theo_ToaAnID", parameters, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return danhSachNhomAn;
        }

        public IEnumerable<MauInModel> DanhSachMauInTheoGiaiDoanVaNhomAn(int? giaiDoan, string nhomAn)
        {
            List<MauInModel> danhSachMauIn = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@giaiDoan", giaiDoan));
                listParameter.Add(new SqlParameter("@nhomAn", nhomAn));
                danhSachMauIn = DBUtils.ExecuteSPList<MauInModel>("SP_MauIn_DanhSach_Theo_GiaiDoan_Va_NhomAn", listParameter, AppName.BizSecurity);
            }
            catch(Exception ex)
            {
                throw (ex);
            }
            return danhSachMauIn;
        }

        public IEnumerable<MauInModel> DanhSachMauInSearchByKeyword(string keyword)
        {
            List<MauInModel> danhSachMauIn = null;
            try
            {
                var parameter = new List<SqlParameter> { new SqlParameter("@keyword",keyword)};
                danhSachMauIn = DBUtils.ExecuteSPList<MauInModel>("SP_TimKiem_MauIn_Theo_Keyword",parameter,AppName.BizSecurity);
            }
            catch(Exception ex)
            {
                throw (ex);
            }
            return danhSachMauIn;
        }
    }
}