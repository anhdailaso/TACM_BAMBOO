using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Lib.TACM.XetXu.Model
{
    public class DuongSuModel
    {
        public int DuongSuID { get; set; }

        [Required]
        public string HoVaTen { get; set; }

        public bool Checked { get; set; }
    }
}
