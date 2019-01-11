using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Biz.Lib.Helpers;
using Microsoft.SqlServer.Server;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biz.Lib.TACM.HuyHoSo.Model;
using Biz.Lib.TACM.HuyHoSo.IDataAccess;

namespace Biz.Lib.TACM.HuyHoSo.DataAccess
{
    public class HuyHoSoDataAccess : IHuyHoSoDataAccess
    {
        public IEnumerable<HoSoVuAnModel> DanhSachHoSo()
        {
            var list = new List<HoSoVuAnModel>();
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                list = DBUtils.ExecuteSPList<HoSoVuAnModel>("SP_HuyHoSo_DanhSach", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }
        public HoSoVuAnModel TimHoSo(string maHoSo)
        {
            HoSoVuAnModel model = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@MaHoSo", maHoSo));
                model = DBUtils.ExecuteSP<HoSoVuAnModel>("SP_HuyHoSo_TimHoSo", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }
        public HoSoVuAnModel ChiTietHuy(int hoSoVuAnID)
        {
            HoSoVuAnModel model = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", hoSoVuAnID));
                model = DBUtils.ExecuteSP<HoSoVuAnModel>("SP_HuyHoSo_ChiTiet", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }
        public ResponseResult HuyHoSo(int hoSoVuAnID, string lyDoHuy, string nguoiHuy)
        {
            ResponseResult model = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", hoSoVuAnID));
                listParameter.Add(new SqlParameter("@LyDoHuy", lyDoHuy));
                listParameter.Add(new SqlParameter("@NguoiTao", nguoiHuy));
                model = DBUtils.ExecuteSP<ResponseResult>("SP_HuyHoSo_HuyHoSo", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }
        public ResponseResult SuaHuyHoSo(int hoSoVuAnID, string lyDoHuy)
        {
            ResponseResult model = null;
            try
            {
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@HoSoVuAnID", hoSoVuAnID));
                listParameter.Add(new SqlParameter("@LyDoHuy", lyDoHuy));
                model = DBUtils.ExecuteSP<ResponseResult>("SP_HuyHoSo_SuaHuyHoSo", listParameter, AppName.BizSecurity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }
    }
}
