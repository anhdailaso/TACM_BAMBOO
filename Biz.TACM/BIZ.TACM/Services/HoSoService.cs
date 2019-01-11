using Biz.TACM.IServices;
using System;
using System.Collections.Generic;
using Biz.Lib.TACM.NhanDon.Repositories;
using Biz.TACM.Models.ViewModel.HoSo;

namespace Biz.TACM.Services
{
    public class HoSoService : ServiceBase, IHoSoService
    {
        protected readonly IHosoRepository HosoRepository;

        public HoSoService(IHosoRepository hosoRepository)
        {
            this.HosoRepository = hosoRepository;
        }

        public IEnumerable<HoSoVuAnSummaryViewModel> Search(TruyVanHoSoViewModel query)
        {
            for(var i = 0; i <10; i ++)
            {
                yield return new HoSoVuAnSummaryViewModel()
                {
                    SoTT = i,
                    MaHoSo = $"{i}/2017/HC-02",
                    DuongSu = $"Nguyen Van A_{i}",
                    ThamPhan = "Nguyen Thi E(NV0024)",
                    NgayNhan = DateTime.Now,
                    TrangThai = new Random().Next(1, 3)
                };
            }
        }

        public bool UpdateState(int id, int newState)
        {
            var hoso = this.HosoRepository.Get(id);

            if(hoso != null && hoso.TrangThai != newState)
            {
                hoso.TrangThai = newState;

                this.SafeExecute(() => this.HosoRepository.Update(hoso));

                return true;
            }

            return false;
        }
    }
}