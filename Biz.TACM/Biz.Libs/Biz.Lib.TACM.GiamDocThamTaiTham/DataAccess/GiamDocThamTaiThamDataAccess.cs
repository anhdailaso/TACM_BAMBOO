using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Biz.Lib.Helpers;
using Microsoft.SqlServer.Server;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biz.Lib.TACM.GiamDocThamTaiTham.Model;
using Biz.Lib.TACM.GiamDocThamTaiTham.IDataAccess;

namespace Biz.Lib.TACM.GiamDocThamTaiTham.DataAccess
{
    public class GiamDocThamTaiThamDataAccess : IGiamDocThamTaiThamDataAccess
    {

        public IEnumerable<GiamDocThamTaiThamViewListModel> DanhSachGDTTT()
        {
            List<GiamDocThamTaiThamViewListModel> list = new List<GiamDocThamTaiThamViewListModel>();
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                list = DBUtils.ExecuteSPList<GiamDocThamTaiThamViewListModel>("SP_GiamDocThamTaiTham_DanhSach", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }
        
        public GiamDocThamTaiThamModel ChiTietGDTTT(int gDTTTid)
        {
            GiamDocThamTaiThamModel dbModel = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@GiamDocThamTaiThamID", gDTTTid));
                dbModel = DBUtils.ExecuteSP<GiamDocThamTaiThamModel>("SP_GiamDocThamTaiTham_ChiTiet", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }
        public ResponseResult SuaGDTTT(GiamDocThamTaiThamModel model)
        {
            ResponseResult result = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@GiamDocThamTaiThamID", model.GiamDocThamTaiThamID));
                listParameter.Add(new SqlParameter("@MaHoSo", model.MaHoSo));
                listParameter.Add(new SqlParameter("@So", model.So));
                listParameter.Add(new SqlParameter("@Nhom", model.Nhom));
                listParameter.Add(new SqlParameter("@NhomAn", model.NhomAn));
                listParameter.Add(new SqlParameter("@SoQuyetDinh", model.SoQuyetDinh));
                listParameter.Add(new SqlParameter("@NgayRaQuyetDinh", model.NgayRaQuyetDinh));
                listParameter.Add(new SqlParameter("@NoiDungQuyetDinh", model.NoiDungQuyetDinh));
                listParameter.Add(new SqlParameter("@NoiDungHuySuaAn", model.NoiDungHuySuaAn));
                listParameter.Add(new SqlParameter("@GhiChu", model.GhiChu));
                listParameter.Add(new SqlParameter("@HuySuaSoTham", model.HuySuaSoTham));
                listParameter.Add(new SqlParameter("@HuySuaPhucTham", model.HuySuaPhucTham));
                listParameter.Add(new SqlParameter("@BanAnSoTham", model.BanAnSoTham));
                listParameter.Add(new SqlParameter("@BanAnPhucTham", model.BanAnPhucTham));
                listParameter.Add(new SqlParameter("@QuyetDinhSoTham", model.QuyetDinhSoTham));
                listParameter.Add(new SqlParameter("@QuyetDinhPhucTham", model.QuyetDinhPhucTham));
                listParameter.Add(new SqlParameter("@NguoiTao", model.NguoiTao));
                result = DBUtils.ExecuteSP<ResponseResult>("SP_GiamDocThamTaiTham_Sua", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public ResponseResult XoaGDTTT(int gDTTTid, string NguoiTao)
        {
            ResponseResult result = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@GiamDocThamTaiThamID", gDTTTid));
                listParameter.Add(new SqlParameter("@NguoiTao", NguoiTao));
                result = DBUtils.ExecuteSP<ResponseResult>("SP_GiamDocThamTaiTham_Xoa", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public HoSoVuAnModel TimHoSo(string maHoSo)
        {
            HoSoVuAnModel dbModel = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@MaHoSo", maHoSo));
                dbModel = DBUtils.ExecuteSP<HoSoVuAnModel>("SP_GiamDocThamTaiTham_TimHoSo", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;

        }
        public List<MaHoSoModel> DanhSachHoSo(string nhomAn)
        {
            List<MaHoSoModel> dbModel = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@NhomAn", nhomAn));
                dbModel = DBUtils.ExecuteSPList<MaHoSoModel>("SP_GiamDocThamTaiTham_MaHoSo", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbModel;
        }
    }
}
