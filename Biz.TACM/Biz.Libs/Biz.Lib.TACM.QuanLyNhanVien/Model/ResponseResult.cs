using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Lib.TACM.QuanLyNhanVien.Model
{
    public class ResponseResult
    {
        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public int NhanVienIDMoi { get; set; }              //Use for getting new Nhan Vien ID from ThemNhanVien in order to add to CapNhatChucNang
    }
}
