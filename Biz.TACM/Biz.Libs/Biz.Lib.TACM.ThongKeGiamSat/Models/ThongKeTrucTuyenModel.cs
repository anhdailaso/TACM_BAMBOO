
using System.Collections.Generic;

namespace Biz.Lib.TACM.ThongKeGiamSat.Models
{
    public class ThongKeTrucTuyenModel
    {
        public string STT { get; set; }
        public string ThamPhan { get; set; }
        public int HSSTTL { get; set; }
        public int HSSTGQ { get; set; }
        public int HSPTTL { get; set; }
        public int HSPTGQ { get; set; }
        public int DSSTTL { get; set; }
        public int DSSTGQ { get; set; }
        public int DSPTTL { get; set; }
        public int DSPTGQ { get; set; }
        public int HNSTTL { get; set; }
        public int HNSTGQ { get; set; }
        public int HNPTTL { get; set; }
        public int HNPTGQ { get; set; }
        public int KTSTTL { get; set; }
        public int KTSTGQ { get; set; }
        public int KTPTTL { get; set; }
        public int KTPTGQ { get; set; }
        public int HCSTTL { get; set; }
        public int HCSTGQ { get; set; }
        public int HCPTTL { get; set; }
        public int HCPTGQ { get; set; }
        public int LDSTTL { get; set; }
        public int LDSTGQ { get; set; }
        public int LDPTTL { get; set; }
        public int LDPTGQ { get; set; }
        public int ADTL { get; set; }
        public int ADGQ { get; set; }

        public int TONGTLST
        {
            get { return HSSTTL + DSSTTL + HNSTTL + KTSTTL + HCSTTL + LDSTTL + ADTL; }
        }
        public int TONGTLPT
        {
            get { return HSPTTL + DSPTTL + HNPTTL + KTPTTL + HCPTTL + LDPTTL + ADTL; } 
        }
        public int TONGTL
        {
            get { return TONGTLST + TONGTLPT; }
        }

        public int TONGGQST
        {
            get { return HSSTGQ + DSSTGQ + HNSTGQ + KTSTGQ + HCSTGQ + LDSTGQ + ADGQ; }
        }
        public int TONGGQPT
        {
            get { return HSPTGQ + DSPTGQ + HNPTGQ + KTPTGQ + HCPTGQ + LDPTGQ + ADGQ; }
        }
        public int TONGGQ
        {
            get { return TONGGQST + TONGGQPT; }
        }

        public int TONGCL
        {
            get { return TONGTL - TONGGQ; }
        }
    }

    public class DuLieuThongKeTrucTuyenModel
    {
        public List<ThongKeTrucTuyenModel> ListData { get; set; }

        public int TONG_HSSTTL { get; set; }
        public int TONG_HSSTGQ { get; set; }
        public int TONG_HSPTTL { get; set; }
        public int TONG_HSPTGQ { get; set; }
        public int TONG_DSSTTL { get; set; }
        public int TONG_DSSTGQ { get; set; }
        public int TONG_DSPTTL { get; set; }
        public int TONG_DSPTGQ { get; set; }
        public int TONG_HNSTTL { get; set; }
        public int TONG_HNSTGQ { get; set; }
        public int TONG_HNPTTL { get; set; }
        public int TONG_HNPTGQ { get; set; }
        public int TONG_KTSTTL { get; set; }
        public int TONG_KTSTGQ { get; set; }
        public int TONG_KTPTTL { get; set; }
        public int TONG_KTPTGQ { get; set; }
        public int TONG_HCSTTL { get; set; }
        public int TONG_HCSTGQ { get; set; }
        public int TONG_HCPTTL { get; set; }
        public int TONG_HCPTGQ { get; set; }
        public int TONG_LDSTTL { get; set; }
        public int TONG_LDSTGQ { get; set; }
        public int TONG_LDPTTL { get; set; }
        public int TONG_LDPTGQ { get; set; }

        public int TONG_ADTL { get; set; }
        public int TONG_ADGQ { get; set; }

        public int TONG_TONGTL { get; set; }
        public int TONG_TONGTLST { get; set; }
        public int TONG_TONGTLPT { get; set; }

        public int TONG_TONGGQ { get; set; }
        public int TONG_TONGGQST { get; set; }
        public int TONG_TONGGQPT { get; set; }

        public int TONG_TONGCL { get; set; }
    }
}
