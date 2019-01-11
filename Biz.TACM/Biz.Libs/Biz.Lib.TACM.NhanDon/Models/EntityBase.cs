using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.Lib.TACM.NhanDon.Models
{
    public abstract class EntityBase<TKey> 
    {
        public abstract TKey Id { get; }
    }
}
